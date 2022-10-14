using AutoMapper;
using StackOverflowAPI_DAL.Dto;
using StackOverflowAPI_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI_DAL
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Question, QuestionDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<TagForCreationDto, Tag>();
            CreateMap<QuestionForCreationDto, Question>();
            CreateMap<CommentForCreationDto, Comment>();
            CreateMap<CommentForUpdateDto, Comment>().ReverseMap();
            CreateMap<UserForRegistrationDto, User>();
        }
       
    }
}
