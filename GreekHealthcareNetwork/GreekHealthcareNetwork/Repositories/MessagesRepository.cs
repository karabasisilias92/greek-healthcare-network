using GreekHealthcareNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Repositories
{
    public class MessagesRepository
    {
        public Message FindById(long id)
        {
            Message message;
            using (ApplicationDbContext db = new ApplicationDbContext())
            { 
                message = db.Messages
                    .Include("Sender")
                    .Include("Recipient")
                    .Include("Sender.Roles")
                    .Include("Recipient.Roles")
                    .SingleOrDefault(i => i.Id == id);
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

        public long NewConversationId()
        {
            long convId;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                convId = db.Messages.Max(m => m.ConversationId);
            }
            return convId;
        }

        public string AdminWithLessVisitorMessagesUnreplied()
        {
            string id = "c0c59e7b-4e90-42bd-922b-ffc699dd6305";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GHNConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("Select RecipientId FROM Messages WHERE MessageStatus = 1 AND Discriminator = 'VisitorMessage' GROUP BY RecipientId, MessageStatus Having COUNT(*) = (SELECT Min(MessagesNumber) FROM (SELECT Count(*) MessagesNumber FROM Messages WHERE MessageStatus = 1 AND Discriminator = 'VisitorMessage' GROUP BY RecipientId, MessageStatus) AS q1)", conn))
                {
                    try
                    {
                        string result = (string)cmd.ExecuteScalar();
                        if(result != null)
                        {
                            id = result;
                        }
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return id;
        }
    }
}