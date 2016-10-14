using System.Xml.Serialization;

namespace MaxSu.Framework.Common.ResourceManager
{
    public class Items
    {
        [XmlElement("item", typeof (Item))] public Item[] items;
    }
}