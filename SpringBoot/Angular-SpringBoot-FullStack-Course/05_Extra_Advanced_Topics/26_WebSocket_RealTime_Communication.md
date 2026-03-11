# WebSocket Real-Time Communication

## Concept Title and Overview

In this lesson, you'll learn how to implement real-time bidirectional communication between your Spring Boot backend and Angular frontend using WebSockets.

## Real-World Importance and Context

Traditional HTTP requests are unidirectional—the client requests, server responds. WebSockets enable bidirectional communication, allowing the server to push data to clients in real-time. This is essential for:
- Chat applications
- Live notifications
- Real-time dashboards
- Collaborative editing tools
- Stock tickers and live feeds

## Detailed Step-by-Step Explanation

### Understanding WebSockets

```
┌─────────────────────────────────────────────────────────────────────────┐
│                 HTTP vs WEBSOCKET                                        │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  HTTP (Request-Response)                                               │
│  ┌──────────┐      Request      ┌──────────┐                          │
│  │ Angular  │ ───────────────►  │ Spring   │                          │
│  │   App    │ ◄───────────────  │ Boot     │                          │
│  └──────────┘      Response     └──────────┘                          │
│                                                                         │
│  WebSocket (Full-Duplex)                                               │
│  ┌──────────┐                    ┌──────────┐                          │
│  │ Angular  │ ◄────────────────► │ Spring   │                          │
│  │   App    │    Real-time       │ Boot     │                          │
│  └──────────┘    bidirectional   └──────────┘                          │
│                                                                         │
│  Connection stays open!                                                 │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### WebSocket Architecture in Spring Boot

```
┌─────────────────────────────────────────────────────────────────────────┐
│              WEBSOCKET ARCHITECTURE                                      │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────┐                                                       │
│  │   STOMP     │ - Simple Text Oriented Messaging Protocol            │
│  │  Protocol   │ - Defines message format and routing                  │
│  └──────┬──────┘                                                       │
│         │                                                              │
│         ▼                                                              │
│  ┌─────────────┐                                                       │
│  │  WebSocket  │ - Spring WebSocket Message Broker                    │
│  │   Broker    │ - Manages connections and message routing           │
│  └──────┬──────┘                                                       │
│         │                                                              │
│         ▼                                                              │
│  ┌─────────────┐                                                       │
│  │  @Message   │ - Endpoint handlers                                  │
│  │  Mapping    │ - Business logic                                      │
│  └─────────────┘                                                       │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Adding WebSocket Dependencies

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-websocket</artifactId>
</dependency>
```

### WebSocket Configuration

```java
@Configuration
@EnableWebSocketMessageBroker
public class WebSocketConfig implements WebSocketMessageBrokerConfigurer {
    
    @Override
    public void configureMessageBroker(MessageBrokerRegistry config) {
        // Enable simple in-memory message broker
        config.enableSimpleBroker("/topic", "/queue");
        // Prefix for messages from clients
        config.setApplicationDestinationPrefixes("/app");
    }
    
    @Override
    public void registerStompEndpoints(StompEndpointRegistry registry) {
        // SockJS fallback options
        registry.addEndpoint("/ws")
            .setAllowedOrigins("*")
            .withSockJS();
        
        // WebSocket only endpoint
        registry.addEndpoint("/ws")
            .setAllowedOrigins("*");
    }
}
```

### Creating a WebSocket Controller

```java
@Controller
public class ChatController {
    
    // Send to specific user
    @MessageMapping("/chat")
    public void handleChatMessage(ChatMessage message, 
            SimpMessageHeaderAccessor headerAccessor) {
        
        // Store session info
        String sessionId = headerAccessor.getSessionId();
        
        // Process and broadcast message
        System.out.println("Received: " + message.getContent());
    }
    
    // Broadcast to all subscribers
    @MessageMapping("/broadcast")
    @SendTo("/topic/public")
    public ChatMessage broadcast(ChatMessage message) {
        return message;
    }
    
    // Send to specific user
    @MessageMapping("/private")
    public void privateMessage(ChatMessage message, 
            SimpMessageHeaderAccessor headerAccessor) {
        // Get username from session
        String username = headerAccessor.getUser().getName();
        
        // Send to specific user
        messagingTemplate.convertAndSendToUser(
            message.getTo(), 
            "/queue/messages", 
            message
        );
    }
}
```

### Notification Service

```java
@Service
public class NotificationService {
    
    private final SimpMessagingTemplate messagingTemplate;
    
    public NotificationService(SimpMessagingTemplate messagingTemplate) {
        this.messagingTemplate = messagingTemplate;
    }
    
    public void sendNotification(String userId, Notification notification) {
        messagingTemplate.convertAndSendToUser(
            userId,
            "/queue/notifications",
            notification
        );
    }
    
    public void broadcastNotification(Notification notification) {
        messagingTemplate.convertAndSend(
            "/topic/notifications",
            notification
        );
    }
}
```

## Angular WebSocket Integration

### Installing SockJS

```bash
npm install @stomp/ng2-stompjs sockjs-client
```

### Angular WebSocket Service

```typescript
import { Injectable } from '@angular/core';
import { Client } from '@stomp/ng2-stompjs';
import { Observable, Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class WebSocketService {
  private client: Client;
  private messagesSubject = new Subject<any>();
  
  public messages$ = this.messagesSubject.asObservable();

  constructor() {
    this.client = new Client({
      brokerURL: 'ws://localhost:8080/ws',
      connectHeaders: { login: 'guest', passcode: 'guest' },
      heartbeatIncoming: 5000,
      heartbeatOutgoing: 5000,
      reconnectDelay: 5000,
    });

    this.client.onConnect = () => {
      console.log('Connected to WebSocket');
      
      // Subscribe to topics
      this.client.subscribe('/topic/public', (message) => {
        this.messagesSubject.next(JSON.parse(message.body));
      });
      
      this.client.subscribe('/user/queue/notifications', (message) => {
        this.handleNotification(JSON.parse(message.body));
      });
    };

    this.client.activate();
  }

  sendMessage(message: any): void {
    this.client.publish({
      destination: '/app/broadcast',
      body: JSON.stringify(message)
    });
  }

  private handleNotification(notification: any): void {
    console.log('Notification received:', notification);
  }
}
```

### Using in Components

```typescript
import { Component, OnInit, OnDestroy } from '@angular/core';
import { WebSocketService } from './websocket.service';

@Component({ selector: 'app-chat', template: `<div>Chat Component</div>` })
export class ChatComponent implements OnInit, OnDestroy {
  constructor(private wsService: WebSocketService) {}

  ngOnInit() {
    this.wsService.messages$.subscribe(message => {
      console.log('Received:', message);
    });
  }

  ngOnDestroy() {
    // Cleanup if needed
  }
}
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Use STOMP over raw WebSockets** - Provides better message routing and structure

2. **Implement reconnection logic** - Handle connection drops gracefully

3. **Secure WebSocket connections** - Use WSS (WebSocket Secure) in production

4. **Limit message size** - Prevent memory issues with large payloads

5. **Implement heartbeats** - Detect dead connections

### Common Pitfalls

1. **Not handling reconnections** - Connections will drop unexpectedly

2. **Memory leaks** - Not unsubscribing from topics

3. **Broadcast storms** - Infinite message loops

4. **Security vulnerabilities** - Not validating WebSocket messages

## Student Hands-On Exercises

### Exercise 1: Basic WebSocket (Easy)
Set up WebSocket with simple broadcast functionality

### Exercise 2: Private Messages (Medium)
Implement private messaging between users

### Exercise 3: Real-Time Notifications (Hard)
Create a notification system that pushes updates to Angular

---

## Summary

In this lesson, you've learned:
- WebSocket fundamentals and architecture
- Spring Boot WebSocket configuration
- STOMP protocol and message routing
- Angular WebSocket integration
- Best practices for real-time applications

---

**Next Lesson**: In the next lesson, we'll explore Docker & Containerization.
