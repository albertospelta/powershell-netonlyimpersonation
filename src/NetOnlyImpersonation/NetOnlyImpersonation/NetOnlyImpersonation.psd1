@{
	ModuleVersion = '1.0.0'
	GUID = '4716A58C-8609-4E33-8D8E-56C8C695B77C'
	Author = 'Alberto Spelta'
	Copyright = '(c) 2017 Alberto Spelta. All rights reserved.'
	Description = 'This module allows using windows identity impersonation to access to access protected resources on remote systems as a different user without any trust. Credentials are only used on the remote system, locally you are still using the identity of the process.'
	PowerShellVersion = '4.0'
	DotNetFrameworkVersion = '4.0'
	CLRVersion = '4.0'
	ProcessorArchitecture = 'None'
	RequiredAssemblies = @() 
	ScriptsToProcess = @()
	TypesToProcess = @()
	FormatsToProcess = @()
	NestedModules = @("NetOnlyImpersonation.dll")
	FunctionsToExport = '*'
	CmdletsToExport = '*'
	VariablesToExport = '*'
	AliasesToExport = '*'
	PrivateData = @{
		PSData = @{
			ProjectUri = 'https://github.com/albertospelta/azure-automation-netonlyimpersonation'
		}
	}
}
