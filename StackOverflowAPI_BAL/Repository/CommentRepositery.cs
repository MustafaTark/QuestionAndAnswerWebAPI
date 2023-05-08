using Microsoft.EntityFrameworkCore;
using StackOverflowAPI_BAL.Contracts;
using StackOverflowAPI_BAL.Data;
using StackOverflowAPI_BAL.Extention;
using StackOverflowAPI_BAL.Models;
using StackOverflowAPI_BAL.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI_BAL.Repository
{
    public class CommentRepositery:RepositeryBase<Comment>,ICommentRepositery
    {
        public CommentRepositery(ApplicationDbContext repositeryContext) : base(repositeryContext)
        {

        }

        public async Task<Comment?> GetCommentAsync(int questionId, int id, bool trackChanges) =>
           await FindByCondition(c => c.QuestionId == questionId &&
            c.Id == id, trackChanges).FirstOrDefaultAsync()!;

        public async Task<PagedList<Comment?>> GetCommentsAsync(int questionId, CommentParameters commentParameters, bool trackChanges)
        {
           var comments= await FindByCondition(c => c.QuestionId == questionId, trackChanges)
             .OrderBy(c => c.Likes!.Count)
             .Search(commentParameters.SearchTerm!)
             .ToListAsync();
            return PagedList<Comment>.ToPagedList(comments, commentParameters.PageNumber, commentParameters.PageSize)!;
        }
        public void CreateCommentToQuestion(Comment comment) =>
            Create(comment);
      

        public void DeleteComment(Comment comment) => 
            Delete(comment);
    }
}
