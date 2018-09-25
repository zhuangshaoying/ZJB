// -------------------------------------------------------
// 文 件 名：EntitiesFunction.cs
// 版    本：ver 6.00.0000
// 功能说明： 实体构造帮助类
// 注意事项：
// 
// 更新记录：
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.ELinq;
using System.Data.Query;
using System.Reflection;
using System.Data.Objects.DataClasses;
using System.Data.Common;
using ZJB.Core.Utilities;
namespace ZJB.Core.Data
{

    /// <summary>
    /// 实体构造帮助类
    /// </summary>
    public static class EntitiesHelper
    {

        /// <summary>
        /// 扩展查询[有值返回第一个对象没有返回对象NULL]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objObjectQuery"></param>
        /// <returns></returns>
        public static T FirstTop<T>(this  IEnumerable<T> objObjectQuery) where T : System.Data.Objects.DataClasses.EntityObject
        {

            List<T> objObjectQueryList = objObjectQuery.ToList<T>();
            if (objObjectQueryList.Count > 0)
            {
                return objObjectQueryList.First<T>();
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 获取分页获取数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="objObjectQuery">ObjectQuery数据源</param>
        /// <param name="condition">条件</param>
        /// <param name="sortExpression">排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="recordCount">返回总数</param>
        /// <returns></returns>
        public static ObjectQuery<T> GetPage<T>(this ObjectQuery<T> objObjectQuery, string condition, string sortExpression, int pageSize, int currentPageIndex, out int recordCount) where T : System.Data.Objects.DataClasses.EntityObject
        {


            recordCount = 0;
            if (condition.IsNoNull())
            {
                objObjectQuery = objObjectQuery.Where(condition);
            }
            recordCount = objObjectQuery.Count();


            return objObjectQuery.Skip(sortExpression, (pageSize * currentPageIndex).ToString()).Top(pageSize.ToString());
        }
        /// <summary>
        /// 获取分页获取数据
        /// </summary>
        /// <param name="objObjectQuery">ObjectQuery数据源</param>
        /// <param name="condition">条件</param>
        /// <param name="sortExpression">排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="recordCount">返回总数</param>
        /// <returns></returns>
        public static ObjectQuery<DbDataRecord> GetPage(this ObjectQuery<DbDataRecord> objObjectQuery, string condition, string sortExpression, int pageSize, int currentPageIndex, out int recordCount)
        {


            recordCount = 0;
            if (condition.IsNoNull())
            {
                objObjectQuery = objObjectQuery.Where(condition);
            }
            recordCount = objObjectQuery.Count();

            if (sortExpression.IsNoNull())
            {
                objObjectQuery = objObjectQuery.OrderBy(sortExpression);
            }

            return objObjectQuery.Skip(sortExpression, (pageSize * currentPageIndex).ToString()).Top(pageSize.ToString());
        }



        /// <summary>
        /// 扩展方法删除数据
        /// </summary>
        /// <param name="objObjectQuery">查询对象</param>
        private static void DeleteData(this ObjectQuery objObjectQuery)
        {
            ObjectContext objContext = objObjectQuery.Context;
            foreach (object objTable in objObjectQuery)
            {
                objContext.DeleteObject(objTable);

            }
            objContext.SaveChanges();

        }
        /// <summary>
        ///删除数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="objObjectQuery">查询对象</param>
        /// <param name="condition">条件</param>
        public static void DeleteData<T>(this ObjectQuery<T> objObjectQuery, string condition) where T : System.Data.Objects.DataClasses.EntityObject
        {
            objObjectQuery.Where(condition).DeleteData();

        }
        /// <summary>
        /// 删除数据根据主健
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="objObjectQuery">查询对象</param>
        /// <param name="IdString">主键串[,隔开]</param>
        /// <returns></returns>
        public static void DeleteDataPrimaryKey<T>(this ObjectQuery<T> objObjectQuery, string IdString) where T : System.Data.Objects.DataClasses.EntityObject
        {
            PropertyInfo objPropertyInfo = GetEntityPrimaryKey<T>();
            string condition = "it." + objPropertyInfo.Name + " in  {" + IdString + "}";
            if (objPropertyInfo.PropertyType == typeof(Guid))
            {
                condition = "it." + objPropertyInfo.Name + " in {" + IdString.FormantEntityGuidID() + "}";
            }
            else if (objPropertyInfo.PropertyType == typeof(string))
            {
                condition = "it." + objPropertyInfo.Name + " in {" + IdString.FormantStringID() + "}";
            }

            objObjectQuery.Where(condition).DeleteData();

        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="objContext">ObjectContext对象</param>
        /// <param name="condition">条件</param>
        public static void DeleteData<T>(this ObjectContext objContext, string condition) where T : System.Data.Objects.DataClasses.EntityObject
        {
            objContext.CreateQuery<T>("[" + typeof(T).Name + "]").Where(condition).DeleteData();

        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="objContext">ObjectContext对象</param>
        /// <param name="IdString">主健值字符串</param>
        public static void DeleteDataPrimaryKey<T>(this ObjectContext objContext, string IdString) where T : System.Data.Objects.DataClasses.EntityObject
        {
            PropertyInfo objPropertyInfo = GetEntityPrimaryKey<T>();
            string condition = "it." + objPropertyInfo.Name + " in  {" + IdString + "}";
            if (objPropertyInfo.PropertyType == typeof(Guid))
            {
                condition = "it." + objPropertyInfo.Name + " in {" + IdString.FormantEntityGuidID() + "}";
            }
            else if (objPropertyInfo.PropertyType == typeof(string))
            {
                condition = "it." + objPropertyInfo.Name + " in {" + IdString.FormantStringID() + "}";
            }
            objContext.DeleteData<T>(condition);

        }

        /// <summary>
        /// 获取对象连接
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="econString">连接串</typeparam>
        /// <returns></returns>
        public static EntityConnection GetEntityConnection<T>(string DbConnectionString) where T : System.Data.Objects.ObjectContext
        {
            string econString = @"metadata=res://*/Entity.{0}.csdl|res://*/Entity.{0}.ssdl|res://*/Entity.{0}.msl;provider=System.Data.SqlClient;provider connection string=""{1};MultipleActiveResultSets=True;App=EntityFramework""";
            string EntityModule = typeof(T).Name.Replace("Entities", "Model");
            econString = string.Format(econString, EntityModule, DbConnectionString);
            EntityConnection objEntityConnection = new EntityConnection(econString);
            return objEntityConnection;
        }

        /// <summary>
        /// 获取对象主健值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns></returns>
        private static PropertyInfo GetEntityPrimaryKey<T>() where T : System.Data.Objects.DataClasses.EntityObject
        {
            Type type = typeof(T);
            PropertyInfo[] objPropertyInfoList = type.GetProperties();
            foreach (PropertyInfo objPropertyInfo in objPropertyInfoList)
            {

                foreach (Attribute attr in objPropertyInfo.GetCustomAttributes(typeof(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), false))
                {

                    EdmScalarPropertyAttribute objEdmScalarPropertyAttribute = attr as EdmScalarPropertyAttribute;
                    if (objEdmScalarPropertyAttribute != null)
                    {
                        if (objEdmScalarPropertyAttribute.EntityKeyProperty)
                        {
                            return objPropertyInfo;
                        }
                    }
                }
            }
            return null;

        }

    }

}
