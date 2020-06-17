using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/trails")]
    // [Route("api/trails")]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTr")]
    [ProducesResponseType(400)]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _repo;
        private readonly  IMapper _mapper;

        public TrailsController(ITrailRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get list of all Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(List<NationalParkDto>))]
        public IActionResult GetTrails()
        {
            var list = _repo.GetTrails();
            var listDto = new List<TrailDto>();
            foreach (var trail in list)
            {
                listDto.Add(_mapper.Map<TrailDto>(trail));
            }
            return Ok(listDto);
        }
        
        /// <summary>
        /// Get trail by its id.
        /// </summary>
        /// <param name="id">The id of a trail.</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200,Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public IActionResult GetTrail(int id)
        {
            var trail = _repo.GetTrail(id);
            if (trail is null)
            {
                return NotFound("Not found message");
            }
            return Ok(_mapper.Map<TrailDto>(trail));
        }

        [HttpPost]
        [ProducesResponseType(201,Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto is null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("error","Trail exists.");
                return StatusCode(404, ModelState);
            }

            var trail = _mapper.Map<Trail>(trailDto);
            
            if (!_repo.CreateTrail(trail))
            {
                ModelState.AddModelError("","Something went wrong.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new
            {
                version = HttpContext.GetRequestedApiVersion().ToString(),
                id = trail.Id
            }, trail);
        }
        
        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto is null || id!=trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            var trail = _mapper.Map<Trail>(trailDto);
            
            if (!_repo.UpdateTrail(trail))
            {
                ModelState.AddModelError("","Something went wrong.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        
        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_repo.TrailExists(id))
            {
                return BadRequest(ModelState);
            }

            var trail = _repo.GetTrail(id);
            if (!_repo.DeleteTrail(trail))
            {
                ModelState.AddModelError("","Something went wrong.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        /// <summary>
        /// API endpoint to get all trails in some national park.
        /// </summary>
        /// <param name="id"> Id of the park.</param>
        /// <returns></returns>
        [HttpGet("parks/{id:int}", Name = "GetTrailsInNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTrailsInNationalPark(int id)
        {
            var listObj = _repo.GetTrailsInNationalPark(id);
            return Ok(listObj);
        }
    }
}