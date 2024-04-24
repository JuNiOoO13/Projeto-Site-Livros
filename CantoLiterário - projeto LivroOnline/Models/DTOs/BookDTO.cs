using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CantoLiterário___projeto_LivroOnline.Models.DTOs
{
    public class BookDTO
    {
        public int? BookId { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Informe Um nome")]
        public string Name { get; set; }
        public string? ImgUrl { get; set; }
        public double AvarageVotes { get; set; }

        [ForeignKey("User")]
        public string? AuthorId { get; set; }
        public User? Author { get; set; }

        public int GendersId { get; set; }
        public Genders? Genders { get; set; }
        public BookContent? MainContent { get; set; }
        public virtual ICollection<BookContent>? bookContents { get; set; }
        public int? Lidos { get; set; } = 0;
        public double? Votes { get; set; } = 0;
        public DateTime? ReadTme { get; set; }
        public string? Description { get; set; }
        public int? Publicado { get; set; }
    }
}
