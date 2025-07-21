
namespace Dart.Dtos
{
    public class StationsDto
    {
        public string StationDesc { get; set; }
    }

    public class TimeTableDto
    {
        public string Station { get; set; }
        public string Direction { get; set; }
        public string Destination { get; set; }
        public int DueIn { get; set; }
        public string EstimateTime { get; set; }
        public string Origin { get; set; }
        public string Lastlocation { get; set; }
    }


}
