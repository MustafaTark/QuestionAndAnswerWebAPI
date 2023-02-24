using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackOverflowAPI_BAL.Filters;
using StackOverflowAPI_BAL.Contracts;
using StackOverflowAPI_BAL.Dto;
using StackOverflowAPI_BAL.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StackOverflowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IRepositeryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public TagsController(
            IRepositeryManager repository,
            ILoggerManager logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]

        //[ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public async Task<IActionResult> GetTags()
        {

            var tags =await _repository.Tag.GetAllTagsAsync(trackChanges: false);
            var tagsDto = _mapper.Map<IEnumerable<TagDto>>(tags);
            return Ok(tagsDto);
        }
        [HttpGet("{id}", Name = "TagById")]
        public async Task<IActionResult> GetTagBYId(int id)
        {
            var tag =await _repository.Tag.GetTagAsync(id, trackChanges: false);
            if (tag == null)
            {
                _logger.LogInfo($"Tag with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var tagDto = _mapper.Map<TagDto>(tag);
                return Ok(tagDto);
            }
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CreateTag([FromBody] TagForCreationDto tag)
        {
           
            var tagEntity = _mapper.Map<Tag>(tag);
            _repository.Tag.CreateTag(tagEntity);
            _repository.SaveAsync(); 
            var tagToReturn = _mapper.Map<TagDto>(tagEntity);
            return CreatedAtRoute("TagBYId",
                new { id = tagToReturn.Id }, tagToReturn);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _repository.Tag.GetTagAsync(id, trackChanges: false);
            if (tag == null)
            {
                _logger.LogInfo($"Question with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Tag.DeleteTag(tag);
           await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id,
           [FromBody] TagForUpdateDto tag)
        {
            if (tag == null)
            {
                _logger.LogError("TagForUpdateDto object sent from client is null.");
                return BadRequest("TagForUpdateDto object is null");
            }
            var tagEntity =await _repository.Tag.GetTagAsync(id,trackChanges:true);
            if (tagEntity == null)
            {
                _logger.LogInfo($"Tag with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(tag, tagEntity);
           await _repository.SaveAsync();
            return NoContent();
        }
    }
}
