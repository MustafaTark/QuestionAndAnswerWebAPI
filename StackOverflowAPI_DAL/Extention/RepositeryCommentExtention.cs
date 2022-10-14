using StackOverflowAPI_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_DAL.Extention
{
    public static class RepositeryCommentExtention
    {
        public static IQueryable<Comment> Search(this IQueryable<Comment> comments, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return comments;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return comments.Where(e => e.Content!.ToLower().Contains(lowerCaseTerm));
        }
    }
}
