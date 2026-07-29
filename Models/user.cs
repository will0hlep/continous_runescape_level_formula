using System.ComponentModel.DataAnnotations;

namespace RSConLvl.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; } = "";
        public string URLModifier { get; set; } = "";
        public HiscoreResponse? Skills { get; set; }
    }

    public class HiscoreResponse
    {
        public required string Name { get; set; }
        public required List<Skill> Skills { get; set; }
        public required List<Activity> Activities { get; set; }
    }

    public class Skill
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int Rank { get; set; }
        public required int Level { get; set; }
        public required int Xp { get; set; }
        public double? VirtualLevel { get; set; }
    }

    public class Activity
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int Rank { get; set; }
        public required int Score { get; set; }
    }

}