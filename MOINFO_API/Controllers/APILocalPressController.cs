using AutoMapper;
using Business.Enums;
using Business.Interfaces;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MOINFO_API.Controllers
{
    [Route("api/LocalPress")]
    [ApiController]
    public class APILocalPressController : ControllerBase
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IUpdateDataService _updateDataService;
        private readonly IMapper _mapper;
        private readonly IDataFetchService _dataFetchService;
        public APILocalPressController(IUnitOfwork unitOfwork, IUpdateDataService updateDataService, IMapper mapper, IDataFetchService dataFetchService)
        {
            _unitOfwork = unitOfwork;
            _updateDataService = updateDataService;
            _mapper = mapper;
            _dataFetchService = dataFetchService;
        }

        [HttpGet]
        [Route("GetActivity")]
        public async Task<IEnumerable<ActivityTypeVM>> GetActivity([FromQuery] int ID)
        {
            var entities = await _unitOfwork
                .genericRepository<ActivityTypesLookup>()
                .GetByCondition(c => c.ServiceId == ID)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<ActivityTypesLookup>, IEnumerable<ActivityTypeVM>>(entities);

            return mapped;
        }
        [HttpGet]
        [Route("GetPesronTypes")]
        public async Task<IEnumerable<PesronTypeLookUpVM>> GetPesronTypes()
        {
            var entities = await _unitOfwork.genericRepository<PesronTypeLookUp>().GetAllAsync();

            var mapped = _mapper.Map<IEnumerable<PesronTypeLookUp>, IEnumerable<PesronTypeLookUpVM>>(entities);

            return mapped;
        }
        [HttpGet]
        [Route("GetAttachmentForRequest")]
        public async Task<IEnumerable<Business.ViewModel.AttachRuleVM>> GetAttachmentForRequest(string ViewType)
        {
            var entities = await _unitOfwork.genericRepository<AttachRule>().GetByCondition(c => c.ViewType == ViewType && c.FlagView == "user")
    .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<AttachRule>, IEnumerable<Business.ViewModel.AttachRuleVM>>(entities);

            return mapped;
        }

        [HttpGet]
        [Route("GetAttachmentForModify")]
        public async Task<IEnumerable<Business.ViewModel.AttachRuleVM>> GetAttachmentForModify(string ViewType)
        {
            var entities = await _unitOfwork.genericRepository<AttachRule>().GetByCondition(c => c.ViewType == ViewType && c.FlagView == "user" && c.ServiceId ==(int) ServiceEnum.LocalPress)
    .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<AttachRule>, IEnumerable<Business.ViewModel.AttachRuleVM>>(entities);

            return mapped;
        }
        [HttpGet]
        [Route("GetScheduleReleaseTypes")]
        public async Task<IEnumerable<ScheduleReleaseTypesVM>> GetScheduleReleaseTypes()
        {
            var entities = await _unitOfwork.genericRepository<ScheduleReleaseTypes>().GetAllAsync();

            var mapped = _mapper.Map<IEnumerable<ScheduleReleaseTypes>, IEnumerable<ScheduleReleaseTypesVM>>(entities);

            return mapped;
        }

        [HttpGet]
        [Route("GetLicensesType")]
        public async Task<IEnumerable<LicenceTypesLookupVM>> GetLicensesType()
        {
            // IDs you want to filter by (IN clause)
            int[] types = new int[] { 1, 2, 3 };

            var entities = await _unitOfwork
                .genericRepository<LicenceTypesLookup>()
                .GetByCondition(c => types.Contains(c.Id)) 
                .ToListAsync();

            // Map entities to VMs
            var mapped = _mapper.Map<IEnumerable<LicenceTypesLookupVM>>(entities);

            return mapped;
        }

    }
}
