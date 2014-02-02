using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace DataBaseManager.Helpers
{
    public class SqlHelperWrapper
    {

        #region "ExecuteNonQuery"

        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// e.g.:  
        /// int result =  ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText);
        }

        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// e.g.:  
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// -commandParameters - an array of SqlParamters used to execute the command
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])commandParameters);
        }

        /// Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36)
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteNonQuery(connectionString, spName, parameterValues);
        }

        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
        /// e.g.:  
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
        /// Parameters:
        /// -connection - a valid SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery((SqlConnection)connection, commandType, commandText);
        }

        /// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery((SqlConnection)connection, commandType, commandText, (SqlParameter[])commandParameters);
        }

        /// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36)
        /// Parameters:
        /// -connection - a valid SqlConnection
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbConnection connection, string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteNonQuery((SqlConnection)connection, spName, parameterValues);
        }

        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
        /// Parameters:
        /// -transaction - a valid SqlTransaction associated with the connection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteNonQuery((SqlTransaction)transaction, commandType, commandText);
        }

        /// Execute a SqlCommand (that returns no resultset) against the specified SqlTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -transaction - a valid SqlTransaction 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return SqlHelper.ExecuteNonQuery((SqlTransaction)transaction, commandType, commandText, (SqlParameter[])commandParameters);
        }

        /// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlTransaction 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int result = SqlHelper.ExecuteNonQuery(trans, "PublishOrders", 24, 36)
        /// Parameters:
        /// -transaction - a valid SqlTransaction 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteNonQuery((SqlTransaction)transaction, spName, parameterValues);
        }

        #endregion

        #region "ExecuteDataset"
        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// e.g.:  
        /// DataSet ds = SqlHelper.ExecuteDataset("", commandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText);
        }

        /// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// -commandParameters - an array of SqlParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])commandParameters);
        }

        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36)
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteDataset(connectionString, spName, parameterValues);
        }

        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connection - a valid SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset((SqlConnection)connection, commandType, commandText);
        }

        /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -connection - a valid SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// -commandParameters - an array of SqlParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command 
        public static DataSet ExecuteDataset(DbConnection connection, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset((SqlConnection)connection, commandType, commandText, (SqlParameter[])commandParameters);
        }

        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36)
        /// Parameters:
        /// -connection - a valid SqlConnection
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbConnection connection, string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteDataset((SqlConnection)connection, spName, parameterValues);
        }

        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        /// Parameters
        /// -transaction - a valid SqlTransaction
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText)
        {
            return SqlHelper.ExecuteDataset((SqlTransaction)transaction, commandType, commandText);
        }

        /// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        /// Parameters
        /// -transaction - a valid SqlTransaction 
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// -commandParameters - an array of SqlParamters used to execute the command
        /// Returns: a dataset containing the resultset generated by the command 
        public static DataSet ExecuteDataset(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] commandParameters)
        {
            return SqlHelper.ExecuteDataset((SqlTransaction)transaction, commandType, commandText, (SqlParameter[])commandParameters);
        }

        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified
        /// SqlTransaction using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36)
        /// Parameters:
        /// -transaction - a valid SqlTransaction 
        /// -spName - the name of the stored procedure
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(DbTransaction transaction, string spName, params object[] parameterValues)
        {
            return SqlHelper.ExecuteDataset((SqlTransaction)transaction, spName, parameterValues);
        }
        #endregion

        public static string GetConditionParameters(params string[] pars)
        {
            string result = "( " + pars[0] + " = @" + pars[0] + " ) ";

            for (int i = 1; i < pars.Length; i++)
            {
                result += "AND " + "( " + pars[i] + " = @" + pars[i] + " ) ";
            }
            return result;

        }

        public static string GetParameterList(params string[] pars)
        {
            string result = "@" + pars[0];
            for (int i = 1; i < pars.Length; i++)
            {
                result += ", @" + pars[i];
            }
            return result;
        }

        public static DbParameter[] CreateParameters(string[] parNames, object[] parValues)
        {
            SqlParameter[] result = new SqlParameter[parNames.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new SqlParameter("@"+parNames[i], parValues[i]);
            }
            return (DbParameter[])result;
        }

        public static DbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
