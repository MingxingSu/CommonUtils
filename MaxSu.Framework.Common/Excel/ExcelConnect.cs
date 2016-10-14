using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace MaxSu.Framework.Common.Excel
{
    /// <summary>
    ///     EXCEL�����봦���࣬�����������ݿ⡢��ѯSQL��ִ��SQL
    /// </summary>
    public class ExcelConnect
    {
        #region Public Static �������ݿ�

        /// <summary>
        ///     �������ݿ�
        /// </summary>
        /// <returns>���ӳɹ�����TRUE�����򷵻�FALSE</returns>
        public static bool connectDatabase()
        {
            try
            {
                //���ݿ��Ƿ��Ѿ���
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
                MessageBox.Show("����ԭ��:" + MyException.Message);
                return false;
            }
        }

        #endregion

        #region Public Static ִ�в�ѯ���

        /// <summary>
        ///     ��ѯһ��SQL��䣬���ѽ������д��ָ��TableName��DataTable��ȥ��
        /// </summary>
        /// <returns>����һ�����в�ѯ�����DataTable�������ѯû�гɹ����򷵻�null</returns>
        /// <param name="strSQL">SQL���</param>
        /// <param name="TableName">DataTable������</param>
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
                MessageBox.Show("����ԭ��:" + MyException.Message);
                return null;
            }
        }

        #endregion

        #region Public Static ִ�в������

        /// <summary>
        ///     ִ��һ��SQL���
        /// </summary>
        /// <returns>ִ�е�����</returns>
        /// <param name="strSQL">Sql���</param>
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
                MessageBox.Show("����ԭ��:" + MyException.Message);
                ParaResultLine = -2;
                return ParaResultLine;
            }
        }

        #endregion

        #region Public Static �����ر����ݿ�

        /// <summary>
        ///     û������Ĺر����ݿ�
        /// </summary>
        public static void closeDatabaseNormal()
        {
            MyConnection.Close();
        }

        #endregion

        /// <summary>
        ///     �����ַ���
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