using System.ComponentModel.DataAnnotations;

namespace DNDHinwil.Lib.Entities;

public class CampaignEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = Text.Campaign;
}
