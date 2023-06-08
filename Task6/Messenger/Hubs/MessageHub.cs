using Messenger.Data;
using Messenger.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Messenger.Hubs
{
    public class MessageHub : Hub
    {
        private static Dictionary<string,string> Conenctions = new Dictionary<string, string>();
        private readonly AppDbContext _context;

        public MessageHub(AppDbContext context)
        {
            _context = context;
        }
    

 
        public async Task SendPersonal (string sender, string receiever, string message)
        {
            System.Console.WriteLine("I've got message: {0} from {1} was send to {2}",message,sender,receiever);
            if (ReceiverExist(receiever))
            {
                var user = await _context.Users.Where(p => p.Name == receiever).FirstOrDefaultAsync();
                await Clients.Client(user.ConnectionId).SendAsync("ReceiveMessage", sender , message);
            }
            else
            {
                await CreateNewUserAsync(receiever);
            }

            await SaveMessageToBd(sender,receiever,message);


        }
        public string GetConnectionId() => Context.ConnectionId;

        private async Task CreateNewUserAsync(string name)
        {
            await _context.Users.AddAsync(
                new User{
                    Id = Guid.NewGuid(),
                    Name = name, 
                    ConnectionId = "blankId"
                });
            await _context.SaveChangesAsync();
        }

        private  bool ReceiverExist(string receiver) 
            =>  _context.Users.Any(p => p.Name == receiver);

        private async Task SaveMessageToBd(string sender, string receiever, string message)
        {
            var user = await _context.Users.Where(p => p.Name == receiever).FirstOrDefaultAsync();
            var senderBd = await _context.Users.Where(p => p.Name == sender).FirstOrDefaultAsync();
            await _context.Messages.AddAsync(new Message
             {
                SenderId = senderBd.Id,
                ReceiverId = user.Id,
                MessageText = message,
                Date = DateTime.Now
             });
            await _context.SaveChangesAsync();
        }
    }
}