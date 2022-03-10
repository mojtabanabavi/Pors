using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Domain.Enums
{
    public enum ExamStatus
    {
        [Description("فعال")]
        Active = 1,

        [Description("غیرفعال")]
        InActive = 0
    }
}