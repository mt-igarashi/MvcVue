using System.Collections.Generic;
using VueMvc.Entities;

namespace VueMvc.Models
{
    /// <summary>
    /// 著者ビューモデル
    /// </summary>
    public class AuthorViewModel
    {
        /// <summary>
        /// 著者リスト
        /// </summary>
        public List<AuthorEntity> Authors { get; set; }
    }
}