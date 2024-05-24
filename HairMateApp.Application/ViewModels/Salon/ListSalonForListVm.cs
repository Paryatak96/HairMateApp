using HairMateApp.Application.ViewModels.Salon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairMateApp.Application.ViewModels.Salon
{
    public class ListSalonForListVm
    {
        public List<SalonForListVm> Salons { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public object Salon { get; internal set; }
    }
}
