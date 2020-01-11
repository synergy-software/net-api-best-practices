# Manage users through API

1. [Get empty list of users](#1-get-empty-list-of-users-1-step)
1. [Create a user](#2-create-a-user-2-steps)
1. [Get user](#3-get-user-1-step)
1. [Try to create user without login](#4-try-to-create-user-without-login-3-steps)
1. [Delete user](#5-delete-user-1-step)

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
api-supported-versions: 1.0
Content-Type: application/json; charset=utf-8
Content-Length: 19
{
  "users": []
}
```

| Expected Results  | Status |
| - | - |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 200 (OK) | OK |
| Convention: Returned HTTP Content-Type is "application/json" | OK |
| Manual: Empty users list is returned | OK |



## 2. Create a user (2 steps)

| # | Step Actions | Status |
| - | - | - |
| 1 | Create new user | OK |
| 2 | Get created user pointed by "Location" header | OK |

### 2.1. Create new user (1 request)

### 2.1.1. Request to [Create a new user with login 'marcin@synergy.com']

- Request
```
POST  /api/v1/users
Content-Type: application/json; charset=utf-8
{
  "Login": "marcin@synergy.com"
}
```

- Response
```
HTTP/1.1 201 Created
Location: http://localhost/api/v1/users/f2d3f31011ae4f149c062d71eb1bb7c0
api-supported-versions: 1.0
Content-Type: application/json; charset=utf-8
Content-Length: 104
{
  "user": {
    "id": "f2d3f31011ae4f149c062d71eb1bb7c0",
    "login": "marcin@synergy.com"
  }
}
```

| Expected Results  | Status |
| - | - |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 201 (Created) | OK |
| Convention: Location header (pointing to newly created element) is returned with response. | OK |
| Convention: Returned HTTP Content-Type is "application/json" | OK |
| Manual: User is created and its details are returned | OK |


### 2.2. Get created user pointed by "Location" header (1 request)

### 2.2.2. Request to [Get user located at http://localhost/api/v1/users/0f12ad06edd34088a13d4df6de50c1f1]

- Request
```
GET  /api/v1/users/57aec90cbe34400cadaaeda89cb540ac
```

- Response
```
HTTP/1.1 200 OK
api-supported-versions: 1.0
Content-Type: application/json; charset=utf-8
Content-Length: 69
{
  "id": "57aec90cbe34400cadaaeda89cb540ac",
  "login": "login"
}
```

| Expected Results  | Status |
| - | - |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 200 (OK) | OK |
| Convention: Returned HTTP Content-Type is "application/json" | OK |
| Manual: User details are returned | OK |



## 3. Get user (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Get user by id | OK |

### 3.1. Get user by id (1 request)

### 3.1.1. Request to [Get user with id 0f12ad06edd34088a13d4df6de50c1f1]

- Request
```
GET  /api/v1/users/57aec90cbe34400cadaaeda89cb540ac
```

- Response
```
HTTP/1.1 200 OK
api-supported-versions: 1.0
Content-Type: application/json; charset=utf-8
Content-Length: 69
{
  "id": "57aec90cbe34400cadaaeda89cb540ac",
  "login": "login"
}
```

| Expected Results  | Status |
| - | - |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 200 (OK) | OK |
| Convention: Returned HTTP Content-Type is "application/json" | OK |
| Manual: User details are returned | OK |



## 4. Try to create user without login (3 steps)

| # | Step Actions | Status |
| - | - | - |
| 1 | Create user with a null login | OK |
| 2 | Create user with an empty login | OK |
| 3 | Create user with a whitespace login | OK |

### 4.1. Create user with a null login (1 request)

### 4.1.1. Request to [Create a new user with login '']

- Request
```
POST  /api/v1/users
Content-Type: application/json; charset=utf-8
{
  "Login": null
}
```

- Response
```
HTTP/1.1 400 BadRequest
api-supported-versions: 1.0
Content-Type: application/json
{
  "message": "'Login' is whitespace",
  "traceId": "0HLSMJ359LHJO"
}
```

| Expected Results  | Status |
| - | - |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 400 (BadRequest) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |
| Manual: User is NOT created and error is returned | OK |


### 4.2. Create user with an empty login (1 request)

### 4.2.2. Request to [Create a new user with login '']

- Request
```
POST  /api/v1/users
Content-Type: application/json; charset=utf-8
{
  "Login": ""
}
```

- Response
```
HTTP/1.1 400 BadRequest
api-supported-versions: 1.0
Content-Type: application/json
{
  "message": "'Login' is whitespace",
  "traceId": "0HLSMJ359LHJP"
}
```

| Expected Results  | Status |
| - | - |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 400 (BadRequest) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |
| Manual: User is NOT created and error is returned | OK |


### 4.3. Create user with a whitespace login (1 request)

### 4.3.3. Request to [Create a new user with login '  ']

- Request
```
POST  /api/v1/users
Content-Type: application/json; charset=utf-8
{
  "Login": "  "
}
```

- Response
```
HTTP/1.1 400 BadRequest
api-supported-versions: 1.0
Content-Type: application/json
{
  "message": "'Login' is whitespace",
  "traceId": "0HLSMJ359LHJQ"
}
```

| Expected Results  | Status |
| - | - |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 400 (BadRequest) | OK |
| Convention: error JSON contains "message" node | OK |
| Convention: error JSON contains "traceId" node | OK |
| Manual: User is NOT created and error is returned | OK |



## 5. Delete user (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Delete user by id | OK |

### 5.1. Delete user by id (1 request)

### 5.1.1. Request to [Delete user with id 0f12ad06edd34088a13d4df6de50c1f1]

- Request
```
DELETE  /api/v1/users/57aec90cbe34400cadaaeda89cb540ac
```

- Response
```
HTTP/1.1 200 OK
api-supported-versions: 1.0
```

| Expected Results  | Status |
| - | - |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 200 (OK) | OK |
| Manual: User is deleted and no details are returned | OK |


