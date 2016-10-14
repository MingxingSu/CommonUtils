using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace MaxSu.Framework.Common.Excel
{
    /// <summary>
    ///     EXCEL连接与处理类，负责连接数据库、查询SQL和执行SQL
    /// </summary>
    public class ExcelConnect
    {
        #region Public Static 连接数据库

        /// <summary>
        ///     连接数据库
        /// </summary>
        /// <returns>连接成功返回TRUE，否则返回FALSE</returns>
        public static bool connectDatabase()
        {
            try
            {
                //数据库是否已经打开
                if (MyConnection.State == ConnectionState.Open)
                {
                    MyConnection.Close();
                }

                MyConnection.ConnectionString = _ConnectString;
                MyConnection.Open();

                return true;
            }
            catch (OleDbException MyException)
            {
                MessageBox.Show("错误原因:" + MyException.Message);
                return false;
            }
        }

        #endregion

        #region Public Static 执行查询语句

        /// <summary>
        ///     查询一个SQL语句，并把结果集填写到指定TableName的DataTable中去。
        /// </summary>
        /// <returns>返回一个存有查询结果的DataTable，如果查询没有成功，则返回null</returns>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="TableName">DataTable的名称</param>
        public static DataTable selectSQL(string strSQL, string TableName)
        {
            try
            {
                MyCommand = new OleDbCommand(strSQL, MyConnection);
                MyDataTable = new DataTable(TableName);

                MyDataAdapter = new OleDbDataAdapter(MyCommand);
                MyDataAdapter.Fill(MyDataTable);
                MyDataAdapter.Dispose();

                return MyDataTable;
            }
            catch (OleDbException MyException)
            {
                MessageBox.Show("错误原因:" + MyException.Message);
                return null;
            }
        }

        #endregion

        #region Public Static 执行操作语句

        /// <summary>
        ///     执行一个SQL语句
        /// </summary>
        /// <returns>执行的行数</returns>
        /// <param name="strSQL">Sql语句</param>
        public static int executeSQL(string strSQL)
        {
            int ParaResultLine;

            try
            {
                MyCommand = new OleDbCommand(strSQL, MyConnection);

                ParaResultLine = MyCommand.ExecuteNonQuery();
                return ParaResultLine;
            }
            catch (OleDbException MyException)
            {
                MessageBox.Show("错误原因:" + MyException.Message);
                ParaResultLine = -2;
                return ParaResultLine;
            }
        }

        #endregion

        #region Public Static 正常关闭数据库

        /// <summary>
        ///     没有事务的关闭数据库
        /// </summary>
        public static void closeDatabaseNormal()
        {
            MyConnection.Close();
        }

        #endregion

        /// <summary>
        ///     连接字符串
        /// </summary>
        private static string _ConnectString;

        private static readonly OleDbConnection MyConnection = new OleDbConnection();
        private static OleDbCommand MyCommand;
        private static DataTable MyDataTable;
        private static OleDbDataAdapter MyDataAdapter;

        public static string ConnectString
        {
            get { return _ConnectString; }
            set { _ConnectString = value; }
        }
    }
}