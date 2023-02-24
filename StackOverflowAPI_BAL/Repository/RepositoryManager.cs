using StackOverflowAPI_BAL.Contracts;
using StackOverflowAPI_BAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Repository
{
    public class RepositoryManager:IRepositeryManager
    {
        private readonly ApplicationDbContext _repositoryContext;
        private  IQuestionRepositery _questionRepositery;
        public  ICommentRepositery _commentRepositery;
        public ITagRepositery _tagRepositery;
        public RepositoryManager(
            ApplicationDbContext repositeryContext,
            IQuestionRepositery questionRepositery,
            ICommentRepositery commentRepositery,ITagRepositery tagRepositery)
        {
            _repositoryContext = repositeryContext;
            _questionRepositery = questionRepositery;
            _commentRepositery = commentRepositery;
            _tagRepositery = tagRepositery;
        }
        public IQuestionRepositery Question {
            get
            {
                if (_questionRepositery == null)
                {
                    _questionRepositery = new QuestionRepositery(_repositoryContext);
                }
                return _questionRepositery;
            }
        }
        public ICommentRepositery Comment
        {
            get
            {
                if (_commentRepositery == null)
                {
                    _commentRepositery = new CommentRepositery(_repositoryContext);
                }
                return _commentRepositery;
            }
        }

        public ITagRepositery Tag
        {
            get
            {
                if (_tagRepositery == null)
                {
                    _tagRepositery = new TagRepositery(_repositoryContext);
                }
                return _tagRepositery;
            }
        }

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();

    }
}
