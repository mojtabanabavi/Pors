using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Domain.Enums
{
    public enum AnswerStatus
    {
        [Description("نامشخص")]
        Unknown = 0,

        [Description("صحیح")]
        Correct = 1,

        [Description("غلط")]
        Wrong = 2,
    }
}
