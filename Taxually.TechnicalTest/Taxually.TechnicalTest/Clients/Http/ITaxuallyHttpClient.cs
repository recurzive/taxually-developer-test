namespace Taxually.TechnicalTest.Clients.HttpClient
{
    public interface ITaxuallyHttpClient
    {
        Task PostAsync<TRequest>(string url, TRequest request);
    }
}
