using System.Threading.Tasks;

namespace Technics
{
    public interface IMainForm
    {
        ProgramStatus ProgramStatus { get; }

        Task<bool> ShowListAsync(FrmListType listType);
    }
}