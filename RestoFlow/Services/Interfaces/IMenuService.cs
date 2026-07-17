using RestoFlow.Dtos.Responses;

namespace RestoFlow.Services.Interfaces
{
    public interface IMenuService
    {
        Task<MenuResultDto> GetMenuAsync();
    }
}
