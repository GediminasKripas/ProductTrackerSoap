using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductTracker.Models
{
    [DataContract(Namespace = "http://www.example.com/product")]
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public long id { set; get; }
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

        public Product(ProductSupplierResponse response)
        {

            if(response.id != null)
            {
                id = (long)response.id;
            }

            itemName = response.itemName;
            price = response.price;
            kCal = response.kCal;
            url = response.url;
            supplierId = response.supplier.id;

        }

        public Product() { }

    }
}
