using AutoMapper;
using HostME.Core.DTOs;
using HostME.Core.Services;
using HostME.Core.UnitOfWork;
using HostME.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HostME.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private ApiUser? _user;
        private readonly IAuthManager _authManager;

        public AuthController(
            IUnitOfWork unitOfWork,
            ILogger<AuthController> logger,
            IMapper mapper,
            UserManager<ApiUser> userManager,
            IAuthManager authManager
            )

        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Missing Fields: PhoneNumber: {userDTO.PhoneNumber}, Email: {userDTO.Email}, Password: {userDTO.Password}, FirstName: {userDTO.FirstName}, LastName: {userDTO.LastName}");

                return BadRequest("Missing Fields");
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email.Split('@')[0];
                user.FirstLogin = "Y";

                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogInformation($"User registration failed:  {error.Code}: {error.Description}");

                        _logger.LogInformation($"Username {user.UserName} firstname: {user.FirstName}");
                    }
                    return BadRequest("Something went wrong");
                }

                if (user.UserName == "Administrator")
                {
                    await _userManager.AddToRoleAsync(user, "Super Administrator");

                    return Ok("Successfully registered");
                }

                await _userManager.AddToRoleAsync(user, "User");

                return Ok("Successfully registered");

            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error: ", ex.Message);
                return StatusCode(500, "Internal Server error, please try again later");
            }

        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO userDTO)
        {
            _logger.LogInformation($"Logging in; Email: {userDTO.Email}, Password: {userDTO.Password}");

            if (userDTO.Email == null || userDTO.Password == null)
            {
                _logger.LogInformation($"Missing Fields Email: {userDTO.Email} password: {userDTO.Password}");
                return BadRequest("Missing fields");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Invalid fields Email: {userDTO.Email} password: {userDTO.Password}");
                return BadRequest(ModelState);
            }

            if (!await _authManager.ValidateUser(userDTO))
            {
                _logger.LogInformation($"Unauthorized Username: {userDTO.Email}, Password: {userDTO.Password}");
                return Unauthorized("Invalid Username or Password");
            }

            _user = await _userManager.FindByNameAsync(userDTO.Email);

            var resident = await _unitOfWork.HostelResidentRepository.Get(r => r.ResidentId == _user.Id);

            if (resident == null) {
                var Noroomuser = new
                {
                    firstName = _user?.FirstName,
                    lastName = _user?.LastName,
                    id = _user?.Id,
                    userName = _user?.UserName,
                    email = _user?.Email,
                    phoneNumber = _user?.PhoneNumber,
                    accessFailedCount = _user?.AccessFailedCount,
                    Token = await _authManager.CreateToken()
                };

                return Accepted(Noroomuser);
            }

            var room = await _unitOfWork.RoomRepository.Get(r => r.Id == resident.RoomId);

            var roomObj = new GetRoomDTO
            {
                Id = room.Id,
                HostelId = room.HostelId,
                RoomType = room.RoomType,
                Capacity = room.Capacity,
                PricePerSemester = room.PricePerSemester,
                RoomStatus = room.RoomStatus
            };

            var user = new
            {
                id = _user?.Id,
                firstName = _user?.FirstName,
                lastName = _user?.LastName,
                userName = _user?.UserName,
                email = _user?.Email,
                phoneNumber = _user?.PhoneNumber,
                room = roomObj,
                accessFailedCount = _user?.AccessFailedCount,
                Token = await _authManager.CreateToken()
            };

            return Accepted(user);

        }

    }
}
