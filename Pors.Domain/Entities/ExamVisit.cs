using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Domain.Entities
{
    public class ExamVisit
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }

        public Exam Exam { get; set; }
    }
}
