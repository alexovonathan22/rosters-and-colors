using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Roster.Core.ApiModels
{
    public class DeleteModel
    {
        [Required]
        public int Id { get; set; }
        public int RemoveColor { get; set; } // 1 or 0;
        public int RemoveRoster { get; set; } // 1 or 0;

    }
}
