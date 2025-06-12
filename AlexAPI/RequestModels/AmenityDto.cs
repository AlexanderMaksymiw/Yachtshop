namespace AlexAPI.RequestModels
{
    public class AmenityDto

    {
        public Guid Id { get; set; }
        public List<string> ToyNames { get; set; } = new();
        public List<string> EquipmentNames { get; set; } = new();


    }
}