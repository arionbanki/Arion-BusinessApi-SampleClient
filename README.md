# Arion-BusinessApi-SampleClient
A basic .net sample client that shows examples on how to consume Business API

## Endpoints
### 1. api/v1/cards
Reads a list of cards potentially with additional information, e.g. balance information.

### 2. api/v1/cards/{cardId}
Reads details about a card.

### 3. api/v1/cards/{cardid}/balances
Read detailed balance information about the addressed card by "card-id".

### 4. api/v1/cards/{cardid}/transactions
Reads transaction list or transaction report from a given card addressed by "card-id".