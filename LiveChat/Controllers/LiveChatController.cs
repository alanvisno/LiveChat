using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveChat.Context;
using LiveChat.Hubs;
using LiveChat.Model;
using LiveChat.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LiveChat.Controllers
{
    [Route("api")]
    [ApiController]
    public class LiveChatController : ControllerBase
    {
        //MVC with services layer
        LiveChatServices _context = LiveChatServices.Instance;


        //Added an initialization to get the "security code" as a temporal token
        [HttpGet("access")]
        public ActionResult<bool> Initialize(AccessModel model)
        {
            //Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string response = _context.GetAccess(model);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound("User not found");
            }
        }

        [HttpGet("init")]
        public async Task<ActionResult<bool>> InitializeChat(MessageModel model)
        {
            //Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool response = await _context.CheckUser(model);
            if (response == true)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpGet("contacts")]
        public ActionResult ContactList(ContactModel model)
        {
            //Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var list = _context.GetContacts(model);
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return NotFound("User not found");
            }
        }

        [HttpGet("messages")]
        public ActionResult MessageList(MessageModel model)
        {
            //Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var list = _context.GetMessages(model);
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return NotFound("User not found");
            }
        }

        //Added a close method to delete the token and stop the flow.
        [HttpGet("close")]
        public ActionResult<bool> Close(CloseModel model)
        {
            //Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool response = _context.CloseAccess(model);
            if (response == true)
            {
                return Ok(response);
            }
            else
            {
                return NotFound("User not found");
            }
        }
    }
}
