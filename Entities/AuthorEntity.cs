using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VueMvc.Enums;

namespace VueMvc.Entities
{
    /// <summary>
    /// 著者エンティティ
    /// </summary>
    public class AuthorEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        /// <value></value>
        public int ID { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [Display(Name = "タイトル")]
        public string Title { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Display(Name = "名前")]
        public string Name { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        [Display(Name = "性別")]
        public string Sex { get; set; }

        /// <summary>
        /// 表示用性別
        /// </summary>
        public Sex? DisplaySex {
            get
            {
                return EnumExtension.Parse<Sex>(this.Sex);
            }
        }

        /// <summary>
        /// 年齢
        /// </summary>
        [Display(Name = "年齢")]
        public int Age {get; set; }
    }
}