namespace Technics
{
    public class ProgramStatus : P3tr0viCh.Utils.ProgramStatus<Enums.Status>
    {
        public ProgramStatus() : base(Enums.Status.Idle)
        {
        }
    }
}