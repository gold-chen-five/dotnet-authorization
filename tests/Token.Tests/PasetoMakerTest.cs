using Xunit;

namespace Token.Tests;
public class PasetoMakerTest
{
    private readonly PasetoMaker _pasetoMaker;
    public PasetoMakerTest()
    {
        string ValidSymmetricKey = Util.Rand.RandomString(32); 
        _pasetoMaker = new PasetoMaker(ValidSymmetricKey);
    }

    [Theory]
    [InlineData(32)]
    public void PasetoMaker_ShouldNotThrowExceptionOnCreation(int symmetricKeyLength)
    {
        // Arrange
        string symmetricKey = Util.Rand.RandomString(symmetricKeyLength);

        // Act & Assert
        var exception = Record.Exception(() => new PasetoMaker(symmetricKey));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(31)]
    [InlineData(30)]
     public void PasetoMaker_InvalidSymmetricKey_ShouldThrowException(int symmetricKeyLength)
    {
        // Arrange
        string symmetricKey = Util.Rand.RandomString(symmetricKeyLength);

        // Act & Assert
        Assert.Throws<Exception>(() => new PasetoMaker(symmetricKey));
    }


    [Fact]
    public void CreateToken_ShouldReturnValidPayload()
    {
        // Arrange
        var username = Util.Rand.RandomUser();
        TimeSpan duration = TimeSpan.FromMinutes(15);
        var issueAt = DateTime.UtcNow;
        var expiresAt = issueAt + duration;

        // Act
        var (token, payload) = _pasetoMaker.CreateToken(username, duration);

        // Assert
        Assert.False(string.IsNullOrEmpty(token));
        Assert.True(Guid.TryParse(payload.ID.ToString(), out _));
        Assert.Equal(username, payload.Username);
        Assert.InRange(payload.IssuedAt, issueAt.AddSeconds(-1), issueAt.AddSeconds(1));
        Assert.InRange(payload.ExpiresAt, expiresAt.AddSeconds(-1), expiresAt.AddSeconds(1));
    }

    [Fact]
    public void VerifyToken_ShouldReturnValidPayload()
    {
        // Arrange
        var username = Util.Rand.RandomUser();
        TimeSpan duration = TimeSpan.FromMinutes(15);

        // Act
        var (token, createdPayload) = _pasetoMaker.CreateToken(username, duration);
        var verifyPayload = _pasetoMaker.VerifyToken(token);

        // Assert
        Assert.Equal(createdPayload.ID, verifyPayload.ID);
        Assert.Equal(createdPayload.Username, verifyPayload.Username);
        Assert.InRange(createdPayload.IssuedAt, verifyPayload.IssuedAt.AddSeconds(-1), verifyPayload.IssuedAt.AddSeconds(1));
        Assert.InRange(createdPayload.ExpiresAt, verifyPayload.ExpiresAt.AddSeconds(-1), verifyPayload.ExpiresAt.AddSeconds(1));
    }

    [Theory]
    [InlineData("invalid-token-string")]
    [InlineData("")]
    public void VerifyToken_InvalidToken_ShouldThrowException(string invalidToken)
    {
        // Act & Assert
        Assert.Throws<Exception>(() => _pasetoMaker.VerifyToken(invalidToken));
    }

    [Fact]
    public void VerifyToken_ExpiredToken_ShouldThrowException()
    {
        // Arrange
        var username = Util.Rand.RandomUser();
        TimeSpan duration = TimeSpan.FromSeconds(-1);

        // Act
        var (expiredToken, _) = _pasetoMaker.CreateToken(username, duration);

        // Assert
        var ex = Assert.Throws<Exception>(() => _pasetoMaker.VerifyToken(expiredToken));
        Assert.Equal("Token validation failed.", ex.Message);
    }

}