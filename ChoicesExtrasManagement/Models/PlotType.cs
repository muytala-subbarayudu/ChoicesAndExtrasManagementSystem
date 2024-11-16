using System.ComponentModel.DataAnnotations;

namespace ChoicesExtrasManagement.Models
{
    public class PlotType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

    }
}
