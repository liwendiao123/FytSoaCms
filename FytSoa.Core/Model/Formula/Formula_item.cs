using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace FytSoa.Core.Model.Formula
{

    /// <summary>
    /// 配方数据
    /// </summary>
    [SugarTable("Formula_Item")]
    public class Formula_Item
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public Formula_Item()
        {
        }




        /// <summary>
        /// Desc:唯一编号 唯一编号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Guid { get; set; }


        /// <summary>
        /// 系列ID
        /// </summary>
        public string CateGuid { get; set; }
        /// <summary>
        /// 系列名称
        /// </summary>
        public string CateName { get; set; }
        /// <summary>
        /// 配方名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeName { get; set; } 
        /// <summary>
        /// 特点描述
        /// </summary>
        public string Features { get; set; }
        /// <summary>
        /// 原料成本
        /// </summary>
        public string RawMaterialCost { get; set; }

        /// <summary>
        /// 保密级别
        /// </summary>
        public string PublicLevel { get; set; }

        /// <summary>
        /// 稳定状态
        /// </summary>
        public string StableStaus { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string CurrentStatus { get; set; }

        /// <summary>
        /// 项目负责人
        /// </summary>
        public string PersonInCharge { get; set; }

        /// <summary>
        /// 配方表现形式
        /// </summary>
        public string WordsHtml { get; set; }

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
