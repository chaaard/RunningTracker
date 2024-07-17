using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunningTracker.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Weight { get; set; }
        public double Height { get; set; }
        public DateTime BirthDate { get; set; }

        [NotMapped]
        public int Age => CalculateAge();
        [NotMapped]
        public double BMI => CalculateBMI();

        public ICollection<RunningActivity> RunningActivities { get; set; } = new List<RunningActivity>();

        private int CalculateAge()
        {
            var today = DateTime.Today;
            return today.Year - BirthDate.Year - (BirthDate.Date > today.AddYears(-1) ? 1 : 0);
        }

        private double CalculateBMI()
        {
            if (Height == 0) return 0;
            return Weight / ((Height / 100) * (Height / 100));
        }
    }
}
