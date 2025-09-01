using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
namespace VisionHive.Application.UseCases
{
    public interface IFilialUseCase
    {
        FilialResponse Post(FilialRequest filialRequest);
    }
}
