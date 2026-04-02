# Stream Encryption, Validation, and Data Protection

## What You'll Learn

- Stream data encryption and decryption
- Stream data validation and sanitization
- Data integrity verification
- Secure stream patterns
- Compliance and standards

## Authenticated Encryption Streams

```javascript
import { createCipheriv, createDecipheriv, randomBytes, createHmac } from 'node:crypto';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

class AesGcmEncryptStream extends Transform {
    constructor(key) {
        super();
        this.key = key;
        this.iv = randomBytes(12); // GCM uses 12-byte IV
        this.cipher = createCipheriv('aes-256-gcm', this.key, this.iv);
        this.push(this.iv); // Prepend IV to output
    }

    _transform(chunk, encoding, callback) {
        const encrypted = this.cipher.update(chunk);
        callback(null, encrypted);
    }

    _flush(callback) {
        this.push(this.cipher.final());
        this.push(this.cipher.getAuthTag()); // Append auth tag
        callback();
    }
}

class AesGcmDecryptStream extends Transform {
    constructor(key) {
        super();
        this.key = key;
        this.chunks = [];
        this.iv = null;
        this.authTag = null;
    }

    _transform(chunk, encoding, callback) {
        this.chunks.push(chunk);
        callback();
    }

    _flush(callback) {
        const allData = Buffer.concat(this.chunks);
        
        // Extract IV (first 12 bytes), auth tag (last 16 bytes), data (middle)
        this.iv = allData.slice(0, 12);
        this.authTag = allData.slice(-16);
        const encrypted = allData.slice(12, -16);

        try {
            const decipher = createDecipheriv('aes-256-gcm', this.key, this.iv);
            decipher.setAuthTag(this.authTag);
            const decrypted = Buffer.concat([decipher.update(encrypted), decipher.final()]);
            callback(null, decrypted);
        } catch (err) {
            callback(new Error('Decryption failed: data may be tampered'));
        }
    }
}

// Usage
const key = randomBytes(32);

await pipeline(
    createReadStream('sensitive.txt'),
    new AesGcmEncryptStream(key),
    createWriteStream('encrypted.bin')
);

await pipeline(
    createReadStream('encrypted.bin'),
    new AesGcmDecryptStream(key),
    createWriteStream('decrypted.txt')
);
```

## HMAC Integrity Verification

```javascript
import { createHmac, createHash } from 'node:crypto';
import { Transform, pipeline } from 'node:stream';

class HmacSignStream extends Transform {
    constructor(secret, algorithm = 'sha256') {
        super();
        this.hmac = createHmac(algorithm, secret);
    }

    _transform(chunk, encoding, callback) {
        this.hmac.update(chunk);
        callback(null, chunk); // Pass through
    }

    _flush(callback) {
        this.signature = this.hmac.digest('hex');
        this.emit('signature', this.signature);
        callback();
    }
}

class HmacVerifyStream extends Transform {
    constructor(secret, expectedSignature, algorithm = 'sha256') {
        super();
        this.hmac = createHmac(algorithm, secret);
        this.expectedSignature = expectedSignature;
    }

    _transform(chunk, encoding, callback) {
        this.hmac.update(chunk);
        callback(null, chunk);
    }

    _flush(callback) {
        const actualSignature = this.hmac.digest('hex');
        if (actualSignature !== this.expectedSignature) {
            callback(new Error('HMAC verification failed: data integrity compromised'));
        } else {
            callback(null);
        }
    }
}

// Hash verification stream
class HashVerifyStream extends Transform {
    constructor(expectedHash, algorithm = 'sha256') {
        super();
        this.hash = createHash(algorithm);
        this.expectedHash = expectedHash;
    }

    _transform(chunk, encoding, callback) {
        this.hash.update(chunk);
        callback(null, chunk);
    }

    _flush(callback) {
        const actual = this.hash.digest('hex');
        if (actual !== this.expectedHash) {
            callback(new Error(`Hash mismatch: expected ${this.expectedHash}, got ${actual}`));
        } else {
            callback();
        }
    }
}
```

## Data Sanitization Stream

```javascript
import { Transform } from 'node:stream';

class SanitizeStream extends Transform {
    constructor(rules = {}) {
        super({ objectMode: true });
        this.rules = {
            stripHtml: rules.stripHtml !== false,
            maxFieldLength: rules.maxFieldLength || 10000,
            allowedFields: rules.allowedFields || null,
            removePatterns: rules.removePatterns || [],
        };
    }

    _transform(record, encoding, callback) {
        const sanitized = {};

        for (const [key, value] of Object.entries(record)) {
            if (this.rules.allowedFields && !this.rules.allowedFields.includes(key)) {
                continue; // Skip disallowed fields
            }

            let clean = String(value);

            // Strip HTML
            if (this.rules.stripHtml) {
                clean = clean.replace(/<[^>]*>/g, '');
            }

            // Remove dangerous patterns
            for (const pattern of this.rules.removePatterns) {
                clean = clean.replace(pattern, '');
            }

            // Truncate
            if (clean.length > this.rules.maxFieldLength) {
                clean = clean.slice(0, this.rules.maxFieldLength);
            }

            sanitized[key] = clean;
        }

        callback(null, sanitized);
    }
}

// Usage
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

await pipeline(
    createReadStream('user-input.jsonl'),
    new JsonParseStream(),
    new SanitizeStream({
        stripHtml: true,
        maxFieldLength: 1000,
        allowedFields: ['name', 'email', 'message'],
        removePatterns: [/javascript:/gi, /on\w+=/gi],
    }),
    new JsonStringifyStream(),
    createWriteStream('sanitized.jsonl')
);
```

## Best Practices Checklist

- [ ] Use AES-256-GCM for authenticated encryption
- [ ] Verify HMAC/hash before processing data
- [ ] Sanitize all user input streams
- [ ] Use timing-safe comparison for signatures
- [ ] Store IV with encrypted data (not secret)
- [ ] Rotate encryption keys regularly
- [ ] Log security events for auditing

## Cross-References

- See [Compression and Encryption](../03-stream-processing-patterns/03-compression-encryption.md) for combined patterns
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for error patterns
- See [Buffer Security](../02-buffer-mastery/03-buffer-advanced-patterns.md) for buffer security

## Next Steps

Continue to [Modern Stream Technologies](../10-modern-stream-technologies/01-express-fastify-integration.md).
