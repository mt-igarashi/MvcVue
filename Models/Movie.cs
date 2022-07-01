using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueMvc.Models
{
    /// <summary>
    /// 映画テーブルモデル
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [Display(Name = "タイトル")]
        [Required(ErrorMessage = "タイトルは必須項目です")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        /// <summary>
        /// 公開日
        /// </summary>
        [Display(Name = "公開日")]
        [Required(ErrorMessage = "公開日は必須項目です")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// ジャンル
        /// </summary>
        /// <value></value>
        [Display(Name = "ジャンル")]
        [Required(ErrorMessage = "ジャンルは必須項目です")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(30)]
        public string Genre { get; set; }

        /// <summary>
        /// 価格
        /// </summary>
        /// <value></value>
        [Display(Name = "価格")]
        [Required(ErrorMessage = "価格は必須項目です")]
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// 評価
        /// </summary>
        /// <value></value>
        [Display(Name = "評価")]
        [Required(ErrorMessage = "評価は必須項目です")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        public string Rating { get; set; }
    }
}