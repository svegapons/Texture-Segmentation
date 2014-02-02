using System;
using DataBaseManager;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TxEstudioApplication
{

    
    public partial class Environment
    {
        DBManager dbManager;

        public void AddDescriptorApplication(TextureDescriptor descriptor, TxImage image, TxMatrix description )
        {
            if (Properties.Settings.Default.UseDatabase)
            {
                MemoryStream imageMemory = new MemoryStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(imageMemory, image);
                dbManager.DescriptorManager.AddDescriptorApplication(AbbreviationAttribute.GetAbbreviation((TxAlgorithm)descriptor), image.GetHash(), imageMemory.ToArray(), AbbreviationAttribute.GetParameters((TxAlgorithm)descriptor), description.ToByteArray());
            }
        }

        public TxMatrix GetDescriptorApplication(TextureDescriptor descriptor, TxImage image)
        {
            if (Properties.Settings.Default.UseDatabase)
            {
                byte[] result = dbManager.DescriptorManager.GetDescriptorApplication(AbbreviationAttribute.GetAbbreviation((TxAlgorithm)descriptor), image.GetHash(), AbbreviationAttribute.GetParameters((TxAlgorithm)descriptor));
                if (result != null)
                {
                    //BinaryFormatter binaryFormatter = new BinaryFormatter();
                    //MemoryStream descriptionMemory = new MemoryStream(result);
                    return TxMatrix.FromByteArray(result);
                    //return new TxMatrix((float[,])binaryFormatter.Deserialize(descriptionMemory));
                }
            }
            return null;
        }
        


        

        
    }
}
