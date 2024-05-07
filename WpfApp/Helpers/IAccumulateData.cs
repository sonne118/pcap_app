using System.Collections.Generic;
using static dotnet_Ptr.dotnet_Ptr;

namespace Services.Data
{
    public interface IAccumulateData
    {
         IEnumerable<myStruct> GetCurrerntData();
    }
}
