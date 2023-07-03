using AutoMapper;
using HostME.Core.DTOs;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HostME.API.Controllers
{
    [Route("api/hostels")]
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
    }
}
