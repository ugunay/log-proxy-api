using LogProxy.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogProxy.API.Interfaces
{
    public interface IMessageService
    {
        Task<Message> GetByIdAsync(string id);

        Task<IEnumerable<Message>> GetAllAsync();

        Task<string> PostAsync(Message message);
    }
}