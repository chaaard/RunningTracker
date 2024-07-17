using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Application.DTOs
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
