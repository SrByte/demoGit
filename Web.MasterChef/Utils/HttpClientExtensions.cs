using System.Net.Http.Headers;
using System.Text.Json;

namespace MasterChef.Web.Utils
{
    public static class HttpClientExtensions
    {
        private static MediaTypeHeaderValue
            contentType = new MediaTypeHeaderValue("application/json");

        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(
                    $"Something got wrong calling API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#pragma warning disable CS8603 // Possible null reference return.
            return JsonSerializer.Deserialize<T>(
                dataAsString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static Task<HttpResponseMessage> PostAsJson<T>(
            this HttpClient httpClient,
            string url,
            T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return httpClient.PostAsync(url, content);

        }
        public static Task<HttpResponseMessage> PutAsJson<T>(
            this HttpClient httpClient,
            string url,
            T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return httpClient.PutAsync(url, content);

        }
    }
}
