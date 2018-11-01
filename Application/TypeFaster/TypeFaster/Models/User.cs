using System.ComponentModel.DataAnnotations;

namespace TypeFaster.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
    }
}