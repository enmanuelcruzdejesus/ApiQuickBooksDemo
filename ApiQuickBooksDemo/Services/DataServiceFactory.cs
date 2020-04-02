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

namespace Webhooks.Models.Services
{
    public class DataServiceFactory
    {
        private Intuit.Ipp.Security.OAuth2RequestValidator oAuthRequestValidator = null;
        private OAuthTokens _token = null;
        private DataService _dataService = null;



        IntuitServicesType intuitServicesType = new IntuitServicesType();
        private ServiceContext _serviceContext = null;
        public ServiceContext getServiceContext { get; set; }
        /// <summary>
        /// Allocate memory for service context objects
        /// </summary>
        /// 




        public DataServiceFactory(OAuthTokensRealmLastUpdateddto oAuthorization)
        {
            try
            {

                if (oAuthorization.OAuthTokens != null)
                {
                    _token = oAuthorization.OAuthTokens;
                    // var principal = User as ClaimsPrincipal;
                    //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(oAuthorization.OAuthTokens.access_token);

                    // Create a ServiceContext with Auth tokens and realmId
                    _serviceContext = new ServiceContext(oAuthorization.OAuthTokens.realmid, IntuitServicesType.QBO, oauthValidator);
                    _serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                    _serviceContext.IppConfiguration.Logger.RequestLog.EnableRequestResponseLogging = true;
                    //_serviceContext.IppConfiguration.Logger.RequestLog.ServiceRequestLoggingLocation = ConfigurationManager.AppSettings["ServiceRequestLoggingLocation"];
                    getServiceContext = _serviceContext;
                    _dataService = new DataService(_serviceContext);



                }





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

        public DataServiceFactory (OAuthTokens token)
        {
            if (token != null)
            {
                _token = token;
                // var principal = User as ClaimsPrincipal;
                //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(_token.access_token);


                // Create a ServiceContext with Auth tokens and realmId
                _serviceContext = new ServiceContext(token.realmid, IntuitServicesType.QBO, oauthValidator);
                _serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                _serviceContext.IppConfiguration.Logger.RequestLog.EnableRequestResponseLogging = true;
                //_serviceContext.IppConfiguration.Logger.RequestLog.ServiceRequestLoggingLocation = ConfigurationManager.AppSettings["ServiceRequestLoggingLocation"];
                getServiceContext = _serviceContext;
                _dataService = new DataService(_serviceContext);



            }



        }



        public OAuthTokens Token
        {
            get { return _token; }
            set
            {
                if (value != null)
                {
                    _token = value;
                    UpdateToken(value);
                }

            }
        }

        private void UpdateToken(OAuthTokensRealmLastUpdateddto token)
        {
            if (token.OAuthTokens != null)
            {
                // var principal = User as ClaimsPrincipal;
                //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(token.OAuthTokens.access_token);

                // Create a ServiceContext with Auth tokens and realmId
                _serviceContext = new ServiceContext(token.realmId, IntuitServicesType.QBO, oauthValidator);
                _serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                _serviceContext.IppConfiguration.Logger.RequestLog.EnableRequestResponseLogging = true;
                //_serviceContext.IppConfiguration.Logger.RequestLog.ServiceRequestLoggingLocation = ConfigurationManager.AppSettings["ServiceRequestLoggingLocation"];
                getServiceContext = _serviceContext;
                _dataService = new DataService(_serviceContext);



            }
        }

        private void UpdateToken(OAuthTokens token)
        {
            if (token != null)
            {
                // var principal = User as ClaimsPrincipal;
                //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(token.access_token);

                // Create a ServiceContext with Auth tokens and realmId
                _serviceContext = new ServiceContext(token.realmid, IntuitServicesType.QBO, oauthValidator);
                _serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                _serviceContext.IppConfiguration.Logger.RequestLog.EnableRequestResponseLogging = true;
                //_serviceContext.IppConfiguration.Logger.RequestLog.ServiceRequestLoggingLocation = ConfigurationManager.AppSettings["ServiceRequestLoggingLocation"];
                getServiceContext = _serviceContext;
                _dataService = new DataService(_serviceContext);



            }
        }

        public DataService getDataService()
        {
            return _dataService;
        }
    }
}