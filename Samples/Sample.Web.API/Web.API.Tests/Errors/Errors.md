# Check API errors

1. [Get not existing resource from API](#1-get-not-existing-resource-from-api-1-step)

## 1. Get not existing resource from API (1 step)

| # | Step Actions | Status |
| - | - | - |
| 1 | Try to retrieve not existing resource | OK |

### 1.1. Try to retrieve not existing resource (1 request)

### 1.1.1. Request to [Make GET request to not existing API]

- Request
```
GET  /api/v1/i-do-not-exist
```

- Response
```
HTTP/1.1 404 NotFound
```

| Expected Results  | Status |
| - | - |
| Error is returned | OK |
| Convention: HTTP request method is GET | OK |
| Convention: Returned HTTP status code is 404 (NotFound) | OK |


