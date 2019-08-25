using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace FytSoa.Core.Model.Formula
{

    /// <summary>
    /// 配方系列管理
    /// </summary>
    [SugarTable("Formula_Cate")]
    public class Formula_Cate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Formula_Cate()
        {

        }

        /// <summary>
        /// Desc:唯一编号 唯一编号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Guid { get; set; }


        /// <summary>
        /// Desc:等级 等级
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Level { get; set; } = 0;


        /// <summary>
        /// Desc:系列名称 
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }


        /// <summary>
        /// Desc:系列图片 系列图片
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Images { get; set; }


        /// <summary>
        /// Desc:状态 状态
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool Status { get; set; } = true;

        /// <summary>
        /// Desc:是否删除
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool IsDel { get; set; } = false;
    }
}
