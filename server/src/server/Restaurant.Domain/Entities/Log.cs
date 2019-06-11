using System;

namespace Restaurant.Domain.Entities
{
    public class Log
    {
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Microsoft.Extensions.Logging.LogLevel"/>.
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// Gets or sets identity User Account
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets connection Remote IP Address
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets connection Remote Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets request Path
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets request QueryString Value
        /// </summary>
        public string QueryString { get; set; }

        public string ExceptionType { get; set; }

        public string ExceptionMessage { get; set; }

        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets request Method
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets response StatusCode
        /// </summary>
        public int HttpStatusCode { get; set; }

        public string LoadTimeInMilliseconds { get; set; }
    }
}