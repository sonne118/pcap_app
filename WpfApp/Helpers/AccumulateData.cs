using System.Collections.Generic;
using static dotnet_Ptr.dotnet_Ptr;

namespace Services.Data
{
    public class AccumulateData : IAccumulateData
    {
        public  IEnumerable<myStruct> GetCurrerntData()
        {
            var results = dotnet_Ptr.dotnet_Ptr.GetPredict();

            return results;
        }
    }
}
