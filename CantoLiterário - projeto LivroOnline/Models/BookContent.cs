using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CantoLiterário___projeto_LivroOnline.Models
{
    public class BookContent
    {
        public int BookContentId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public double AvarageVotes { get; set; } = 0;

        public int BookId { get; set; }
        public Book Book { get; set; }
        public virtual ICollection<Comments>? comments { get; set; }
        public virtual ICollection<CapContent>? Lines { get; set; }
    }
}
