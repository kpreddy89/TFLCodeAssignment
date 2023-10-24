using System.Net;
using System.Text.Json;
using TFLCodingChallenge.DTOs;

namespace TFLCodingChallenge.Service
{
    public class RoadService : IRoadService
    {
        private readonly IHttpClientFactory _clientFactory;

        public RoadService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ResponseDto> GetRoadStatus(string endpointUrl)
        {
            try
            {
                var responseMsg = await _clientFactory.CreateClient().GetAsync(endpointUrl);

                var responseData = await responseMsg.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var responseDto = new ResponseDto(responseMsg.StatusCode, null);

                if (responseMsg.IsSuccessStatusCode)
                {
                    var roads = JsonSerializer.Deserialize<List<RoadDto>>(responseData, options);
                    if (roads is not null)
                    {
                        responseDto.RoadDetails = roads[0];
                    }
                    return responseDto;
                }
                else
                {
                    responseDto = JsonSerializer.Deserialize<ResponseDto>(responseData, options);
                    return responseDto ?? new ResponseDto(HttpStatusCode.InternalServerError, "DATA_NOT_FOUND");
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
