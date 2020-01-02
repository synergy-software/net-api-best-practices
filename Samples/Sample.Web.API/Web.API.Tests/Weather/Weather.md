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
GET /api/v1/weather/forecast
```

- Response
```
HTTP/1.1 200 OK
api-supported-versions: 1.0
[
  {
    "date": "2020-01-03T12:57:18.7256736+01:00",
    "temperatureC": 44,
    "temperatureF": 111,
    "summary": "Freezing"
  },
  {
    "date": "2020-01-04T12:57:18.7262556+01:00",
    "temperatureC": -1,
    "temperatureF": 31,
    "summary": "Sweltering"
  },
  {
    "date": "2020-01-05T12:57:18.7262647+01:00",
    "temperatureC": 2,
    "temperatureF": 35,
    "summary": "Sweltering"
  },
  {
    "date": "2020-01-06T12:57:18.7262655+01:00",
    "temperatureC": 26,
    "temperatureF": 78,
    "summary": "Warm"
  },
  {
    "date": "2020-01-07T12:57:18.7262671+01:00",
    "temperatureC": 45,
    "temperatureF": 112,
    "summary": "Sweltering"
  }
]
```

| Expected Results  | Status |
| - | - |
| Weather forecast is returned | OK |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 200 (OK) | OK |



## 2. Create an item (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Create TODO item | OK |

### 2.1. Create TODO item (1 request)

### 2.1.1. Request to [Create a new TODO item named 'do sth']

- Request
```
POST /api/v1/weather
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
POST /api/v1/weather
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
  "errorId": "a027ea18bab14e6a96ca54d0db75d05d"
}
```

| Expected Results  | Status |
| - | - |
| Item is NOT created and error is returned | OK |
| Returned HTTP status code is 400 (BadRequest) | OK |


### 3.2. Create TODO item with an empty name (1 request)

### 3.2.2. Request to [Create a new TODO item named '']

- Request
```
POST /api/v1/weather
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
  "errorId": "04205e055c064e2b89aaf561c474643e"
}
```

| Expected Results  | Status |
| - | - |
| Item is NOT created and error is returned | OK |
| Returned HTTP status code is 400 (BadRequest) | OK |


### 3.3. Create TODO item with an whitespace name (1 request)

### 3.3.3. Request to [Create a new TODO item named '  ']

- Request
```
POST /api/v1/weather
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
  "errorId": "59238ad3538347279000f0afc36118c9"
}
```

| Expected Results  | Status |
| - | - |
| Item is NOT created and error is returned | OK |
| Returned HTTP status code is 400 (BadRequest) | OK |


