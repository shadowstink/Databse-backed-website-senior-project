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

namespace MusicDatabase6.Pages.Instruments
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

        // Bind the Instrument; this contains database fields.
        [BindProperty]
        public Instrument Instrument { get; set; } = default!;

        // Bind the file separately.
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument.FirstOrDefaultAsync(m => m.InstrumentID == id);
            if (instrument == null)
            {
                return NotFound();
            }
            Instrument = instrument;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Retrieve the existing instrument from the database to retain its current image if needed.
            var instrumentToUpdate = await _context.Instrument.FirstOrDefaultAsync(i => i.InstrumentID == Instrument.InstrumentID);
            if (instrumentToUpdate == null)
            {
                return NotFound();
            }

            // Update the basic fields from the posted data.
            instrumentToUpdate.InstrumentSerial = Instrument.InstrumentSerial;
            instrumentToUpdate.InstrumentType = Instrument.InstrumentType;
            instrumentToUpdate.IsReturned = Instrument.IsReturned;

            // If a new image file is uploaded, save it and update the ImageName.
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Generate a unique file name using a GUID
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var imagePath = Path.Combine(_environment.WebRootPath, "image");

                // Ensure that the directory exists
                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                var filePath = Path.Combine(imagePath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                instrumentToUpdate.ImageName = fileName;
            }
            else
            {
                // No new file uploaded—retain the current image.
                // (instrumentToUpdate.ImageName is left unchanged.)
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstrumentExists(instrumentToUpdate.InstrumentID))
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

        private bool InstrumentExists(int id)
        {
            return _context.Instrument.Any(e => e.InstrumentID == id);
        }
    }
}
