namespace Technics.Presenters
{
    internal partial class PresenterStatusStripMain
    {
        public class CountModel
        {
            private readonly PresenterStatusStripMain parent;
            private readonly StatusLabel statusLabel;
            private readonly string textCount;
            private readonly string textSelectedCount;

            public CountModel(PresenterStatusStripMain parent, StatusLabel statusLabel,
                string textCount, string textSelectedCount)
            {
                this.parent = parent;
                this.statusLabel = statusLabel;
                this.textCount = textCount;
                this.textSelectedCount = textSelectedCount;
            }

            private int count = 0;

            public int Count
            {
                get => count;
                set
                {
                    count = value;

                    UpdateText();
                }
            }

            private int selectedCount = 0;

            public int SelectedCount
            {
                get => selectedCount;
                set
                {
                    selectedCount = value;

                    UpdateText();
                }
            }

            private void UpdateText()
            {
                parent.View.GetLabel(statusLabel).Text = 
                    selectedCount > 1 ?
                        string.Format(textSelectedCount, count, selectedCount) :
                        string.Format(textCount, count);
            }
        }
    }
}