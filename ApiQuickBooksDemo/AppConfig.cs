using System;
using System.Configuration;
using System.Data;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ApiQuickBooksDemo
{
    public class AppConfig : IDisposable
    {
        #region FIELDS
        private static AppConfig _instance;
        private static string _connectionString;
        private static IDbConnectionFactory _dbFactory;
        private IDbConnection _db;



        #endregion

        #region CTOR
        private AppConfig()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            _instance = null;
            _dbFactory = new OrmLiteConnectionFactory(_connectionString, SqlServer2014Dialect.Provider);
        }
        #endregion


        #region GETTERS AND SETTERS
        public static AppConfig Instance()
        {
            if (_instance == null)
                _instance = new AppConfig();

            return _instance;
        }
        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
            }
        }
        public IDbConnectionFactory DbFactory { get { return _dbFactory; } }




        #endregion

        #region PUBLIC METHODS
        public void Dispose()
        {
            //_instance = null;
            //_connectionString = null;
            //_dbFactory = null;
        }

        #endregion



    }
}
