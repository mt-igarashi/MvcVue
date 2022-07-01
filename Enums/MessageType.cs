using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace VueMvc.Enums
{
    /// <summary>
    /// メッセージ種別
    /// </summary>
    public enum MessageType
    {

        /// <summary>
        /// デフォルト
        /// </summary>
        Default = 1,

        /// <summary>
        /// 削除エラー
        /// </summary>
        DeletionError = 2,

        /// <summary>
        /// 楽観ロックエラー
        /// </summary>
        OptimisticError = 3
    }
}