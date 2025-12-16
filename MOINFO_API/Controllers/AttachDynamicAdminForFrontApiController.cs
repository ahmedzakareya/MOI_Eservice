using AutoMapper;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MOINFO_API.Controllers
{
    [Route("api/AttachDynamicAdmin")]
    public class AttachDynamicAdminForFrontApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IMapper _mapper;

        public AttachDynamicAdminForFrontApiController(IUnitOfwork unitOfwork, IMapper mapper)
        {
            _unitOfwork = unitOfwork;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Route("GetFileUploadConfigurations")]
        public async Task<IEnumerable<FileUploadConfigurationsFront>> GetFileUploadConfigurations()
        {
            var fileWithSpec = new FileUploadConfigFrontWithSpec();

            var fileConfigs = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>().GetTableWithSpec(fileWithSpec);

            //if (fileConfigs == null || !fileConfigs.Any())
            //{
            //    return NotFound();  // Return a 404 if no file configurations are found
            //}

            return fileConfigs;  // Return the list of file configurations
        }

        // GET: api/FileUploadConfigApi/5
        [HttpGet]
        [Route("GetFileUploadConfig/{id}")]
        public async Task<ActionResult<FileUploadConfigurationsFront>> GetFileUploadConfig(int id)
        {

            var fileWithSpec=new FileUploadConfigFrontWithSpec(id);
            var fileConfig = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>().GetByIdWithSpec(fileWithSpec);

            if (fileConfig == null)
            {
                return NotFound();
            }

            return fileConfig;
        }

        // POST: api/FileUploadConfigApi
        [HttpPost]
        [Route("PostFileUploadConfig")]
        public async Task<ActionResult<FileUploadConfigurationsFront>> PostFileUploadConfig(FileUploadConfigurationsFront model)
        {
            if (ModelState.IsValid)
            {
                await _unitOfwork.genericRepository<FileUploadConfigurationsFront>().Create(model);
                await _unitOfwork.Complete();

                return CreatedAtAction(nameof(GetFileUploadConfig), new { id = model.Id }, model);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/FileUploadConfigApi/5
        [HttpPost]
        [Route("PutFileUploadConfig")]
        public async Task<IActionResult> PutFileUploadConfig( FileUploadConfigurationsFront model)
        {
            if ( model.Id ==0)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _unitOfwork.genericRepository<FileUploadConfigurationsFront>().UpdateAsync(model);
                await _unitOfwork.Complete();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/FileUploadConfigApi/5
        [HttpDelete]
        [Route("DeleteFileUploadConfig/{id}")]
        public async Task<IActionResult> DeleteFileUploadConfig(int id)
        {
            var fileConfig = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>().GetbyId(id);

            if (fileConfig == null)
            {
                return NotFound();
            }

          await  _unitOfwork.genericRepository<FileUploadConfigurationsFront>().Delete(fileConfig);
            await _unitOfwork.Complete();

            return NoContent();
        }

    }
}
