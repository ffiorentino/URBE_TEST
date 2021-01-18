using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.IServices;
using Test.Models;
using Test.Models.DTO;
using Test.Models.Request;
using Test.Models.Response;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService _oBookService;

        public BookController(IBookService oBookService)
        {
            _oBookService = oBookService;
        }

        [HttpPost]
        public Task<ResponseBook> Create(Book book)
        {
            try
            {
                return _oBookService.Save(book);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        public Task<ResponseBook> Cancel(int id)
        {
            try
            {
                return _oBookService.Cancel(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public Task<IEnumerable<ReportDTO>> GetReport(ReportRequest oRequest)
        {
            try
            {
                return _oBookService.GetReport(oRequest);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
