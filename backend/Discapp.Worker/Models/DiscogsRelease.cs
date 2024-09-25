namespace Discapp.Worker.Models;

public class DiscogsRelease
{
    public string Thumb { get; set; } = "";
    public List<DiscogReleaseIdentifiers> Identifiers { get; set; } = [];
}
