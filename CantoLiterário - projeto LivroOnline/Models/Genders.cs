using System.ComponentModel.DataAnnotations;

namespace CantoLiterário___projeto_LivroOnline.Models
{
    public class Genders
    {
        public int GendersId { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
        public string description { get; set; }
        public string? Color { get; set; }
    }
}
