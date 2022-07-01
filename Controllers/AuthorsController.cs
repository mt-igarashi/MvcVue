using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;
using VueMvc.Entities;
using VueMvc.Models;
using VueMvc.Result;
using VueMvc.Service;

namespace VueMvc.Controllers
{
    /// <summary>
    /// 著者コントローラ
    /// </summary>
    [Route("authors")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthorsController : ControllerBase
    {
        /// <summary>
        /// 著者サービス
        /// </summary>
        private readonly IAuthorsService _service;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="service">著者サービス</param>
        public AuthorsController(IAuthorsService service)
        {
            _service = service;
        }

        /// <summary>
        /// 著者一覧を取得します。
        /// </summary>
        /// <returns>著者一覧</returns>
        [Route("search")]
        public async Task<ActionResult<ApiResult<List<AuthorEntity>>>> Search() {
            return await _service.GetAuthors();
        }

        /// <summary>
        /// 著者詳細を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>著者</returns>
        [Route("details")]
        public async Task<ActionResult<ApiResult<Author>>> Details(int? id)
        {
            return await _service.GetAuthorDetail(id);
        }

        /// <summary>
        /// 著者を更新します。
        /// </summary>
        /// <param name="author">著者</param>
        /// <returns>実行結果</returns>
        [Route("edit")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResult<bool>>> Edit([Bind("ID,MovieID,Name,Sex,Age,UpdateDate")] Author author)
        {
            if (!ModelState.IsValid)
            {
                var result = new ApiResult<bool>();
                result.AddErrorMessage("不正なリクエストが送信されました");
                return result;
            }
            else
            {
                return await _service.UpdateAuthor(author);
            }
        }
    }
}