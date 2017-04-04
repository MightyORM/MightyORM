using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

using Mighty.ConnectionProviders;
using Mighty.DatabasePlugins;
using Mighty.Interfaces;

namespace Mighty
{
	public partial class MightyORM
		// (- wait till we're ready to actually implement! -)
		// : MicroORM
		// , DataAccessWrapper
		// , NpgsqlCursorController
	{
		protected string _connectionString;
		protected DbProviderFactory _factory;
		private DatabasePlugin _plugin = null;

		// these should all be properties
		// initialise table name from class name, but only if not == MicroORM(!); get, set, throw
		// exception if attempt to use it when not set
		public string TableName; // NB this may have a dot in to specify owner/schema, and then needs splitting by us, but ONLY when getting information schema
		public List<string> PrimaryKeys;
		public string Columns;

		// primaryKeySequence is for sequence-based databases (Oracle, PostgreSQL) - there is no default, specify either null or empty string to disable and manually specify your PK values;
		// primaryKeyRetrievalFunction is for non-sequence based databases (MySQL, SQL Server, SQLite) - defaults to default for DB, specify empty string to disable and manually specify your PK values;
		// primaryKeyFields is a comma separated list; if it has more than one column, you cannot specify primaryKeySequence or primaryKeyRetrievalFunction
		// (if neither primaryKeySequence nor primaryKeyRetrievalFunction are set (which is always the case for compound primary keys), you MUST specify non-null, non-default values for every column in your primary key
		// before saving an object)
		public MightyORM(string connectionStringOrName = null, string tableName = null, string primaryKeyFields = null, string primaryKeySequence = null, string primaryKeyRetrievalFunction = null, string defaultColumns = null, ConnectionProvider connectionProvider = null)
		{
			if (connectionProvider == null)
			{
// #if !COREFX
// 				connectionProvider = new ConfigFileConnectionProvider().Init(connectionStringOrName);
// 				if (connectionProvider.ConnectionString == null)
// #endif
				{
					connectionProvider = new PureConnectionStringProvider()
// #if !COREFX
// 						.UsedAfterConfigFile()
// #endif
						.Init(connectionStringOrName);
				}
			}
			else
			{
				connectionProvider.Init(connectionStringOrName);
			}

			_connectionString = connectionProvider.ConnectionString;
			_factory = connectionProvider.ProviderFactory;
			if (tableName != null)
			{
				TableName = tableName;
			}
			else
			{
				var type = this.GetType();
				// enforce subclass
				// (otherwise we work with no table name and data wrapper but not ORM features are available)
				if (type != typeof(MightyORM))
				{
					TableName = CreateTableNameFromClassName(type.Name);
				}
			}
			PrimaryKeys = primaryKeyFields.Split(',').Select(k => k.Trim()).ToList();
			Columns = defaultColumns;
		}

		// mini-factory for non-table specific access
		public static MightyORM DB(string connectionStringOrName = null)
		{
			return new MightyORM(connectionStringOrName);
		}

		private DatabasePlugin GetPlugin(SupportedDatabase supportedDatabase)
		{
			var pluginClassName = "Mighty.Plugins." + supportedDatabase.ToString();
			var type = Type.GetType(pluginClassName);
			if (type == null)
			{
				throw new NotImplementedException("Cannot find type " + pluginClassName);
			}
			var plugin = (DatabasePlugin)Activator.CreateInstance(type, false);
			plugin._mightyInstance = this;
			return plugin;
		}

		private IEnumerable<T> QueryNWithParams<T>(string sql, object args, object inParams = null, object outParams = null, object ioParams = null, object returnParams = null, bool isProcedure = false, DbConnection connection = null, DbCommand command = null)
		{
			using (var localConn = (connection == null ? OpenConnection() : null))
			{
				if (command != null)
				{
					command.Connection = localConn;
				}
				else
				{
					command = CreateCommandWithParams(sql, inParams, outParams, ioParams, returnParams, isProcedure, connection ?? localConn, args);
				}
				// manage wrapping transaction if required, and if we have not been passed an incoming connection
				using (var trans = ((connection == null
#if !COREFX
					////&& Transaction.Current == null
#endif
					&& _plugin.RequiresWrappingTransaction(command)) ? localConn.BeginTransaction() : null))
				{
					// TO DO: Apply single result hint when appropriate
					// (since all the cursors we might dereference come in the first result set, we can do this even
					// if we are dereferencing PostgreSQL cursors)
					using (var rdr = _plugin.ExecuteDereferencingReader(command, connection ?? localConn))
					{
						if (typeof(T) == typeof(IEnumerable<dynamic>))
						{
							// query multiple pattern
							do
							{
								yield return (T)rdr.YieldResult();
							}
							while (rdr.NextResult());
						}
						else
						{
							// query pattern
							while (rdr.Read())
							{
								yield return rdr.RecordToExpando();
							}
						}
					}
					if (trans != null) trans.Commit();
				}
			}
		}

		public dynamic ResultsAsExpando(DbCommand cmd)
		{
			dynamic result = new ExpandoObject();
			var resultDictionary = (IDictionary<string, object>)result;
			for (int i = 0; i < cmd.Parameters.Count; i++)
			{
				var param = cmd.Parameters[i];
				if (param.Direction != ParameterDirection.Input)
				{
					var name = _plugin.DeprefixParameterName(param.ParameterName, cmd);
					var value = _plugin.GetValue(param);
					resultDictionary.Add(name, value == DBNull.Value ? null : value);
				}
			}
			return result;
		}

		// The ones which really are the same cross-db don't need to be put into the plugin classes; plugin can be extended if required at a future point.
		private string BuildScalar(string expression)
		{
			return string.Format("SELECT {0} FROM {1}", expression, TableName);
		}

		public string CreateTableNameFromClassName(string className)
		{
			return className;
		}

#region Not Implemented - TEMP
		public DbConnection OpenConnection()
		{
			throw new NotImplementedException();
		}

		public DbCommand CreateCommandWithParams(string sql,
			object inParams = null, object outParams = null, object ioParams = null, object returnParams = null, bool isProcedure = false,
			DbConnection connection = null,
			params object[] args)
		{
			throw new NotImplementedException();
		}
#endregion
	}
}