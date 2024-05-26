using Xunit;

namespace Util.Tests;
public class RandomTests
{
    [Fact]
    public void RandomStringTest()
    {
        // Arrange
        int length = 10;

        // Act
        string result = Rand.RandomString(length);

        // Assert
        Assert.Equal(length, result.Length);
    }

    [Fact]
    public void RandomString_ShouldThrowArgumentException_ForNonPositiveLength()
    {
        // Arrange
        int length = 0;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Rand.RandomString(length));
    }

}