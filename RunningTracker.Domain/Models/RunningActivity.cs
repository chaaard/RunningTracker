using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Domain.Models
{
    public class RunningActivity
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double Distance { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public double Duration => (EndDateTime - StartDateTime).TotalMinutes;

        [NotMapped]
        public double AveragePace => Duration > 0 ? Duration / Distance : 0;
    }
}
