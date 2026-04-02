# gRPC Integration

## Overview

gRPC provides high-performance RPC for FastAPI microservices.

## Protocol Buffers

### Service Definition

```protobuf
// Example 1: user.proto
syntax = "proto3";
package user;

service UserService {
  rpc GetUser(GetUserRequest) returns (User);
  rpc CreateUser(CreateUserRequest) returns (User);
  rpc ListUsers(ListUsersRequest) returns (ListUsersResponse);
}

message GetUserRequest {
  int32 user_id = 1;
}

message CreateUserRequest {
  string username = 1;
  string email = 2;
}

message User {
  int32 id = 1;
  string username = 2;
  string email = 3;
}
```

## gRPC with FastAPI

### Server Implementation

```python
# Example 2: gRPC server with FastAPI
import grpc
from concurrent import futures
from fastapi import FastAPI

import user_pb2
import user_pb2_grpc

class UserServiceServicer(user_pb2_grpc.UserServiceServicer):
    def GetUser(self, request, context):
        user = get_user_from_db(request.user_id)
        return user_pb2.User(
            id=user.id,
            username=user.username,
            email=user.email
        )

app = FastAPI()

@app.on_event("startup")
async def startup():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    user_pb2_grpc.add_UserServiceServicer_to_server(
        UserServiceServicer(), server
    )
    server.add_insecure_port('[::]:50051')
    server.start()
```

## Summary

gRPC enables high-performance service communication.

## Next Steps

Continue learning about:
- [Serverless Advanced](./03_serverless_advanced.md)
- [Edge Computing](./04_edge_computing.md)
