using Microsoft.AspNetCore.Mvc;
using VueMvc.Result;

namespace VueMvc.Controllers
{
    /// <summary>
    /// ホームコントローラ
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 初期表示処理
        /// </summary>
        /// <returns>アクションリザルト</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// About表示処理
        /// </summary>
        /// <returns>アクションリザルト</returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        /// <summary>
        /// Contact表示処理
        /// </summary>
        /// <returns>アクションリザルト</returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Privacy表示処理
        /// </summary>
        /// <returns>アクションリザルト</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Error表示処理
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <returns>アクションリザルト</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            return View(new ErrorMessage { Message = message });
        }
    }
}
