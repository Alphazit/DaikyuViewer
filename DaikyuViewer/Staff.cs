using System;

namespace DaikyuViewer
{
    /// <summary>
    /// 社員クラス
    /// </summary>
    public class Staff
    {
        public int Id { get; set; }
        public string Sruname { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public int IsAdmin { get; set; }
        public DateTime DateOfEntry { get; set; }   //未使用。入社後X箇月間は代休なしとかで使える鴨
    }
}
