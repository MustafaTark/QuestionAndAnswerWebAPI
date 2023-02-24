using Microsoft.EntityFrameworkCore;
using StackOverflowAPI_BAL.Contracts;
using StackOverflowAPI_BAL.Extention;
using StackOverflowAPI_BAL.Data;
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
    public class QuestionRepositery:RepositeryBase<Question>,IQuestionRepositery
    {
        public QuestionRepositery(ApplicationDbContext repositoryContext) :
            base(repositoryContext)
        {
        }
        public async Task<PagedList<Question>> GetQuestionsAsync(int tagId, QuestionParamters questionParamters, bool trackChanges)
        {
          var questiones=  await FindByCondition(q =>
         q.Tags!.FirstOrDefault(t => t.Id == tagId)!.Id == tagId,
             trackChanges)
            .OrderBy(c => c.Likes!.Count)
            .Search(questionParamters.SearchTerm!)
            .ToListAsync();
            return PagedList<Question>.ToPagedList(questiones, questionParamters.PageNumber, questionParamters.PageSize);
        }

        public async Task<Question?> GetQuestionToTagAsync(int tagId,int id, bool trackChanges) =>
          await  FindByCondition(q => q.Id == id &&
            q.Tags!.FirstOrDefault(t => t.Id == tagId)!.Id == tagId, trackChanges).FirstOrDefaultAsync()!;

        public void CreateQuestion(Question question) => Create(question);

        public void DeleteQuestion(Question question) =>
            Delete(question);

        public async Task<Question?> GetQuestionAsync(int id, bool trackChanges)=>
           await FindByCondition(q => q.Id == id, trackChanges).FirstOrDefaultAsync()!;
    }
}
