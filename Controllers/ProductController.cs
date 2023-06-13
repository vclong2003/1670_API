using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace _1670_API.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public ProductController(DataContext dataContext)
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
            var products = await query.Select(p => new { p.Id, p.Name, p.Price, p.ThumbnailUrl, p.Author, p.Publisher, p.PublishcationDate, p.Quantity }).ToListAsync();

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
            product.Quantity = productDTO.Quantity ?? product.Quantity;
            product.Author = productDTO.Author ?? product.Author;
            product.Publisher = productDTO.Publisher ?? product.Publisher;
            product.ThumbnailUrl = productDTO.ThumbnailUrl ?? product.ThumbnailUrl;
            product.PublishcationDate = productDTO.PublishcationDate ?? product.PublishcationDate;

            await _dataContext.SaveChangesAsync();

            return StatusCode(200, product);
        }

        [HttpGet("home-products")]
        public async Task<ActionResult> BestSelling()
        {
            ProductHomeDTO productHome = new ProductHomeDTO();
            try
            {
                SqlConnection conn = Conn.Connection();
                SqlCommand cmd = new SqlCommand("PRO_Selling_Products", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var r = new
                    {
                        id = reader.GetValue(0),
                        name = reader.GetValue(1),
                        price = reader.GetValue(2),
                        url = reader.GetValue(3),
                        author = reader.GetValue(4),
                    };
                    productHome.bestSelling.Add(r);
                }
                conn.Close();

                SqlCommand cmd2= new SqlCommand("PRO_Newly_Products", conn);
                cmd2.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    var r = new
                    {
                        id = reader2.GetValue(0),
                        name = reader2.GetValue(1),
                        price = reader2.GetValue(2),
                        url = reader2.GetValue(3),
                        author = reader2.GetValue(4),
                    };
                    productHome.newlyProduct.Add(r);
                }
                conn.Close();

                return StatusCode(200, new { productHome.bestSelling, productHome.newlyProduct });
            }
            catch
            {
                return StatusCode(401, null);
            }

        }
    }
}
