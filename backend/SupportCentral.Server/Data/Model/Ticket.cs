namespace SupportCentral.Server.Data.Model;

public class Ticket {
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public Sector ToSector { get; set; }
    public User User { get; set; }
    public User? Attendant { get; set; }
    public ProblemType Type { get; set; }
    public TicketStatus Status { get; set; }
    public bool Paused { get; set; }
    public DateTime OpenedOn { get; set; }
    public DateTime? ClosedOn { get; set; }
}
