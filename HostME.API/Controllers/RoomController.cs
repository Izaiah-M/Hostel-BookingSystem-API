using AutoMapper;
using HostME.Core.DTOs;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HostME.API.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomController> _logger;


        public RoomController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<RoomController> logger
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
            _logger = logger;
        }

        [Authorize(Roles = "Super Administrator, User, Manager")]
        [HttpPost]
        [Route("hostelrooms")]
        public async Task<ActionResult> GetHostelRooms([FromBody] AllRoomsDTO roomDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Missing Fields");
            }

            var hostel = await _unitOfWork.HostelRepository.Get(h => h.Id == roomDTO.HostelId);

            if(hostel == null) 
            {
                return BadRequest("Hostel Not Found!");
            }

            var rooms = await _unitOfWork.RoomRepository.GetAll(r => r.HostelId == roomDTO.HostelId);

            var results = _mapper.Map<List<GetRoomDTO>>(rooms);

            return Ok(results);

        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRoom([FromBody] RoomDTO roomDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Missing Fields");
            }

            var hostel = await _unitOfWork.HostelRepository.Get(h => h.Id == roomDTO.HostelId);

            if(hostel == null)
            {
                return NotFound("Hostel was not found");
            }

            var room = _mapper.Map<Room>(roomDTO);


            await _unitOfWork.RoomRepository.Insert(room);
            await _unitOfWork.Save();

            _logger.LogInformation($"Created Room {room.Id} for hostel {hostel.Name}");

            return Created("Created room", room);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomDTO roomDTO)
        {
            if(!ModelState.IsValid) { 
                return BadRequest("Missing Fields"); 
            }

            var hostel = await _unitOfWork.HostelRepository.Get(h => h.Id == roomDTO.HostelId);

            if (hostel == null)
            {
                return NotFound("Hostel was not found");
            }

            var room = await _unitOfWork.RoomRepository.Get(h => h.Id == roomDTO.Id);

            if (room == null)
            {
                return NotFound("Room was not found");
            }

            _mapper.Map(roomDTO, room);

            _unitOfWork.RoomRepository.Update(room);
            await _unitOfWork.Save();

            return Created("Room Updated", room);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteRoom([FromBody] DeleteRoomDTO roomDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Missing Fields");
            }

            var hostel = await _unitOfWork.HostelRepository.Get(h => h.Id == roomDTO.HostelId);

            if (hostel == null)
            {
                return NotFound("Hostel was not found");
            }

            var room = await _unitOfWork.RoomRepository.Get(h => h.Id == roomDTO.Id);

            if (room == null)
            {
                return NotFound("Room was not found");
            }

            await _unitOfWork.RoomRepository.Delete(room.Id);
            await _unitOfWork.Save();

            return Ok($"Room {room.Id} deleted");
        }
    }
}
