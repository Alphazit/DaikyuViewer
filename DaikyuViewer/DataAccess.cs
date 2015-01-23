using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DaikyuViewer
{
    class DataAccess
    {
        #region "定数"

        private const string DBPath = @"L:\⑫社内ツール\代休管理ツール\開発資料\代休データ\daikyu.db";

        #endregion

        #region "参照"

        /// <summary>
        /// 社員一覧を取得
        /// </summary>
        internal List<Staff> GetStaffList()
        {
            List<Staff> sList = new List<Staff>();
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + DBPath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        string query = "SELECT * FROM Staff";
                        command.CommandText = query;
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Staff item = new Staff();
                                item.Id = int.Parse(reader["Id"].ToString());
                                item.Sruname = reader["Surname"].ToString();
                                item.FirstName = reader["FirstName"].ToString();
                                item.IsAdmin = int.Parse(reader["IsAdmin"].ToString());
                                item.Password = reader["Password"].ToString();
                                item.DateOfEntry = DateTime.Parse(reader["DateOfEntry"].ToString());
                                sList.Add(item);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //例外処理省略
            }
            return sList;
        }

        /// <summary>
        /// 社員IDとパスワードによって社員を取得
        /// </summary>
        internal List<Staff> GetStaffByIDAndPassword(int userId, string password)
        {
            List<Staff> sList = new List<Staff>();
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + DBPath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        string query = "SELECT * FROM Staff WHERE Id='" + userId.ToString() + "' AND Password='" + password + "';";
                        command.CommandText = query;
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Staff item = new Staff();
                                item.Id = int.Parse(reader["Id"].ToString());
                                item.Sruname = reader["Surname"].ToString();
                                item.FirstName = reader["FirstName"].ToString();
                                item.IsAdmin = int.Parse(reader["IsAdmin"].ToString());
                                item.Password = reader["Password"].ToString();
                                item.DateOfEntry = DateTime.Parse(reader["DateOfEntry"].ToString());
                                sList.Add(item);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //例外処理省略
            }
            return sList;
        }

        /// <summary>
        /// 社員IDによって社員を取得
        /// </summary>
        internal List<Staff> GetStaffListByID(int userId)
        {
            List<Staff> sList = new List<Staff>();
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + DBPath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        string query = "SELECT * FROM Staff WHERE Id='" + userId.ToString() + "';";
                        command.CommandText = query;
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Staff item = new Staff();
                                item.Id = int.Parse(reader["Id"].ToString());
                                item.Sruname = reader["Surname"].ToString();
                                item.FirstName = reader["FirstName"].ToString();
                                item.IsAdmin = int.Parse(reader["IsAdmin"].ToString());
                                item.Password = reader["Password"].ToString();
                                item.DateOfEntry = DateTime.Parse(reader["DateOfEntry"].ToString());
                                sList.Add(item);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //例外処理省略
            }
            return sList;
        }

        /// <summary>
        /// 社員IDによって代休を取得
        /// </summary>
        internal List<Kyuka> GetDaikyuListByStaffID(int staffId)
        {
            List<Kyuka> kList = new List<Kyuka>();
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + DBPath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        string query = "SELECT * FROM Daikyu WHERE StaffId='" + staffId.ToString() + "' ORDER BY TargetDate DESC;";
                        command.CommandText = query;
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Kyuka item = new Kyuka();
                                item.Id = int.Parse(reader["Id"].ToString());
                                item.IsComplete = int.Parse(reader["IsComplete"].ToString());
                                item.Type = int.Parse(reader["IsDaikyu"].ToString());
                                item.IsHalfDay = int.Parse(reader["IsHalfDay"].ToString());
                                item.StaffID = int.Parse(reader["StaffID"].ToString());
                                item.TargetDate = DateTime.Parse(reader["TargetDate"].ToString()).ToString("yyyy/MM/dd");
                                //表示用
                                string dispType = string.Empty;
                                if (item.Type == 0)
                                {
                                    dispType = "代出";
                                }
                                else if (item.Type == 1)
                                {
                                    dispType = "代休";
                                }
                                else if (item.Type == 2)
                                {
                                    dispType = "有給";
                                }
                                item.DispType = dispType;
                                item.DispIsHalfDay = item.IsHalfDay == 1 ? "半" : "全";
                                item.DispIsComplete = item.IsComplete == 1 ? "庶務処理済" : "処理待ち";
                                kList.Add(item);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //例外処理省略
            }
            return kList;
        }

        #endregion

        #region "更新系"

        /// <summary>
        /// 代休を新規登録
        /// </summary>
        internal bool InsertDaikyu(Kyuka kItem)
        {
            bool ret = false;
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + DBPath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        string query = "INSERT INTO Daikyu (IsDaikyu, IsHalfDay, StaffId, TargetDate, IsComplete) VALUES ("
                            + kItem.Type + ", " + kItem.IsHalfDay + ", " + kItem.StaffID + ", '"
                            + (DateTime.Parse(kItem.TargetDate)).ToString("yyyy-MM-dd")
                            + "', " + kItem.IsComplete + ");";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        ret = true;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //例外処理省略
            }
            return ret;
        }

        /// <summary>
        /// 代休を更新
        /// </summary>
        internal bool UpdateDaikyu(Kyuka kItem)
        {
            bool ret = false;
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + DBPath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        string query = "UPDATE Daikyu SET IsDaikyu=" + kItem.Type
                            + ", IsHalfDay=" + kItem.IsHalfDay
                            + ", IsComplete=" + kItem.IsComplete
                            + ", TargetDate='" + (DateTime.Parse(kItem.TargetDate)).ToString("yyyy-MM-dd")
                            + "' WHERE Id=" + kItem.Id + ";";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        ret = true;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //例外処理省略
            }
            return ret;
        }

        /// <summary>
        /// パスワードを更新
        /// </summary>
        internal bool UpdatePass(Staff staff)
        {
            bool ret = false;
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + DBPath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        string query = "UPDATE Staff SET Password='" + staff.Password
                            + "' WHERE Id=" + staff.Id + ";";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        ret = true;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //例外処理省略
            }
            return ret;
        }

        /// <summary>
        /// IDによって代休を削除
        /// </summary>
        internal bool DeleteDaikyu(int id)
        {
            bool ret = false;
            try
            {
                using (var conn = new SQLiteConnection("Data Source=" + DBPath))
                {
                    conn.Open();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        string query = "DELETE FROM Daikyu WHERE Id=" + id + ";";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        ret = true;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //例外処理省略
            }
            return ret;
        }

        #endregion
    }
}