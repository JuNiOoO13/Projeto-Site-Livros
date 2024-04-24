using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CantoLiterário___projeto_LivroOnline.Models
{
    public class User : IdentityUser
    {
        
        public override string? UserName { get => base.UserName; set => base.UserName = value; }
        public double Avalia { get; set; } = 0;
        public string? ImgUrl { get; set; }
        public double AvarageVotes { get; set; } = 0;
        public string? Bio { get; set; }
        public string? BackgroundUrl { get; set; }
        public virtual ICollection<User>? Followers { get; set; }
        public virtual ICollection<User>? Follows { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
        public virtual ICollection<Comments>? Comments { get; set; }
    }
}
