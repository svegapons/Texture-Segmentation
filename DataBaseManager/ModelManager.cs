using System;
using System.Collections.Generic;
using System.Text;
using DataBaseManager.Helpers;
using System.Data;
using System.Data.Common;

namespace DataBaseManager
{
    public class ModelManager
    {
        public ModelManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        string connectionString = "";
        string cmt = "";
        DbParameter[] pars = null;

        public void AddModel(string abbreviation, string modelName, string paramTypes, string paramNames)
        {
            cmt = "INSERT INTO MODELINFO (ABBREVIATION, MODELNAME, PARAMTYPES, PARAMNAMES) VALUES (" + DbHelper.GetParameterList("ABBREVIATION", "MODELNAME", "PARAMTYPES", "PARAMNAMES") + ")";
            pars = DbHelper.CreateParameters(new string[] { "ABBREVIATION", "MODELNAME", "PARAMTYPES", "PARAMNAMES" }, new object[] { abbreviation, modelName, paramTypes, paramNames });
            DbHelper.ExecuteNonQuery(connectionString, CommandType.Text, cmt, pars);
        }

        public void RemoveModel(string abbreviation)
        {
            cmt = "DELETE FROM MODELINFO WHERE " + DbHelper.GetConditionParameters("ABBREVIATION");
            pars = DbHelper.CreateParameters(new string[] { "ABBREVIATION" }, new object[] { abbreviation });
            DbHelper.ExecuteNonQuery(connectionString, CommandType.Text, cmt, pars);
        }

        public string[] GetParametersTypes(string abbreviation)
        {
            cmt = "SELECT PARAMTYPES FROM MODELINFO WHERE " + DbHelper.GetConditionParameters("ABBREVIATION");
            pars = DbHelper.CreateParameters(new string[] { "ABBREVIATION" }, new object[] { abbreviation });
            DataSet ds = DbHelper.ExecuteDataset(connectionString, CommandType.Text, cmt, pars);

            DataTable result = ds.Tables[0];
            if (result.Rows.Count == 0)
                return null;
            else
                return ((string)result.Rows[0][0]).Split('_');
        }

        public string[] GetParametersNames(string abbreviation)
        {
            cmt = "SELECT PARAMNAMES FROM MODELINFO WHERE " + DbHelper.GetConditionParameters("ABBREVIATION");
            pars = DbHelper.CreateParameters(new string[] { "ABBREVIATION" }, new object[] { abbreviation });
            DataSet ds = DbHelper.ExecuteDataset(connectionString, CommandType.Text, cmt, pars);

            DataTable result = ds.Tables[0];
            if (result.Rows.Count == 0)
                return null;
            else
                return ((string)result.Rows[0][0]).Split('_');
        }

        public bool Contains(string abbreviation)
        {
            cmt = "SELECT MODELNAME FROM MODELINFO WHERE " + DbHelper.GetConditionParameters("ABBREVIATION");
            pars = DbHelper.CreateParameters(new string[] { "ABBREVIATION" }, new object[] { abbreviation });
            DataSet ds = DbHelper.ExecuteDataset(connectionString, CommandType.Text, cmt, pars);

            DataTable result = ds.Tables[0];
            return !(result.Rows.Count == 0);
               
        }
     
    }
}
