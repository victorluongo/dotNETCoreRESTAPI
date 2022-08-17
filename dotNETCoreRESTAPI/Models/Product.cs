using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotNETCoreRESTAPI.Models
{
    public class Product : Entity
    {
        [DisplayName("Supplier")]
        public Guid SupplierId {get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(200, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 3)]
        public string Name {get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(1000, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 5)]
        public string Description {get; set;}

        public string Image {get; set;}
        
        [NotMapped]
        [DisplayName("Product Image")]
        public IFormFile ImageUpload {get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        public decimal Price {get; set;}

        [DisplayName("Created At")]
        public DateTime Created_At {get; set;}

        public bool Active {get; set;}

        public Supplier Supplier {get;set;}
    }
}