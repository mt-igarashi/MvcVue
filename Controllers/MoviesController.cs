using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Mime;
using VueMvc.Models;
using VueMvc.Result;

namespace VueMvc.Controllers
{
    /// <summary>
    /// 映画コントローラ
    /// このクラスではDIによるサービスのインジェクションを行わず
    /// 直接処理を記述しています。
    /// DIのサンプルはAuthorsControllerクラスを参照してください。
    /// </summary>
    [Route("movies")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class MoviesController : ControllerBase
    {
        /// <summary>
        /// DBコンテキスト
        /// </summary>
        private readonly MvcMovieContext _context;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 指定した条件で映画テーブルを検索します。
        /// </summary>
        /// <param name="movieGenre">映画ジャンル</param>
        /// <param name="searchString">検索文字列(タイトル)</param>
        /// <returns>ビューモデル</returns>
        [Route("search")]
        public async Task<ActionResult<ApiResult<MovieGenreViewModel>>> Search(string movieGenre, string searchString, int pageNumber = 0, int pageSize = 5)
        {
            var movieGenreVM = new MovieGenreViewModel();

            // 映画テーブルのジャンルをselectするQueryを生成
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            // ToListAsyncを呼び出したタイミングでSQLが発行される
            // Distinctメソッドを呼ぶとselectにdistinctが追加される
            // select distinct genre from movie 
            movieGenreVM.Genres = await genreQuery.Distinct().ToListAsync();

            // 映画テーブルをselectするQueryを生成
            var movies = from m in _context.Movie
                         select m;
            
            // ジャンルが指定されている場合は、Queryに条件を追加する
            if (!String.IsNullOrEmpty(movieGenre)) {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            // タイトルが指定されている場合は、Queryに条件を追加する
            if (!String.IsNullOrEmpty(searchString)) {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            // ページングのため、selectでの取得開始位置を設定
            int offset = pageNumber * pageSize;
            // CountAsyncを呼び出した時点でcountのSQLが発行される
            // 条件は追加したジャンル、タイトルと同じになる
            // select count(*) from movie 
            // select count(*) from movie where genre=... > 0
            // select count(*) from movie where instr(title, ...) > 0
            // select count(*) from movie where genre=... and instr(title, ...) > 0
            // Containsメソッドを呼ぶと
            // SQLiteなのでinstrだが、ない場合はlike '%...%'になると思われる
            // StartsWithメソッドで '%...'、EndsWithメソッドで'...%'の条件になる
            // 上記だとAND条件になってしまうため、細かく制御したいなら下記のように記述する
            // movies = movies.Where(x => x.Genre == movieGenre || x.Title.Contains(searchString));
            int total = await movies.CountAsync();

            // 映画総件数がページングでのサイズより小さい場合はリストを初期化
            if (total < (pageNumber * pageSize)) {
                movieGenreVM.Movies = new();
            } else {
                // ToListAsyncを呼び出したタイミングでSQLが発行される
                // select... from movie where... Limit pageSize Offset offset
                // 条件はcountの場合と同様
                // 外部結合の例はAuthorsServiceを参照
                movieGenreVM.Movies = await movies.Skip(offset).Take(pageSize).ToListAsync();
            }
            movieGenreVM.TotalCount = total;
            
            return new ApiResult<MovieGenreViewModel>(movieGenreVM);
        }

        /// <summary>
        /// 映画詳細を取得します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>映画モデル</returns>
        [Route("find")]
        public async Task<ActionResult<ApiResult<Movie>>> Find(int? id)
        {
            var result = new ApiResult<Movie>();
            if (id == null)
            {
                result.AddErrorMessage("不正なリクエストが送信されました");
                return result;
            }

            // FirstOrDefaultでレコードがない場合は、nullを返却するように指定
            var movie = await _context.Movie
                    .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                result.AddDeletionErrorMessage("対象の映画は既に削除されています");
                return result;
            }

            return new ApiResult<Movie>(movie);
        }

        /// <summary>
        /// 映画を登録します。
        /// Bind属性でBindする対象プロパティを制限しています。
        /// その必要がない場合は指定する必要はありません。
        /// </summary>
        /// <param name="movie">映画モデル</param>
        /// <returns>メッセージ</returns>
        [Route("create")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResult<bool>>> Create([Bind("ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            var result = new ApiResult<bool>();

            // 属性による自動検証結果がfalseの場合はエラーを返却する
            // 属性の詳細はMovieクラスを参照
            // 基本的にクライアント側と同様のバリデーションを行うので
            // 冗長なのでここではエラーのみ返却する
            if (!ModelState.IsValid)
            {
                result.AddErrorMessage("不正なリクエストが送信されました");
                return result;
            }
            
            _context.Add(movie);
            await _context.SaveChangesAsync();

            result.Result = true;
            result.AddSuccessMessage("登録に成功しました");
            return result;
        }

        /// <summary>
        /// 映画を更新します。
        /// Bind属性でBindする対象プロパティを制限しています。
        /// その必要がない場合は指定する必要はありません。
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="movie">映画モデル</param>
        /// <returns>実行結果</returns>
        [Route("edit")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<ApiResult<bool>>> Edit([Bind("ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            var result = new ApiResult<bool>();
            if (!ModelState.IsValid)
            {
                result.AddErrorMessage("不正なリクエストが送信されました");
                return result;
            }

            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.ID))
                {
                    result.AddDeletionErrorMessage("対象の映画は既に削除されています");
                    return result;
                }
                result.AddOptimisticErrorMessage("対象の映画は既に更新されています。再度処理を実行してください");
                return result;
            }

            result.Result = true;
            result.AddSuccessMessage("更新に成功しました");
            return result;
        }

        /// <summary>
        /// 指定した映画を削除します
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>実行結果</returns>
        [Route("delete")]
        public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null) 
            {
                _context.Movie.Remove(movie);
                await _context.SaveChangesAsync();
            }

            var result = new ApiResult<bool>();
            result.Result = true;
            result.AddSuccessMessage("削除に成功しました");

            return result;
        }

        /// <summary>
        /// 映画が存在するかを検証します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>true/false</returns>
        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
