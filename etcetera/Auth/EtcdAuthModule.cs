using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace etcetera.Auth
{
    public class EtcdAuthModule : IEtcdAuthModule
    {
        private const string AUTH_RESOURCE = "/v2/auth/enable";
        private const string USERS_RESOURCE = "/v2/auth/users";
        private const string ROLES_RESOURCE = "/v2/auth/roles";

        private readonly IRestClient _client;

        public EtcdAuthModule(Uri etcdLocation, string username, string password)
        {
            if (etcdLocation == null)
            {
                throw new ArgumentNullException("etcdLocation");
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }


            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            _client = new RestClient(etcdLocation);
            _client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        public void EnableAuth()
        {
            var request = new RestRequest(AUTH_RESOURCE);
            
            ExecuteRequest(request, Method.PUT);
        }

        public void DisableAuth()
        {
            var request = new RestRequest(AUTH_RESOURCE);
            
            ExecuteRequest(request, Method.DELETE);
        }

        public EtcdAuthStatusResponse GetAuthStatus()
        {
            var request = new RestRequest(AUTH_RESOURCE);
            
            var response = ExecuteRequest<EtcdAuthStatusResponse>(request, Method.GET);
            return response;
        }

        public EtcdGetUsersResponse GetUsers()
        {
            var request = new RestRequest(USERS_RESOURCE);

            var response = ExecuteRequest<EtcdGetUsersResponse>(request, Method.GET);

            return response;
        }

        public EtcdGetUserDetailsResponse GetUser(string user)
        {
            if (string.IsNullOrEmpty("user"))
            {
                throw new ArgumentNullException("user");
            }

            var request = new RestRequest(string.Concat(USERS_RESOURCE, "/{user}"));
            request.AddParameter("user", user);

            var response = ExecuteRequest<EtcdGetUserDetailsResponse>(request, Method.GET);

            return response;
        }

        public EtcdSetUserResponse SetUser(EtcdSetUserRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrEmpty(request.user))
            {
                throw new ArgumentNullException("request.user");
            }

            var restRequest = new RestRequest(string.Concat(USERS_RESOURCE, "/{user}"));
            restRequest.AddParameter("user", request.user);
            restRequest.AddJsonBody(request);
            

            var response = ExecuteRequest<EtcdSetUserResponse>(restRequest, Method.PUT);
            return response;
        }

        public void RemoveUser(string user)
        {
            if (string.IsNullOrEmpty("user"))
            {
                throw new ArgumentNullException("user");
            }

            var restRequest = new RestRequest(string.Concat(USERS_RESOURCE, "/{user}"));
            restRequest.AddParameter("user", user);

            ExecuteRequest(restRequest, Method.DELETE);
        }

        public EtcdGetRolesResponse GetRoles()
        {
           var request = new RestRequest(ROLES_RESOURCE);

           var response = ExecuteRequest<EtcdGetRolesResponse>(request, Method.GET);

           return response;
        }

        public EtcdGetRoleDetailsResponse GetRoleDetails(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            var restRequest = new RestRequest(string.Concat(ROLES_RESOURCE, "/{role}"));
            restRequest.AddParameter("role", role);    
            var response = ExecuteRequest<EtcdGetRoleDetailsResponse>(restRequest, Method.GET);
            return response;
        }

        public EtcdSetRoleResponse SetRole(EtcdSetRoleRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrEmpty(request.role))
            {
                throw new ArgumentNullException("request.role");
            }

            var restRequest = new RestRequest(string.Concat(ROLES_RESOURCE, "/{role}"));
            restRequest.AddParameter("role", request.role);
            restRequest.AddJsonBody(request);
            var response = ExecuteRequest<EtcdSetRoleResponse>(restRequest, Method.PUT);
            return response;
        }

        public void RemoveRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            var restRequest = new RestRequest(string.Concat(ROLES_RESOURCE, "/{role}"));
            restRequest.AddParameter("role", role);

            ExecuteRequest(restRequest, Method.DELETE);
        }

        private TResponse ExecuteRequest<TResponse>(IRestRequest request, Method method) where TResponse : new()
        {
            request.Method = method;
            
            var response = _client.Execute<TResponse>(request);
            request.RequestFormat = DataFormat.Json;
            HandleResponse(request);

            return response.Data;
        }

        private void ExecuteRequest(IRestRequest request, Method method)
        {
            request.Method = method;
            request.RequestFormat = DataFormat.Json;
            HandleResponse(request);
            
            _client.Execute(request);
        }

        private void HandleResponse(IRestRequest request)
        {
            request.OnBeforeDeserialization = response =>
            {
                var statusCode = (int)response.StatusCode;
                if (statusCode < 400) return;

                var errorMessage = response.Content;
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<EtcdError>(errorMessage);
                    errorMessage = errorObj.message;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
                var sb = new StringBuilder();
                sb.AppendFormat("Failed to execute request to '{0}' with method '{1}'", request.Resource, request.Method);
                if (request.Parameters.Count > 0)
                {
                    sb.Append(" (paramter values: ");
                    request.Parameters.ForEach(parameter =>
                    {
                        string value = null;
                        if (parameter.Value != null)
                        {
                            value = parameter.Value as string ?? JsonConvert.SerializeObject(parameter.Value);
                        }
                        sb.AppendFormat("{0}={1}", parameter.Name, value);
                    });
                    sb.Append(")");
                }
                sb.Append(": ");
                sb.Append(errorMessage);
                throw new EtceteraException(sb.ToString());
            };
        }
    }
}
