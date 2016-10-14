using System.IO;
using System.Xml.Serialization;

namespace MaxSu.Framework.Common.ResourceManager
{
    internal class ResourcesSerializer
    {
        public static Resources DeSerialize(string filePath)
        {
            var XmlSerializer = new XmlSerializer(typeof (Resources));
            var FileStream = new FileStream(filePath, FileMode.Open);
            var Resources = XmlSerializer.Deserialize(FileStream) as Resources;
            FileStream.Close();
            return Resources;
        }

        public static void Serialize(string filePath, Resources Resources)
        {
            var XmlSerializer = new XmlSerializer(typeof (Resources));
            var FileStream = new FileStream(filePath, FileMode.Create);
            XmlSerializer.Serialize(FileStream, Resources);
            FileStream.Close();
        }
    }
}