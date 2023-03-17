namespace SenseTowerEventAPI.Interfaces
{
    public interface IUser
    {
        public Guid Id { get; set; }
        string Username { get; set; }
        List<ITicket> Tickets { get; set; }
    }
}