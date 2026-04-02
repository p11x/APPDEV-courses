# Protocol Buffers

## What You'll Learn

- How Protocol Buffers work
- How to define message types
- How to generate TypeScript types
- How to handle schema evolution

## Proto3 Syntax

```protobuf
// user.proto

syntax = "proto3";

package user;

// Enum
enum UserRole {
  USER = 0;
  ADMIN = 1;
  MODERATOR = 2;
}

// Message types
message User {
  string id = 1;
  string name = 2;
  string email = 3;
  UserRole role = 4;
  repeated string tags = 5;
  Address address = 6;
}

message Address {
  string street = 1;
  string city = 2;
  string country = 3;
}

// Service
service UserService {
  rpc GetUser (GetUserRequest) returns (User);
  rpc CreateUser (CreateUserRequest) returns (User);
}

message GetUserRequest {
  string id = 1;
}

message CreateUserRequest {
  string name = 1;
  string email = 2;
  UserRole role = 3;
}
```

## Generate TypeScript Types

```bash
# Install protobuf tools
npm install -D @bufbuild/protoc-gen-es @bufbuild/protobuf

# Generate TypeScript
protoc --es_out=src/gen --es_opt=target=ts user.proto
```

## Schema Evolution

| Change | Safe? |
|--------|-------|
| Add new field | Yes (backward compatible) |
| Remove field | Yes (if not reused) |
| Rename field | No (add new, deprecate old) |
| Change field type | No |

## Next Steps

For gRPC Gateway, continue to [gRPC Gateway](./03-gRPC-gateway.md).
