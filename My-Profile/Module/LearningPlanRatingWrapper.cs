using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Profile
{

    public class LearningPlanRatingWrapper
    {
        public string LearningPlanId { get; set; }
        public string UserId { get; set; }
        public int Star { get; set; }

    }
}