using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Roster.Core.Service.Interfaces;
using Roster.Core.Models;
using Roster.Core.Util;
using Roster.Core.ApiModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roster.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly ILogger<AuthController> _log;


        public AuthController(IAuthService auth, ILogger<AuthController> log)
        {
            _auth = auth;
            _log = log;
        }

        /// <summary>
        /// This Endpoint logs users in to the app.
        /// </summary>
        /// <remarks>
        /// Use as sample User login
        /// {
        ///    "username": "OvoDGreat",
        ///    "password": "123456"
        /// }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        // api/<AuthController>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel user)
        {
            _log.LogInformation($"Attempting to log user {user.Username}");
            var response = new APIResponse();
            response.ApiMessage = $"Error: Login credentials incorrect!";
            response.StatusCode = "01";
            response.Result = null;
            var jwt = await _auth.LogUserIn(user);
            if (jwt != null)
            {
                response.Result = jwt;
                response.StatusCode = "00";
                response.ApiMessage = "Successful Login!";
                return Ok(response);
            }

            return BadRequest(response);

        }

    /// <summary>
    /// This Endpoint creates new users.
    /// </summary>
    /// <remarks>
    /// Use as sample User
    /// {
    ///     
    ///    "username": "OvoDGreat",
    ///    "email": "OvoDGreat@gmail.com",
    ///    "password": "123456"
    ///
    /// }
    /// Note When you run the application for first time, admin is created
    /// with username=adminovo, password=01234Admin
    /// </remarks>
    /// <param name="userdto"></param>
    /// <returns></returns>
    [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(SignUpModel userdto)
        {
            var response = new APIResponse();
            response.StatusCode = "01";
            response.Result = null;
            var (user, message) = await _auth.RegisterUser(userdto); ;
            if (user != null)
            {
                response.Result = user;
                response.StatusCode = "00";
                response.ApiMessage = message;
                return Ok(response);
            }
            response.ApiMessage = message;
            
            return BadRequest(response);
        }

        

        // [HttpPost("refresh")]
        // public async Task<IActionResult> RefreshToken(SignUp userdto)
        // {
        //     var response = new APIResponse();
        //     response.StatusCode = "01";
        //     response.Result = null;
        //     var (user, message) = await _auth.RegisterUser(userdto); ;
        //     if (user != null)
        //     {
        //         response.Result = user;
        //         response.StatusCode = "00";
        //         response.ApiMessage = message;
        //         return Ok(response);
        //     }
        //     response.ApiMessage = message;

        //     return BadRequest(response);
        // }

    }
}
