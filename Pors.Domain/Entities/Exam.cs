using System;
using System.Linq;
using System.Text;
using Pors.Domain.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Image { get; set; }
        public ExamStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public ICollection<ExamAttempt> Attempts { get; set; }
        public ICollection<ExamQuestion> Questions { get; set; }
    }
}