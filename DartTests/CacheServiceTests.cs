//using Dart;
//using Microsoft.Extensions.Logging;
//using FakeItEasy;
//using Xunit;

//namespace DartTests
//{
//    public class CacheServiceTests
//    {
//        private readonly ILogger<CacheService> _fakeLogger;
//        private readonly ICacheService _cacheService;

//        public CacheServiceTests()
//        {
//            _fakeLogger = A.Fake<ILogger<CacheService>>();
//            _cacheService = new CacheService(_fakeLogger);
//        }

//        [Fact]
//        public void Set_And_Get_String_ShouldReturnSameValue()
//        {
//            // Arrange
//            const string key = "test_key";
//            const string value = "test_value";

//            // Act
//            _cacheService.Set(key, value);
//            var result = _cacheService.Get(key);

//            // Assert
//            Assert.Equal(value, result);
//        }

//        [Fact]
//        public void Get_NonExistentKey_ShouldReturnNull()
//        {
//            // Arrange
//            const string key = "non_existent_key";

//            // Act
//            var result = _cacheService.Get(key);

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public void Set_And_Get_GenericList_ShouldReturnSameList()
//        {
//            // Arrange
//            const string key = "recent_stations";
//            var stations = new List<string> { "Connolly", "Tara Street", "Pearse" };

//            // Act
//            _cacheService.Set(key, stations);
//            var result = _cacheService.Get<List<string>>(key);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(3, result.Count);
//            Assert.Equal("Connolly", result[0]);
//            Assert.Equal("Tara Street", result[1]);
//            Assert.Equal("Pearse", result[2]);
//        }

//        [Fact]
//        public void Set_And_Get_GenericArray_ShouldReturnSameArray()
//        {
//            // Arrange
//            const string key = "stations_array";
//            var stations = new[] { "Connolly", "Tara Street", "Pearse" };

//            // Act
//            _cacheService.Set(key, stations);
//            var result = _cacheService.Get<string[]>(key);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(3, result.Length);
//            Assert.Equal("Connolly", result[0]);
//        }

//        [Fact]
//        public void Get_Generic_NonExistentKey_ShouldReturnDefault()
//        {
//            // Arrange
//            const string key = "non_existent_generic";

//            // Act
//            var result = _cacheService.Get<List<string>>(key);

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public void Clear_ExistingKey_ShouldRemoveValue()
//        {
//            // Arrange
//            const string key = "to_remove";
//            const string value = "temporary";
//            _cacheService.Set(key, value);

//            // Act
//            _cacheService.Clear(key);
//            var result = _cacheService.Get(key);

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public void Clear_MultipleKeys_ShouldRemoveOnlySpecifiedKey()
//        {
//            // Arrange
//            _cacheService.Set("key1", "value1");
//            _cacheService.Set("key2", "value2");

//            // Act
//            _cacheService.Clear("key1");
//            var result1 = _cacheService.Get("key1");
//            var result2 = _cacheService.Get("key2");

//            // Assert
//            Assert.Null(result1);
//            Assert.Equal("value2", result2);
//        }

//        [Fact]
//        public void ContainsKey_ExistingKey_ShouldReturnTrue()
//        {
//            // Arrange
//            const string key = "existing_key";
//            _cacheService.Set(key, "value");

//            // Act
//            var result = _cacheService.ContainsKey(key);

//            // Assert
//            Assert.True(result);
//        }

//        [Fact]
//        public void ContainsKey_NonExistentKey_ShouldReturnFalse()
//        {
//            // Arrange
//            const string key = "non_existent";

//            // Act
//            var result = _cacheService.ContainsKey(key);

//            // Assert
//            Assert.False(result);
//        }
//    }
//}
