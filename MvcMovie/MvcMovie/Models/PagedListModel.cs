using X.PagedList;

namespace MvcMovie.Models
{
    public class PagedListModel<T> where T : class
    {
        public int? Total;

        public string OrderType;

        public IPagedList<T> Items;
    }
}
