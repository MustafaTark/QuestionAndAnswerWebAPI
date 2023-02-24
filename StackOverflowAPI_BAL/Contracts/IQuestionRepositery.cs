using StackOverflowAPI_BAL.Models;
using StackOverflowAPI_BAL.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI_BAL.Contracts
{
    public interface IQuestionRepositery
    {
        
        Task<PagedList<Question>> GetQuestionsAsync(int tagId, QuestionParamters questionParamters,bool trackChanges);
       Task<Question?> GetQuestionToTagAsync(int tagId,int id, bool trackChanges);
        Task<Question?> GetQuestionAsync(int id, bool trackChanges);
        void CreateQuestion(Question question);
        void DeleteQuestion(Question question);
    }
}
