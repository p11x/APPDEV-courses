/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 07_Security_Patterns
 * Topic: 03_Authentication_Patterns
 * Purpose: Deep dive into Authentication Patterns with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Depends on implementation
 * Security: Critical - prevents unauthorized access
 */

/**
 * Authentication Patterns - Comprehensive Guide
 * ============================================
 * 
 * WHAT: Patterns for verifying user identity through various methods including
 * passwords, tokens, and biometrics.
 * 
 * WHY:
 * - Verify user identity
 * - Protect resources
 * - Enable personalization
 * - Audit trails
 * 
 * HOW:
 * - Store password hashes
 * - Use secure tokens
 * - Implement MFA
 * - Handle sessions securely
 */

// ============================================================================
// SECTION 1: PASSWORD HASHING
// ============================================================================

interface PasswordHasher {
  hash(password: string): Promise<string>;
  verify(password: string, hash: string): Promise<boolean>;
}

class SimplePasswordHasher implements PasswordHasher {
  async hash(password: string): Promise<string> {
    const encoder = new TextEncoder();
    const data = encoder.encode(password);
    const hashBuffer = await crypto.subtle.digest("SHA-256", data);
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    return hashArray.map(b => b.toString(16).padStart(2, "0")).join("");
  }
  
  async verify(password: string, hash: string): Promise<boolean> {
    const passwordHash = await this.hash(password);
    return passwordHash === hash;
  }
}

// ============================================================================
// SECTION 2: TOKEN AUTHENTICATION
// ============================================================================

interface TokenService {
  createToken(userId: string): Promise<string>;
  verifyToken(token: string): Promise<TokenPayload>;
}

interface TokenPayload {
  userId: string;
  exp: number;
  iat: number;
}

class JwtTokenService implements TokenService {
  private secret: string;
  
  constructor(secret: string) {
    this.secret = secret;
  }
  
  async createToken(userId: string): Promise<string> {
    const header = btoa(JSON.stringify({ alg: "HS256", typ: "JWT" }));
    const payload = btoa(JSON.stringify({
      userId,
      iat: Date.now(),
      exp: Date.now() + 3600000
    }));
    const signature = await this.sign(`${header}.${payload}`);
    
    return `${header}.${payload}.${signature}`;
  }
  
  async verifyToken(token: string): Promise<TokenPayload> {
    const [header, payload, signature] = token.split(".");
    
    const validSignature = await this.sign(`${header}.${payload}`);
    if (signature !== validSignature) {
      throw new Error("Invalid signature");
    }
    
    const decoded = JSON.parse(atob(payload));
    
    if (decoded.exp < Date.now()) {
      throw new Error("Token expired");
    }
    
    return decoded;
  }
  
  private async sign(data: string): Promise<string> {
    const encoder = new TextEncoder();
    const key = await crypto.subtle.importKey(
      "raw",
      encoder.encode(this.secret),
      { name: "HMAC", hash: "SHA-256" },
      false,
      ["sign"]
    );
    const signature = await crypto.subtle.sign("HMAC", key, encoder.encode(data));
    return btoa(String.fromCharCode(...new Uint8Array(signature)));
  }
}

// ============================================================================
// SECTION 3: SESSION MANAGEMENT
// ============================================================================

interface Session {
  id: string;
  userId: string;
  createdAt: Date;
  expiresAt: Date;
}

class SessionManager {
  private sessions: Map<string, Session> = new Map();
  
  create(userId: string, ttl: number = 3600000): Session {
    const session: Session = {
      id: crypto.randomUUID(),
      userId,
      createdAt: new Date(),
      expiresAt: new Date(Date.now() + ttl)
    };
    
    this.sessions.set(session.id, session);
    return session;
  }
  
  get(sessionId: string): Session | null {
    const session = this.sessions.get(sessionId);
    
    if (!session) return null;
    
    if (session.expiresAt < new Date()) {
      this.sessions.delete(sessionId);
      return null;
    }
    
    return session;
  }
  
  destroy(sessionId: string): void {
    this.sessions.delete(sessionId);
  }
  
  cleanup(): void {
    const now = new Date();
    for (const [id, session] of this.sessions) {
      if (session.expiresAt < now) {
        this.sessions.delete(id);
      }
    }
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Authentication Performance:
// - Hashing is CPU intensive
// - Token verification fast
// - Session cleanup important

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Compatible with:
// - TypeScript 1.6+
// - All ES targets
// - Node.js and browsers

// ============================================================================
// SECURITY CONSIDERATIONS
// ============================================================================

// Security critical:
// - Use strong hashing (bcrypt, Argon2)
// - Secure token storage
// - Session timeout

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing authentication:
// 1. Test password hashing
// 2. Test token creation/verification
// 3. Test session management

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Input Validation (01_Input_Validation.ts)
// - Authorization (04_Authorization_Patterns.ts)
// - Encryption (05_Encryption_Types.ts)

// Next: 04_Authorization_Patterns.ts
