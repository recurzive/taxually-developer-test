using System.Text;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Clients.QueueClient;
using Taxually.TechnicalTest.Requests;

namespace Taxually.TechnicalTest.Serializers
{
    public class VatRegistrationRequestSerializer : IVatRegistrationRequestSerializer
    {
        public byte[] SerializeToCsv(VatRegistrationRequest request)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CompanyName,CompanyId");
            csvBuilder.AppendLine($"{request.CompanyName}{request.CompanyId}");
            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        public string SerializeToXml(VatRegistrationRequest request)
        {
            using (var stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                serializer.Serialize(stringwriter, request);
                return stringwriter.ToString();
            }
        }
    }
}
