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
    public class RoomController : ControllerBase
    {
        private IRoomService _oRoomService;

        public RoomController(IRoomService oRoomService)
        {
            _oRoomService = oRoomService;
        }

        [HttpPost]
        public Task<ResponseRoom> Create(Room room)
        {
            return _oRoomService.Save(room);
        }

        [HttpPut]
        public Task<ResponseRoom> Update(Room room)
        {
            return _oRoomService.Update(room);
        }

        [HttpDelete]
        public Task<ResponseRoom> Delete(int id)
        {
            return _oRoomService.Delete(id);
        }

    }
}
