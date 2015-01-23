using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DaikyuViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DaikyuView : Window
    {
        #region "クラス変数"

        private Staff staff;
        private int isAdmin;
        List<Kyuka> dList = new List<Kyuka>();

        #endregion

        #region "初期化"

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DaikyuView(Staff staff, int isAdmin)
        {
            InitializeComponent();
            this.staff = staff;
            this.isAdmin = isAdmin;
            InitShomu();
        }

        /// <summary>
        /// 庶務操作初期化
        /// </summary>
        private void InitShomu()
        {
            if (isAdmin == 1)
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
                        cbStaff.Items.Add(cbItem);
                    }
                }
                else
                {
                    MessageBox.Show("データが見付かりません。プログラム終了します。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            else
            {
                cbStaff.IsEnabled = false;
                bSubstitute.IsEnabled = false;
            }
        }

        #endregion

        #region "イベント"

        /// <summary>
        /// 新規ボタン押下
        /// </summary>
        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            //選択行を編集
            Kyuka item = new Kyuka();
            item.StaffID = staff.Id;
            DaikyuDetail detail = new DaikyuDetail(item, staff, isAdmin);
            detail.Show();
            this.Close();
        }

        /// <summary>
        /// 修正ボタン押下
        /// </summary>
        private void bEdit_Click(object sender, RoutedEventArgs e)
        {
            //選択行があるか？
            if (dList.Count > 0 && dgDaikyu.SelectedIndex != -1
                && dList.Count != dgDaikyu.SelectedIndex)
            {
                Kyuka selectItem = (Kyuka)dgDaikyu.SelectedValue;
                if (isAdmin != 1 && selectItem.IsComplete == 1)
                {
                    MessageBox.Show("庶務処理済のデータは修正できません。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //選択行を編集
                    DaikyuDetail detail = new DaikyuDetail(selectItem, staff, isAdmin);
                    detail.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("行が選択されていません。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 削除ボタン押下
        /// </summary>
        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            //選択行があるか？
            if (dList.Count > 0 && dgDaikyu.SelectedIndex != -1
                && dList.Count != dgDaikyu.SelectedIndex)
            {
                Kyuka selectItem = (Kyuka)dgDaikyu.SelectedValue;
                if (isAdmin != 1 && selectItem.IsComplete == 1)
                {
                    MessageBox.Show("庶務処理済のデータは削除できません。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (MessageBox.Show("削除しますか？", "代休管理", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        //選択行を削除
                        DataAccess da = new DataAccess();
                        if (da.DeleteDaikyu(selectItem.Id))
                        {
                            MessageBox.Show("削除しました。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Information);
                            //リフレッシュ
                            SetData();
                        }
                        else
                        {
                            MessageBox.Show("削除に失敗しました。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("行が選択されていません。", "代休管理", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 管理ボタン押下
        /// </summary>
        private void bSubstitute_Click(object sender, RoutedEventArgs e)
        {
            //社員IDを削り出す
            staff.Id = int.Parse(cbStaff.SelectionBoxItem.ToString().
                Substring(cbStaff.SelectionBoxItem.ToString().IndexOf(':') - 1, 1));
            SetData();
        }

        /// <summary>
        /// 閉じるボタン押下
        /// </summary>
        private void bQuit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("終了しますか？", "代休管理", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        /// <summary>
        /// パスワードの変更ボタン押下
        /// </summary>
        private void bChangePass_Click(object sender, RoutedEventArgs e)
        {
            EditPass ep = new EditPass(staff);
            ep.Show();
        }

        #endregion

        #region "internalメソッド"

        /// <summary>
        /// データをセット
        /// </summary>
        internal void SetData()
        {
            DataAccess da = new DataAccess();
            List<Staff> item = da.GetStaffListByID(staff.Id);
            dList = da.GetDaikyuListByStaffID(staff.Id);
            //社員名表示
            lStaffName.Content = item[0].Sruname + " " + item[0].FirstName;
            //データをバインド
            dgDaikyu.ItemsSource = null;
            dgDaikyu.ItemsSource = dList;
            //代出値（全日は2コマ、半日は1コマ）を算出
            int daishutsuCount =
                (dList.Count(d => d.Type == 0 && d.IsComplete == 1 && d.IsHalfDay == 0)) * 2
                + (dList.Count(d => d.Type == 0 && d.IsComplete == 1 && d.IsHalfDay == 1));
            int daikyuCount =
                (dList.Count(d => d.Type == 1 && d.IsComplete == 1 && d.IsHalfDay == 0)) * 2
                + (dList.Count(d => d.Type == 1 && d.IsComplete == 1 && d.IsHalfDay == 1));
            int shokaKoma = daishutsuCount - daikyuCount;
            //コマを日数に換算
            double shokaDay = (double)shokaKoma / (double)2;
            //有給値
            int yukyuCount = GetYukyuCount();

            lDaikyuState.Content = shokaDay.ToString();
            EditDG(dgDaikyu);
        }

        /// <summary>
        /// 有給残数を取得
        /// </summary>
        private int GetYukyuCount()
        {
            int ret = 0;
            //入社日から前回付加値+前々回付加値が最大値

            //前々回付加以降の消化数

            //差し引いて算出            

            return ret;
        }

        #endregion

        #region "privateメソッド"

        /// <summary>
        /// データグリッドの体裁を整える
        /// Showしてから呼び出さないとダメ
        /// </summary>
        private void EditDG(System.Windows.Controls.DataGrid dgDaikyu)
        {
            dgDaikyu.Columns[0].Visibility = System.Windows.Visibility.Hidden;
            dgDaikyu.Columns[1].Visibility = System.Windows.Visibility.Hidden;
            dgDaikyu.Columns[2].Visibility = System.Windows.Visibility.Hidden;
            dgDaikyu.Columns[3].Visibility = System.Windows.Visibility.Hidden;
            dgDaikyu.Columns[4].Visibility = System.Windows.Visibility.Hidden;
            dgDaikyu.Columns[5].Width = 100;
            dgDaikyu.Columns[6].Width = 80;
            dgDaikyu.Columns[7].Width = 80;
            dgDaikyu.Columns[8].Width = 80;
            dgDaikyu.Columns[5].Header = "代休/代出日";
            dgDaikyu.Columns[6].Header = "種　別";
            dgDaikyu.Columns[7].Header = "全日/半日";
            dgDaikyu.Columns[8].Header = "完了状況";
            dgDaikyu.CanUserDeleteRows = false;
        }

        #endregion
    }
}
