using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using VueMvc.Models;
using VueMvc.Result;

namespace VueMvc.Filter {

    /// <summary>
    /// トランザクションフィルター
    /// 例外発生時、または、エラーメッセージがある場合はロールバックを行います。
    /// それ以外の場合はトランザクションをコミットします。
    /// </summary>
    public class TransactionFilter : IAsyncActionFilter, IOrderedFilter
    {
        /// <summary>
        /// DBコンテキスト
        /// </summary>
        private readonly MvcMovieContext _context;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">DBコンテキスト</param>
        public TransactionFilter(MvcMovieContext context) {
            _context = context;
        }

        /// <summary>
        /// 実行順
        /// </summary>
        public int Order => 1;

        /// <summary>
        /// フィルター処理
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <param name="next">デリゲート</param>
        /// <returns>Task</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using(IDbContextTransaction tran = await _context.Database.BeginTransactionAsync())
            {
                var resultContext = await next();                
                if (resultContext.Exception != null || GetErrorMessages(resultContext.Result).Count > 0)
                {
                    tran.Rollback();
                }
                else
                {
                    tran.Commit();
                }
            }
        }

        /// <summary>
        /// エラーメッセージを取得します。
        /// </summary>
        /// <param name="objectResult">オブジェクトリザルト</param>
        /// <returns>エラーメッセージ</returns>
        private List<ErrorMessage> GetErrorMessages(object obj)
        {
            var objectResult = obj as ObjectResult;
            var messages = new List<ErrorMessage>();
            if (objectResult == null || objectResult.Value == null)
            {
                return messages;
            }

            try
            {
                var type = objectResult.Value.GetType();
                var prop = type.GetProperty(nameof(ApiResult<object>.ErrorMessages));

                var errors = prop.GetValue(objectResult.Value) as List<ErrorMessage>;
                messages = errors ?? messages;

                return messages;
            }
            catch
            {
                return messages;
            }
        }
    }
}