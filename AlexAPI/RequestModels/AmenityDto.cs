namespace AlexAPI.RequestModels
{
    public class AmenityDto
    {
        public Guid Id { get; set; }

        // Popular Equipment
        public bool AirConditioning { get; set; }
        public bool WiFi { get; set; }
        public bool Stabilizers { get; set; }
        public bool Sunpads { get; set; }
        public bool Jacuzzi { get; set; }
        public bool Gym { get; set; }

        // Popular Toys
        public bool SnorkellingEquipment { get; set; }
        public bool FishingEquipment { get; set; }
        public bool WaterSki { get; set; }
        public bool ScubaDivingEquipment { get; set; }
        public bool Seabob { get; set; }
        public bool WakeBoard { get; set; }

        // You can add more later

        public List<string> ToyNames { get; set; } = new();
        public List<string> EquipmentNames { get; set; } = new();

    }
}