using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class MessagesRepository
    {
        private static readonly List<Message> _message = new List<Message>()
        {
            new Message
            {
                Id = 1,
            },
            new Message
            {
                Id=2
            }

        };

        public Message FindById(int id)
        {
            return _message.SingleOrDefault(i => i.Id == id);
        }
        public void Add(Message message)
        {
            int newId = _message.Max(i => i.Id) + 1;
            message.Id = newId;
            _message.Add(message);
        }
    }
}