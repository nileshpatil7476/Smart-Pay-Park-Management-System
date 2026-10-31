using SmartPayPark.Application.Authentication.Interfaces.Repositories;
using SmartPayPark.Domain.Entities.Enums;
using SmartPayPark.Application.Operators.DTOs;
using SmartPayPark.Application.Operators.Interfaces.Services;
using SmartPayPark.Domain.Interfaces.Repositories;
using SmartPayPark.Domain.Entities;

namespace SmartPayPark.Api.Services
{
    public class OperatorService : IOperatorService
    {
        private readonly IOperatorRepository _operatorRepository;
        public OperatorService(
            IOperatorRepository operatorRepository) {
            _operatorRepository = operatorRepository;
        }

        public async Task<OperatorProfileResponse> GetMyProfileAsync(Guid userId)
        {
            var operatorEntity = await _operatorRepository.GetByUserIdAsync(userId);

            if (operatorEntity == null)
            {
                return null;
            }

            return new OperatorProfileResponse
            {
                Id = operatorEntity.Id,
                BusinessAddress = operatorEntity.BusinessAddress,
                BusinessName = operatorEntity.BusinessName,
                RejectionReason = operatorEntity.RejectionReason,
                VerificationStatus = operatorEntity.VerificationStatus.ToString(),
                CreatedAtUtc = operatorEntity.CreatedAtUtc,
                ReviewedAtUtc = operatorEntity.ReviewedAtUtc,
            };
        }

        public async Task<OperatorResponse> RegisterAsync(CreateOperatorRequest request, Guid userId)
        {
            var operatorEntity = new Operator { 
                Id = Guid.NewGuid(),
                BusinessName = request.BusinessName,
                BusinessAddress = request.BusinessAddress,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone,
                ContactName = request.ContactName,
                
                VerificationStatus = OperatorStatus.Pending,
                CreatedAtUtc = DateTime.UtcNow,

                OperatorUsers = new List<OperatorUser>()
                {
                    new OperatorUser
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        OperatorRole = OperatorRole.Owner,
                        IsActive = true,
                        JoinedAtUtc = DateTime.UtcNow,
                        CreatedAtUtc = DateTime.UtcNow
                    }
                }
            };

            await _operatorRepository.AddAsync(operatorEntity);
            await _operatorRepository.SaveChangesAsync();

            return new OperatorResponse
            {
                Id = operatorEntity.Id,
                BusinessName = operatorEntity.BusinessName,
                VerificationStatus = operatorEntity.VerificationStatus.ToString(),
                CreatedAtUtc = operatorEntity.CreatedAtUtc
            };
        }
    }
}
