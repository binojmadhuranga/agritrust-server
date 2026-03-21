using System.Net;

namespace AgriTrust.API.Exceptions;

    public class ForbiddenException : Exception
    {
        public int StatusCode { get; }

        public ForbiddenException(string message)
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.Forbidden;
        }
    }
