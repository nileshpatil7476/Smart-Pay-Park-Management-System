using SmartPayPark.Application.Admin.Operators.DTOs;
using SmartPayPark.Application.Operators.DTOs;

namespace SmartPayPark.Application.Admin.Operators.Interfaces;

public interface IAdminOperatorService
{
    Task<IEnumerable<OperatorResponse>> GetPendingOperatorsAsync();

    Task<IEnumerable<OperatorResponse>> GetAllOperatorsAsync();

    Task<AdminOperatorDetailsResponse?> GetOperatorByIdAsync(Guid id);

    Task<AdminOperatorDetailsResponse> ReviewOperatorAsync(
        Guid id,
        OperatorReviewRequest request,
        Guid reviewedByUserId);
}