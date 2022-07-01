using System.Collections.Generic;

namespace VueMvc.Models
{
    /// <summary>
    /// 映画ビューモデル
    /// </summary>
    public class MovieGenreViewModel
    {
        /// <summary>
        /// 映画リスト
        /// </summary>
        public List<Movie> Movies { get; set; }

        /// <summary>
        /// 映画リスト総件数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// ジャンルリスト
        /// </summary>
        public List<string> Genres { get; set; }

        /// <summary>
        /// ジャンル
        /// </summary>
        public string MovieGenre { get; set; }

        /// <summary>
        /// 検索文字列(タイトル)
        /// </summary>
        public string SearchString { get; set; }
    }
}