using System.Collections.Generic;
using System.Linq;
using VueMvc.Enums;

namespace VueMvc.Result
{
    /// <summary>
    /// API実行結果
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ApiResult()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="t">実行結果</param>
        public ApiResult(T result)
        {
            Result = result;
        }

        /// <summary>
        /// 処理成功フラグ
        /// </summary>
        public bool Success { get; set; } = true;
        
        /// <summary>
        /// 実行結果
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 成功メッセージ
        /// </summary>
        public List<string> SuccessMessages { get; set; } = new List<string>();

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public List<ErrorMessage> ErrorMessages { get; set; } = new List<ErrorMessage>();

        /// <summary>
        /// 楽観ロックエラー存在フラグ
        /// </summary>
        public bool ExistOptimisticError
        {
            get
            {
                return ErrorMessages.Any(e => e.MessageType == MessageType.OptimisticError);
            }
        }

        /// <summary>
        /// 削除エラー存在フラグ
        /// </summary>
        public bool ExistDeletionError
        {
            get
            {
                return ErrorMessages.Any(e => e.MessageType == MessageType.DeletionError);
            }
        }

        /// <summary>
        /// 成功メッセージを追加します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void AddSuccessMessage(string message)
        {
            SuccessMessages.Add(message);
        }

        /// <summary>
        /// エラーメッセージを追加します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void AddErrorMessage(string message)
        {
            ErrorMessages.Add(new ErrorMessage(message, MessageType.Default));
        }

        /// <summary>
        /// 楽観ロックエラーメッセージを追加します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void AddOptimisticErrorMessage(string message)
        {
            ErrorMessages.Add(new ErrorMessage(message, MessageType.OptimisticError));
        }

        /// <summary>
        /// 削除エラーメッセージを追加します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void AddDeletionErrorMessage(string message)
        {
            ErrorMessages.Add(new ErrorMessage(message, MessageType.DeletionError));
        }
    }
}