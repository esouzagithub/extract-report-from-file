namespace Core.Reports.Interfaces
{
    public interface IReport
    {
        string Id { get; }
        bool IsValid();
    }
}