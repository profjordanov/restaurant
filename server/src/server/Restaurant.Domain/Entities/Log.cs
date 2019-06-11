using System;

namespace Restaurant.Domain.Entities
{
    public class Log
    {
        public long Id { get; set; }

        public string LogLevel { get; set; }

        public string Username { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Url { get; set; }

        public string QueryString { get; set; }

        public string ExceptionType { get; set; }

        public string ExceptionMessage { get; set; }

        public DateTime Date { get; set; }

        public string HttpMethod { get; set; }

        public int HttpStatusCode { get; set; }
    }
}