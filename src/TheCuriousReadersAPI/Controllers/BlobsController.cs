using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Authorize(Policy = Policies.RequireAdministratorOrLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public BlobsController(IBlobService blobService)
        {
            _blobService = blobService;
        }


        [HttpGet("list")]
        public async Task<IActionResult> ListBlobs()
        {
            return Ok(await _blobService.ListBlobsAsync());
        }

        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> Get(string fileName)
        {
            var imgBytes = await _blobService.GetAsync(fileName);

            if (imgBytes.IsNullOrEmpty())
                return NotFound();

            return File(imgBytes, "image/webp");
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileModel model)
        {
            try
            {
                return Ok(await _blobService.UploadAsync(model));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string fileName)
        {
            try
            {
                await _blobService.DeleteAsync(fileName);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            return Ok();
        }
    }
}
