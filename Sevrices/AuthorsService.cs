
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueMvc.Enums;
using VueMvc.Entities;
using VueMvc.Models;
using VueMvc.Result;

namespace VueMvc.Service {
    /// <summary>
    /// 著者サービス実装クラス
    /// </summary>
    public class AuthorsService : IAuthorsService
    {
        /// <summary>
        /// DBコンテキスト
        /// </summary>
        private readonly MvcMovieContext _context;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        public AuthorsService(MvcMovieContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 著者一覧を取得します。
        /// </summary>
        /// <returns>著者リスト10</returns>
        public async Task<ApiResult<List<AuthorEntity>>> GetAuthors()
        {
            var authors = from movie in _context.Movie
                          join author in _context.Author
                          on movie.ID equals author.MovieID into ma
                          from subauthor in ma.DefaultIfEmpty()
                          select new AuthorEntity { ID = subauthor.ID,
                                                    Title = movie.Title,
                                                    Name = subauthor.Name,
                                                    Sex = subauthor.Sex,
                                                    Age = subauthor.Age };

            var result = new ApiResult<List<AuthorEntity>>();
            result.Result = await authors.ToListAsync();

            return result;
        }

        /// <summary>
        /// 著者詳細を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>著者</returns>
        public async Task<ApiResult<Author>> GetAuthorDetail(int? id)
        {
            var result = new ApiResult<Author>();
            if (id == null) 
            {
                result.AddErrorMessage("不正なリクエストが送信されました");
                return result;
            }
            
            var author = await _context.Author
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(a => a.ID == id);

            if (author == null)
            {
                result.AddDeletionErrorMessage("対象の著者は既に削除されています");
                return result;
            }

            result.Result = author;
            return result;
        }

        /// <summary>
        /// 著者を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>著者</returns>
        public async Task<ApiResult<Author>> GetAuthor(int? id)
        {
            var result = new ApiResult<Author>();
            if (id == null) 
            {
                result.AddErrorMessage("不正なリクエストが送信されました");
                return result;
            }

            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                result.AddDeletionErrorMessage("対象の著者は既に削除されています");
                return result;
            }

            result.Result = author;
            return result;
        }

        /// <summary>
        /// 著者を更新します。
        /// </summary>
        /// <param name="author">著者</param>
        /// <returns>更新結果</returns>
        public async Task<ApiResult<bool>> UpdateAuthor(Author author)
        {
            var result = new ApiResult<bool>();
            if (author.Sex == Sex.Female.getStringValue() && author.Age >= 40) 
            {
                result.AddErrorMessage("女性の場合は40歳未満を入力してください。");
                return result;
            }

            try
            {
                var entry = _context.Add<Author>(author);
                
                entry.Entity.UpdateDate = DateTime.Now;
                _context.Update(author);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Author.Any(e => e.ID == author.ID))
                {
                    result.AddDeletionErrorMessage("対象の著者は既に既に削除されています");
                }
                else
                {
                    result.AddOptimisticErrorMessage("対象の著者は既に更新されています。再度処理を実行してください");
                }
            }
            return result;
        }
    }
}