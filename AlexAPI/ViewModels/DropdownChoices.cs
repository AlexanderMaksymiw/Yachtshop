using AlexAPI.Models;

namespace AlexAPI.ViewModels
{
    public class DropdownChoices
    {
        public IEnumerable<Location> Destinations { get; set; }
        public string HullType { get; set; }
    }
}
