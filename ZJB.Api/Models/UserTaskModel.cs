using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class UserTaskModel
    { 
        #region 字段
        /// <summary>
        /// TaskId
        /// </summary>		
        private int _taskid;
        public int TaskId
        {
            get { return _taskid; }
            set { _taskid = value; }
        }
        /// <summary>
        /// 任务名称
        /// </summary>		
        private string _taskname;
        public string TaskName
        {
            get { return _taskname; }
            set { _taskname = value; }
        }
        /// <summary>
        /// 任务描述
        /// </summary>		
        private string _taskdescription;
        public string TaskDescription
        {
            get { return _taskdescription; }
            set { _taskdescription = value; }
        }
        /// <summary>
        /// 得分值
        /// </summary>		
        private int _points;
        public int Points
        {
            get { return _points; }
            set { _points = value; }
        }
        /// <summary>
        /// 任务类别  1 新手任务 2日常任务 3特殊任务
        /// </summary>		
        private int _type;
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 任务状态 1 开启中 0 关闭中
        /// </summary>		
        private int _state;
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>		
        private DateTime _addtime;
        public DateTime AddTime
        {
            get { return _addtime; }
            set { _addtime = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>		
        private DateTime _begintime;
        public DateTime BeginTime
        {
            get { return _begintime; }
            set { _begintime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>		
        private DateTime _endtime;
        public DateTime EndTime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }
        /// <summary>
        /// 任务Url
        /// </summary>		
        private string _taskUrl;
        public string TaskUrl
        {
            get { return _taskUrl; }
            set { _taskUrl = value; }
        }
        /// <summary>
        /// APP任务Url
        /// </summary>		
        private string _appUrl;
        public string AppUrl
        {
            get { return _appUrl; }
            set { _appUrl = value; }
        } 
        #endregion

        #region 其他表字段
        /// <summary>
        /// -1 尚未做任务 0是做完未领取
        /// </summary>
        public int TaskStatus { get; set; }
        /// <summary>
        /// 特殊任务完成的次数
        /// </summary>
        public int SpecialTaskCount { get; set; }
        #endregion
    }
}
