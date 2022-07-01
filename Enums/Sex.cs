using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace VueMvc.Enums
{
    /// <summary>
    /// 性別
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 男性
        /// </summary>
        [Display(Name = "男性")]
        Male = 1,

        /// <summary>
        /// 女性
        /// </summary>
        [Display(Name = "女性")]
        Female = 2
    }

    /// <summary>
    /// 性別拡張クラス
    /// </summary>
    public static class SexExtension
    {
        /// <summary>
        /// 値を取得します。
        /// </summary>
        /// <param name="sex">性別</param>
        /// <returns>数値</returns>
        public static int getValue (this Sex sex)
        {
            return (int)sex;
        }

        /// <summary>
        /// 文字列を取得します。
        /// </summary>
        /// <param name="sex">性別</param>
        /// <returns>文字列</returns>

        public static string getStringValue (this Sex sex)
        {
            return ((int)sex).ToString();
        }

        /// <summary>
        /// ディスプレイ名を取得します。
        /// </summary>
        /// <param name="sex">性別</param>
        /// <returns>ディスプレイ名</returns>
        public static string getDisplayName(this Sex sex)
        {
            DisplayAttribute attr = sex.GetType().GetCustomAttributes(typeof(DisplayAttribute)).FirstOrDefault() as DisplayAttribute;
            return attr?.GetName() ?? string.Empty;
        }
    }
}