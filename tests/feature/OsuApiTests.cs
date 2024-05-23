using NUnit.Framework.Legacy;
using OsuMultiplayerLobbyFinder.feature;
using OsuMultiplayerLobbyFinder.models;
using System.Text.Json;

namespace tests.feature
{
    public class OsuApiTests
    {
        private OsuApi api;
        private ConfigModel config;
        private string invalidKey;
        private string validKey;

        [SetUp]
        protected void SetUp()
        {
            api = new OsuApi();

            config = JsonSerializer.Deserialize<ConfigModel>(File.ReadAllText("./config.json"));

            invalidKey = string.Empty;
            validKey = config.ApiKey;
        }

        /// <summary>
        /// Assert that ApiKeyIsValid method returns false when invalid key
        /// is provided.
        /// </summary>
        [Test]
        public async Task ApiKeyIsValidWithInvalidKey()
        {
            // Act
            bool result = await api.ApiKeyIsValid(invalidKey);

            // Assert
            ClassicAssert.IsFalse(result);
        }

        /// <summary>
        /// Assert that ApiKeyIsValid method returns true when valid key
        /// is provided.
        /// </summary>
        [Test]
        public async Task ApiKeyIsValidWIthValidKey()
        {
            // Act
            bool result = await api.ApiKeyIsValid(validKey);

            // Assert
            ClassicAssert.IsTrue(result);
        }
    }
}
