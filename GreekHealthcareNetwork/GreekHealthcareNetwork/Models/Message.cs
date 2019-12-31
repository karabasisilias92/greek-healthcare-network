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
        public static int ConversationIdCounter { get; set; } = 1;

        [Key]
        public long Id { get; set; }

        public long ConversationId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string RecipientId { get; set; }

        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime SentDate { get; set; }

        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        public TimeSpan SentTime { get; set; }

        public MessageStatus MessageStatus { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string MessageText { get; set; }

    }
}