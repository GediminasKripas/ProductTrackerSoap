using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductTracker.Services
{
    public class SoapService : ISoapService
    {
        private readonly ProductContext _context;
        private readonly string contactsUrl = "http://contacts:5000/contacts/";

        public SoapService()
        {
            var contextOptions = new DbContextOptionsBuilder<ProductContext>().UseInMemoryDatabase("Products").Options;
            _context = new ProductContext(contextOptions);
          

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
        [XmlInclude(typeof(ProductSupplierResponse))]
        public async Task<List<ProductSupplierResponse>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            List<ProductSupplierResponse> productsResponse = new List<ProductSupplierResponse>();

            foreach (var item in products)
            {

                if (item.supplierId == null)
                {
                    productsResponse.Add(new ProductSupplierResponse(item));
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

        public async Task<Product> GetProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);

            return product;
        }

        public async Task<dynamic> GetProductSuplier(long id)
        {
            ProductSupplierResponse psresponse = null;
            Supplier supplier = null;
            Product product = await _context.Products.FindAsync(id);

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

        public async Task<SoapResponse<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            SoapResponse<Product> response = new SoapResponse<Product>();

            response.obj = product;
            response.code = 201;

            return response;
            //return CreatedAtAction(nameof(GetProduct), new { id = product.id }, product);
        }

        public async Task<SoapResponse<ProductSupplierResponse>> PostProductContacts(ProductSupplierResponse body)
        {
            SoapResponse<ProductSupplierResponse> soapresponse = new SoapResponse<ProductSupplierResponse>();
            if (body.supplier == null)
            {
                soapresponse.obj = null;
                soapresponse.code = 404;
                return soapresponse;
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
                soapresponse.obj = null;
                soapresponse.code = 503;
                return soapresponse;
            }

            Product product = new Product(body);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            if (body.id == null)
            {
                body.id = product.id;
            }
            if (body.supplierId == null)
            {
                body.supplierId = supplier.id;
            }
            soapresponse.obj = new ProductSupplierResponse(product,supplier);
            soapresponse.code = 201;
            return soapresponse;
        }

        public async Task<SoapResponse<Product>> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            SoapResponse<Product> soapresponse = new SoapResponse<Product>();
            if (product == null)
            {
                soapresponse.obj = null;
                soapresponse.code = 404;
                return soapresponse;
            }

            if (product.supplierId != null)
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
                    soapresponse.obj = null;
                    soapresponse.code = 503;
                    return soapresponse;
                }

            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            soapresponse.obj = null;
            soapresponse.code = 204;
            return soapresponse;

        }

        public async Task<SoapResponse<Product>> DeleteProductContacts(long id)
        {
            var product = await _context.Products.FindAsync(id);
            SoapResponse<Product> soapresponse = new SoapResponse<Product>();

            if (product == null)
            {
                soapresponse.obj = null;
                soapresponse.code = 400;
                return soapresponse;
            }

            if (product.supplierId == null)
            {
                soapresponse.obj = null;
                soapresponse.code = 400;
                return soapresponse;
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
                soapresponse.obj = null;
                soapresponse.code = 503;
                return soapresponse;
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
                    soapresponse.obj = null;
                    soapresponse.code = 404;
                    return soapresponse;
                }
                else
                {
                    throw;
                }
            }

            soapresponse.obj = null;
            soapresponse.code = 204;
            return soapresponse;
        }

        public async Task<SoapResponse<Product>> PutProduct(long id, Product product)
        {
            SoapResponse<Product> soapresponse = new SoapResponse<Product>();
            if (product.id == 0)
            {
                product.id = id;
            }

            if (id != product.id)
            {
                soapresponse.obj = null;
                soapresponse.code = 400;
                return soapresponse;
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
                    soapresponse.obj = null;
                    soapresponse.code = 404;
                    return soapresponse;
                }
                else
                {
                    throw;
                }
            }

            soapresponse.obj = product;
            soapresponse.code = 200;
            return soapresponse;

        }

        public async Task<SoapResponse<SupplierPut>> PutProductContacts(long id, SupplierPut supplier)
        {
            SoapResponse<SupplierPut> soapresponse = new SoapResponse<SupplierPut>();
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                soapresponse.obj = null;
                soapresponse.code = 404;
                return soapresponse;
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
                soapresponse.obj = null;
                soapresponse.code = 503;
                return soapresponse;
            }
            soapresponse.obj = supplier;
            soapresponse.code = 200;
            return soapresponse;
        }


        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.id == id);
        }

    }
}
