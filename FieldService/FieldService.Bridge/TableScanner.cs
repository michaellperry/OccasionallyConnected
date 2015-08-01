using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Bridge
{
    public class TableScanner<T> : ITableScanner
    {
        private readonly string _tableName;
        private readonly Func<DataRow, T> _read;
        private readonly Func<T, Task> _handleInsert;
        private readonly Func<T, T, Task> _handleUpdate;
        private readonly Func<T, Task> _handleDelete;

        private byte[] _lsn;
        
        public TableScanner(
            string tableName,
            Func<DataRow, T> read,
            Func<T, Task> handleInsert,
            Func<T, T, Task> handleUpdate = null,
            Func<T, Task> handleDelete = null)
        {
            _tableName = tableName;
            _read = read;
            _handleInsert = handleInsert;
            _handleUpdate = handleUpdate;
            _handleDelete = handleDelete;
        }

        public async Task DoWork(DbConnection connection)
        {
            byte[] fromLsn;
            if (_lsn == null)
            {
                // Get the starting Log Sequence Number.
                fromLsn = await connection.ExecuteScalarAsync<byte[]>(String.Format(
                    "select sys.fn_cdc_get_min_lsn('dbo_{0}')", _tableName));
            }
            else
            {
                // Get the next Log Sequence Number.
                fromLsn = await connection.ExecuteScalarAsync<byte[]>(
                    "select sys.fn_cdc_increment_lsn(@p1)", _lsn);
            }

            // Get the last Log Sequence Number.
            byte[] toLsn = await connection.ExecuteScalarAsync<byte[]>(
                "select sys.fn_cdc_get_max_lsn()");
            if (_lsn != null && toLsn.SequenceEqual(_lsn))
                return;     // There are no changes if the LSN hasn't moved.

            // Get all changes.
            var changes = await connection.ExecuteQueryAsync(String.Format(
                "select * from cdc.fn_cdc_get_all_changes_dbo_{0}(@p1, @p2, 'all update old')", _tableName),
                ReadChange,
                fromLsn, toLsn);

            var changeQueue = new Queue<Change<T>>(changes);
            while (changeQueue.Any())
            {
                var top = changeQueue.Dequeue();
                if (top.Operation == ChangeOperation.Delete)
                    await HandleDelete(top.Record);
                else if (top.Operation == ChangeOperation.Insert)
                    await HandleInsert(top.Record);
                else
                    await HandleUpdate(top.Record, changeQueue.Dequeue().Record);
            }

            _lsn = toLsn;
        }

        private Change<T> ReadChange(DataRow row)
        {
            var operation = row.GetInt32("__$operation");
            var record = _read(row);
            return new Change<T>
            {
                Operation =
                    operation == 1 ? ChangeOperation.Delete :
                    operation == 2 ? ChangeOperation.Insert :
                    operation == 3 ? ChangeOperation.UpdateOldValue :
                                     ChangeOperation.UpdateNewValue,
                Record = record
            };
        }

        private async Task HandleDelete(T record)
        {
            if (_handleDelete != null)
                await _handleDelete(record);
        }

        private async Task HandleInsert(T record)
        {
            await _handleInsert(record);
        }

        private async Task HandleUpdate(T oldRecord, T newRecord)
        {
            if (_handleUpdate != null)
                await _handleUpdate(oldRecord, newRecord);
        }
    }
}
