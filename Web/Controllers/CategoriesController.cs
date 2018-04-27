using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web.Authorization;
using Web.Data;

namespace Web.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IAuthorizationService _authorizationService;

		public CategoriesController(ApplicationDbContext context, IAuthorizationService authorizationService)
		{
			_context = context;
			_authorizationService = authorizationService;
		}

		// GET: Categories
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Categories.Include(c => c.User);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Categories/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _context.Categories
				.Include(c => c.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// GET: Categories/Create
		public IActionResult Create()
		{
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
			return View();
		}

		// POST: Categories/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[AuthorizeResource(typeof(InsertCategoryRequirement))]
		public async Task<IActionResult> CreateWithAuth([Bind("Id,Name,UserId")] Category category)
		{
			if (ModelState.IsValid)
			{
				_context.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", category.UserId);
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]]
		public async Task<IActionResult> Create([Bind("Id,Name,UserId")] Category category)
		{
			var authorizationResult = await _authorizationService.AuthorizeAsync(User, category, new InsertCategoryRequirement());
			if (!authorizationResult.Succeeded) // If user has no access show him 403
			{
				return new ForbidResult();
			}


			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", category.UserId);

			// If ModelState isn't valid, return to view immediately
			if (!ModelState.IsValid)
			{
				return View(category);
			}

			// Add category, save changes and redirect to Category/Index - Categories List page
			_context.Add(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


		// GET: Categories/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", category.UserId);
			return View(category);
		}

		// POST: Categories/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Id,Name,UserId")] Category category)
		{
			if (id != category.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(category);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CategoryExists(category.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", category.UserId);
			return View(category);
		}

		// GET: Categories/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _context.Categories
				.Include(c => c.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var category = await _context.Categories.FindAsync(id);
			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CategoryExists(string id)
		{
			return _context.Categories.Any(e => e.Id == id);
		}
	}
}
