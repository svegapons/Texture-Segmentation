using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Reflection;
using System.IO;
using DataBaseManager.Helpers;
//using DataBaseCommunication.Oracle;
//using DataBaseCommunication.SQL_Server;
//using DataBaseCommunication.SQL_Server.Sql_txEstudioDataSetTableAdapters;

namespace DataBaseManager
{
    public class DBManager
    {
        public DBManager()
        {
          
            DbHelper.HelperName = global::DataBaseManager.Properties.Settings.Default.HelperName;
            connectionString = global::DataBaseManager.Properties.Settings.Default.ConnectionString;

            imageManager = new ImageManager(connectionString);
            modelManager = new ModelManager(connectionString);
            descriptorManager = new DescriptorManager(connectionString);
        }

       #region Variables

        private ModelManager modelManager = null;
        private ImageManager imageManager = null;
        private DescriptorManager descriptorManager = null;
        protected string connectionString = "";

       #endregion


        public string ConnectionString
        {
            get { return connectionString; }
        }
    

        public ModelManager ModelManager
        {
            get
            {
                return modelManager;
            }
        }

        public DescriptorManager DescriptorManager
        {
            get
            {
                return descriptorManager;
            }
        }

        public ImageManager ImageManager
        {
            get
            {
                return imageManager;
            }
        }
    }

  //  public enum DBPType { SQL_Server, Oracle};
}
