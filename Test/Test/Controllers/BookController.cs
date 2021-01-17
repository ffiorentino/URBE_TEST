using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.IServices;
using Test.Models;
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
            return _oBookService.Save(book);
        }

        [HttpPut]
        public Task<ResponseBook> Cancel(int id)
        {
            return _oBookService.Cancel(id);
        }
    }
}
