namespace Application.Interfaces
{
    public interface IPageRequest
    {
        int Items { get; set; }
        int Page { get; set; }
    }
}