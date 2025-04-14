using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicDatabase6.Data;
using MusicDatabase6.Models;

namespace MusicDatabase6.Pages.Songs
{
    public class DetailsModel : PageModel
    {
        private readonly MusicDatabase6.Data.ApplicationDbContext _context;

        public DetailsModel(MusicDatabase6.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Song Song { get; set; } = default!;

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
            else
            {
                Song = song;
            }
            return Page();
        }
    }
}
