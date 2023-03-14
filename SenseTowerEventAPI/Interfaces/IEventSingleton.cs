namespace SenseTowerEventAPI.Interfaces
{
    public interface IEventSingleton
    {
        public List<IEvent> Events { get; set; }
        /// <summary>
        /// Заглушка для проверки существования пространства
        /// </summary>
        public List<Guid> Spaces { get; set; }
        /// <summary>
        /// Заглушка для проверки существования изображения
        /// </summary>
        public List<Guid> Images { get; set; }
    }
}