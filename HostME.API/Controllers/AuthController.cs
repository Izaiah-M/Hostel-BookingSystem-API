using AutoMapper;
using HostME.Core.DTOs;
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
        private readonly UserManager<ApiUser> _usermanager;
        private ApiUser? _user;

        // Dependency Injection
        public AuthController(
            IUnitOfWork unitOfWork,
            ILogger<AuthController> logger,
            IMapper mapper,
            UserManager<ApiUser> usermanager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _usermanager = usermanager;
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

                var result = await _usermanager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogInformation($"User registration failed:  {error.Code}: {error.Description}");

                        _logger.LogInformation($"Username {user.UserName} firstname: {user.FirstName}");
                    }
                    return BadRequest("Something went wrong");
                }

                await _usermanager.AddToRoleAsync(user, "User");

                return Ok("Successfully registered");

            }
            catch (Exception ex)
            {
                _logger.LogError("Server Error: ", ex.Message);
                return StatusCode(500, "Internal Server error, please try again later");
            }

        }
    }
}
