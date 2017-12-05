﻿using Air.ModelRepository;
using Air.Models;
using Air.Properties;
using Air.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air.ModelConnection
{
    public class SqlConnection : IConnection
    {
        private static SqlConnection _instance;
        public DbConnection DbConnection { get; }
        public static SqlConnection Instance => _instance ?? (_instance = new SqlConnection());

        private SqlConnection()
        {
            SqlConnectionStringBuilder stringConnection = new SqlConnectionStringBuilder();
            stringConnection.DataSource = @"(localdb)\MSSQLLocalDB";
            stringConnection.InitialCatalog = "Airport";
            stringConnection.UserID = Settings.Default.Username;
            stringConnection.Password = Settings.Default.Password;

            DbConnection = new System.Data.SqlClient.SqlConnection(stringConnection.ToString());
        }

        public void Open()
        {
            DbConnection.Open();
        }

        public void Close()
        {
            DbConnection.Close();
        }

        public IRepository<AirlineModel> Airlines() => new AirlineRepository();
    }
}
