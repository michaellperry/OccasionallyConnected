using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Bridge
{
    static class DatabaseExtensions
    {
        public static async Task<T> ExecuteScalarAsync<T>(
            this DbConnection connection, string query, params object[] parameters)
        {
            using (var command = connection.CreateCommand())
            {
                InitializeCommand(command, query, parameters);
                return (T)await command.ExecuteScalarAsync();
            }
        }

        public static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(
            this DbConnection connection,
            string query,
            Func<DataRow, T> handleRow,
            params object[] parameters)
        {
            var list = new List<T>();
            using (var command = connection.CreateCommand())
            {
                InitializeCommand(command, query, parameters);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(handleRow(new DataRow(reader)));
                    }
                }
            }
            return list;
        }

        private static void InitializeCommand(DbCommand command, string query, object[] parameters)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;
            foreach (var parameter in parameters.Select((v, i) => new { v, i }))
            {
                var dbParameter = command.CreateParameter();
                dbParameter.ParameterName = String.Format("@p{0}", parameter.i + 1);
                dbParameter.Value = parameter.v;
                command.Parameters.Add(dbParameter);
            }
        }
    }
}
