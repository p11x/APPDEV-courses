/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 07_Security_Patterns
 * Topic: 05_Encryption_Types
 * Purpose: Deep dive into Encryption Types with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Depends on algorithm
 * Security: Critical - protects data at rest/transit
 */

/**
 * Encryption Types - Comprehensive Guide
 * ======================================
 * 
 * WHAT: Patterns for encrypting data using various encryption algorithms
 * for data at rest and in transit.
 * 
 * WHY:
 * - Protect sensitive data
 * - Meet compliance requirements
 * - Ensure privacy
 * - Data integrity
 * 
 * HOW:
 * - Use symmetric encryption for data at rest
 * - Use asymmetric for key exchange
 * - Use TLS for data in transit
 * - Hash for integrity
 */

// ============================================================================
// SECTION 1: SYMMETRIC ENCRYPTION
// ============================================================================

interface Encryptor {
  encrypt(plaintext: string): Promise<string>;
  decrypt(ciphertext: string): Promise<string>;
}

class AESEncryptor implements Encryptor {
  private key: CryptoKey;
  
  constructor(password: string) {
    this.key = {} as CryptoKey;
  }
  
  async encrypt(plaintext: string): Promise<string> {
    const encoder = new TextEncoder();
    const data = encoder.encode(plaintext);
    
    const iv = crypto.getRandomValues(new Uint8Array(12));
    const encrypted = await crypto.subtle.encrypt(
      { name: "AES-GCM", iv },
      this.key,
      data
    );
    
    const combined = new Uint8Array(iv.length + encrypted.byteLength);
    combined.set(iv);
    combined.set(new Uint8Array(encrypted), iv.length);
    
    return btoa(String.fromCharCode(...combined));
  }
  
  async decrypt(ciphertext: string): Promise<string> {
    const combined = Uint8Array.from(atob(ciphertext), c => c.charCodeAt(0));
    const iv = combined.slice(0, 12);
    const encrypted = combined.slice(12);
    
    const decrypted = await crypto.subtle.decrypt(
      { name: "AES-GCM", iv },
      this.key,
      encrypted
    );
    
    return new TextDecoder().decode(decrypted);
  }
}

// ============================================================================
// SECTION 2: HASHING
// ============================================================================

interface Hasher {
  hash(data: string): Promise<string>;
  verify(data: string, hash: string): Promise<boolean>;
}

class SHA256Hasher implements Hasher {
  async hash(data: string): Promise<string> {
    const encoder = new TextEncoder();
    const hashBuffer = await crypto.subtle.digest(
      "SHA-256",
      encoder.encode(data)
    );
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    return hashArray.map(b => b.toString(16).padStart(2, "0")).join("");
  }
  
  async verify(data: string, hash: string): Promise<boolean> {
    const dataHash = await this.hash(data);
    return dataHash === hash;
  }
}

// ============================================================================
// SECTION 3: DIGITAL SIGNATURES
// ============================================================================

interface Signer {
  sign(data: string): Promise<string>;
  verify(data: string, signature: string): Promise<boolean>;
}

class RSASigner implements Signer {
  private keyPair: CryptoKeyPair;
  
  constructor() {
    this.keyPair = {} as CryptoKeyPair;
  }
  
  async sign(data: string): Promise<string> {
    const encoder = new TextEncoder();
    const signature = await crypto.subtle.sign(
      "RS256",
      this.keyPair.privateKey,
      encoder.encode(data)
    );
    
    return btoa(String.fromCharCode(...new Uint8Array(signature)));
  }
  
  async verify(data: string, signature: string): Promise<boolean> {
    const encoder = new TextEncoder();
    const sigBytes = Uint8Array.from(atob(signature), c => c.charCodeAt(0));
    
    return crypto.subtle.verify(
      "RS256",
      this.keyPair.publicKey,
      sigBytes,
      encoder.encode(data)
    );
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Encryption Performance:
// - AES fast, RSA slower
// - Hashing fast
// - Key size affects performance

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Compatible with:
// - TypeScript 1.6+
// - All ES targets
// - Node.js and browsers (Web Crypto API)

// ============================================================================
// SECURITY CONSIDERATIONS
// ============================================================================

// Security critical:
// - Use strong algorithms (AES-256, RSA-2048+)
// - Protect keys
// - Use random IVs

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing encryption:
// 1. Test encryption/decryption
// 2. Test hashing
// 3. Test signatures

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Authentication (03_Authentication_Patterns.ts)
// - Authorization (04_Authorization_Patterns.ts)
// - Input Validation (01_Input_Validation.ts)

// End of Security Patterns
