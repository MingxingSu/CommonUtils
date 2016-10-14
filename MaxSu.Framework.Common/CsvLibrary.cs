using System.Data;
using System.IO;
using System.Text;

namespace MaxSu.Framework.Common
{
    public static class CsvLibrary
    {
        /// <summary>
        ///     导出报表为Csv
        /// </summary>
        /// <param name="dtInput">DataTable</param>
        /// <param name="strFilePath">物理路径</param>
        /// <param name="tableHeader">表头</param>
        /// <param name="columName">字段标题,逗号分隔</param>
        public static bool DataTabkeToCsvFile(DataTable dtInput, string strFilePath, string tableHeader,
                                              string columName)
        {
            try
            {
                var strmWriterObj = new StreamWriter(strFilePath, false, Encoding.UTF8);
                strmWriterObj.WriteLine(tableHeader);
                strmWriterObj.WriteLine(columName);
                for (int i = 0; i < dtInput.Rows.Count; i++)
                {
                    string strBufferLine = "";
                    for (int j = 0; j < dtInput.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dtInput.Rows[i][j].ToString();
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        public static DataTable CsvToDatatable(string filePath, int n, DataTable dtOutput)
        {
            var reader = new StreamReader(filePath, Encoding.UTF8, false);
            int i = 0, m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    string[] split = str.Split(',');

                    DataRow dr = dtOutput.NewRow();
                    for (i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dtOutput.Rows.Add(dr);
                }
            }
            return dtOutput;
        }
    }
}