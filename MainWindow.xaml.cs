using RegistrationTool.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegistrationTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string connString = "";
        OleDbConnection conn = null;
        private CryptED Crypt = new CryptED();
        const int C_RegKey = 33990;
        const ushort con_Initial = 0xFFFF;
        const int CommandTimeout = 36000;
        ushort[] t16 =
        {
              0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7, 0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD,
              0xE1CE, 0xF1EF, 0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6, 0x9339, 0x8318, 0xB37B, 0xA35A,
              0xD3BD, 0xC39C, 0xF3FF, 0xE3DE, 0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485, 0xA56A, 0xB54B,
              0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D, 0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4,
              0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC, 0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861,
              0x2802, 0x3823, 0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B, 0x5AF5, 0x4AD4, 0x7AB7, 0x6A96,
              0x1A71, 0x0A50, 0x3A33, 0x2A12, 0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A, 0x6CA6, 0x7C87,
              0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41, 0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
              0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70, 0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A,
              0x9F59, 0x8F78, 0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F, 0x1080, 0x00A1, 0x30C2, 0x20E3,
              0x5004, 0x4025, 0x7046, 0x6067, 0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E, 0x02B1, 0x1290,
              0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256, 0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D,
              0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405, 0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E,
              0xC71D, 0xD73C, 0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634, 0xD94C, 0xC96D, 0xF90E, 0xE92F,
              0x99C8, 0x89E9, 0xB98A, 0xA9AB, 0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3, 0xCB7D, 0xDB5C,
              0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A, 0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
              0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9, 0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83,
              0x1CE0, 0x0CC1, 0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8, 0x6E17, 0x7E36, 0x4E55, 0x5E74,
              0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
            };
        private int Key0_int = 0;
        public MainWindow()
        {
            InitializeComponent();
            refreshData();
        }

        private void SetOleStr()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            connString = path + "RegTool.mdb";
            conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + connString);
        }

        private void colseOle()
        {
            if (conn != null)
                conn.Close();
            conn = null;
        }

        private void openOle()
        {
            colseOle();
            SetOleStr();
            conn.Open();
        }
        ushort UpdateCRC(ushort src, byte ch)
        {
            int ret = (src << 8) ^ t16[ch ^ (src >> 8)];
            while ((ret > 65535) || (ret < 0)) ret = ret & 0xffff;
            return Convert.ToUInt16(ret);
        }
        ushort CRCCheck(char[] X, int Num)
        {
            ushort tmp = 0;
            if (Num > 0)
            {
                tmp = UpdateCRC(con_Initial, Convert.ToByte(X[0]));
                for (int i = 1; i < Num; i++) tmp = UpdateCRC(tmp, Convert.ToByte(X[i]));
            }
            return tmp;
        }
        public struct RegisterInfo
        {
            public static string ProductName = "Taurus(HYSOON)";
            public static string Serial = "";
            public static bool MustReg = false;
            public static bool IsReg = false;
            public static bool IsAlways = false;
            public static bool IsTest = false;
            public static bool IsValid = false;
            public static string RegUser = "";
            public static string RegKey = "";
            public static string StateText = "";
            public static DateTime StartDate;
            public static DateTime EndDate;
            public static DateTime ValidDate;
            public static string RegDateText = "";
        }
        string GetKeyEx(string Key)
        {
            string ret = Key;
            while (ret.Length < 5) ret = "0" + ret;
            return ret;
        }

        bool IsRightKey(string Key)
        {
            string tmp = Crypt.StrEncrypt(RegisterInfo.Serial, C_RegKey);
            char[] P = new char[tmp.Length];
            tmp.CopyTo(0, P, 0, tmp.Length);
            ushort CRC = CRCCheck(P, P.Length);
            return (GetKeyEx(Convert.ToString(CRC)) == Key);
        }

        bool IsRightKeyAll(string Key)
        {
            string tmp = Crypt.StrEncrypt("8EA9B7DF48CEE555", C_RegKey);
            char[] P = new char[tmp.Length];
            tmp.CopyTo(0, P, 0, tmp.Length);
            ushort CRC = CRCCheck(P, P.Length);
            return (GetKeyEx(Convert.ToString(CRC)) == Key);
        }

        bool IsRightSoft(string Key)
        {
            string tmp = Crypt.StrEncrypt(RegisterInfo.ProductName, Key0_int);
            char[] P = new char[tmp.Length];
            tmp.CopyTo(0, P, 0, tmp.Length);
            ushort CRC = CRCCheck(P, P.Length);
            return (GetKeyEx(Convert.ToString(CRC)) == Key);
        }

        bool IsUserKey(string User, string Key)
        {
            string tmp = Crypt.StrEncrypt(User, Key0_int);
            char[] P = new char[tmp.Length];
            tmp.CopyTo(0, P, 0, tmp.Length);
            ushort CRC = CRCCheck(P, P.Length);
            return (GetKeyEx(Convert.ToString(CRC)) == Key);
        }

        string GetRightKey()
        {
            string tmp = Crypt.StrEncrypt(RegisterInfo.Serial, C_RegKey);
            char[] P = new char[tmp.Length];
            tmp.CopyTo(0, P, 0, tmp.Length);
            ushort CRC = CRCCheck(P, P.Length);
            return GetKeyEx(Convert.ToString(CRC));
        }

        string GetRightSoft(int Key0_int)
        {
            string tmp = Crypt.StrEncrypt(RegisterInfo.ProductName, Key0_int);
            char[] P = new char[tmp.Length];
            tmp.CopyTo(0, P, 0, tmp.Length);
            ushort CRC = CRCCheck(P, P.Length);
            return GetKeyEx(Convert.ToString(CRC));
        }

        string GetUserKey(string User, int Key0_int)
        {
            string tmp = Crypt.StrEncrypt(User, Key0_int);
            char[] P = new char[tmp.Length];
            tmp.CopyTo(0, P, 0, tmp.Length);
            ushort CRC = CRCCheck(P, P.Length);
            return GetKeyEx(Convert.ToString(CRC));
        }

        string GetDateKey(bool IsAlways, DateTime ValidDate, string Key0)
        {
            string tmp = "36526";
            if (!IsAlways) tmp = ValidDate.ToOADate().ToString();
            return Crypt.StrEncrypt(tmp, Key0, 0);
        }

        public string GetRegKey(string User, bool IsAlways, DateTime ValidDate)
        {
            string Key0 = GetRightKey();
            int Key0_int = Convert.ToInt32(Key0) * 2;
            string Key1 = GetRightSoft(Key0_int);
            string Key2 = GetDateKey(IsAlways, ValidDate, Key0);
            string Key3 = GetUserKey(User, Key0_int);
            return Key0 + "-" + Key1 + "-" + Key2 + "-" + Key3;
        }

        private void refreshData()
        {
            List<ModeType> modeType = new List<ModeType>();
            modeType.Add(new ModeType(1, "海系模块"));
            modeType.Add(new ModeType(2, "星系模块"));
            modeType.Add(new ModeType(3, "工资模块"));
            cbbMode.ItemsSource = modeType;
            cbbMode.SelectedIndex = 0;
            string sql = "select * from KeyList";
            selectData(sql);
        }

        private void selectData(string SqlStr)
        {
            openOle();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = SqlStr;
            OleDbDataReader dr = cmd.ExecuteReader();
            int count = 0;
            DataGrid.ItemsSource = null;
            DataGridDataInit.Instance.RegistrationList.Clear();
            while (dr.Read())
            {
                count++;
                DataGridDataInit.Instance.RegistrationList.Add(new Model.RegistrationData {
                    No = dr["No"].ToString(),
                    Count = count,
                    SerialNumber = dr["Serial"].ToString(),
                    Name = dr["User"].ToString(),
                    RegistrationCode = dr["Key"].ToString(),
                    DateTimeCode = dr["BuildTime"].ToString(),
                }) ; 
            }
            cmd.Dispose();
            colseOle();
            DataGrid.ItemsSource = DataGridDataInit.Instance.RegistrationList;
            UIHelper.DoEvents();
        }

        public int ExecSQL(string SQLQuery)
        {
            int ret = 0;
            if (SQLQuery == "") return ret;
            openOle();
            try
            {
                OleDbCommand oleCmd = new OleDbCommand(SQLQuery, conn);
                oleCmd.CommandTimeout = CommandTimeout;
                ret = oleCmd.ExecuteNonQuery();
                oleCmd.Dispose();
                oleCmd = null;
                ret = 1;
            }
            catch (Exception E)
            {
                MessageBox.Show(E + "\r\n" + SQLQuery);
            }
            finally
            {
                colseOle();
            }
            return ret;
        }

        private void btnGenerateRegCode_Click(object sender, RoutedEventArgs e)
        {
            string ProdKey = Crypt.StrEncrypt(RegisterInfo.ProductName, C_RegKey);
            string tmp = "";
            RegisterInfo.RegUser = "";
            RegisterInfo.RegKey = "";
            RegisterInfo.IsAlways = false;
            RegisterInfo.IsTest = true;
            RegisterInfo.IsValid = true;
            if (tmp == Crypt.StrEncrypt("36891", ProdKey, 0))
                RegisterInfo.StartDate = DateTime.Now.Date;
            if(txtSerialNumber.Text=="")
            {
                MessageBox.Show("Please enter the serial number!");
                txtSerialNumber.Focus();
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter a name!");
                txtName.Focus();
                return;
            }
            RegisterInfo.Serial = txtSerialNumber.Text.Trim();
            string RegName = txtName.Text.Trim();
            bool isAlways = true;
            string RegKey = GetRegKey(RegName,true,DateTime.Now.AddYears(100));
            txtRegistrationCode.Text = RegKey;
            byte Always = Convert.ToByte(isAlways);
           
            string sqlStr = "INSERT INTO KeyList([ID],[Area],[User],[Serial],[Always],[ValidDate],[Key],[BuildTime]) " +
                            "VALUES('" + RegisterInfo.ProductName + "','','" + RegName + "','"+ RegisterInfo.Serial + "',"+ Always + ",'" + (DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss") + "','" + RegKey + "','" + (DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss") + "')";
            if (ExecSQL(sqlStr) != 0)
                refreshData();
            
            string msg = "\r\n" + "用户名称: " + RegName + "\r\n\r\n"; 
            msg = msg + "序 列  号: " + RegisterInfo.Serial + "\r\n\r\n";
            msg = msg + "注 册  码: " + RegKey + "\r\n\r\n";
            msg = msg + "提示：序列号、注册码均由数字0－9、A－F组成，注册码中的“-”不可忽略。\r\n";
            WindowPrompt winP = new WindowPrompt(msg);
            winP.ShowDialog();  
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int index = DataGrid.SelectedIndex;
            if (DataGridDataInit.Instance.RegistrationList.Count > DataGrid.SelectedIndex && index >= 0)
            {
                txtSerialNumber.Text = DataGridDataInit.Instance.RegistrationList[index].SerialNumber;
                txtName.Text = DataGridDataInit.Instance.RegistrationList[index].Name;
                txtRegistrationCode.Text = DataGridDataInit.Instance.RegistrationList[index].RegistrationCode;
                UIHelper.DoEvents();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int index = DataGrid.SelectedIndex;
            if (DataGridDataInit.Instance.RegistrationList.Count > DataGrid.SelectedIndex && index >= 0)
            {
                string sno = DataGridDataInit.Instance.RegistrationList[index].No;
                string msg = "\r\n" + "用户名称: " + DataGridDataInit.Instance.RegistrationList[index].Name + "\r\n\r\n";
                msg = msg + "序 列  号: " + DataGridDataInit.Instance.RegistrationList[index].SerialNumber + "\r\n\r\n";
                msg = msg + "注 册  码: " + DataGridDataInit.Instance.RegistrationList[index].RegistrationCode + "\r\n\r\n";
                msg = msg + "您确定要删除吗?";
                WindowPrompt winP = new WindowPrompt(msg);
                
                if(winP.ShowDialog()==true)
                {
                    string sqlStr = "delete from KeyList where [No]=" + sno + "";
                    if (ExecSQL(sqlStr) != 0)
                        refreshData();
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string text = txtSearch.Text.Trim();
            string sql = "select * from KeyList where [Serial]='" + text + "' OR ([Serial] like '%" + text + "%')" +
                " OR [User]='" + text + "' OR ([User] like '%" + text + "%')";
            selectData(sql);
        }

        private void txtSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string text = txtSearch.Text.Trim();
                string sql = "select * from KeyList where [Serial]='" + text + "' OR ([Serial] like '%" + text + "%')" +
                    " OR [User]='" + text + "' OR ([User] like '%" + text + "%')";
                selectData(sql);
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = DataGrid.SelectedIndex;
            if (DataGridDataInit.Instance.RegistrationList.Count > DataGrid.SelectedIndex && index >= 0)
            {
                string msg = "\r\n" + "用户名称: " + DataGridDataInit.Instance.RegistrationList[index].Name + "\r\n\r\n";
                msg = msg + "序 列  号: " + DataGridDataInit.Instance.RegistrationList[index].SerialNumber + "\r\n\r\n"; 
                msg = msg + "注 册  码: " + DataGridDataInit.Instance.RegistrationList[index].RegistrationCode + "\r\n\r\n";
                msg = msg + "提示：序列号、注册码均由数字0－9、A－F组成，注册码中的“-”不可忽略。\r\n";
                WindowPrompt winP = new WindowPrompt(msg);
                winP.ShowDialog();
            }
        }

        private void cbbMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count>0)
            {
                //int index = ((ModeType)cbbMode.SelectedItem).id;
                //switch (index)
                //{
                //    case 1:
                //        RegisterInfo.ProductName = "Taurus(SEA)";
                //        break;
                //    case 2:
                //        RegisterInfo.ProductName = "Taurus(STAR)";
                //        break;
                //    case 3:
                //        RegisterInfo.ProductName = "Taurus(GZ)";
                //        break;
                //}
            }  
        }
    }
    public class ModeType
    {
        private int _id;
        private string _name;

        public ModeType(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
