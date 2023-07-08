using AutoMapper;
using HostME.Core.DTOs;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HostME.API.Controllers
{
    [Route("api/hostel")]
    [ApiController]
    public class HostelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HostelController> _logger;
        private readonly UserManager<ApiUser> _usermanager;

        public HostelController(ILogger<HostelController> logger, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApiUser> usermanager)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _usermanager = usermanager;
        }

        [Authorize(Roles = "Super Administrator, User")]
        [HttpGet]
        public async Task<ActionResult> GetAllHostels()
        {
            var hostels = await _unitOfWork.HostelRepository.GetAll();

            var results = _mapper.Map<List<GetHostelDTO>>(hostels);

            // Populate ManagerId for each hostel
            foreach (var hostel in results)
            {
                var hostelManager = await _unitOfWork.HostelManagerRepository.Get(q => q.Id == hostel.Id);
                hostel.ManagerId = hostelManager?.ManagerId ?? 0;
            }

            return Ok(results);
        }

        [Authorize(Roles = "Super Administrator, User")]
        [HttpPost]
        [Route("hostel")]
        public async Task<ActionResult> GetHostel([FromBody] OneHostelDTO hostelDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Missing fields");
            }

            // Because we want to return the rooms as well we are going to use our list property of the get method in the gen repository
            var hostel = await _unitOfWork.HostelRepository.Get(h => h.Id == hostelDTO.Id, new List<string> { "Rooms" });

            if (hostel == null)
            {
                return NotFound("Hostel was not found");
            }

            var result = _mapper.Map<GetHostelDTO>(hostel);

            // Populate ManagerId for the hostel
            var hostelManager = await _unitOfWork.HostelManagerRepository.Get(h => h.Id == hostel.Id);
            result.ManagerId = hostelManager?.ManagerId ?? 0;

            return Ok(result);
        }

        [Authorize(Roles = "Super Administrator")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateHostel([FromBody] HostelDTO hostelDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Missing Fields");
            }

            var hostel = _mapper.Map<Hostel>(hostelDTO);

            var manager = await _usermanager.FindByIdAsync(hostelDTO.ManagerId.ToString());
            if (manager == null)
            {
                return BadRequest("User does not exist!");
            }

            await _unitOfWork.HostelRepository.Insert(hostel);
            await _unitOfWork.Save();

            var hostelManager = new HostelManager
            {
                ManagerId = hostelDTO.ManagerId,
                Hostel = hostel,
                Manager = manager
            };



            await _unitOfWork.HostelManagerRepository.Insert(hostelManager);
            await _unitOfWork.Save();

            _logger.LogInformation($"Created Hostel: {hostel.Id}, {hostel.Name}, {hostel.Address}, {hostel.NoOfRooms} : Manager: {hostelManager.Manager}");

            return Created("Hostel successfully created", hostel);
        }

        [Authorize(Roles = "Super Administrator")]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateHostel([FromBody] UpdateHostelDTO hostelDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Missing Fields");
            }

            var hostel = await _unitOfWork.HostelRepository.Get(h => h.Id == hostelDTO.Id);

            if(hostel == null)
            {
                return BadRequest("Hostel Not Found");
            }

            _mapper.Map(hostelDTO, hostel);

            _unitOfWork.HostelRepository.Update(hostel);
            await _unitOfWork.Save();

            return Created("Hostel Updated", hostel);
        }

        [Authorize]
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteHostel([FromBody] DeleteHostelDTO hostelDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Missing Fields");
            }

            var hostel = await _unitOfWork.HostelRepository.Get(h => h.Id == hostelDTO.Id);

            if(hostel == null)
            {
                return NotFound("Hostel was not found");
            }

            await _unitOfWork.HostelRepository.Delete(hostelDTO.Id);
            await _unitOfWork.Save(); 
            
            return Ok($"{hostel.Name} Deleted");
        }
    }
}
