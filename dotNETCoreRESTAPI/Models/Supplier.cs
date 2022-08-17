using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotNETCoreRESTAPI.Models
{
    public class Supplier : Entity
    {
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 3)]
        public string Name {get; set;}
        
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(22, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 14)]
        public string Document {get; set;}
        
        public SupplierType SupplierType {get; set;}
        
        public Address Address {get; set;}
        
        public bool Active {get; set;}

        public IEnumerable<Product> Products {get; set;}
    }
}