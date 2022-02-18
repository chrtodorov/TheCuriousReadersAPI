﻿using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Publishers;
using BusinessLayer.Requests;
using DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = Policies.RequireLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublishersService _publishersService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(IPublishersService publishersService, ILogger<PublishersController> logger)
        {
            this._publishersService = publishersService;
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{publisherId}")]
        public async Task<IActionResult> Get(Guid publisherId)
        {
            _logger.LogInformation("Get Publisher {@PublisherId}", publisherId);

            var result = await _publishersService.Get(publisherId);

            if (result is null)
            {
                return NotFound("Publisher with such Id is not found!");
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get All Publishers");
            var result = await _publishersService.GetAll();

            if (result is null)
            {
                return NotFound("No existing publishers");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]PublisherRequest publisherRequest)
        {
            _logger.LogInformation("Create Publisher" + publisherRequest.ToString());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _publishersService.Create(publisherRequest.ToPublisher()));
        }

        [HttpPut ("{publisherId}")]
        public async Task<IActionResult> Update(Guid publisherId, [FromBody] PublisherRequest publisherRequest)
        {
            _logger.LogInformation("Update Publisher" + publisherRequest.ToString());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _publishersService.Contains(publisherId))
            {
                return NotFound("Publisher with such Id cannot be found and updated!");
            }

            return Ok(await _publishersService.Update(publisherId, publisherRequest.ToPublisher()));
        }

        [HttpDelete ("{publisherId}")]
        public async Task<IActionResult> Delete(Guid publisherId)
        {
            _logger.LogInformation("Delete Publisher with {@PublisherId}", publisherId);

            if (!await _publishersService.Contains(publisherId))
            {
                return NotFound("Publisher with such Id is not found!");
            }
            await _publishersService.Delete(publisherId);
            return Ok("Publisher deleted with ID:" + publisherId);
        }

    }
}