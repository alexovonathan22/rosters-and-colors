using Roster.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Roster.Core.ApiModels
{
    public class RosterDetails
    {
        public string RosterName { get; set; }
        public string Color { get; set; }

        internal Rosters PopulateRoster(RosterDetails model)
        {
            
            if (model == null) return null;
            var roster = new Rosters();
            roster.Name = model.RosterName;
            roster.Color = model.Color;
            roster.CreatedAt = DateTime.Now;
            roster.ModifiedAt = DateTime.Now;

            return roster;
        }
    }
}
