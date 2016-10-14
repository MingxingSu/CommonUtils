using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MaxSu.Framework.Common.Xml
{
    public static class XsltTransformer
    {
        /// <summary>
        /// </summary>
        /// <param name="stmXsltFile"></param>
        /// <param name="strXmlContent"></param>
        /// <param name="xsltArgumentList"></param>
        /// <returns></returns>
        public static Stream Transform(Stream stmXsltFile, string strXmlContent,
                                       XsltArgumentList xsltArgumentList = null)
        {
            xsltArgumentList = xsltArgumentList ?? new XsltArgumentList();

            XmlTextReader xmlReader = null;
            StringReader objStringReader = null;

            try
            {
                // Opens the XSLTFile and put the content in an XSLTransform object
                xmlReader = new XmlTextReader(stmXsltFile);

                var xsltFile = new XslCompiledTransform();
                xsltFile.Load(xmlReader);

                var objOutputStream = new MemoryStream();
                var xmlWriter = new StreamWriter(objOutputStream, Encoding.UTF8);

                objStringReader = new StringReader(strXmlContent);
                xmlReader = new XmlTextReader(objStringReader);
                var xmlPathDoc = new XPathDocument(xmlReader);

                // Transform the data and send the output to the console.
                xsltFile.Transform(xmlPathDoc, xsltArgumentList, xmlWriter);

                return objOutputStream;
            }
            finally
            {
                if (objStringReader != null)
                {
                    objStringReader.Close();
                }

                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }
    }
}