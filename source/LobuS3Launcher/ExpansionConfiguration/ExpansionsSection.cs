using System.Collections.Generic;

namespace LobuS3Launcher.ExpansionConfiguration;

public class ExpansionsSection
{
	public static readonly string SectionName = "Expansions";

	public List<ExpansionItem> ExpansionPacks { get; set; } = [];
	public List<ExpansionItem> StuffPacks { get; set; } = [];
}
