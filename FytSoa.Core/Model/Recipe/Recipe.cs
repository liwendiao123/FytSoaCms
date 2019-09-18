using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace FytSoa.Core.Model
{

    /// <summary>
    /// 配方信息
    /// </summary>
    [SugarTable("Recipe")]
    public class Recipe
    {

        /// <summary>
        /// Ctor构造函数
        /// </summary>
        public Recipe()
        {
        }
        /// <summary>
        /// 唯一Id
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 参考配方
        /// </summary>
        public string ReferenceRecipes { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Cate { get; set; }
        /// <summary>
        /// 特点介绍
        /// </summary>
        public string Features { get; set; }
        /// <summary>
        /// 原料成本
        /// </summary>
        public string CostOfRawMaterials{ get;set; }
        /// <summary>
        /// 公开状态
        /// </summary>
        public string PublicLevel { get; set; }
        /// <summary>
        /// 稳定性状态
        /// </summary>
        public string StableDesc { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public string CurrentStatus { get; set; }
        /// <summary>
        /// 文档描述
        /// </summary>
        public string OriginDoc { get; set; }
        /// <summary>
        /// 原始文档保存路径
        /// </summary>
        public string OriginDocPath { get; set; }
        /// <summary>
        /// 工程师Id
        /// </summary>
        public string EngineerId { get; set; }
        /// <summary>
        /// 工程师
        /// </summary>
        public string Engineer { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

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
