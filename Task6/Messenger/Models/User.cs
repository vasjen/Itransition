using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
    
        [InverseProperty("Sender")]
        public ICollection<Message> SentMessages { get; set; }
    
        [InverseProperty("Receiver")]
        public ICollection<Message> ReceivedMessages { get; set; }
    }
}

