using System.Data.Common;
using System.Threading.Tasks;

namespace FieldService.Bridge.Scanning
{
    public interface ITableScanner
    {
        Task DoWork(DbConnection connection);
    }
}
