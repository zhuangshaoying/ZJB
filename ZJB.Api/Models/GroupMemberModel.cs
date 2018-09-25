
namespace ZJB.Api.Models
{
    public class GroupMemberModel
    {
        public int Id { get; set; }
        public int OperateUserId { get; set; }
        public int MemberType { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Tel { get; set; }
        public string Portrait { get; set; }
    }
}
