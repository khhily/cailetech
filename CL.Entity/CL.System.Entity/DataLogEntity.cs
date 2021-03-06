//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-04-25 20:41:09 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Dapper;

namespace CL.SystemInfo.Entity
{

    /// <summary>
    ///DataLog info
    /// </summary>
    [Table("CT_DataLog")]
    public class DataLogEntity
    {

        /// <summary>
        /// 数据操作日志流水号
        /// </summary>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// 方法描述
        /// </summary>
        public string MethedDescribe { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethedName { get; set; }

        /// <summary>
        /// 数据类型(1.Table;2.View;3.StoredProcedure)
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// 操作类型(1.Insert 2.Delete 3.Select 4.Update)
        /// </summary>
        public int? OperateType { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperateContent { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime? RecordTime { get; set; }

    }
}
