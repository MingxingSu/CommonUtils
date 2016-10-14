using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MaxSu.Framework.Common
{
    public sealed class DatabaseLibrary
    {
        private DatabaseLibrary()
        {
        }

        public static string FormatMySQLDate(object objDate)
        {
            string str;
            try
            {
                if (Information.IsDBNull(RuntimeHelpers.GetObjectValue(objDate)))
                {
                    return "null";
                }
                DateTime time = Convert.ToDateTime(RuntimeHelpers.GetObjectValue(objDate));
                string str2 = (Conversions.ToString(time.Year) + "-" + Conversions.ToString(time.Month) + "-" +
                               Conversions.ToString(time.Day)) + " ";
                str = str2 + Conversions.ToString(time.Hour) + ":" + Conversions.ToString(time.Minute) + ":" +
                      Conversions.ToString(time.Second);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        public static string FormatSQLParameter(object objValue, string strType)
        {
            string str;
            try
            {
                str = FormatSQLParameter(RuntimeHelpers.GetObjectValue(objValue), strType, true);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                throw;
            }
            return str;
        }

        public static string FormatSQLParameter(object objValue, string strType, bool boolAddQuote)
        {
            string str;
            try
            {
                string str2;
                if (Information.IsDBNull(RuntimeHelpers.GetObjectValue(objValue)))
                {
                    return "null";
                }
                string str3 = Strings.LCase(strType);
                switch (str3)
                {
                    case "boolean":
                        if (Versioned.IsNumeric(RuntimeHelpers.GetObjectValue(objValue)))
                        {
                            return Convert.ToString(RuntimeHelpers.GetObjectValue(objValue));
                        }
                        return "0";

                    case "byte":
                    case "currency":
                    case "double":
                    case "integer":
                    case "long":
                    case "single":
                    case "int":
                    case "autonumber":
                        if (Versioned.IsNumeric(RuntimeHelpers.GetObjectValue(objValue)))
                        {
                            return Convert.ToString(RuntimeHelpers.GetObjectValue(objValue));
                        }
                        return "0";

                    case "string":
                    case "varchar":
                    case "char":
                    case "text":
                        str2 =
                            Strings.Replace(
                                Strings.Replace(Convert.ToString(RuntimeHelpers.GetObjectValue(objValue)), "'", "''", 1,
                                                -1, CompareMethod.Binary), @"\", @"\\", 1, -1, CompareMethod.Binary);
                        if (boolAddQuote)
                        {
                            str2 = "'" + str2 + "'";
                        }
                        return str2;
                }
                if ((str3 != "date") && (str3 != "datetime"))
                {
                    throw new Exception("FormatSQLParameter - Type: " + strType + " unsupported.");
                }
                str2 = FormatMySQLDate(RuntimeHelpers.GetObjectValue(objValue));
                if (boolAddQuote)
                {
                    str2 = "'" + str2 + "'";
                }
                str = str2;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                throw;
            }
            return str;
        }

        /// <summary>
        ///
        /// </summary>
        public static DataSet DataTableToDataSet(DataTable dv, string strTableName)
        {

            DataTable dtTemp = null;
            DataRow drv = null;
            DataSet dsTemp = null;

            try
            {

                //// Copy the table
                dtTemp = dv.Clone();

                //// Clone the structure of the table behind the view
                dtTemp.TableName = strTableName;

                //// Populate the table with rows in the view
                foreach (DataRow drv_loopVariable in dv.Rows)
                {
                    drv = drv_loopVariable;
                    dtTemp.ImportRow(drv);
                }

                dsTemp = new DataSet(dv.TableName);

                //// Add the new table to a DataSet
                dsTemp.Tables.Add(dtTemp);

                return dsTemp;

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        ///
        /// </summary>
        public static DataSet CopyDataRowToNewDataSet(DataRow drRow)
        {

            DataTable dtTemp = null;
            DataSet dsTemp = null;

            try
            {
                dtTemp = drRow.Table.Clone();

                dtTemp.ImportRow(drRow);

                dsTemp = new DataSet(dtTemp.TableName);

                //// Add the new table to a DataSet
                dsTemp.Tables.Add(dtTemp);

                return dsTemp;

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public static DataSet FilterDataTableAsNewDataset(DataTable dtTable, string strFilter, string strSort = "")
        {

            DataView dvDataView = null;


            try
            {
                dvDataView = new DataView(dtTable);
                dvDataView.RowFilter = strFilter;
                if (strSort != "")
                {
                    dvDataView.Sort = strSort;
                }

                return DataViewToDataSet(dvDataView, "Table");

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static DataSet DataViewToDataSet(DataView dv, string strTableName)
        {

            DataTable dtTemp = null;
            DataRowView drv = null;
            DataSet dsTemp = null;


            try
            {
                //// Copy the table
                dtTemp = dv.Table.Clone();

                //// Clone the structure of the table behind the view
                dtTemp.TableName = strTableName;

                //// Populate the table with rows in the view
                foreach (DataRowView drv_loopVariable in dv)
                {
                    drv = drv_loopVariable;
                    dtTemp.ImportRow(drv.Row);
                }

                dsTemp = new DataSet(dv.Table.TableName);

                //// Add the new table to a DataSet
                dsTemp.Tables.Add(dtTemp);

                return dsTemp;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static void DataTablesToXML(string strFileName, DataTable d1, DataTable d2)
        {
            DataSet dsNewDS1 = d1.DataSet;
            DataSet dsNewDS2 = d2.DataSet;
            if (dsNewDS1 == null)
            {
                dsNewDS1 = new DataSet();
                dsNewDS1.Tables.Add(d1);
            }
            if (dsNewDS2 == null)
            {
                dsNewDS2 = new DataSet();
                dsNewDS2.Tables.Add(d2);
            }

            string xmlDS1 = Xml.XmlHelper.Dataset2XML(dsNewDS1);
            string xmlDS2 = Xml.XmlHelper.Dataset2XML(dsNewDS2);

            xmlDS1 = xmlDS1.Replace("&#x16;", "");
            xmlDS2 = xmlDS2.Replace("&#x16;", "");

            xmlDS1 = xmlDS1.Replace("Tablas", "Table");
            xmlDS2 = xmlDS2.Replace("Tablas", "Table");

            xmlDS1 = xmlDS1.Replace("<Name>", "<NAME>");
            xmlDS1 = xmlDS1.Replace("</Name>", "</NAME>");

            xmlDS1 = xmlDS1.Replace("<Solution>", "<NewDataSet>");
            xmlDS1 = xmlDS1.Replace("</Solution>", "</NewDataSet>");

            xmlDS1 = xmlDS1.Replace("<Date>", "<DATE>");
            xmlDS1 = xmlDS1.Replace("</Date>", "</DATE>");

            XDocument xDoc1 = XDocument.Parse(xmlDS1);
            XDocument xDoc2 = XDocument.Parse(xmlDS2);

            xDoc1.Save("C:/temp/SqlExtraction/" + strFileName + "-1.xml");
            xDoc2.Save("C:/temp/SqlExtraction/" + strFileName + "-2.xml");
        }
    }
}