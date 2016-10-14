using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MaxSu.Framework.Common
{
    public class SerializeHelper
    {
        #region XML序列化

        /// <summary>
        ///     文件化XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        ///     文件化XML反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        ///     文本化XML序列化
        /// </summary>
        /// <param name="item">对象</param>
        public string ToXml<T>(T item)
        {
            var serializer = new XmlSerializer(item.GetType());
            var sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, item);
                return sb.ToString();
            }
        }

        /// <summary>
        ///     文本化XML反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public T FromXml<T>(string str)
        {
            var serializer = new XmlSerializer(typeof (T));
            using (XmlReader reader = new XmlTextReader(new StringReader(str)))
            {
                return (T) serializer.Deserialize(reader);
            }
        }

        #endregion

        #region Json序列化

        /// <summary>
        ///     JsonSerializer序列化
        /// </summary>
        /// <param name="item">对象</param>
        public string ToJson<T>(T item)
        {
            var serializer = new DataContractJsonSerializer(item.GetType());
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        ///     JsonSerializer反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public T FromJson<T>(string str) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof (T));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                return serializer.ReadObject(ms) as T;
            }
        }

        #endregion

        #region SoapFormatter序列化

        /// <summary>
        ///     SoapFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public string ToSoap<T>(T item)
        {
            var formatter = new SoapFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(ms);
                return xmlDoc.InnerXml;
            }
        }

        /// <summary>
        ///     SoapFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public T FromSoap<T>(string str)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            var formatter = new SoapFormatter();
            using (var ms = new MemoryStream())
            {
                xmlDoc.Save(ms);
                ms.Position = 0;
                return (T) formatter.Deserialize(ms);
            }
        }

        #endregion

        #region BinaryFormatter序列化

        /// <summary>
        ///     BinaryFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public string ToBinary<T>(T item)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                byte[] bytes = ms.ToArray();
                var sb = new StringBuilder();
                foreach (byte bt in bytes)
                {
                    sb.Append(string.Format("{0:X2}", bt));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        ///     BinaryFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public T FromBinary<T>(string str)
        {
            int intLen = str.Length/2;
            var bytes = new byte[intLen];
            for (int i = 0; i < intLen; i++)
            {
                int ibyte = Convert.ToInt32(str.Substring(i*2, 2), 16);
                bytes[i] = (byte) ibyte;
            }
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                return (T) formatter.Deserialize(ms);
            }
        }

        #endregion

        #region 调用示例
        //[Serializable]
        //public class Car
        //{
        //    private string _Owner;
        //    private string _Price;
        //    private string m_filename;

        //    public Car(string o, string p)
        //    {
        //        Price = p;
        //        Owner = o;
        //    }

        //    public Car()
        //    {
        //    }

        //    [XmlElement(ElementName = "Price")]
        //    public string Price
        //    {
        //        get { return _Price; }
        //        set { _Price = value; }
        //    }

        //    [XmlElement(ElementName = "Owner")]
        //    public string Owner
        //    {
        //        get { return _Owner; }
        //        set { _Owner = value; }
        //    }

        //    public string Filename
        //    {
        //        get { return m_filename; }
        //        set { m_filename = value; }
        //    }
        //}

        //#endregion

        //public class Demo
        //{
        //    public void DemoFunction()
        //    {
        //        //序列化
        //        var car = new Car("chenlin", "120万");

        //        Serialize.SoapSerialize("Soap序列化", car);
        //        Serialize.XmlSerialize("XML序列化", car);
        //        

        //        var car2 = (Car) Serialize.BinaryDeserialize("Binary序列化");
        //        car2 = (Car) Serialize.SoapDeserialize("Soap序列化");
        //        car2 = (Car) Serialize.XmlDeserailize("XML序列化");
        //    }
        

        #endregion
    }
}
