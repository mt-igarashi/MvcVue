namespace VueMvc.Models
{
    /// <summary>
    /// エラービューモデル
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// リクエストID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// リクエスID有無
        /// </summary>
        /// <returns>true/false</returns>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}