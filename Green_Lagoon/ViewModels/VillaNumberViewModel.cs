using Green_Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Green_Lagoon.ViewModels
{
    public class VillaNumberViewModel
    {
        public VillaNumber VillaNumber { get; set; }
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
