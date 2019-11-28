using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClearCity.Models
{
    public class MicrodistrictListViewModel
    {
        public IEnumerable<Microdistrict> Microdistricts { get; set; }
        public SelectList District { get; set; }
    }
}