using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DataBaseManager.Helpers;
using System.Data;
using System.Data.Common;

namespace DataBaseManager
{
    public class ImageManager
    {
        public ImageManager(string connectionString)
        {
            this.connectionString = connectionString;
        }
        string connectionString = "";
        string cmt = "";
        DbParameter[] pars = null;

        public int AddImage(string hash, byte[] image, string description)
        {
           cmt="INSERT INTO IMAGE(HASH, IMAGE, DESCRIPTION) VALUES ("+DbHelper.GetParameterList("HASH","IMAGE","DESCRIPTION")+")";
           pars=DbHelper.CreateParameters(new string[]{"HASH","IMAGE","DESCRIPTION"},new object[]{hash,image,description});
           return DbHelper.ExecuteNonQuery(connectionString, CommandType.Text, cmt, pars);
        }

        public int AddImage(string hash, byte[] image, string description,DbTransaction transaction)
        {
            cmt = "INSERT INTO IMAGE(HASH, IMAGE, DESCRIPTION) VALUES (" + DbHelper.GetParameterList("HASH", "IMAGE", "DESCRIPTION") + ")";
            pars = DbHelper.CreateParameters(new string[] { "HASH", "IMAGE", "DESCRIPTION" }, new object[] { hash, image, description });
            return DbHelper.ExecuteNonQuery(transaction, CommandType.Text, cmt, pars);
        }

        public int RemoveImage(string hash)
        {
           cmt = "DELETE FROM IMAGE WHERE " + DbHelper.GetConditionParameters("HASH");
           pars = DbHelper.CreateParameters(new string[] { "HASH" }, new object[] { hash });
           return DbHelper.ExecuteNonQuery(connectionString, CommandType.Text, cmt, pars);            

        }

        public byte[] GetImage(string hash)
        {
            cmt = "SELECT IMAGE FROM IMAGE WHERE " + DbHelper.GetConditionParameters("HASH");
            pars = DbHelper.CreateParameters(new string[] { "HASH" }, new object[] { hash });
            DataSet ds = DbHelper.ExecuteDataset(connectionString, CommandType.Text, cmt, pars);
           
            DataTable result=ds.Tables[0];
            if (result.Rows.Count == 0)
                return null;
            else
                return (byte[])result.Rows[0][0];
        }

        public DataSet GetDataSet()
        {
            cmt = "SELECT * FROM IMAGE";
            return DbHelper.ExecuteDataset(connectionString, CommandType.Text, cmt);
        }


       
    }


}
