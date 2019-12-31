# Manage weather through API

1. [Get weather forecast](#1-get-weather-forecast-(1-step))
1. [Create an item](#2-create-an-item-(1-step))

## 1. Get weather forecast (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Retrieve weather forecast | OK |

### 1.1. Retrieve weather forecast (1 request)


<details><summary>Request to [Get weather forecast]</summary>

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
    "date": "2020-01-01T15:12:15.6087068+01:00",
    "temperatureC": 38,
    "temperatureF": 100,
    "summary": "Warm"
  },
  {
    "date": "2020-01-02T15:12:15.6091263+01:00",
    "temperatureC": -4,
    "temperatureF": 25,
    "summary": "Chilly"
  },
  {
    "date": "2020-01-03T15:12:15.6091319+01:00",
    "temperatureC": 25,
    "temperatureF": 76,
    "summary": "Balmy"
  },
  {
    "date": "2020-01-04T15:12:15.6091323+01:00",
    "temperatureC": 0,
    "temperatureF": 32,
    "summary": "Sweltering"
  },
  {
    "date": "2020-01-05T15:12:15.6091327+01:00",
    "temperatureC": 53,
    "temperatureF": 127,
    "summary": "Scorching"
  }
]
```

| Expected Results  | Status |
| - | - |
| Weather forecast is returned | OK |
| Returned HTTP status code is 200 (OK) | OK |

</details>


## 2. Create an item (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Create TODO item | OK |

### 2.1. Create TODO item (1 request)


<details><summary>Request to [Create a new TODO item named 'do sth']</summary>

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
| Returned HTTP status code is 201 (Created) | OK |

</details>

