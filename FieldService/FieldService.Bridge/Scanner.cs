using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Bridge
{
    public class Scanner : AsyncProcess
    {
        private FileMessageQueue _queue;
        private HttpMessagePump _pump;

        private List<ITableScanner> _tableScanners = new List<ITableScanner>();

        public Scanner(string queueFolderName, Uri distributorUri)
        {
            _queue = new FileMessageQueue(queueFolderName);
            _pump = new HttpMessagePump(distributorUri, _queue, new NoOpBookmarkStore());
        }

        public Scanner Add<T>(
            string tableName,
            Func<DataRow, T> read,
            Func<T, Task> handleInsert,
            Func<T, T, Task> handleUpdate = null,
            Func<T, Task> handleDelete = null)
        {
            _tableScanners.Add(new TableScanner<T>(
                tableName,
                read,
                handleInsert,
                handleUpdate,
                handleDelete));
            return this;
        }

        protected override async Task StartUp()
        {
            var messages = await _queue.LoadAsync();
            _pump.SendAllMessages(messages);
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
