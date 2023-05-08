using StackOverflowAPI_BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Dto
{
    public class QuestionForCreationDto
    {
        public string? Title { get; set; }

        public string? Content { get; set; }
        public DateTime? Created { get; set; }
        public ICollection<TagForCreationDto>? Tags { get; set; }
        public string? UserId { get; set; }
    }
}
