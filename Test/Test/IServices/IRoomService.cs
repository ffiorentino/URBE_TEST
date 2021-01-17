using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Models;
using Test.Models.Response;

namespace Test.IServices
{
    public interface IRoomService
    {
        Task<ResponseRoom> Save(Room oRoom);
        Task<IEnumerable<Room>> GetAll(Room oRoom);
        Task<Room> Get(int id);
        Task<ResponseRoom> Delete(int id);
        Task<ResponseRoom> Update(Room oRoom);
    }
}
