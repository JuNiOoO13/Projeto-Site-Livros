using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CantoLiterário___projeto_LivroOnline.Models.DTOs
{
    public class CommentDTO
    {
        public string? CommentsId { get; set; }
        public string? Mensagem { get; set; }


        public string? AuthorId { get; set; }
        public User? User { get; set; }
        public virtual ICollection<Comments>? Replys { get; set; }

        public int? CommentId { get; set; }
        public Comments? Comment { get; set; }

        public int BookContentId { get; set; }
        public BookContent? BookContent { get; set; }
    }
}
