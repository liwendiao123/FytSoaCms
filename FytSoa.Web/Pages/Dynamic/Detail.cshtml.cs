using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FytSoa.Common;
using FytSoa.Core.Model.Cms;
using FytSoa.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FytSoa.Web.Pages.Dynamic
{
    public class DetailModel : PageModel
    {
        private readonly ICacheService _cacheService;
        private readonly ICmsSiteService _siteService;
        private readonly ICmsColumnService _columnService;
        private readonly ICmsArticleService _articleService;
        public DetailModel(ICacheService cacheServicee, ICmsSiteService siteService, ICmsColumnService columnService
            , ICmsArticleService articleService)
        {
            _cacheService = cacheServicee;
            _siteService = siteService;
            _columnService = columnService;
            _articleService = articleService;
        }
        public CmsSite Site { get; set; }
        List<CmsColumn> Column { get; set; }
        public CmsArticle Article { get; set; }
        public CmsColumn ParentColumn { get; set; }
        public List<CmsArticle> ArticleList { get; set; }
        public void OnGet(int id)
        {
            //���վ����Ϣ
            if (_cacheService.Exists(CacheKey.WEBCMSSITE))
            {
                Site = _cacheService.GetCache<CmsSite>(CacheKey.WEBCMSSITE);
            }
            else
            {
                Site = _siteService.GetModelAsync(m => m.Guid == "78756a6c-50c8-47a5-b898-5d6d24a20327").Result.data;
                //���뵽����
                _cacheService.SetCache(CacheKey.WEBCMSSITE, Site, DateTimeOffset.Now.AddDays(30));
            }
            //�����Ŀ��Ϣ
            if (_cacheService.Exists(CacheKey.WEBCMSCOLUMN))
            {
                Column = _cacheService.GetCache<List<CmsColumn>>(CacheKey.WEBCMSCOLUMN);
            }
            else
            {
                Column = _columnService.GetListAsync(m => true, m => m.Sort, DbOrderEnum.Asc).Result.data;
                //���뵽����
                _cacheService.SetCache(CacheKey.WEBCMSCOLUMN, Column, DateTimeOffset.Now.AddDays(30));
            }
            Article = _articleService.GetModelAsync(m => m.Id == id).Result.data;
            ParentColumn = Column.Find(m => m.Id == Article.ColumnId);
            #region ���ӵ����
            //�ж��Ƿ�Ϊ��ǰ��
            if (Convert.ToDateTime(Article.LastHitDate).Day == DateTime.Now.Day)
            {
                Article.DayHits += 1;
            }
            //�ж��Ƿ�Ϊ��ǰ����
            if (Convert.ToDateTime(Article.LastHitDate).DayOfWeek == DateTime.Now.DayOfWeek)
            {
                Article.WeedHits += 1;
            }
            //�ж��Ƿ�Ϊ��ǰ���·�
            if (Convert.ToDateTime(Article.LastHitDate).Month == DateTime.Now.Month)
            {
                Article.MonthHits += 1;
            }
            Article.Hits += 1;
            Article.LastHitDate = DateTime.Now;
            _articleService.UpdateAsync(Article);
            #endregion

            #region ��ѯ��ص�����
            ArticleList = _articleService.GetPagesAsync(new Service.DtoModel.PageParm() { limit = 6 },
                m => m.Audit && !m.IsRecyc && m.ColumnId == Article.ColumnId && m.Id != Article.Id,
                m => m.Sort, DbOrderEnum.Desc).Result.data.Items;
            #endregion
        }
    }
}