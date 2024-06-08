using System.Collections.Generic;
using WpfApp.Model;

namespace WpfApp.Services.Helpers
{
    public class AccumulateData : IAccumulateData
    {
        public IEnumerable<PcapStruct> GetCurrerntData()
        {
            var results = DotnetPtr.GetPredict();

            return results;
        }
    }
}
