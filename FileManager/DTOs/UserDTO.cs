using System.ComponentModel.DataAnnotations;

namespace FileManager.DTOs
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage ="Your password must have between {2} and {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

    }
    public class UserDTO 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public LoginDTO Credentials { get; set; }
        
    }
}
