using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class MessagesRepository
    {
        //
        public Message FindById(long id)
        {
            Message message;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {//me single or defaultt prwta ta include k meta ena lamda gia to id tou  
                message= db.Messages.Find(id); 
            }
            return message;
        }
        public void Add(Message message)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }      
        }
        public IEnumerable<Message> GetAllMessages(string UserId)
        {
            IEnumerable<Message> messages;
            if (UserId==null)
            {
                throw new ArgumentNullException("UserId");
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                messages = db.Messages.Where(m => m.RecipientId.Equals(UserId) || m.SenderId.Equals(UserId))
                                  .Include("Sender")
                                  .Include("Sender.Roles")
                                  .Include("Recipient")
                                  .Include("Recipient.Roles")
                                  .ToList();
            }
            return messages;
        }
    }
}