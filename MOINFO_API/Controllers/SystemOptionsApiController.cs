using AutoMapper;
using Business.Interfaces;
using Business.ViewModel;
using Business.ViewModel.Account;
using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MOINFO_API.Controllers
{
    [Route("SystemOptions")]
    public class SystemOptionsApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IMapper _mapper;

        public SystemOptionsApiController(IUnitOfwork unitOfwork,IMapper mapper)
        {
            _unitOfwork = unitOfwork;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetAllOptions")]
        public async Task<IActionResult> GetAllOptions()
        {
            var options=await _unitOfwork.genericRepository<SystemOption>().GetByCondition(c=>c.IsDeleted ==false).ToListAsync();
            return Ok(_mapper.Map<List<SystemOption>,List<SystemOptionVM>>(options)); 
        }

        [HttpGet]
        [Route("GetAllOptionsWithUserInfo")]
        public async Task<IActionResult> GetAllOptionsWithUserInfo(string civilid)
        {
            var options = await _unitOfwork.genericRepository<SystemOption>().GetByCondition(c => c.IsDeleted == false).ToListAsync();
            var user = await _unitOfwork.genericRepository<AspNetUser>()
                .GetByCondition(a => a.CivilId == civilid).FirstOrDefaultAsync();
            var mappedOption = _mapper.Map<List<SystemOption>, List<SystemOptionVM>>(options);
            return Ok(new userWithSystemOption
            {
                SystemOptions = mappedOption,
                aspnetUserVM=_mapper.Map<AspNetUser,AspnetUserVM>(user),

            } );
        }
        [HttpGet]
        [Route("GetSystemOption")]
        public async Task<ActionResult<SystemOption>> GetSystemOption(int id)
        {
            var systemOption = await _unitOfwork.genericRepository<SystemOption>().GetbyId(id);

            if (systemOption == null)
            {
                return NotFound();
            }

            return Ok(systemOption);
        }
        [HttpPost]
        [Route("HandlePost")]
        public async Task<IActionResult> HandlePost([FromBody] SystemOptionRequest request)
        {

            if (request == null)
            {
                return BadRequest("Request body is null");
            }

            var actionType = request.ActionType;
            var data = request.Data;  // This is the SystemOptionVM object

            if (actionType == "create")
            {
                // Map SystemOptionVM to SystemOption entity using AutoMapper
                var systemOption = _mapper.Map<SystemOptionVM, SystemOption>(data);
                systemOption.CreationDate = DateTime.Now; // Set CreationDate manually if needed

                await _unitOfwork.genericRepository<SystemOption>().Create(systemOption);
                await _unitOfwork.Complete();

                return CreatedAtAction(nameof(GetAllOptions), new { id = systemOption.Id }, systemOption);
            }
            else if (actionType == "update")
            {
                var systemOption = await _unitOfwork.genericRepository<SystemOption>().GetbyId(data.Id);

                if (systemOption == null)
                {
                    return NotFound();
                }

                // Map SystemOptionVM to SystemOption for updating
                _mapper.Map(data, systemOption);
                systemOption.ModificationDate = DateTime.Now;

                _unitOfwork.genericRepository<SystemOption>().Update(systemOption);
                await _unitOfwork.Complete();

                return Ok();
            }
            else if (actionType == "delete")
            {
                var systemOption = await _unitOfwork.genericRepository<SystemOption>().GetbyId(data.Id);

                if (systemOption == null)
                {
                    return NotFound();
                }

                systemOption.IsDeleted = true;
                systemOption.ModificationDate = DateTime.Now;
                await _unitOfwork.genericRepository<SystemOption>().Update(systemOption);
                await _unitOfwork.Complete();

                return Ok();
            }

            return BadRequest("Invalid action type");
        }

     
    }
}
