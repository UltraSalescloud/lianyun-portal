using Lianyun.UST.Infrastructure.Logging;
using Lianyun.UST.Infrastructure.Utility;
using Lianyun.UST.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Lianyun.UST.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class,new()
    {
        private DbContext _db;

        protected DbContext DB
        {
            get
            {
                return _db ;//?? (new Lianyun_DSPContext());
            }
        }

        private readonly IDbSet<T> dbSet;

        protected IDbSet<T> DbSet
        {
            get
            {
                return dbSet == null ? _db.Set<T>() : dbSet;
            }
        }

        private ILogger _logger;

        public BaseRepository(DbContext db,ILogger logger)
        {
            _db = db;
            dbSet = DB.Set<T>();
            _logger = logger;
        }

        public virtual bool Exist(T model)
        {
            return dbSet.Contains(model);
        }

        public virtual T Find(T model)
        {
            return dbSet.Find(model);
        }

        public virtual int Add(T model)
        {
            dbSet.Add(model);
            int resultcount = 0;
            try
            {
               // _db.Configuration.ValidateOnSaveEnabled = false;
                resultcount = _db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
              //  _db.Configuration.ValidateOnSaveEnabled = true;
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
        }

        private void CatchDbEntityValidationExceptionLog(Exception ee)
        {
            string result = "";
            if (ee is DbEntityValidationException)
            {
                DbEntityValidationException dbEx = ee as DbEntityValidationException;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        result += string.Format("Property:{0},Error:{1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                        //result += "{";
                        //result += string.Format(@"'Property':'{0}','Error':'{1}'", validationError.PropertyName.ToString(), validationError.ErrorMessage.ToString());
                        //result += "},";
                    }
                }
            }
            else
            {
                result = ee.ToString();
            }

            _logger.Error(result);
        }


        public virtual int Add(List<T> modelList)
        {
            modelList.ForEach(u =>
            {
                dbSet.Add(u);
            });
            int resultcount = 0;
            try 
            {
                resultcount=_db.SaveChanges();
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;
        }

        public virtual int Del(T model)
        {
            dbSet.Attach(model);
            dbSet.Remove(model);
            int resultcount = 0;
            try
            {
                resultcount = _db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;
        }

        public virtual int DelBy(Expression<Func<T, bool>> delWhere)
        {

            //3.1查询要删除的数据
            List<T> listDeleting = dbSet.Where(delWhere).ToList();
            //3.2将要删除的数据 用删除方法添加到 EF 容器中
            listDeleting.ForEach(u =>
            {
                dbSet.Attach(u);//先附加到 EF容器
                dbSet.Remove(u);//标识为 删除 状态
            });
            //3.3一次性 生成sql语句到数据库执行删除
            int resultcount = 0;
            try
            {
                resultcount = _db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;
        }

        public virtual int Modify(T model)
        {
            dbSet.Attach(model);
            DB.Entry(model).State = EntityState.Modified;

            int resultcount = 0;
            try
            {
                resultcount = _db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;
        }

        public virtual int Modify(T model,long id=0)
        {
            try
            {
                dbSet.Attach(model);
                DB.Entry(model).State = EntityState.Modified;
            }
            catch (InvalidOperationException ex)
            {
                T old = FindById(id);
                DB.Entry(old).CurrentValues.SetValues(model);
            }
            int resultcount = 0;
            try
            {
                resultcount = _db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;
        }

        public virtual int Modify(T model, params string[] proNames)
        {
            //4.1将 对象 添加到 EF中
            DbEntityEntry entry = DB.Entry(model);
            //4.2先设置 对象的包装 状态为 Unchanged
            entry.State = EntityState.Unchanged;
            //4.3循环 被修改的属性名 数组
            foreach (string proName in proNames)
            {
                //4.4将每个 被修改的属性的状态 设置为已修改状态;后面生成update语句时，就只为已修改的属性 更新
                entry.Property(proName).IsModified = true;
            }
            //4.4一次性 生成sql语句到数据库执行
            int resultcount = 0;
            try
            {
                resultcount = _db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;
        }

        public virtual int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames)
        {
            //4.1查询要修改的数据
            List<T> listModifing = dbSet.Where(whereLambda).ToList();

            //获取 实体类 类型对象
            Type t = typeof(T); // model.GetType();
            //获取 实体类 所有的 公有属性
            List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //创建 实体属性 字典集合
            Dictionary<string, PropertyInfo> dictPros = new Dictionary<string, PropertyInfo>();
            //将 实体属性 中要修改的属性名 添加到 字典集合中 键：属性名  值：属性对象
            proInfos.ForEach(p =>
            {
                if (modifiedProNames.Contains(p.Name))
                {
                    dictPros.Add(p.Name, p);
                }
            });

            //4.3循环 要修改的属性名
            foreach (string proName in modifiedProNames)
            {
                //判断 要修改的属性名是否在 实体类的属性集合中存在
                if (dictPros.ContainsKey(proName))
                {
                    //如果存在，则取出要修改的 属性对象
                    PropertyInfo proInfo = dictPros[proName];
                    //取出 要修改的值
                    object newValue = proInfo.GetValue(model, null); //object newValue = model.uName;

                    //4.4批量设置 要修改 对象的 属性
                    foreach (T usrO in listModifing)
                    {
                        //为 要修改的对象 的 要修改的属性 设置新的值
                        proInfo.SetValue(usrO, newValue, null); //usrO.uName = newValue;
                    }
                }
            }
            //4.4一次性 生成sql语句到数据库执行
            int resultcount = 0;
            try
            {
                resultcount = _db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;
        }


        public virtual List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return dbSet.Where(whereLambda).ToList();
        }

        public virtual List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool orderDesc = false)
        {
            if (orderDesc)
            {
                return dbSet.Where(whereLambda).OrderByDescending(orderLambda).ToList();
            }
            else
            {
                return dbSet.Where(whereLambda).OrderBy(orderLambda).ToList();
            }

        }

        public virtual List<T> GetPagedList<TKey>(ref Paging<T> paging, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool orderDesc = false)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (orderDesc)
            {
                paging.TotalCount = dbSet.Where(whereLambda).OrderByDescending(orderBy).ToList().Count;
                return dbSet.Where(whereLambda).OrderByDescending(orderBy).Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize).ToList();
            }
            else
            {
                paging.TotalCount = dbSet.Where(whereLambda).OrderBy(orderBy).ToList().Count;
                return dbSet.Where(whereLambda).OrderBy(orderBy).Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize).ToList();
            }

        }

        public T FindById(long id)
        {
            return dbSet.Find(id);
        }

        public T FindById(string id)
        {
            return dbSet.Find(id);
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).FirstOrDefault<T>();
        }

        public void Dispose()
        {
            if (_db != null)
                _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual int Add_All(List<T> modelList)
        {
            int resultcount = 1;
            try
            {
                BulkInsertAll(modelList);
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
            }
            return resultcount;
        }
        public virtual int Add_All(DataTable table)
        {
            int resultcount = 1;
            try
            {
                BulkInsertAll(table);
            }
            catch (Exception dbEx)
            {
                resultcount = -1;
                CatchDbEntityValidationExceptionLog(dbEx);
                throw dbEx;
            }
            return resultcount;
        }

        void BulkInsertAll(DataTable table)
        {
            string cs = _db.Database.Connection.ConnectionString;

            Type t = typeof(T);

            try
            {
                using (SqlBulkCopy sqlRevdBulkCopy = new SqlBulkCopy(cs))//引用SqlBulkCopy  
                {
                    sqlRevdBulkCopy.DestinationTableName = string.Format("[{0}]", t.Name);//数据库中对应的表名  

                    sqlRevdBulkCopy.NotifyAfter = table.Rows.Count;//有几行数据  

                    for (int i = 0, j = table.Columns.Count; i < j; i++)
                    {
                        string name = table.Columns[i].ToString();
                        sqlRevdBulkCopy.ColumnMappings.Add(name, name);
                    }

                    sqlRevdBulkCopy.WriteToServer(table);//数据导入数据库  

                    sqlRevdBulkCopy.Close();//关闭连接  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        void BulkInsertAll(IEnumerable<T> entities)
        {
            entities = entities.ToArray();

            string cs = _db.Database.Connection.ConnectionString;
            var conn = new SqlConnection(cs);
            conn.Open();

            Type t = typeof(T);

            var bulkCopy = new SqlBulkCopy(conn)
            {
                DestinationTableName = string.Format("[{0}]", t.Name)
            };

            var properties = t.GetProperties().Where(EventTypeFilter).ToArray();
            var table = new DataTable();

            foreach (var property in properties)
            {
                Type propertyType = property.PropertyType;
                if (propertyType.IsGenericType &&
                    propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                table.Columns.Add(new DataColumn(property.Name, propertyType));
            }

            foreach (var entity in entities)
            {
                table.Rows.Add(properties.Select(
                  property => GetPropertyValue(
                  property.GetValue(entity, null))).ToArray());
            }

            bulkCopy.WriteToServer(table);
            conn.Close();
            conn.Dispose();
        }
        private bool EventTypeFilter(System.Reflection.PropertyInfo p)
        {
            var attribute = Attribute.GetCustomAttribute(p,
                typeof(AssociationAttribute)) as AssociationAttribute;

            if (attribute == null) return true;
            if (attribute.IsForeignKey == false) return true;

            return false;
        }
        private object GetPropertyValue(object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }
    }
}
