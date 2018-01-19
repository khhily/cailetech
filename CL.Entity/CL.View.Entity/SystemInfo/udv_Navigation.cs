
namespace CL.View.Entity.SystemInfo
{
    public class udv_Navigation
    {
        public int id { get; set; }
        public int ParentID { get; set; }
        public int class_layer { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public int SortID { get; set; }
        public byte? IsLock { get; set; }
        public string Remark { get; set; }
        public string ActionType { get; set; }
        public byte IsSys { get; set; }
    }
}
