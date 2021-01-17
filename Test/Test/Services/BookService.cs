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
using System.Globalization;

namespace Test.Services
{
    public class BookService : IBookService
    {
        public async Task<ResponseBook> Save(Book oBook)
        {
            try
            {
                DateTime fromDate = DateTime.ParseExact(oBook.fromDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(oBook.toDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@IdRoom", oBook.idRoom, DbType.Int32);
                    param.Add("@attendant", oBook.attendant, DbType.Int32);
                    param.Add("@useProjector", oBook.useProjector, DbType.Boolean);
                    param.Add("@useBlackboard", oBook.useBlackboard, DbType.Boolean);
                    param.Add("@useWifi", oBook.useWifi, DbType.Boolean);
                    param.Add("@fromDate", fromDate, DbType.DateTime);
                    param.Add("@toDate", toDate, DbType.DateTime);
                    param.Add("@state", StateBook.Confirmed, DbType.Int32);

                    int result = await con.ExecuteScalarAsync<int>("uspSaveBook", param, commandType: CommandType.StoredProcedure);

                    return new ResponseBook { success = true, message = new MessageResponse { code = 1, text = "La reserva fue dada de alta correctamente con el ID " + result } };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBook { success = false, message = new MessageResponse { code = 0, text = ex.Message } };
            }
        }

        public async Task<ResponseBook> Cancel(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(Global.ConnectionString))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Id", id, DbType.Int32);
                    param.Add("@State", StateBook.Cancelled, DbType.Int32);

                    var result = await con.ExecuteAsync("uspUpdateStateBook", param, commandType: CommandType.StoredProcedure);

                    return new ResponseBook { success = true, message = new MessageResponse { code = 1, text = "Se cancelo la reserva con ID " + id } };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBook { success = false, message = new MessageResponse { code = 0, text = ex.Message } };
            }
        }

    }
}

