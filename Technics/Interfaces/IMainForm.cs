using System.Threading.Tasks;

namespace Technics.Interfaces
{
    public interface IMainForm
    {
        Task<bool> ShowListAsync(FrmListType listType);
    }
}