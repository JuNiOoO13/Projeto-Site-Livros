using System.ComponentModel.DataAnnotations;

namespace CantoLiterário___projeto_LivroOnline.Models.DTOs
{
    public class ContentDTO
    {
        public int BookContentId { get; set; }
        [StringLength(50)]
        public string? Title { get; set; }
        public DateTime? Date { get; set; }
        public double? AvarageVotes { get; set; } = 0;

        public int? BookId { get; set; }
        public Book? Book { get; set; }
        public virtual ICollection<Comments>? comments { get; set; }
        public virtual ICollection<CapContent>? Lines { get; set; }
        public string? ProxName { get; set; }
    }
}
