using AutoFixture;
using Moq;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Clients.HttpClient;
using Taxually.TechnicalTest.Clients.QueueClient;
using Taxually.TechnicalTest.Requests;
using Taxually.TechnicalTest.Serializers;
using Taxually.TechnicalTest.Services.Vat;

namespace Taxually.TechnicalTest.Test
{
    [TestClass]
    public class TaxuallyVatServiceTest
    {
        private Fixture _fixtureBuilder;

        private Mock<ITaxuallyHttpClient> _httpClient;
        private Mock<ITaxuallyQueueClient> _queueClient;
        private Mock<IVatRegistrationRequestSerializer> _requestSerializer;

        [TestInitialize]
        public void InitializeTest()
        {
            _fixtureBuilder = new Fixture();

            _httpClient = new Mock<ITaxuallyHttpClient>();
            _queueClient= new Mock<ITaxuallyQueueClient>();
            _requestSerializer = new Mock<IVatRegistrationRequestSerializer>();            
        }

        #region RegisterVatUsingHttpClient
        [TestMethod]
        public async Task When_RegisterVatUsingHttpClientCalled_Then_TaxuallyHttpClientCalledWithProperRequest()
        {
            // Arrange
            var request = _fixtureBuilder.Create<VatRegistrationRequest>();
            var vatService = new TaxuallyVatService(_httpClient.Object, _queueClient.Object, _requestSerializer.Object);            

            // Act
            await vatService.RegisterVatUsingHttpClient(request);

            // Assert
            _httpClient.Verify(httpClient => httpClient.PostAsync(It.IsAny<string>(), request));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task When_RegisterVatUsingHttpClientCalledAndExceptionOccours_Then_ExceptionPropagateToTheCaller()
        {
            // Arrange
            var request = _fixtureBuilder.Create<VatRegistrationRequest>();
            _httpClient.Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), request)).Throws<InvalidOperationException>();
            var vatService = new TaxuallyVatService(_httpClient.Object, _queueClient.Object, _requestSerializer.Object);

            // Act
            await vatService.RegisterVatUsingHttpClient(request);
        }
        #endregion

        #region RegisterVAtUsingQueueClient
        [TestMethod]
        public async Task When_RegisterVatUsingQueueClientCalled_Then_TaxuallyQueueClientCalled()
        {
            // Arrange
            var request = _fixtureBuilder.Create<VatRegistrationRequest>();
            var serializedRequest = _fixtureBuilder.Create<byte[]>();
            _requestSerializer.Setup(requestSerializer => requestSerializer.SerializeToCsv(request)).Returns(serializedRequest);
            var vatService = new TaxuallyVatService(_httpClient.Object, _queueClient.Object, _requestSerializer.Object);

            // Act
            await vatService.RegisterVatUsingQueueClient(request);

            // Assert
            _queueClient.Verify(queueClient => queueClient.EnqueueAsync(It.IsAny<string>(), serializedRequest));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task When_RegisterVatUsingQueueClientCalledAndExceptionOccours_Then_ExceptionPropagateToTheCaller()
        {
            // Arrange
            var request = _fixtureBuilder.Create<VatRegistrationRequest>();
            var serializedRequest = _fixtureBuilder.Create<byte[]>();
            _requestSerializer.Setup(requestSerializer => requestSerializer.SerializeToCsv(request)).Returns(serializedRequest);
            _queueClient.Setup(queueClient => queueClient.EnqueueAsync(It.IsAny<string>(), serializedRequest)).Throws<InvalidOperationException>();
            var vatService = new TaxuallyVatService(_httpClient.Object, _queueClient.Object, _requestSerializer.Object);

            // Act
            await vatService.RegisterVatUsingQueueClient(request);
        }
        #endregion

        #region RegisterVAtUsingXmlClient
        [TestMethod]
        public async Task When_RegisterVatUsingXmlClientCalled_Then_TaxuallyHttpClientCalledWithProperRequest()
        {
            // Arrange
            var request = _fixtureBuilder.Create<VatRegistrationRequest>();
            var serializedRequest = _fixtureBuilder.Create<string>();
            _requestSerializer.Setup(requestSerializer => requestSerializer.SerializeToXml(request)).Returns(serializedRequest);
            var vatService = new TaxuallyVatService(_httpClient.Object, _queueClient.Object, _requestSerializer.Object);

            // Act
            await vatService.RegisterVatUsingXmlClient(request);


            // Assert
            _queueClient.Verify(httpClient => httpClient.EnqueueAsync(It.IsAny<string>(), serializedRequest));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task When_RegisterVatUsingXmlClientCalledAndExceptionOccours_Then_ExceptionPropagateToTheCaller()
        {
            // Arrange
            var request = _fixtureBuilder.Create<VatRegistrationRequest>();
            var serializedRequest = _fixtureBuilder.Create<string>();
            _requestSerializer.Setup(requestSerializer => requestSerializer.SerializeToXml(request)).Returns(serializedRequest);
            _queueClient.Setup(_queueClient => _queueClient.EnqueueAsync(It.IsAny<string>(), serializedRequest)).Throws<InvalidOperationException>();
            var vatService = new TaxuallyVatService(_httpClient.Object, _queueClient.Object, _requestSerializer.Object);

            // Act
            await vatService.RegisterVatUsingXmlClient(request);
        }
        #endregion
    }
}
