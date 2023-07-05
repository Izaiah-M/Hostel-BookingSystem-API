using AutoMapper;
using HostME.Core.DTOs;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HostME.API.Controllers
{
    // room status can be...Vacant, Booked, Occupied, Maintanence
    // TODO: In user mgt, someone Approves the booked room and the record is deleteted from the bookings table
    // The room status is changed to Occupied and the id of the user and room are stored in the Hostel Resident table

    [Route("api/user")]
    [ApiController]
    public class UserMgtController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomController> _logger;
        private readonly UserManager<ApiUser> _userManager;

        public UserMgtController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<RoomController> logger,
            UserManager<ApiUser> userManager
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("approve-resident")]
        public async Task<IActionResult> ApproveResident([FromBody] HostelResidentDTO hostelResidentDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Missing fields: {ModelState}");
                return BadRequest("Missing Fields");
            }

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var resident = _mapper.Map<HostelResident>(hostelResidentDTO);
                    var user = await _userManager.FindByIdAsync(hostelResidentDTO.ResidentId.ToString());

                    if (user == null)
                    {
                        return NotFound("User does not exist!");
                    }

                    var bookingRecord = await _unitOfWork.BookingsRepository.Get(b => b.UserId == hostelResidentDTO.ResidentId);

                    if (bookingRecord == null)
                    {
                        return BadRequest("User did not book");
                    }

                    await _userManager.AddToRoleAsync(user, "Resident");
                    await _unitOfWork.HostelResidentRepository.Insert(resident);
                    await _unitOfWork.BookingsRepository.Delete(bookingRecord.Id);

                    var room = await _unitOfWork.RoomRepository.Get(r => r.Id == hostelResidentDTO.RoomId);

                    if (room == null)
                    {
                        return NotFound("Room not found");
                    }

                    var updatedRoom = new Room
                    {
                        Id = room.Id,
                        RoomStatus = "Occupied",
                        HostelId = room.HostelId,
                        RoomType = room.RoomType,
                        Capacity = room.Capacity,
                        PricePerSemester = room.PricePerSemester
                    };

                    _unitOfWork.RoomRepository.Update(updatedRoom);
                    await _unitOfWork.Save();
                    await _unitOfWork.CommitTransactionAsync();

                    _logger.LogInformation($"User {user} is now a Resident");
                    return Ok("User Updated");
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogError("An error occurred during resident approval transaction: ", ex.Message);
                    return StatusCode(500, "Something went wrong during resident approval.");
                }
            }
        }


    }
}
