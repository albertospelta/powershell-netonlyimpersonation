# NetOnlyImpersonation PowerShell module

A PowerShell module that allows using windows integrated authentication through identity impersonation to access protected resources on remote systems as a different user without any trust.

## Example - Azure automation runbook

Connecting from Azure automation runbook to SQLSever VM using windows integrated security

```PowerShell
workflow Main-Workflow
{
    InlineScript
    {
        Import-Module -Name NetOnlyImpersonation

        $credential = Get-AutomationPSCredential -Name 'my-automation-credential';

        $context = New-Object NetOnlyImpersonation.NetOnlyImpersonationContext(".", $credential.UserName, $credential.GetNetworkCredential().Password);
        try
        {
            $connection = New-Object System.Data.SqlClient.SqlConnection("Server=tcp:sqlserver-vm.cloudapp.net,1433;Integrated Security=True;");
            try
            {
                $connection.Open();

                $command = New-Object System.Data.SqlClient.SqlCommand("SELECT ORIGINAL_LOGIN();", $connection);  
                $message = $command.ExecuteScalar();
                $message
            }
            finally
            {
                $connection.Dispose();        
            }
        }
        finally
        {
            $context.Dispose();
        }
    }
}

$ErrorActionPreference = "Stop"
Write-Output "Start"
Main-Workflow
Write-Output "Stop"
```

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/albertospelta/azure-automation-netonlyimpersonation/tags). 

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
