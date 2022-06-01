using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductTracker.Models;

namespace ProductTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly string contactsUrl = "http://contacts:5000/contacts/";

        public ProductsController(ProductContext context)
        {
            _context = context;

            if (!_context.Products.Any())
            {
                _context.Products.Add(new Product
                { 
                    itemName = "Coca-cola",
                    price = 0.89,
                    kCal = 201,
                    supplierId = 12345
                });

                _context.Products.Add(new Product
                {
                    itemName = "Pepsi",
                    price = 0.89,
                    url = "https://www.pepsi.com/en-us/uploads/images/twil-can.png",
                    supplierId = 11234
                });

                _context.Products.Add(new Product
                {
                    itemName = "Fanta",
                    price = 0.89
                });

                _context.SaveChanges();
            }

        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            List<Object> productsResponse = new List<object>();

            foreach(var item in products)
            {

                if(item.supplierId == null)
                {
                    productsResponse.Add(item);
                }
                else
                {

                    Supplier supplier = null;

                    try
                    {
                        using (var httpClient = new HttpClient())
                        {
                            using (var response = await httpClient.GetAsync(contactsUrl + item.supplierId))
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                supplier = JsonConvert.DeserializeObject<Supplier>(apiResponse);
                            }
                        }

                        ProductSupplierResponse psresponse = new ProductSupplierResponse(item, supplier);
                        productsResponse.Add(psresponse);

                    }
                    catch (Exception exception)
                    {
                        System.Diagnostics.Debug.WriteLine(exception);
                        productsResponse.Add(new ProductSupplierResponse(item));
                    }

                }

            }

            return productsResponse;
        }



        // GET: api/Products/{id}/contacts
        [HttpGet("{id}/contacts")]
        public async Task<ActionResult<dynamic>> GetProductSuplier(long id)
        {
            ProductSupplierResponse psresponse = null;
            Supplier supplier = null;
            Product product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return BadRequest("Product with this id doesnt exist!");
            }

            if (product.supplierId == null)
            {
                return BadRequest("Product doesnt have supplier");
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(contactsUrl + product.supplierId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        supplier = JsonConvert.DeserializeObject<Supplier>(apiResponse);
                    }
                }

                psresponse = new ProductSupplierResponse(product, supplier);

            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
                return product;
            }

            return psresponse;

        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        //
        //
        //
        // : api/Products/5

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutProduct(long id, Product product)
        {

            if (product.id == 0)
            {
                product.id = id;
            }

            if (id != product.id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return product;
        }
        //Task<ActionResult<Supplier>>
        [HttpPut("{id}/contacts")]
        public async Task<ActionResult<Supplier>> PutProductContacts(long id, SupplierPut supplier)
        {

            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound("Product doesnt exist!");
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(supplier);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync(contactsUrl + product.supplierId.ToString(), httpContent))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }


                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
                return StatusCode(503);
            }
            return StatusCode(200, supplier);
        }

        // POST: api/Products

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.id }, product);
        }

        [HttpPost("contacts")]
        public async Task<ActionResult<ProductSupplierResponse>> PostProductContacts(ProductSupplierResponse body)
        {

            if (body.supplier == null)
            {
                return BadRequest("Product doesnt have supplier");
            }

            Supplier supplier = body.supplier;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string json = JsonConvert.SerializeObject(supplier);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(contactsUrl, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
                return StatusCode(503);
            }

            Product product = new Product(body);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            if (body.id == null) {
                body.id = product.id;
            }
            if(body.supplierId == null)
            {
                body.supplierId = supplier.id;
            }
            return StatusCode(201, body);
        }
        

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            if(product.supplierId != null)
            {

                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.DeleteAsync(contactsUrl + product.supplierId))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }

                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                    return StatusCode(503);
                }

            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}/contacts")]
        public async Task<ActionResult<Product>> DeleteProductContacts(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return BadRequest("Product with this id doesnt exist!");
            }

            if (product.supplierId == null)
            {
                return BadRequest("This product doesnt have supplier contacts!");
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(contactsUrl + product.supplierId))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }

            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
                return StatusCode(503);
            }

            product.supplierId = null;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchProduct(long id, [FromBody] JsonPatchDocument<Product> patchProduct)
        {

            var fromDb = _context.Products.FirstOrDefault(x => x.id == id);
            patchProduct.ApplyTo(fromDb, ModelState);

            var isValid = TryValidateModel(fromDb);

            if (!isValid)
            {
                return BadRequest();
            }

            _context.Update(fromDb);
            _context.SaveChanges();


            return Ok(fromDb);
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.id == id);
        }
    }
}
