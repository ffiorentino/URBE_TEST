using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Models;
using Test.Models.DTO;
using Test.Models.Request;
using Test.Models.Response;

namespace Test.IServices
{
    public interface IBookService
    {
        Task<ResponseBook> Save(Book oBook);
        Task<ResponseBook> Cancel(int id);
        Task<IEnumerable<ReportDTO>> GetReport(ReportRequest oRequest);
    }
}
