//using _1670_API.Data;
//using _1670_API.Models;
//using _1670_API.Models.DTOs;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace _1670_API.Controllers
//{
//    [Route("api/categories")]
//    [ApiController]
//    public class CategoriesController : ControllerBase
//    {
//        private readonly DataContext _dataContext;
//        public CategoriesController(DataContext dataContext)
//        {
//            _dataContext = dataContext;
//        }

//        [HttpGet]
//        public async Task<ActionResult> Get()
//        {
//            var result = await _dataContext.Categories.Select(c => new { c.Id, c.Name }).ToListAsync();
//            return StatusCode(200, result);
//        }

//        [HttpGet("test/{id}")]
//        public async Task<ActionResult> GetTest(int id)
//        {
//            var result = await _dataContext.Categories.FindAsync(id);
//            return StatusCode(200, result.Books);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<List<Category>>> GetOne(int id)
//        {
//            var result = await _dataContext.Categories.FindAsync(id);

//            return StatusCode(200, result);
//        }

//        [HttpPost]
//        public async Task<ActionResult<List<Category>>> Add(CategoryDTO newCategory)
//        {
//            var category = new Category()
//            {
//                Name = newCategory.Name,
//            };
//            _dataContext.Categories.Add(category);
//            await _dataContext.SaveChangesAsync();
//            var result = await _dataContext.Categories.ToListAsync();
//            return StatusCode(200, result);
//        }

//        [HttpPut("id")]
//        public async Task<ActionResult<List<Category>>> Update(int id, CategoryDTO newCategory)
//        {
//            var current = await _dataContext.Categories.FindAsync(id);
//            if (current is null)
//            {
//                return NotFound();
//            }
//            else
//            {
//                current.Name = newCategory.Name;
//                await _dataContext.SaveChangesAsync();
//                var result = await _dataContext.Categories.ToListAsync();
//                return Ok(result);
//            }
//        }
//    }
//}
