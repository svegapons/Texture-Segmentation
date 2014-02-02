using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Data.Common;

namespace DataBaseManager.Helpers
{
    class OracleHelperWrapper
    {
        #region "ExecuteNonQuery"

        /// Execute a OracleCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// e.g.:  
        /// int result =  ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// Parameters:
        /// -connectionString - a valid connection string for a OracleConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return OracleHelper.ExecuteNonQuery(connectionString, commandType, commandText);
        }

        /// Execute a OracleCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// e.g.:  
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24))
        /// Parameters:
        /// -connectionString - a valid connection string for a OracleConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command
        /// -commandParameters - an array of OracleParamters used to execute the command
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return OracleHelper.ExecuteNonQuery(connectionString, commandType, commandText, (OracleParameter[])commandParameters);
        }

        /// Execute a stored procedure via a OracleCommand (that returns no resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36)
        /// Parameters:
        /// -connectionString - a valid connection string for a OracleConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            return OracleHelper.ExecuteNonQuery(connectionString, spName, parameterValues);
        }

        /// Execute a OracleCommand (that returns no resultset and takes no parameters) against the provided OracleConnection. 
        /// e.g.:  
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
        /// Parameters:
        /// -connection - a valid OracleConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText)
        {
            return OracleHelper.ExecuteNonQuery((OracleConnection)connection, commandType, commandText);
        }

        /// Execute a OracleCommand (that returns no resultset) against the specified OracleConnection 
        /// using the provided parameters.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24))
        /// Parameters:
        /// -connection - a valid OracleConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command 
        /// -commandParameters - an array of OracleParamters used to execute the command 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return OracleHelper.ExecuteNonQuery((OracleConnection)connection, commandType, commandText, (OracleParameter[])commandParameters);
        }

        /// Execute a stored procedure via a OracleCommand (that returns no resultset) against the specified OracleConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36)
        /// Parameters:
        /// -connection - a valid OracleConnection
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbConnection connection, string spName, params object[] parameterValues)
        {
            return OracleHelper.ExecuteNonQuery((OracleConnection)connection, spName, parameterValues);
        }

        /// Execute a OracleCommand (that returns no resultset and takes no parameters) against the provided OracleTransaction.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
        /// Parameters:
        /// -transaction - a valid OracleTransaction associated with the connection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-Oracle command 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return OracleHelper.ExecuteNonQuery((OracleTransaction)transaction, commandType, commandText);
        }

        /// Execute a OracleCommand (that returns no resultset) against the specified OracleTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        /// Parameters:
        /// -transaction - a valid OracleTransaction 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-Oracle command 
        /// -commandParameters - an array of OracleParamters used to execute the command 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return OracleHelper.ExecuteNonQuery((OracleTransaction)transaction, commandType, commandText, (OracleParameter[])commandParameters);
        }

        /// Execute a stored procedure via a OracleCommand (that returns no resultset) against the specified OracleTransaction 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int result = OracleHelper.ExecuteNonQuery(trans, "PublishOrders", 24, 36)
        /// Parameters:
        /// -transaction - a valid OracleTransaction 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            return OracleHelper.ExecuteNonQuery((OracleTransaction)transaction, spName, parameterValues);
        }

        #endregion

        #region "ExecuteDataset"
        /// Execute a OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// e.g.:  
        /// DataSet ds = OracleHelper.ExecuteDataset("", commandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connectionString - a valid connection string for a OracleConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return OracleHelper.ExecuteDataset(connectionString, commandType, commandText);
        }

        /// Execute a OracleCommand (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        /// Parameters:
        /// -connectionString - a valid connection string for a OracleConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command
        /// -commandParameters - an array of OracleParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return OracleHelper.ExecuteDataset(connectionString, commandType, commandText, (OracleParameter[])commandParameters);
        }

        /// Execute a stored procedure via a OracleCommand (that returns a resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36)
        /// Parameters:
        /// -connectionString - a valid connection string for a OracleConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            return OracleHelper.ExecuteDataset(connectionString, spName, parameterValues);
        }

        /// Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection. 
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connection - a valid OracleConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText)
        {
            return OracleHelper.ExecuteDataset((OracleConnection)connection, commandType, commandText);
        }

        /// Execute a OracleCommand (that returns a resultset) against the specified OracleConnection 
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        /// Parameters:
        /// -connection - a valid OracleConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command
        /// -commandParameters - an array of OracleParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command 
        public static DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return OracleHelper.ExecuteDataset((OracleConnection)connection, commandType, commandText, (OracleParameter[])commandParameters);
        }

        /// Execute a stored procedure via a OracleCommand (that returns a resultset) against the specified OracleConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36)
        /// Parameters:
        /// -connection - a valid OracleConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbConnection connection, string spName, params object[] parameterValues)
        {
            return OracleHelper.ExecuteDataset((OracleConnection)connection, spName, parameterValues);
        }

        /// Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction. 
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        /// Parameters
        /// -transaction - a valid OracleTransaction
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return OracleHelper.ExecuteDataset((OracleTransaction)transaction, commandType, commandText);
        }

        /// Execute a OracleCommand (that returns a resultset) against the specified OracleTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        /// Parameters
        /// -transaction - a valid OracleTransaction 
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-Oracle command
        /// -commandParameters - an array of OracleParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command 
        public static DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return OracleHelper.ExecuteDataset((OracleTransaction)transaction, commandType, commandText, (OracleParameter[])commandParameters);
        }

        /// Execute a stored procedure via a OracleCommand (that returns a resultset) against the specified
        /// OracleTransaction using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36)
        /// Parameters:
        /// -transaction - a valid OracleTransaction 
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            return OracleHelper.ExecuteDataset((OracleTransaction)transaction, spName, parameterValues);
        }
        #endregion

        public static string GetConditionParameters(params string[] pars)
        {
            string result = "";

            result += "( " + pars[0] + " = :" + pars[0] + " ) ";

            for (int i = 1; i < pars.Length; i++)
            {
                result += "AND " + "( " + pars[i] + " = :" + pars[i] + " ) ";
            }
            return result;

        }

        public static string GetParameterList(params string[] pars)
        {
            string result = ":" + pars[0];
            for (int i = 1; i < pars.Length; i++)
            {
                result += ", :" + pars[i];
            }
            return result;
        }

        public static DbParameter[] CreateParameters(string[] parNames, object[] parValues)
        {
            OracleParameter[] result = new OracleParameter[parNames.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new OracleParameter(":"+parNames[i], parValues[i]);
            }
            return (DbParameter[])result;
        }

        public static DbConnection GetConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }
    }
}
