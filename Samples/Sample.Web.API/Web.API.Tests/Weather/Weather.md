# Manage weather through API

1. [Get weather forecast](#1-get-weather-forecast-1-step)
1. [Create an item](#2-create-an-item-1-step)
1. [Try to create an item without a name](#3-try-to-create-an-item-without-a-name-3-steps)

## 1. Get weather forecast (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Retrieve weather forecast | OK |

### 1.1. Retrieve weather forecast (1 request)

### 1.1.1. Request to [Get weather forecast]

- Request
```
GET  /api/v1/weather/forecast
```

- Response
```
HTTP/1.1 200 OK
api-supported-versions: 1.0
[
  {
    "date": "2020-01-04T14:00:06.0784313+01:00",
    "temperatureC": 33,
    "temperatureF": 91,
    "summary": "Balmy"
  },
  {
    "date": "2020-01-05T14:00:06.0787814+01:00",
    "temperatureC": 18,
    "temperatureF": 64,
    "summary": "Hot"
  },
  {
    "date": "2020-01-06T14:00:06.0787866+01:00",
    "temperatureC": -10,
    "temperatureF": 15,
    "summary": "Balmy"
  },
  {
    "date": "2020-01-07T14:00:06.078787+01:00",
    "temperatureC": 52,
    "temperatureF": 125,
    "summary": "Warm"
  },
  {
    "date": "2020-01-08T14:00:06.0787874+01:00",
    "temperatureC": 25,
    "temperatureF": 76,
    "summary": "Balmy"
  }
]
```

| Expected Results  | Status |
| - | - |
| Weather forecast is returned | OK |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 200 (OK) | OK |
| Convention: Returned HTTP Content-Type is "application/json" | OK |



## 2. Create an item (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Create TODO item | OK |

### 2.1. Create TODO item (1 request)

### 2.1.1. Request to [Create a new TODO item named 'do sth']

- Request
```
POST  /api/v1/weather
{
  "Id": 123,
  "Name": "do sth",
  "IsComplete": false
}
```

- Response
```
HTTP/1.1 201 Created
Location: forecast
api-supported-versions: 1.0
{
  "id": 123,
  "name": "do sth",
  "isComplete": false
}
```

| Expected Results  | Status |
| - | - |
| Item is created and its details are returned | OK |
| Convention: HTTP request method is POST | OK |
| Convention: Returned HTTP status code is 201 (Created) | OK |
| Convention: Location header (pointing to newly created element) is returned with response. | OK |
| Convention: Returned HTTP Content-Type is "application/json" | OK |



## 3. Try to create an item without a name (3 steps)

| # | Step Actions | Status |
| - | - | - |
| 1 | Create TODO item with a null name | OK |
| 2 | Create TODO item with an empty name | OK |
| 3 | Create TODO item with an whitespace name | OK |

### 3.1. Create TODO item with a null name (1 request)

### 3.1.1. Request to [Create a new TODO item named '']

- Request
```
POST  /api/v1/weather
{
  "Id": 123,
  "Name": null,
  "IsComplete": false
}
```

- Response
```
HTTP/1.1 400 BadRequest
api-supported-versions: 1.0
{
  "message": "'Name' is whitespace",
  "traceId": "0HLSG9REJHPO6"
}
```

| Expected Results  | Status |
| - | - |
| Item is NOT created and error is returned | OK |
| Convention: HTTP request method is POST | OK |
| Convention: Returned HTTP status code is 400 (Bad Request) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |


### 3.2. Create TODO item with an empty name (1 request)

### 3.2.2. Request to [Create a new TODO item named '']

- Request
```
POST  /api/v1/weather
{
  "Id": 123,
  "Name": "",
  "IsComplete": false
}
```

- Response
```
HTTP/1.1 400 BadRequest
api-supported-versions: 1.0
{
  "message": "'Name' is whitespace",
  "traceId": "0HLSG9REJHPO7"
}
```

| Expected Results  | Status |
| - | - |
| Item is NOT created and error is returned | OK |
| Convention: HTTP request method is POST | OK |
| Convention: Returned HTTP status code is 400 (Bad Request) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |


### 3.3. Create TODO item with an whitespace name (1 request)

### 3.3.3. Request to [Create a new TODO item named '  ']

- Request
```
POST  /api/v1/weather
{
  "Id": 123,
  "Name": "  ",
  "IsComplete": false
}
```

- Response
```
HTTP/1.1 400 BadRequest
api-supported-versions: 1.0
{
  "message": "'Name' is whitespace",
  "traceId": "0HLSG9REJHPO8"
}
```

| Expected Results  | Status |
| - | - |
| Item is NOT created and error is returned | OK |
| Convention: HTTP request method is POST | OK |
| Convention: Returned HTTP status code is 400 (Bad Request) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |


