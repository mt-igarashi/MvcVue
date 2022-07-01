using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueMvc.Models
{
    /// <summary>
    /// 著者テーブルモデル
    /// </summary>
    public class Author 
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <value></value>
        public int ID { get; set; }

        /// <summary>
        /// 映画ID
        /// </summary>
        public int MovieID { get; set; }

        /// <summary>
        /// 映画モデル
        /// </summary>
        [ForeignKey("MovieID")]
        public Movie Movie { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Display(Name = "名前")]
        [Required(ErrorMessage = "名前は必須項目です")]
        [StringLength(60, ErrorMessage="名前は６０文字以内で入力してください")]
        public string Name { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        [Display(Name = "性別")]
        [Required(ErrorMessage = "性別は必須項目です")]
        [StringLength(1, MinimumLength = 1, ErrorMessage="性別を選択してください")]
        public string Sex { get; set; }

        /// <summary>
        /// 年齢
        /// </summary>
        [Display(Name = "年齢")]
        [Required(ErrorMessage = "年齢は必須項目です")]
        [Range(1, 100, ErrorMessage="年齢は1～100までの数値を入力してください")]
        public int Age { get; set; }
        
        /// <summary>
        /// 更新日付
        /// </summary>
        [ConcurrencyCheck]
        public DateTime UpdateDate { get; set; }
    }
}