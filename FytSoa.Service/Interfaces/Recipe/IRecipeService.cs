using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FytSoa.Common;
using FytSoa.Core.Model;
using FytSoa.Service.DtoModel;

namespace FytSoa.Service.Interfaces
{
  public interface IRecipeService : IBaseServer<FytSoa.Core.Model.Recipe>
    {
        /// <summary>
        /// 查询用户信息-增加组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Common.ApiResult<Page<FytSoa.Core.Model.Recipe>>> GetPageList(PageParm parm);
    }
}
