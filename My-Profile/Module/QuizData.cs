using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Profile
{
    public class QuizData
    {
        public string TechName { get; set; }
        public string AttemptedOn { get; set; }
        public List<AttemptedConcept> ConceptsAttempted = new List<AttemptedConcept>();
        public QuizData()
        {
            
        }
        public QuizData(string techname, string attemptedon ,List<AttemptedConcept> conceptattempted)
        {
            TechName = techname;
            AttemptedOn = attemptedon;
            ConceptsAttempted.AddRange(conceptattempted);
        }
    }
}