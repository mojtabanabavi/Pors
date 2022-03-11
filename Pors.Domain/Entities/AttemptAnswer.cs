using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class AttemptAnswer
    {
        public string AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }

        public ExamQuestion Question { get; set; }
        public QuestionOption Option { get; set; }
        public ExamAttempt Attempt { get; set; }
    }
}
