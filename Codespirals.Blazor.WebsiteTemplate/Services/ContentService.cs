using System.Net.Http.Json;

namespace DNDHinwil.Website;

public interface IContentService
{

}

public class ContentService(HttpClient client) : IContentService
{
    private readonly HttpClient _client = client;


}
