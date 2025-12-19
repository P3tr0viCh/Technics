namespace Technics
{
    public partial class Database
    {
        public class Interfaces
        {
            public interface ITechId
            {
                long? TechId { get; set; }
                string TechText { get; set; }
            }

            public interface IPartId
            {
                long? PartId { get; set; }
                string PartText { get; set; }
            }
        }
    }
}