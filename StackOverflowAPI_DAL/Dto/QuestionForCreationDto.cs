using StackOverflowAPI_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_DAL.Dto
{
    public class QuestionForCreationDto
    {
        public string? Title { get; set; }

        public string? Content { get; set; }
        public DateTime? Created { get; set; }
        public ICollection<Tag>? Tags { get; set; }
    }
}
