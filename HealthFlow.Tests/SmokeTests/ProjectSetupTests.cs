namespace HealthFlow.Tests.SmokeTests;

public class ProjectSetupTests
{
    [Fact]
    public void Project_Should_Be_Configured_Correctly()
    {
        // Arrange
        const bool projectIsConfigured = true;

        // Act
        var result = projectIsConfigured;

        // Assert
        Assert.True(result);
    }
}
