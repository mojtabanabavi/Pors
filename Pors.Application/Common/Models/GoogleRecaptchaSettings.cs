using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class GoogleRecaptchaSettings
    {
        public string ClientKey { get; set; }
        public string SecretKey { get; set; }
    }
}