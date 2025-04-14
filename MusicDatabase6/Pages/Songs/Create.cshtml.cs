using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicDatabase6.Data;
using MusicDatabase6.Models;

namespace MusicDatabase6.Pages.Songs
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
        public Song Song { get; set; } = default!;

        // Bind the uploaded file separately.
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

            // Process the image file upload if provided.
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Generate unique file name.
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var imageFolder = Path.Combine(_environment.WebRootPath, "image");

                // Ensure that the image folder exists.
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                var filePath = Path.Combine(imageFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Set the ImageName property so it is saved to the DB.
                Song.ImageName = fileName;
            }

            _context.Song.Add(Song);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
