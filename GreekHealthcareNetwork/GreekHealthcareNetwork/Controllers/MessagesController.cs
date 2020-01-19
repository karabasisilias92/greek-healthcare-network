using GreekHealthcareNetwork.Models;
using GreekHealthcareNetwork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.WebPages;

namespace GreekHealthcareNetwork.Controllers
{
    [System.Web.Mvc.Authorize]
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

        //
        [System.Web.Http.Route("api/Admin/GetVisitorMessages")]
        public IHttpActionResult GetVisitorMessages(string UserId)
        {
            return Ok(_message.GetAllVisitorMessages(UserId));
        }

        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.AllowAnonymous]
        public IHttpActionResult Post(SearchLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.Message != null)
            {
                model.Message.SentDate = DateTime.Now.Date;
                model.Message.SentTime = DateTime.Now.TimeOfDay;
                model.Message.MessageStatus = MessageStatus.Unread;
                if (model.Message.ConversationId == 0)
                {
                    model.Message.ConversationId = _message.NewConversationId() + 1;
                }

                try
                {
                    _message.Add(model.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error has occured. Please try again.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                model.VisitorMessage.RecipientId = _message.AdminWithLessVisitorMessagesUnreplied();
                model.VisitorMessage.SentDate = DateTime.Now.Date;
                model.VisitorMessage.SentTime = DateTime.Now.TimeOfDay;
                model.VisitorMessage.MessageStatus = MessageStatus.Unread;
                model.VisitorMessage.ConversationId = _message.NewConversationId() + 1;
                try
                {
                    _message.Add(model.VisitorMessage);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error has occured. Please try again.");
                    return BadRequest(ModelState);
                }
            }
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
            //h return Ok();
        }


        [System.Web.Http.HttpPut, System.Web.Http.HttpPatch]
        public IHttpActionResult Update(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _message.UpdateMessage(message);
            return Ok();
        }
    }
}
