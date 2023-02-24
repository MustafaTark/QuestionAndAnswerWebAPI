using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Question>? Questions { get; set; }
        public Tag()
        {
            Questions = new HashSet<Question>();
        }
    }
}
