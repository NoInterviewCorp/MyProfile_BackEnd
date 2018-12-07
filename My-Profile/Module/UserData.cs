using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Profile {
    public class UserData {
        public string UserId { get; set; }
        public QuizData Data;
        public UserData (string userId, QuizData data) {
            UserId = userId;
            Data = data;
        }
    }
}