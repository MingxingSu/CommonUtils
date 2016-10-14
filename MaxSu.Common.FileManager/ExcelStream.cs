using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace MaxSu.Common.FileManager
{
    public static class ExcelStream
    {

        public static byte[] ExportExcelToByte(DataSet export, Array columnNames, Array columnHeaderNames, char seperator = ',')
        {
            ExcelPackage pck = null;
            OfficeOpenXml.ExcelWorkbook wb;
            ExcelWorksheet ws;
            byte[] result;
            int count = 2;
            int i = 0;

            try
            {
                // We start making the excel package
                pck = new ExcelPackage();

                // Translated from Tecncocom :
                // This excel template generates an error when it is loaded for the first time.
                // After searching on the web, the solution that I have found consists in loading the template a second time.
                // Besides altering/managing the library, I don't see any other solutions.
                try
                {
                    wb = pck.Workbook;
                }
                catch
                {
                    wb = pck.Workbook;
                }

                ws = wb.Worksheets.Add("List Results");

                for (i = 0; i < columnHeaderNames.Length; i++)
                {
                    ws.Cells[1, i + 1].Value = columnHeaderNames.GetValue(i);
                }

                // We set the data values
                foreach (DataRow dr in export.Tables[0].Rows)
                {
                    for (i = 1; i <= columnNames.Length; i++)
                    {
                        ws.Cells[count, i].Value = dr[columnNames.GetValue(i - 1).ToString()];
                    }

                    count++;
                }

                FormatCellsForDataType(ws, columnNames, export.Tables[0]);

                result = pck.GetAsByteArray();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pck.Dispose();
            }
        }

        private static void FormatCellsForDataType(ExcelWorksheet ws, Array columnNames, DataTable dt)
        {
            ExcelRange er;

            int intDecimalsToDisplay = -1;

            //if(dt.Columns.Contains("DecimalsToDisplay") && dt.Rows.Count > 0)
            //{
            //    intDecimalsToDisplay = int.Parse(dt.Rows[0]["DecimalsToDisplay"].ToString());
            //}

            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= columnNames.Length; i++)
                {
                    // Format all cells that contain Datetime date
                    if (dt.Columns[columnNames.GetValue(i - 1).ToString()].DataType == System.Type.GetType("System.DateTime"))
                    {
                        er = ws.Cells[2, i, dt.Rows.Count + 1, i];
                        er.Style.Numberformat.Format = @"dd-MM-yyyy HH:mm";
                    }

                    // Format cells with decimal values.
                    if (dt.Columns[columnNames.GetValue(i - 1).ToString()].DataType == System.Type.GetType("System.Decimal"))
                    {
                        if (dt.Columns.Contains("DecimalsToDisplay"))
                        {
                            int intRowIndex = 1;
                            foreach (DataRow dr in dt.Rows)
                            {
                                intDecimalsToDisplay = int.Parse(dr["DecimalsToDisplay"].ToString());

                                if (intDecimalsToDisplay == 0)
                                {
                                    er = ws.Cells[intRowIndex + 1, i];
                                    er.Style.Numberformat.Format = @"#,##0";
                                }
                                else if (intDecimalsToDisplay == 1)
                                {
                                    er = ws.Cells[intRowIndex + 1, i];
                                    er.Style.Numberformat.Format = @"#,##0.0";
                                }
                                else if (intDecimalsToDisplay == 2)
                                {
                                    er = ws.Cells[intRowIndex + 1, i];
                                    er.Style.Numberformat.Format = @"#,##0.00";
                                }
                                else if (intDecimalsToDisplay == 3)
                                {
                                    er = ws.Cells[intRowIndex + 1, i];
                                    er.Style.Numberformat.Format = @"#,##0.000";
                                }

                                intRowIndex++;
                            }

                        }
                    }

                }
            }
        }
    }
}
