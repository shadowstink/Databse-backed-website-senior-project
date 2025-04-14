using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicDatabase6.Models
{
    public class Musician
    {
        public required int MusicianID { get; set; }
        [Required(ErrorMessage = "Please enter a FName!")]
        public string MusFName { get; set; }
        [Required(ErrorMessage = "Please enter a LName!")]
        public string MusLName { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? ImageName { get; set; }
    }
}
