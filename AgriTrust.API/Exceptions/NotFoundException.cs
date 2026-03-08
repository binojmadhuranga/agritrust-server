using System.Net;

namespace AgriTrust.API.Exceptions
{
    public class NotFoundException : Exception
    {
        public int StatusCode { get; }

        public NotFoundException(string message)
            : base(message)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}