using System.ComponentModel.DataAnnotations;

namespace DNDHinwil.Lib.Entities;

public class SettingsEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CampaignId { get; set; }
}
