using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Main
    {
        private async Task TechPartsLoadAsync(List<TechModel> techs)
        {
            DebugWrite.Line("start");

            try
            {
                techs.ForEach(tech => DebugWrite.Line(tech.Text));

                var techPartList = await ListLoadAsync<TechPartModel>();

                foreach (var techPart in techPartList)
                {
                    techPart.TechText = Lists.Default.Techs.Find(t => t.Id == techPart.TechId)?.Text;
                }

                bindingSourceTechParts.DataSource = techPartList;

                TechPartsListChanged();

                Utils.Log.Info(ResourcesLog.LoadOk);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private IEnumerable<TechPartModel> TechPartList => bindingSourceTechParts.Cast<TechPartModel>();

        public TechPartModel TechPartSelected
        {
            get => bindingSourceTechParts.Current as TechPartModel;
        }

        private void TechPartsListChanged()
        {
           // tsbtnMileagesDelete.Enabled = tsbtnMileagesChange.Enabled = bindingSourceMileages.Count > 0;
        }
    }
}