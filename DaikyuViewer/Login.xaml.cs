using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DaikyuViewer
{
    /// <summary>
    /// Login.xaml の相互作用ロジック
    /// </summary>
    public partial class Login : Window
    {
        #region "初期化"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Login()
        {
            InitializeComponent();
            InitCB();
        }

        /// <summary>
        /// 社員名一覧を取得
        /// </summary>
        private void InitCB()
        {
            DataAccess da = new DataAccess();
            List<Staff> sList = da.GetStaffList();
            if (sList.Count > 0)
            {
                //空白行
                ComboBoxItem cbItem;
                //データ
                foreach (var sItem in sList)
                {
                    cbItem = new ComboBoxItem();
                    cbItem.Content = sItem.Id + ":" + sItem.Sruname + " " + sItem.FirstName;
                    cbAccount.Items.Add(cbItem);
                }
            }
            else
            {
                MessageBox.Show("データが見付かりません。プログラム終了します。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        #endregion

        #region "イベント"

        /// <summary>
        /// ログインボタン押下
        /// </summary>
        private void bEnter_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }

        /// <summary>
        /// キー押下
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LogIn();
            }
        }

        /// <summary>
        /// ログイン
        /// </summary>
        private void LogIn()
        {
            if (cbAccount.SelectedIndex != -1)
            {
                //社員IDを削り出す
                int userId = int.Parse(cbAccount.SelectionBoxItem.ToString().
                    Substring(cbAccount.SelectionBoxItem.ToString().IndexOf(':') - 1, 1));
                //認証
                Check check = new Check();
                Staff staff = new Staff();
                if (check.IsValid(userId, pbPass.Password, out staff))
                {
                    //メイン画面へ遷移
                    int isAdmin = staff.IsAdmin;
                    DaikyuView window = new DaikyuView(staff, isAdmin);
                    window.Show();
                    window.SetData();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("パスワードが違います。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("社員名を選択してください。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        #endregion
    }
}
