
using System.Collections.Generic;

namespace ZJB.Api.Models
{
    /// <summary>
    /// 字典
    /// </summary>
    public class HDictionary
    {

        private static HDictionary _instance = null;
        public static HDictionary Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HDictionary();
                }
                return _instance;
            }
        }

        private SortedList<string, string> mSex = new SortedList<string, string>(); //性别
        private SortedList<string, string> mUserStatus = new SortedList<string, string>(); //用户状态
        private SortedList<string, string> mBuildingType = new SortedList<string, string>(); //房屋类型
        private SortedList<string, string> mTradeType = new SortedList<string, string>(); //发布类型
        private SortedList<string, string> mETradeType = new SortedList<string, string>(); //发布类型
        private SortedList<string, string> mPostTyp = new SortedList<string, string>(); //房源发布操作类型
        
        /// <summary>
        /// 构造函数
        /// </summary>
        private HDictionary()
        {
           
            #region 性别
            this.mSex.Add("1", "男");
            this.mSex.Add("2", "女");
            #endregion
            #region 用户状态
            this.mUserStatus.Add("0", "待审");
            this.mUserStatus.Add("1", "已审");
            this.mUserStatus.Add("2", "冻结");
            #endregion
            #region 房屋类型
            this.mBuildingType.Add("1", "住宅");
            this.mBuildingType.Add("2", "别墅");
            this.mBuildingType.Add("3", "商铺");
            this.mBuildingType.Add("4", "写字楼");
            this.mBuildingType.Add("5", "厂房");
            #endregion
            #region 发布类型
            this.mTradeType.Add("1", "出售");
            this.mTradeType.Add("2", "求购");
            this.mTradeType.Add("3", "出租");
            this.mTradeType.Add("4", "求租");
           
            #endregion
            #region 发布类型
            this.mETradeType.Add("1", "esf");
            this.mETradeType.Add("2", "qg");
            this.mETradeType.Add("3", "zf");
            this.mETradeType.Add("4", "qz");

            #endregion
            #region 房源发布操作类型
            this.mPostTyp.Add("0", "先删后发");
            this.mPostTyp.Add("1", "发布");
            this.mPostTyp.Add("2", "先删后发");
            this.mPostTyp.Add("3", "不发");

            #endregion
        }
   
        #region 性别
        /// <summary>
        /// 性别
        /// </summary>
        /// <param name="key">数字</param>
        /// <returns>返回相应的文字</returns>
        public string Sex(string key)
        {
            if (!string.IsNullOrEmpty(key) && this.mSex.ContainsKey(key))
                return this.mSex[key].ToString();
            else
                return "";
        }
        #endregion

        #region 用户状态
        /// <summary>
        /// 用户状态
        /// </summary>
        /// <param name="key">数字</param>
        /// <returns>返回相应的文字</returns>
        public string UserStatus(string key)
        {
            if (!string.IsNullOrEmpty(key) && this.mUserStatus.ContainsKey(key))
                return this.mUserStatus[key].ToString();
            else
                return "";
        }
        #endregion

        #region 房屋类型
        /// <summary>
        /// 房屋类型
        /// </summary>
        /// <param name="key">数字</param>
        /// <returns>返回相应的文字</returns>
        public string BuildingType(string key)
        {
            if (!string.IsNullOrEmpty(key) && this.mBuildingType.ContainsKey(key))
                return this.mBuildingType[key].ToString();
            else
                return "";
        }
        #endregion

        #region 发布类型
        /// <summary>
        /// 发布类型
        /// </summary>
        /// <param name="key">数字</param>
        /// <returns>返回相应的文字</returns>
        public string TradeType(string key)
        {
            if (!string.IsNullOrEmpty(key) && this.mTradeType.ContainsKey(key))
                return this.mTradeType[key].ToString();
            else
                return "";
        }
        #endregion

        #region 发布类型
        /// <summary>
        /// 发布类型
        /// </summary>
        /// <param name="key">数字</param>
        /// <returns>返回相应的文字</returns>
        public string ETradeType(string key)
        {
            if (!string.IsNullOrEmpty(key) && this.mETradeType.ContainsKey(key))
                return this.mETradeType[key].ToString();
            else
                return "";
        }
        #endregion
        #region 房源发布操作类型
        /// <summary>
        /// 房源发布操作类型
        /// </summary>
        /// <param name="key">数字</param>
        /// <returns>返回相应的文字</returns>
        public string PostTyp(string key)
        {
            if (!string.IsNullOrEmpty(key) && this.mPostTyp.ContainsKey(key))
                return this.mPostTyp[key].ToString();
            else
                return "";
        }
        #endregion
    }
}
