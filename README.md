## Development basics
 * TODO: Use DbC - always think about contract you expose (contract does not only means API contract, but every public method is a contract - even if not exposed outside your application)
   * Sample contracts library you may use - https://www.nuget.org/packages/Synergy.Contracts/
 * TODO: Use container - e.g. Windsor Containaer (or any)

## ASP Basics
 * Add reading configuration hierarchy based on environment - see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1
 * Add API versioning - https://github.com/microsoft/aspnet-api-versioning
   * Even if you don't need versioning now, you will need it later
   * Setup every controller to be v1 (at the beginning)
 * Add swagger to your project - e.g. Swashbuckle - see https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
   * Add versioned swagger documents
   * Add returned HTTP codes - e.g. using [ProducesResponseType]
   * TODO: Add test that regenerates JSON API description document and fails when it's changed - e.g. using Ms Test Server - https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1
 * TODO: Add logging engine
 * TODO: Add exception handling

## Controllers
  * Controller methods should be thin - containing only mapping/composing/dispatching code
  * Fullfil Rest API Tutorial rules
    * Use proper HTTP methods
    * Use resource naming rules
  * Add swagger that describes your API
  * Add test that fails when your API changes and writes the API description to a file. Each API change must be followed by the test run. Reviewer sees the changes explicitly in the API description file. Commit the file with code.
			
## DTOs

Incoming and outgoing Controllers data are called Data Transfer Objects (DTO) and are dedicated for passing data. Do not keep logic in there. Do not use the DTO deeper in the system.
  * DTOs: 
    * Query
    * QueryResult
    * Command
    * CommandResult
    * Request
    * Response
  * Add JSON attributes to your DTOs with filed names - (e.g. `[JsonProperty("id")]` or `[DataMember(Name="id")]`). Then you can change name of the properties without worrying about API accidental damage.

```csharp
    [DataContract]
    public partial class Tag
    { 
        [DataMember(Name="id")]
        public long Id { get; set; }
    }
```

  * ReadOnlyCollection<T> as collection property

## Commands & Queries
  * Do not reuse DTOs between Commands & Queries
    
### Command + CommandHandler
  * Controller woła CommandDispatcher i przekazuje mu Command
  
### Query + QueryHandler
  * Controller query action is: 
    * thin - calls QueryDispatcher passing incoming TQuery object to it; and returns TQueryResult
    * GET http method
  * QueryDispatcher
    * Searches for IQueryHandler<TQuery>
  * Query must be deserializable 
  * Sometimes TQuery is composed of additional DTO - TRequest. It happens when there is a request object incoming to API and TQuery must be extended by Controller action - composing TRequest and some additional data into TQuery object - passed to the QueryDispatcher in the next step
  * QueryResult: 
    * Should be immutable and serializable only
    * Contains mapping constructor
    * Is always immutable - { get; } only
    * Is serializable only - it is never deserialized by our API


