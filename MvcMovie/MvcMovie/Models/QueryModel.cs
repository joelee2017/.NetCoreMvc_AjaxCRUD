using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class QueryModel
    {
        public QueryModel()
        {
            this.Page = 1;
            this.PageSize = 10;
            this.OrderType = "Asc";
        }

        /// <summary>頁碼</summary>
        public int Page { get; set; }

        /// <summary>回傳筆數</summary>
        public int PageSize { get; set; }

        /// <summary>排序 - 昇降</summary>
        public string OrderType { get; set; }

        /// <summary>排序 - 屬性名稱</summary>
        public string OrderName { get; set; }
    }
}
