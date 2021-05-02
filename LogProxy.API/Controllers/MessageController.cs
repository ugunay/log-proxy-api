using LogProxy.API.Entities;
using LogProxy.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogProxy.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> Get()
        {
            var messages = await _messageService.GetAllAsync();
            if (!messages.Any())
            {
                return NotFound();
            }
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> Get(string id)
        {
            var message = await _messageService.GetByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult<Message>> Post(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            message.Id = Guid.NewGuid().ToString();
            message.ReceivedAt = DateTime.Now.ToUniversalTime();
            var recordId = await _messageService.PostAsync(message);
            return CreatedAtAction(nameof(Get), new { id = recordId }, message);
        }
    }
}