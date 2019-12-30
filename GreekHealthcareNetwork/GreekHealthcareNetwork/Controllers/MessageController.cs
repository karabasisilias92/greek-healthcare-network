using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace GreekHealthcareNetwork.Controllers
{
    public class MessageController : ApiController
    {
        private readonly MessagesRepository _message = new MessagesRepository();
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return Ok(_message.FindById(id));
        }
        [HttpPost]
        public IHttpActionResult Post(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _message.Add(message);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
            //h return Ok();
        }
    }
}
