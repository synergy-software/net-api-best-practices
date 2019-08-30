
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


