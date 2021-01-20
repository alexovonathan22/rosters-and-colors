using System;
using System.Collections.Generic;
using System.Text;

namespace Roster.Core.Models
{
    public class Color : BaseEntity
    {
        public string ColorName { get; set; }
        public string HexValue { get; set; }
    }
}
