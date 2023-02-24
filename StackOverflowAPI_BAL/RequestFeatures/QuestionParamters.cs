using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.RequestFeatures
{
    public class QuestionParamters:RequestParameters
    {
        public QuestionParamters()
        {
            OrderBy = "Title";
        }
        public string? SearchTerm { get; set; }
    }
}
