using SmartPayPark.Application.Operators.DTOs;

namespace SmartPayPark.Application.Operators.Interfaces.Services
{
    public interface IOperatorService
    {
        Task<OperatorResponse> RegisterAsync(
            CreateOperatorRequest request,
            Guid userId);

        Task<OperatorProfileResponse> GetMyProfileAsync(Guid userId);
    }
}
