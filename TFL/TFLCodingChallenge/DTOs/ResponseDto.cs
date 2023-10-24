using System.Net;

namespace TFLCodingChallenge.DTOs
{
    public class ResponseDto
    {
        public ResponseDto(HttpStatusCode httpStatusCode, string message)
        {
            HttpStatusCode = httpStatusCode;
            Message = message;
        }
        public HttpStatusCode HttpStatusCode {  get; set; }

        public string? Message { get; set; }

        public RoadDto? RoadDetails { get; set; }

    }
}
