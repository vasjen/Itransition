using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.Models
{
    public class Message
    {
        public Guid Id { get; set; }
    
        [ForeignKey("SenderId")]
        public User Sender { get; set; }
        public Guid SenderId { get; set; }
    
        [ForeignKey("ReceiverId")]
        public User Receiver { get; set; }
        public Guid ReceiverId { get; set; }
    
        public string MessageText { get; set; }
        public DateTime Date { get; set; }
    }
}

