using System;
using System.Threading.Tasks;
using Craft.Core;
using Craft.Core.Contracts;
using Craft.Tests.Fixtures;
using NUnit.Framework;

namespace Craft.Tests
{
    public class ProcessCommandAsyncShould
    {
        private IEngine engine;

        [SetUp]
        public void Setup()
        {
            this.engine = new Engine();
            new LaunchSettingsFixture();
        }

        [Test]
        public async Task CreateFreshdeskContact_WhenDataIsValid()
        {
            // Arrange
            string command = "CreateFreshdeskContact kristiyanivanovx testdeskapp";

            // Act
            string? result = await this.engine.ProcessCommandAsync(command);

            // Assert
            Assert.True(result?.Contains("Created a new Freshdesk contact!"));
        }

        [Test]
        public void ThrowExceptionWhenCommandIsEmpty()
        {
            // Arrange
            string command = "";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this.engine.ProcessCommandAsync(command))
                ?.Message
                ?.Contains("Invalid data provided!");
        }

        [Test]
        public void ThrowExceptionWhenOnlyCommandProvided()
        {
            // Arrange
            string command = "CreateFreshdeskContact";

            // Act & Assert.gitignore

            Assert.ThrowsAsync<ArgumentException>(async () => await this.engine.ProcessCommandAsync(command))
                ?.Message
                ?.Contains("Failed to parse \"CreateFreshdeskContact\" command parameters.");
        }

        [Test]
        public void ThrowExceptionWhenOnlyCommandAndParameterAreProvided()
        {
            // Arrange
            string command = "CreateFreshdeskContact x";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await this.engine.ProcessCommandAsync(command))
                ?.Message
                ?.Contains("Failed to parse \"CreateFreshdeskContact\" command parameters.");
        }
    }
}