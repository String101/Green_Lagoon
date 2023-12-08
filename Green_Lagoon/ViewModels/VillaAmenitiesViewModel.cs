using Green_Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Green_Lagoon.ViewModels
{
    public class VillaAmenitiesViewModel
    {
        public Amenity Amenity { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
