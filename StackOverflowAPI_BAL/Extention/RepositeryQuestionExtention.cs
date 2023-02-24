using StackOverflowAPI_BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Extention
{
    public static class RepositeryQuestionExtention
    {
        public static IQueryable<Question> Search(this IQueryable<Question> questiones, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return questiones;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return questiones.Where(e => e.Title!.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Question> Sort(this IQueryable<Question> questions, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return questions.OrderBy(e => e.Created);
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Question).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            { 
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()}" +
                    $" {direction}, ");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
                return questions.OrderBy(e => e.Title);
            return questions.OrderBy(q=>q.Title).ThenByDescending(q=>q.Likes!.Count);
            
        }
    }
}
