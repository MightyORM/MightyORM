using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using Mighty.Plugins;
using Mighty.Mapping;
using Mighty.Profiling;
using Mighty.Validation;

namespace Mighty.Interfaces
{
    abstract public partial class MightyOrmAbstractInterface<T> where T : class, new()
    {
        // 'Interface' for the general purpose data access wrapper methods (i.e. the ones which can be used
        // even if no table has been specified).
        // All versions which simply redirect to other versions are defined here, not in the main class.
        #region Non-table specific methods
        /// <summary>
        /// Creates a new DbConnection. You do not normally need to call this! (MightyOrm normally manages its own
        /// connections. Create a connection here and pass it on to other MightyOrm commands only in non-standard use
        /// cases where you need to explicitly manage transactions or share connections, e.g. when using explicit cursors.)
        /// </summary>
        /// <returns></returns>
        abstract public DbConnection OpenConnection();

        /// <summary>
        /// Get <see cref="IEnumerable{T}"/> of items returned by database command.
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public IEnumerable<T> Query(
            DbCommand command,
            DbConnection connection = null);

        /// <summary>
        /// Get <see cref="IEnumerable{T}"/> of items returned by SQL query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="args">Auto-numbered parameter values for SQL</param>
        /// <returns></returns>
        /// <remarks>
        /// 'Easy-calling' version, optional args straight after SQL.
        /// </remarks>
        abstract public IEnumerable<T> Query(
            string sql,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{T}"/> of items returned by SQL query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="connection">The connection to use</param>
        /// <param name="args">Auto-numbered parameter values for SQL</param>
        /// <returns></returns>
        abstract public IEnumerable<T> Query(
            string sql,
            DbConnection connection,
            params object[] args);

        /// <summary>
        /// Get single item from query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        /// <remarks>
        /// 'Easy-calling' version, optional args straight after SQL.
        /// </remarks>
        abstract public T SingleFromQuery(
            string sql,
            params object[] args);

        /// <summary>
        /// Get single item from query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="connection">The connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public T SingleFromQuery(
            string sql,
            DbConnection connection,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{T}"/> of items returned by SQL query with support for named parameters.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered parameter values for SQL</param>
        /// <returns></returns>
        abstract public IEnumerable<T> QueryWithParams(
            string sql,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get single item from query with support for named parameters.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public T SingleFromQueryWithParams(
            string sql,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{T}"/> of items from stored procedure call with support for named parameters.
        /// </summary>
        /// <param name="spName">Stored procedure name</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public IEnumerable<T> QueryFromProcedure(
            string spName,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get single item from stored procedure call with support for named parameters.
        /// </summary>
        /// <param name="spName">Stored procedure name</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public T SingleFromProcedure(
            string spName,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{R}"/> of result sets, each of which is itself an <see cref="IEnumerable{T}"/> of items, returned by database command.
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public MultipleResultSets<T> QueryMultiple(
            DbCommand command,
            DbConnection connection = null);

        /// <summary>
        /// Get <see cref="IEnumerable{R}"/> of result sets, each of which is itself an <see cref="IEnumerable{T}"/> of items, returned by SQL query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="args">Auto-numbered parameter values for SQL</param>
        /// <returns></returns>
        /// <remarks>
        /// 'Easy-calling' version, optional args straight after SQL.
        /// </remarks>
        abstract public MultipleResultSets<T> QueryMultiple(
            string sql,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{R}"/> of result sets, each of which is itself an <see cref="IEnumerable{T}"/> of items, returned by SQL query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="connection">The connection to use</param>
        /// <param name="args">Auto-numbered parameter values for SQL</param>
        /// <returns></returns>
        abstract public MultipleResultSets<T> QueryMultiple(
            string sql,
            DbConnection connection,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{R}"/> of result sets, each of which is itself an <see cref="IEnumerable{T}"/> of items, returned by SQL query with support for named parameters.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered parameter values for SQL</param>
        /// <returns></returns>
        abstract public MultipleResultSets<T> QueryMultipleWithParams(
            string sql,
            object inParams = null, 
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{R}"/> of result sets, each of which is itself an <see cref="IEnumerable{T}"/> of items, from stored procedure call with support for named parameters.
        /// </summary>
        /// <param name="spName">Stored procedure name</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public MultipleResultSets<T> QueryMultipleFromProcedure(
            string spName,
            object inParams = null, 
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Execute database command.
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns>The number of rows affected</returns>
        abstract public int Execute(
            DbCommand command,
            DbConnection connection = null);

        /// <summary>
        /// Execute SQL query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="args">Auto-numbered parameters for the SQL</param>
        /// <returns>The number of rows affected</returns>
        /// <remarks>
        /// 'Easy-calling' version, optional args straight after SQL.
        /// </remarks>
        abstract public int Execute(
            string sql,
            params object[] args);

        /// <summary>
        /// Execute SQL query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered parameters for the SQL</param>
        /// <returns>The number of rows affected</returns>
        abstract public int Execute(
            string sql,
            DbConnection connection,
            params object[] args);

        /// <summary>
        /// Execute SQL query with support for named parameters.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns>A dynamic object containing the names and output values of all output, input-output and return parameters</returns>
        abstract public dynamic ExecuteWithParams(
            string sql,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Execute stored procedure with support for named parameters.
        /// </summary>
        /// <param name="spName">Stored procedure name</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns>A dynamic object containing the names and output values of all output, input-output and return parameters</returns>
        abstract public dynamic ExecuteProcedure(
            string spName,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Return scalar result (value of first or only column from first or only row) from database command.
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public object Scalar(
            DbCommand command,
            DbConnection connection = null);

        /// <summary>
        /// Return scalar result (value of first or only column from first or only row) from SQL query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        /// <remarks>
        /// 'Easy-calling' version, optional args straight after SQL.
        /// </remarks>
        abstract public object Scalar(
            string sql,
            params object[] args);

        /// <summary>
        /// Return scalar result (value of first or only column from first or only row) from SQL query.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object Scalar(
            string sql,
            DbConnection connection,
            params object[] args);

        /// <summary>
        /// Return scalar result (value of first or only column from first or only row) from SQL query with support for named parameters.
        /// </summary>
        /// <param name="sql">The command SQL</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object ScalarWithParams(
            string sql,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Return scalar result (value of first or only column from first or only row) from stored procedure with support for named parameters.
        /// </summary>
        /// <param name="spName">Stored procedure name</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object ScalarFromProcedure(
            string spName,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Return paged results from arbitrary select statement.
        /// </summary>
        /// <param name="columns">Column spec (here, you can pass "[column-list]" or "SELECT [column-list]")</param>
        /// <param name="tableNameOrJoinSpec">A table name, or a complete join specification (i.e. anything you can SELECT FROM in SQL)</param>
        /// <param name="orderBy">ORDER BY clause</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns>The result of the paged query. Result properties are Items, TotalPages, and TotalRecords.</returns>
        abstract public PagedResults<T> PagedFromSelect(
            string tableNameOrJoinSpec,
            string orderBy,
            string columns = null,
            string where = null,
            int pageSize = 20,
            int currentPage = 1,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Return paged results from arbitrary select statement with support for named parameters.
        /// </summary>
        /// <param name="columns">Column spec (here, you can pass "[column-list]" or "SELECT [column-list]")</param>
        /// <param name="tableNameOrJoinSpec">A table name, or a complete join specification (i.e. anything you can SELECT FROM in SQL)</param>
        /// <param name="orderBy">ORDER BY clause</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns>The result of the paged query. Result properties are Items, TotalPages, and TotalRecords.</returns>
        abstract public PagedResults<T> PagedFromSelectWithParams(
            string tableNameOrJoinSpec,
            string orderBy,
            string columns = null,
            string where = null,
            int pageSize = 20,
            int currentPage = 1,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Yield return values for single or multiple resultsets.
        /// </summary>
        /// <typeparam name="X">Use with <typeparamref name="T"/> for single or <see cref="IEnumerable{T}"/> for multiple</typeparam>
        /// <param name="sql">The command SQL</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="isProcedure">Is the SQL a stored procedure name (with optional argument spec) only?</param>
        /// <param name="behavior">The command behaviour</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered parameters for the SQL</param>
        /// <returns></returns>
        abstract protected IEnumerable<X> QueryNWithParams<X>(
            string sql,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            bool isProcedure = false,
            CommandBehavior behavior = CommandBehavior.Default,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Yield return values for single or multiple resultsets.
        /// </summary>
        /// <typeparam name="X">Use with <typeparamref name="T"/> for single or <see cref="IEnumerable{T}"/> for multiple</typeparam>
        /// <param name="command">The command to execute</param>
        /// <param name="behavior">The command behaviour</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="outerReader">The outer reader when this is a call to the inner reader in QueryMultiple</param>
        /// <returns></returns>
        abstract protected internal IEnumerable<X> QueryNWithParams<X>(
            DbCommand command,
            CommandBehavior behavior = CommandBehavior.Default,
            DbConnection connection = null,
            DbDataReader outerReader = null);
        #endregion

        #region Table specific methods
        /// <summary>
        /// Perform COUNT on current table.
        /// </summary>
        /// <param name="where">WHERE clause</param>
        /// <param name="columns">Columns (defaults to *, but can be specified, e.g., to count non-nulls in a given field)</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object Count(
            string where = null,
            string columns = "*",
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Perform COUNT on current table.
        /// </summary>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="columns">Columns (defaults to *, but can be specified, e.g., to count non-nulls in a given field)</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public object Count(
            object whereParams = null,
            string columns = "*",
            DbConnection connection = null);

        /// <summary>
        /// Get MAX of column on current table.
        /// </summary>
        /// <param name="columns">Columns</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object Max(
            string columns,
            string where = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get MAX of column on current table.
        /// </summary>
        /// <param name="columns">Columns</param>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public object Max(
            string columns,
            object whereParams = null,
            DbConnection connection = null);

        /// <summary>
        /// Get MIN of column on current table.
        /// </summary>
        /// <param name="columns">Columns</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object Min(
            string columns,
            string where = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get MIN of column on current table.
        /// </summary>
        /// <param name="columns">Columns</param>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public object Min(
            string columns,
            object whereParams = null,
            DbConnection connection = null);

        /// <summary>
        /// Get SUM of column on current table.
        /// </summary>
        /// <param name="columns">Columns</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object Sum(
            string columns,
            string where = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get SUM of column on current table.
        /// </summary>
        /// <param name="columns">Columns</param>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public object Sum(
            string columns,
            object whereParams = null,
            DbConnection connection = null);

        /// <summary>
        /// Get AVG of column on current table.
        /// </summary>
        /// <param name="columns">Columns</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object Avg(
            string columns,
            string where = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get AVG of column on current table.
        /// </summary>
        /// <param name="columns">Columns</param>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public object Avg(
            string columns,
            object whereParams = null,
            DbConnection connection = null);

        /// <summary>
        /// Perform aggregate operation on the current table (use for SUM, MAX, MIN, AVG, etc.)
        /// </summary>
        /// <param name="function">Aggregate function</param>
        /// <param name="columns">Columns for aggregate function</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object Aggregate(
            string function,
            string columns,
            string where = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Perform aggregate operation on the current table (use for SUM, MAX, MIN, AVG, etc.)
        /// </summary>
        /// <param name="function">Aggregate function</param>
        /// <param name="columns">Columns for aggregate function</param>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public object Aggregate(
            string function,
            string columns,
            object whereParams = null,
            DbConnection connection = null);

        /// <summary>
        /// Perform aggregate operation on the current table (use for SUM, MAX, MIN, AVG, etc.), with support for named params.
        /// </summary>
        /// <param name="function">Aggregate function</param>
        /// <param name="columns">Columns for aggregate function</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public object AggregateWithParams(
            string function,
            string columns,
            string where = null,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get single item returned by database command.
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public T Single(
            DbCommand command,
            DbConnection connection = null);

        /// <summary>
        /// Get single item from the current table using primary key or name-value where specification.
        /// </summary>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="columns">List of columns to return</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public T Single(
            object whereParams,
            string columns = null,
            DbConnection connection = null);

        /// <summary>
        /// Get single item from the current table using WHERE specification.
        /// </summary>
        /// <param name="where">WHERE clause</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        /// <remarks>
        /// 'Easy-calling' version, optional args straight after where.
        /// </remarks>
        abstract public T Single(
            string where,
            params object[] args);

        /// <summary>
        /// Get single item from the current table using WHERE specification.
        /// </summary>
        /// <param name="where">WHERE clause</param>
        /// <param name="connection">The connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        /// <remarks>
        /// 'Easy-calling' version, optional args straight after where.
        /// </remarks>
        abstract public T Single(
            string where,
            DbConnection connection,
            params object[] args);

        /// <summary>
        /// Get single item from the current table using WHERE specification.
        /// </summary>
        /// <param name="where">WHERE clause</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="orderBy">ORDER BY clause</param>
        /// <param name="columns">Columns to return</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        /// <remarks>
        /// <see cref="DbConnection"/> arg coming early (not just before params) in this one case is really useful, because
        /// it avoids any ambiguity between the `columns` and `orderBy` strings and any params string args;
        /// i.e. Single("WHERE Title=@0", "Star Wars") is definitely a param, not an order by.
        /// TO DO: Test! Why isn't null, string, string still treated as params? Or maybe it is, but that's okay.
        /// </remarks>
        abstract public T Single(
            string where,
            DbConnection connection = null, // *** position - see remarks
            string orderBy = null,
            string columns = null,
            params object[] args);

        /// <summary>
        /// Get single item from the current table using WHERE specification with support for named parameters.
        /// </summary>
        /// <param name="where">WHERE clause</param>
        /// <param name="orderBy">ORDER BY clause</param>
        /// <param name="columns">Columns to return</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public T SingleWithParams(
            string where,
            string orderBy = null,
            string columns = null,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{T}"/> of items from the current table with WHERE and TOP/LIMIT specification.
        /// </summary>
        /// <param name="where">WHERE clause</param>
        /// <param name="orderBy">ORDER BY clause</param>
        /// <param name="columns">Columns to return</param>
        /// <param name="limit">Maximum number of items to return</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public IEnumerable<T> All(
            string where = null,
            string orderBy = null,
            string columns = null,
            int limit = 0,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Get <see cref="IEnumerable{T}"/> of items from the current table with primary key or name-value where specification and TOP/LIMIT specification.
        /// </summary>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="orderBy">ORDER BY clause</param>
        /// <param name="columns">Columns to return</param>
        /// <param name="limit">Maximum number of items to return</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns></returns>
        abstract public IEnumerable<T> All(
            object whereParams = null,
            string orderBy = null,
            string columns = null,
            int limit = 0,
            DbConnection connection = null);

        /// <summary>
        /// Get <see cref="IEnumerable{T}"/> of items from the current table with WHERE and TOP/LIMIT specification and support for named parameters.
        /// </summary>
        /// <param name="where">WHERE clause</param>
        /// <param name="orderBy">ORDER BY clause</param>
        /// <param name="columns">Columns to return</param>
        /// <param name="limit">Maximum number of items to return</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns></returns>
        abstract public IEnumerable<T> AllWithParams(
            string where = null,
            string orderBy = null,
            string columns = null,
            int limit = 0,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Table-specific paging; there is also a data wrapper version of paging <see cref="PagedFromSelect"/>.
        /// </summary>
        /// <param name="orderBy">You may provide orderBy, if you don't it will try to order by PK and will produce an exception if there is no PK defined.</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="columns">Columns to return</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns>The result of the paged query. Result properties are Items, TotalPages, and TotalRecords.</returns>
        /// <remarks>
        /// <paramref name="columns"/> parameter is not placed first because it's an override to something we may have alread provided in the constructor,
        /// so we don't want the user to have to non-fluently re-type it, or else type null, every time.
        /// </remarks>
        abstract public PagedResults<T> Paged(
            string orderBy = null,
            string columns = null,
            string where = null,
            int pageSize = 20,
            int currentPage = 1,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Table-specific paging with support for named parameters; there is also a data wrapper version of paging <see cref="PagedFromSelect"/>.
        /// </summary>
        /// <param name="orderBy">You may provide orderBy, if you don't it will try to order by PK and will produce an exception if there is no PK defined.</param>
        /// <param name="where">WHERE clause</param>
        /// <param name="columns">Columns to return</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="inParams">Named input parameters</param>
        /// <param name="outParams">Named output parameters</param>
        /// <param name="ioParams">Named input-output parameters</param>
        /// <param name="returnParams">Named return parameters</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns>The result of the paged query. Result properties are Items, TotalPages, and TotalRecords.</returns>
        /// <remarks>
        /// <paramref name="columns"/> parameter is not placed first because it's an override to something we may have alread provided in the constructor,
        /// so we don't want the user to have to non-fluently re-type it, or else type null, every time.
        /// </remarks>
        abstract public PagedResults<T> PagedWithParams(
            string orderBy = null,
            string columns = null,
            string where = null,
            int pageSize = 20,
            int currentPage = 1,
            object inParams = null,
            object outParams = null,
            object ioParams = null,
            object returnParams = null,
            DbConnection connection = null,
            params object[] args);

        /// <summary>
        /// Save one or more items.
        /// 'Save' means
        /// objects with missing or default primary keys are inserted
        /// and objects with non-default primary keys are updated.
        /// </summary>
        /// <param name="items">The items</param>
        /// <returns></returns>
        abstract public int Save(
            params object[] items);

        /// <summary>
        /// Save one or more items.
        /// 'Save' means
        /// objects with missing or default primary keys are inserted
        /// and objects with non-default primary keys are updated.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="items">The items</param>
        /// <returns></returns>
        abstract public int Save(
            DbConnection connection,
            params object[] items);

        /// <summary>
        /// Save array or other <see cref="IEnumerable"/> of items.
        /// 'Save' means
        /// objects with missing or default primary keys are inserted
        /// and objects with non-default primary keys are updated.
        /// </summary>
        /// <param name="items">The items</param>
        /// <returns></returns>
        abstract public int Save(
            IEnumerable<object> items);

        /// <summary>
        /// Save array or other <see cref="IEnumerable"/> of items.
        /// 'Save' means
        /// objects with missing or default primary keys are inserted
        /// and objects with non-default primary keys are updated.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="items">The items</param>
        /// <returns></returns>
        abstract public int Save(
            IEnumerable<object> items,
            DbConnection connection);

        /// <summary>
        /// Insert single item.
        /// Call <see cref="New"/> before insert if you need to pre-populate your inserted items with any defined database column defaults.
        /// </summary>
        /// <param name="item">The item to insert, in any reasonable format (for MightyOrm&lt;T&gt; this includes, but is not limited to, in instance of type T)</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns>The item sent in but with the primary key populated</returns>
        abstract public T Insert(
            object item,
            DbConnection connection = null);

        /// <summary>
        /// Insert one or more items.
        /// Call <see cref="New"/> before insert if you need to pre-populate your inserted items with any defined database column defaults.
        /// </summary>
        /// <param name="items">The items</param>
        /// <returns>The items sent in but with the primary keys populated</returns>
        abstract public List<T> Insert(
            params object[] items);

        /// <summary>
        /// Insert one or more items.
        /// Call <see cref="New"/> before insert if you need to pre-populate your inserted items with any defined database column defaults.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="items">The items</param>
        /// <returns>The items sent in but with the primary keys populated</returns>
        abstract public List<T> Insert(
            DbConnection connection,
            params object[] items);

        /// <summary>
        /// Insert array or other <see cref="IEnumerable"/> of items.
        /// Call <see cref="New"/> before insert if you need to pre-populate your inserted items with any defined database column defaults.
        /// </summary>
        /// <param name="items">The items</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns>The items sent in but with the primary keys populated</returns>
        abstract public List<T> Insert(
            IEnumerable<object> items,
            DbConnection connection = null);

        /// <summary>
        /// Update one or more items.
        /// </summary>
        /// <param name="items">The items</param>
        /// <returns></returns>
        abstract public int Update(
            params object[] items);

        /// <summary>
        /// Update one or more items.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="items">The items</param>
        /// <returns></returns>
        abstract public int Update(
            DbConnection connection,
            params object[] items);

        /// <summary>
        /// Update array or other <see cref="IEnumerable"/> of items.
        /// </summary>
        /// <param name="items">The items</param>
        /// <returns></returns>
        abstract public int Update(
            IEnumerable<object> items);

        /// <summary>
        /// Update array or other <see cref="IEnumerable"/> of items.
        /// </summary>
        /// <param name="connection">The connection to use</param>
        /// <param name="items">The items</param>
        /// <returns></returns>
        abstract public int Update(
            IEnumerable<object> items,
            DbConnection connection);

        /// <summary>
        /// Delete one or more items.
        /// Each argument may be (or contain) a value (or values) only, in which case
        /// it specifies the primary key value(s) of the item to delete, or it can be any object containing name-values pairs in which case
        /// it should contain fields with names matching the primary key(s) whose values will specify the item to delete (but it may contain
        /// other fields as well which will be ignored here).
        /// </summary>
        /// <param name="items">The items</param>
        /// <returns>The number of items affected</returns>
        abstract public int Delete(
            params object[] items);

        /// <summary>
        /// Delete one or more items.
        /// Each argument may be (or contain) a value (or values) only, in which case
        /// it specifies the primary key value(s) of the item to delete, or it can be any object containing name-values pairs in which case
        /// it should contain fields with names matching the primary key(s) whose values will specify the item to delete (but it may contain
        /// other fields as well which will be ignored here).
        /// </summary>
        /// <param name="items">The items</param>
        /// <param name="connection">The connection to use</param>
        /// <returns>The number of items affected</returns>
        abstract public int Delete(
            DbConnection connection,
            params object[] items);

        /// <summary>
        /// Delete an array or other <see cref="IEnumerable"/> of items.
        /// Each argument may be (or contain) a value (or values) only, in which case
        /// it specifies the primary key value(s) of the item to delete, or it can be any object containing name-values pairs in which case
        /// it should contain fields with names matching the primary key(s) whose values will specify the item to delete (but it may contain
        /// other fields as well which will be ignored here).
        /// </summary>
        /// <param name="items">The items</param>
        /// <param name="connection">Optional connection to use</param>
        /// <returns>The number of items affected</returns>
        abstract public int Delete(
            IEnumerable<object> items,
            DbConnection connection = null);

        /// <summary>
        /// Update the row(s) specified by the primary key(s) or WHERE values sent in using the values from the item sent in.
        /// If `keys` has been specified on the current Mighty instance then any primary key fields in the item are ignored.
        /// The item is not filtered to remove fields not in the table, if you need that you can call <see cref="New"/> with first parameter `partialItem` and second parameter `false` first.
        /// </summary>
        /// <param name="partialItem">Item containing values to update with</param>
        /// <param name="whereParams">Value(s) to be mapped to the table's primary key(s), or object containing named value(s) to be mapped to the matching named column(s)</param>
        /// <param name="connection">Optional connection to use</param>
        abstract public int UpdateUsing(
            object partialItem,
            object whereParams,
            DbConnection connection = null);

        /// <summary>
        /// Update all items matching WHERE clause using fields from the item sent in.
        /// If `keys` has been specified on the current Mighty instance then any primary key fields in the item are ignored.
        /// The item is not filtered to remove fields not in the table, if you need that you can call <see cref="New"/> with first parameter `partialItem` and second parameter `false` first.
        /// </summary>
        /// <param name="partialItem">Item containing values to update with</param>
        /// <param name="where">WHERE clause specifying which rows to update</param>
        /// <param name="args">Auto-numbered input parameters</param>
        abstract public int UpdateUsing(
            object partialItem,
            string where,
            params object[] args);

        /// <summary>
        /// Update all items matching WHERE clause using fields from the item sent in.
        /// If `keys` has been specified on the current Mighty instance then any primary key fields in the item are ignored.
        /// The item is not filtered to remove fields not in the table, if you need that you can call <see cref="New"/> with first parameter `partialItem` and second parameter `false` first.
        /// </summary>
        /// <param name="partialItem">Item containing values to update with</param>
        /// <param name="where">WHERE clause specifying which rows to update</param>
        /// <param name="connection">Optional connection to use</param>
        /// <param name="args">Auto-numbered input parameters</param>
        abstract public int UpdateUsing(
            object partialItem,
            string where,
            DbConnection connection,
            params object[] args);

        /// <summary>
        /// Delete one or more items based on a WHERE clause.
        /// </summary>
        /// <param name="where">
        /// Non-optional WHERE clause.
        /// Specify "1=1" if you are sure that you want to delete all rows.
        /// </param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <returns>The number of items affected</returns>
        abstract public int Delete(
            string where,
            params object[] args);

        /// <summary>
        /// Delete one or more items based on a WHERE clause.
        /// </summary>
        /// <param name="where">
        /// Non-optional WHERE clause.
        /// Specify "1=1" if you are sure that you want to delete all rows.
        /// </param>
        /// <param name="args">Auto-numbered input parameters</param>
        /// <param name="connection">The connection to use</param>
        /// <returns>The number of items affected</returns>
        abstract public int Delete(
            string where,
            DbConnection connection,
            params object[] args);

#if KEY_VALUES
        /// <summary>
        /// Returns a string-string dictionary which can be directly bound to ASP.NET dropdowns etc. (see https://stackoverflow.com/a/805610/795690).
        /// </summary>
        /// <param name="orderBy">Order by, defaults to primary key</param>
        /// <returns></returns>
        abstract public IDictionary<string, string> KeyValues(string orderBy = null);
#endif
#endregion
    }
}
