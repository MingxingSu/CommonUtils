using System.Xml.Serialization;

namespace MaxSu.Framework.Common.ResourceManager
{
    public class Item
    {
        [XmlAttribute("key")] public string key = string.Empty;
        [XmlText] public string value = string.Empty;
    }
}