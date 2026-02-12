using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages_Todos
{
    public class DeleteModel : PageModel
    {
        private readonly WebApplication1.Data.AppDbContext _context;

        public DeleteModel(WebApplication1.Data.AppDbContext context)
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

            var todoitem = await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);

            if (todoitem is not null)
            {
                TodoItem = todoitem;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoitem = await _context.Todos.FindAsync(id);
            if (todoitem != null)
            {
                TodoItem = todoitem;
                _context.Todos.Remove(TodoItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
