namespace VisionHive.Application.DTO.Request;

public class PaginatedRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public String Subject { get; set; }
}