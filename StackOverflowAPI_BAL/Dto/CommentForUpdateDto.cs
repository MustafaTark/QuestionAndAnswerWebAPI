using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Dto
{
    public class CommentForUpdateDto
    {
       
        public string? Content { get; set; }
        public DateTime? Created { get; set; }
    }
}
