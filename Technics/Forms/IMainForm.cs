using System.Threading.Tasks;

namespace Technics
{
    public interface IMainForm
    {
        Task<bool> ShowListAsync(FrmListType listType);
    }
}