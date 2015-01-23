using System.Collections.Generic;
using System;

namespace DaikyuViewer
{
    class Check
    {
        /// <summary>
        /// ログイン認証
        /// </summary>
        internal bool IsValid(int userId, string password, out Staff staff)
        {
            bool ret = false;
            staff = new Staff();
            DataAccess da = new DataAccess();
            List<Staff> sList = da.GetStaffByIDAndPassword(userId, password);
            if (sList.Count > 0)
            {
                staff = sList[0];
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 入力値チェック
        /// </summary>
        internal bool CheckInputData(DateTime? date)
        {
            bool ret = false;
            if (date != null)
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// パスワード値チェック
        /// </summary>
        internal bool CheckPassword(string pass)
        {
            bool ret = false;
            if (pass != string.Empty && pass.Length <= 10)
            {
                ret = true;
            }
            return ret;
        }
    }
}
