using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Models;
using Test.Models.Response;

namespace Test.IServices
{
    public interface IBookService
    {
        Task<ResponseBook> Save(Book oBook);
        Task<ResponseBook> Cancel(int id);
    }
}
