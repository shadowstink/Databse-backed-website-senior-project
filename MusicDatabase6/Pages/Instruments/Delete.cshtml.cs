using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDatabase6.Data;
using MusicDatabase6.Models;

namespace MusicDatabase6.Pages.Instruments
{
    public class DeleteModel : PageModel
    {
        private readonly MusicDatabase6.Data.ApplicationDbContext _context;

        public DeleteModel(MusicDatabase6.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Instrument Instrument { get; set; } = default!;

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
            else
            {
                Instrument = instrument;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument.FindAsync(id);
            if (instrument != null)
            {
                Instrument = instrument;
                _context.Instrument.Remove(Instrument);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
