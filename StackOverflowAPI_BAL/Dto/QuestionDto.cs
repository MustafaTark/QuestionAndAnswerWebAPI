using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Dto
{
    public class QuestionDto
    {
        public int Id { get; set; }
       
        public string? Title { get; set; }
    
        public string? Content { get; set; }
        public DateTime? Created { get; set; }
    }
}
