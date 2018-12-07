using System.Collections.Generic;

namespace My_Profile.Models
{
    public class UserReport
    {
        public string UserId { get; set; }
        public List<TechnologyReport> TechnologyReports { get; set; }
        
    }
}