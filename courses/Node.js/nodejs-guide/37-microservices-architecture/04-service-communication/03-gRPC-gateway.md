# gRPC Gateway

## What You'll Learn

- How to expose gRPC services as REST APIs
- How to use gRPC-Gateway
- How to handle REST to gRPC translation

## gRPC-Gateway

gRPC-Gateway generates a REST API from gRPC service definitions.

```protobuf
// user.proto with REST annotations

syntax = "proto3";

import "google/api/annotations.proto";

service UserService {
  rpc GetUser (GetUserRequest) returns (User) {
    option (google.api.http) = {
      get: "/api/users/{id}"
    };
  }

  rpc CreateUser (CreateUserRequest) returns (User) {
    option (google.api.http) = {
      post: "/api/users"
      body: "*"
    };
  }

  rpc ListUsers (ListUsersRequest) returns (ListUsersResponse) {
    option (google.api.http) = {
      get: "/api/users"
    };
  }
}
```

## Node.js Implementation

```ts
// Use grpc-js to proxy REST to gRPC

import express from 'express';
import { credentials, loadPackageDefinition } from '@grpc/grpc-js';
import { loadSync } from '@grpc/proto-loader';

const proto = loadPackageDefinition(loadSync('./user.proto'));
const client = new proto.UserService('localhost:50051', credentials.createInsecure());

const app = express();

app.get('/api/users/:id', (req, res) => {
  client.getUser({ id: req.params.id }, (err, user) => {
    if (err) return res.status(500).json({ error: err.message });
    res.json(user);
  });
});

app.listen(3000);
```

## Next Steps

For patterns, continue to [gRPC Patterns](./04-grpc-patterns.md).
