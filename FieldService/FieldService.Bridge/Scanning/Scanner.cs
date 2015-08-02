using FieldService.Bridge.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FieldService.Bridge.Scanning
{
    public abstract class Scanner : AsyncProcess
    {
        private List<ITableScanner> _tableScanners = new List<ITableScanner>();

        protected void AddTableScanner<T>(
            string tableName,
            Func<DataRow, T> read,
            Func<T, DbConnection, Task> handleInsert,
            Func<T, T, DbConnection, Task> handleUpdate = null,
            Func<T, DbConnection, Task> handleDelete = null)
        {
            _tableScanners.Add(new TableScanner<T>(
                tableName,
                read,
                handleInsert,
                handleUpdate,
                handleDelete));
        }

        protected override async Task DoWork()
        {
            using (SqlConnection connection = new SqlConnection(
                "Data Source=.;Initial Catalog=FieldService;Integrated Security=True"))
            {
                await connection.OpenAsync();

                foreach (var tableScanner in _tableScanners)
                {
                    await tableScanner.DoWork(connection);
                }
            }
        }
    }
}
