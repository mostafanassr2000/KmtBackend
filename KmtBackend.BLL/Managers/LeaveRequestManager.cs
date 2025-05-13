using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;
using KmtBackend.Models.DTOs.Common;
using KmtBackend.Models.DTOs.Leave;
using MapsterMapper;
using Microsoft.AspNetCore.Http;

namespace KmtBackend.BLL.Managers
{
    public class LeaveRequestManager : ILeaveRequestManager
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LeaveRequestManager(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveBalanceRepository leaveBalanceRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LeaveRequestResponse> CreateLeaveRequestAsync(Guid userId, CreateLeaveRequestRequest request)
        {
            // Validate user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            
            // Calculate the number of days requested
            var days = (int)(request.EndDate.Date - request.StartDate.Date).TotalDays + 1;
            
            if (days <= 0)
            {
                throw new ArgumentException("End date must be after start date");
            }
            
            // Get the user's leave balance for this leave type
            var currentYear = DateTime.Now.Year;
            var leaveBalance = await _leaveBalanceRepository.GetUserBalanceAsync(userId, request.LeaveTypeId, currentYear);
            
            if (leaveBalance == null)
            {
                throw new InvalidOperationException("No leave balance found for this leave type");
            }
            
            // Check if user has enough balance
            if (leaveBalance.RemainingDays < days)
            {
                throw new InvalidOperationException($"Insufficient leave balance. Available: {leaveBalance.RemainingDays}, Requested: {days}");
            }
            
            // Create the leave request
            var leaveRequest = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                LeaveTypeId = request.LeaveTypeId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Days = days,
                Status = LeaveRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            
            var createdRequest = await _leaveRequestRepository.CreateAsync(leaveRequest);
            
            return _mapper.Map<LeaveRequestResponse>(createdRequest);
        }

        public async Task<LeaveRequestResponse?> GetLeaveRequestByIdAsync(Guid id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null) return null;
            
            return _mapper.Map<LeaveRequestResponse>(leaveRequest);
        }

        public async Task<PaginatedResult<LeaveRequestResponse>> GetUserLeaveRequestsPaginatedAsync(Guid userId, PaginationQuery pagination)
        {
            var result = await _leaveRequestRepository.GetByUserIdPaginatedAsync(userId, pagination);
            
            var responses = _mapper.Map<IEnumerable<LeaveRequestResponse>>(result.Items).ToList();
            
            return new PaginatedResult<LeaveRequestResponse>
            {
                Items = responses,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<PaginatedResult<LeaveRequestResponse>> GetDepartmentLeaveRequestsPaginatedAsync(Guid departmentId, PaginationQuery pagination)
        {
            var result = await _leaveRequestRepository.GetByDepartmentIdPaginatedAsync(departmentId, pagination);
            
            var responses = _mapper.Map<IEnumerable<LeaveRequestResponse>>(result.Items).ToList();
            
            return new PaginatedResult<LeaveRequestResponse>
            {
                Items = responses,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<PaginatedResult<LeaveRequestResponse>> GetAllLeaveRequestsPaginatedAsync(PaginationQuery pagination)
        {
            var result = await _leaveRequestRepository.GetAllPaginatedAsync(pagination);
            
            var responses = _mapper.Map<IEnumerable<LeaveRequestResponse>>(result.Items).ToList();
            
            return new PaginatedResult<LeaveRequestResponse>
            {
                Items = responses,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<LeaveRequestResponse> ApproveLeaveRequestAsync(Guid id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
            {
                throw new KeyNotFoundException("Leave request not found");
            }
            
            if (leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be approved");
            }
            
            // Get the leave balance
            var currentYear = leaveRequest.StartDate.Year; // Use the year of the leave date
            var leaveBalance = await _leaveBalanceRepository.GetUserBalanceAsync(
                leaveRequest.UserId, leaveRequest.LeaveTypeId, currentYear);
                
            if (leaveBalance == null)
            {
                throw new InvalidOperationException("No leave balance found for this leave type");
            }
            
            // Check if user still has enough balance
            if (leaveBalance.RemainingDays < leaveRequest.Days)
            {
                throw new InvalidOperationException($"Insufficient leave balance. Available: {leaveBalance.RemainingDays}, Requested: {leaveRequest.Days}");
            }
            
            // Update the leave request
            var processedById = _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value!;
            leaveRequest.Status = LeaveRequestStatus.Approved;
            leaveRequest.ProcessedById = Guid.Parse(processedById);
            leaveRequest.ProcessedDate = DateTime.UtcNow;
            
            var updatedRequest = await _leaveRequestRepository.UpdateAsync(leaveRequest);
            
            // Deduct from leave balance
            leaveBalance.UsedDays += leaveRequest.Days;
            await _leaveBalanceRepository.UpdateAsync(leaveBalance);
            
            return _mapper.Map<LeaveRequestResponse>(updatedRequest);
        }

        public async Task<LeaveRequestResponse> RejectLeaveRequestAsync(Guid id, RejectLeaveRequestRequest request)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
            {
                throw new KeyNotFoundException("Leave request not found");
            }
            
            if (leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be rejected");
            }

            // Update the leave request
            var processedById = _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value!;
            leaveRequest.Status = LeaveRequestStatus.Rejected;
            leaveRequest.ProcessedById = Guid.Parse(processedById);
            leaveRequest.ProcessedDate = DateTime.UtcNow;
            leaveRequest.RejectionReason = request.RejectionReason;
            
            var updatedRequest = await _leaveRequestRepository.UpdateAsync(leaveRequest);
            
            return _mapper.Map<LeaveRequestResponse>(updatedRequest);
        }

        public async Task<bool> CancelLeaveRequestAsync(Guid id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
            {
                throw new KeyNotFoundException("Leave request not found");
            }
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value!;
            // Only the request owner can cancel it
            if (leaveRequest.UserId.ToString() != userId)
            {
                throw new UnauthorizedAccessException("You can only cancel your own leave requests");
            }
            
            // Only pending requests can be canceled by the user
            if (leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be canceled");
            }
            
            leaveRequest.Status = LeaveRequestStatus.Canceled;
            leaveRequest.UpdatedAt = DateTime.UtcNow;
            
            await _leaveRequestRepository.UpdateAsync(leaveRequest);
            
            return true;
        }
    }
}