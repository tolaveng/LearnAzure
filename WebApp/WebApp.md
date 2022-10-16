# WebApp Deployment
## App service plan
	- shared, dedicated, isolated
	
## Automated deployment
	- Github, Bitbucket
	- Azure DevOps
	
## Manual deployment
	- Git, Zip, FTP
	- CLI: az webapp up
	
## Built-in authentication
	- Identity providers: /.auth/login/facebook
	- HTTP request -> authentication (validate, session, token, inject identity in header -> code
	- runs separately in the same sandbox, no SDK or changes to the code
	- Authentication flow:
		- without provider SDK: browser app, server-directed flow or server flow
		- with provider SDK: brower less, client-directed flow or client flow. REST API, native mobile app
	- Authorization:
		- Allow unauthenticated
		- Require authenticated
		
## Netwwork
	- default: accessible from the internet
	- multitenant: free, shared, basic, standard, premium
	- sing-tenant: ASE isolated SKU
	- outbound addrss ip in Properties
	> az webapp show --resource-group <group_name> --name <app_name> --query outboundIpAddresses[,possibleOutboundIpAddresses] --output tsv
	-Inbound and Outbound features
	- App-assigned address <-> Hybrid Connections (can be used to control outbound network traffic)
	- Access restrictions <-> Gateway-requred virtual network intergration
	- Service endpoint <-> Virtual network intergration
	- Private endpoint <-> 
	
# Configure app setting
	- Settings -> Configuration -> Application settings
	- app setting passed as environment variables
	- container using --env flag
	- it will override the Web.config and appsettings.json
	- swap in deployment slot
	- JSON key structure naming like, app:key, needs to be configured as app__key
	 (: should be replaced by __)
	```
	[
	  {
		"name": "<key-1>",
		"value": "<value-1>",
		"slotSetting": false
	  },
	  {
		"name": "<key-2>",
		"value": "<value-2>",
		"type": "SQLServer", // connection string
		"slotSetting": false
	  },
	]
	```
	- General settings
		Stack settings, Platform, Debugging (auto off 48hours), incoming client certificates
	- Path mapping
	- Diagnostic logging: Application logging, web server, details error, failed request, deployment logging
		Application log in code
		ASP.net System.Diagnostics.Trace.TraceError("If you're seeing this, something bad happened");
		By default, ASP.net core use the Microsoft.Extensions.Logging.AzureAppServices
	- stream logs
		> az webapp log tail --name <appname> --resource-group <resource-group>
	- access the log files
		- Azure storage blobs
		- App service file system,
			Linux/container apps: https://<app-name>.scm.azurewebsites.net/api/logs/docker/zip
			Windows apps: https://<app-name>.scm.azurewebsites.net/api/dump
	- certificates
		resource group and region combination calls webspace, can access to other app in the same combination
		custom SSL does'nt support in F1 and D1 tier
	- App features management
		Feature flag: on/off
		Feature manager: handle the life cycle, caching feature flag or updating the states
		Filter: rule for evaluating the states
		declaration
		```
		"FeatureManagement": {
			"FeatureA": true, // Feature flag set to on
			"FeatureB": false, // Feature flag set to off
			"FeatureC": {
				"EnabledFor": [
					{
						"Name": "Percentage",
						"Parameters": {
							"Value": 50
						}
					}
				]
			}
		}
		```
		In code
		``` if(featureFlag) ... ```
# Auto scaling
	- Combining rule
		scale out if any rule is met
		scale in if all rules are met
	- not support in F1 and D1, B1 manual scaling
	- record in activity log can be notified by email sms
	- avoid flapping metric, auto scale back and forth

# Slot
	- availabe from Standard tier (5 slots)
	- each tier has different support slot (APP service limit), no addtional charge
	- new slot has no content even clone setting from other slot
	- When swap 
		- apply all settings from the target slot: slot specific, connection string,continue deployment setting, auth setting
		- wait for all source slot instances to restart
		- trigger local cache if enabled ...
		- warm up ...
		- swap the two slots by switching the routing rules
		- source slot has the pre-swap app previously in the target slot,
	- To make settings swappable, add the app setting WEBSITE_OVERRIDE_PRESERVE_DEFAULT_STICKY_SLOT_SETTINGS in every slot of the app and set its value to 0 or false
	- Specific warm up applicationInitialization in web.config
	- can route a portion of the traffic to another slot. in Traffic %. Can check in x-ms-routing-name cookie
	- manually route, use x-ms-routing-name query paramete,  <webappname>.azurewebsites.net/?x-ms-routing-name=self
	- By default, new slots are given a routing rule of 0%, This is an advanced scenario where you can "hide" your staging slot from the public

# Usefull commands
	- Find app with 'imgapi*' prefix
	> az webapp list --resource-group ManagedPlatform --query "[?starts_with(name, 'imgapi')]"
	- render only the name of the single app that has the imgapi* prefix
	 az webapp list --resource-group ManagedPlatform --query "[?starts_with(name, 'imgapi')].{Name:name}" --output tsv
	 
	- deploy App
	> az webapp deployment source config-zip --resource-group ManagedPlatform --src api.zip --name <name-of-your-api-app>
	