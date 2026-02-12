using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages_Todos
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TodoItem TodoItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoitem = await _context.Todos.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (todoitem == null)
            {
                return NotFound();
            }

            TodoItem = todoitem;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Traer el registro actual desde DB (para proteger campos no editables como CreatedAtUtc)
            var existing = await _context.Todos.FirstOrDefaultAsync(t => t.Id == TodoItem.Id);
            if (existing == null)
            {
                return NotFound();
            }

            // Actualizar solo campos editables
            existing.Title = TodoItem.Title;
            existing.IsDone = TodoItem.IsDone;

            // Si existe DueAtUtc y viene del form como Unspecified, convertir a UTC
            if (TodoItem.DueAtUtc.HasValue)
            {
                var localTime = DateTime.SpecifyKind(TodoItem.DueAtUtc.Value, DateTimeKind.Local);
                existing.DueAtUtc = localTime.ToUniversalTime();
            }
            else
            {
                existing.DueAtUtc = null;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(TodoItem.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool TodoItemExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}
