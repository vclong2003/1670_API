using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public ProductsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: /api/product/?category={categoryId}&search={searchValue}&skip={skip}&limit={limit}
        // Retrieves products based on optional categoryId and searchValue parameters
        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] int? category = null, [FromQuery] string? search = null, [FromQuery] int? skip = null, [FromQuery] int? limit = null)
        {
            // Create a query to retrieve products
            IQueryable<Product> query = _dataContext.Products;

            // Apply filters if they are provided
            if (category != null) { query = query.Where(p => p.CategoryId == category); }
            if (search != null) { query = query.Where(p => p.Name.Contains(search) || p.Author.Contains(search)); } // Search by name or author
            if (skip != null) { query = query.Skip((int)skip); }
            if (limit != null) { query = query.Take((int)limit); }

            // Execute the query and retrieve products
            var products = await query.Select(p => new { p.Id, p.Name, p.Price, p.ThumbnailUrl, p.Author, p.Publisher, p.PublishcationDate }).ToListAsync();

            return StatusCode(200, products);
        }

        // GET: /api/products/{id}
        // Retrieves a specific product by its id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) { return StatusCode(404, "Product not found!"); }
            return StatusCode(200, product);
        }

        // POST: /api/products
        // Adds a new product with the provided information
        // Body parameters: name, price, description, categoryId, author, publisher, thumbnailUrl, publishcationDate, quantity
        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductDTO productDTO)
        {
            Product newProduct = new()
            {
                Name = productDTO.Name,
                Price = (double)productDTO.Price,
                Description = productDTO.Description,
                CategoryId = productDTO.CategoryId,
                Author = productDTO.Author,
                Publisher = productDTO.Publisher,
                ThumbnailUrl = productDTO.ThumbnailUrl,
                PublishcationDate = (DateTime)productDTO.PublishcationDate,
                Quantity = (int)productDTO.Quantity,

            };

            _dataContext.Products.Add(newProduct);
            await _dataContext.SaveChangesAsync();

            return StatusCode(200, newProduct);
        }

        // PUT: /api/products/{id}
        // Updates an existing product with the provided information
        // Body parameters: name, price, description, categoryId 
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {
            var accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "unauthorized"); }
            if (accountDTO.Role != "MANAGER") { return StatusCode(403, "Forbidden"); }

            var product = await _dataContext.Products.FindAsync(id);
            if (product == null) { return StatusCode(404, "product-not-found"); }

            product.Name = productDTO.Name ?? product.Name; // If productDTO.Name is null, product.Name will not be updated
            product.Price = productDTO.Price ?? product.Price;
            product.Description = productDTO.Description ?? product.Description;
            product.CategoryId = productDTO.CategoryId ?? product.CategoryId;

            await _dataContext.SaveChangesAsync();

            return StatusCode(200, product);
        }
    }
}
