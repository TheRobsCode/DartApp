using Dart.Dtos;
using Xunit;

namespace DartTests
{
    public class TimeTableDtoTests
    {
        [Fact]
        public void IsDart_DartTrain_ShouldReturnTrue()
        {
            // Arrange
            var timeTable = new TimeTableDto
            {
                Station = "Connolly",
                Direction = "Northbound",
                Destination = "Howth",
                DueIn = 5,
                EstimateTime = "10:30",
                Origin = "Bray",
                Lastlocation = "Pearse",
                Traintype = "DART"
            };

            // Act
            var result = timeTable.IsDart();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDart_DartTrainLowerCase_ShouldReturnTrue()
        {
            // Arrange
            var timeTable = new TimeTableDto
            {
                Station = "Connolly",
                Direction = "Northbound",
                Destination = "Howth",
                DueIn = 5,
                EstimateTime = "10:30",
                Origin = "Bray",
                Lastlocation = "Pearse",
                Traintype = "dart"
            };

            // Act
            var result = timeTable.IsDart();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDart_CommuterTrain_ShouldReturnFalse()
        {
            // Arrange
            var timeTable = new TimeTableDto
            {
                Station = "Connolly",
                Direction = "Northbound",
                Destination = "Drogheda",
                DueIn = 10,
                EstimateTime = "10:45",
                Origin = "Dublin Connolly",
                Lastlocation = "",
                Traintype = "Commuter"
            };

            // Act
            var result = timeTable.IsDart();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDart_NullTrainType_ShouldReturnFalse()
        {
            // Arrange
            var timeTable = new TimeTableDto
            {
                Station = "Connolly",
                Direction = "Northbound",
                Destination = "Howth",
                DueIn = 5,
                EstimateTime = "10:30",
                Origin = "Bray",
                Lastlocation = "Pearse",
                Traintype = null
            };

            // Act
            var result = timeTable.IsDart();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDart_EmptyTrainType_ShouldReturnFalse()
        {
            // Arrange
            var timeTable = new TimeTableDto
            {
                Station = "Connolly",
                Direction = "Northbound",
                Destination = "Howth",
                DueIn = 5,
                EstimateTime = "10:30",
                Origin = "Bray",
                Lastlocation = "Pearse",
                Traintype = ""
            };

            // Act
            var result = timeTable.IsDart();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDart_WhitespaceTrainType_ShouldReturnFalse()
        {
            // Arrange
            var timeTable = new TimeTableDto
            {
                Station = "Connolly",
                Direction = "Northbound",
                Destination = "Howth",
                DueIn = 5,
                EstimateTime = "10:30",
                Origin = "Bray",
                Lastlocation = "Pearse",
                Traintype = "   "
            };

            // Act
            var result = timeTable.IsDart();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TimeTableDto_Properties_ShouldSetAndGetCorrectly()
        {
            // Arrange & Act
            var timeTable = new TimeTableDto
            {
                Station = "Pearse",
                Direction = "Southbound",
                Destination = "Greystones",
                DueIn = 12,
                EstimateTime = "11:15",
                Origin = "Malahide",
                Lastlocation = "Tara Street",
                Traintype = "DART"
            };

            // Assert
            Assert.Equal("Pearse", timeTable.Station);
            Assert.Equal("Southbound", timeTable.Direction);
            Assert.Equal("Greystones", timeTable.Destination);
            Assert.Equal(12, timeTable.DueIn);
            Assert.Equal("11:15", timeTable.EstimateTime);
            Assert.Equal("Malahide", timeTable.Origin);
            Assert.Equal("Tara Street", timeTable.Lastlocation);
            Assert.Equal("DART", timeTable.Traintype);
        }
    }
}
