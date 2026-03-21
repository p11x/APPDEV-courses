# gRPC with Express

## 📌 What You'll Learn

- What gRPC is and when to use it
- Setting up gRPC with Express
- Using proto files for contract definition

## 🧠 Concept Explained (Plain English)

**gRPC** is a high-performance RPC framework that uses HTTP/2 and Protocol Buffers. It's efficient for service-to-service communication.

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';

const app = express();

// Load proto
const packageDefinition = protoLoader.loadSync('./service.proto');
const proto = grpc.loadPackageDefinition(packageDefinition);

// gRPC service implementation
const server = new grpc.Server();

server.addService(proto.MyService.service, {
  GetUser: (call, callback) => {
    callback(null, { id: call.request.id, name: 'John' });
  },
  CreateUser: (call, callback) => {
    callback(null, { id: '1', ...call.request });
  }
});

server.bindAsync('0.0.0.0:50051', grpc.ServerCredentials.createInsecure(), () => {
  console.log('gRPC server running on port 50051');
});

// Express HTTP endpoint (for non-gRPC clients)
app.get('/health', (req, res) => res.json({ status: 'ok' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Express: ${PORT}`));
```

## ✅ Quick Recap

- gRPC uses HTTP/2 and Protocol Buffers
- Efficient for service-to-service communication
- Requires .proto file definitions

## 🔗 What's Next

Learn about [Designing Webhooks](./../02_Webhook_Patterns/01_designing-webhooks.md).
