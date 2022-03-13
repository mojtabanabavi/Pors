using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class AttemptAnswer
    {
        public int Id { get; set; }
        public string AttemptId { get; set; }
        public int OptionId { get; set; }
        public bool IsCorrect { get; set; }
        public string Description { get; set; }

        public QuestionOption Option { get; set; }
        public ExamAttempt Attempt { get; set; }
    }
}
