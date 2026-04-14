namespace Common
{
    public class Expansion
    {
        public ExpansionSource[] Sources { get; private set; }
        public ExpansionSource? PreferredSource { get; private set; }
        public bool IsInstalled => Sources.Length > 0;
        public bool IsSelected { get; private set; }

        public string GameKey { get; }

        public Expansion(string gameKey)
        {
            GameKey = gameKey;

            // TODO: Use Update() instead.
            Sources = FindInstalledSources();
            PreferredSource = FindPreferredSource();
            IsSelected = FindIfSelected();
        }

        public void Update()
        {
            Sources = FindInstalledSources();
            PreferredSource = FindPreferredSource(); // TODO: Only accept if the source is installed.
            IsSelected = FindIfSelected();
        }

        private ExpansionSource? FindPreferredSource()
        {
            return MachineRegistry.GetValue(MachineRegistry.SimLKey, "Source", GameKey) switch
            {
                "Disc" => ExpansionSource.Disc,
                "Steam" => ExpansionSource.Steam,
                _ => null,
            };
        }

        private ExpansionSource[] FindInstalledSources()
        {
            var sources = new List<ExpansionSource>();

            if (MachineRegistry.KeyExists(MachineRegistry.SimsDiscKey, GameKey))
                sources.Add(ExpansionSource.Disc);

            if (MachineRegistry.KeyExists(MachineRegistry.SimsSteamKey, GameKey))
                sources.Add(ExpansionSource.Steam);

            return sources.ToArray();
        }

        private bool FindIfSelected()
        {
            return MachineRegistry.KeyExists(MachineRegistry.SimLKey, GameKey);
        }

        public void Select()
        {
            Update();

            if (!IsInstalled)
                return;

            if (IsSelected)
                return; // TODO: Maybe have a special case here.

            if (PreferredSource == null)
                PreferredSource = Sources[0];

            // Specify the source in SimL\Source.
            MachineRegistry.CreateKey(MachineRegistry.SimLKey, "Source");
            if (PreferredSource == ExpansionSource.Disc)
                MachineRegistry.CreateValue(MachineRegistry.SimLKey, "Source", GameKey, "Disc");
            if (PreferredSource == ExpansionSource.Steam)
                MachineRegistry.CreateValue(MachineRegistry.SimLKey, "Source", GameKey, "Steam");

            // Copy the game key to SimL.
            if (PreferredSource == ExpansionSource.Disc) // NOTE: throws registrykeynotfound if the specified source is not installed.
                MachineRegistry.CopyKey(MachineRegistry.SimsDiscKey, MachineRegistry.SimLKey, GameKey);
            if (PreferredSource == ExpansionSource.Steam)
                MachineRegistry.CopyKey(MachineRegistry.SimsSteamKey, MachineRegistry.SimLKey, GameKey);

            IsSelected = true;
        }

        public void Deselect()
        {
            // Delete the game key from SimL.
            MachineRegistry.DeleteKey(MachineRegistry.SimLKey, GameKey);

            IsSelected = false;
        }

        public void SetPreferredSource(ExpansionSource source)
        {
            Update();

            PreferredSource = source;

            // Specify the source in SimL\Source.
            MachineRegistry.CreateKey(MachineRegistry.SimLKey, "Source");
            if (PreferredSource == ExpansionSource.Disc)
                MachineRegistry.CreateValue(MachineRegistry.SimLKey, "Source", GameKey, "Disc");
            if (PreferredSource == ExpansionSource.Steam)
                MachineRegistry.CreateValue(MachineRegistry.SimLKey, "Source", GameKey, "Steam");

            // Finish if this expansion is not selected.
            if (!IsSelected)
                return;

            // Delete the game key from SimL.
            MachineRegistry.DeleteKey(MachineRegistry.SimLKey, GameKey);

            // Copy the game key to SimL.
            if (PreferredSource == ExpansionSource.Disc) // NOTE: throws registrykeynotfound if the specified source is not installed.
                MachineRegistry.CopyKey(MachineRegistry.SimsDiscKey, MachineRegistry.SimLKey, GameKey);
            if (PreferredSource == ExpansionSource.Steam)
                MachineRegistry.CopyKey(MachineRegistry.SimsSteamKey, MachineRegistry.SimLKey, GameKey);
        }
    }
}
