using Taxually.TechnicalTest.Requests;

namespace Taxually.TechnicalTest.Serializers
{
    public interface IVatRegistrationRequestSerializer
    {
        byte[] SerializeToCsv(VatRegistrationRequest request);
        string SerializeToXml(VatRegistrationRequest request);
    }
}
