

using System.Collections;
using System.Collections.Generic;
namespace ZJB.Pager
{
    public interface IPagedList : IEnumerable
    {
        int CurrentPageIndex { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
    }
    public interface IPagedList<T> : IEnumerable<T>, IPagedList { }
}
