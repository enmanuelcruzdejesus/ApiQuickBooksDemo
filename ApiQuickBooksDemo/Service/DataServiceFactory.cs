////*********************************************************
// <copyright company="Intuit">
// Author:Nimisha
//
////*********************************************************
using Intuit.Ipp.Core;
using Intuit.Ipp.DataService;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Webhooks.Models.DTO;

namespace Webhooks.Models.Service
{
    public class DataServiceFactory
    {
        private Intuit.Ipp.Security.OAuth2RequestValidator oAuthRequestValidator = null;
        private DataService _dataService = null;
        IntuitServicesType intuitServicesType = new IntuitServicesType();
        private ServiceContext _serviceContext = null;
        public ServiceContext getServiceContext { get; set; }
        /// <summary>
        /// Allocate memory for service context objects
        /// </summary>
        /// 
        public static string code;
        public static string realmId;
        public static TokenResponse Token = null;

        static string clientid = ConfigurationManager.AppSettings["clientid"];
        static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        static string environment = ConfigurationManager.AppSettings["appEnvironment"];


        //public static ServiceContext ServiceContext = null;
        public static OAuth2Client auth2Client = new OAuth2Client(clientid, clientsecret, redirectUrl, environment);


        public DataServiceFactory(OAuthTokensRealmLastUpdateddto oAuthorization)
        {
            try
            {


                if(Token != null)
                {
                    // var principal = User as ClaimsPrincipal;
                    //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(Token.AccessToken);

                    // Create a ServiceContext with Auth tokens and realmId
                    _serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                    _serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                    _serviceContext.IppConfiguration.Logger.RequestLog.EnableRequestResponseLogging = true;
                    _serviceContext.IppConfiguration.Logger.RequestLog.ServiceRequestLoggingLocation = ConfigurationManager.AppSettings["ServiceRequestLoggingLocation"];
                    getServiceContext = _serviceContext;
                    _dataService = new DataService(_serviceContext);





                }



                //oAuthRequestValidator = new OAuth2RequestValidator(
                //        oAuthorization.OAuthTokens.access_token, 
                //        oAuthorization.OAuthTokens.access_secret, 
                //        oAuthorization.ConsumerKey, 
                //        oAuthorization.ConsumerSecret);


                //    intuitServicesType = oAuthorization.OAuthTokens.datasource == "QBO" ? IntuitServicesType.QBO : IntuitServicesType.None;
                //    serviceContext = new ServiceContext(oAuthorization.OAuthTokens.realmid.ToString(), intuitServicesType, oAuthRequestValidator);
                //    serviceContext.IppConfiguration.BaseUrl.Qbo = ConfigurationManager.AppSettings["ServiceContext.BaseUrl.Qbo"];
                //    //serviceContext.IppConfiguration.Logger.RequestLog.EnableRequestResponseLogging = true;
                //    //serviceContext.IppConfiguration.Logger.RequestLog.ServiceRequestLoggingLocation = ConfigurationManager.AppSettings["ServiceRequestLoggingLocation"];
                //    getServiceContext = serviceContext;
                //    dataService = new DataService(serviceContext);


            }
            catch (Intuit.Ipp.Exception.FaultException ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Return the current data service
        /// </summary>
        /// 





        public DataService getDataService()
        {
            return _dataService;
        }
    }
}