using System.Xml.Serialization;

namespace DartTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var service = new DartService();
            var dart = await service.GetStations();
        }
    }
    public class DartService 
    {
        private readonly HttpClient _client;
        private const string _stationsUrl = "https://api.irishrail.ie/realtime/realtime.asmx/getAllStationsXML";
        public DartService()
        {
            _client = new HttpClient();
        }
        public async Task<string[]> GetStations()
        {
            var response = await _client.GetAsync(_stationsUrl);
            response.EnsureSuccessStatusCode(); // Throw exception on bad status codes
            var xml = await response.Content.ReadAsStringAsync();

            ArrayOfObjStation result;

            XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfObjStation));

            using (StringReader reader = new StringReader(xml))
            {
                result = (ArrayOfObjStation)serializer.Deserialize(reader);
            }

            return result.objStation.Select(x => x.StationDesc).ToArray();
        }
    }
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://api.irishrail.ie/realtime/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://api.irishrail.ie/realtime/", IsNullable = false)]
    public class ArrayOfObjStation
    {

        private ArrayOfObjStationObjStation[] objStationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("objStation")]
        public ArrayOfObjStationObjStation[] objStation
        {
            get
            {
                return this.objStationField;
            }
            set
            {
                this.objStationField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://api.irishrail.ie/realtime/")]
    public class ArrayOfObjStationObjStation
    {

        private string stationDescField;

        private string stationAliasField;

        private decimal stationLatitudeField;

        private decimal stationLongitudeField;

        private string stationCodeField;

        private ushort stationIdField;

        /// <remarks/>
        public string StationDesc
        {
            get
            {
                return this.stationDescField;
            }
            set
            {
                this.stationDescField = value;
            }
        }

        /// <remarks/>
        public string StationAlias
        {
            get
            {
                return this.stationAliasField;
            }
            set
            {
                this.stationAliasField = value;
            }
        }

        /// <remarks/>
        public decimal StationLatitude
        {
            get
            {
                return this.stationLatitudeField;
            }
            set
            {
                this.stationLatitudeField = value;
            }
        }

        /// <remarks/>
        public decimal StationLongitude
        {
            get
            {
                return this.stationLongitudeField;
            }
            set
            {
                this.stationLongitudeField = value;
            }
        }

        /// <remarks/>
        public string StationCode
        {
            get
            {
                return this.stationCodeField;
            }
            set
            {
                this.stationCodeField = value;
            }
        }

        /// <remarks/>
        public ushort StationId
        {
            get
            {
                return this.stationIdField;
            }
            set
            {
                this.stationIdField = value;
            }
        }
    }
}
