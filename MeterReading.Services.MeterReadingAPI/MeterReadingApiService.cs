using AutoMapper;
using MeterReading.Core.Models.MeterReading;
using MeterReading.Core.Services.MeterReadingApi;
using MeterReading.Services.MeterReadingAPI.Models.MeterReading;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeterReading.Services.MeterReadingAPI
{
    public class MeterReadingApiService : IMeterReadingApiService
    {
        IMapper _mapper;

        public MeterReadingApiService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task PostMeterReadingAsync(string uri, string deviceId, MeterReadingEntry model, CancellationToken token)
        {
            var request = _mapper.Map<MeterReadingEntry, MeterReadingEntryApiModel>(model);
            request.DeviceId = deviceId;

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(uri, content, token);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task PostMeterImageAsync(string uri, Stream image, string filename, CancellationToken token)
        {
            var content = new StreamContent(image);

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(content, "File", filename);
                var response = await client.PostAsync(uri, formData, token);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
