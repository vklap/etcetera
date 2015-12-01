using System.Net;

namespace etcetera
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EtceteraException : Exception
    {
        public EtceteraException()
        {
        }

        public EtceteraException(string message) : base(message)
        {
        }

        public EtceteraException(string message, Exception innerException, HttpStatusCode statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        protected EtceteraException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}