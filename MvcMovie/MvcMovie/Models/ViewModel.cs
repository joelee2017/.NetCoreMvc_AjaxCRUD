using X.PagedList;

namespace MvcMovie.Views.Movies
{
    public class ViewModel<T> where T : class
    {
        /// <summary>資料總數</summary>
        public int? Total { get; set; }

        /// <summary>排序 - 屬性名稱</summary>
        public string OrderType { get; set; }

        /// <summary>分頁資料</summary>
        public IPagedList<T> Items { get; set; }
    }
}
