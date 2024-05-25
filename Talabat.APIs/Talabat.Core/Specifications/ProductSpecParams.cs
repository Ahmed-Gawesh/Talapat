using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecParams   // ال parameters الخاصة ب ال product
    {
        private const int MaxPageSize = 10;
        private int pageSize = 5;
        public int PageSize {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }   
            // لو القيمة اكبر من القيمة الكبري هات القيمة الكبري لو اقل هات القيمة العادي
        }

        public int PageIndex { get; set; } = 1; // Default=1
        public string? sort { get; set; }
        public int? brandid { get; set; }
        public int? typeid { get; set; }

        private string? search { get; set; }
        public string? Search 
        {
            get { return search;}
            set { search = value.ToLower(); }
        }

    }
}
