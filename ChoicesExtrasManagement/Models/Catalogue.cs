using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.Models
{
    public class Catalogue
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool SingleOrMultiple { get; set; }

    }
}
