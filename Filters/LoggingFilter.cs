using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using VueMvc.Logs;

namespace VueMvc.Filter {

    /// <summary>
    /// ログフィルター
    /// </summary>
    public class LoggingFilter : IAsyncActionFilter, IOrderedFilter
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        public LoggingFilter(ILogger<LoggingFilter> logger) {
            _logger = logger;
        }

        /// <summary>
        /// 実行順
        /// </summary>
        public int Order => 2;

        /// <summary>
        /// フィルター処理
        /// </summary>
        /// <param name="context">コンテキスト</param>
        /// <param name="next">デリゲート</param>
        /// <returns>Task</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.ActionArguments.ToList().ForEach((p) => {
                _logger.LogInformation("request parameter key : {key}, param : {param}",
                                        p.Key, p.Value.ToFormattedString());
            });
            
            var resultContext = await next();

            if (resultContext.Exception is not null) {
                _logger.LogError(resultContext.Exception, "exception occured. while executing resuest.");
            }
            else
            {
                var result = resultContext.Result as ObjectResult;
                if (result is not null)
                {
                    _logger.LogInformation("response : {response}", result.Value.ToFormattedString());
                }
            }
        }
    }
}