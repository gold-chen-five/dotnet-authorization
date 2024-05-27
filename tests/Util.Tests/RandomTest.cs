using Xunit;

namespace Util.Tests;
public class RandomTests
{
    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(100)]
    public void RandomString_NumberGreaterThanZero_ReturnRandomStringMatchLength(int length)
    {
        // Act
        string result = Rand.RandomString(length);

        // Assert
        Assert.Equal(length, result.Length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RandomString_NumberLowerThanOne_ReturnException(int length)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Rand.RandomString(length));
    }

}