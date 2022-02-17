using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class ExamQuestion
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public Exam Exam { get; set; }
        public ICollection<QuestionOption> Options { get; set; }
    }
}
