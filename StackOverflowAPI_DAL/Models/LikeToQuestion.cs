using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_DAL.Models
{
    public class LikeToQuestion
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public User? UserObject { get; set; }
        [ForeignKey(nameof(Question))]
        public int? QuestionId { get; set; }
        public Question? QuestionObject { get; set; }
    }
}
