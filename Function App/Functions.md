# Function
	- function.json, a config file, one trigger,  pass data into and return data
	for compiled languages: auto generated. for scripting: must provide a config file
	- binding: type (queueTrigger, httpTrigger), direction (in, out), name. dataType(binary, string)
	- function app, organise or collective of functions, managed, deployed and scale together. same languages
	- host.json is runtime specific config in root folder of the function app
	- bin contains packages and library files
	
	- Identity-based connections are not supported with Durable Function
	
# Local dev VScode
	- install Azure functions core tools, https://github.com/Azure/azure-functions-core-tools#installing
	- Azure functions extension

# Durable Function
	- Create Durable function HTTP starter (execute orchestrator fun)
	-> Durable fun orchestrator (calls activities funs) -> Durab fun activity
	
	- Orchestrator, ordinary, deterministic
	- Activity function, unit of work, DurableActivityContext param
	- Entity functions define operations for reading and updating small pieces of state, entity trigger
		Entities are accessed via a unique identifier, Entity ID, Operation name
	- client function , a trigger function (HTTP-triggered ), to starts an orchestrator or entity function
	
	
	