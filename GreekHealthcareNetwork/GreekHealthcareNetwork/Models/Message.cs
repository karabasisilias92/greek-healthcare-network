using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public int ConversationId { get; set; }

        public string SenderId { get; set; }

        public string RecipientId { get; set; }

        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }

        [DataType(DataType.Date)]
        public DateTime SentDate { get; set; }

        [DataType(DataType.Time)]
        public DateTime SentTime { get; set; }

        public MessageStatus MessageStatus { get; set; }
    }
}