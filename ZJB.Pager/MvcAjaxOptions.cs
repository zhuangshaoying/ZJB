namespace ZJB.Pager
{
    public class MvcAjaxOptions : System.Web.Mvc.Ajax.AjaxOptions
    {
        public bool EnablePartialLoading { get; set; }
        public string DataFormId { get; set; }
    }
}