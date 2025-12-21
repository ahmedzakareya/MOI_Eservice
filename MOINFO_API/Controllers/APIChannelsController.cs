using AutoMapper;
using Business.Enums;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.ViewModel;
using Business.ViewModel.Channels;
//using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.BuilderProperties;
using System.ComponentModel;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MOINFO_API.Controllers
{
    [Route("api/Channels")]
    public class APIChannelsController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IUpdateDataService _updateDataService;
        private readonly IMapper _mapper;
        private readonly IDataFetchService _dataFetchService;

        public APIChannelsController(IUnitOfwork unitOfwork, IUpdateDataService updateDataService, IMapper mapper, IDataFetchService dataFetchService)
        {
            _unitOfwork = unitOfwork;
            _updateDataService = updateDataService;
            _mapper = mapper;
            _dataFetchService = dataFetchService;
        }
        [Route("GetAllRequest")]
        [HttpGet]
        public async Task<IEnumerable<RequestVM>> GetAllRequest(int ServiceId, string requestTypes)
        {
            var requestTypeIds = requestTypes.Split(',').Select(int.Parse).ToList();
            var requestspec = new RequestWithSpecificService(ServiceId, requestTypeIds);
            var Requests = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpecService(requestspec);
            var RequestMapped = _mapper.Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(Requests);
            if (requestTypeIds.Contains((int)RequestTypeEnum.ChangeData))
            {
                foreach (var request in RequestMapped)
                {
                    var transactionSpec = new TransactionWithSpec(ServiceId);
                    var transactions = await _unitOfwork.genericRepository<Transaction>().GetTableWithSpec(transactionSpec);
                    var filteredTransactions = transactions
                                     .Where(t => t.RequestId == request.RequestId)
                                     .Select(t => new TransactionVM
                                     {
                                         Id = t.Id,
                                         LicenseId = t.LicenseId,
                                         ServiceId = t.ServiceId ?? 0,
                                         TransTypeId = t.TransTypeId,
                                         MotletterNo = t.MotletterNo,
                                         Changes = t.Changes,
                                         Commited = t.Commited,
                                         Notes = t.Notes,
                                         LastUpdateUser = t.LastUpdateUser,
                                         LastUpdateDate = t.LastUpdateDate,
                                         RequestId = t.RequestId,
                                         MotletterDate = t.MotletterDate,
                                         RequestDate = t.RequestDate,
                                         UsercivilId = t.UsercivilId,
                                         ReqStatusId = t.ReqStatusId,
                                         TransDate = t.TransDate
                                     });

                    // Assign mapped transactions to the request
                    request.Transactions = filteredTransactions.ToList();
                }
            }

            return RequestMapped;
        }
        [Route("GetAllLicences")]
        [HttpGet]
        public async Task<IEnumerable<LicencesVM>> GetAllLicences(int ServiceId)
        {

            var Licencesspec = new LicencesWithSpecificService(ServiceId, false);
            var Licences = await _unitOfwork.genericRepository<Licence>().GetTableWithSpecService(Licencesspec);
            var LicencesMapped = _mapper.Map<IEnumerable<Licence>, IEnumerable<LicencesVM>>(Licences);

            return LicencesMapped;
        }


        [Route("GetAllCountries")]
        [HttpGet]
        public async Task<IEnumerable<CountriesLookupVM>> GetAllCountries()
        
        {


            var model = await _unitOfwork.genericRepository<CountriesLookup>().GetByCondition(c => c.Id != 156).ToListAsync();
            var Mapped = _mapper.Map<IEnumerable<CountriesLookup>, IEnumerable<CountriesLookupVM>>(model);

            return Mapped;
        }


        [Route("GetAttchRule")]
        [HttpGet]
        public async Task<IEnumerable<AttachRuleVM>> GetAttchRule([FromQuery] AttachRuleVM model)
        {
            var spec = await _unitOfwork
                .genericRepository<AttachRule>()
                .GetByCondition(c =>
                    c.ActivityTypeId == model.ActivityTypeId &&
                    c.ServiceId == model.ServiceId &&
                    c.RequestTypeId == model.RequestTypeId && c.RequestStatusId == model.RequestStatusId)
                .ToListAsync();

            var Mapped = _mapper.Map<IEnumerable<AttachRule>, IEnumerable<AttachRuleVM>>(spec);

            return Mapped;
        }


        [Route("AddNewChannelRequest")]
        [HttpPost]
        public async Task<MoiEserviceLicensesRequestVM> AddNewChannelRequest([FromBody] MoiEserviceLicensesRequestVM model)
        {
            try
            {
                await _unitOfwork.BeginTransactionAsync();

                var requestEntity = new MoiEserviceLicensesRequest();

     
                var nextRequestId = await GetNextRequestIdForService(9);
                model.Reqno = "MOICH-" + nextRequestId;
                model.ServiceId = 9;
                

                var companyEntity = new Company
                {
                    Name = model.Licname,
                    ServiceId = 9,
                    ActivityTypeId = 33
                };

                await _unitOfwork.genericRepository<Company>().Create(companyEntity);
                await _unitOfwork.Complete();

                requestEntity.CompanyId = companyEntity.Id;

                _mapper.Map(model, requestEntity);


                await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);
                await _unitOfwork.Complete();


                var licenseEntity = new Licence
                {
                   
                    IssueDate = DateTime.Now,
                   
                    LicName = model.Licname,
                    CompanyId = companyEntity.Id,
                    ServiceId = 9,
                    ActiivityTypeId = 33,
                    LicStatusId = 1,
                    LicTypeId =1,
                    Location = model.LicLocation,
                    LicenseNationality = model.LicNationality,
                   
                };

                await _unitOfwork.genericRepository<Licence>().Create(licenseEntity);
                await _unitOfwork.Complete();

                await _unitOfwork.CommitTransactionAsync();

                var result = _mapper.Map<MoiEserviceLicensesRequestVM>(requestEntity);
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfwork.RollbackTransactionAsync();
                throw;
            }
        }








        private async Task<long> GetNextRequestIdForService(int serviceId)
        {
            var requests = await _unitOfwork
                .genericRepository<MoiEserviceLicensesRequest>()
                .GetAll();

            var lastRequestId = requests
                .Where(x => x.ServiceId == serviceId)
                .Select(x => x.RequestId)
                .DefaultIfEmpty(0)
                .Max();

            return lastRequestId + 1;
        }
       




        //// new request 
        //[Route("RepresentativeOffice")]
        //[HttpPost]
        //public async Task <MoiEserviceLicensesRequestVM> RepresentativeOfficeNewRequest (MoiEserviceLicensesRequestVM model)
        //{
        //     // Add licese 



        //}

    }
}
