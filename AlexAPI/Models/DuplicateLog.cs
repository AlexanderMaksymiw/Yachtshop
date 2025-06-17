namespace AlexAPI.Models
{
    public class DuplicateYachtLog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConflictFields { get; set; }
        public DateTime DateFlagged { get; set; }
    }
}
