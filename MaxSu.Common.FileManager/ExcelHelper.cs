using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace MaxSu.Common.FileManager
{
    /// <summary>
    /// 数据导出工具类
    /// 前提：Microsoft Office 11.0 Object Library
    /// .NET Framework 4.0
    /// </summary>
    public class ExcelHelper
    {
        public static void Export2Xls(DataTable data, string filename, bool exportHeader = true)
        {
            if (System.IO.File.Exists(filename))
                System.IO.File.Delete(filename);

            Excel._Application xlsApp = null;
            Excel._Workbook xlsBook = null;
            Excel._Worksheet xstSheet = null;
            try
            {
                xlsApp = new Excel.Application();

                xlsBook = xlsApp.Workbooks.Add();
                xstSheet = (Excel._Worksheet)xlsBook.Worksheets[1];

                var buffer = new StringBuilder();
                if (exportHeader)
                {
                    // Excel中列与列之间按照Tab隔开
                    foreach (DataColumn col in data.Columns)
                        buffer.Append(col.ColumnName + "/t");

                    buffer.AppendLine();
                }
                foreach (DataRow row in data.Rows)
                {
                    foreach (DataColumn col in data.Columns)
                        buffer.Append(row[col].ToString() + "/t");

                    buffer.AppendLine();
                }
                System.Windows.Forms.Clipboard.SetDataObject("");
                // 放入剪切板
                System.Windows.Forms.Clipboard.SetDataObject(buffer.ToString());
                var range = (Excel.Range)xstSheet.Cells[1, 1];
                range.Select();
                xstSheet.Paste();
                // 清空剪切板
                System.Windows.Forms.Clipboard.SetDataObject("");

                xlsBook.SaveCopyAs(filename);
            }
            finally
            {
                if (xlsBook != null)
                    xlsBook.Close();

                if (xlsApp != null)
                    xlsApp.Quit();

                // finally里清空Com对象
                Marshal.ReleaseComObject(xlsApp);
                Marshal.ReleaseComObject(xlsBook);
                Marshal.ReleaseComObject(xstSheet);

                xstSheet = null;
                xlsBook = null;
                xlsApp = null;
            }
        }

        public static void Export2CSV(DataTable data, string filename, bool exportHeader = true)
        {
            if (File.Exists(filename))
                File.Delete(filename);

            var buffer = new StringBuilder();
            if (exportHeader)
            {
                for (var i = 0; i < data.Columns.Count; i++)
                {
                    buffer.AppendFormat("\"{0}\"", data.Columns[i].ColumnName);
                    if (i < data.Columns.Count - 1)
                        buffer.Append(",");
                }
                buffer.AppendLine();
            }

            for (var i = 0; i < data.Rows.Count; i++)
            {
                for (var j = 0; j < data.Columns.Count; j++)
                {
                    buffer.AppendFormat("\"{0}\"", data.Rows[i][j].ToString());
                    if (j < data.Columns.Count - 1)
                        buffer.Append(",");
                }
                buffer.AppendLine();
            }

            File.WriteAllText(filename, buffer.ToString(), Encoding.Default);
        }

        public static void Export2Xls<T>(List<T> data, string filename, bool exportHeader = true) 
        {
            if (File.Exists(filename))
                File.Delete(filename);

            Excel._Application xlsApp = null;
            Excel._Workbook xlsBook = null;
            Excel._Worksheet xstSheet = null;

            var type = typeof(T);
            var properties = type.GetProperties();
            var buffer = new StringBuilder();
            if (exportHeader)
            {
                xlsApp = new Excel.Application();

                xlsBook = xlsApp.Workbooks.Add();
                xstSheet = (Excel._Worksheet)xlsBook.Worksheets[1];
                
                if (exportHeader)
                {
                    foreach (var property in properties)
                        buffer.Append(property.Name + "/t");

                    buffer.AppendLine();
                }
                foreach (var row in data)
                {
                    foreach (var property in properties)
                        buffer.Append(property.GetValue(row, null) + "/t");

                    buffer.AppendLine();
                }
                System.Windows.Forms.Clipboard.SetDataObject("");
                // 放入剪切板
                System.Windows.Forms.Clipboard.SetDataObject(buffer.ToString());
                var range = (Excel.Range)xstSheet.Cells[1, 1];
                range.Select();
                xstSheet.Paste();
                // 清空剪切板
                System.Windows.Forms.Clipboard.SetDataObject("");

                xlsBook.SaveCopyAs(filename);
            }
        }

        public static void Export2CSV<T>(List<T> data, string filename, bool exportHeader = true)
        {
            if (File.Exists(filename))
                File.Delete(filename);

            var type = typeof(T);
            var properties = type.GetProperties();
            var buffer = new StringBuilder();

            if (exportHeader)
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    buffer.AppendFormat("\"{0}\"", properties[i].Name);
                    if (i < properties.Length - 1)
                        buffer.Append(",");
                }
                buffer.AppendLine();
            }

            for (var i = 0; i < data.Count; i++)
            {
                for (var j = 0; j < properties.Length; j++)
                {
                    buffer.AppendFormat("\"{0}\"", properties[j].GetValue(data[i], null).ToString());
                    if (j < properties.Length - 1)
                        buffer.Append(",");
                }
                buffer.AppendLine();
            }

            File.WriteAllText(filename, buffer.ToString());
        }
    }
}