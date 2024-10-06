using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections;
using System.Dynamic;
using Microsoft.Data.SqlClient;

namespace Common.UnitOfWork.UnitOfWorkPattern;

public interface IUnitOfWork
{
    bool SaveAndReload<T>(T entity) where T : class;
    int SaveChanges();
    Task<int> SaveChangesAsync();
    DbSet<T> Repository<T>() where T : class;
    void Dispose();

    IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity>> ChangeTracker<TEntity>()
        where TEntity : class;

    DataTable ExecuteStoreProcedure(string storeProcedure, SqlParameter[] data);
    DataTable ExecuteStoreProcedure(string storeProcedure, Hashtable data);
    IList<T> ExecuteStoreProcedure<T>(string storeProcedure, SqlParameter[] data);
    IList<T> ExecuteStoreProcedure<T>(string storeProcedure, Hashtable data);
    DataSet ExecuteStoreProcedureGetMultiTables(string storeProcedure, Hashtable data);
    DataSet ExecuteStoreProcedureGetMultiTables(string storeProcedure, SqlParameter[] data);
    /// <summary>
    /// execute store procedure 
    /// </summary>
    /// <param name="storeProcedure">Store procedure name</param>
    /// <param name="param">Parameters, maybe is dynamic object, expando object</param>
    /// <returns></returns>
    IEnumerable<T> QueryMultiple<T>(string storeProcedure, ExpandoObject param);
    T ExecuteScalarFunction<T>(string functionName, Dictionary<string, object> data);
    IList<T> ExecuteReaderFunction<T>(string functionName, Dictionary<string, object> data);
}