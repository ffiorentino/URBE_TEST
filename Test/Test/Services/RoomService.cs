using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using Test.Common;
using Test.IServices;
using Test.Models;
using Test.Models.Response;
using System.Threading.Tasks;
using StatusCodeNet = System.Net.HttpStatusCode;

namespace Test.Services
{
    public class RoomService : IRoomService
    {
        //private readonly IDbConnection _connection;

        //public RoomService(string connectionString)
        //{
        //    if (_connection == null)
        //        _connection = new SqlConnection(connectionString);
        //}

        public async Task<ResponseRoom> Delete(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id, DbType.Int32);
     
                    var result = await con.ExecuteAsync("uspDeleteRoom", param, commandType: CommandType.StoredProcedure);

                    return new ResponseRoom { success = true, message = new MessageResponse { code = 1, text = "Se elimino la sala " + id } };
                }
            }
            catch (Exception ex)
            {
                return new ResponseRoom { success = false, message = new MessageResponse { code = 0, text = ex.Message } };
            }
        }

        public async Task<Room> Get(int id)
        {
            Room oRoom = null;
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id, DbType.Int32);

                    var result = await con.QueryAsync<Room>("uspGetRoomById", param, commandType: CommandType.StoredProcedure);

                    oRoom = result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new ApiException { statusCode = StatusCodeNet.InternalServerError, stackTrace = ex.StackTrace };
            }

            return oRoom;
        }

        public async Task<IEnumerable<Room>> GetAll(Room oRoom)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Capacity", oRoom.capacity, DbType.Int32);
                    param.Add("@Projector", oRoom.projector, DbType.Boolean);
                    param.Add("@Blackboard", oRoom.blackboard, DbType.Boolean);
                    param.Add("@Wifi", oRoom.wifi, DbType.Boolean);

                    return await con.QueryAsync<Room>("uspGetRooms", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new ApiException { statusCode = StatusCodeNet.InternalServerError, stackTrace = ex.StackTrace };
            }
        }

        public async Task<ResponseRoom> Save(Room oRoom)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Name", oRoom.name, DbType.String);
                    param.Add("@Capacity", oRoom.capacity, DbType.Int32);
                    param.Add("@Projector", oRoom.projector, DbType.Boolean);
                    param.Add("@Blackboard", oRoom.blackboard, DbType.Boolean);
                    param.Add("@Wifi", oRoom.wifi, DbType.Boolean);

                    int result = await con.ExecuteScalarAsync<int>("uspSaveRoom", param, commandType: CommandType.StoredProcedure);

                    return new ResponseRoom { success = true, message = new MessageResponse { code = 1, text = "La sala fue dada de alta correctamente con el ID " + result } };
                }
            }
            catch(Exception ex)
            {
                return new ResponseRoom { success = false, message = new MessageResponse { code = 0, text = ex.Message } };
            }
        }

        public async Task<ResponseRoom> Update(Room oRoom)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", oRoom.id, DbType.Int32);
                    param.Add("@Name", oRoom.name, DbType.String);
                    param.Add("@Capacity", oRoom.capacity, DbType.Int32);
                    param.Add("@Projector", oRoom.projector, DbType.Boolean);
                    param.Add("@Blackboard", oRoom.blackboard, DbType.Boolean);
                    param.Add("@Wifi", oRoom.wifi, DbType.Boolean);

                    var result = await con.ExecuteAsync("uspUpdateRoom", param, commandType: CommandType.StoredProcedure);

                    return new ResponseRoom { success = true, message = new MessageResponse { code = 1, text = "Se actualizo la sala " + oRoom.id } };
                }
            }
            catch (Exception ex)
            {
                return new ResponseRoom { success = false, message = new MessageResponse { code = 0, text = ex.Message } };
            }
        }
    }
}
