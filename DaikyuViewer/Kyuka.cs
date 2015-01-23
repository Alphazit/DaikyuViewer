using System;

namespace DaikyuViewer
{
    /// <summary>
    /// 代休クラス
    /// </summary>
    public class Kyuka
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int IsComplete { get; set; }
        public int IsHalfDay { get; set; }
        public int StaffID { get; set; }
        public string TargetDate { get; set; }
        //表示用
        public string DispType { get; set; }
        public string DispIsHalfDay { get; set; }
        public string DispIsComplete { get; set; }
    }
}
