using Microsoft.EntityFrameworkCore;

namespace VueMvc.Models
{
    /// <summary>
    /// DBコンテキスト
    /// </summary>
    public class MvcMovieContext : DbContext
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="options">DBコンテキストオプション</param>
        public MvcMovieContext (DbContextOptions<MvcMovieContext> options) : base(options)
        {
        }
        
        /// <summary>
        /// 映画テーブル
        /// </summary>
        public DbSet<Movie> Movie { get; set; }

        /// <summary>
        /// 著者テーブル
        /// </summary>
        /// <value></value>
        public DbSet<Author> Author { get; set; }
    }
}