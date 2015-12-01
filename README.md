etcetera
========

.Net client for [etcd](https://github.com/coreos/etcd) - a highly-available key value store for shared configuration and service discovery.

#Getting started

## Initialize

Create the client to work with assuming that you installed etcd on localhost:

```
IEtcdClient client = new EtcdClient(new Uri("http://localhost:4001/v2/keys/"));
```

## Set a key

```
var test = client.Set("test/one", "1"); 
```

## Get a key value
```
var test = client.Get("test/one", true); 
```

## Watch for key changes
```
client.Watch("test/one", FollowUp); 
```

Followup is the callback function:

```
private static void FollowUp(EtcdResponse obj)
{
	// do the thing  
}
```
# Authorization Support

The AuthModule provides a facade to the new [etcd 2.2 security enhancements](https://github.com/coreos/etcd/blob/master/Documentation/auth_api.md). In order to use those new enhancements, you should proceed with the following 3 simple steps:

## 1. Enable new Auth enhancemsnts:
```
AuthModule = new EtcdAuthModule(new Uri("http://etcd:2379/"), "root", "<YOUR-PASSWORD>");
AuthModule.EnableAuth()
```
## 2. Authenticate
```
client.SetBasicAuthentication(<YOUR-USERNAME>, <YOUR-PASSWORD>);
```

## 3. Set Roles and Permissions 

Declare roles with permissions:

```
var permissions = new EtcdPermissions();
permissions.AddWritePermissions("/a/keys/*");
permissions.AddWritePermissions("/a/b/keys/*");
permissions.AddReadPermissions("/a/keys/*");
permissions.AddReadPermissions("/a/b/keys/*");

AuthModule.SetRole(new EtcdSetRoleRequest
            {
                role = <YOUR-ROLE-NAME>,
                permissions = Permissions
            });
```

Create users with relevant roles:

```
 AuthModule.SetUser(new EtcdSetUserRequest
            {
                user = "<YOUR-USERNAME>",
                password = "<YOUR-PASSWORD>",
                roles = new List<string> { <YOUR-ROLE-NAME> }
            });
```

# Other features

To be done...
