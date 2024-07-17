using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Application.DTOs
{
    public class RunningActivityDto
    {
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public double Distance { get; set; }
        public string Location { get; set; }
        public int UserId { get; set; }
    }
}
