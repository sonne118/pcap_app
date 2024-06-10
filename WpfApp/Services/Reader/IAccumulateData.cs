using System.Collections.Generic;
using WpfApp.Model;

namespace WpfApp.Services.Reader
{
    public interface IAccumulateData
    {
        IEnumerable<PcapStruct> GetCurrerntData();
    }
}
