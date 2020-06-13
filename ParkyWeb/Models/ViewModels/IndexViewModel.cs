using System.Collections.Generic;

namespace ParkyWeb.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Trail> Trails { get; set; }
        public IEnumerable<NationalPark> Parks { get; set; }
    }
}