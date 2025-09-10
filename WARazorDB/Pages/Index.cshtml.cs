using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WARazorDB.Data;
using WARazorDB.Models;

namespace WARazorDB.Pages
{
    public class IndexModel : PageModel
    {
        private readonly WARazorDB.Data.TareaDbContext _context;

        public IndexModel(WARazorDB.Data.TareaDbContext context)
        {
            _context = context;
        }

        public IList<Tarea> Tarea { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Tarea = await _context.Tareas.ToListAsync();
        }

        public string GetFechaClass(DateTime fecha)
        {
            if (fecha.Date < DateTime.Now.Date)
                return "fecha-vencida";
            if (fecha.Date == DateTime.Now.Date)
                return "fecha-proxima";
            return "fecha-normal";
        }

        
    }
}
