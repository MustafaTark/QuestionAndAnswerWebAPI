using StackOverflowAPI_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI_DAL.Contracts
{
    public interface ITagRepositery
    {
        Task<IEnumerable<Tag?>> GetAllTagsAsync(bool trackChanges);
        Task<Tag?> GetTagAsync(int id, bool trackChanges);
        void CreateTag(Tag tag);
        void DeleteTag(Tag tag);
    }
}
