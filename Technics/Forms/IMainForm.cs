using P3tr0viCh.Database;
using System.Threading.Tasks;

namespace Technics
{
    public interface IMainForm
    {
        ProgramStatus ProgramStatus { get; }

        Task ListItemSaveAsync<T>(T value) where T : BaseId;
    }
}