using Dart.Dtos;
using System.Xml.Serialization;
using Dart.Xml;

namespace Dart
{
    public interface IDartService
    {
        Task<StationsDto[]> GetStations();
        Task<TimeTableDto[]> GetTimeTable(string station);
    }

    public class DartService : IDartService
    {
        private static readonly HttpClient _client = CreateHttpClient();
        private const string StationsUrl = "https://api.irishrail.ie/realtime/realtime.asmx/getAllStationsXML";
        private const string TimeTableUrl = "https://api.irishrail.ie/realtime/realtime.asmx/getStationDataByNameXML?StationDesc=";

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            return new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(15) };
        }

        public async Task<StationsDto[]> GetStations()
        {
            var res = await DoHttpRequest<ArrayOfObjStation>(StationsUrl);
            if (res?.objStation == null)
                return [];

            return res.objStation
                .Select(x => new StationsDto { StationDesc = x.StationDesc })
                .ToArray();
        }

        public async Task<TimeTableDto[]> GetTimeTable(string station)
        {
            var res = await DoHttpRequest<ArrayOfObjStationData>(TimeTableUrl + Uri.EscapeDataString(station));
            if (res?.objStationData == null)
                return [];

            return res.objStationData
                .Select(x => new TimeTableDto
                {
                    Station = x.Stationfullname,
                    Origin = x.Origin,
                    Destination = x.Destination,
                    DueIn = x.Duein,
                    EstimateTime = x.Exparrival,
                    Direction = x.Direction,
                    Lastlocation = x.Lastlocation
                })
                .ToArray();
        }

        private async Task<T?> DoHttpRequest<T>(string url) where T : class
        {
            try
            {
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return null;

                var xml = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(xml))
                    return null;

                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StringReader(xml);
                return serializer.Deserialize(reader) as T;
            }
            catch
            {
                // Optionally log the exception
                return null;
            }
        }
    }
}
