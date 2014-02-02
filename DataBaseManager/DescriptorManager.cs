using System;
using System.Collections.Generic;
using System.Text;
using DataBaseManager.Helpers;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SqlClient;

namespace DataBaseManager 
{
    public class DescriptorManager
    {
        public DescriptorManager(string connectionString)
        {
            this.connectionString = connectionString;
            modelManager=new ModelManager(connectionString);
            imageManager=new ImageManager(connectionString);
        }

        string connectionString = "";
        string cmt = "";
        DbParameter[] pars = null;
        ModelManager modelManager =null;
        ImageManager imageManager=null;


        private string[] GetParamNames(string model,bool matrix)
        {
            string[] pn = modelManager.GetParametersNames(model);
            List<string> aux = new List<string>();
            aux.Add("HASH");
            aux.AddRange(pn);
            aux.Add("FEATURE");
            if(matrix)
               aux.Add("MATRIX");
            return aux.ToArray();
        }

        private string GetPNList(string[] pars)
        {
            string result = pars[0];
            for (int i = 1; i < pars.Length; i++)
                result += ", " + pars[i];
            return result;
        }

        private object[] GetParamValues(string hash, string feature, object[] parameters, byte[] matrix)
        {
            List<object> aux = new List<object>();
            aux.Add(hash);
            aux.AddRange(parameters);
            aux.Add(feature);
            if(matrix!=null)
               aux.Add(matrix);
            return aux.ToArray();
        }

        public int AddDescriptorApplication(string modelAbbreviation, string featureAbbreviation, string hash, byte[] image, object[] parameters, byte[] result)
        {
            //MemoryStream ms = new MemoryStream();
            //BinaryFormatter bf = new BinaryFormatter();
            //bf.Serialize(ms, result);

            int res = 0;

            byte[] matrix = result;
            string[] pn = GetParamNames(modelAbbreviation,true);
            object[] pv = GetParamValues(hash, featureAbbreviation, parameters, matrix);

            DbConnection conn = DbHelper.GetConnection(connectionString);
            conn.Open();
            DbTransaction trans = conn.BeginTransaction();

            try
            {
                if (imageManager.GetImage(hash) == null)
                    imageManager.AddImage(hash, image, "Primer descriptor aplicado " + modelAbbreviation + "_" + featureAbbreviation, trans);

                cmt = "INSERT INTO Desc_" + modelAbbreviation + " (" + GetPNList(pn) + ") VALUES (" + DbHelper.GetParameterList(pn) + ")";
                pars = DbHelper.CreateParameters(pn, pv);
                res = DbHelper.ExecuteNonQuery(trans, CommandType.Text, cmt, pars);
                trans.Commit();
                
                conn.Close();            

            }
            catch (Exception ex)
            {
                trans.Rollback();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw ex;
            }

            return res;
        }




        public byte[] GetDescriptorApplication(string modelAbbreviation, string featureAbbreviation, string hash, object[] parameters)
        {
            if (!modelManager.Contains(modelAbbreviation))
                return null;

            string[] pn = GetParamNames(modelAbbreviation,false);
            object[] pv = GetParamValues(hash, featureAbbreviation, parameters,null);

            cmt = "SELECT MATRIX FROM DESC_" + modelAbbreviation + " WHERE " + DbHelper.GetConditionParameters(pn);
            pars = DbHelper.CreateParameters(pn, pv);
            DataSet ds = DbHelper.ExecuteDataset(connectionString, CommandType.Text, cmt, pars);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                return null;
           
            //MemoryStream ms = new MemoryStream();
            //BinaryFormatter bf = new BinaryFormatter();
            return (byte[])dt.Rows[0][0];
        }



        public void AddDescriptorApplication(string abbreviation, string hash, byte[] image, object[] parameters, byte[] result)
        {
            string[] arr = abbreviation.Split('_');
            AddDescriptorApplication(arr[0], arr[1], hash, image, parameters, result);
        }

        public byte[] GetDescriptorApplication(string abbreviation, string hash, object[] parameters)
        {
            string[] arr = abbreviation.Split('_');
            return GetDescriptorApplication(arr[0], arr[1], hash, parameters);
        }


    }
}
