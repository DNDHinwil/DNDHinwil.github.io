namespace Codespirals.Blazor.WebsiteTemplate;

public class BlogPost
{
    public PostMetadata Metadata { get; set; } = new();
    public string Text { get; set; } = string.Empty;
}
