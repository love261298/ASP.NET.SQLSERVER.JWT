namespace ASP.NET.SQLSERVER.JWT.Data.Models
{
    public class UserUpdateDTO
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? ImageUrl { get; set; }

        public string? Password { get; set; }
    }
}
