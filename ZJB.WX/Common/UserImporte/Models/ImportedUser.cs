﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using MongoDB.Bson.Serialization.Attributes;

namespace ZJB.WX.Common.UserImporte.Models
{
    public partial class ImportedUser
    {
        [BsonId]
        public string UserId { get; set; }  //对象网站用户ID
        public string UserName { get; set; } //用户名
        public string Tel { get; set; }  //注册电话
        public string Company { get; set; } //公司名
        public string Store { get; set; } //门店名
        public string Portrait { get; set; } //头像
        public int InviteUid { get; set; } //当前邀请人用户ID
        public string City { get; set; } //城市名
    }
}