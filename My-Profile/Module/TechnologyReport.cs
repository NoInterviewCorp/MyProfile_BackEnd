using System.Collections.Generic;

namespace My_Profile.Models
{
    public class TechnologyReport
    {
        public string TechnologyName { get; set; }
        public List<ConceptReport> ConceptReports { get; set; }
    }
}