using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;


namespace TxEstudioApplication
{

    public partial class Environment
    {

        AppDomain addOnsDomain;
        LinkedList<AddOn> addOns = new LinkedList<AddOn>();
        public void LoadAddOns()
        {
            addOnsDomain = AppDomain.CreateDomain("add-ons");
            string addOnsDirectory = Application.StartupPath + Properties.Settings.Default.AddOnsPath;
            if (Directory.Exists(addOnsDirectory))
            {
                foreach (string file in Directory.GetFiles(addOnsDirectory, "*.dll"))
                {
#warning Hay que cargarlos en otro appdomain
                    try
                    {
                       
                        foreach (Type type in Assembly.LoadFile(file).GetExportedTypes())
                        {
                            if (type.IsSubclassOf(typeof(AddOn)))
                            {
                                addOns.AddLast((AddOn)type.GetConstructor(new Type[] { typeof(Environment) }).Invoke(new object[] { this }));
                                mainForm.addOnsMenuItem.DropDownItems.Add(addOns.Last.Value.GetAddOnMenu());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //pass
                    }
                }
            }
        }
    }
}