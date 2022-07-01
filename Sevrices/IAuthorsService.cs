using System.Collections.Generic;
using System.Threading.Tasks;
using VueMvc.Entities;
using VueMvc.Models;
using VueMvc.Result;

namespace VueMvc.Service {
    /// <summary>
    /// 著者サービスインターフェイス
    /// </summary>
    public interface IAuthorsService {

        /// <summary>
        /// 著者一覧を取得します。
        /// </summary>
        /// <returns>著者一覧</returns>
        Task<ApiResult<List<AuthorEntity>>> GetAuthors(); 

        /// <summary>
        /// 著者詳細を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>著者</returns>
        Task<ApiResult<Author>> GetAuthorDetail(int? id);

        /// <summary>
        /// 著者を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>著者</returns>
        Task<ApiResult<Author>> GetAuthor(int? id);

        /// <summary>
        /// 著者を更新します。
        /// </summary>
        /// <param name="author">著者</param>
        /// <returns>true/false</returns>
        Task<ApiResult<bool>> UpdateAuthor(Author author);
    }
}