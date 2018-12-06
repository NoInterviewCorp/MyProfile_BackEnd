using System.Collections.Generic;

namespace My_Profile
{
    public class AttemptedConcept
    {
        public string ConceptName { get; set; }
        public int QuestionAttemptedCorrect { get; set; }
        public int TotalQuestionAttempted { get; set; }
        public AttemptedConcept()
        {
            
        }
        public AttemptedConcept(string concept)
        {
            this.ConceptName = concept;
            QuestionAttemptedCorrect = 0;
            TotalQuestionAttempted = 0;
        }
    }
}