using Microsoft.EntityFrameworkCore;
using StackOverflowAPI_DAL.Contracts;
using StackOverflowAPI_DAL.Data;
using StackOverflowAPI_DAL.Extention;
using StackOverflowAPI_DAL.Models;
using StackOverflowAPI_DAL.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI_DAL.Repository
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
        public void CreateCommentToQuestion(int questionId, Comment comment) {
            comment.QuestionId = questionId;
            Create(comment);
        }

        public void DeleteComment(Comment comment) => 
            Delete(comment);
    }
}
