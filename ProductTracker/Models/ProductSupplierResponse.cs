using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductTracker.Models
{
    [DataContract(Namespace = "http://www.example.com/product")]
    public class ProductSupplierResponse
    {
        [DataMember]
        public long? id { set; get; }
        [DataMember(IsRequired = true)]
        public string itemName { set; get; }
        [DataMember(IsRequired = true)]
        public double price { set; get; }
        [DataMember]
        public int? kCal { set; get; }
        [DataMember]
        public string? url { get; set; }
        [DataMember]
        public int? supplierId { get; set; }
        [DataMember]
        public Supplier supplier { get; set; }

        public ProductSupplierResponse(Product product, Supplier supplier)
        {
            id = product.id;
            itemName = product.itemName;
            price = product.price;
            kCal = product.kCal;
            url = product.url;
            supplierId = supplier.id;
            this.supplier = supplier;
        }
        public ProductSupplierResponse(Product product)
        {
            id = product.id;
            itemName = product.itemName;
            price = product.price;
            kCal = product.kCal;
            url = product.url;
            if (product.supplierId == null)
            {
                supplierId = null;
            }
            else
            {
                supplierId = product.supplierId;
            }
            supplier = null;
        }
        public ProductSupplierResponse() { }

    }
}
