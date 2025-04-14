using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDatabase6.Data;
using MusicDatabase6.Models;

namespace MusicDatabase6.Pages.Musicians
{
    public class DeleteModel : PageModel
    {
        private readonly MusicDatabase6.Data.ApplicationDbContext _context;

        public DeleteModel(MusicDatabase6.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Musician Musician { get; set; } = default!;

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
            else
            {
                Musician = musician;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musician = await _context.Musician.FindAsync(id);
            if (musician != null)
            {
                Musician = musician;
                _context.Musician.Remove(Musician);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
