using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/nationalparks")]
    //Route("api/[controller]")]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly  IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// Get list of all National Parks.
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        [ProducesResponseType(200,Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var list = _npRepo.GetNationalParks();
            var listDto = new List<NationalParkDto>();
            foreach (var park in list)
            {
                listDto.Add(_mapper.Map<NationalParkDto>(park));
            }
            return Ok(listDto);
        }
        
        /// <summary>
        /// Get national park by its id.
        /// </summary>
        /// <param name="id">The id of a national park.</param>
        /// <returns></returns>
        
        [HttpGet("{id:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200,Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int id)
        {
            var park = _npRepo.GetNationalPark(id);
            if (park is null)
            {
                return NotFound("Not found message");
            }
            return Ok(_mapper.Map<NationalParkDto>(park));
        }

        [HttpPost]
        [ProducesResponseType(201,Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto is null)
            {
                return BadRequest(ModelState);
            }

            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("error","National park exists.");
                return StatusCode(404, ModelState);
            }

            var park = _mapper.Map<NationalPark>(nationalParkDto);
            
            if (!_npRepo.CreateNationalPark(park))
            {
                ModelState.AddModelError("","Something went wrong.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new
            {
                version = HttpContext.GetRequestedApiVersion().ToString(),
                id = park.Id
            }, park);
        }
        
        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto is null || id!=nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            var park = _mapper.Map<NationalPark>(nationalParkDto);
            
            if (!_npRepo.UpdateNationalPark(park))
            {
                ModelState.AddModelError("","Something went wrong.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        
        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult DeleteNationalPark(int id)
        {
            if (!_npRepo.NationalParkExists(id))
            {
                return BadRequest(ModelState);
            }

            var park = _npRepo.GetNationalPark(id);
            if (!_npRepo.DeleteNationalPark(park))
            {
                ModelState.AddModelError("","Something went wrong.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
    
}