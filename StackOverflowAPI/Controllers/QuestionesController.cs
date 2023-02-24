using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackOverflowAPI_BAL.Filters;
using StackOverflowAPI_BAL.Contracts;
using StackOverflowAPI_BAL.Dto;
using StackOverflowAPI_BAL.Models;
using StackOverflowAPI_BAL.Repository;
using StackOverflowAPI_BAL.RequestFeatures;
using System.Xml.Linq;

namespace StackOverflowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionesController : ControllerBase
    {
        private readonly IRepositeryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public QuestionesController(IRepositeryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("{tagId}")]
       
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Question>))]
        public async Task<IActionResult> GetQuestiones(int tagId,[FromQuery]QuestionParamters questionParamters)
        {
            var tag =await _repository.Tag.GetTagAsync(tagId, trackChanges: false);
            if (tag == null)
            {
                _logger.LogInfo($"Tag with id: {tagId} doesn't exist in the database.");
                return NotFound();
            }

            var questions =await _repository.Question.GetQuestionsAsync(tagId,questionParamters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(questions.MetaData));
            var questionsDto = _mapper.Map<IEnumerable<QuestionDto>>(questions);
            return Ok(questionsDto);
        }
        [HttpGet("{tagId}/{id}")]
        [ResponseCache(CacheProfileName = "120SecondsDuration")]
        public async Task<IActionResult> GetQuestionToTag(int tagId, int id)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);
            if (tag == null)
            {
                _logger.LogInfo($"Tag with id: {tagId} doesn't exist in the database.");
                return NotFound();
            }
            var question =await _repository.Question.GetQuestionToTagAsync(tagId, id, trackChanges: false);
            if (question == null)
            {
                _logger.LogInfo($"Question with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var questionDto = _mapper.Map<QuestionDto>(question);
                return Ok(questionDto);
            }
        }
      
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CreatQuestion([FromBody]QuestionForCreationDto question)
        {
            
            var questionEntity=_mapper.Map<Question>(question);
            _repository.Question.CreateQuestion(questionEntity);
            _repository.SaveAsync();
            var questionToReturn = _mapper.Map<QuestionDto>(questionEntity);
            return Ok(questionToReturn);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion(int id)
        { 
            var question =await _repository.Question.GetQuestionAsync(id, trackChanges: false);
            if (question == null)
            {
                _logger.LogInfo($"Question with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Question.DeleteQuestion(question);
           await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id,
          [FromBody] QuestionForUpdateDto question)
        {
            if (question == null)
            {
                _logger.LogError("QuestionForUpdateDto object sent from client is null.");
                return BadRequest("QuestionForUpdateDto object is null");
            }
            var questionEntity = await _repository.Question.GetQuestionAsync(id, trackChanges: true);
            if (questionEntity == null)
            {
                _logger.LogInfo($"Question with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(question, questionEntity);
          await  _repository.SaveAsync();
            return NoContent();
        }
    }
}
