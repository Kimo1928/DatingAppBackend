using System.ComponentModel.DataAnnotations;

namespace DatingAppWebApi.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [RegularExpression("([a-zA-Z0-9_\\s]+)")]
        public  string DisplayName { get; set; }


        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
        public  string Email { get; set; }



        [Required]
        [MinLength(4)]
        public  string Password { get; set; }
    }
}
