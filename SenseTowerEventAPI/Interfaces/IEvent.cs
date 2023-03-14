namespace SenseTowerEventAPI.Interfaces
{
    public interface IEvent
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public Guid ImageID { get; set; }
        public Guid SpaceID { get; set; }
    }
}