namespace Dart.Xml
{
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
