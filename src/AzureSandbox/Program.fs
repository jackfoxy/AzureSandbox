namespace AzureSandbox

open Microsoft.Azure.Management.ResourceManager.Fluent
open Microsoft.Azure.Management.Fluent
open System
open System.Configuration


//type AppSettings = FSharp.Configuration.AppSettings<"App.config">
//should work in conjunction w/ FSharp.Configuration, test later

//https://docs.microsoft.com/en-us/dotnet/azure/dotnet-sdk-azure-authenticate?view=azure-dotnet#mgmt-auth
module console1 =
    let ClientId =  ConfigurationManager.AppSettings.["ClientId"]
    let ServicePrincipalPassword = ConfigurationManager.AppSettings.["ServicePrincipalPassword"]
    let AzureTenantId = ConfigurationManager.AppSettings.["AzureTenantId"]
    let AzureSubscriptionId =  ConfigurationManager.AppSettings.["AzureSubscriptionId"]

    [<EntryPoint>]
    let main argv = 
        printfn "%A" argv

        let azureCredentials =
            let servicePrincipalLoginInformation = Authentication.ServicePrincipalLoginInformation()
            servicePrincipalLoginInformation.ClientId <- ClientId
            servicePrincipalLoginInformation.ClientSecret <- ServicePrincipalPassword
            Authentication.AzureCredentials(servicePrincipalLoginInformation, AzureTenantId, AzureEnvironment.AzureGlobalCloud)

        let azure = Azure.Configure().Authenticate(azureCredentials).WithSubscription(AzureSubscriptionId)

        azure.ResourceGroups.List()
        |> Seq.iter (fun x -> printfn "%s" x.Name)

        printfn "Hit any key to exit."
        System.Console.ReadKey() |> ignore
        0