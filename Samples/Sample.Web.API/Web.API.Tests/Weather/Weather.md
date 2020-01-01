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
    "date": "2020-01-01T17:45:42.6175116+01:00",
    "temperatureC": 50,
    "temperatureF": 121,
    "summary": "Mild"
  },
  {
    "date": "2020-01-02T17:45:42.6178945+01:00",
    "temperatureC": 10,
    "temperatureF": 49,
    "summary": "Sweltering"
  },
  {
    "date": "2020-01-03T17:45:42.6179+01:00",
    "temperatureC": 24,
    "temperatureF": 75,
    "summary": "Mild"
  },
  {
    "date": "2020-01-04T17:45:42.6179005+01:00",
    "temperatureC": 31,
    "temperatureF": 87,
    "summary": "Sweltering"
  },
  {
    "date": "2020-01-05T17:45:42.6179009+01:00",
    "temperatureC": 13,
    "temperatureF": 55,
    "summary": "Chilly"
  }
]
```

| Expected Results  | Status |
| - | - |
| Weather forecast is returned | OK |
| HTTP request method is GET | OK |
| Returned HTTP status code is 200 (OK) | OK |



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
| HTTP request method is POST | OK |
| Returned HTTP status code is 201 (Created) | OK |
| Location header (pointing to newly created element) is returned with response. | OK |



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
  "errorId": "1b04144902a2459caaf83ed2d0a48cfc"
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
  "errorId": "e4807873ab084d3792fab7c600e608c0"
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
  "errorId": "878c2e8633344d73bb072c18545d3f8b"
}
```

| Expected Results  | Status |
| - | - |
| Item is NOT created and error is returned | OK |
| Returned HTTP status code is 400 (BadRequest) | OK |


