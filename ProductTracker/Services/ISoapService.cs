using ProductTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductTracker.Services
{
    //[ServiceContract(Name = "ProductsTracker", Namespace = "http://www.example.com/products")]
    [ServiceContract]
    interface ISoapService
    {
        [OperationContract]
        Task<List<ProductSupplierResponse>> GetProducts();

        [OperationContract]
        Task<Product> GetProduct(long id);

        [OperationContract]
        Task<dynamic> GetProductSuplier(long id);

        [OperationContract]
        Task<SoapResponse<Product>> PostProduct(Product product);

        [OperationContract]
        Task<SoapResponse<Product>> DeleteProduct(long id);

        [OperationContract]
        Task<SoapResponse<Product>> DeleteProductContacts(long id);
        [OperationContract]
        Task<SoapResponse<ProductSupplierResponse>> PostProductContacts(ProductSupplierResponse body);
        [OperationContract]
        Task<SoapResponse<Product>> PutProduct(long id, Product product);
        [OperationContract]
        Task<SoapResponse<SupplierPut>> PutProductContacts(long id, SupplierPut supplier);
    }
}
