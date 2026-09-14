namespace Codespirals.Blazor.WebsiteTemplate;

public class PostMetadata
{
    public string Id => Title.NormalizeForUrl();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Tags { get; set; } = [];
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? Edited { get; set; }
    public string? ContentUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool ShowInList { get; set; } = true;
    public int? EstimatedReadTimeInMinutes { get; set; }
}
