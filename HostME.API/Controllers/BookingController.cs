using AutoMapper;
using HostME.Core.DTOs;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HostME.API.Controllers
{

    //  TODO: When User posts a booking, simultaneously we need to change the status of the room being booked to Booked

    // room status can be...Vacant, Booked, Occupied, Maintanence
    // TODO: In user mgt, someone Approves the booked room and the record is deleteted from the bookings table
    // The room status is changed to Occupied and the id of the user and room are stored in the Hostel Resident table
    [Route("api/book")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HostelController> _logger;


        public BookingController(ILogger<HostelController> logger, IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApiUser> usermanager)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> GetReservations()
        {
            var reservations = await _unitOfWork.BookingsRepository.GetAll(null, null, new List<string> { "Hostel", "Room", "Customer" });

            var results = _mapper.Map<List<AllBookingsDTO>>(reservations);

            return Ok(results);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Book([FromBody] BookingDTO bookingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Missing fields");
            }

            var booking = _mapper.Map<Booking>(bookingDTO);

            var room = await _unitOfWork.RoomRepository.Get(r => r.Id == bookingDTO.RoomId);

            if (room == null)
            {
                return NotFound("Room not found");
            }

            var roomStatus = room.RoomStatus;

            if (roomStatus == "Booked" || roomStatus == "Maintenance")
            {
                return BadRequest("Room is not available.");
            }

            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var updatedRoom = new Room
                    {
                        Id = room.Id,
                        RoomStatus = "Booked",
                        HostelId = room.HostelId,
                        RoomType = room.RoomType,
                        Capacity = room.Capacity,
                        PricePerSemester = room.PricePerSemester
                    };

                    _unitOfWork.RoomRepository.Update(updatedRoom);

                    await _unitOfWork.BookingsRepository.Insert(booking);

                    await _unitOfWork.Save();

                    await _unitOfWork.CommitTransactionAsync();

                    _logger.LogInformation($"User {bookingDTO.UserId} booked room ${bookingDTO.RoomId} in hostel {bookingDTO.HostelId}");
                    return Ok($"Room {bookingDTO.RoomId} reserved");
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogError(ex, "An error occurred during booking transaction.");
                    return StatusCode(500, "Something went wrong during booking.");
                }
            }
        }

    }
}
