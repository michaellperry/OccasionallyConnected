using System.Data.Common;
using System.Threading.Tasks;

namespace FieldService.Bridge
{
    public interface ITableScanner
    {
        Task DoWork(DbConnection connection);
    }
}
