using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDatabase6.Data;
using MusicDatabase6.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MusicDatabase6.Pages.Instruments
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Bind the Instrument object
        [BindProperty]
        public Instrument Instrument { get; set; }

        // Bind the file separately.
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // File upload logic—adapted from your MVC controller
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Generate a new unique file name using a GUID and preserve the file extension.
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);

                // Determine the path to the image folder inside wwwroot.
                var imagePath = Path.Combine(_environment.WebRootPath, "image");

                // Ensure the directory exists.
                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                // Save the file.
                var filePath = Path.Combine(imagePath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Save the file name in the model.
                Instrument.ImageName = fileName;
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please upload an image file.");
                return Page();
            }

            // Save the instrument to the database.
            _context.Instrument.Add(Instrument);
            await _context.SaveChangesAsync();

            // Redirect to the Index page.
            return RedirectToPage("Index");
        }
    }
}
