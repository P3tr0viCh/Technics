using P3tr0viCh.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Technics
{
    public interface IMainForm
    {
        ProgramStatus ProgramStatus { get; }

        Task<IEnumerable<T>> ListLoadAsync<T>(string sql = null, object param = null);

        Task ListItemSaveAsync<T>(T value) where T : BaseId;

        Task ListItemDeleteAsync<T>(T value) where T : BaseId;

        Task ListItemDeleteAsync<T>(IEnumerable<T> values) where T : BaseId;
    }
}