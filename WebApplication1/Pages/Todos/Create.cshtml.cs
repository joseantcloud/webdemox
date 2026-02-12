using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages_Todos
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TodoItem TodoItem { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Siempre establecer fecha de creación en UTC
            TodoItem.CreatedAtUtc = DateTime.UtcNow;

            // Si el formulario envía un datetime-local,
            // viene como Kind = Unspecified → convertir a UTC
            if (TodoItem.DueAtUtc.HasValue)
            {
                var localTime = DateTime.SpecifyKind(
                    TodoItem.DueAtUtc.Value,
                    DateTimeKind.Local
                );

                TodoItem.DueAtUtc = localTime.ToUniversalTime();
            }

            _context.Todos.Add(TodoItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
