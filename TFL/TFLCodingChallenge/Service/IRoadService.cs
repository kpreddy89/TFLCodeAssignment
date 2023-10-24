using TFLCodingChallenge.DTOs;

namespace TFLCodingChallenge.Service
{
    public interface IRoadService
    {
        Task<ResponseDto> GetRoadStatus(string endpointUrl);
    }
}
