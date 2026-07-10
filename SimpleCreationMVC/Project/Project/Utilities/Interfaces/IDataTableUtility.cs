
using System.Data;

namespace Utilities.Interfaces
{
    public interface IDataTableUtility
    {
        DataTable Convert<T>(IEnumerable<T> lists) where T : class;
    }
}
