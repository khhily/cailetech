
namespace CL.View.Entity.Game
{
    public class udv_NewsTypes
    {
        /// <summary>
        /// 类别ID号
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 是否系统类别
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ClassLayer { set; get; }
    }
}
