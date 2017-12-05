using Air.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Air.ModelRepository
{
    public class PlaneRepository : IRepository<PlaneModel>
    {
        private static SqlConnection _connection;

        private readonly SqlTransaction _transaction;

        public PlaneRepository(SqlTransaction transaction)
        {
            _connection = ModelConnection.SqlConnection.Instance.DbConnection as SqlConnection;
            _transaction = transaction;
        }

        public PlaneModel CreateModel(SqlDataReader reader)
        {
            return new PlaneModel
            {
                PlaneID = Convert.ToInt32(reader["PlaneID"]),
                AirlineID = Convert.ToInt32(reader["AirlineID"]),
                AirplaneModel = reader["AirplaneModel"].ToString(),
                OnboardNumber = reader["OnboardNumber"].ToString()
            };
        }

        public bool Create(PlaneModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneCreate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@AirlineID", item.AirlineID),
                new SqlParameter("@AirplaneModel", item.AirplaneModel),
                new SqlParameter("@OnboardNumber", item.OnboardNumber)
            });
            return command.ExecuteNonQuery() == 1;
        }

        public bool Delete(PlaneModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneDelete";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@PlaneID", item.PlaneID));

            return command.ExecuteNonQuery() == 1;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public PlaneModel Select(PlaneModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneSelect";
            command.Transaction = _transaction;
            command.Parameters.Add(new SqlParameter("@PlaneID", item.PlaneID));

            SqlDataReader reader = command.ExecuteReader();

            return CreateModel(reader);
        }

        public IEnumerable<PlaneModel> SelectList()
        {
            List<PlaneModel> planes = new List<PlaneModel>();

            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneSelectList";
            command.Transaction = _transaction;

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    planes.Add(CreateModel(reader));
                }
            }
            reader.Close();

            return planes;
        }

        public bool Update(PlaneModel item)
        {
            SqlCommand command = _connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PlaneUpdate";
            command.Transaction = _transaction;
            command.Parameters.AddRange(new[]
            {
                new SqlParameter("@PlaneID", item.PlaneID),
                new SqlParameter("@AirlineID", item.AirlineID),
                new SqlParameter("@AirplaneModel", item.AirplaneModel),
                new SqlParameter("@OnboardNumber", item.OnboardNumber)
            });
            return command.ExecuteNonQuery() == 1;
        }
    }
}
