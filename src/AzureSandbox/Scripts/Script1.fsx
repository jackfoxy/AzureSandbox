#load "load-project-debug.fsx"

open Microsoft.Azure.Management.ResourceManager.Fluent
open Microsoft.Azure.Management.Fluent

//https://docs.microsoft.com/en-us/dotnet/azure/dotnet-sdk-azure-authenticate?view=azure-dotnet#mgmt-auth
//https://docs.microsoft.com/en-in/azure/active-directory/role-based-access-control-configure
let ClientId = ""  //"<Service Principal Application ID>"
let ServicePrincipalPassword = ""  //"<Service Principal Password>"
let AzureTenantId = ""  //"<tenant ID goes here>"
let AzureSubscriptionId = ""  //"<azure subscription ID goes here>"

let azureCredentials =
    let servicePrincipalLoginInformation = Authentication.ServicePrincipalLoginInformation()
    servicePrincipalLoginInformation.ClientId <- ClientId
    servicePrincipalLoginInformation.ClientSecret <- ServicePrincipalPassword
    Authentication.AzureCredentials(servicePrincipalLoginInformation, AzureTenantId, AzureEnvironment.AzureGlobalCloud)

let azure = Azure.Configure().Authenticate(azureCredentials).WithSubscription(AzureSubscriptionId)

let resourceGroups = azure.ResourceGroups.List() 

resourceGroups
|> Seq.iter (fun x -> printfn "%s" x.Name)