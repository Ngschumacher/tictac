using System.ComponentModel.DataAnnotations;

namespace TicTac.Core.Models {
    public class User {
        [Key] 
        public int Id { get; set; }
        public string Username { get; set; }
    }
}