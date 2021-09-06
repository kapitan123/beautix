using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPoller.Services.Poller
{
    public interface IPoller
    {
        // AK TODO return poll status with count of records and poll time
        Task Run();
    }
}
