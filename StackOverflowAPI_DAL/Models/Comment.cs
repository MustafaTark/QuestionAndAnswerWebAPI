﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI_BAL.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime? Created { get; set; }
        public LinkedList<Reply>? Replies { get; set; }
        public LinkedList<LikeToComment>? Likes { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
       
        [ForeignKey(nameof(Question))]
        public int? QuestionId { get; set; }
       
        public Comment()
        {
            Replies=new LinkedList<Reply>();
            Likes = new LinkedList<LikeToComment>();
        }
    }
}
