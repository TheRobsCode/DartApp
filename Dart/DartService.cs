using Dart.Dtos;
using System.Xml.Serialization;
using Dart.Xml;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Dart
{
    public interface IDartService
    {
        Task<StationsDto[]> GetStations();
        Task<TimeTableDto[]> GetTimeTable(string station);
    }

    public class DartService : IDartService
    {
        private readonly ILogger<DartService> _logger;
        private static readonly HttpClient _client = CreateHttpClient();
        private const string StationsUrl = "https://api.irishrail.ie/realtime/realtime.asmx/getAllStationsXML";
        private const string TimeTableUrl = "https://api.irishrail.ie/realtime/realtime.asmx/getStationDataByNameXML?StationDesc=";

        public DartService(ILogger<DartService> logger)
        {
            _logger = logger;
        }

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
            _logger.LogInformation("Fetching all stations from Irish Rail API");
            var res = await DoHttpRequest<ArrayOfObjStation>(StationsUrl);
            if (res?.objStation == null)
            {
                _logger.LogWarning("No station data returned from API");
                return [];
            }

            var stations = res.objStation
                .Select(x => new StationsDto { StationDesc = x.StationDesc })
                .ToArray();

            _logger.LogInformation("Successfully retrieved {StationCount} stations", stations.Length);
            return stations;
        }

        public async Task<TimeTableDto[]> GetTimeTable(string station)
        {
            _logger.LogInformation("Fetching timetable for station: {Station}", station);
            var res = await DoHttpRequest<ArrayOfObjStationData>(TimeTableUrl + Uri.EscapeDataString(station));
            if (res?.objStationData == null)
            {
                _logger.LogWarning("No timetable data returned for station: {Station}", station);
                return [];
            }

            var timetable = res.objStationData
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

            _logger.LogInformation("Successfully retrieved {TrainCount} train entries for station: {Station}", timetable.Length, station);
            return timetable;
        }

        private async Task<T?> DoHttpRequest<T>(string url) where T : class
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogDebug("HTTP GET request to: {Url}", url);

            try
            {
                var response = await _client.GetAsync(url);
                stopwatch.Stop();

                _logger.LogDebug("HTTP response received in {ElapsedMs}ms with status: {StatusCode}",
                    stopwatch.ElapsedMilliseconds, (int)response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("HTTP request failed with status code: {StatusCode} for URL: {Url}",
                        (int)response.StatusCode, url);
                    return null;
                }

                var xml = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(xml))
                {
                    _logger.LogWarning("Empty XML response received from URL: {Url}", url);
                    return null;
                }

                _logger.LogDebug("Deserializing XML response of type {Type}, length: {Length}",
                    typeof(T).Name, xml.Length);

                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StringReader(xml);
                var result = serializer.Deserialize(reader) as T;

                if (result == null)
                {
                    _logger.LogWarning("XML deserialization returned null for type {Type}", typeof(T).Name);
                }

                return result;
            }
            catch (TaskCanceledException ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "HTTP request timed out after {ElapsedMs}ms for URL: {Url}",
                    stopwatch.ElapsedMilliseconds, url);
                return null;
            }
            catch (HttpRequestException ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "HTTP request exception occurred after {ElapsedMs}ms for URL: {Url}",
                    stopwatch.ElapsedMilliseconds, url);
                return null;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Unexpected error in HTTP request after {ElapsedMs}ms for URL: {Url}",
                    stopwatch.ElapsedMilliseconds, url);
                return null;
            }
        }
    }
}
