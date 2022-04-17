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

        [Description("غلط")]
        Wrong = 1,

        [Description("نسبتا غلط")]
        SomewhatWrong = 2,

        [Description("نسبتا صحيح")]
        SomewhatCorrect = 3,

        [Description("صحيح")]
        Correct = 4,
    }
}
