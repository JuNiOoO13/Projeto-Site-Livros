using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CantoLiterário___projeto_LivroOnline.Models
{
    public class Comments
    {
        public int CommentsId { get; set; }
        public string? Titulo { get; set; }
        public string Mensagem { get; set; }

        public virtual ICollection<Comments>? Replys { get; set; }

        public int? CommentId { get; set; }
        public Comments? Comment { get; set; }
        [ForeignKey("User")]
        public string? AuthorId { get; set; }
        public User User { get; set; }

        public int BookContentId { get; set; }
        public BookContent BookContent { get; set; }
        
    }
}
