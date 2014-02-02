using System;
using System.Collections;
using System.Data;
using System.Xml; 
using System.Data.SqlClient;

namespace DataBaseManager.Helpers
{
    public sealed class SqlHelper
    {
        #region "private utility methods & constructors"

        /// Since this class provides only static methods, make the default constructor private to prevent 
        /// instances from being created with "new SqlHelper()".*/

        private SqlHelper()
        {
        }


        /// This method is used to attach array of SqlParameters to a SqlCommand.
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// Parameters:
        /// -command - The command to which the parameters will be added
        /// -commandParameters - an array of SqlParameters tho be added to command*/

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            foreach (SqlParameter p in commandParameters)
            {
                if (p.Direction == ParameterDirection.InputOutput & p.Value == null)
                {
                    p.Value = null;
                }
                command.Parameters.Add(p);
            }
        }

        /// This method assigns an array of values to an array of SqlParameters.
        /// Parameters:
        /// -commandParameters - array of SqlParameters to be assigned values
        /// -array of objects holding the values to be assigned
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) & (parameterValues == null))
            {
                return;
            }
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }
            int j = commandParameters.Length - 1;
            for (int i = 0; i <= j; i++)
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }

        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command.
        /// Parameters:
        /// -command - the SqlCommand to be prepared
        /// -connection - a valid SqlConnection, on which to execute this command
        /// -transaction - a valid SqlTransaction, or 'null'
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// -commandParameters - an array of SqlParameters to be associated with the command or 'null' if no parameters are required
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (!((transaction == null)))
            {
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (!((commandParameters == null)))
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }
        #endregion

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
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
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
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                cn.Open();
                return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
            }
            finally
            {
                cn.Dispose();
            }
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
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
        /// e.g.:  
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
        /// Parameters:
        /// -connection - a valid SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: an int representing the number of rows affected by the command
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, null);
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
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            int retval;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
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
        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlTransaction.
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
        /// Parameters:
        /// -transaction - a valid SqlTransaction associated with the connection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: an int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, null);
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
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            int retval;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
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
        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
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
            return ExecuteDataset(connectionString, commandType, commandText, null);
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
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                cn.Open();
                return ExecuteDataset(cn, commandType, commandText, commandParameters);
            }
            finally
            {
                cn.Dispose();
            }
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
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connection - a valid SqlConnection
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, null);
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
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
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
        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction. 
        /// e.g.:  
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        /// Parameters
        /// -transaction - a valid SqlTransaction
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command
        /// Returns: a dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, null);
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
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
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
        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region "ExecuteReader"
        /// this enum is used to indicate whether the connection was provided by the caller, or created by SqlHelper, so that
        /// we can set the appropriate CommandBehavior when calling ExecuteReader()
        private enum SqlConnectionOwnership
        {
            //Connection is owned and managed by SqlHelper
            Internal,
            //Connection is owned and managed by the caller
            External
        }

        /// Create and prepare a SqlCommand, and call ExecuteReader with the appropriate CommandBehavior.
        /// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        /// If the caller provided the connection, we want to leave it to them to manage.
        /// Parameters:
        /// -connection - a valid SqlConnection, on which to execute this command 
        /// -transaction - a valid SqlTransaction, or 'null' 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParameters to be associated with the command or 'null' if no parameters are required 
        /// -connectionOwnership - indicates whether the connection parameter was provided by the caller, or created by SqlHelper 
        /// Returns: SqlDataReader containing the results of the command 
        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);
            if (connectionOwnership == SqlConnectionOwnership.External)
            {
                dr = cmd.ExecuteReader();
            }
            else
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            cmd.Parameters.Clear();
            return dr;
        }

        /* Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
                ' the connection string. 
                ' e.g.:  
                ' Dim dr As SqlDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders")
                ' Parameters:
                ' -connectionString - a valid connection string for a SqlConnection 
                ' -commandType - the CommandType (stored procedure, text, etc.) 
                ' -commandText - the stored procedure name or T-SQL command 
                ' Returns: a SqlDataReader containing the resultset generated by the command */
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, ((SqlParameter)(null)));
        }

        /* Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
                ' using the provided parameters.
                ' e.g.:  
                ' Dim dr As SqlDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
                ' Parameters:
                ' -connectionString - a valid connection string for a SqlConnection 
                ' -commandType - the CommandType (stored procedure, text, etc.) 
                ' -commandText - the stored procedure name or T-SQL command 
                ' -commandParameters - an array of SqlParamters used to execute the command 
                ' Returns: a SqlDataReader containing the resultset generated by the command */
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            cn.Open();
            try
            {
                return ExecuteReader(cn, ((SqlTransaction)(null)), commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                cn.Dispose();
            }
            return null;
        }

        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// SqlDataAdapter dr = ExecuteReader(connString, "GetOrders", 24, 36)
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: a SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// e.g.:  
        /// SqlDataAdapter dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: a SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, ((SqlParameter)(null)));
        }

        /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// e.g.:  
        /// SqlDataAdapter dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: a SqlDataReader containing the resultset generated by the command */
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(connection, ((SqlTransaction)(null)), commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36)
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: a SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction.
        /// e.g.:  
        /// SqlDataAdapter dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -transaction - a valid SqlTransaction  
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: a SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, ((SqlParameter)(null)));
        }

        /// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// SqlDataAdapter dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -transaction - a valid SqlTransaction 
        /// -commandType - the CommandType (stored procedure, text, etc.)
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: a SqlDataReader containing the resultset generated by the command 
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// SqlDataAdapter dr = ExecuteReader(trans, "GetOrders", 24, 36)
        /// Parameters:
        /// -transaction - a valid SqlTransaction 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure
        /// Returns: a SqlDataReader containing the resultset generated by the command
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

        #region "ExecuteScalar"

        /// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
        /// the connection string. 
        /// e.g.:  
        ///  int orderCount = CInt(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount"))
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, ((SqlParameter)(null)));
        }

        /// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// e.g.:  
        /// int orderCount = Cint(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                cn.Open();
                return ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
            finally
            {
                cn.Dispose();
            }
        }

        /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
        /// the connection string using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int orderCount = CInt(ExecuteScalar(connString, "GetOrderCount", 24, 36))
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
        /// e.g.:  
        /// int orderCount = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount"))
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, ((SqlParameter)(null)));
        }

        /// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// e.g.:  
        /// int orderCount = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            object retval;
            PrepareCommand(cmd, connection, ((SqlTransaction)(null)), commandType, commandText, commandParameters);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int orderCount = CInt(ExecuteScalar(conn, "GetOrderCount", 24, 36))
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlTransaction.
        /// e.g.:  
        /// int orderCount = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount"))
        /// Parameters:
        /// -transaction - a valid SqlTransaction 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, ((SqlParameter)(null)));
        }

        /// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// Dim orderCount As Integer = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24)))
        /// Parameters:
        /// -transaction - a valid SqlTransaction  
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            object retval;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        /// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// int orderCount = CInt(ExecuteScalar(trans, "GetOrderCount", 24, 36))
        /// Parameters:
        /// -transaction - a valid SqlTransaction 
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion

        #region "ExecuteXmlReader"

        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
        /// e.g.:  
        /// XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        /// Returns: an XmlReader containing the resultset generated by the command 
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(connection, commandType, commandText, ((SqlParameter)(null)));
        }

        /// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameters.
        /// e.g.:  
        /// XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: an XmlReader containing the resultset generated by the command 
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            XmlReader retval;
            PrepareCommand(cmd, connection, ((SqlTransaction)(null)), commandType, commandText, commandParameters);
            retval = cmd.ExecuteXmlReader();
            cmd.Parameters.Clear();
            return retval;
        }

        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36)
        /// Parameters:
        /// -connection - a valid SqlConnection 
        /// -spName - the name of the stored procedure using "FOR XML AUTO" 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: an XmlReader containing the resultset generated by the command */
        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlTransaction
        /// e.g.:  
        /// XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders")
        /// Parameters:
        /// -transaction - a valid SqlTransaction
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        /// Returns: an XmlReader containing the resultset generated by the command 
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteXmlReader(transaction, commandType, commandText, null);
        }

        /// Execute a SqlCommand (that returns a resultset) against the specified SqlTransaction
        /// using the provided parameters.
        /// e.g.:  
        /// XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24))
        /// Parameters:
        /// -transaction - a valid SqlTransaction
        /// -commandType - the CommandType (stored procedure, text, etc.) 
        /// -commandText - the stored procedure name or T-SQL command using "FOR XML AUTO" 
        /// -commandParameters - an array of SqlParamters used to execute the command 
        /// Returns: an XmlReader containing the resultset generated by the command
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            XmlReader retval;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            retval = cmd.ExecuteXmlReader();
            cmd.Parameters.Clear();
            return retval;
        }

        /// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction 
        /// using the provided parameter values.  This method will discover the parameters for the 
        /// stored procedure, and assign the values based on parameter order.
        /// This method provides no access to output parameters or the stored procedure's return value parameter.
        /// e.g.:  
        /// XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36)
        /// Parameters:
        /// -transaction - a valid SqlTransaction
        /// -spName - the name of the stored procedure 
        /// -parameterValues - an array of objects to be assigned as the input values of the stored procedure 
        /// Returns: a dataset containing the resultset generated by the command
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            SqlParameter[] commandParameters;
            if (!((parameterValues == null)) & parameterValues.Length > 0)
            {
                commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                AssignParameterValues(commandParameters, parameterValues);
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        #endregion

    }

    /// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
    /// ability to discover parameters for stored procedures at run-time.
    public sealed class SqlHelperParameterCache
    {
        #region "Private methods, variables, and constructors"
        /// Since this class provides only static methods, make the default constructor private to prevent 
        /// instances from being created with "new SqlHelperParameterCache()".

        private SqlHelperParameterCache()
        {
        }

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// Parameters:
        /// - connectionString - a valid connection string for a SqlConnection
        /// - spName - the name of the stored procedure
        /// - includeReturnValueParameter - whether or not to include their return value parameter>
        /// Returns: SqlParameter() 
        private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter, params object[] parameterValues)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(spName, cn);
            SqlParameter[] discoveredParameters;
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(cmd);
                if (!(includeReturnValueParameter))
                {
                    cmd.Parameters.RemoveAt(0);
                }
                discoveredParameters = new SqlParameter[cmd.Parameters.Count - 1];
                cmd.Parameters.CopyTo(discoveredParameters, 0);
            }
            finally
            {
                cmd.Dispose();
                cn.Dispose();
            }
            return discoveredParameters;
        }

        /// deep copy of cached SqlParameter array
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            int j = originalParameters.Length - 1;
            SqlParameter[] clonedParameters = new SqlParameter[j];
            for (int i = 0; i <= j; i++)
            {
                clonedParameters[i] = ((SqlParameter)(((ICloneable)(originalParameters[i])).Clone()));
            }
            return clonedParameters;
        }

        #endregion

        #region "caching functions"
        /// add parameter array to the cache
        /// Parameters
        /// -connectionString - a valid connection string for a SqlConnection 
        /// -commandText - the stored procedure name or T-SQL command 
        /// -commandParameters - an array of SqlParamters to be cached */
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;
            paramCache[hashKey] = commandParameters;
        }

        /// retrieve a parameter array from the cache
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection 
        /// -commandText - the stored procedure name or T-SQL command 
        /// Returns: an array of SqlParamters 
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;
            SqlParameter[] cachedParameters = ((SqlParameter[])(paramCache[hashKey]));
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }
        #endregion

        #region Parameter Discovery Functions
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        ///  
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection 
        /// -spName - the name of the stored procedure 
        /// Returns: an array of SqlParameters 
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        ///  
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// Parameters:
        /// -connectionString - a valid connection string for a SqlConnection
        /// -spName - the name of the stored procedure 
        /// -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results 
        /// Returns: an array of SqlParameters 
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] cachedParameters;
            string hashKey;
            hashKey = connectionString + ":" + spName + ((includeReturnValueParameter == true) ? ":include ReturnValue Parameter" : "");
            cachedParameters = ((SqlParameter[])(paramCache[hashKey]));
            if ((cachedParameters == null))
            {
                paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter);
                cachedParameters = ((SqlParameter[])(paramCache[hashKey]));
            }
            return CloneParameters(cachedParameters);
        }
        #endregion

    }
}