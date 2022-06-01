using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ProductTracker.Models
{
    [DataContract(Namespace = "http://www.example.com/product")]
    public class SupplierPut
    {
        //public int id { get; set; }
        [DataMember(IsRequired = true)]
        public string surname { get; set; }
        [DataMember(IsRequired = true)]
        public string name { get; set; }
        [DataMember(IsRequired = true)]
        public string number { get; set; }
        [DataMember(IsRequired = true)]
        public string email { get; set; }

        public SupplierPut(ProductSupplierResponse response)
        {
            Supplier supplier = response.supplier;
            //id = supplier.id;
            surname = supplier.surname;
            name = supplier.surname;
            number = supplier.number;
            email = supplier.email;
        }

        public SupplierPut() { }
    }
}
