using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChoicesExtrasManagement.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location? Location { get;set; }
    }
}
