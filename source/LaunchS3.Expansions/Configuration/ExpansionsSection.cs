namespace LaunchS3.Expansions.Configuration;

public class ExpansionsSection
{
	public static readonly string SectionName = "Expansions";

	public List<ExpansionItem> ExpansionPacks { get; set; } = [];
	public List<ExpansionItem> StuffPacks { get; set; } = [];
}
