using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DaikyuViewer
{
    /// <summary>
    /// EditPass.xaml の相互作用ロジック
    /// </summary>
    public partial class EditPass : Window
    {
        #region "クラス変数"

        private Staff staff;

        #endregion

        #region "初期化"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditPass(Staff staff)
        {
            InitializeComponent();
            this.staff = staff;
        }

        #endregion

        #region "イベント"

        /// <summary>
        /// ログインボタン押下
        /// </summary>
        private void bEnter_Click(object sender, RoutedEventArgs e)
        {
            Check check = new Check();
            if (check.CheckPassword(pbPass.Password))
            {
                DataAccess da = new DataAccess();
                Staff sItem = new Staff();
                sItem.Password = pbPass.Password;
                sItem.Id = staff.Id;
                if (da.UpdatePass(sItem))
                {
                    MessageBox.Show("変更しました。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Information);
                    //閉じる
                    this.Close();
                }
                else
                {
                    MessageBox.Show("変更に失敗しました。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("入力値が不正です。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion
    }
}
