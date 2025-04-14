using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicDatabase6.Models
{
    public class Instrument
    {
        public int InstrumentID { get; set; }
        [Required(ErrorMessage = "Please enter an ID!")]

        [Display(Name = "Instrument Serial")]
        public int InstrumentSerial { get; set; }
        [Required(ErrorMessage = "Please enter a Serial!")]
        [Display(Name = "Instrument Type")]
        public string InstrumentType { get; set; }

        [Required(ErrorMessage = "Please enter a Type!")]
        [Display(Name = "Returned?")]
        public string IsReturned { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? ImageName { get; set; }
    }
}
