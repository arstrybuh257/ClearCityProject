using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClearCity.Models
{
    public class HouseListViewModel
    {
        public IEnumerable<House> Houses { get; set; }
        public SelectList District { get; set; }
        public SelectList Microdistrict { get; set; }
    }
}