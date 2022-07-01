using Microsoft.AspNetCore.Mvc;

namespace VueMvc.Controllers
{
    /// <summary>
    /// Hello Worldコントローラ
    /// </summary>
    public class HelloWorldController : Controller
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
        /// Welcom表示処理
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="numTimes">回数</param>
        /// <returns>アクションリザルト</returns>
        public IActionResult Welcome(string name, int numTimes = 1) {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}