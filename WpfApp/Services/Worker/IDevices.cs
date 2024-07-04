using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfapp.Services.Worker
{
    public interface IDevices
    {
        IEnumerable<string> GetDevices();
    }
}
