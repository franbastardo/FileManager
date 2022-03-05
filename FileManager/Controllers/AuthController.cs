using AutoMapper;
using FileManager.DTOs;
using FileManager.IRepository;
using FileManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IEncrypting _encrypting;

        public AuthController(IUnitOfWork unitOfWork, ILogger<AuthController> logger, IMapper mapper, IEncrypting encrypting)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _encrypting = encrypting;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attemps for {userDTO.Credentials.Email} ");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var existingEmail = await _unitOfWork.Credentials.Get(q => q.Email == userDTO.Credentials.Email);

                if (existingEmail != null)
                {
                    return BadRequest(new ResponseDTO { Message = "Email already used" });
                }
                userDTO.Credentials.Password = _encrypting.HashPassword(userDTO.Credentials.Password);

                var newUser = _mapper.Map<User>(userDTO);
                var insertedUser = await _unitOfWork.Users.Insert(newUser);
                await _unitOfWork.Save();

                return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the {nameof(User)}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation($"Login Attemps for {loginDTO.Email} ");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var existingEmail = await _unitOfWork.Credentials.Get(q => q.Email == loginDTO.Email);
                if (existingEmail == null)
                {
                    return BadRequest(new ResponseDTO { Message = "Email not found" });
                }
                if (!_encrypting.ComparePasswords(loginDTO.Password, existingEmail.Password))
                {
                    return Unauthorized(new ResponseDTO { Message = "Wrong Credentials" });
                }
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the {nameof(User)}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
