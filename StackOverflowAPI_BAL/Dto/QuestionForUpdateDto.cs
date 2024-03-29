﻿using StackOverflowAPI_BAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Dto
{
    public class QuestionForUpdateDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public ICollection<TagForUpdateDto>? Tags { get; set; }
    }
}
