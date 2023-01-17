using Taxually.TechnicalTest.Requests;

namespace Taxually.TechnicalTest.Services.Vat
{
    public interface ITaxuallyVatService
    {
        Task RegisterVatUsingHttpClient(VatRegistrationRequest request);
        Task RegisterVatUsingQueueClient(VatRegistrationRequest request);
        Task RegisterVatUsingXmlClient(VatRegistrationRequest request);
    }
}
