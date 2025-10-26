namespace DatingAppWebApi.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
