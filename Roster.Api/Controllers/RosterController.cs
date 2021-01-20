using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roster.Core.ApiModels;
using Roster.Core.Service.Interfaces;
using Roster.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roster.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RosterController : ControllerBase
    {
        private readonly IRosterService _rosterservice;
        public RosterController(IRosterService rosterservice)
        {
            _rosterservice = rosterservice;
        }

        /// <summary>
        /// Endpoint to get all available colors.
        /// TO CREATE A ROSTER THE USER MUST FOR CALL THE GETALLCOLORS ENDPOINT, IF THERE WAS A CLIENT APP IT WOULD BE A DROPDOWN
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetColors")]
        public async Task<IActionResult> GetColors()
        {
            var response = new APIResponse();
            var (entity, message) = await _rosterservice.GetAllAvailableColors();
            if (entity != null)
            {
                response.Result = entity;
                response.ApiMessage = message;
                response.StatusCode = "00";
                return Ok(response);
            }
            response.ApiMessage = message;
            response.Result = entity;

            return BadRequest(response);
        }

        // POST api/<RosterController>/AuthorizedMember
        /// <summary>
        /// This Endpoint helps to create a roster, it is created by a Church Admin.
        /// Memebers cannot create.
        /// Below is the payload to pass.
        /// TO CREATE A ROSTER THE USER MUST FOR CALL THE GetColors() ENDPOINT, IF THERE WAS A CLIENT APP IT WOULD BE A DROPDOWN
        /// </summary>
        /// <remarks>
        /// {
        ///     rosterName: "North",
        ///     color: "blue Ivy"
        /// }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Policy = "AuthorizedAdmin")]
        [HttpPost("create")]
        public async Task<IActionResult> Post(RosterDetails model)
        {
            var response = new APIResponse();
            var (entity, message) = await _rosterservice.CreateRoster(model);
            if (entity != null)
            {
                response.Result = entity;
                response.ApiMessage = message;
                response.StatusCode = "00";
                return Ok(response);
            }
            response.ApiMessage = message;
            response.Result = entity;

            return BadRequest(response);

        }



        /// <summary>
        /// This endpoint carries out a delete operation for both color and roster.
        /// (1.) if Color option is set it will only delete the color of that roster.
        /// (2.) if Roster option is set it will delete both the roster and color.
        /// if none is set the endpoint will do (2.)
        /// Below is the payload to pass.
        /// </summary>
        /// 
        /// <remarks>
        /// {
        ///     id: 1,
        ///     RemoveColor: 0,
        ///     RemoveRoster: 1
        /// }
        /// </remarks>
        /// <param name="model"></param>
        // DELETE api/<RosterController>/5
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(DeleteModel model)
        {
            var response = new APIResponse();
            var (entity, message) = await _rosterservice.DeleteColorOrRoster(model);
            if (entity != null)
            {
                response.Result = entity;
                response.ApiMessage = message;
                response.StatusCode = "00";
                return Ok(response);
            }
            response.ApiMessage = message;
            response.Result = entity;

            return BadRequest(response);
        }
    }
}
