using System.Collections.Generic;

namespace wpfapp.Services.Worker
{
    public interface IDevices
    {
        IEnumerable<string> GetDevices();
    }
}
