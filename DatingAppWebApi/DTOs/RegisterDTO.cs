using System.ComponentModel.DataAnnotations;

namespace DatingAppWebApi.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [RegularExpression("([a-zA-Z0-9_\\s]+)")]
        public  string UserName { get; set; }


        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
        public  string Email { get; set; }



        [Required]
        [MinLength(4)]
        public  string Password { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }
    }
}
