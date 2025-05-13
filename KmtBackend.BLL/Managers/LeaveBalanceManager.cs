using KmtBackend.BLL.Managers.Interfaces;
using KmtBackend.DAL.Constants;
using KmtBackend.DAL.Entities;
using KmtBackend.DAL.Repositories.Interfaces;
using KmtBackend.Models.DTOs.Leave;
using MapsterMapper;

namespace KmtBackend.BLL.Managers
{
    public class LeaveBalanceManager : ILeaveBalanceManager
    {
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LeaveBalanceManager(
            ILeaveBalanceRepository leaveBalanceRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _leaveBalanceRepository = leaveBalanceRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<LeaveBalanceResponse?> GetLeaveBalanceByIdAsync(Guid id)
        {
            var leaveBalance = await _leaveBalanceRepository.GetByIdAsync(id);
            if (leaveBalance == null) return null;
            
            return _mapper.Map<LeaveBalanceResponse>(leaveBalance);
        }

        public async Task<IEnumerable<LeaveBalanceResponse>> GetUserLeaveBalancesPaginatedAsync(Guid userId, int? year)
        {
            var balances = await _leaveBalanceRepository.GetByUserIdAsync(userId, year);
                
            var responses = balances.Select(_mapper.Map<LeaveBalanceResponse>);

            return responses;
        }

        public async Task<LeaveBalanceResponse> UpdateLeaveBalanceAsync(Guid id, UpdateLeaveBalanceRequest request)
        {
            var leaveBalance = await _leaveBalanceRepository.GetByIdAsync(id);
            if (leaveBalance == null)
            {
                throw new KeyNotFoundException("Leave balance not found");
            }

            leaveBalance.TotalDays = request.TotalDays ?? leaveBalance.TotalDays;
            leaveBalance.UsedDays = request.UsedDays ?? leaveBalance.UsedDays;

            var updatedBalance = await _leaveBalanceRepository.UpdateAsync(leaveBalance);
            
            return _mapper.Map<LeaveBalanceResponse>(updatedBalance);
        }

        public async Task<bool> CreateInitialBalancesForUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            
            var currentYear = DateTime.Now.Year;
            
            // Create initial leave balances based on user's experience
            var leaveTypes = await _leaveTypeRepository.GetAllAsync();
            
            foreach (var leaveType in leaveTypes)
            {
                int entitledDays = CalculateEntitledDays(leaveType, user);
                
                var balance = new LeaveBalance
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    LeaveTypeId = leaveType.Id,
                    Year = currentYear,
                    TotalDays = entitledDays,
                    UsedDays = 0,
                    CreatedAt = DateTime.UtcNow
                };
                
                await _leaveBalanceRepository.CreateAsync(balance);
            }
            
            return true;
        }

        public async Task<int> ResetAllUserBalancesAsync(int year)
        {
            // Get all active users
            var users = await _userRepository.GetAllAsync();
            var activeUsers = users.Where(u => u.TerminationDate == null || u.TerminationDate > DateTime.UtcNow);
            
            int count = 0;
            
            foreach (var user in activeUsers)
            {
                // Get leave types
                var leaveTypes = await _leaveTypeRepository.GetAllAsync();
                
                foreach (var leaveType in leaveTypes)
                {
                    // Check if balance already exists
                    var existingBalance = await _leaveBalanceRepository.GetUserBalanceAsync(user.Id, leaveType.Id, year);
                    
                    int entitledDays = CalculateEntitledDays(leaveType, user);
                    
                    if (existingBalance == null)
                    {
                        // Create new balance
                        var balance = new LeaveBalance
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            LeaveTypeId = leaveType.Id,
                            Year = year,
                            TotalDays = entitledDays,
                            UsedDays = 0,
                            CreatedAt = DateTime.UtcNow
                        };
                        
                        await _leaveBalanceRepository.CreateAsync(balance);
                    }
                    else
                    {
                        // Handle carry-over if applicable
                        int carryOverDays = 0;
                        
                        //if (leaveType.AllowCarryOver && year > DateTime.Now.Year - 1)
                        //{
                        //    // Check previous year balance
                        //    var previousYearBalance = await _leaveBalanceRepository.GetUserBalanceAsync(
                        //        user.Id, leaveType.Id, year - 1);
                                
                        //    if (previousYearBalance != null && previousYearBalance.RemainingDays > 0)
                        //    {
                        //        carryOverDays = Math.Min(previousYearBalance.RemainingDays, 10); // Max 10 days carried over
                        //    }
                        //}
                        
                        // Update existing balance
                        existingBalance.TotalDays = entitledDays + carryOverDays;
                        existingBalance.UsedDays = 0;
                        existingBalance.UpdatedAt = DateTime.UtcNow;
                        
                        await _leaveBalanceRepository.UpdateAsync(existingBalance);
                    }
                    
                    count++;
                }
            }
            
            return count;
        }
        
        private int CalculateEntitledDays(LeaveType leaveType, User user)
        {
            if (!leaveType.IsSeniorityBased)
            {
                // For non-seniority based leave types, return default entitlement
                return GetDefaultEntitlement(leaveType.Name);
            }
            
            // Calculate total work experience (including prior experience)
            int totalYearsExperience = user.TotalWorkExperienceYears;
            
            // Determine entitlement based on seniority
            if (leaveType.Name == LeaveConstants.RegularAnnualLeave)
            {
                return totalYearsExperience >= LeaveConstants.SeniorityYearsThreshold
                    ? LeaveConstants.SeniorRegularLeaveDays
                    : LeaveConstants.JuniorRegularLeaveDays;
            }
            else if (leaveType.Name == LeaveConstants.CasualLeave)
            {
                return totalYearsExperience >= LeaveConstants.SeniorityYearsThreshold
                    ? LeaveConstants.SeniorCasualLeaveDays
                    : LeaveConstants.JuniorCasualLeaveDays;
            }
            
            return GetDefaultEntitlement(leaveType.Name);
        }
        
        private int GetDefaultEntitlement(string leaveTypeName)
        {
            return leaveTypeName switch
            {
                LeaveConstants.RegularAnnualLeave => LeaveConstants.JuniorRegularLeaveDays,
                LeaveConstants.CasualLeave => LeaveConstants.JuniorCasualLeaveDays,
                LeaveConstants.SickLeave => LeaveConstants.SickLeaveDays,
                LeaveConstants.MaternityLeave => LeaveConstants.MaternityLeaveDays,
                LeaveConstants.MarriageLeave => LeaveConstants.MarriageLeaveDays,
                LeaveConstants.BereavementLeave => LeaveConstants.BereavementLeaveDays,
                LeaveConstants.PilgrimageLeave => LeaveConstants.PilgrimageLeaveDays,
                _ => 0
            };
        }
    }
}