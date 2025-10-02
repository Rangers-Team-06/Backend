# Tools Management API

This document describes the Tools Management API endpoints that have been implemented.

## Database Schema

The Tools table has been created with the following structure:

```sql
CREATE TABLE Tools (
    Id VARCHAR(36) PRIMARY KEY, -- Represents the immutable ToolUID (UUID/GUID)
    FriendlyName VARCHAR(255) NOT NULL,
    Description TEXT, -- From P1-S01, useful for the label
    Make VARCHAR(100),
    Model VARCHAR(100),
    Category VARCHAR(100),
    Supplier VARCHAR(100),
    PurchaseDate DATETIME,
    UnitCost DECIMAL(18, 2),
    Currency VARCHAR(3),
    QRData TEXT NOT NULL, -- Stores the data encoded in the QR code
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

## API Endpoints

### 1. Register a New Tool
**POST** `/api/tools/register`

Registers a new tool in the system.

**Request Body:**
```json
{
  "friendlyName": "Hammer", // Required
  "description": "Claw hammer for general use",
  "make": "Stanley",
  "model": "16oz",
  "category": "Hand Tools",
  "supplier": "Hardware Store",
  "purchaseDate": "2024-01-15T00:00:00Z",
  "unitCost": 25.99,
  "currency": "USD",
  "qrData": "QR_HAMMER_001" // Required
}
```

**Response:** `201 Created`
```json
{
  "id": "12345678-1234-1234-1234-123456789012",
  "friendlyName": "Hammer",
  "description": "Claw hammer for general use",
  "make": "Stanley",
  "model": "16oz",
  "category": "Hand Tools",
  "supplier": "Hardware Store",
  "purchaseDate": "2024-01-15T00:00:00Z",
  "unitCost": 25.99,
  "currency": "USD",
  "qrData": "QR_HAMMER_001",
  "createdAt": "2024-01-20T10:30:00Z",
  "updatedAt": "2024-01-20T10:30:00Z"
}
```

### 2. Get Tools by Name
**GET** `/api/tools/search/name/{name}`

Retrieves tools that contain the specified name in their FriendlyName field.

**Example:** `/api/tools/search/name/hammer`

**Response:** `200 OK`
```json
[
  {
    "id": "12345678-1234-1234-1234-123456789012",
    "friendlyName": "Hammer",
    // ... other fields
  }
]
```

### 3. Get Tool by ID
**GET** `/api/tools/{id}`

Retrieves a specific tool by its ID.

**Example:** `/api/tools/12345678-1234-1234-1234-123456789012`

**Response:** `200 OK` or `404 Not Found`

### 4. Get All Tools
**GET** `/api/tools`

Retrieves all tools in the system.

**Response:** `200 OK`
```json
[
  {
    "id": "12345678-1234-1234-1234-123456789012",
    "friendlyName": "Hammer",
    // ... other fields
  },
  // ... more tools
]
```

### 5. Search Tools
**GET** `/api/tools/search?searchTerm={term}`

Searches tools across multiple fields (FriendlyName, Description, Make, Model, Category, Supplier).

**Example:** `/api/tools/search?searchTerm=Stanley`

**Response:** `200 OK`

### 6. Update a Tool
**PUT** `/api/tools/{id}`

Updates an existing tool. Only provided fields will be updated.

**Request Body:** (All fields optional)
```json
{
  "friendlyName": "Updated Hammer",
  "description": "Updated description",
  "make": "Updated Make"
  // ... other fields
}
```

**Response:** `200 OK` or `404 Not Found`

### 7. Delete a Tool
**DELETE** `/api/tools/{id}`

Deletes a tool from the system.

**Response:** `204 No Content` or `404 Not Found`

## Key Features

1. **Tool Registration**: Register new tools with all relevant information
2. **Name-based Search**: Find tools by searching their friendly names
3. **Comprehensive Search**: Search across multiple tool attributes
4. **CRUD Operations**: Full Create, Read, Update, Delete functionality
5. **Validation**: Input validation on required fields
6. **Logging**: Comprehensive logging for debugging and monitoring
7. **Error Handling**: Proper HTTP status codes and error messages

## Getting Started

1. **Database Setup**: Run the Entity Framework migration to create the Tools table:
   ```bash
   dotnet ef database update --project Team-06.Data
   ```

2. **Run the API**: Start the application:
   ```bash
   dotnet run --project Team-06.API
   ```

3. **Access Swagger**: Navigate to `/swagger` to see the interactive API documentation

4. **Test the API**: Use the provided test endpoints or run the unit tests:
   ```bash
   dotnet test Team-06.Tests
   ```

## Notes

- All tool IDs are generated as GUIDs for uniqueness
- QRData field is required and stores the QR code information
- FriendlyName is required for tool identification
- Timestamps (CreatedAt, UpdatedAt) are automatically managed
- The API includes comprehensive logging for monitoring and debugging