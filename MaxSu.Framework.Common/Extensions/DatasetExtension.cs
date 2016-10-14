using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MaxSu.Framework.Common.Extensions
{
   public  static class DatasetExtension
    {
       public static DataRow GetRowFromTable(this DataSet dataSet, int tableIndex = 0, int rowIndex = 0)
       {
           if (dataSet == null || dataSet.Tables.Count <= tableIndex ||
               dataSet.Tables[tableIndex].Rows.Count <= rowIndex)
               return null;

           return dataSet.Tables[tableIndex].Rows[rowIndex];
       }
    }
}
