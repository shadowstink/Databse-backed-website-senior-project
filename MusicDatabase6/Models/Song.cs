using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicDatabase6.Models
{
    public class Song
    {
        public required int SongID { get; set; }
        [Required(ErrorMessage = "Please enter an ID!")]

        public required string SongName { get; set; }
        [Required(ErrorMessage = "Please enter a Serial!")]

        public required string SongArtist { get; set; }

        [Required(ErrorMessage = "Please enter a Type!")]
        public string BandPlaying { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? ImageName { get; set; }
    }
}
