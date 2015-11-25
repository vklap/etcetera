using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etcetera.Auth
{
    public interface IEtcdAuthModule
    {
        void EnableAuth();
        void DisableAuth();
        EtcdAuthStatusResponse GetAuthStatus();

        EtcdGetUsersResponse GetUsers();
        EtcdGetUserDetailsResponse GetUser(string user);
        EtcdSetUserResponse SetUser(EtcdSetUserRequest request);
        void RemoveUser(string user);

        EtcdGetRolesResponse GetRoles();
        EtcdGetRoleDetailsResponse GetRoleDetails(string role);
        EtcdSetRoleResponse SetRole(EtcdSetRoleRequest request);
        void RemoveRole(string role);
    }
}
