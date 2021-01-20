using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Roster.Core.ApiModels;
using Roster.Core.DataAccess;
using Roster.Core.Models;
using Roster.Core.Service.Interfaces;
using Roster.Core.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Roster.Core.Service
{
    public class RosterService : IRosterService
    {

        private readonly IRepository<Rosters> _rosrepo;
        private readonly IRepository<Color> _colorepo;

        private readonly IRepository<User> _userrepo;
        private readonly ILogger<RosterService> log;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IHttpClientFactory _httpClientFactory { get; set; }
        public RosterService(ILogger<RosterService> log, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IRepository<Rosters> rosrepo, IRepository<User> userrepo, IHttpClientFactory httpClientFactory, IRepository<Color> colorepo)
        {
            this.log = log;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _rosrepo = rosrepo;
            _userrepo = userrepo;
            _httpClientFactory = httpClientFactory;
            _colorepo = colorepo;
        }

        public async Task<(ResponseModel response, string message)> DeleteColorOrRoster(DeleteModel model)
        {
            if(model.Id < 1 || model.RemoveColor >1 || model.RemoveColor < 0 || model.RemoveRoster > 1|| model.RemoveRoster < 0)
            {
                return (response: null, message: $"Check parameters entered.");
            }

            //check the id exist if it does continue
            var roster = await _rosrepo.FirstOrDefault(f => f.Id == model.Id);
            if (roster == null) return (response: null, message: $"Roster doesnt exist.");
            //check if remove parameters are set
            ResponseModel res = new ResponseModel();
            string msg = "";
            if (model.RemoveRoster==0 && model.RemoveColor == 0)
            {
                await _rosrepo.Remove(roster); 
                msg= $"Roster is deleted. Color/Roster was not set{model.RemoveRoster}";
            }
            if(model.RemoveRoster == 0 && model.RemoveColor == 1)
            {
                roster.Color = "";
                await _rosrepo.Update(roster);
                msg= $"Roster's color is deleted. Colr was set {model.RemoveColor}";
            }
            if (model.RemoveRoster == 1 && model.RemoveColor == 0)
            {
                await _rosrepo.Remove(roster);
                msg = $"Roster is deleted. Roster was set{model.RemoveRoster}";
            }
            //and carry out delete actions accordingly
            res.Color = roster.Color;
            res.RosterName = roster.Name;
            res.Id = roster.Id;
            return (response: res, message: msg);


        }

        public async Task<(ResponseModel response, string message)> AddColorToRoster(RosterDetails model)
        {
            if (model.Color == null || model.RosterName == null)
            {
                return (response: null, message: $"An error occured. Check details passed.");
            }
            //Check if the roster to change color exists,
            var roster = await _rosrepo.FirstOrDefault(t=>t.Name==model.RosterName);
            if (roster == null) return (response: null, message: $"Rsoter doesnt exist. So color cant be added.");
            //CHeck if color is approved color, if true go to next line
            var toCapsColor = model.Color;
            var hexColor = configuration[$"SupportedColors:{toCapsColor}"];
            if (string.IsNullOrEmpty(hexColor)) return (response: null, message: $"Enter a valid color. call getcolors endpoint.");

            try
            {
                roster.Color = model.Color;
                await _rosrepo.Update(roster);
            }catch(DbUpdateException e)
            {
                log.LogError($"Couldnt update. {e.Message}.");
            }
            var res = new ResponseModel
            {
                Id = roster.Id,
                RosterName = roster.Name,
                Color = roster.Color
            };
            return (response:res, message:$"Updated color.");
            //if it does add color to roster and return response to user
        }

        public async Task<(ResponseModel response, string message)> CreateRoster(RosterDetails model)
        {
            // Validate model and check the user role that is accessing this method
            // return immediately if model is null, or authorization not enough.
            // TO CREATE A ROSTER THE USER MUST FOR CALL THE GETALLCOLORS ENDPOINT, IF THERE WAS A CLIENT APP IT WOULD BE A DROPDOWN
            if (model.Color == null || model.RosterName ==null)
            {
                return (response: null, message: $"An error occured. Check details passed.");
            } 
            var userctx = _httpContextAccessor.HttpContext.User.Identity.Name;
            var userInCtx = await _userrepo.FirstOrDefault(r => r.Username == userctx);
            if (userInCtx.Role != UserRoles.Admin)
            {
                return (response: null, message: $"You are not an admin.");
            }

            var toCapsColor = model.Color;
            var hexColor = configuration[$"SupportedColors:{toCapsColor}"];
            if (string.IsNullOrEmpty(hexColor)) return (response: null, message: $"Enter a valid color. call getcolors endpoint.");
            var roster = new RosterDetails().PopulateRoster(model);

            // Go on and create the roster.
            var newroster = await _rosrepo.Insert(roster);
            var res = new ResponseModel
            {
                Id = newroster.Id,
                RosterName = newroster.Name,
                Color = newroster.Color
            };
            return (response: res, message:$"Roster created {model.RosterName}.");
        }

        /// <summary>
        /// A service method to get all available colors.
        /// </summary>
        /// <returns></returns>
        public async Task<(object response, string message)> GetAllAvailableColors()
        {
            //Do note this is done for convinience sake.

            var colors = new List<string>();
            colors.Add("Baker-Miller pink");
            colors.Add("Bright yellow");
            colors.Add("Coquelicot");
            colors.Add("Candy apple red");
            colors.Add("Bisque");
            colors.Add("Atomic tangerine");
            colors.Add("Amber");

            /* colors.Add("Baker-Miller pink", "#FF91AF");
            colors.Add("Bright yellow", "#FFAA1D");
            colors.Add("Coquelicot", "#FF3800");
            colors.Add("Candy apple red", "#FF0800");
            colors.Add("Bisque", "#FFE4C4");
            colors.Add("Atomic tangerine", "#FF9966");
            colors.Add("Amber", "#FFBF00");*/

            return (response:colors, message:$"These are the colors you can use.");
           
        }
    }
}