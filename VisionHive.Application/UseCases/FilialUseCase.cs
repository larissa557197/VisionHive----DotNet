using VisionHive.Application.DTO.Request;
using VisionHive.Application.DTO.Response;
namespace VisionHive.Application.UseCases
{
    public class FilialUseCase : IFilialUseCase
    {

        public FilialResponse Post(FilialRequest filialRequest)
        {
            var FilialResponse = new FilialResponse()
            {
                Id = new Guid(),
            };
            
            return FilialResponse;
        }
    }
}
