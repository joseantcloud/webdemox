using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages_Todos
{
    public class EditModel : PageModel
    {
        private readonly WebApplication1.Data.AppDbContext _context;

        public EditModel(WebApplication1.Data.AppDbContext context)
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

            var todoitem =  await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);
            if (todoitem == null)
            {
                return NotFound();
            }
            TodoItem = todoitem;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TodoItem).State = EntityState.Modified;

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
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TodoItemExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}
