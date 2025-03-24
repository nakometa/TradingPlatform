# TradingPlatform.ApiGateway

A simple API Gateway for demonstrating microservice architecture.

## Endpoints

### 1. Add Order

- **Method:** `POST`
- **URL:** `http://localhost:5006/api/gateway/order/add/{userId}`
- **Body:**
```json
{
  "ticker": "AAPL",
  "quantity": 2,
  "side": "buy"
}

### 2. Get User Portfolio

- **Method:** `GET`
- **URL:** `http://localhost:5006/api/gateway/portfolio/{userId}`