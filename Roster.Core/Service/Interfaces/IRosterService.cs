using Roster.Core.ApiModels;
using Roster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Roster.Core.Service.Interfaces
{
    public interface IRosterService
    {
        Task<(ResponseModel response, string message)> CreateRoster(RosterDetails model);
        Task<(ResponseModel response, string message)> AddColorToRoster(RosterDetails model);
        Task<(ResponseModel response, string message)> DeleteColorOrRoster(DeleteModel model);

        Task<(object response, string message)> GetAllAvailableColors();
    }
}
