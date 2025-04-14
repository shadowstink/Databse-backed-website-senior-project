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

namespace MusicDatabase6.Pages.Musicians
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
        public Musician Musician { get; set; } = default!;

        // Bind the file upload separately.
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musician.FirstOrDefaultAsync(m => m.MusicianID == id);
            if (musician == null)
            {
                return NotFound();
            }
            Musician = musician;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Retrieve the existing musician record.
            var musicianToUpdate = await _context.Musician.FirstOrDefaultAsync(m => m.MusicianID == Musician.MusicianID);
            if (musicianToUpdate == null)
            {
                return NotFound();
            }

            // Update the basic fields.
            musicianToUpdate.MusFName = Musician.MusFName;
            musicianToUpdate.MusLName = Musician.MusLName;

            // If a new file has been uploaded, process it.
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Generate a unique file name using a GUID.
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var imageFolder = Path.Combine(_environment.WebRootPath, "image");

                // Ensure that the folder exists.
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                var filePath = Path.Combine(imageFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Update the ImageName field.
                musicianToUpdate.ImageName = fileName;
            }
            // Else, no file was uploaded so the existing ImageName is preserved.

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicianExists(musicianToUpdate.MusicianID))
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

        private bool MusicianExists(int id)
        {
            return _context.Musician.Any(e => e.MusicianID == id);
        }
    }
}
