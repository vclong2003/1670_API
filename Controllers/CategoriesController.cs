using _1670_API.Data;
using _1670_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public CategoriesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: /api/category
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var categories = await _dataContext.Categories.Select(c => new { c.Id, c.Name }).ToListAsync();
            return StatusCode(200, categories);
        }

        // GET: /api/category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOne(int id)
        {
            var result = await _dataContext.Categories.FindAsync(id);

            return StatusCode(200, result);
        }

        // POST: /api/category
        // Body parameters: name
        [HttpPost]
        public async Task<ActionResult> Add(CategoryDTO categoryDTO)
        {
            Category newCategory = new()
            {
                Name = categoryDTO.Name,
            };

            _dataContext.Categories.Add(newCategory);

            await _dataContext.SaveChangesAsync();

            return StatusCode(200, newCategory);
        }

        // PUT: /api/category/{id}
        // Body parameters: name
        [HttpPut("id")]
        public async Task<ActionResult> Update(int id, CategoryDTO categoryDTO)
        {
            var category = await _dataContext.Categories.FindAsync(id);
            if (category == null) { return StatusCode(404, "category-not-found"); }

            category.Name = categoryDTO.Name;

            await _dataContext.SaveChangesAsync();

            return StatusCode(200, category);
        }
    }
}
