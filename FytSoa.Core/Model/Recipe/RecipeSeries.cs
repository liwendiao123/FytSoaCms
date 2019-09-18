using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace FytSoa.Core.Model
{

    /// <summary>
    /// 配方系列
    /// </summary>
    [SugarTable("Recipe_Series")]
    public partial class Recipe_Series
    {


        /// <summary>
        /// Ctor 构造函数
        /// </summary>
        public Recipe_Series()
        {

        }


        /// <summary>
        /// 唯一标志
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


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
        /// <summary>
        /// 创建时间
        /// </summary>
       public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
