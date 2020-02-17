using SmartHunter.Core;

namespace SmartHunter.Game.Config
{
    public class MemoryConfig
    {
        public string ProcessName = "MonsterHunterWorld";

        public int ThreadsPerScan = 1;

        public BytePatternConfig CurrentPlayerNamePattern = new BytePatternConfig(
            "CurrentPlayerNamePattern",
            "48 8B 0D ?? ?? ?? ?? 48 8D 55 ?? 45 31 C9 41 89 C0 E8"
            );

        public BytePatternConfig CurrentWeaponPattern = new BytePatternConfig(
            "CurrentWeaponPattern",
            "48 8B 0D ?? ?? ?? ?? 4C 8D 45 ?? 48 8D 97 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 "
            );

        public BytePatternConfig PlayerDamagePattern = new BytePatternConfig(
            "PlayerDamagePattern",
            "48 8B 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 75 04 33 C9"
            );

        public BytePatternConfig PlayerNamePattern = new BytePatternConfig(
            "PlayerNamePattern",
            "48 8B 0D ?? ?? ?? ?? 48 8D 54 24 38 C6 44 24 20 00 E8 ?? ?? ?? ?? 48 8B 5C 24 70 48 8B 7C 24 60 48 83 C4 68 C3"
            );

        public BytePatternConfig MonsterPattern = new BytePatternConfig(
            "MonsterPattern",
            "48 8B 0D ?? ?? ?? ?? B2 01 E8 ?? ?? ?? ?? C6 83 ?? ?? ?? ?? ?? 48 8B 0D"
            );

        public BytePatternConfig PlayerBuffPattern = new BytePatternConfig(
            "PlayerBuffPattern",
            "48 8B 05 ?? ?? ?? ?? 41 8B 94 00 ?? ?? ?? ?? 89 57 ??"
            );

        public BytePatternConfig SelectedMonsterPattern = new BytePatternConfig(
            "SelectedMonsterPattern",
            "48 8B 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 83 A0 ?? ?? ?? ?? ?? C6 43 ?? ??"
            );

        public BytePatternConfig LobbyStatusPattern = new BytePatternConfig(
            "LobbyStatusPattern",
            "48 8B 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 84 C0 75 14 33 D2 48 8B CD E8 ?? ?? ?? ?? 84 C0"
            );
    }
}
