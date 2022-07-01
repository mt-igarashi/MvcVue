using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VueMvc.Attribute
{ 
  /// <summary>
  /// 配列内に存在するかを検証するクラス
  /// </summary>
 [AttributeUsage(AttributeTargets.Property, 
                  AllowMultiple = false)]
  public class InArrayAttribute : 
               ValidationAttribute, IClientModelValidator
  {
    /// <summary>
    /// 値リスト
    /// </summary>
    private string _opts;
 
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="opts">値リスト</param>
    public InArrayAttribute(string opts)
    {
      this._opts = opts;
      this.ErrorMessage = "{0} は「{1}」のいずれかで指定してください。";
    }
 
    /// <summary>
    /// エラーメッセージをフォーマットします。
    /// </summary>
    /// <param name="name">名前</param>
    /// <returns>エラーメッセージ</returns>
    public override string FormatErrorMessage(string name)
    {
      return String.Format(CultureInfo.CurrentCulture, 
                           ErrorMessageString, name, _opts);
    }
 
    /// <summary>
    /// 検証を実行します(サーバーサイド)
    /// </summary>
    /// <param name="value">値</param>
    /// <param name="validationContext">コンテキスト</param>
    /// <returns>バリデーションリザルト</returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      // 入力値が空の場合は検証をスキップ 
      if (value == null) { return ValidationResult.Success; }
 
      // カンマ区切りテキストを分解し、入力値valueと比較 
      if (Array.IndexOf(_opts.Split(','), value) == -1)
      {
        return new ValidationResult("正しい区分を選択してください");
      }

      return ValidationResult.Success;
    }
 
    /// <summary>
    /// クライアントバリデーションに必要な属性を追加します。
    /// </summary>
    /// <param name="context">コンテキスト</param>
    public void AddValidation(ClientModelValidationContext context)
    {
      MergeAttribute(context.Attributes, "data-val", "true");

      var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
      MergeAttribute(context.Attributes, "data-val-inarray", errorMessage);
      MergeAttribute(context.Attributes, "data-val-inarray-opts", this._opts);
    }
 
    /// <summary>
    /// AddValidationメソッドで使うヘルパーメソッド
    /// </summary>
    /// <param name="attributes">属性</param>
    /// <param name="key">キー</param>
    /// <param name="value">値</param>
    /// <returns>true/false</returns>
    private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
      if (attributes.ContainsKey(key))
      {
        return false;
      }

      attributes.Add(key, value);
      return true;
    }
  }
}