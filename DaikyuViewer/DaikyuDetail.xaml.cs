using System;
using System.Windows;
using System.ComponentModel;

namespace DaikyuViewer
{
    /// <summary>
    /// DaikyuDetail.xaml の相互作用ロジック
    /// </summary>
    public partial class DaikyuDetail : Window
    {
        #region "クラス変数"

        private Kyuka dItem;
        private Staff staff;
        private int isAdmin;

        #endregion

        #region "初期化"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DaikyuDetail(Kyuka item, Staff staff, int isAdmin)
        {
            InitializeComponent();
            this.dItem = item;
            this.staff = staff;
            this.isAdmin = isAdmin;
            SetItem();
        }

        /// <summary>
        /// データを表示
        /// </summary>
        private void SetItem()
        {
            //庶務のみ使用可
            if (isAdmin != 1)
            {
                cbShori.IsEnabled = false;
            }
            //IDが0なら新規
            if (dItem.Id != 0)
            {
                if (dItem.Type == 1)
                {
                    rbDaikyu.IsChecked = true;
                }
                if (dItem.Type == 2)
                {
                    //rbYukyu.IsChecked = true;
                }
                else
                {
                    rbDaishutsu.IsChecked = true;
                }
                dpDate.DisplayDate = DateTime.Parse(dItem.TargetDate);
                dpDate.SelectedDate = DateTime.Parse(dItem.TargetDate);
                if (dItem.IsHalfDay == 1)
                {
                    rbHannichi.IsChecked = true;
                }
                else
                {
                    rbZennichi.IsChecked = true;
                }
                if (dItem.IsComplete == 1)
                {
                    cbShori.IsChecked = true;
                }
            }
        }

        #endregion

        #region "イベント"

        /// <summary>
        /// 登録ボタン押下
        /// </summary>
        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            Check check = new Check();
            if  (rbDaikyu.IsChecked == false && rbDaishutsu.IsChecked == false)
            {
                MessageBox.Show("代休・代出を選択してください。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else if (!check.CheckInputData(dpDate.SelectedDate))
            {
                MessageBox.Show("日付を正しく指定してください。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                if (SaveDaikyu())
                {
                    MessageBox.Show("登録しました。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Information);
                    //DaikyuView view = new DaikyuView(dItem.StaffID);
                    //view.Show();
                    //view.SetData();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("登録に失敗しました。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 閉じるボタン押下
        /// </summary>
        private void bQuit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("ウィンドウを閉じてよろしいですか？", "代休管理", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        /// <summary>
        /// ウィンドウ閉じる前イベント
        /// </summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DaikyuView view = new DaikyuView(staff, isAdmin);
            view.Show();
            view.SetData();
        }

        /// <summary>
        /// 有給休暇を選択
        /// </summary>
        private void rbYukyu_Checked(object sender, RoutedEventArgs e)
        {
            if (rbHannichi.IsChecked == true)
            {
                MessageBox.Show("有給休暇は全日となります。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                rbZennichi.IsChecked = true;
            }
            //半日は選択不可
            rbHannichi.IsEnabled = false;
        }

        /// <summary>
        /// 代休を選択
        /// </summary>
        private void rbDaikyu_Checked(object sender, RoutedEventArgs e)
        {
            //半日を選択可
            rbHannichi.IsEnabled = true;
        }

        /// <summary>
        /// 代出を選択
        /// </summary>
        private void rbDaishutsu_Checked(object sender, RoutedEventArgs e)
        {
            //半日を選択可
            rbHannichi.IsEnabled = true;
        }

        #endregion

        #region "privateメソッド"

        /// <summary>
        /// 代休を登録
        /// </summary>
        private bool SaveDaikyu()
        {
            bool ret = false;
            int type = 0;
            if (rbDaikyu.IsChecked == true)
            {
                type = 1;
            }            
            dItem.Type = type;
            dItem.TargetDate = dpDate.SelectedDate.ToString();
            dItem.IsHalfDay = rbHannichi.IsChecked == true ? 1 : 0;
            dItem.IsComplete = cbShori.IsChecked == true ? 1 : 0;
            DataAccess da = new DataAccess();
            //IDが0なら新規
            if (dItem.Id == 0)
            {
                ret = da.InsertDaikyu(dItem);
            }
            else
            {
                ret = da.UpdateDaikyu(dItem);
            }            
            return ret;
        }

        #endregion

    }
}
