# Stream Data Protection and Privacy

## What You'll Learn

- AES-256-GCM authenticated encryption with PBKDF2 key derivation
- Stream data masking and redaction for GDPR compliance
- Stream data anonymization patterns
- Rate limiting streams to prevent data exfiltration
- Stream access control patterns
- Data classification streams (sensitive/public/internal)
- Stream audit logging for compliance
- Real-world GDPR-compliant log stream processor

## AES-256-GCM Encryption with PBKDF2 Key Derivation

```javascript
import { createCipheriv, createDecipheriv, randomBytes, pbkdf2Sync } from 'node:crypto';
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

class EncryptedStream extends Transform {
    constructor(password, salt = randomBytes(32)) {
        super();
        this.salt = salt;
        this.key = pbkdf2Sync(password, this.salt, 100_000, 32, 'sha512');
        this.iv = randomBytes(12);
        this.cipher = createCipheriv('aes-256-gcm', this.key, this.iv);
        this.push(Buffer.concat([this.salt, this.iv]));
    }

    _transform(chunk, encoding, callback) {
        callback(null, this.cipher.update(chunk));
    }

    _flush(callback) {
        this.push(this.cipher.final());
        this.push(this.cipher.getAuthTag());
        callback();
    }
}

class DecryptedStream extends Transform {
    constructor(password) {
        super();
        this.password = password;
        this.chunks = [];
    }

    _transform(chunk, encoding, callback) {
        this.chunks.push(chunk);
        callback();
    }

    _flush(callback) {
        const data = Buffer.concat(this.chunks);
        const salt = data.subarray(0, 32);
        const iv = data.subarray(32, 44);
        const authTag = data.subarray(-16);
        const encrypted = data.subarray(44, -16);
        const key = pbkdf2Sync(this.password, salt, 100_000, 32, 'sha512');

        try {
            const decipher = createDecipheriv('aes-256-gcm', key, iv);
            decipher.setAuthTag(authTag);
            callback(null, Buffer.concat([decipher.update(encrypted), decipher.final()]));
        } catch (err) {
            callback(new Error('Decryption failed: data integrity check failed'));
        }
    }
}

await pipeline(
    createReadStream('patient-records.csv'),
    new EncryptedStream(process.env.DATA_ENCRYPTION_KEY),
    createWriteStream('patient-records.enc')
);
```

## GDPR-Compliant PII Redaction Stream

```javascript
import { Transform } from 'node:stream';

const PII_PATTERNS = {
    email: { pattern: /[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}/g, token: '[EMAIL_REDACTED]' },
    ssn: { pattern: /\b\d{3}-\d{2}-\d{4}\b/g, token: '[SSN_REDACTED]' },
    phone: { pattern: /\b(\+?\d{1,3}[-.\s]?)?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}\b/g, token: '[PHONE_REDACTED]' },
    creditCard: { pattern: /\b\d{4}[-\s]?\d{4}[-\s]?\d{4}[-\s]?\d{4}\b/g, token: '[CARD_REDACTED]' },
    ipAddress: { pattern: /\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b/g, token: '[IP_REDACTED]' },
};

class PiiRedactionStream extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.enabledTypes = options.enabledTypes || Object.keys(PII_PATTERNS);
        this.customPatterns = options.customPatterns || [];
        this.redactedCount = 0;
    }

    _transform(chunk, encoding, callback) {
        let text = typeof chunk === 'string' ? chunk : chunk.toString();

        for (const type of this.enabledTypes) {
            const rule = PII_PATTERNS[type];
            if (rule) {
                const matches = text.match(rule.pattern);
                if (matches) this.redactedCount += matches.length;
                text = text.replace(rule.pattern, rule.token);
            }
        }

        for (const custom of this.customPatterns) {
            const matches = text.match(custom.pattern);
            if (matches) this.redactedCount += matches.length;
            text = text.replace(custom.pattern, custom.token);
        }

        callback(null, text);
    }

    _flush(callback) {
        this.emit('redaction-summary', { totalRedacted: this.redactedCount });
        callback();
    }
}

// Usage: Redact PII from log stream
import { createInterface } from 'node:readline';
import { createReadStream } from 'node:fs';

const redactor = new PiiRedactionStream({
    enabledTypes: ['email', 'ssn', 'creditCard'],
    customPatterns: [
        { pattern: /patient[_-]?id[:\s]+[\w-]+/gi, token: 'patient_id:[REDACTED]' },
    ],
});

redactor.on('redaction-summary', (s) => console.log(`Redacted ${s.totalRedacted} PII instances`));
const rl = createInterface({ input: createReadStream('access.log') });
rl.on('line', (line) => redactor.write(line));
rl.on('close', () => redactor.end());
redactor.pipe(process.stdout);
```

## Stream Data Anonymization

```javascript
import { createHash } from 'node:crypto';
import { Transform } from 'node:stream';

class AnonymizationStream extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.salt = options.salt || 'default-salt';
        this.fieldsToHash = new Set(options.fieldsToHash || ['email', 'userId']);
        this.fieldsToRemove = new Set(options.fieldsToRemove || ['ssn', 'creditCard']);
        this.generalizationMap = options.generalizationMap || {};
    }

    _hashValue(value) {
        return createHash('sha256')
            .update(String(value) + this.salt)
            .digest('hex')
            .subarray(0, 16)
            .toString('hex');
    }

    _generalize(field, value) {
        const rules = this.generalizationMap[field];
        if (!rules) return value;
        if (rules.type === 'range' && typeof value === 'number') {
            const bucket = Math.floor(value / rules.bucketSize) * rules.bucketSize;
            return `${bucket}-${bucket + rules.bucketSize - 1}`;
        }
        if (rules.type === 'date' && typeof value === 'string') {
            return value.subarray(0, 7); // YYYY-MM
        }
        return value;
    }

    _transform(record, encoding, callback) {
        const anonymized = {};
        for (const [key, value] of Object.entries(record)) {
            if (this.fieldsToRemove.has(key)) continue;
            if (this.fieldsToHash.has(key)) anonymized[key] = this._hashValue(value);
            else if (this.generalizationMap[key]) anonymized[key] = this._generalize(key, value);
            else anonymized[key] = value;
        }
        callback(null, anonymized);
    }
}

// Usage
import { pipeline } from 'node:stream/promises';

await pipeline(
    createReadStream('user-analytics.jsonl'),
    new JsonParseStream(),
    new AnonymizationStream({
        salt: process.env.ANON_SALT,
        fieldsToHash: ['userId', 'email'],
        fieldsToRemove: ['ssn', 'creditCard'],
        generalizationMap: {
            age: { type: 'range', bucketSize: 10 },
            signupDate: { type: 'date' },
        },
    }),
    new JsonStringifyStream(),
    createWriteStream('analytics-anonymized.jsonl')
);
```

## Rate Limiting Stream (Exfiltration Prevention)

```javascript
import { Transform } from 'node:stream';

class RateLimitStream extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.maxBytesPerSecond = options.maxBytesPerSecond || 1_000_000;
        this.maxRecordsPerMinute = options.maxRecordsPerMinute || 600;
        this.burstLimit = options.burstLimit || 50;
        this.bytesThisWindow = 0;
        this.recordsThisMinute = 0;
        this.burstCount = 0;
        this.windowStart = Date.now();
        this.minuteStart = Date.now();
        this.burstWindowStart = Date.now();
    }

    _transform(chunk, encoding, callback) {
        const now = Date.now();
        const size = Buffer.byteLength(JSON.stringify(chunk));

        if (now - this.burstWindowStart > 1000) { this.burstCount = 0; this.burstWindowStart = now; }
        if (now - this.minuteStart > 60_000) { this.recordsThisMinute = 0; this.minuteStart = now; }
        if (now - this.windowStart > 1000) { this.bytesThisWindow = 0; this.windowStart = now; }

        this.burstCount++;
        this.recordsThisMinute++;
        this.bytesThisWindow += size;

        if (this.burstCount > this.burstLimit) return callback(new Error('Burst limit exceeded'));
        if (this.recordsThisMinute > this.maxRecordsPerMinute) return callback(new Error('Record rate limit exceeded'));
        if (this.bytesThisWindow > this.maxBytesPerSecond) return callback(new Error('Bandwidth limit exceeded'));

        callback(null, chunk);
    }
}

// Usage
await pipeline(
    createReadStream('customer-data.jsonl'),
    new JsonParseStream(),
    new RateLimitStream({ maxBytesPerSecond: 5_000_000, maxRecordsPerMinute: 1000, burstLimit: 100 }),
    new JsonStringifyStream(),
    createWriteStream('export.jsonl')
);
```

## Stream Access Control and Data Classification

```javascript
import { Transform } from 'node:stream';

class AccessControlStream extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.requiredRole = options.requiredRole || 'reader';
        this.fieldPermissions = options.fieldPermissions || {};
        this.getContext = options.getContext || (() => ({}));
    }

    _transform(record, encoding, callback) {
        const context = this.getContext();
        if (!context.roles?.includes(this.requiredRole)) {
            return callback(null, { __accessDenied: true, __reason: 'insufficient_role' });
        }
        const filtered = {};
        for (const [field, value] of Object.entries(record)) {
            const requiredLevel = this.fieldPermissions[field];
            filtered[field] = (!requiredLevel || context.clearance >= requiredLevel)
                ? value : '[RESTRICTED]';
        }
        callback(null, filtered);
    }
}

function classifyRecord(record) {
    const values = Object.values(record).map(String).join(' ');
    if (/\b(ssn|credit.?card|password|secret.?key)\b/i.test(values)) return { level: 3, label: 'RESTRICTED' };
    if (/\b(salary|revenue|financial|medical)\b/i.test(values)) return { level: 2, label: 'CONFIDENTIAL' };
    if (/\b(internal|employee|department)\b/i.test(values)) return { level: 1, label: 'INTERNAL' };
    return { level: 0, label: 'PUBLIC' };
}

class DataClassificationStream extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.minLevel = options.minLevel || 0;
    }

    _transform(record, encoding, callback) {
        const classification = classifyRecord(record);
        if (classification.level < this.minLevel) return callback();
        callback(null, { ...record, __classification: classification.label, __level: classification.level });
    }
}
```

## Stream Audit Logging for Compliance

```javascript
import { Transform } from 'node:stream';
import { createHash } from 'node:crypto';
import { appendFile } from 'node:fs/promises';

class AuditLogStream extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.logFile = options.logFile || 'audit.log';
        this.actor = options.actor || 'system';
        this.action = options.action || 'data-access';
        this.recordCount = 0;
        this.buffer = [];
    }

    _transform(record, encoding, callback) {
        this.recordCount++;
        const fingerprint = createHash('sha256').update(JSON.stringify(record)).digest('hex').subarray(0, 16);

        this.buffer.push({
            timestamp: new Date().toISOString(),
            actor: this.actor,
            action: this.action,
            recordIndex: this.recordCount,
            fingerprint,
            fieldsPresent: Object.keys(record),
            classification: record.__classification || 'UNCLASSIFIED',
        });

        if (this.buffer.length >= 100) this._flushBuffer();
        callback(null, record);
    }

    async _flushBuffer() {
        if (this.buffer.length === 0) return;
        const entries = this.buffer.map((e) => JSON.stringify(e)).join('\n') + '\n';
        this.buffer = [];
        await appendFile(this.logFile, entries);
    }

    async _flush(callback) {
        await this._flushBuffer();
        this.emit('audit-summary', { totalRecords: this.recordCount, actor: this.actor });
        callback();
    }
}

// Usage: Audit-logged pipeline
const auditStream = new AuditLogStream({
    logFile: '/var/log/compliance/data-access.log',
    actor: 'etl-pipeline-v2',
    action: 'customer-data-export',
});

await pipeline(
    createReadStream('customers.jsonl'),
    new JsonParseStream(),
    new PiiRedactionStream({ enabledTypes: ['email', 'ssn'] }),
    new DataClassificationStream({ enrichOnly: true }),
    auditStream,
    new JsonStringifyStream(),
    createWriteStream('customers-export.jsonl')
);
```

## Real-World Example: GDPR-Compliant Log Stream Processor

```javascript
import { Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';
import { createInterface } from 'node:readline';

class LogLineParser extends Transform {
    constructor() { super({ objectMode: true }); }

    _transform(line, encoding, callback) {
        const match = line.match(/^(\d{4}-\d{2}-\d{2}T[\d:.]+Z)\s+(\w+)\s+(.*)$/);
        callback(null, match
            ? { timestamp: match[1], level: match[2], message: match[3] }
            : { raw: line, level: 'UNKNOWN', message: line }
        );
    }
}

class GdprLogProcessor extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.retentionDays = options.retentionDays || 90;
        this.stats = { total: 0, redacted: 0, dropped: 0 };
    }

    _isExpired(timestamp) {
        return new Date(timestamp) < new Date(Date.now() - this.retentionDays * 86_400_000);
    }

    _redact(message) {
        return message
            .replace(/[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}/g, '[EMAIL]')
            .replace(/\b\d{3}-\d{2}-\d{4}\b/g, '[SSN]')
            .replace(/\b\d{4}[-\s]?\d{4}[-\s]?\d{4}[-\s]?\d{4}\b/g, '[CARD]')
            .replace(/bearer\s+[\w.-]+/gi, 'bearer [TOKEN]')
            .replace(/password[=:]\s*\S+/gi, 'password=[REDACTED]');
    }

    _transform(record, encoding, callback) {
        this.stats.total++;
        if (record.timestamp && this._isExpired(record.timestamp)) {
            this.stats.dropped++;
            return callback();
        }

        const original = record.message || '';
        const redacted = this._redact(original);
        if (original !== redacted) this.stats.redacted++;

        callback(null, {
            ...record,
            message: redacted,
            __gdpr: { redacted: original !== redacted, processedAt: new Date().toISOString() },
        });
    }

    _flush(callback) {
        console.log(`GDPR: ${this.stats.total} records, ${this.stats.redacted} redacted, ${this.stats.dropped} expired`);
        callback();
    }
}

// Process raw logs through GDPR pipeline
const rl = createInterface({ input: createReadStream('application-raw.log'), crlfDelay: Infinity });
const processor = new GdprLogProcessor({ retentionDays: 90 });

rl.on('line', (line) => processor.write(line));
rl.on('close', () => processor.end());

await pipeline(
    processor,
    new JsonStringifyStream(),
    createWriteStream('application-gdpr-compliant.log')
);
```

## Best Practices Checklist

- [ ] Use AES-256-GCM with PBKDF2 (100k+ iterations) for encryption
- [ ] Redact PII before logging or exporting data
- [ ] Anonymize data for analytics (hash, generalize, suppress)
- [ ] Enforce rate limits on data export streams
- [ ] Implement field-level access control on sensitive data
- [ ] Classify data at ingestion time and route by sensitivity
- [ ] Maintain audit logs with record fingerprints for compliance
- [ ] Apply GDPR retention policies (auto-expire old records)
- [ ] Use unique salts per anonymization context
- [ ] Test redaction patterns against edge cases regularly

## Cross-References

- See [Stream Encryption, Validation](./01-encryption-validation.md) for base encryption patterns
- See [Transform Streams](../03-stream-processing-patterns/01-transform-streams.md) for Transform patterns
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for error propagation
- See [Buffer Security](../02-buffer-mastery/03-buffer-advanced-patterns.md) for buffer protection

## Next Steps

Continue to [Stream Security Architecture](./03-stream-security-architecture.md) to learn threat modeling and attack prevention.
