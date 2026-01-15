using Dart.Helpers;
using Xunit;

namespace DartTests
{
    public class ColorHelperTests
    {
        [Fact]
        public void GetTrainBorderColor_DartTrain_ShouldReturnGreenColor()
        {
            // Arrange
            const string trainType = "DART";

            // Act
            var color = ColorHelper.GetTrainBorderColor(trainType);

            // Assert
            Assert.NotNull(color);
            // Green color for DART trains: #14A085
            Assert.Equal(0.078f, color.Red, 2);
            Assert.Equal(0.627f, color.Green, 2);
            Assert.Equal(0.522f, color.Blue, 2);
        }

        [Fact]
        public void GetTrainBorderColor_DartTrainLowerCase_ShouldReturnGreenColor()
        {
            // Arrange
            const string trainType = "dart";

            // Act
            var color = ColorHelper.GetTrainBorderColor(trainType);

            // Assert
            Assert.NotNull(color);
            Assert.Equal(0.078f, color.Red, 2);
        }

        [Fact]
        public void GetTrainBorderColor_OtherTrain_ShouldReturnOrangeColor()
        {
            // Arrange
            const string trainType = "Commuter";

            // Act
            var color = ColorHelper.GetTrainBorderColor(trainType);

            // Assert
            Assert.NotNull(color);
            // Orange color: #FF8C00
            Assert.Equal(1.0f, color.Red, 2);
            Assert.Equal(0.549f, color.Green, 2);
            Assert.Equal(0.0f, color.Blue, 2);
        }

        [Fact]
        public void GetTrainBorderColor_NullTrainType_ShouldReturnOrangeColor()
        {
            // Arrange
            string? trainType = null;

            // Act
            var color = ColorHelper.GetTrainBorderColor(trainType);

            // Assert
            Assert.NotNull(color);
            Assert.Equal(1.0f, color.Red, 2);
        }

        [Fact]
        public void GetCardBorderColor_ShouldReturnColor()
        {
            // Act
            var color = ColorHelper.GetCardBorderColor();

            // Assert
            Assert.NotNull(color);
        }

        [Fact]
        public void GetCardBackgroundColor_ShouldReturnColor()
        {
            // Act
            var color = ColorHelper.GetCardBackgroundColor();

            // Assert
            Assert.NotNull(color);
        }

        [Fact]
        public void GetDueTimeLabelColor_UrgentTime_ShouldReturnRedColor()
        {
            // Arrange - 5 minutes or less is urgent
            const int dueInMinutes = 3;

            // Act
            var color = ColorHelper.GetDueTimeLabelColor(dueInMinutes);

            // Assert
            Assert.NotNull(color);
            // Red color for urgent: #ff6b6b
            Assert.Equal(1.0f, color.Red, 2);
            Assert.Equal(0.42f, color.Green, 2);
            Assert.Equal(0.42f, color.Blue, 2);
        }

        [Fact]
        public void GetDueTimeLabelColor_NormalTime_ShouldReturnCyanOrTealColor()
        {
            // Arrange - More than 5 minutes is normal
            const int dueInMinutes = 10;

            // Act
            var color = ColorHelper.GetDueTimeLabelColor(dueInMinutes);

            // Assert
            Assert.NotNull(color);
            // Should be cyan or teal depending on theme
        }

        [Fact]
        public void GetSecondaryTextColor_ShouldReturnColor()
        {
            // Act
            var color = ColorHelper.GetSecondaryTextColor();

            // Assert
            Assert.NotNull(color);
        }

        [Fact]
        public void GetTertiaryTextColor_ShouldReturnColor()
        {
            // Act
            var color = ColorHelper.GetTertiaryTextColor();

            // Assert
            Assert.NotNull(color);
        }

        [Fact]
        public void GetArrowIconColor_ShouldReturnColor()
        {
            // Act
            var color = ColorHelper.GetArrowIconColor();

            // Assert
            Assert.NotNull(color);
        }

        [Fact]
        public void GetSelectedButtonColor_ShouldReturnPurpleColor()
        {
            // Act
            var color = ColorHelper.GetSelectedButtonColor();

            // Assert
            Assert.NotNull(color);
            // Purple color: #512BD4
            Assert.Equal(0.318f, color.Red, 2);
            Assert.Equal(0.169f, color.Green, 2);
            Assert.Equal(0.831f, color.Blue, 2);
        }

        [Fact]
        public void GetUnselectedButtonColor_ShouldReturnTransparentColor()
        {
            // Act
            var color = ColorHelper.GetUnselectedButtonColor();

            // Assert
            Assert.NotNull(color);
            Assert.Equal(0.0f, color.Alpha);
        }

        [Fact]
        public void GetSuccessButtonColor_ShouldReturnGreenColor()
        {
            // Act
            var color = ColorHelper.GetSuccessButtonColor();

            // Assert
            Assert.NotNull(color);
            // Green color: #4caf50
            Assert.Equal(0.298f, color.Red, 2);
            Assert.Equal(0.686f, color.Green, 2);
            Assert.Equal(0.314f, color.Blue, 2);
        }

        [Fact]
        public void GetRecentItemBackgroundColor_ShouldReturnColor()
        {
            // Act
            var color = ColorHelper.GetRecentItemBackgroundColor();

            // Assert
            Assert.NotNull(color);
        }
    }
}
