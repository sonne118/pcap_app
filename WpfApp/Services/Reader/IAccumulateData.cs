using System.Collections.Generic;
using WpfApp.Model;

namespace WpfApp.Services.Helpers
{
    public interface IAccumulateData
    {
        IEnumerable<PcapStruct> GetCurrerntData();
    }
}
