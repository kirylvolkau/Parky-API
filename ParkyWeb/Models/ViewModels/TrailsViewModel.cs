using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkyWeb.Models.ViewModels
{
    public class TrailsViewModel
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set;  }
        public Trail Trail { get; set; }
    }
}