# Inter-Service Communication

## What You'll Learn
- Synchronous communication
- gRPC
- REST vs messaging

## Prerequisites
- Completed service discovery

## REST Communication

```python
import httpx

async def call_user_service(user_id: int) -> dict:
    """Synchronous call to another service"""
    async with httpx.AsyncClient() as client:
        response = await client.get(
            f"http://user-service:8000/users/{user_id}",
            timeout=5.0
        )
        response.raise_for_status()
        return response.json()
```

## gRPC

```bash
pip install grpcio grpcio-tools
```

```protobuf
// user.proto
syntax = "proto3";
service UserService {
    rpc GetUser (UserRequest) returns (User);
}
message UserRequest { int32 id = 1; }
message User { int32 id = 1; string name = 2; }
```

```python
import grpc
import user_pb2, user_pb2_grpc

def call_grpc(user_id: int) -> dict:
    channel = grpc.insecure_channel('user-service:50051')
    stub = user_pb2_grpc.UserServiceStub(channel)
    response = stub.GetUser(user_pb2.UserRequest(id=user_id))
    return {"id": response.id, "name": response.name}
```

## Summary
- Use REST for simple cases
- Use gRPC for performance
- Consider async messaging

## Next Steps
→ Continue to `04-event-driven-architecture.md`
