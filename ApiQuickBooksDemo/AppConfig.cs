using System;
using System.Configuration;
using ApiQuickBooksDemo.Services;
using Intuit.Ipp.OAuth2PlatformClient;

using ServiceStack.Data;
using ServiceStack.OrmLite;


namespace ApiCore
{
    public class AppConfig : IDisposable
    {
        #region FIELDS
        private static AppConfig _instance;
        private static string _connectionString;
        private static IDbConnectionFactory _dbFactory;
        #endregion


        private Database _db = null;

        public static string code;
        public static string realmId;
        public static TokenResponse Token;

        private static string clientid = ConfigurationManager.AppSettings["clientid"];
        private static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        private static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        private static string environment = ConfigurationManager.AppSettings["appEnvironment"];

        private static OAuth2Client _auth2Client = null;

        public OAuth2Client Auth2Client
        {
            get
            {
                return _auth2Client;
            }
        }
        public Database Db
        {
            get
            {
                if (_db == null)
                    _db = new Database(ConnectionString);

                return _db;
            }
        }





        #region CTOR
        private AppConfig()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _auth2Client = new OAuth2Client(clientid, clientsecret, redirectUrl, environment);
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
            _instance = null;
            _connectionString = null;
            _dbFactory = null;
        }

        #endregion



    }
}
