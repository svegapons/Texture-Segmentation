using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace DataBaseManager.Helpers
{
    class DbHelper
    {

        private static string helperName;
        private static Type helperType;

        public static string HelperName
        {
            get { return DbHelper.helperName; }
            set
            {
                DbHelper.helperName = value;
                helperType = Type.GetType("DataBaseManager.Helpers."+helperName + "Wrapper");
          
            }
        }

        #region "ExecuteNonQuery"

        /// Execute a DbCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// e.g.:  
        /// int result =  ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// Parameters:
        /// -connectionString - a valid connection string for a DbConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return (int)helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(string), typeof(CommandType), typeof(string) }).Invoke(null, new object[] { connectionString, commandType, commandText });
        }

        /// Execute a DbCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// e.g.:  
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24))
        /// Parameters:
        /// -connectionString - a valid connection string for a DbConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command
        /// -commandParameters - an array of DbParamters used to execute the command
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            MethodInfo mf = helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(string), typeof(CommandType), typeof(string), typeof(DbParameter[]) });
            return (int)mf.Invoke(null, new object[] { connectionString, commandType, commandText, commandParameters });
        }

        /// Execute a stored procedure via a DbCommand (that returns no resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36)
        /// Parameters:
        /// -connectionString - a valid connection string for a DbConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            return (int)helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(string), typeof(string), typeof(object[]) }).Invoke(null, new object[] { connectionString, spName, parameterValues });

        }

        /// Execute a DbCommand (that returns no resultset and takes no parameters) against the provided DbConnection. 
        /// e.g.:  
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
        /// Parameters:
        /// -connection - a valid DbConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText)
        {
            return (int)helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(DbConnection), typeof(CommandType), typeof(string) }).Invoke(null, new object[] { connection, commandType, commandText });
        }

        /// Execute a DbCommand (that returns no resultset) against the specified DbConnection 
        /// using the provided parameters.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new DbParameter("@prodid", 24))
        /// Parameters:
        /// -connection - a valid DbConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command 
        /// -commandParameters - an array of DbParamters used to execute the command 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return (int)helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(DbConnection), typeof(CommandType), typeof(string), typeof(DbParameter[]) }).Invoke(null, new object[] { connection, commandType, commandText, commandParameters });
        }

        /// Execute a stored procedure via a DbCommand (that returns no resultset) against the specified DbConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36)
        /// Parameters:
        /// -connection - a valid DbConnection
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbConnection connection, string spName, params object[] parameterValues)
        {
            return (int)helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(DbConnection), typeof(string), typeof(object[]) }).Invoke(null, new object[] { connection, spName, parameterValues });
        }

        /// Execute a DbCommand (that returns no resultset and takes no parameters) against the provided DbTransaction.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
        /// Parameters:
        /// -transaction - a valid DbTransaction associated with the connection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-Db command 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return (int)helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(DbTransaction), typeof(CommandType), typeof(string) }).Invoke(null, new object[] { transaction, commandType, commandText });
        }

        /// Execute a DbCommand (that returns no resultset) against the specified DbTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24))
        /// Parameters:
        /// -transaction - a valid DbTransaction 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-Db command 
        /// -commandParameters - an array of DbParamters used to execute the command 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return (int)helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(DbTransaction), typeof(CommandType), typeof(string), typeof(DbParameter[]) }).Invoke(null, new object[] { transaction, commandType, commandText, commandParameters });
        }

        /// Execute a stored procedure via a DbCommand (that returns no resultset) against the specified DbTransaction 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int result = DbHelper.ExecuteNonQuery(trans, "PublishOrders", 24, 36)
        /// Parameters:
        /// -transaction - a valid DbTransaction 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            return (int)helperType.GetMethod("ExecuteNonQuery", new Type[] { typeof(DbTransaction), typeof(string), typeof(object[]) }).Invoke(null, new object[] { transaction, spName, parameterValues });
        }

        #endregion

        #region "ExecuteDataset"
        /// Execute a DbCommand (that returns a resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// e.g.:  
        /// DataSet ds = DbHelper.ExecuteDataset("", commandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connectionString - a valid connection string for a DbConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(string), typeof(CommandType), typeof(string) }).Invoke(null, new object[] { connectionString, commandType, commandText });
        }

        /// Execute a DbCommand (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24))
        /// Parameters:
        /// -connectionString - a valid connection string for a DbConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command
        /// -commandParameters - an array of DbParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(string), typeof(CommandType), typeof(string), typeof(DbParameter[]) }).Invoke(null, new object[] { connectionString, commandType, commandText, commandParameters });
        }

        /// Execute a stored procedure via a DbCommand (that returns a resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36)
        /// Parameters:
        /// -connectionString - a valid connection string for a DbConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(string), typeof(string), typeof(object[]) }).Invoke(null, new object[] { connectionString, spName, parameterValues });
        }

        /// Execute a DbCommand (that returns a resultset and takes no parameters) against the provided DbConnection. 
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connection - a valid DbConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(DbConnection), typeof(CommandType), typeof(string) }).Invoke(null, new object[] { connection, commandType, commandText });
        }

        /// Execute a DbCommand (that returns a resultset) against the specified DbConnection 
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24))
        /// Parameters:
        /// -connection - a valid DbConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command
        /// -commandParameters - an array of DbParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command 
        public static DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(DbConnection), typeof(CommandType), typeof(string), typeof(DbParameter[]) }).Invoke(null, new object[] { connection, commandType, commandText, commandParameters });
        }

        /// Execute a stored procedure via a DbCommand (that returns a resultset) against the specified DbConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36)
        /// Parameters:
        /// -connection - a valid DbConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbConnection connection, string spName, params object[] parameterValues)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(DbConnection), typeof(string), typeof(object[]) }).Invoke(null, new object[] { connection, spName, parameterValues });
        }

        /// Execute a DbCommand (that returns a resultset and takes no parameters) against the provided DbTransaction. 
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        /// Parameters
        /// -transaction - a valid DbTransaction
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(DbTransaction), typeof(CommandType), typeof(string) }).Invoke(null, new object[] { transaction, commandType, commandText });
        }

        /// Execute a DbCommand (that returns a resultset) against the specified DbTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new DbParameter("@prodid", 24))
        /// Parameters
        /// -transaction - a valid DbTransaction 
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Db command
        /// -commandParameters - an array of DbParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command 
        public static DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(DbTransaction), typeof(CommandType), typeof(string), typeof(DbParameter[]) }).Invoke(null, new object[] { transaction, commandType, commandText, commandParameters });
        }

        /// Execute a stored procedure via a DbCommand (that returns a resultset) against the specified
        /// DbTransaction using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36)
        /// Parameters:
        /// -transaction - a valid DbTransaction 
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            return (DataSet)helperType.GetMethod("ExecuteDataset", new Type[] { typeof(DbTransaction), typeof(string), typeof(object[]) }).Invoke(null, new object[] { transaction, spName, parameterValues });
        }
        #endregion

        public static string GetConditionParameters(params string[] pars)
        {
            return (string)helperType.GetMethod("GetConditionParameters", new Type[] { typeof(string[]) }).Invoke(null, new object[] { pars });
        }
        public static string GetParameterList(params string[] pars)
        {
            return (string)helperType.GetMethod("GetParameterList", new Type[] { typeof(string[]) }).Invoke(null, new object[] { pars });
        }
        public static DbParameter[] CreateParameters(string[] parNames, object[] parValues)
        {
            return (DbParameter[])helperType.GetMethod("CreateParameters", new Type[] { typeof(string[]), typeof(object[]) }).Invoke(null, new object[] { parNames, parValues });
        }

        public static DbConnection GetConnection(string connectionString)
        {
            return (DbConnection)helperType.GetMethod("GetConnection", new Type[] { typeof(string) }).Invoke(null, new object[] { connectionString });
        }
    }
}
