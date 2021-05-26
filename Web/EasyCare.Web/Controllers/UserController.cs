using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Core.Services.Notification;
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Device = EasyCare.Core.Domain.Entities.Device;
using Participant = EasyCare.Core.Domain.Entities.Participant;

namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CrudController<Supervisor, UserDto>
    {
        private IParticipantRepository _participantRepository;
        private IGroupRepository _groupRepository;
        private IDeviceRepository _deviceRepository;
        private readonly ITANRepository _TANRepository;
        private IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public UserController(ISupervisorRepository repository,
            IGroupRepository groupRepository,
            IParticipantRepository participantRepository,
            IDeviceRepository deviceRepository,
            ILogger<SupervisorController> logger,
            IMapper mapper,
            ITANRepository TANRepository,
            INotificationService notificationService,
            IConfiguration configuration)
            : base(repository, logger, mapper)
        {
            _groupRepository = groupRepository;
            _participantRepository = participantRepository;
            _deviceRepository = deviceRepository;
            _TANRepository = TANRepository;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        [HttpPost]
        public override async Task<ActionResult<UserDto>> PostItem(UserDto item)
        {
            try
            {
                Supervisor objSupervisor = null;
                Core.Domain.Entities.Device objDevice = null;
                TAN objTan = null;

                var SupervisorExist = await (_repository as ISupervisorRepository).GetSupervisorByEmail(item.EMail);

                if (SupervisorExist == null)
                {
                    User user = await CreateAzureADUser(item);

                    if (user != null)
                    {
                        var entity = _mapper.Map<Supervisor>(item);
                        entity.Id = Guid.Parse(user.Id);
                        entity.IsActive = true;

                        if (string.IsNullOrEmpty(item.Code))
                        {
                            entity.IsModerator = true;
                            objSupervisor = await _repository.Add(entity);

                            if (objSupervisor != null)
                            {
                                objDevice = await _deviceRepository.AddDevice(objSupervisor.Id);

                                if (objDevice != null)
                                {
                                    //var success = await _notificationService.CreateOrUpdateRegistrationDevice(item.DeviceCode);
                                    //if (success)
                                    //  await _TANRepository.AddTAN(objDevice.Id, item.DeviceCode);
                                    //objTan = await _TANRepository.AddTAN(objDevice.Id, item.DeviceCode);
                                    var tagsCode = await _notificationService.CreateOrUpdateRegistrationDevice(item.DeviceCode, objSupervisor.Id.ToString());
                                    if (!string.IsNullOrEmpty(tagsCode))
                                    {
                                        await _TANRepository.AddTAN(objDevice.Id, item.DeviceCode,tagsCode);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var group = await _groupRepository.GetGroupByCode(item.Code);
                            if (group != null)
                            {
                                //objSupervisor.IsSenior = true;
                                objSupervisor = await _repository.Add(entity);
                                if (objSupervisor != null)
                                {
                                    objDevice = await _deviceRepository.AddDevice(objSupervisor.Id);

                                    await _participantRepository.AddParticipant(objSupervisor.Id, group.Id);

                                    if (objDevice != null)
                                    {
                                        var tagsCode = await _notificationService.CreateOrUpdateRegistrationDevice(item.DeviceCode, objSupervisor.Id.ToString());
                                        if (!string.IsNullOrEmpty(tagsCode))
                                        {
                                            await _TANRepository.AddTAN(objDevice.Id, item.DeviceCode, tagsCode);
                                        }
                                        //var success = await _notificationService.CreateOrUpdateRegistrationDevice(item.DeviceCode);
                                        //if (success)
                                        //await _TANRepository.AddTAN(objDevice.Id, item.DeviceCode);
                                        //objTan = await _TANRepository.AddTAN(objDevice.Id, item.DeviceCode);
                                    }

                                }
                            }
                        }
                    }
                    return Ok(objSupervisor);
                }
                else
                {
                    return Conflict();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddSenior")]
        public async Task<ActionResult<UserDto>> AddSenior(UserDto item)
        {
            try
            {
                Supervisor objSupervisor = null;
                Core.Domain.Entities.Device objDevice = null;
                TAN objTan = null;

                var SupervisorExist = await (_repository as ISupervisorRepository).GetSupervisorByEmail(item.EMail);

                if (SupervisorExist == null)
                {
                    var objGroup = await _groupRepository.GetGroupBySupervisorId(item.Id);

                    if (objGroup != null)
                    {
                        User user = await CreateAzureADUser(item);

                        if (user != null)
                        {
                            var entity = _mapper.Map<Supervisor>(item);
                            entity.Id = Guid.Parse(user.Id);
                            entity.IsActive = true;
                            entity.IsSenior = true;
                            objSupervisor = await _repository.Add(entity);

                            if (objSupervisor != null)
                            {
                                objDevice = await _deviceRepository.AddDevice(objSupervisor.Id);

                                await _participantRepository.AddParticipant(objSupervisor.Id, objGroup.Id);

                                if (objDevice != null)
                                {
                                    var tagsCode = await _notificationService.CreateOrUpdateRegistrationDevice(item.DeviceCode, objSupervisor.Id.ToString());
                                    if (!string.IsNullOrEmpty(tagsCode))
                                    {
                                        await _TANRepository.AddTAN(objDevice.Id, item.DeviceCode, tagsCode);
                                    }
                                }
                            }
                        }
                        return Ok(objSupervisor);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return Conflict();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public override async Task<ActionResult<UserDto>> DeleteItem(Guid id)
        {
            try
            {
                var objSupervisor = _repository.Get(id);

                if (objSupervisor != null)
                {
                    objSupervisor.Result.IsActive = false;
                    var result = await _repository.Update(id, objSupervisor.Result);
                    if (result)
                    {
                        var objParticipant = await _participantRepository.GetBySupervisorId(id);
                        if (objParticipant != null)
                        {
                            await _participantRepository.Delete(objParticipant.Id);
                            return Ok();
                        }
                    }
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, id);
                return BadRequest();
            }
        }

        //[Authorize]
        [HttpPut]
        [Route("SetModerator")]
        public async Task<ActionResult<SupervisorDto>> SetModerator(SetUserDto item)
        {
            try
            {
                var participantExist = _participantRepository.GetBySupervisorId(item.ParticipantId);
                if (participantExist.Result != null)
                {
                    var objGroupParticipantList = await (_repository as ISupervisorRepository).GetSupervisorByGroupId(participantExist.Result.GroupId, true);

                    if (objGroupParticipantList.Count() > 0)
                    {
                        foreach (var objGroupParticipant in objGroupParticipantList)
                        {
                            objGroupParticipant.IsModerator = false;
                            await _repository.Update(objGroupParticipant.Id, objGroupParticipant);
                        }
                    }

                    var objSupervisor = await _repository.Get(item.ParticipantId);
                    if (objSupervisor != null)
                    {
                        objSupervisor.IsModerator = true;
                        objSupervisor.IsSenior = false;
                        var result = await _repository.Update(objSupervisor.Id, objSupervisor);
                        if (result)
                        {
                            var dto = _mapper.Map<SupervisorDto>(objSupervisor);
                            return Ok(dto);
                        }
                    }
                    return BadRequest();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        //[Authorize]
        [HttpPut]
        [Route("SetSenior")]
        public async Task<ActionResult<SupervisorDto>> SetSenior(SetUserDto item)
        {
            try
            {
                var participantExist = _participantRepository.GetBySupervisorId(item.ParticipantId);
                if (participantExist.Result != null)
                {
                    var objGroupParticipantList = await (_repository as ISupervisorRepository).GetSupervisorByGroupId(participantExist.Result.GroupId, false);

                    if (objGroupParticipantList.Count() > 0)
                    {
                        foreach (var objGroupParticipant in objGroupParticipantList)
                        {
                            objGroupParticipant.IsSenior = false;
                            await _repository.Update(objGroupParticipant.Id, objGroupParticipant);
                        }
                    }

                    var objSupervisor = await _repository.Get(item.ParticipantId);
                    if (objSupervisor != null)
                    {
                        objSupervisor.IsSenior = true;
                        objSupervisor.IsModerator = false;
                        var result = await _repository.Update(objSupervisor.Id, objSupervisor);
                        if (result)
                        {
                            var dto = _mapper.Map<SupervisorDto>(objSupervisor);
                            return Ok(dto);
                        }
                    }
                    return BadRequest();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        private async Task<User> CreateAzureADUser(UserDto item)
        {
            try
            {
                var graphClient = ConfigureGraphClient();
                string tenantId = _configuration.GetValue<string>("AzureAdB2C:Tenant");
                string b2cExtensionAppClientId = _configuration.GetValue<string>("AzureAdB2C:b2cExtensionAppClientId");

                var result = await graphClient.Users
                .Request()
                .AddAsync(new User
                {
                    GivenName = item.FirstName,
                    Surname = item.SecondName,
                    DisplayName = item.FirstName + " " + item.SecondName,
                    Identities = new List<ObjectIdentity>
                    {
                        new ObjectIdentity()
                        {
                            SignInType = "emailAddress",
                            Issuer = tenantId,
                            IssuerAssignedId = item.EMail
                        }
                    },
                    PasswordProfile = new PasswordProfile()
                    {
                        ForceChangePasswordNextSignIn = false,
                        Password = item.PwHash//Helpers.PasswordHelper.GenerateNewPassword(4, 8, 4)
                    },
                    PasswordPolicies = "DisablePasswordExpiration,DisableStrongPassword",
                    //PasswordPolicies = "DisablePasswordExpiration",
                    // AdditionalData = extensionInstance
                });
                string userId = result.Id;
                Console.WriteLine($"Created the new user. Now get the created user with object ID '{userId}'...");
                // Get created user by object ID
                result = await graphClient.Users[userId]
                    .Request()
                    .Select($"id,givenName,surName,displayName,identities")
                    .GetAsync();
                if (result != null)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
                    return result;
                }
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            return null;
        }

        private GraphServiceClient ConfigureGraphClient()
        {
            string clientId = _configuration.GetValue<string>("AzureAdB2C:ClientId");
            string tenant = _configuration.GetValue<string>("AzureAdB2C:Tenant");
            string clientSecret = _configuration.GetValue<string>("AzureAdB2C:ClientSecret");

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                   .Create(clientId)
                   .WithTenantId(tenant)
                   .WithClientSecret(clientSecret)
                   .Build();
            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
            // Set up the Microsoft Graph service client with client credentials
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);
            return graphClient;
        }

        private async Task<bool> RemoveAzureADUser(Guid userID)
        {
            try
            {
                var graphClient = ConfigureGraphClient();
                string tenantId = _configuration.GetValue<string>("AzureAdB2C:Tenant");
                string b2cExtensionAppClientId = _configuration.GetValue<string>("AzureAdB2C:b2cExtensionAppClientId");
                string userId = userID.ToString();

                await graphClient.Users[userId]
                       .Request()
                       .DeleteAsync();

                return true;
                //if (result != null)
                //{
                //    Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
                //    return result;
                //}
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            return false;
        }
    }
}
