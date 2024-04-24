using System.ComponentModel.DataAnnotations;

namespace CantoLiterário___projeto_LivroOnline.Models
{
    public class CapContent
    {
        public int CapContentId { get; set; }
        public string? Content { get; set; }
        public int BookContentId { get; set; }
        public BookContent Book { get; set; }

    }
}
