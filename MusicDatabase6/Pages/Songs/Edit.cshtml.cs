using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDatabase6.Data;
using MusicDatabase6.Models;

namespace MusicDatabase6.Pages.Songs
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Song Song { get; set; } = default!;

        // Bind file upload separately.
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Song.FirstOrDefaultAsync(m => m.SongID == id);
            if (song == null)
            {
                return NotFound();
            }
            Song = song;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Retrieve existing record.
            var songToUpdate = await _context.Song.FirstOrDefaultAsync(m => m.SongID == Song.SongID);
            if (songToUpdate == null)
            {
                return NotFound();
            }

            // Update basic fields.
            songToUpdate.SongName = Song.SongName;
            songToUpdate.SongArtist = Song.SongArtist;
            songToUpdate.BandPlaying = Song.BandPlaying;

            // If a new file is uploaded, process it.
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var imageFolder = Path.Combine(_environment.WebRootPath, "image");

                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                var filePath = Path.Combine(imageFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Update the image name field.
                songToUpdate.ImageName = fileName;
            }
            // If no file is uploaded, the previous image name remains.

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(songToUpdate.SongID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SongExists(int id)
        {
            return _context.Song.Any(e => e.SongID == id);
        }
    }
}
