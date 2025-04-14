using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDatabase6.Data;
using MusicDatabase6.Models;

namespace MusicDatabase6.Pages.Musicians
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

        [BindProperty]
        public Musician Musician { get; set; } = default!;

        // Bind the file separately; note that we use a separate property and not Musician.ImageFile.
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if a file was uploaded
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Generate a unique file name using a GUID while preserving the extension.
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var imageFolder = Path.Combine(_environment.WebRootPath, "image");

                // Ensure the image folder exists
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                var filePath = Path.Combine(imageFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Save the unique file name in the database field.
                Musician.ImageName = fileName;
            }
            // (You could add an else clause here to issue a validation error if an image is required)

            _context.Musician.Add(Musician);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
