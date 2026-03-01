using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var userForRegisterDto = new UserForRegisterDto
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
            };

            // Using standard user reg limits the scope. If we need a dedicated Admin register, use RegisterAdmin.
            // Using RegisterAdmin per plan to enforce non-customer type.
            var authResult = _authService.RegisterAdmin(userForRegisterDto, dto.Password);
            if (!authResult.Success)
            {
                return new ErrorResult(authResult.Message);
            }

            var userId = authResult.Data.Id;

            try
            {
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

                return new SuccessResult("Location Manager successfully created and assigned.");
            }
            catch (Exception ex)
            {
                // If claim assignments fail after user creation, we need to rollback the user.
                _userService.Delete(authResult.Data);
                return new ErrorResult("Failed to assign manager roles. Registration reverted. Error: " + ex.Message);
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
                    
                    dtos.Add(new LocationManagerDto
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        LocationId = role.LocationId,
                        LocationName = (locCheck.Success && locCheck.Data != null) ? locCheck.Data.LocationName : "Unknown",
                        LocationUserRoleId = role.Id
                    });
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
            return new SuccessResult("Location Manager privilege revoked.");
        }

        public IResult UpdateLocationManager(LocationManagerUpdateDto dto)
        {
             var userCheck = _userService.GetById(dto.UserId);
             if (!userCheck.Success || userCheck.Data == null)
             {
                 return new ErrorResult("User not found.");
             }

             // Update Profile
             var user = userCheck.Data;
             user.FirstName = dto.FirstName;
             user.LastName = dto.LastName;
             user.Email = dto.Email;
             user.PhoneNumber = dto.PhoneNumber;
             _userService.UpdateUserNames(user);

             if (dto.OldLocationId != dto.NewLocationId)
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
             }

             return new SuccessResult("Location Manager updated.");
        }
    }
}
