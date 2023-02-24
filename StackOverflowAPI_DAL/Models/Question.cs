
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI_BAL.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title of the question is a required field.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Content of the question is a required field.")]
        public string? Content { get; set; }
        [Required(ErrorMessage = "Date of creation is a required field.")]
        public DateTime? Created { get; set; }
        public LinkedList<Comment>? Comments { get; set; }
        public LinkedList<LikeToQuestion>? Likes { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }  
        public User? UserObject { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public Question()
        {
            Comments=new LinkedList<Comment>();
            Likes = new LinkedList<LikeToQuestion>();
            Tags = new HashSet<Tag>();
        }
    }
}
