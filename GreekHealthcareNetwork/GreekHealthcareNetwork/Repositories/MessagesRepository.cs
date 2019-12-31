using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class MessagesRepository
    {
        public Message FindById(int id)
        {
            Message message;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
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
            if (UserId==null)
            {
                throw new ArgumentNullException("UserId");
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Messages.Where(m => m.RecipientId == UserId)
                                  .Include("Sender")
                                  .ToList();
            } 
        }
    }
}