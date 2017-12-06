using Air.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelRepository
{
    public class CityRepository : IRepository<CityModel>
    {
        private static SqlConnection _connection;

        private readonly SqlTransaction _transaction;

        public CityRepository(SqlTransaction transaction)
        {
            _connection = ModelConnection.SqlConnection.Instance.DbConnection as SqlConnection;
            _transaction = transaction;
        }

        public CityModel CreateModel(SqlDataReader reader)
        {
            return new CityModel
            {
                CityID = Convert.ToInt32(reader["CityID"]),
                CountryID = Convert.ToInt32(reader["CountryID"]),
                CityName = reader["CityName"].ToString(),
                CountryName = reader["CountryName"].ToString(),
                Population = Convert.ToInt64(reader["Population"]),
                GMT = Convert.ToDateTime(reader["GMT"]),
                SignGMT = reader["SignGMT"].ToString()
            };
        }

        public bool Create(CityModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CityCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@CountryID", item.CountryID),
                new SqlParameter("@CityName", item.CityName),
                new SqlParameter("@Population", item.Population),
                new SqlParameter("@GMT", item.GMT),
                new SqlParameter("@SignGMT", item.SignGMT)
            });
            return command.ExecuteNonQuery() == 1;
        }

        public bool Delete(CityModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CityDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CityID", item.CityID));

            return command.ExecuteNonQuery() == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public CityModel Select(CityModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CitySelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CityID", item.CityID));

            SqlDataReader reader = command.ExecuteReader();

            return CreateModel(reader);
        }

        public async Task<IEnumerable<CityModel>> SelectListAsync()
        {
            List<CityModel> cities = new List<CityModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CitySelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    cities.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return cities;
        }

        public IEnumerable<CityModel> SelectListByCountry(CityModel item)
        {
            List<CityModel> cities = new List<CityModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CitySelectListByCountry";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@CountryID", item.CountryID));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cities.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return cities;
        }

        public async Task<bool> UpdateAsync(CityModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "CityUpdate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@CityID", item.CityID),
                new SqlParameter("@CountryID", item.CountryID),
                new SqlParameter("@CityName", item.CityName),
                new SqlParameter("@Population", item.Population),
                new SqlParameter("@GMT", item.GMT),
                new SqlParameter("@SignGMT", item.SignGMT)
            });
            int x = await command.ExecuteNonQueryAsync();
            return x == 1;
        }
    }
}
