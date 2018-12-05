using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Profile
{

    public class LearningPlanSubscriptionWrapper
    {
        //public int LearningPlanFeedBackId { get; set; }
        public string LearningPlanId { get; set; }
        public string UserId { get; set; }
        //public int Star { get; set; }
       // public int Subscribe { get; set; }

    }
public class LearningPlanRatingWrapper
    {
       // public int LearningPlanFeedBackId { get; set; }
        public string LearningPlanId { get; set; }
        public string UserId { get; set; }
        public int Star { get; set; }
      //  public int Subscribe { get; set; }

    }
}
