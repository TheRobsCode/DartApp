namespace Dart.Xml
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://api.irishrail.ie/realtime/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://api.irishrail.ie/realtime/", IsNullable = false)]
    public class ArrayOfObjStationData
    {

        private ArrayOfObjStationDataObjStationData[] objStationDataField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("objStationData")]
        public ArrayOfObjStationDataObjStationData[] objStationData
        {
            get
            {
                return this.objStationDataField;
            }
            set
            {
                this.objStationDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://api.irishrail.ie/realtime/")]
    public partial class ArrayOfObjStationDataObjStationData
    {

        private System.DateTime servertimeField;

        private string traincodeField;

        private string stationfullnameField;

        private string stationcodeField;

        private System.DateTime querytimeField;

        private string traindateField;

        private string originField;

        private string destinationField;

        private string origintimeField;

        private string destinationtimeField;

        private string statusField;

        private string lastlocationField;

        private byte dueinField;

        private sbyte lateField;

        private string exparrivalField;

        private string expdepartField;

        private string scharrivalField;

        private string schdepartField;

        private string directionField;

        private string traintypeField;

        private string locationtypeField;

        /// <remarks/>
        public System.DateTime Servertime
        {
            get
            {
                return this.servertimeField;
            }
            set
            {
                this.servertimeField = value;
            }
        }

        /// <remarks/>
        public string Traincode
        {
            get
            {
                return this.traincodeField;
            }
            set
            {
                this.traincodeField = value;
            }
        }

        /// <remarks/>
        public string Stationfullname
        {
            get
            {
                return this.stationfullnameField;
            }
            set
            {
                this.stationfullnameField = value;
            }
        }

        /// <remarks/>
        public string Stationcode
        {
            get
            {
                return this.stationcodeField;
            }
            set
            {
                this.stationcodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
        public System.DateTime Querytime
        {
            get
            {
                return this.querytimeField;
            }
            set
            {
                this.querytimeField = value;
            }
        }

        /// <remarks/>
        public string Traindate
        {
            get
            {
                return this.traindateField;
            }
            set
            {
                this.traindateField = value;
            }
        }

        /// <remarks/>
        public string Origin
        {
            get
            {
                return this.originField;
            }
            set
            {
                this.originField = value;
            }
        }

        /// <remarks/>
        public string Destination
        {
            get
            {
                return this.destinationField;
            }
            set
            {
                this.destinationField = value;
            }
        }

        /// <remarks/>
        public string Origintime
        {
            get
            {
                return this.origintimeField;
            }
            set
            {
                this.origintimeField = value;
            }
        }

        /// <remarks/>
        public string Destinationtime
        {
            get
            {
                return this.destinationtimeField;
            }
            set
            {
                this.destinationtimeField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Lastlocation
        {
            get
            {
                return this.lastlocationField;
            }
            set
            {
                this.lastlocationField = value;
            }
        }

        /// <remarks/>
        public byte Duein
        {
            get
            {
                return this.dueinField;
            }
            set
            {
                this.dueinField = value;
            }
        }

        /// <remarks/>
        public sbyte Late
        {
            get
            {
                return this.lateField;
            }
            set
            {
                this.lateField = value;
            }
        }

        /// <remarks/>
        public string Exparrival
        {
            get
            {
                return this.exparrivalField;
            }
            set
            {
                this.exparrivalField = value;
            }
        }

        /// <remarks/>
        public string Expdepart
        {
            get
            {
                return this.expdepartField;
            }
            set
            {
                this.expdepartField = value;
            }
        }

        /// <remarks/>
        public string Scharrival
        {
            get
            {
                return this.scharrivalField;
            }
            set
            {
                this.scharrivalField = value;
            }
        }

        /// <remarks/>
        public string Schdepart
        {
            get
            {
                return this.schdepartField;
            }
            set
            {
                this.schdepartField = value;
            }
        }

        /// <remarks/>
        public string Direction
        {
            get
            {
                return this.directionField;
            }
            set
            {
                this.directionField = value;
            }
        }

        /// <remarks/>
        public string Traintype
        {
            get
            {
                return this.traintypeField;
            }
            set
            {
                this.traintypeField = value;
            }
        }

        /// <remarks/>
        public string Locationtype
        {
            get
            {
                return this.locationtypeField;
            }
            set
            {
                this.locationtypeField = value;
            }
        }
    }


}
