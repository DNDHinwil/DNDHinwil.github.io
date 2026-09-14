using System.Net.Http.Json;

namespace Codespirals.Blazor.WebsiteTemplate;

public interface IContentService
{
    Task<IEnumerable<PostMetadata>> GetMetadata();
    Task<IEnumerable<PostMetadata>> GetMetadataByTag(string tag);
    Task<BlogPost?> GetPost(string title);
    Task<BlogPost?> GetLatest();
    Task<int> GetPostCount();
    Task<IEnumerable<LinkItem>> GetNavItems();
}

public class ContentService(HttpClient client) : IContentService
{
    private readonly HttpClient _client = client;

    public async Task<IEnumerable<PostMetadata>> GetMetadata()
    {
        var metadata = await _client.GetFromJsonAsync<IEnumerable<PostMetadata>>($"posts/metadata.json");
        if (metadata is null)
            return [];
        return [.. metadata.Where(p => p.IsActive && p.ShowInList).OrderByDescending(p => p.Edited ?? p.Created)];
    }

    public async Task<IEnumerable<PostMetadata>> GetMetadataByTag(string tagId)
    {
        var metadata = await _client.GetFromJsonAsync<IEnumerable<PostMetadata>>($"posts/metadata.json");
        if (metadata is null)
            return [];
        return [.. metadata.Where(p => p.IsActive && p.ShowInList && !p.Tags.Any(t => t.NormalizeForUrl() == tagId)).OrderByDescending(p => p.Edited ?? p.Created)];
    }

    public async Task<BlogPost?> GetPost(string title)
    {
        var metadata = await _client.GetFromJsonAsync<IEnumerable<PostMetadata>>($"posts/metadata.json");
        var post = metadata?.FirstOrDefault(m => m.Id == title);
        if (post is null)
            return null;
        var markdown = await _client.GetStringAsync(post.ContentUrl);
        if (string.IsNullOrWhiteSpace(markdown))
            return null;
        return new BlogPost { Metadata = post, Text = Markdig.Markdown.ToHtml(markdown) };
    }
    public async Task<BlogPost?> GetLatest()
    {
        var metadata = await _client.GetFromJsonAsync<IEnumerable<PostMetadata>>($"posts/metadata.json");
        var post = metadata?.Where(p => p.IsActive).OrderByDescending(p => p.Created).FirstOrDefault();
        if (post is null)
            return null;
        var markdown = await _client.GetStringAsync(post.ContentUrl);
        if (markdown is null)
            return null;
        return new BlogPost { Metadata = post, Text = Markdig.Markdown.ToHtml(markdown) };
    }
    public async Task<int> GetPostCount()
    {
        var metadata = await _client.GetFromJsonAsync<IEnumerable<PostMetadata>>($"posts/metadata.json");
        if (metadata is null)
            return 0;
        return metadata.Count();
    }
    public async Task<IEnumerable<LinkItem>> GetNavItems()
    {
        var navItems = await _client.GetFromJsonAsync<IEnumerable<LinkItem>>($"resources/navlinks.json");
        if (navItems is null)
            return [];
        return navItems;
    }
}
