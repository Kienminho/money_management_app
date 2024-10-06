using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Data;
using System.Dynamic;

namespace Infrastructure
{
    public class BaseManager
    {
        #region Database Helper
        /*/// <summary>
        /// This method calls a stored procedure on a SQL Server database using the provided DBContext object
        /// </summary>
        /// <param name="ctx"> DBContext object</param>
        /// <param name="storeProcedure"> The name of the stored procedure</param>
        /// <param name="data">Stored procedure's parameters</param>
        /// <returns>DataTable object.</returns>
        protected DataTable ExecuteStoreProcedure(IDbContext ctx, string storeProcedure, Hashtable data)
        {
            try
            {
                var unitOfWork = new UnitOfWork(ctx);
                return unitOfWork.ExecuteStoreProcedure(storeProcedure, data);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// This method calls a stored procedure on a SQL Server database using the provided DBContext object
        /// </summary>
        /// <param name="ctx">DBContext object</param>
        /// <param name="storeProcedure">The name of the stored procedure</param>
        /// <param name="data">array of SqlParameter objects</param>
        /// <returns>DataTable object.</returns>
        protected DataTable ExecuteStoreProcedure(IDbContext ctx, string storeProcedure, SqlParameter[] data)
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.ExecuteStoreProcedure(storeProcedure, data);
        }
        /// <summary>
        /// This method calls a stored procedure on a SQL Server database using the provided DBContext object
        /// </summary>
        /// <param name="ctx"> DBContext object</param>
        /// <param name="storeProcedure"> The name of the stored procedure</param>
        /// <param name="data">Stored procedure's parameters</param>
        /// <returns> list of any type T..</returns>
        protected IList<T> ExecuteStoreProcedure<T>(IDbContext ctx, string storeProcedure, Hashtable data)
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.ExecuteStoreProcedure<T>(storeProcedure, data);
        }
        /// <summary>
        /// This method calls a stored procedure on a SQL Server database using the provided DBContext object
        /// </summary>
        /// <param name="ctx">DBContext object</param>
        /// <param name="storeProcedure">The name of the stored procedure</param>
        /// <param name="data">array of SqlParameter objects</param>
        /// <returns>list of any type T.</returns>
        protected IList<T> ExecuteStoreProcedure<T>(IDbContext ctx, string storeProcedure, SqlParameter[] data)
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.ExecuteStoreProcedure<T>(storeProcedure, data);
        }
        /// <summary>
        /// This method calls a stored procedure on a SQL Server database using the provided DBContext object
        /// </summary>
        /// <param name="ctx"> DBContext object</param>
        /// <param name="storeProcedure"> The name of the stored procedure</param>
        /// <param name="data">Stored procedure's parameters</param>
        /// <returns>DataSet object.</returns>
        protected DataSet ExecuteStoreProcedureGetMultipleTables(IDbContext ctx, string storeProcedure, Hashtable data)
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.ExecuteStoreProcedureGetMultiTables(storeProcedure, data);
        }

        protected DataSet ExecuteStoreProcedureGetMultipleTables(IDbContext ctx, string storeProcedure, SqlParameter[] data)
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.ExecuteStoreProcedureGetMultiTables(storeProcedure, data);
        }

        protected IEnumerable<T> QueryMultiple<T>(IDbContext ctx, string storeProcedure, object param)
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.QueryMultiple<T>(storeProcedure, TryGetExpandoObject(param));
        }

        protected T ExecuteScalarFunction<T>(IDbContext ctx, string functionName, Dictionary<string, object> data)
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.ExecuteScalarFunction<T>(functionName, data);
        }

        protected IList<T> ExecuteReaderFunction<T>(IDbContext ctx, string functionName, Dictionary<string, object> data)
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.ExecuteReaderFunction<T>(functionName, data);
        }

        protected IEnumerable<T> GetAllObjects<T>(IDbContext ctx, bool isNoTracking = false) where T : class
        {
            var unitOfWork = new UnitOfWork(ctx);
            IEnumerable<T> result = Enumerable.Empty<T>();
            if (isNoTracking)
                result = unitOfWork.GetRepository<T>().AllAsNoTracking();
            else
                result = unitOfWork.GetRepository<T>().All();
            return result;
        }

        protected virtual IQueryable<T> FindObjects<T>(IDbContext ctx, Expression<Func<T, bool>> filter = null, bool isLazyLoadingEnabled = true, string includeProperties = "") where T : class
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.GetRepository<T>(isLazyLoadingEnabled).Find(filter, includeProperties);
        }

        protected T GetObjectById<T>(IDbContext ctx, object id, bool isLazyLoadingEnabled = true) where T : class
        {
            var unitOfWork = new UnitOfWork(ctx);
            return unitOfWork.GetRepository<T>(isLazyLoadingEnabled).GetById(id);
        }

        protected bool AddNewObject<T>(IDbContext ctx, T entity) where T : class
        {
            try
            {
                var unitOfWork = new UnitOfWork(ctx);
                unitOfWork.GetRepository<T>().Add(entity);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        protected bool UpdateExistedObject<T>(IDbContext ctx, T entity) where T : class
        {
            try
            {
                var unitOfWork = new UnitOfWork(ctx);
                unitOfWork.GetRepository<T>().Update(entity);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        protected bool DeleteObject<T>(IDbContext ctx, params object[] id) where T : class
        {
            try
            {
                var unitOfWork = new UnitOfWork(ctx);
                var entity = unitOfWork.GetRepository<T>().GetById(id);
                unitOfWork.GetRepository<T>().Delete(entity);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        protected bool DeleteObject<T>(IDbContext ctx, T entity) where T : class
        {
            try
            {
                var unitOfWork = new UnitOfWork(ctx);
                unitOfWork.GetRepository<T>().Delete(entity);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        protected bool Save<TEntity>(IDbContext ctx, TEntity entity = null) where TEntity : class
        {
            try
            {
                var unitOfWork = new UnitOfWork(ctx);
                return unitOfWork.SaveAndReload(entity);
            }
            catch (Exception ex) { throw ex; }
        }

        protected bool Save(IDbContext ctx)
        {
            try
            {
                var unitOfWork = new UnitOfWork(ctx);
                var result = unitOfWork.Save();
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        protected ExpandoObject TryGetExpandoObject(object param)
        {
            if (param == null)
                return new ExpandoObject();

            if (param is string json)
                return UtilsExtension.JsonToExpandoObject(json);

            if (param is Hashtable hash)
            {
                var paras = hash.OfType<DictionaryEntry>().FirstOrDefault(x => $"{x.Key}" == "1").Value;
                return TryGetExpandoObject(paras);
            }

            var str = Newtonsoft.Json.JsonConvert.SerializeObject(param);
            return UtilsExtension.JsonToExpandoObject(str);
        }

        protected IList<dynamic> GetDynamics(object result)
        {
            if (result is string jsonData && IsJson(jsonData))
            {
                return ((IList)DeserializeObject(jsonData)).Cast<dynamic>().ToList();
            }
            return ((IList)result).Cast<dynamic>().ToList();
        }

        protected dynamic GetDynamic(object result)
        {
            return GetDynamics(result).FirstOrDefault();
        }

        #region Json-Expando Object

        /// <summary>
        /// Deserialize multiple object
        /// </summary>
        /// <returns></returns>
        private List<ExpandoObject> DeserializeMultiple(string jsonData)
        {
            var converters = new ExpandoObjectConverter();
            return JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonData, converters);
        }

        /// <summary>
        /// Deserialize single object
        /// </summary>
        /// <returns></returns>
        private ExpandoObject DeserializeSingle(string jsonData)
        {
            var converters = new ExpandoObjectConverter();
            return JsonConvert.DeserializeObject<ExpandoObject>(jsonData, converters);
        }

        /// <summary>
        /// Deserialize object 
        /// </summary>
        /// <returns></returns>
        public dynamic DeserializeObject(string jsonData)
        {
            return jsonData.StartsWith("[") ? (dynamic)DeserializeMultiple(jsonData) : DeserializeSingle(jsonData);
        }

        /// <summary>
        /// Poco-serialize dynamic
        /// </summary>
        /// <param name="sender">sender</param>
        /// <returns></returns>
        public dynamic DeserializeDynamicObject(object sender)
        {
            var json = JsonConvert.SerializeObject(sender);
            return DeserializeObject(json);
        }

        /// <summary>
        /// Check string is json or not. 
        /// </summary>
        /// <returns></returns>
        private bool IsJson(string jsonData)
        {
            return jsonData.StartsWith("[") || jsonData.StartsWith("{");
        }

        #endregion
        */
        #endregion
    }
}