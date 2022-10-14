using StackOverflowAPI_DAL.Models;
using StackOverflowAPI_DAL.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI_DAL.Contracts
{
    public interface ICommentRepositery
    {
        Task<PagedList<Comment?>> GetCommentsAsync(int questionId,CommentParameters commentParameters, bool trackChanges);
        Task<Comment?> GetCommentAsync(int questionId, int id, bool trackChanges);
        void CreateCommentToQuestion(int questionId,Comment comment);
        void DeleteComment(Comment comment);
    }
}
