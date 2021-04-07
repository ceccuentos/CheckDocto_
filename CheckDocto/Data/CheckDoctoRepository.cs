
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using CheckDocto.Models;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheckDocto.Data
{

    public class CheckDoctoRepository
    {

        private readonly string _connectionString;

        public CheckDoctoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }


        public async Task<List<KeyDocto>> Check(double Id)
        {
            using (SqlConnection connection = new SqlConnection(
                       _connectionString))
            {
                SqlCommand command = new SqlCommand("CheckDocto", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                //command.Parameters.Add("@Empresa", Empresa);
                //command.Parameters.Add("@TipoDocto", Tipodocto);
                command.Parameters.Add(new SqlParameter("@Correlativo", Id));
                var response = new List<KeyDocto>();

                await connection.OpenAsync();

                using(var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue(reader));
                    }
                }

                
                return response;
            }

        }

        public async Task<List<KeyDocto>> CheckDocumento(KeyDocto key)
        {
            using (SqlConnection connection = new SqlConnection(
                       _connectionString))
            {
                SqlCommand command = new SqlCommand("CheckDocto", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Empresa", key.Empresa));
                command.Parameters.Add(new SqlParameter("@TipoDocto", key.Tipodocto));
                command.Parameters.Add(new SqlParameter("@Correlativo", key.Correlativo));
                var response = new List<KeyDocto>();

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue(reader));
                    }
                }


                return response;
            }

        }

        private KeyDocto MapToValue(SqlDataReader reader)
        {
            return new KeyDocto()
            {
                Empresa = reader["Empresa"].ToString(),
                Tipodocto = reader["Tipodocto"].ToString(),
                Correlativo = (decimal)reader["Correlativo"],
                Msg = reader["Msg"].ToString()
                };
        }
    }
}
