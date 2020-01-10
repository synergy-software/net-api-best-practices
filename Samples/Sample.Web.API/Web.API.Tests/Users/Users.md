# Manage users through API

1. [Get empty list of users](#1-get-empty-list-of-users-1-step)
1. [Create a user](#2-create-a-user-1-step)
1. [Try to create user without login](#3-try-to-create-user-without-login-3-steps)

## 1. Get empty list of users (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Retrieve users | OK |

### 1.1. Retrieve users (1 request)

### 1.1.1. Request to [Get list of users]

- Request
```
GET  /api/v1/users
```

- Response
```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
api-supported-versions: 1.0
{
  "users": []
}
```

| Expected Results  | Status |
| - | - |
| Empty users list is returned | OK |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 200 (OK) | OK |
| Convention: Returned HTTP Content-Type is "application/json" | OK |



## 2. Create a user (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Create new user | OK |

### 2.1. Create new user (1 request)

### 2.1.1. Request to [Create a new user with login 'marcin@synergy.com']

- Request
```
POST  /api/v1/users
{
  "Login": "marcin@synergy.com"
}
```

- Response
```
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Location: http://localhost/api/v1/users/236556f912714824bdd2f445466dcd46
api-supported-versions: 1.0
{
  "user": {
    "id": "236556f912714824bdd2f445466dcd46",
    "login": "marcin@synergy.com"
  }
}
```

| Expected Results  | Status |
| - | - |
| User is created and its details are returned | OK |
| Convention: HTTP request method is POST | OK |
| Convention: Returned HTTP status code is 201 (Created) | OK |
| Convention: Location header (pointing to newly created element) is returned with response. | OK |
| Convention: Returned HTTP Content-Type is "application/json" | OK |



## 3. Try to create user without login (3 steps)

| # | Step Actions | Status |
| - | - | - |
| 1 | Create user with a null login | OK |
| 2 | Create user with an empty login | OK |
| 3 | Create user item with a whitespace login | OK |

### 3.1. Create user with a null login (1 request)

### 3.1.1. Request to [Create a new user with login '']

- Request
```
POST  /api/v1/users
{
  "Login": null
}
```

- Response
```
HTTP/1.1 400 BadRequest
Content-Type: application/json; charset=utf-8
api-supported-versions: 1.0
{
  "message": "'Login' is whitespace",
  "traceId": "0HLSL55KTTJTI"
}
```

| Expected Results  | Status |
| - | - |
| User is NOT created and error is returned | OK |
| Convention: HTTP request method is POST | OK |
| Convention: Returned HTTP status code is 400 (Bad Request) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |


### 3.2. Create user with an empty login (1 request)

### 3.2.2. Request to [Create a new user with login '']

- Request
```
POST  /api/v1/users
{
  "Login": ""
}
```

- Response
```
HTTP/1.1 400 BadRequest
Content-Type: application/json; charset=utf-8
api-supported-versions: 1.0
{
  "message": "'Login' is whitespace",
  "traceId": "0HLSL55KTTJTJ"
}
```

| Expected Results  | Status |
| - | - |
| User is NOT created and error is returned | OK |
| Convention: HTTP request method is POST | OK |
| Convention: Returned HTTP status code is 400 (Bad Request) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |


### 3.3. Create user item with a whitespace login (1 request)

### 3.3.3. Request to [Create a new user with login '  ']

- Request
```
POST  /api/v1/users
{
  "Login": "  "
}
```

- Response
```
HTTP/1.1 400 BadRequest
Content-Type: application/json; charset=utf-8
api-supported-versions: 1.0
{
  "message": "'Login' is whitespace",
  "traceId": "0HLSL55KTTJTK"
}
```

| Expected Results  | Status |
| - | - |
| User is NOT created and error is returned | OK |
| Convention: HTTP request method is POST | OK |
| Convention: Returned HTTP status code is 400 (Bad Request) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |


