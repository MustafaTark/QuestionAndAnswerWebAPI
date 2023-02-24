using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public User? UserObject { get; set; }
        [ForeignKey(nameof(Comment))]
        public int? CommentId { get; set; }
        public Comment? CommentObject { get; set; }
    }
}
