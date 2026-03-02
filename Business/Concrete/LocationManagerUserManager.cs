using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DataAccess.Concrete.EntityFramework;

namespace Business.Concrete
{
    public class LocationManagerUserManager : ILocationManagerUserService
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly ILocationUserRoleService _locationUserRoleService;
        private readonly IOperationClaimService _operationClaimService;
        private readonly ILocationService _locationService;

        public LocationManagerUserManager(
            IAuthService authService,
            IUserService userService,
            IUserOperationClaimService userOperationClaimService,
            ILocationUserRoleService locationUserRoleService,
            IOperationClaimService operationClaimService,
            ILocationService locationService)
        {
            _authService = authService;
            _userService = userService;
            _userOperationClaimService = userOperationClaimService;
            _locationUserRoleService = locationUserRoleService;
            _operationClaimService = operationClaimService;
            _locationService = locationService;
        }

        public IResult AddLocationManager(LocationManagerAddDto dto)
        {
            // 1. Validate location exists
            var locationCheck = _locationService.GetById(dto.LocationId);
            if (!locationCheck.Success || locationCheck.Data == null)
            {
                return new ErrorResult("Specified location not found.");
            }

            // 2. Get the operation claim id for 'locationmanager'
            var operationClaimResult = _operationClaimService.GetByName("locationmanager");
            if (!operationClaimResult.Success || operationClaimResult.Data == null)
            {
                // Auto seed claim if missing
                _operationClaimService.Add(new OperationClaim { Name = "locationmanager" });
                operationClaimResult = _operationClaimService.GetByName("locationmanager");
            }
            int claimId = operationClaimResult.Data.Id;

            // 3. Coordinate creation process
            // We use AuthManager to ensure everything is encrypted properly.
            var userForRegisterDto = new IndividualRegisterDto
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password,
                IdentityNumber = "00000000000" // System admin identity fallback
            };

            using (var transactionScope = new TransactionScope())
            {
                // Using RegisterAdmin per plan to enforce non-customer type.
                var authResult = _authService.RegisterAdmin(userForRegisterDto);
                if (!authResult.Success)
                {
                    return new ErrorResult(authResult.Message);
                }

                var userId = authResult.Data.Id;

                // 4. Assign base Claim
                _userOperationClaimService.Add(new UserOperationClaim
                {
                    UserId = userId,
                    OperationClaimId = claimId
                });

                // 5. Assign Location Specific Claim
                _locationUserRoleService.Add(new LocationUserRole
                {
                    UserId = userId,
                    LocationId = dto.LocationId,
                    OperationClaimId = claimId
                });

                transactionScope.Complete();
                return new SuccessResult("Location Manager successfully created and assigned.");
            }
        }

        public IDataResult<List<LocationManagerDto>> GetLocationManagers()
        {
            var users = _userService.GetAll(); // Requires GetAll on IUserService (if not present, we can just fetch via claim)
            
            // To be more efficient and not require modifications to IUserService:
            var rolesResult = _locationUserRoleService.GetAll();
            if (!rolesResult.Success)
            {
                return new ErrorDataResult<List<LocationManagerDto>>("Failed to load roles");
            }

            var dtos = new List<LocationManagerDto>();
            foreach (var role in rolesResult.Data)
            {
                var userCheck = _userService.GetById(role.UserId);
                if(userCheck.Success && userCheck.Data != null)
                {
                    var user = userCheck.Data;
                    var locCheck = _locationService.GetById(role.LocationId);
                    
                    // Cross context fetch for admin names
                    using (var context = new RentACarContext()) {
                        var customer = context.IndividualCustomers.FirstOrDefault(c => c.Id == user.CustomerId);
                        dtos.Add(new LocationManagerDto
                        {
                            UserId = user.Id,
                            FirstName = customer != null ? customer.FirstName : "Admin",
                            LastName = customer != null ? customer.LastName : "User",
                            Email = user.Email,
                            PhoneNumber = customer != null ? customer.PhoneNumber : "",
                            LocationId = role.LocationId,
                            LocationName = (locCheck.Success && locCheck.Data != null) ? locCheck.Data.LocationName : "Unknown",
                            LocationUserRoleId = role.Id
                        });
                    }
                }
            }

            return new SuccessDataResult<List<LocationManagerDto>>(dtos, "Location managers listed");
        }

        public IResult RevokeLocationManager(int userId, int locationId)
        {
            var roleResult = _locationUserRoleService.GetByUserAndLocation(userId, locationId);
            if (!roleResult.Success || roleResult.Data == null)
            {
                return new ErrorResult("User is not managing this location.");
            }

            using (var transactionScope = new TransactionScope())
            {
                _locationUserRoleService.Delete(roleResult.Data);

                // Check if user has other locations to manage
                var remainingRoles = _locationUserRoleService.GetByUserId(userId);
                if (remainingRoles.Success && remainingRoles.Data.Count == 0)
                {
                    // If 0, revoke global claim
                    var globalClaims = _userOperationClaimService.GetByUserId(userId);
                    if (globalClaims.Success)
                    {
                        var claimResult = _operationClaimService.GetByName("locationmanager");
                        if (claimResult.Success && claimResult.Data != null)
                        {
                            var managerClaim = globalClaims.Data.FirstOrDefault(c => c.OperationClaimId == claimResult.Data.Id);
                            if (managerClaim != null)
                            {
                                _userOperationClaimService.Delete(managerClaim);
                            }
                        }
                    }
                }
                
                transactionScope.Complete();
            }
            return new SuccessResult("Location Manager privilege revoked.");
        }

        public IResult UpdateLocationManager(LocationManagerUpdateDto dto)
        {
             var userCheck = _userService.GetById(dto.UserId);
             if (!userCheck.Success || userCheck.Data == null)
             {
                 return new ErrorResult("User not found.");
             }

             if (dto.OldLocationId != dto.NewLocationId)
             {
                 using (var transactionScope = new TransactionScope())
                 {
                     var roleResult = _locationUserRoleService.GetByUserAndLocation(dto.UserId, dto.OldLocationId);
                     if (roleResult.Success && roleResult.Data != null)
                     {
                         var role = roleResult.Data;
                         role.LocationId = dto.NewLocationId;
                         _locationUserRoleService.Update(role);
                     }
                     else 
                     {
                          return new ErrorResult("User was not mapped to the specified old location.");
                     }

                     transactionScope.Complete();
                 }
             }

             return new SuccessResult("Location Manager updated.");
        }
    }
}
