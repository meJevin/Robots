using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robots.Application.Tests.Extensions
{
    public class OrientationExtensionsTests
    {
        [Theory]
        [InlineData(Orientation.Unknown, OrientationNames.Unknown)]
        [InlineData(Orientation.North, OrientationNames.North)]
        [InlineData(Orientation.West, OrientationNames.West)]
        [InlineData(Orientation.South, OrientationNames.South)]
        [InlineData(Orientation.East, OrientationNames.East)]
        public void ToReadable_Converts_Orientation_To_Correct_Readable_String(
            Orientation orientation, string expectedString)
        {
            // Act
            var result = OrientationExtensions.ToReadable(orientation);

            // Assert
            result.Should().Be(expectedString);
        }

        [Theory]
        [InlineData(OrientationNames.North, Orientation.North)]
        [InlineData(OrientationNames.West, Orientation.West)]
        [InlineData(OrientationNames.South, Orientation.South)]
        [InlineData(OrientationNames.East, Orientation.East)]
        public void TryParse_Parses_Correct_String_Into_Orientation_And_Returns_True(
            string orientationStr, Orientation expectedOrientation)
        {
            // Act
            var isParsed = OrientationExtensions.TryParse(orientationStr, out var readableOrientation);

            // Assert
            isParsed.Should().Be(true);
            readableOrientation.Should().Be(expectedOrientation);
        }

        [Fact]
        public void TryParse_Parses_Incorrect_String_Into_Unknown_Orientation_And_Returns_False(
            )
        {
            // Act
            var isParsed = OrientationExtensions.TryParse("adsasdgdf", out var parsedOrientation);

            isParsed.Should().Be(false);
            parsedOrientation.Should().Be(Orientation.Unknown);
        }
    }
}
