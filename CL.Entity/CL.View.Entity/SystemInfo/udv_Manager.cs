using Dapper;
using System;

namespace CL.View.Entity.SystemInfo
{
    /// <summary>
    /// udv_Manager
    /// </summary>
    [Table("udv_Manager")]
    public class udv_Manager
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 管理员类型0超管1系管
        /// </summary>
        public byte RoleType { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public byte IsLock { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RoleName { get; set; }
    }
}
