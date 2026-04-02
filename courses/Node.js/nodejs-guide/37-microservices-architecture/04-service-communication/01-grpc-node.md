# gRPC with Node.js

## What You'll Learn

- What gRPC is and how it works
- How to set up gRPC with Node.js
- How to define services with Protocol Buffers
- How gRPC compares to REST

## What Is gRPC?

gRPC is a high-performance RPC framework that uses Protocol Buffers for serialization. It is faster than REST/JSON and provides type-safe contracts.

| Feature | REST | gRPC |
|---------|------|------|
| Protocol | HTTP/1.1 | HTTP/2 |
| Format | JSON | Protobuf (binary) |
| Type safety | No | Yes (generated) |
| Streaming | No | Yes (bidirectional) |
| Speed | Moderate | Fast |

## Setup

```bash
npm install @grpc/grpc-js @grpc/proto-loader
```

## Service Definition

```protobuf
// user.proto

syntax = "proto3";

service UserService {
  rpc GetUser (GetUserRequest) returns (User);
  rpc ListUsers (ListUsersRequest) returns (ListUsersResponse);
  rpc CreateUser (CreateUserRequest) returns (User);
}

message GetUserRequest {
  string id = 1;
}

message User {
  string id = 1;
  string name = 2;
  string email = 3;
}

message ListUsersRequest {
  int32 page = 1;
  int32 limit = 2;
}

message ListUsersResponse {
  repeated User users = 1;
  int32 total = 2;
}

message CreateUserRequest {
  string name = 1;
  string email = 2;
}
```

## Server Implementation

```ts
// server.ts

import { Server, ServerCredentials, loadPackageDefinition } from '@grpc/grpc-js';
import { loadSync } from '@grpc/proto-loader';

const packageDef = loadSync('./user.proto', {
  keepCase: true,
  longs: String,
  enums: String,
  defaults: true,
  oneofs: true,
});

const proto = loadPackageDefinition(packageDef);

const server = new Server();

server.addService(proto.UserService.service, {
  getUser: (call, callback) => {
    const user = { id: call.request.id, name: 'Alice', email: 'alice@example.com' };
    callback(null, user);
  },

  listUsers: (call, callback) => {
    const users = [
      { id: '1', name: 'Alice', email: 'alice@example.com' },
      { id: '2', name: 'Bob', email: 'bob@example.com' },
    ];
    callback(null, { users, total: users.length });
  },

  createUser: (call, callback) => {
    const user = { id: String(Date.now()), ...call.request };
    callback(null, user);
  },
});

server.bindAsync('0.0.0.0:50051', ServerCredentials.createInsecure(), () => {
  console.log('gRPC server on port 50051');
  server.start();
});
```

## Next Steps

For Protobuf, continue to [gRPC Protobuf](./02-grpc-protobuf.md).
