using AutoMapper;
using HostME.Core.DTOs;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        private readonly RoleManager<ApiRoles> _roleManager;

        public UserMgtController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<RoomController> logger,
            UserManager<ApiUser> userManager,
            RoleManager<ApiRoles> roleManager
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Manager")]
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

                    // Get the roles of the user.
                    var currentRoles = await _userManager.GetRolesAsync(user);

                    // Remove the "User" role from the user's roles
                    await _userManager.RemoveFromRoleAsync(user, "User");

                    // Add the "Resident" role to the user's roles
                    await _userManager.AddToRoleAsync(user, "Resident");

                    await _unitOfWork.HostelResidentRepository.Insert(resident);

                    //await _unitOfWork.BookingsRepository.Delete(bookingRecord.Id);

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

        [Authorize(Roles = "Super Administrator")]
        [HttpPost]
        [Route("roles")]
        public async Task<IActionResult> UpdateRole([FromBody] UserManagerDTO userDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Missing Fields");
            }

            var user = await _userManager.FindByIdAsync(userDTO.Id.ToString());

            if (user == null)
            {
                return NotFound("User does not exist!");
            }

            var role = await _roleManager.FindByIdAsync(userDTO.Id.ToString());

            if (role == null)
            {
                return NotFound("Role was not found");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove the "User" role from the user's roles
            foreach (var roleToremove in currentRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, roleToremove);
            }

            // Add the "Resident" role to the user's roles
            await _userManager.AddToRoleAsync(user, role.Name);

            _logger.LogInformation($"User {user} role changed to {role.Name}");

            return Ok("User Updated");
        }

        [Authorize(Roles = "Super Administrator")]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var results = new List<OneUserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userDto = new OneUserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Roles = roles
                };

                var resident = await _unitOfWork.HostelResidentRepository.Get(r => r.ResidentId == user.Id);

                if (resident == null)
                {
                    results.Add(userDto);

                    return Ok(results);
                }

                    var room = await _unitOfWork.RoomRepository.Get(r => r.Id == resident.RoomId);
                    if (room != null)
                    {
                        userDto.Room = new GetRoomDTO
                        {
                            Id = room.Id,
                            HostelId = room.HostelId,
                            RoomType = room.RoomType,
                            Capacity = room.Capacity,
                            PricePerSemester = room.PricePerSemester,
                            RoomStatus = room.RoomStatus
                        };
                    }
                

                results.Add(userDto);
            }

            return Ok(results);
        }


    }
}
