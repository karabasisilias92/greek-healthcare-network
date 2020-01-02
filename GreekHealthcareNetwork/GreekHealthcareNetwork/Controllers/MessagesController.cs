using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace GreekHealthcareNetwork.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly MessagesRepository _message = new MessagesRepository();
        [System.Web.Http.HttpGet]
        public IHttpActionResult Get(long id)
        {
            Message message = _message.FindById(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }
        [System.Web.Http.HttpGet]
        public IHttpActionResult Get(string UserId)
        {
            return Ok(_message.GetAllMessages(UserId));
        }
        
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.AllowAnonymous]
        public IHttpActionResult Post(SearchLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //implementation for return model to Modal pending
                return BadRequest(ModelState);
            }
            model.Message.SentDate = DateTime.Now.Date;
            model.Message.SentTime = DateTime.Now.TimeOfDay;
            model.Message.MessageStatus = MessageStatus.Unread;
            model.Message.ConversationId = Message.ConversationIdCounter++;
            try
            {
                _message.Add(model.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error has occured. Please try again.");
                return BadRequest(ModelState);
            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
            //h return Ok();
        }
    }
}
