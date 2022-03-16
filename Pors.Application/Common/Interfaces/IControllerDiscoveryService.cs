using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pors.Application.Common.Models;

namespace Pors.Application.Common.Interfaces
{
    public interface IControllerDiscoveryService
    {
        List<ControllerInfo> DiscoverControllers();
        List<ControllerInfo> DiscoverSecuredControllers();
    }
}