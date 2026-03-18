# gRPC Introduction

## What You'll Learn
- gRPC fundamentals and Protocol Buffers
- Defining .proto files
- Generating Python code
- Implementing gRPC services
- HTTP/2 and streaming

## Prerequisites
- Understanding of APIs
- Python async knowledge

## What Is gRPC?

gRPC is a high-performance RPC framework that uses HTTP/2 and Protocol Buffers:

```
REST:        JSON over HTTP/1.1
gRPC:        Binary (Protocol Buffers) over HTTP/2
```

## Protocol Buffers

Protocol Buffers (proto) define your data structures and services:

```protobuf
// user.proto
syntax = "proto3";

message User {
  int32 id = 1;
  string username = 2;
  string email = 3;
}

message CreateUserRequest {
  string username = 1;
  string email = 2;
}

message CreateUserResponse {
  User user = 1;
}

service UserService {
  rpc CreateUser (CreateUserRequest) returns (CreateUserResponse);
  rpc GetUser (GetUserRequest) returns (User);
}
```

## Installing gRPC

```bash
pip install grpcio grpcio-tools
```

## Generating Python Code

```bash
python -m grpc_tools.protoc -I. --python_out=. --grpc_python_out=. user.proto
```

## Implementing gRPC Server

```python
import grpc
from concurrent import futures
import user_pb2
import user_pb2_grpc

class UserServicer(user_pb2_grpc.UserServiceServicer):
    def CreateUser(self, request, context):
        # Create user logic
        user = user_pb2.User(
            id=1,
            username=request.username,
            email=request.email
        )
        return user_pb2.CreateUserResponse(user=user)
    
    def GetUser(self, request, context):
        # Get user logic
        return user_pb2.User(
            id=request.id,
            username="john",
            email="john@example.com"
        )

def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    user_pb2_grpc.add_UserServiceServicer_to_server(UserServicer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    print("Server started on port 50051")
    server.wait_for_termination()

if __name__ == '__main__':
    serve()
```

## gRPC Client

```python
import grpc
import user_pb2
import user_pb2_grpc

def run():
    channel = grpc.insecure_channel('localhost:50051')
    stub = user_pb2_grpc.UserServiceStub(channel)
    
    response = stub.CreateUser(user_pb2.CreateUserRequest(
        username="john",
        email="john@example.com"
    ))
    
    print(f"User created: {response.user}")

if __name__ == '__main__':
    run()
```

## Streaming

```protobuf
service ChatService {
  rpc Chat(stream ChatMessage) returns (stream ChatMessage);
}

message ChatMessage {
  string user = 1;
  string message = 2;
}
```

## Summary

- gRPC uses HTTP/2 and Protocol Buffers for performance
- Define services in .proto files
- Generate Python code with grpc_tools
- Supports streaming for bidirectional communication
