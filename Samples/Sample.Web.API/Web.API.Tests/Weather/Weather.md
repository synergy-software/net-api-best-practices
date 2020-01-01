# Manage weather through API

1. [Get weather forecast](#1-get-weather-forecast-1-step)
1. [Create an item](#2-create-an-item-1-step)

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


