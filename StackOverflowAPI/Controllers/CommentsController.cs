using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackOverflowAPI_BAL.Contracts;
using StackOverflowAPI_BAL.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.Design;
using StackOverflowAPI_BAL.Dto;
using StackOverflowAPI_BAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using StackOverflowAPI_BAL.Filters;
using StackOverflowAPI_BAL.RequestFeatures;
using Newtonsoft.Json;

namespace StackOverflowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IRepositeryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CommentsController(
            IRepositeryManager repository,
            ILoggerManager logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetCommentsForQuestion(int questionId, [FromQuery] CommentParameters commentParameters)
        {
            var question =await _repository.Question.GetQuestionAsync(questionId,trackChanges:false);
            if (question == null)
            {
                _logger.LogInfo($"Company with id: {question} doesn't exist in the database.");
                return NotFound();
            }
            var comments = await _repository.Comment.GetCommentsAsync(questionId, commentParameters, trackChanges:false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(comments.MetaData));
            var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Ok(commentsDto);
        }
        [HttpGet("{questionId}/{id}")]
        public async Task<IActionResult> GetCommentForQuestion(int questionId, int id)
        { 
            var question =await _repository.Question.GetQuestionAsync(questionId, trackChanges: false);
            if (question == null)
            {
                _logger.LogInfo($"Question with id: {questionId} doesn't exist in the database.");
                return NotFound();
            } 
            var comment =await _repository.Comment.GetCommentAsync(questionId, id, trackChanges: false);
            if (comment == null) 
            { 
                _logger.LogInfo($"Comment with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var commentDto = _mapper.Map<CommentDto>(comment);
            return Ok(commentDto);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public  IActionResult CreatComment(int questionId,[FromBody]CommentForCreationDto comment)
        {
            var question = _repository.Question.GetQuestionAsync(questionId, trackChanges: false);
            if (question == null)
            {
                _logger.LogInfo($"Question with id: {questionId} doesn't exist in the database.");
                return NotFound();
            }
           
            var commentEntity = _mapper.Map<Comment>(comment);
            _repository.Comment.CreateCommentToQuestion(commentEntity);
            _repository.SaveAsync();
            var questionToReturn = _mapper.Map<QuestionDto>(question);
            return Ok(questionToReturn);
        }
        [HttpDelete]
        [Route("Remove/{questionId}/{id}")]
        public async Task<IActionResult> DeleteComment(int questionId,int id)
        {
            var comment =await _repository.Comment.GetCommentAsync(questionId, id,false);
            if (comment == null)
            {
                _logger.LogInfo($"Commment with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Comment.DeleteComment(comment);
           await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommentToQuestion(int questionId, int id,
            [FromBody] CommentForUpdateDto comment)
        {
            if (comment == null)
            {
                _logger.LogError("CommentForUpdateDto object sent from client is null."); 
                return BadRequest("CommentForUpdateDto object is null");
            }
            var question = await _repository.Question.GetQuestionAsync(questionId, trackChanges: false);
            if (question == null)
            { 
                _logger.LogInfo($"Question with id: {question} doesn't exist in the database.");
                return NotFound();
            }
            var commentEntity =await _repository.Comment.GetCommentAsync(questionId, id, trackChanges: true); 
            if (commentEntity == null)
            {
                _logger.LogInfo($"Comment with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(comment, commentEntity);
           await _repository.SaveAsync();
            return NoContent(); 
        }
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateEmployeeForCompany(int questionId, int id,
            [FromBody] JsonPatchDocument<CommentForUpdateDto> patchDoc)
        { 
            if (patchDoc == null)
            { 
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            } 
            var question = _repository.Question.GetQuestionAsync(questionId, trackChanges: false);
            if (question == null) 
            { 
                _logger.LogInfo($"Question with id: {questionId} doesn't exist in the database.");
                return NotFound();
            }
            var commentEntity = _repository.Comment.GetCommentAsync(questionId, id, trackChanges: true);
            if (commentEntity == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            } 
            var commentToPatch = _mapper.Map<CommentForUpdateDto>(commentEntity);
            patchDoc.ApplyTo(commentToPatch);
            _mapper.Map(commentToPatch, commentEntity);
            _repository.SaveAsync();
            return NoContent();
        }

    }
}
