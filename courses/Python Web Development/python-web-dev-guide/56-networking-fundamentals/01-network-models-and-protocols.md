# Network Models and Protocols

## What You'll Learn

- OSI and TCP/IP models
- Common protocols (HTTP, TCP, IP)
- DNS, DHCP basics
- Ports and sockets

## Prerequisites

- Completed folder 55 (Linux and Server Administration)

## OSI Model

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    OSI MODEL (7 LAYERS)                                      │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  7. Application    → HTTP, DNS, FTP                                        │
│  6. Presentation  → SSL/TLS, JPEG, PNG                                    │
│  5. Session       → RPC, NetBIOS                                           │
│  4. Transport     → TCP, UDP                                               │
│  3. Network       → IP, ICMP, Router                                      │
│  2. Data Link    → Ethernet, MAC Address                                  │
│  1. Physical      → Cables, Switches                                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## TCP vs UDP

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    TCP VS UDP                                                │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  TCP (Transmission Control Protocol):                                       │
│  • Connection-oriented                                                      │
│  • Reliable (guaranteed delivery)                                           │
│  • Ordered                                                                 │
│  • Slower                                                                  │
│  • Use for: HTTP, SSH, Email                                              │
│                                                                             │
│  UDP (User Datagram Protocol):                                             │
│  • Connectionless                                                           │
│  • Unreliable (no guarantee)                                              │
│  • Faster                                                                  │
│  • Use for: Video streaming, DNS, VoIP                                    │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Common Protocols

| Protocol | Port | Purpose |
|----------|------|---------|
| HTTP | 80 | Web |
| HTTPS | 443 | Secure web |
| SSH | 22 | Secure shell |
| FTP | 21 | File transfer |
| DNS | 53 | Domain names |
| SMTP | 25 | Email send |
| IMAP | 143 | Email receive |

## Summary

- OSI model has 7 layers
- TCP is reliable, UDP is fast
- HTTP, HTTPS, SSH, FTP are common protocols

## Next Steps

→ Continue to more advanced networking topics in this folder.
