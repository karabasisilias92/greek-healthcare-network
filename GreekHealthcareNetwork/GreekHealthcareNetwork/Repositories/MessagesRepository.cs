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

        public long CountUnreadMessagesOfUser(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }
            long count;
            using (var db = new ApplicationDbContext())
            {
                count = db.Messages.Count(m => m.RecipientId.Equals(userId) && m.MessageStatus == MessageStatus.Unread && !m.SenderId.Equals(db.Users.FirstOrDefault(u => u.Email.Equals("visitor@ghn.gr")).Id));
            }
            return count;
        }

        public long CountUnreadVisitorMessagesOfUser(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }
            long count;
            using (var db = new ApplicationDbContext())
            {
                count = db.Messages.Count(m => m.RecipientId.Equals(userId) && m.MessageStatus == MessageStatus.Unread && m.SenderId.Equals(db.Users.FirstOrDefault(u => u.Email.Equals("visitor@ghn.gr")).Id));
            }
            return count;
        }

        public void Add(Message message)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }      
        }
        public void UpdateMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Messages.Attach(message);
                db.Entry(message).State = System.Data.Entity.EntityState.Modified;
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
                var visitorId = db.Users.SingleOrDefault(u => u.Email.Equals("visitor@ghn.gr")).Id;
                messages = db.Messages.Where(m => m.RecipientId.Equals(UserId) && !m.SenderId.Equals(visitorId) || m.SenderId.Equals(UserId))
                                  .OrderByDescending(m => m.SentDate)
                                  .ThenByDescending(m => m.SentTime)
                                  .Include("Sender")
                                  .Include("Sender.Roles")
                                  .Include("Recipient")
                                  .Include("Recipient.Roles")
                                  .ToList();
            }
            return messages;
        }

        //
        public IEnumerable<Message> GetAllVisitorMessages(string UserId)
        {
            IEnumerable<Message> messages;
            if (UserId == null)
            {
                throw new ArgumentNullException("UserId");
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var visitorId = db.Users.SingleOrDefault(u => u.Email.Equals("visitor@ghn.gr")).Id;
                messages = db.Messages.Where(m => m.RecipientId.Equals(UserId) && m.SenderId.Equals(visitorId))
                                  .OrderByDescending(m => m.SentDate)
                                  .ThenByDescending(m => m.SentTime)
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

        // Used for "load balancing" visitor messages to administrators, if we have more than one
        public string AdminWithLessVisitorMessagesUnreplied()
        {
            string id = "c0c59e7b-4e90-42bd-922b-ffc699dd6305";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GHNConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP(1) u.Id FROM AspNetUsers u JOIN AspNetUserRoles ur ON u.Id = ur.UserId JOIN AspNetRoles r ON ur.RoleId = r.Id LEFT JOIN Messages m ON u.Id = m.RecipientId WHERE r.Name = 'Administrator' GROUP BY u.Id Having COUNT(case when Discriminator = 'VisitorMessage' AND MessageStatus = 1 then 1 else null end) = (SELECT Min(MessagesNumber) FROM (SELECT COUNT(case when Discriminator = 'VisitorMessage' AND MessageStatus = 1 then 1 else null end) MessagesNumber FROM AspNetUsers u JOIN AspNetUserRoles ur ON u.Id = ur.UserId JOIN AspNetRoles r ON ur.RoleId = r.Id LEFT JOIN Messages m ON u.Id = m.RecipientId WHERE r.Name = 'Administrator' GROUP BY u.Id) AS q1)", conn))
                {
                    try
                    {
                        string result = (string)cmd.ExecuteScalar();
                        if (result != null)
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

        public string AdminWithLessNonVisitorMessagesUnreplied()
        {
            string id = "c0c59e7b-4e90-42bd-922b-ffc699dd6305";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GHNConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP(1) u.Id FROM AspNetUsers u JOIN AspNetUserRoles ur ON u.Id = ur.UserId JOIN AspNetRoles r ON ur.RoleId = r.Id LEFT JOIN Messages m ON u.Id = m.RecipientId WHERE r.Name = 'Administrator' GROUP BY u.Id Having COUNT(case when Discriminator = 'Message' AND MessageStatus = 1 then 1 else null end) = (SELECT Min(MessagesNumber) FROM (SELECT COUNT(case when Discriminator = 'Message' AND MessageStatus = 1 then 1 else null end) MessagesNumber FROM AspNetUsers u JOIN AspNetUserRoles ur ON u.Id = ur.UserId JOIN AspNetRoles r ON ur.RoleId = r.Id LEFT JOIN Messages m ON u.Id = m.RecipientId WHERE r.Name = 'Administrator' GROUP BY u.Id) AS q1)", conn))
                {
                    try
                    {
                        string result = (string)cmd.ExecuteScalar();
                        if (result != null)
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