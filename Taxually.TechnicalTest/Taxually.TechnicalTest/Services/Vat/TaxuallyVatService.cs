using System.Text;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Clients.HttpClient;
using Taxually.TechnicalTest.Clients.QueueClient;
using Taxually.TechnicalTest.Requests;
using Taxually.TechnicalTest.Serializers;

namespace Taxually.TechnicalTest.Services.Vat
{
    public class TaxuallyVatService : ITaxuallyVatService
    {
        public const string Url = "https://api.uktax.gov.uk";

        private readonly ITaxuallyHttpClient _httpClient;
        private readonly ITaxuallyQueueClient _queueClient;
        private readonly IVatRegistrationRequestSerializer _requestSerializer;

        public TaxuallyVatService(ITaxuallyHttpClient httpClient, ITaxuallyQueueClient queueClient, 
            IVatRegistrationRequestSerializer requestSerializer)
        {
            _httpClient = httpClient;
            _queueClient = queueClient;
            _requestSerializer = requestSerializer;
        }

        public Task RegisterVatUsingHttpClient(VatRegistrationRequest request)
        {
            return _httpClient.PostAsync(Url, request);
        }

        public Task RegisterVatUsingQueueClient(VatRegistrationRequest request)
        {
            var csv = _requestSerializer.SerializeToCsv(request);
            // Queue file to be processed
            return _queueClient.EnqueueAsync("vat-registration-csv", csv);
        }

        public Task RegisterVatUsingXmlClient(VatRegistrationRequest request)
        {
            var xml = _requestSerializer.SerializeToXml(request);
            // Queue xml doc to be processed
            return _queueClient.EnqueueAsync("vat-registration-xml", xml);
        }
    }
}
