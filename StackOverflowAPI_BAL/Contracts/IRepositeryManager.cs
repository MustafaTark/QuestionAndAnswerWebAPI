using StackOverflowAPI_BAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Contracts
{
    public interface IRepositeryManager
    {
        IQuestionRepositery Question { get; }
        ICommentRepositery Comment { get; }
        ITagRepositery Tag { get; }
        Task SaveAsync();
    }
}
