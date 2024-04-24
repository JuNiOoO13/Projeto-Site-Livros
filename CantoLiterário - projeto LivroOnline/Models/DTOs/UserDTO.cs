namespace CantoLiterário___projeto_LivroOnline.Models.DTOs
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public double Avalia { get; set; }
        public string? ImgUrl { get; set; }
        public double AvarageVotes { get; set; }
        public string? Bio { get; set; }
        public string? BackgroundUrl { get; set; }
        public virtual ICollection<User>? Followers { get; set; }
        public virtual ICollection<User>? Follows { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
        public virtual ICollection<Comments>? Comments { get; set; }
    }
}
