using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotNETCoreRESTAPI.Models
{
    public class Address : Entity
    {
        public Guid SupplierId {get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(200, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 3)]
        [DisplayName("Street Name")]
        public string StreetName {get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(15, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 3)]
        public string Number {get; set;}
        
        public string Complemment {get; set;}
        
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(8, ErrorMessage = "{0} must be {1}")]
        public string ZipCode {get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 3)]
        public string City {get; set;}
        
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(2, ErrorMessage = "{0} must be {1}")]        
        public string State {get; set;}

        public Supplier Supplier {get; set;}
    }
}