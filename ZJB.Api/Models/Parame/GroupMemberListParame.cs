
namespace ZJB.Api.Models
{
    public  class GroupMemberListParame
    {
        public int GroupId { get; set; }
        public int PageSize { get; set; }
        public int LastId { get; set; }
        /// <summary>
        /// 0 不在群组里 1 正常 2 申请加入状态
        /// </summary>
        public int Status { get; set; }
        public string KeyWord { get; set; }
    }
}
