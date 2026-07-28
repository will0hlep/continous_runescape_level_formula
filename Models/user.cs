using System.ComponentModel.DataAnnotations;

namespace RSConLvl.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; } = "";
        public string URLModifier { get; set; } = "";
        public double? AgilityLevel { get; set; }
        public int? AgilityXP { get; set; }
        public double? ArchaeologyLevel { get; set; }
        public int? ArchaeologyXP { get; set; }
        public double? AttackLevel { get; set; }
        public int? AttackXP { get; set; }
        public double? ConstitutionLevel { get; set; }
        public int? ConstitutionXP { get; set; }
        public double? ConstructionLevel { get; set; }
        public int? ConstructionXP { get; set; }
        public double? CookingLevel { get; set; }
        public int? CookingXP { get; set; }
        public double? CraftingLevel { get; set; }
        public int? CraftingXP { get; set; }
        public double? DefenceLevel { get; set; }
        public int? DefenceXP { get; set; }
        public double? DivinationLevel { get; set; }
        public int? DivinationXP { get; set; }
        public double? DungeoneeringLevel { get; set; }
        public int? DungeoneeringXP { get; set; }
        public double? FarmingLevel { get; set; }
        public int? FarmingXP { get; set; }
        public double? FiremakingLevel { get; set; }
        public int? FiremakingXP { get; set; }
        public double? FishingLevel { get; set; }
        public int? FishingXP { get; set; }
        public double? FletchingLevel { get; set; }
        public int? FletchingXP { get; set; }
        public double? HerbloreLevel { get; set; }
        public int? HerbloreXP { get; set; }
        public double? HitpointsLevel { get; set; }
        public int? HitpointsXP { get; set; }
        public double? HunterLevel { get; set; }
        public int? HunterXP { get; set; }
        public double? InventionLevel { get; set; }
        public int? InventionXP { get; set; }
        public double? MagicLevel { get; set; }
        public int? MagicXP { get; set; }
        public double? MiningLevel { get; set; }
        public int? MiningXP { get; set; }
        public double? NecromancyLevel { get; set; }
        public int? NecromancyXP { get; set; }
        public double? PrayerLevel { get; set; }
        public int? PrayerXP { get; set; }
        public double? RangedLevel { get; set; }
        public int? RangedXP { get; set; }
        public double? RunecraftingLevel { get; set; }
        public int? RunecraftingXP { get; set; }
        public double? RunecraftLevel { get; set; }
        public int? RunecraftXP { get; set; }
        public double? SailingLevel { get; set; }
        public int? SailingXP { get; set; }
        public double? SlayerLevel { get; set; }
        public int? SlayerXP { get; set; }
        public double? SmithingLevel { get; set; }
        public int? SmithingXP { get; set; }
        public double? StrengthLevel { get; set; }
        public int? StrengthXP { get; set; }
        public double? SummoningLevel { get; set; }
        public int? SummoningXP { get; set; }
        public double? ThievingLevel { get; set; }
        public int? ThievingXP { get; set; }
        public double? WoodcuttingLevel { get; set; }
        public int? WoodcuttingXP { get; set; }
        public string? Notes { get; set; }
    }
}