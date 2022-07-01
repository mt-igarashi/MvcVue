using VueMvc.Enums;

namespace VueMvc.Result
{
    /// <summary>
    /// エラーメッセージ
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrorMessage()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="messageType">メッセージ種別</param>
        public ErrorMessage(string message, MessageType messageType)
        {
            Message = message;
            MessageType = messageType;
        }

        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// メッセージ種別
        /// </summary>
        public MessageType MessageType { get; set; }
    }
}