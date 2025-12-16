using AutoMapper;
using Business.Helpers;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Mail;
using Business.ViewModel.Account;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Business.Enums;
using Business.ViewModel.HomePage;


namespace MOINFO_API.Controllers
{
    [Route("Dynamic")]
    
    public class DynamicLookupApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public DynamicLookupApiController(IUnitOfwork unitOfwork, IMapper mapper,HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        #region WorkFlow For Status 
        [HttpGet("GetServices")]
        public async Task<IActionResult> GetServices()
        {
            var services = await _unitOfWork.genericRepository<Eservice>()
                .GetFilteredWithProjection(
                 selector: x => new { x.EserviceName, x.ServiceId })
                .ToListAsync();
            return Ok(services);
        }
        [HttpGet("GetActivityTypes/{serviceId}")]
        public async Task<IActionResult> GetActivityTypes(int serviceId)
        {
            var activities = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                .GetFilteredWithProjection(
                    filter: a => a.ServiceId == serviceId,
                    selector: a => new { a.Id, a.NameAr }
                )
                .ToListAsync();
            return Ok(activities);
        }
        #region RequestTypeCrud
        [HttpGet("GetRequestTypes")]
        public async Task<IActionResult> GetRequestTypes()
        {
            var requestTypes = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    // Assuming ServiceId exists in RequestTypesLookup
                    selector: r => new { r.Id, r.NameAr ,r.Status}
                )
                .ToListAsync();

            return Ok(requestTypes);
        }

     

        [HttpPost]
        [Route("CreateRequestsType")]
        public async Task<IActionResult> CreateRequestsType(RequestsTypesLookup model)
        {
            if (ModelState.IsValid)
            {
                //var newModel = new RequestsTypesLookup
                //{
                //    Status=true,
                //     NameAr=model.NameAr,   
                //};

                await _unitOfWork.genericRepository<RequestsTypesLookup>().Create(model);
                await _unitOfWork.Complete();
              
            }
            return Ok(model);
        }
        [HttpGet("EditRequestsType")]

        public async Task<IActionResult> EditRequestsType(int id)
        {
            var item = await _unitOfWork.genericRepository<RequestsTypesLookup>().GetbyId(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Route("EditRequestsType")]

        public async Task<IActionResult> EditRequestsType(RequestsTypesLookup model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.genericRepository<RequestsTypesLookup>().Update(model);
                await _unitOfWork.Complete();
               
            }
            return Ok(model);
        }

        [HttpGet("DeleteRequestsType")]

        public async Task<IActionResult> DeleteRequestsType(int id)
        {
            var item = await _unitOfWork.genericRepository<RequestsTypesLookup>().GetbyId(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Route("DeleteConfirmedRequestsType")]
        public async Task<IActionResult> DeleteConfirmedRequestsType(int id)
        {
            var item = await _unitOfWork.genericRepository<RequestsTypesLookup>().GetbyId(id);
            if (item == null) return NotFound();

            await _unitOfWork.genericRepository<RequestsTypesLookup>().Delete(item);
            await _unitOfWork.Complete();
            return Ok();
        }
        #endregion
        [HttpGet]
        [Route("GetAllTransactionTypes")]

        public async Task<IActionResult> GetAllTransactionTypes()
        {
            var list = await _unitOfWork.genericRepository<TransactionTypesLookup>().GetAllAsync();
            return Ok(list);
        }
        #region ActivityType
        [HttpGet]
        [Route("GetAllActivityTypes")]

        public async Task<IActionResult> GetAllActivityTypes()
        {
            var list = await _unitOfWork.genericRepository<ActivityTypesLookup>().GetAllAsync();
            return Ok(list);
        }

        [HttpGet]
        [Route("GetActivityType")]
        public async Task<IActionResult> GetActivityType(int id)
        {
            var item = await _unitOfWork.genericRepository<ActivityTypesLookup>().GetbyId(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Route("CreateActivityTypes")]
        public async Task<IActionResult> CreateActivityTypes([FromBody] ActivityTypesLookup model)
        {
            await _unitOfWork.genericRepository<ActivityTypesLookup>().Create(model);
            await _unitOfWork.Complete();
            return Ok(model);
        }

        [HttpPost]
        [Route("UpdateActivityTypes")]
        public async Task<IActionResult> UpdateActivityTypes(int id, [FromBody] ActivityTypesLookup model)
        {
            var existing = await _unitOfWork.genericRepository<ActivityTypesLookup>().GetbyId(id);
            if (existing == null) return NotFound();

            existing.NameAr = model.NameAr;
            existing.NameEn = model.NameEn;
            existing.ServiceId = model.ServiceId;
            existing.MainLicenseId = model.MainLicenseId;
            existing.ActivityCode = model.ActivityCode;
            existing.EserviceId = model.EserviceId;

            _unitOfWork.genericRepository<ActivityTypesLookup>().Update(existing);
            await _unitOfWork.Complete();

            return NoContent();
        }

        [HttpPost]
        [Route("DeleteActivityTypes")]
        public async Task<IActionResult> DeleteActivityTypes(int id)
        {
            var item = await _unitOfWork.genericRepository<ActivityTypesLookup>().GetbyId(id);
            if (item == null) return NotFound();

            await _unitOfWork.genericRepository<ActivityTypesLookup>().Delete(item);
            await _unitOfWork.Complete();

            return NoContent();
        }
        #endregion
        #region EserviceTypes
        [HttpGet]
        [Route("GetAllEserviceTypes")]
        public async Task<IActionResult> GetAllEserviceTypes()
        {
            var types = await _unitOfWork.genericRepository<EserviceTypesLookup>().GetAllAsync();
            return Ok(types);
        }

        [HttpGet]
        [Route("GetByIdEserviceTypes")]
        public async Task<IActionResult> GetByIdEserviceTypes(int id)
        {
            var type = await _unitOfWork.genericRepository<EserviceTypesLookup>().GetbyId(id);
            if (type == null) return NotFound();
            return Ok(type);
        }

        [HttpPost]
        [Route("CreateEserviceTypes")]
        public async Task<IActionResult> CreateEserviceTypes([FromBody] EserviceTypesLookup model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _unitOfWork.genericRepository<EserviceTypesLookup>().Create(model);
            await _unitOfWork.Complete();
            return Ok(model);
        }

        [HttpPost]
        [Route("UpdateEserviceTypes")]
        public async Task<IActionResult> UpdateEserviceTypes(int id, [FromBody] EserviceTypesLookup model)
        {
            var existing = await _unitOfWork.genericRepository<EserviceTypesLookup>().GetbyId(id);
            if (existing == null) return NotFound();

            existing.EserviceTypeEn = model.EserviceTypeEn;
            existing.EserviceTypeAr = model.EserviceTypeAr;
            existing.EserviceId = model.EserviceId;
            existing.Url = model.Url;
            existing.IsDeleted = model.IsDeleted;
            existing.CreatedOn = model.CreatedOn;

            _unitOfWork.genericRepository<EserviceTypesLookup>().Update(existing);
            await _unitOfWork.Complete();
            return Ok(existing);
        }

        [HttpPost]
        [Route("DeleteEserviceTypes")]
        public async Task<IActionResult> DeleteEserviceTypes(int id)
        {
            var type = await _unitOfWork.genericRepository<EserviceTypesLookup>().GetbyId(id);
            if (type == null) return NotFound();

            _unitOfWork.genericRepository<EserviceTypesLookup>().Delete(type);
            await _unitOfWork.Complete();
            return Ok();
        }
        #endregion
        #region Eservices
        [HttpGet]
        [Route("GetAllEservice")]

        public async Task<ActionResult<IEnumerable<Eservice>>> GetAllEservice()
        {
            var data = await _unitOfWork.genericRepository<Eservice>().GetAllAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetByIdEservice")]

        public async Task<ActionResult<Eservice>> GetByIdEservice(string id)
        {
            var eservice = await _unitOfWork.genericRepository<Eservice>().GetbyId(id);
            if (eservice == null)
                return NotFound();

            return Ok(eservice);
        }

        [HttpPost]
        [Route("CreateEservice")]
        public async Task<ActionResult> CreateEservice([FromBody] Eservice model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.genericRepository<Eservice>().Create(model);
            await _unitOfWork.Complete();
            return Ok(model);
        }

        [HttpPost]
        [Route("UpdateEservice")]
        public async Task<ActionResult> UpdateEservice(string id, [FromBody] Eservice model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _unitOfWork.genericRepository<Eservice>().GetbyId(id);
            if (existing == null)
                return NotFound();

            existing.EserviceName = model.EserviceName;
            existing.EserviceNameAr = model.EserviceNameAr;
            existing.Url = model.Url;
            existing.CreatedOn = model.CreatedOn;
            existing.IsDeleted = model.IsDeleted;
            existing.ServiceId = model.ServiceId;

           await _unitOfWork.genericRepository<Eservice>().Update(existing);
            await _unitOfWork.Complete();
            return Ok(existing);
        }

        [HttpPost]
        [Route("DeleteEservice")]

        public async Task<ActionResult> DeleteEservice(string id)
        {
            var existing = await _unitOfWork.genericRepository<Eservice>().GetbyId(id);
            if (existing == null)
                return NotFound();

           await _unitOfWork.genericRepository<Eservice>().Delete(existing);
            await _unitOfWork.Complete();
            return Ok();
        }
        #endregion
        #region LicencesInfo
        [HttpGet]
        [Route("GetAllLicenseInfo")]
        public async Task<IActionResult> GetAllLicenseInfo()
        {
            var licencesInfoSpec = new LicencesInfoWithSpec();
            var list = await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().GetTableWithSpec(licencesInfoSpec);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetLicenseInfo")]
        public async Task<IActionResult> GetLicenseInfo(int id)
        {
            var resultspec = new LicencesInfoWithSpec(id);
            var result = await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().GetByIdWithSpec(resultspec);
            return result == null ? NotFound() : Ok(result);
        }
        [HttpGet]
        [Route("GetLicencesInfoDropDown")]
        public async Task<IActionResult> GetLicencesInfoDropDown()
        {
            var servicesTask = await _unitOfWork.genericRepository<Eservice>()
    .GetFilteredWithProjection(
        selector: x => new Eservice { EserviceName = x.EserviceName, ServiceId = x.ServiceId })
    .ToListAsync();

            var activitiesTask = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                .GetFilteredWithProjection(
                    selector: a => new ActivityTypesLookup { Id = a.Id, NameAr = a.NameAr })
                .ToListAsync();

            var requestTypesTask = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new RequestsTypesLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();

            var EserviceTypeBranchTask = await _unitOfWork.genericRepository<EserviceTypeBranch>()
                .GetFilteredWithProjection(
                    selector: r => new EserviceTypeBranch { Id = r.Id, EserviceTypeBranchAr = r.EserviceTypeBranchAr })
                .ToListAsync();
            var LicenceTypeTask = await _unitOfWork.genericRepository<LicenceTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new LicenceTypesLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();

            var transactionType = await _unitOfWork.genericRepository<TransactionTypesLookup>()
                .GetFilteredWithProjection(
                selector: r => new TransactionTypesLookup { Id = r.Id, NameAr = r.NameAr }
                ).ToListAsync();

            var result= new CreateLicencesInfo
            {
                EserviceTypeBranchModel= EserviceTypeBranchTask,
                RequestTypesModel=requestTypesTask,
                LicenceTypesModel=LicenceTypeTask,
                ServicesModel=servicesTask,
                transactionTypesModel=transactionType,
                ActivityTypesModel=activitiesTask
            };
            return Ok(result);
        }

        [HttpGet]
        [Route("GetLicenseInfoWithDropDown")]
        public async Task<IActionResult> GetLicenseInfoWithDropDown(int id)
        {
            var resultspec = new LicencesInfoWithSpec(id);
            var licenseInfo = await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().GetByIdWithSpec(resultspec);
            if (licenseInfo == null)
                return NotFound();

            var servicesTask = await _unitOfWork.genericRepository<Eservice>()
    .GetFilteredWithProjection(
        selector: x => new Eservice { EserviceName = x.EserviceName, ServiceId = x.ServiceId })
    .ToListAsync();

            var activitiesTask = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                .GetFilteredWithProjection(
                    selector: a => new ActivityTypesLookup { Id = a.Id, NameAr = a.NameAr })
                .ToListAsync();

            var requestTypesTask = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new RequestsTypesLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();
            var LicencesTypeTask = await _unitOfWork.genericRepository<LicenceTypesLookup>()
               .GetFilteredWithProjection(
                   selector: r => new LicenceTypesLookup  { Id = r.Id, NameAr = r.NameAr })
               .ToListAsync();

            var EserviceTypeBranchTask = await _unitOfWork.genericRepository<EserviceTypeBranch>()
                .GetFilteredWithProjection(
                    selector: r => new EserviceTypeBranch { Id = r.Id, EserviceTypeBranchAr = r.EserviceTypeBranchAr })
                .ToListAsync();

            var transactionType = await _unitOfWork.genericRepository<TransactionTypesLookup>()
                .GetFilteredWithProjection(
                selector: r => new TransactionTypesLookup { Id = r.Id, NameAr = r.NameAr }
                ).ToListAsync();

            var combinedResult = new LicenseEditViewModel
            {
                License = licenseInfo,
                LicenceTypesModel=LicencesTypeTask,
                ActivityTypesModel = activitiesTask,
                EserviceTypeBranchModel = EserviceTypeBranchTask,
                ServicesModel = servicesTask,
                RequestTypesModel = requestTypesTask,
                TransactionTypesModel = transactionType
            };

            return Ok(combinedResult);
        }

        [HttpPost]
        [Route("AddLicenseInfo")]
        public async Task<IActionResult> AddLicenseInfo(MoiEserviceLicenseInfo model)
        {

            await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().Create(model);
            await _unitOfWork.Complete();
            return Ok(model);
        }

        [HttpPost]
        [Route("UpdateLicenseInfo")]
        public async Task<IActionResult> UpdateLicenseInfo(int id, MoiEserviceLicenseInfo model)
        {
            var existing = await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().GetbyId(id);
            if (existing == null) return NotFound();

            await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().UpdateAsync(model);
            await _unitOfWork.Complete();
            return Ok(model);
        }

        [HttpPost]
        [Route("DeleteLicenseInfo")]
        public async Task<IActionResult> DeleteLicenseInfo(int id)
        {
            var entity = await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().GetbyId(id);
            if (entity == null) return NotFound();

            await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().Delete(entity);
            await _unitOfWork.Complete();
            return NoContent();
        }

        #endregion
        #region BranchType

        [HttpGet]
        [Route("GetAllTypeBranch")]
        public async Task<IActionResult> GetAllTypeBranch()
        {
            var eserviceTypeWithSpec = new EserviceTypeBranchWithSpec();
            var list = await _unitOfWork.genericRepository<EserviceTypeBranch>().GetTableWithSpec(eserviceTypeWithSpec);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetByIdTypeBranch")]
        public async Task<IActionResult> GetByIdTypeBranch(int id)
        {
            var eserviceTypeWithSpec = new EserviceTypeBranchWithSpec(id);
            var item = await _unitOfWork.genericRepository<EserviceTypeBranch>().GetByIdWithSpec(eserviceTypeWithSpec);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [HttpGet]
        [Route("GetEserviceTypeBranchDropDown")]
        public async Task<IActionResult> GetEserviceTypeBranchDropDown()
        {
            var activitiesTask = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                 .GetFilteredWithProjection(
                     selector: a => new ActivityTypesLookup { Id = a.Id, NameAr = a.NameAr })
                 .ToListAsync();

            var requestTypesTask = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new RequestsTypesLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();

            var model = new EserviceTypeBranchViewModel
            {
                ActivityTypes = activitiesTask,
                RequestTypes = requestTypesTask
            };

            return Ok(model);
        }
        [HttpGet]
        [Route("GetEserviceTypeBranchWithIdDropDown")]
        public async Task<IActionResult> GetEserviceTypeBranchWithIdDropDown(int id)
        {
            var eserviceTypeWithSpec = new EserviceTypeBranchWithSpec(id);
            var item = await _unitOfWork.genericRepository<EserviceTypeBranch>().GetByIdWithSpec(eserviceTypeWithSpec);
            var activitiesTask = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                 .GetFilteredWithProjection(
                     selector: a => new ActivityTypesLookup { Id = a.Id, NameAr = a.NameAr })
                 .ToListAsync();

            var requestTypesTask = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new RequestsTypesLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();

            var model = new EserviceTypeBranchViewModel
            {
                Branch= item,
                ActivityTypes = activitiesTask,
                RequestTypes = requestTypesTask
            };

            return Ok(model);
        }

        [HttpPost]
        [Route("CreateTypeBranch")]
        public async Task<IActionResult> CreateTypeBranch([FromBody] EserviceTypeBranch model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.genericRepository<EserviceTypeBranch>().Create(model);
            await _unitOfWork.Complete();
            return Ok( model);
        }

        [HttpPost]
        [Route("UpdateTypeBranch")]
        public async Task<IActionResult> UpdateTypeBranch(int Id,EserviceTypeBranch model)
        {
            //if (Id != model.Id)
            //    return BadRequest("Mismatched ID");

            var existing = await _unitOfWork.genericRepository<EserviceTypeBranch>().GetbyId(model.Id);
            if (existing == null)
                return NotFound();

            await _unitOfWork.genericRepository<EserviceTypeBranch>().UpdateAsync(model);
            await _unitOfWork.Complete();
            return NoContent();
        }

        [HttpPost]
        [Route("DeleteTypeBranch")]
        public async Task<IActionResult> DeleteTypeBranch(int id)
        {
            var item = await _unitOfWork.genericRepository<EserviceTypeBranch>().GetbyId(id);
            if (item == null)
                return NotFound();

            await _unitOfWork.genericRepository<EserviceTypeBranch>().Delete(item);
            await _unitOfWork.Complete();
            return NoContent();
        }
        #endregion
        #region ValidEserviceCombinations CRUD

        [HttpGet]
        [Route("GetValidEserviceCombinations")]
        public async Task<IActionResult> GetValidEserviceCombinations()
        {
            var ValidEserviceCominationWithSpec = new ValidEserviceCominationWithSpec();
            var combinations = await _unitOfWork
                .genericRepository<ValidEserviceCombinations>()
                .GetTableWithSpecService(ValidEserviceCominationWithSpec);

            return Ok(combinations);
        }
        [HttpGet]
        [Route("GetValidEserviceDropDown")]
        public async Task<IActionResult> GetValidEserviceDropDown()
        {
            var activitiesTask = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                 .GetFilteredWithProjection(
                     selector: a => new ActivityTypesLookup { Id = a.Id, NameAr = a.NameAr })
                 .ToListAsync();

            var requestTypesTask = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new RequestsTypesLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();
            var licencesTypesTask = await _unitOfWork.genericRepository<LicenceTypesLookup>()
               .GetFilteredWithProjection(
                   selector: r => new LicenceTypesLookup { Id = r.Id, NameAr = r.NameAr })
               .ToListAsync();

            var model = new ValidEserviceHomePage
            {
                ActivityTypesModel = activitiesTask,
                RequestTypesModel = requestTypesTask,
                LicenceTypesLookup = licencesTypesTask
            };

            return Ok(model);
        }
        [HttpGet]
        [Route("GetValidEserviceWithIdDropDown")]
        public async Task<IActionResult> GetValidEserviceWithIdDropDown(int id)
        {
            var validationWithSpec = new ValidEserviceCominationWithSpec(id);
            var item = await _unitOfWork.genericRepository<ValidEserviceCombinations>().GetByIdWithSpec(validationWithSpec);
            var activitiesTask = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                 .GetFilteredWithProjection(
                     selector: a => new ActivityTypesLookup { Id = a.Id, NameAr = a.NameAr })
                 .ToListAsync();

            var requestTypesTask = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new RequestsTypesLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();
            var licencesTypesTask = await _unitOfWork.genericRepository<LicenceTypesLookup>()
              .GetFilteredWithProjection(
                  selector: r => new LicenceTypesLookup { Id = r.Id, NameAr = r.NameAr })
              .ToListAsync();
            var model = new ValidEserviceHomePage
            {
                ValidEserviceCombinations = item,
                ActivityTypesModel = activitiesTask,
                RequestTypesModel = requestTypesTask,
                LicenceTypesLookup=licencesTypesTask
            };

            return Ok(model);
        }

        [HttpPost]
        [Route("CreateValidEserviceCombination")]
        public async Task<IActionResult> CreateValidEserviceCombination([FromBody] ValidEserviceCombinations model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.genericRepository<ValidEserviceCombinations>().Create(model);
                await _unitOfWork.Complete();
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("EditValidEserviceCombination")]
        public async Task<IActionResult> EditValidEserviceCombination(int id)
        {
            var item = await _unitOfWork.genericRepository<ValidEserviceCombinations>().GetbyId(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Route("EditValidEserviceCombination")]
        public async Task<IActionResult> EditValidEserviceCombination([FromBody] ValidEserviceCombinations model)
        {
            if (ModelState.IsValid)
            {
                var modelWithId= await _unitOfWork.genericRepository<ValidEserviceCombinations>().GetbyId(model.Id);
                await _unitOfWork.genericRepository<ValidEserviceCombinations>().UpdateAsync(model);
                await _unitOfWork.Complete();
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("DeleteValidEserviceCombination")]
        public async Task<IActionResult> DeleteValidEserviceCombination(int id)
        {
            var item = await _unitOfWork.genericRepository<ValidEserviceCombinations>().GetbyId(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Route("DeleteConfirmedValidEserviceCombination")]
        public async Task<IActionResult> DeleteConfirmedValidEserviceCombination(int id)
        {
            var item = await _unitOfWork.genericRepository<ValidEserviceCombinations>().GetbyId(id);
            if (item == null) return NotFound();

            await _unitOfWork.genericRepository<ValidEserviceCombinations>().Delete(item);
            await _unitOfWork.Complete();
            return Ok();
        }

        #endregion

        [HttpGet("GetLicencesTypes")]
        public async Task<IActionResult> GetLicencesTypes()
        {
            var requestTypes = await _unitOfWork.genericRepository<LicenceTypesLookup>().GetAll();
        
         

            var licenceTypes = requestTypes.Select(r => new LicencesTypeVM
            {
                Id = r.Id,
                NameAr = r.NameAr
            }).ToList();

            return Ok(licenceTypes);
        }

        [HttpGet("GetRequestStatus")]
        public async Task<IActionResult> GetRequestStatus()
        {
            var requestStatus = await _unitOfWork.genericRepository<RequestStatusLookup>()
                .GetFilteredWithProjection(
                    // Assuming ServiceId exists in RequestTypesLookup
                    selector: r => new { r.Id, r.NameAr }
                )
                .ToListAsync();

            return Ok(requestStatus);
        }


        [HttpGet]
        [Route("GetAllData")]
        public async Task<IActionResult> GetAllData()
        {
            var servicesTask = await _unitOfWork.genericRepository<Eservice>()
                .GetFilteredWithProjection(
                    selector: x => new Eservice { EserviceName = x.EserviceName, ServiceId = x.ServiceId })
                .ToListAsync();

            //var activitiesTask = await _unitOfWork.genericRepository<ActivityTypesLookup>()
            //    .GetFilteredWithProjection(
            //        selector: a => new ActivityTypesLookup { Id = a.Id, NameAr = a.NameAr })
            //    .ToListAsync();

            var requestTypesTask = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new RequestsTypesLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();

            var requestStatusTask = await _unitOfWork.genericRepository<RequestStatusLookup>()
                .GetFilteredWithProjection(
                    selector: r => new RequestStatusLookup { Id = r.Id, NameAr = r.NameAr })
                .ToListAsync();

            var transactionType = await _unitOfWork.genericRepository<TransactionTypesLookup>()
                .GetFilteredWithProjection(
                selector: r=>new TransactionTypesLookup { Id=r.Id,NameAr=r.NameAr}
                ).ToListAsync();
            // await Task.WhenAll(servicesTask, activitiesTask, requestTypesTask, requestStatusTask);

            var result = new WorkFlowAllSystem
            {
                Services = servicesTask,
               // ActivityTypes = activitiesTask,
                RequestTypes = requestTypesTask,
                RequestStatuses = requestStatusTask,
                transactionTypesLookups=transactionType
            };

            return Ok(result);
        }
        [HttpGet]
        [Route("GetAllWorkflows")]
        public async Task<IActionResult> GetAllWorkflows()
        {

            var workflows = _unitOfWork.genericRepository<WorkFlow>()
                    .GetFilteredWithProjection(
                        filter:null,
                        selector: w => new
                        {
                            w.Id,
                            w.ServiceId,
                            ServiceName = w.Eservice.EserviceNameAr,
                           // ActivityTypeName = w.ActivityTypesLookup.NameAr,
                            CurrentStatusName = w.RequestStatusCurrent.NameAr,
                            NextStatusName = w.RequestStatusNext.NameAr,
                            TransactionTypeName=w.TransactionTypesLookup.NameAr,
                            RequestTypeName = w.RequestsTypesLookup.NameAr,
                            FlagRequestStatus = w.FlagRequestStatus,
                            Conditions=w.Conditions,

                        },
                        w => w.Eservice/*, w => w.ActivityTypesLookup*/, w => w.RequestStatusCurrent, w => w.RequestStatusNext, w => w.RequestsTypesLookup
                          ).ToList();
            return Ok(workflows);
        }

        [HttpGet("GetWorkflow/{id}")]
        public async Task<IActionResult> GetWorkflow(int id)
        {
            // Fetch workflow details
            var workflow = await _unitOfWork.genericRepository<WorkFlow>()
                .GetFilteredWithProjection(
                    filter: w => w.Id == id,
                    selector: w => new
                    {
                        w.Id,
                        w.ServiceId,
                        ServiceName = w.Eservice.EserviceNameAr,
                      //  ActivityTypeName = w.ActivityTypesLookup.NameAr,
                        CurrentStatusName = w.RequestStatusCurrent.NameAr,
                        NextStatusName = w.RequestStatusNext.NameAr,
                        TransactionTypeName = w.TransactionTypesLookup.NameAr,
                       // ActivityTypeId = w.ActivityTypeId,
                        RequestTypeId = w.RequestTypeId,
                        CurrentStatusId = w.CurrentStatusId,
                        NextStatusId = w.NextStatusId,
                        TransactionTypeId = w.TransactionTypeId,
                        RequestTypeName = w.RequestsTypesLookup.NameAr,
                        Flag = w.FlagRequestStatus,
                        Conditions = w.Conditions,
                        IsPermissionRequired = w.IsPermissionRequired

                    },
                    w => w.Eservice/*, w => w.ActivityTypesLookup*/, w => w.RequestStatusCurrent, w => w.TransactionTypesLookup, w => w.RequestStatusNext, w => w.RequestsTypesLookup
                )
                .FirstOrDefaultAsync();

            if (workflow == null)
            {
                return NotFound(new { success = false, message = "Workflow not found" });
            }

            // Fetch dropdown data
            var services = await _unitOfWork.genericRepository<Eservice>()
                .GetFilteredWithProjection(
                    selector: x => new { x.ServiceId, x.EserviceName })
                .ToListAsync();

            var activityTypes = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                .GetFilteredWithProjection(
                    selector: a => new { a.Id, a.NameAr })
                .ToListAsync();

            var requestTypes = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new { r.Id, r.NameAr })
                .ToListAsync();

            var requestStatuses = await _unitOfWork.genericRepository<RequestStatusLookup>()
                .GetFilteredWithProjection(
                    selector: r => new { r.Id, r.NameAr })
                .ToListAsync();

            var transactionTypes = await _unitOfWork.genericRepository<TransactionTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new { r.Id, r.NameAr })
                .ToListAsync();

            // Map dropdown data to SelectListItem
            var result = new WorkFlowVM
            {
                Id = workflow.Id,
                ServiceId = workflow.ServiceId,
                ServiceName = workflow.ServiceName,
                IsPermissionRequired=workflow.IsPermissionRequired,
                
              //  ActivityTypeId = workflow.ActivityTypeId,
               // ActivityTypeName = workflow.ActivityTypeName,
                RequestTypeId = workflow.RequestTypeId,
                RequestTypeName = workflow.RequestTypeName,
                CurrentStatusId = workflow.CurrentStatusId,
                CurrentStatusName = workflow.CurrentStatusName,
                NextStatusId = workflow.NextStatusId,
                NextStatusName = workflow.NextStatusName,
                TransactionTypeName=workflow.TransactionTypeName,
                Conditions = workflow.Conditions,
                FlagRequestStatus = workflow.Flag,
                TransactionTypeId = workflow.TransactionTypeId,
                TransactionTypes = transactionTypes.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.NameAr,
                    Selected = t.Id == workflow.TransactionTypeId
                }),
                Services = services.Select(s => new SelectListItem
                {
                    Value = s.ServiceId.ToString(),
                    Text = s.EserviceName,
                    Selected = s.ServiceId == workflow.ServiceId
                }),
                //ActivityTypes = activityTypes.Select(a => new SelectListItem
                //{
                //    Value = a.Id.ToString(),
                //    Text = a.NameAr,
                //    Selected = a.Id == workflow.ActivityTypeId
                //}),
                RequestTypes = requestTypes.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.NameAr,
                    Selected = r.Id == workflow.RequestTypeId
                }),
                RequestStatus = requestStatuses.Select(rs => new SelectListItem
                {
                    Value = rs.Id.ToString(),
                    Text = rs.NameAr,
                    Selected = rs.Id == workflow.CurrentStatusId || rs.Id == workflow.NextStatusId
                }),
            };

            return Ok(result);
        }




        [HttpPost("AddWorkflow")]
        public async Task<IActionResult> AddWorkflow([FromBody] WorkFlowVM workflowData)
        {
            if (workflowData == null ||
                workflowData.ServiceId <= 0 ||
              //  workflowData.ActivityTypeId <= 0 ||
                workflowData.RequestTypeId <= 0 ||
                workflowData.CurrentStatusId <= 0 ||
                workflowData.NextStatusId <= 0)
            {
                return BadRequest(new { Success = false, Message = "Invalid workflow data. Please ensure all required fields are provided." });
            }

            try
            {
                // Create the workflow object
                var workflow = new WorkFlow
                {
                    ServiceId = workflowData.ServiceId,
                    //ActivityTypeId = workflowData.ActivityTypeId,
                    RequestTypeId = workflowData.RequestTypeId,
                    CurrentStatusId = workflowData.CurrentStatusId,
                    NextStatusId = workflowData.NextStatusId,
                    TransactionTypeId=workflowData.TransactionTypeId,
                    FlagRequestStatus = workflowData.FlagRequestStatus,   
                    Conditions = workflowData.Conditions,   

                };

                // Save the workflow data into the database
                await _unitOfWork.genericRepository<WorkFlow>().Create(workflow);
                var result = await _unitOfWork.Complete();

                if (result > 0)
                {
                    return Ok(new { Success = true, Message = "Workflow saved successfully!" });
                }

                return StatusCode(500, new { Success = false, Message = "Failed to save workflow. Please try again." });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes (optional)
                Console.WriteLine($"Error saving workflow: {ex.Message}");

                return StatusCode(500, new { Success = false, Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost]
        [Route("UpdateMenuItem")]
        public async Task<IActionResult> UpdateMenuItem([FromBody] AddMenuItemVM menuItem)
        {
            if (menuItem == null)
            {
                return BadRequest(new { message = "Invalid menu item data." });
            }

            // Check if the menu item exists
            var existingMenuItem = await _unitOfWork.genericRepository<MenuItem>().GetbyId(menuItem.Id);
            if (existingMenuItem == null)
            {
                return NotFound(new { message = "Menu item not found." });
            }

            // Update the existing menu item with the new values
            existingMenuItem.Name = menuItem.Name;

            existingMenuItem.Url = menuItem.Url;


            existingMenuItem.IsVisible = menuItem.IsVisible;

            // Update the menu item in the database
            await _unitOfWork.genericRepository<MenuItem>().Update(existingMenuItem);
            await _unitOfWork.Complete();

            return Ok(new { message = "Menu item updated successfully." });
        }

        [HttpPost]
        [Route("UpdateWorkFlow")]
        public async Task<IActionResult> UpdateWorkFlow([FromBody] WorkFlowVM workflow)
        {
            if (workflow == null)
            {
                return BadRequest(new { message = "Invalid menu item data." });
            }

            // Check if the menu item exists
            var existingworkflow = await _unitOfWork.genericRepository<WorkFlow>().GetbyId(workflow.Id);
            if (existingworkflow == null)
            {
                return NotFound(new { message = "Menu item not found." });
            }

            // Update the existing menu item with the new values
            existingworkflow.RequestTypeId = workflow.RequestTypeId;
            existingworkflow.CurrentStatusId = workflow.CurrentStatusId;
            existingworkflow.NextStatusId = workflow.NextStatusId;
            //existingworkflow.ActivityTypeId = workflow.ActivityTypeId;
            existingworkflow.ServiceId = workflow.ServiceId;
            existingworkflow.IsPermissionRequired = workflow.IsPermissionRequired;
            existingworkflow.TransactionTypeId = workflow.TransactionTypeId;
            existingworkflow.FlagRequestStatus = workflow.FlagRequestStatus;  
            existingworkflow.Conditions = workflow.Conditions;  
            // Update the menu item in the database
            await _unitOfWork.genericRepository<WorkFlow>().Update(existingworkflow);
            await _unitOfWork.Complete();

            return Ok(new { message = "Menu item updated successfully." });
        }
        // delete WorkFlow row 
        [HttpPost("DeleteWorkflow/{id}")]
        public async Task<IActionResult> DeleteWorkflow(int id)
        {
            var workflow = await _unitOfWork.genericRepository<WorkFlow>().GetbyId(id);
            if (workflow == null)
            {
                return NotFound(new { success = false, message = "Workflow not found" });
            }

            await _unitOfWork.genericRepository<WorkFlow>().Delete(workflow);
            await _unitOfWork.Complete();

            return Ok(new { success = true, message = "Workflow deleted successfully" });
        }


        [HttpGet]
        [Route("GetTransactionTypes")]
        public async Task<IActionResult> GetTransactionTypes()
        {
            var transactionTypes =await _unitOfWork.genericRepository<TransactionTypesLookup>().GetAllAsync();
            var result = transactionTypes.Select(tt => new
            {
                id = tt.Id,
                name = tt.NameAr
            });
            return Ok(result);

           
        }

        // Submit transaction
        //[HttpPost("transactions/submit")]
        //public async Task<IActionResult> SubmitTransactionAsync([FromBody] MoiEservicesRequestTransaction transaction)
        //{
        //    if (transaction == null)
        //    {
        //        return BadRequest("Invalid transaction data.");
        //    }

        //    // Example: Save transaction using UoW
        //    var newTransaction = new MoiEservicesRequestTransaction
        //    {
        //        RequestId = transaction.RequestId,
        //        OperationDate = DateTime.UtcNow,
        //        ServiceId = transaction.ServiceId,
        //        EmployeeId = transaction.EmployeeId,
        //        Notes = transaction.Notes,
        //        StatusId = transaction.StatusId
        //    };

        //    await _unitOfWork.genericRepository<MoiEservicesRequestTransaction>().Create(newTransaction);
        //    await _unitOfWork.Complete();

        //    return Ok(new { Success = true, Message = "Transaction submitted successfully." });
        //}


        #region GetNext Status Dynamically
        [HttpGet("GetNextStatus")]
        public async Task<IActionResult> GetNextStatus(int serviceId/*, int activityTypeId*/, int requestTypeId, int currentStatusId)
        {
            // Retrieve the workflow for the given parameters
            var workflow = await _unitOfWork.genericRepository<WorkFlow>()
                .GetFilteredWithProjection(
                    filter: w => w.ServiceId == serviceId
                             // && w.ActivityTypeId == activityTypeId
                              && w.RequestTypeId == requestTypeId
                              && w.CurrentStatusId == currentStatusId,
                    selector: w => new
                    {
                        w.Id,
                        w.NextStatusId,
                        NextStatusName = w.RequestStatusNext.NameAr,
                        flagRequestStatus= w.FlagRequestStatus,   
                        Conditions= w.Conditions,
                        requestTypeValue = w.FlagRequestType
                    },
                    w => w.RequestStatusNext
                ).FirstOrDefaultAsync();

            
            if (workflow == null)
            {
                return NotFound(new { success = false, message = "No next status found for the given parameters." });
            }

            return Ok(new
            {
                success = true,
                nextStatusId = workflow.NextStatusId,
                flag=workflow.flagRequestStatus,
                nextStatusName = workflow.NextStatusName,
                Conditions=workflow?.Conditions,
                requestTypeValue = workflow.requestTypeValue
            });
        }
        [HttpGet("GetNextStatusToTransaction")]
        public async Task<IActionResult> GetNextStatusToTransaction(int serviceId/*, int activityTypeId*/, int requestTypeId, int currentStatusId,int TransId)
        {
            // Retrieve the workflow for the given parameters
            var workflow = await _unitOfWork.genericRepository<WorkFlow>()
                .GetFilteredWithProjection(
                    filter: w => w.ServiceId == serviceId
                              // && w.ActivityTypeId == activityTypeId
                              && w.RequestTypeId == requestTypeId
                              && w.CurrentStatusId == currentStatusId
                              &&w.TransactionTypeId==TransId,
                    selector: w => new
                    {
                        w.Id,
                        w.NextStatusId,
                        NextStatusName = w.RequestStatusNext.NameAr,
                        flagRequestStatus = w.FlagRequestStatus,
                        Conditions = w.Conditions,
                        requestTypeValue = w.FlagRequestType,
                        TransTypeId=w.TransactionTypeId,
                        TransName=w.TransactionTypesLookup.NameAr
                    },
                    w => w.RequestStatusNext
                ).FirstOrDefaultAsync();


            if (workflow == null)
            {
                return NotFound(new { success = false, message = "No next status found for the given parameters." });
            }

            return Ok(new
            {
                success = true,
                nextStatusId = workflow.NextStatusId,
                flag = workflow.flagRequestStatus,
                nextStatusName = workflow.NextStatusName,
                Conditions = workflow?.Conditions,
                requestTypeValue = workflow.requestTypeValue
            });
        }
        [HttpGet("GetCurrentStatusToTransaction")]
        public async Task<IActionResult> GetCurrentStatusToTransaction(int serviceId,/* int activityTypeId,*/ int requestTypeId, int currentStatusId, int TransId)
        {
            // Retrieve the workflow for the given parameters
            var workflow = await _unitOfWork.genericRepository<WorkFlow>()
                .GetFilteredWithProjection(
                    filter: w => w.ServiceId == serviceId
                              // && w.ActivityTypeId == activityTypeId
                              && w.RequestTypeId == requestTypeId
                              && w.CurrentStatusId == currentStatusId
                              && w.TransactionTypeId == TransId,
                    selector: w => new
                    {
                        w.Id,
                        w.NextStatusId,
                        w.CurrentStatusId,
                        NextStatusName = w.RequestStatusNext.NameAr,
                        CurrentStatusName = w.RequestStatusCurrent.NameAr,
                        flagRequestStatus = w.FlagRequestStatus,
                        TransTypeId = w.TransactionTypeId,
                        TransName = w.TransactionTypesLookup.NameAr,
                        Conditions = w.Conditions,
                    },
                    w => w.RequestStatusNext,
                    w => w.RequestStatusCurrent
                ).FirstOrDefaultAsync();


            if (workflow == null)
            {
                return NotFound(new { success = false, message = "No next status found for the given parameters." });
            }

            return Ok(new
            {
                success = true,
                nextStatusId = workflow.NextStatusId,
                flag = workflow.flagRequestStatus,
                nextStatusName = workflow.NextStatusName,
                Conditions = workflow?.Conditions,
                currentStatusId = workflow.CurrentStatusId,
                currentStatusName = workflow.CurrentStatusName
            });
        }

        [HttpGet("GetCurrentStatus")]
        public async Task<IActionResult> GetCurrentStatus(int serviceId,/* int activityTypeId,*/ int requestTypeId, int currentStatusId)
        {
            // Retrieve the workflow for the given parameters
            var workflow = await _unitOfWork.genericRepository<WorkFlow>()
                .GetFilteredWithProjection(
                    filter: w => w.ServiceId == serviceId
                             // && w.ActivityTypeId == activityTypeId
                              && w.RequestTypeId == requestTypeId
                              && w.CurrentStatusId == currentStatusId,
                    selector: w => new
                    {
                        w.Id,
                        w.NextStatusId,
                        w.CurrentStatusId,
                        NextStatusName = w.RequestStatusNext.NameAr,
                        CurrentStatusName=w.RequestStatusCurrent.NameAr,
                        flagRequestStatus = w.FlagRequestStatus,
                        Conditions = w.Conditions,
                    },
                    w => w.RequestStatusNext,
                    w=>w.RequestStatusCurrent
                ).FirstOrDefaultAsync();


            if (workflow == null)
            {
                return NotFound(new { success = false, message = "No next status found for the given parameters." });
            }

            return Ok(new
            {
                success = true,
                nextStatusId = workflow.NextStatusId,
                flag = workflow.flagRequestStatus,
                nextStatusName = workflow.NextStatusName,
                Conditions = workflow?.Conditions,
                currentStatusId=workflow.CurrentStatusId,
                currentStatusName=workflow.CurrentStatusName
            });
        }
        #endregion
        #endregion
        #region WorkFlow For attachment

        [HttpGet]
        [Route("GetAllAttachmentRules")]
        public async Task<IActionResult> GetAllAttachmentRules()
        {
            var attachmentRules = await _unitOfWork.genericRepository<AttachRule>()
                .GetFilteredWithProjection(
                filter:null,
                   selector:  ar => new AddWorkflowWithAttachmentsVM
                    {
                       Id= ar.Id,
                        AttachName =  ar.AttachName,
                        IsMandatory = ar.IsMandatory,
                        ServiceName = ar.Eservice.EserviceName,
                        RequestTypeName = ar.RequestsTypesLookup.NameAr,
                        TransactionTypeName = ar.TransactionTypesLookup.NameAr,
                        ActivityTypeName = ar.ActivityTypesLookup.NameAr,
                        RequestStatusName = ar.RequestStatusLookup.NameAr,
                       FieldName=ar.FieldName,
                       AllowedFileTypes=ar.AllowedFileTypes,
                       ViewTypeForAttach=ar.ViewType,
                       FlagView = ar.FlagView
                   },
                   ar => ar.ActivityTypesLookup, Ar => Ar.RequestStatusLookup, a => a.TransactionTypesLookup,
                   a => a.RequestsTypesLookup, a => a.Eservice

                ).ToListAsync();

            return Ok(attachmentRules);
        }
        [HttpGet("GetWorkflowAttach/{id}")]
        public async Task<IActionResult> GetWorkflowAttach(int id)
        {
            var workflow = await _unitOfWork.genericRepository<AttachRule>()
                        .GetFilteredWithProjection(

                        filter: w => w.Id == id,
                        selector: w => new
                        {
                            w.Id,
                            w.ServiceId,
                            ServiceName = w.Eservice.EserviceNameAr,
                            AttachName=w.AttachName,
                            RequestTypeId=w.RequestTypeId,
                            TransactionTypeId=w.TransactionTypeId,
                            RequestStatusId=w.RequestStatusId,
                            ActivityTypeName = w.ActivityTypesLookup.NameAr,
                            RequestStatusName =w.RequestStatusLookup.NameAr,
                            TransactionTypeName=w.TransactionTypesLookup.NameAr,
                            RequestTypeName = w.RequestsTypesLookup.NameAr,
                            FieldName = w.FieldName,
                            AllowedFileTypes = w.AllowedFileTypes,
                            ViewTypeForAttach = w.ViewType,
                            FlagView = w.FlagView
                        },
                        w => w.Eservice, w => w.ActivityTypesLookup, w => w.RequestStatusLookup, w => w.TransactionTypesLookup, w => w.RequestsTypesLookup
                          ).FirstOrDefaultAsync();
            if (workflow == null)
            {
                return NotFound(new { success = false, message = "Workflow not found" });
            }



            return Ok(workflow);
        }
        [HttpGet]
        [Route("GetAttachmentRules")]
        public async Task<IActionResult> GetAttachmentRules(int serviceId/*, int activityTypeId*/,int RequestStatusId, int requestTypeId,int? TransactionTypeId)
        {
            var attachmentRules = await _unitOfWork.genericRepository<AttachRule>()
                .GetFilteredWithProjection(
                    filter: ar => ar.ServiceId == serviceId &&
                                 // ar.ActivityTypeId == activityTypeId &&
                                  ar.TransactionTypeId==TransactionTypeId&&
                                  ar.RequestStatusId== RequestStatusId&&
                                  ar.RequestTypeId == requestTypeId&&
                                  ar.FlagView=="Admin",
                    selector: ar => new
                    {
                        ar.Id,
                        ar.AttachName,
                        ar.IsMandatory,
                        ar.ViewType,
                        ar.FlagView,
                        ar.FieldName,
                        
                    },
                   /*ar=>ar.ActivityTypesLookup,*/Ar=>Ar.RequestStatusLookup,a=>a.TransactionTypesLookup,
                   a=>a.RequestsTypesLookup, a => a.Eservice

                ).ToListAsync();

            return Ok(attachmentRules);
        }

        [HttpPost]
        [Route("AddAttachRule")]
        public async Task<IActionResult> AddAttachRule([FromBody] AddAttachmentsRulesVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rule = new AttachRule
            {
                ServiceId = model.ServiceId,
                ActivityTypeId = model.ActivityTypeId,
                RequestTypeId = model.RequestTypeId,
                RequestStatusId = model.RequestStatusId,
                TransactionTypeId = model.TransactionTypeId,
                AttachName = model.AttachName,
                IsMandatory = model.IsMandatory,
                ViewType=model.ViewTypeForAttach,
                AllowedFileTypes = model.AllowedFileTypes,
                FieldName = model.FieldName,    
                FlagView = model.FlagView   ,
                MaxFileSize = model.MaxFileSize,
                Description = model.Description,


            };

            await _unitOfWork.genericRepository<AttachRule>().Create(rule);
            await _unitOfWork.Complete();

            return Ok(new { message = "Attachment Rule created successfully!" });
        }
        [HttpGet("GetWorkflowAttachWithDropDown/{id}")]
        public async Task<IActionResult> GetWorkflowAttachWithDropDown(int id)
        {
            // Fetch workflow details
            var workflow = await _unitOfWork.genericRepository<AttachRule>()
                .GetFilteredWithProjection(
                    filter: w =>  w.Id == id,
                    selector: w => new
                    {
                        w.Id,
                        w.ServiceId,
                        ServiceName = w.Eservice.EserviceNameAr,
                          ActivityTypeName = w.ActivityTypesLookup.NameAr,
                        AttachName=w.AttachName,
                        IsMandatory=w.IsMandatory,
                        TransactionTypeName = w.TransactionTypesLookup.NameAr,
                         ActivityTypeId = w.ActivityTypeId,
                        RequestTypeId = w.RequestTypeId,
                        RequestStatusId=w.RequestStatusId,
                        RequestStatusName=w.RequestStatusLookup.NameAr,
                        TransactionTypeId = w.TransactionTypeId,
                        RequestTypeName = w.RequestsTypesLookup.NameAr,
                        Description=w.Description,
                        MaxFileSize=w.MaxFileSize,
                        FieldName=w.FieldName,
                        AllowedFileTypes=w.AllowedFileTypes,
                        ViewType=w.ViewType,
                        FlagView=w.FlagView,

                    },
                    w => w.Eservice, w => w.ActivityTypesLookup, w => w.RequestStatusLookup, w => w.TransactionTypesLookup, w => w.RequestsTypesLookup
                )
                .FirstOrDefaultAsync();

            if (workflow == null)
            {
                return NotFound(new { success = false, message = "Workflow not found" });
            }

            // Fetch dropdown data
            var services = await _unitOfWork.genericRepository<Eservice>()
                .GetFilteredWithProjection(
                    selector: x => new { x.ServiceId, x.EserviceName })
                .ToListAsync();

            var requestTypes = await _unitOfWork.genericRepository<RequestsTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new { r.Id, r.NameAr })
                .ToListAsync();

            var activityTypes = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new { r.Id, r.NameAr })
                .ToListAsync();

            var requestStatuses = await _unitOfWork.genericRepository<RequestStatusLookup>()
                .GetFilteredWithProjection(
                    selector: r => new { r.Id, r.NameAr })
                .ToListAsync();

            var transactionTypes = await _unitOfWork.genericRepository<TransactionTypesLookup>()
                .GetFilteredWithProjection(
                    selector: r => new { r.Id, r.NameAr })
                .ToListAsync();

            // Map dropdown data to SelectListItem
            var result = new AddAttachmentsRulesVM
            {
                Id = workflow.Id,
                ServiceId = workflow.ServiceId,
                ServiceName = workflow.ServiceName,
                ActivityTypeId = workflow.ActivityTypeId,
                ActivityTypeName = workflow.ActivityTypeName,
                RequestTypeId = workflow.RequestTypeId,
                RequestTypeName = workflow.RequestTypeName,
                RequestStatusName=workflow.RequestStatusName,
                RequestStatusId=workflow.RequestStatusId,
                AttachName=workflow.AttachName,
                IsMandatory=workflow.IsMandatory,
                TransactionTypeName = workflow.TransactionTypeName,
                Description=workflow.Description,
                MaxFileSize=workflow.MaxFileSize,
                FlagView=workflow.FlagView,
                FieldName=workflow.FieldName,
                AllowedFileTypes=workflow.AllowedFileTypes,
                ViewTypeForAttach=workflow.ViewType,
                
                TransactionTypeId = workflow.TransactionTypeId,
                TransactionTypes = transactionTypes.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.NameAr,
                    Selected = t.Id == workflow.TransactionTypeId
                }),
                Services = services.Select(s => new SelectListItem
                {
                    Value = s.ServiceId.ToString(),
                    Text = s.EserviceName,
                    Selected = s.ServiceId == workflow.ServiceId
                }),
                ActivityTypes = activityTypes.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.NameAr,
                    Selected = a.Id == workflow.ActivityTypeId
                }),
                RequestTypes = requestTypes.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.NameAr,
                    Selected = r.Id == workflow.RequestTypeId
                }),
                RequestStatus = requestStatuses.Select(rs => new SelectListItem
                {
                    Value = rs.Id.ToString(),
                    Text = rs.NameAr,
                    Selected = rs.Id == workflow.RequestStatusId 
                }),
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("UpdateWorkFlowAttach")]
        public async Task<IActionResult> UpdateWorkFlowAttach([FromBody] AddAttachmentsRulesVM workflow)
        {
            if (workflow == null)
            {
                return BadRequest(new { message = "Invalid menu item data." });
            }

            // Check if the workflow exists
            var existingworkflow = await _unitOfWork.genericRepository<AttachRule>().GetbyId(workflow.Id);
            if (existingworkflow == null)
            {
                return NotFound(new { message = "Menu item not found." });
            }

            // Update the existing workflow with the new values
            existingworkflow.RequestTypeId = workflow.RequestTypeId;
            existingworkflow.RequestStatusId = workflow.RequestStatusId;
            existingworkflow.AttachName = workflow.AttachName;
            existingworkflow.ActivityTypeId = workflow.ActivityTypeId;
            existingworkflow.ServiceId = workflow.ServiceId;
            existingworkflow.TransactionTypeId = workflow.TransactionTypeId;
            existingworkflow.IsMandatory = workflow.IsMandatory;
            existingworkflow.AllowedFileTypes = workflow.AllowedFileTypes;  
            existingworkflow.FieldName = workflow.FieldName;
            existingworkflow.FlagView = workflow.FlagView;
            existingworkflow.MaxFileSize = workflow.MaxFileSize;
            existingworkflow.Description = workflow.Description;
            existingworkflow.ViewType = workflow.ViewTypeForAttach;
            
            //Update the workflow in the database
            await _unitOfWork.genericRepository<AttachRule>().Update(existingworkflow);
            await _unitOfWork.Complete();

            return Ok(new { message = "Menu item updated successfully." });
        }
        [HttpPost]
        [Route("DeleteAttachmentRule/{id}")]
        public async Task<IActionResult> DeleteAttachmentRule(int id)
        {
            var workflowattach = await _unitOfWork.genericRepository<AttachRule>().GetbyId(id);
            if (workflowattach == null)
            {
                return NotFound(new { success = false, message = "Workflow Attach not found" });
            }

            await _unitOfWork.genericRepository<AttachRule>().Delete(workflowattach);
            await _unitOfWork.Complete();

            return Ok(new { success = true, message = "Workflow Attach deleted successfully" });
        }

        #endregion
        #region Get Dynamic MenuItem
        //public static Expression<Func<RolePermission, bool>> BuildPredicate(List<string> userPermissions)
        //{
        //    return rp => userPermissions.Any(up =>
        //    {
        //        var parts = up.Split('_');
        //        return parts.Length == 3 &&
        //               parts[0] == rp.ModuleId.ToString() &&
        //               parts[1] == rp.MenuItemId.ToString() &&
        //               parts[2] == rp.PermissionId.ToString();
        //    });
        //}

        //[HttpGet]
        //[Route("GetDynamicMenuItems")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetDynamicMenuItems()
        //{
        //    try
        //    {

        //        //if (string.IsNullOrEmpty(token))
        //        //{
        //        //    return RedirectToAction("Login", "Account");
        //        //}

        //        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

        //        // Fetch user permissions from claims
        //        var userPermissions = User.Claims
        //            .Where(c => c.Type == "Permission")
        //            .Select(c => c.Value)
        //            .ToList();

        //        //var predicate = BuildPredicate(userPermissions);

        //        // Fetch all RolePermissions based on userPermissions
        //        var moduleResults = await _unitOfWork.genericRepository<RolePermission>()
        //            .GetFilteredWithProjection(
        //                filter: rp => userPermissions.Any(up => up.StartsWith(rp.ModuleId.ToString() + "_")),
        //                selector: rp => new RolePermissionVM
        //                {
        //                    ModuleId=rp.ModuleId,
        //                    MenuItemId=rp.MenuItemId,
        //                    PermissionId=rp.PermissionId,
        //                    MenuItemName=rp.MenuItem.Name,
        //                    MenuItemUrl=rp.MenuItem.Url,
        //                    MenuItemParentId=rp.MenuItem.ParentId
        //                })
        //            .ToListAsync();

        //        var menuItemResults = await _unitOfWork.genericRepository<RolePermission>()
        //            .GetFilteredWithProjection(
        //                filter: rp => userPermissions.Any(up => up.Contains("_" + rp.MenuItemId.ToString() + "_")),
        //                selector: rp => new RolePermissionVM
        //                {
        //                    ModuleId = rp.ModuleId,
        //                    MenuItemId = rp.MenuItemId,
        //                    PermissionId = rp.PermissionId,
        //                    MenuItemName = rp.MenuItem.Name,
        //                    MenuItemUrl = rp.MenuItem.Url,
        //                    MenuItemParentId = rp.MenuItem.ParentId
        //                })
        //            .ToListAsync();

        //        var permissionResults = await _unitOfWork.genericRepository<RolePermission>()
        //            .GetFilteredWithProjection(
        //                filter: rp => userPermissions.Any(up => up.EndsWith("_" + rp.PermissionId.ToString())),
        //                selector: rp => new RolePermissionVM
        //                {
        //                    ModuleId = rp.ModuleId,
        //                    MenuItemId = rp.MenuItemId,
        //                    PermissionId = rp.PermissionId,
        //                    MenuItemName = rp.MenuItem.Name,
        //                    MenuItemUrl = rp.MenuItem.Url,
        //                    MenuItemParentId = rp.MenuItem.ParentId
        //                })
        //            .ToListAsync();

        //        // Combine results in memory
        //        var combinedResults = moduleResults
        //                         .Concat(menuItemResults)
        //                         .Concat(permissionResults)
        //                         .GroupBy(rp => rp.MenuItemId) // Group by MenuItemId to remove duplicates
        //                         .Select(g => new RolePermissionVM
        //                         {
        //                             MenuItemId = g.First().MenuItemId,
        //                             MenuItemName = g.First().MenuItemName,
        //                             MenuItemUrl = g.First().MenuItemUrl,
        //                             MenuItemParentId = g.First().MenuItemParentId,
        //                             // Collect unique permissions
        //                         })
        //                         .ToList();



        //        return Ok(combinedResults);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
        [HttpGet]
        [Route("GetDynamicMenuItems")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDynamicMenuItems()
        {
            try
            {
                var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
                //var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.r);

                // Fetch user roles from claims
                var userRoles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                // Fetch user permissions from claims
                var userPermissions = User.Claims
                    .Where(c => c.Type == "Permission")
                    .Select(c => c.Value)
                    .ToList();

                // Fetch all RolePermissions based on userRoles and userPermissions
                var moduleResults = await _unitOfWork.genericRepository<RolePermissionAdmin>()
                    .GetFilteredWithProjection(
                        filter: rp => userRoles.Contains(rp.Role.Name) && userPermissions.Any(up => up.StartsWith(rp.ModuleId.ToString() + "_")),
                        selector: rp => new RolePermissionVM
                        {
                            ModuleId = rp.ModuleId,
                            ModuleName=rp.Module.Name,
                            MenuItemId = rp.MenuItemId,
                            PermissionId = rp.PermissionAdminId,
                            MenuItemName = rp.MenuItem.Name,
                            MenuItemUrl = rp.MenuItem.Url,
                            MenuItemParentId = rp.MenuItem.ParentId
                        },
                        includes:x=>x.Module)
                    .ToListAsync();

                var menuItemResults = await _unitOfWork.genericRepository<RolePermissionAdmin>()
                    .GetFilteredWithProjection(
                        filter: rp => userRoles.Contains(rp.Role.Name) && userPermissions.Any(up => up.Contains("_" + rp.MenuItemId.ToString() + "_")),
                        selector: rp => new RolePermissionVM
                        {
                            ModuleId = rp.ModuleId,
                            ModuleName=rp.Module.Name,
                            MenuItemId = rp.MenuItemId,
                            PermissionId = rp.PermissionAdminId,
                            MenuItemName = rp.MenuItem.Name,
                            MenuItemUrl = rp.MenuItem.Url,
                            MenuItemParentId = rp.MenuItem.ParentId
                        })
                    .ToListAsync();

                var permissionResults = await _unitOfWork.genericRepository<RolePermissionAdmin>()
                    .GetFilteredWithProjection(
                        filter: rp => userRoles.Contains(rp.Role.Name) && userPermissions.Any(up => up.EndsWith("_" + rp.PermissionAdminId.ToString())),
                        selector: rp => new RolePermissionVM
                        {
                            ModuleId = rp.ModuleId,
                            MenuItemId = rp.MenuItemId,
                            PermissionId = rp.PermissionAdminId,
                            ModuleName=rp.Module.Name,
                            MenuItemName = rp.MenuItem.Name,
                            MenuItemUrl = rp.MenuItem.Url,
                            MenuItemParentId = rp.MenuItem.ParentId
                        })
                    .ToListAsync();

                // Combine results in memory and group by ModuleId to remove duplicates
                var combinedResults = moduleResults
                                 .Concat(menuItemResults)
                                 .Concat(permissionResults)
                                 .GroupBy(rp => rp.ModuleId) // Group by ModuleId to remove duplicates
                                 .Select(g => new
                                 {
                                     Id = g.Key,
                                    
                                     Name = g.First().ModuleName,  // Assuming the Module name is derived from MenuItemName
                                     MenuItems = g
                                      .GroupBy(item => item.MenuItemId) // Group by MenuItemId to remove duplicates
                                      .Select(itemGroup => new
                                      {
                                          Id = itemGroup.Key,
                                          Name = itemGroup.First().MenuItemName,
                                          Url = itemGroup.First().MenuItemUrl
                                      }).ToList()
                                 })
                                 .ToList();

                return Ok(combinedResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region workFlowActionButton

        [HttpGet]
        [Route("GetAllWorkFlowActionButtons")]
        public async Task<IActionResult> GetAllWorkFlowActionButtons()
        {

            var WorkFlowActionWithSpec = new WorkFlowActionButtonWithSpec();
            var buttons = await _unitOfWork.genericRepository<WorkFlowActionButton>().GetTableWithSpec(WorkFlowActionWithSpec);


            return Ok(_mapper.Map<IEnumerable<WorkFlowActionButton>, IEnumerable<WorkFlowActionButtonVM>>(buttons));
        }

        [HttpGet]
        [Route("GetByIdWorkFlowActionButton/{id}")]
        public async Task<IActionResult> GetByIdWorkFlowActionButton(int id)
        {
            var WorkFlowActionWithSpec = new WorkFlowActionButtonWithSpec(id);
            var button = await _unitOfWork.genericRepository<WorkFlowActionButton>().GetByIdWithSpec(WorkFlowActionWithSpec);


            if (button == null)
                return NotFound();

            return Ok(_mapper.Map<WorkFlowActionButton, WorkFlowActionButtonVM>(button));
        }

        [HttpPost]
        [Route("AddWorkFlowActionButton")]
        public async Task<IActionResult> AddWorkFlowActionButton([FromBody] WorkFlowActionButtonVM model)
        {
            var entity = new WorkFlowActionButton
            {
                WorkFlowId = model.WorkFlowId,
                ButtonText = model.ButtonText,
                ActionKey = model.ActionKey,
                PermissionKey = model.PermissionKey
            };

            await _unitOfWork.genericRepository<WorkFlowActionButton>().Create(entity);
            await _unitOfWork.Complete();

            return Ok(model);
        }

        [HttpPost]
        [Route("UpdateWorkFlowActionButton/{id}")]
        public async Task<IActionResult> UpdateWorkFlowActionButton(int id, [FromBody] WorkFlowActionButtonVM model)
        {
            var button = await _unitOfWork.genericRepository<WorkFlowActionButton>().GetbyId(id);
            if (button == null)
                return NotFound();

            button.ButtonText = model.ButtonText;
            button.ActionKey = model.ActionKey;
            button.PermissionKey = model.PermissionKey;
            button.WorkFlowId = model.WorkFlowId;

            await _unitOfWork.genericRepository<WorkFlowActionButton>().Update(button);
            await _unitOfWork.Complete();

            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("DeleteWorkFlowActionButton/{id}")]
        public async Task<IActionResult> DeleteWorkFlowActionButton(int id)
        {
            var button = await _unitOfWork.genericRepository<WorkFlowActionButton>().GetbyId(id);
            if (button == null)
                return NotFound();

            await _unitOfWork.genericRepository<WorkFlowActionButton>().Delete(button);
            await _unitOfWork.Complete();

            return Ok(new { success = true });
        }

        #endregion
        #region workFlowButtonRoleAdmin

        [HttpGet]
        [Route("GetAllWorkFlowButtonRoleAdmin")]
        public async Task<IActionResult> GetAllWorkFlowButtonRoleAdmin()
        {

            var WorkFlowButtonRoleAdminWithSpec = new WorkFlowButtonRoleAdminWithSpec();
            var buttons = await _unitOfWork.genericRepository<WorkFlowButtonRoleAdmin>().GetTableWithSpec(WorkFlowButtonRoleAdminWithSpec);


            return Ok(buttons);
        }

        [HttpGet]
        [Route("GetByIdWorkFlowButtonRoleAdmin/{id}")]
        public async Task<IActionResult> GetByIdWorkFlowButtonRoleAdmin(int id)
        {
            var WorkFlowButtonRoleAdminWithSpec = new WorkFlowButtonRoleAdminWithSpec(id);
            var button = await _unitOfWork.genericRepository<WorkFlowButtonRoleAdmin>().GetByIdWithSpec(WorkFlowButtonRoleAdminWithSpec);


            if (button == null)
                return NotFound();

            return Ok(button);
        }

        [HttpPost]
        [Route("AddWorkFlowButtonRoleAdmin")]
        public async Task<IActionResult> AddWorkFlowButtonRoleAdmin([FromBody] WorkFlowButtonRoleAdmin model)
        {
            var entity = new WorkFlowButtonRoleAdmin
            {

                RoleAdminId = model.RoleAdminId,
                WorkFlowActionButtonId = model.WorkFlowActionButtonId,
            };

            await _unitOfWork.genericRepository<WorkFlowButtonRoleAdmin>().Create(entity);
            await _unitOfWork.Complete();

            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("UpdateWorkFlowButtonRoleAdmin/{id}")]
        public async Task<IActionResult> UpdateWorkFlowButtonRoleAdmin(int id,[FromBody] WorkFlowButtonRoleAdmin model)
        {
            var button = await _unitOfWork.genericRepository<WorkFlowButtonRoleAdmin>().GetbyId(model.Id);
            if (button == null)
                return NotFound();

            button.WorkFlowActionButtonId = model.WorkFlowActionButtonId;
            button.RoleAdminId = model.RoleAdminId;

            await _unitOfWork.genericRepository<WorkFlowButtonRoleAdmin>().Update(button);
            await _unitOfWork.Complete();

            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("DeleteWorkFlowButtonRoleAdmin/{id}")]
        public async Task<IActionResult> DeleteWorkFlowButtonRoleAdmin(int id)
        {
            var button = await _unitOfWork.genericRepository<WorkFlowButtonRoleAdmin>().GetbyId(id);
            if (button == null)
                return NotFound();

            await _unitOfWork.genericRepository<WorkFlowButtonRoleAdmin>().Delete(button);
            await _unitOfWork.Complete();

            return Ok(new { success = true });
        }

        #endregion

        #region CheckForUserAccessButtonOrNot
        [HttpGet]
        [Route("GetAllowedButtons")]
        public async Task<IActionResult> GetAllowedButtons(long requestId,int NextStatusId,int userId)
        {
            // 1. Get request by Id
            var request = await _unitOfWork.genericRepository<MoiEserviceLicensesRequest>()
                .GetbyId(requestId);

            //if (request == null)
            //    return NotFound("Request not found");

            //var currentStatusId = request.StatusId;

            // 2. Get current user ID (from session or claim)
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            
            // 3. Get current user's role (assumes user table has RoleAdminId)
            var user = await _unitOfWork.genericRepository<MoiEserviceSysUser>()
                .GetbyId(userId);

            var role = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>()
                .GetByCondition(a => a.SysUserId == userId).FirstOrDefaultAsync();

            //if (roleId == null)
            //    return BadRequest("User does not have a role");
            var Workflowt = await _unitOfWork.genericRepository<WorkFlow>()
                .GetByCondition(w => w.NextStatusId == NextStatusId
                &&w.RequestTypeId==request.ReqtypeId
                &&w.ServiceId==request.ServiceId).FirstOrDefaultAsync();

            // 4. Get all buttons related to the current status
            var buttonsForThisStatus = await _unitOfWork.genericRepository<WorkFlowActionButton>()
                .GetByCondition(b => b.WorkFlowId == Workflowt.Id)
                .ToListAsync();

            // 5. Get allowed button IDs for this role
            var allowedButtonIds = await _unitOfWork.genericRepository<WorkFlowButtonRoleAdmin>()
                .GetByCondition(rb => rb.RoleAdminId == role.RoleId)
                .Select(rb => rb.WorkFlowActionButtonId)
                .ToListAsync();

            // 6. Filter allowed buttons
            var allowedButtons = buttonsForThisStatus
                .Where(b => allowedButtonIds.Contains(b.Id))
                .Select(b => new
                {
                    b.Id,
                    b.ButtonText,
                    b.WorkFlow.CurrentStatusId,
                    b.WorkFlow.NextStatusId,
                    b.ActionKey
                })
                .ToList();

            return Ok(allowedButtons);
        }

        #endregion
    }
}
