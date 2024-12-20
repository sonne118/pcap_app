using WpfApp.Model;

namespace wpfapp.Services.Reader
{
    public interface IAccumulateData
    {
        IEnumerable<PcapStruct> GetCurrerntData();
    }
}
