using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        public long Id { get; set; }

        public long ConversationId { get; set; }

        [Required]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public virtual ApplicationUser Recipient { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Sent Date")]
        public DateTime SentDate { get; set; }

        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        [Display(Name = "Sent Time")]
        public TimeSpan SentTime { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [Display(Name = "Message Status")]
        public MessageStatus MessageStatus { get; set; }

        [Required]
        public string Subject { get; set; }

        [Display(Name = "Message Text")]
        [Required]
        public string MessageText { get; set; }

    }
}