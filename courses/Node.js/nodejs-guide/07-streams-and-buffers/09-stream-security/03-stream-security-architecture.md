# Stream Security Architecture and Threat Mitigation

## What You'll Learn

- STRIDE threat model applied to stream processing
- Stream injection attack prevention techniques
- Stream size limit enforcement for DoS prevention
- Stream input validation and sanitization patterns
- Certificate pinning for TLS streams
- Stream integrity verification with HMAC chains
- Security monitoring for stream anomalies
- Incident response for stream-based attacks
- Real-world secure data pipeline architecture

## STRIDE Threat Model for Stream Processing

```javascript
/**
 * STRIDE applied to Node.js streams:
 * S - Spoofing: Forged data injected into readable streams
 * T - Tampering: Data modified in transit through transforms
 * R - Repudiation: No audit trail for data sent/received
 * I - Info Disclosure: Sensitive data leaked via unencrypted streams
 * D - Denial of Service: Oversized payloads overwhelm streams
 * E - Elevation: Unauthorized access through stream side-channels
 */

import { Transform } from 'node:stream';
import { createHash } from 'node:crypto';

class ThreatAwareStream extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.maxPayloadBytes = options.maxPayloadBytes || 10_000_000;
        this.maxFieldCount = options.maxFieldCount || 100;
        this.maxNestingDepth = options.maxNestingDepth || 10;
        this.seen = new Set();
        this.bytesProcessed = 0;
    }

    _validateStructure(obj, depth = 0) {
        if (depth > this.maxNestingDepth) throw new Error('Structure too deeply nested');
        if (Array.isArray(obj)) { for (const item of obj) this._validateStructure(item, depth + 1); }
        else if (obj && typeof obj === 'object') {
            if (Object.keys(obj).length > this.maxFieldCount) throw new Error('Field count exceeds limit');
            for (const value of Object.values(obj)) this._validateStructure(value, depth + 1);
        }
    }

    _transform(record, encoding, callback) {
        try {
            this.bytesProcessed += Buffer.byteLength(JSON.stringify(record));
            if (this.bytesProcessed > this.maxPayloadBytes) return callback(new Error('Payload size limit exceeded'));
            const id = createHash('md5').update(JSON.stringify(record)).digest('hex');
            if (this.seen.has(id)) return callback(); // Replay prevention
            this.seen.add(id);
            this._validateStructure(record);
            callback(null, record);
        } catch (err) { callback(err); }
    }
}
```

## Stream Injection Attack Prevention

```javascript
import { Transform } from 'node:stream';

// Strip CR/LF to prevent HTTP header injection
class CrlfSanitizer extends Transform {
    constructor() { super({ decodeStrings: false }); }
    _transform(chunk, encoding, callback) {
        if (typeof chunk === 'string') callback(null, chunk.replace(/[\r\n]/g, ''));
        else callback(null, Buffer.from(chunk).filter((b) => b !== 0x0d && b !== 0x0a));
    }
}

// Sanitize control characters to prevent log injection
class LogInjectionGuard extends Transform {
    constructor() { super({ objectMode: true }); }

    _sanitize(value) {
        if (typeof value !== 'string') return value;
        return value.replace(/[\x00-\x08\x0b\x0c\x0e-\x1f\x7f]/g, '')
            .replace(/\n/g, '\\n').replace(/\r/g, '\\r');
    }

    _transform(record, encoding, callback) {
        const sanitized = {};
        for (const [key, value] of Object.entries(record)) {
            sanitized[this._sanitize(key)] = Array.isArray(value)
                ? value.map((v) => this._sanitize(v)) : this._sanitize(value);
        }
        callback(null, sanitized);
    }
}

// Block template injection patterns ({{}}, ${}, <% %>)
class TemplateInjectionGuard extends Transform {
    constructor() { super({ objectMode: true }); }
    _transform(record, encoding, callback) {
        for (const [key, value] of Object.entries(record)) {
            if (typeof value === 'string' && /\{\{.*\}\}|\$\{.*\}|<%.*%>/.test(value)) {
                return callback(new Error(`Template injection in field "${key}"`));
            }
        }
        callback(null, record);
    }
}

// Usage: Layered injection prevention pipeline
import { pipeline } from 'node:stream/promises';
import { createInterface } from 'node:readline';
import { createReadStream, createWriteStream } from 'node:fs';

const rl = createInterface({ input: createReadStream('user-logs.raw') });
const sanitizer = new CrlfSanitizer();
rl.on('line', (line) => sanitizer.write(line));
rl.on('close', () => sanitizer.end());

await pipeline(sanitizer, new JsonParseStream(), new LogInjectionGuard(),
    new TemplateInjectionGuard(), new JsonStringifyStream(),
    createWriteStream('sanitized-logs.jsonl'));
```

## Stream Size Limit Enforcement (DoS Prevention)

```javascript
import { Transform } from 'node:stream';

class StreamSizeLimiter extends Transform {
    constructor(options = {}) {
        super();
        this.maxBytes = options.maxBytes || 50_000_000;
        this.maxChunkSize = options.maxChunkSize || 1_048_576;
        this.maxChunks = options.maxChunks || 10_000;
        this.bytesReceived = 0;
        this.chunkCount = 0;
    }

    _transform(chunk, encoding, callback) {
        this.chunkCount++;
        if (chunk.length > this.maxChunkSize) return callback(new Error('Chunk size exceeds maximum'));
        if (this.chunkCount > this.maxChunks) return callback(new Error('Chunk count limit exceeded'));
        this.bytesReceived += chunk.length;
        if (this.bytesReceived > this.maxBytes) return callback(new Error('Stream size exceeds maximum'));
        callback(null, chunk);
    }
}

// Timeout guard: kill streams that stall or run too long
class StreamTimeoutGuard extends Transform {
    constructor(options = {}) {
        super();
        this.timeoutMs = options.timeoutMs || 30_000;
        this.idleTimeoutMs = options.idleTimeoutMs || 5_000;
        this.startTime = Date.now();
        this._resetIdleTimer();
    }

    _resetIdleTimer() {
        clearTimeout(this.idleTimer);
        this.idleTimer = setTimeout(() => this.destroy(new Error('Idle timeout')), this.idleTimeoutMs);
    }

    _transform(chunk, encoding, callback) {
        if (Date.now() - this.startTime > this.timeoutMs) return callback(new Error('Total timeout exceeded'));
        this._resetIdleTimer();
        callback(null, chunk);
    }

    destroy(err) { clearTimeout(this.idleTimer); super.destroy(err); }
}

// Usage: Protect against upload DoS
import { createServer } from 'node:http';

createServer((req, res) => {
    req.pipe(new StreamSizeLimiter({ maxBytes: 10_000_000 }))
       .pipe(new StreamTimeoutGuard({ timeoutMs: 60_000 }))
       .on('error', (err) => { res.writeHead(413); res.end(err.message); })
       .pipe(createWriteStream(`uploads/${Date.now()}.dat`))
       .on('finish', () => { res.writeHead(200); res.end('OK'); });
}).listen(3000);
```

## Stream Input Validation

```javascript
import { Transform } from 'node:stream';

class SchemaValidationStream extends Transform {
    constructor(schema) {
        super({ objectMode: true });
        this.schema = schema;
    }

    _validate(record) {
        const errors = [];
        for (const [field, rules] of Object.entries(this.schema)) {
            const value = record[field];
            if (rules.required && (value === undefined || value === null)) { errors.push(`Missing: ${field}`); continue; }
            if (value === undefined || value === null) continue;
            if (rules.type && typeof value !== rules.type) errors.push(`"${field}" type mismatch`);
            if (rules.maxLength && typeof value === 'string' && value.length > rules.maxLength) errors.push(`"${field}" too long`);
            if (rules.enum && !rules.enum.includes(value)) errors.push(`"${field}" invalid value`);
        }
        for (const field of Object.keys(record)) {
            if (!this.schema[field]) errors.push(`Unknown field: ${field}`); // Whitelist
        }
        return errors;
    }

    _transform(record, encoding, callback) {
        const errors = this._validate(record);
        if (errors.length > 0) {
            this.emit('validation-error', { record, errors });
            return callback(); // Drop invalid records
        }
        callback(null, record);
    }
}

const validator = new SchemaValidationStream({
    email: { type: 'string', required: true, maxLength: 200 },
    name: { type: 'string', required: true, maxLength: 200 },
    role: { type: 'string', enum: ['user', 'admin'] },
});
```

## Certificate Pinning for TLS Streams

```javascript
import { connect } from 'node:tls';
import { createHash } from 'node:crypto';

class PinnedTlsStream {
    constructor(options = {}) {
        this.host = options.host;
        this.port = options.port || 443;
        this.pinnedPublicKeys = new Set(options.pinnedPublicKeys || []);
    }

    connect() {
        return new Promise((resolve, reject) => {
            const socket = connect({ host: this.host, port: this.port, rejectUnauthorized: true });
            socket.on('secureConnect', () => {
                const cert = socket.getPeerCertificate(true);
                const pin = createHash('sha256').update(cert.pubkey).digest('base64');
                if (this.pinnedPublicKeys.size > 0 && !this.pinnedPublicKeys.has(pin)) {
                    socket.destroy();
                    return reject(new Error('Certificate pin mismatch'));
                }
                resolve(socket);
            });
            socket.on('error', reject);
        });
    }
}

// Usage
const pinned = new PinnedTlsStream({ host: 'api.secure-service.com',
    pinnedPublicKeys: ['sha256/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA='] });
const socket = await pinned.connect();
```

## Stream Integrity Verification with HMAC Chains

```javascript
import { createHmac } from 'node:crypto';
import { Transform } from 'node:stream';

class HmacChainSigner extends Transform {
    constructor(secret, algorithm = 'sha256') {
        super({ objectMode: true });
        this.secret = secret;
        this.algorithm = algorithm;
        this.chainIndex = 0;
        this.previousHmac = null;
    }

    _transform(record, encoding, callback) {
        const hmac = createHmac(this.algorithm, this.secret);
        hmac.update(Buffer.from(String(this.chainIndex)));
        hmac.update(Buffer.from(JSON.stringify(record)));
        if (this.previousHmac) hmac.update(Buffer.from(this.previousHmac));
        const signature = hmac.digest('hex');
        callback(null, {
            data: record,
            __integrity: { index: this.chainIndex, hmac: signature, previousHmac: this.previousHmac },
        });
        this.previousHmac = signature;
        this.chainIndex++;
    }
}

class HmacChainVerifier extends Transform {
    constructor(secret, algorithm = 'sha256') {
        super({ objectMode: true });
        this.secret = secret;
        this.algorithm = algorithm;
        this.expectedIndex = 0;
        this.previousHmac = null;
    }

    _transform(record, encoding, callback) {
        try {
            const { data, __integrity: integ } = record;
            if (integ.index !== this.expectedIndex) throw new Error(`Sequence gap at ${integ.index}`);
            if (integ.previousHmac !== this.previousHmac) throw new Error(`Chain break at ${integ.index}`);
            const hmac = createHmac(this.algorithm, this.secret);
            hmac.update(Buffer.from(String(integ.index)));
            hmac.update(Buffer.from(JSON.stringify(data)));
            if (integ.previousHmac) hmac.update(Buffer.from(integ.previousHmac));
            if (hmac.digest('hex') !== integ.hmac) throw new Error(`Data tampered at ${integ.index}`);
            this.previousHmac = integ.hmac;
            this.expectedIndex++;
            callback(null, data);
        } catch (err) {
            this.emit('integrity-violation', { index: record.__integrity?.index, error: err.message });
            callback(err);
        }
    }
}
```

## Security Monitoring and Incident Response

```javascript
import { Transform } from 'node:stream';
import { appendFile } from 'node:fs/promises';
import { createHash } from 'node:crypto';

// Detect anomalous stream behavior (burst, bandwidth spikes)
class AnomalyDetector extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.maxBytesPerSecond = options.maxBytesPerSecond || 10_000_000;
        this.maxBurstPerSecond = options.maxBurstPerSecond || 1000;
        this.byteWindow = [];
        this.timeWindow = [];
    }

    _transform(chunk, encoding, callback) {
        const now = Date.now();
        const size = Buffer.byteLength(JSON.stringify(chunk));
        this.byteWindow = [...this.byteWindow.filter((e) => now - e.time < 1000), { bytes: size, time: now }];
        this.timeWindow = [...this.timeWindow.filter((t) => now - t < 1000), now];
        const byteRate = this.byteWindow.reduce((s, e) => s + e.bytes, 0);
        if (byteRate > this.maxBytesPerSecond) this.emit('anomaly', { type: 'HIGH_BANDWIDTH', byteRate });
        if (this.timeWindow.length > this.maxBurstPerSecond) this.emit('anomaly', { type: 'BURST', rate: this.timeWindow.length });
        callback(null, chunk);
    }
}

// Circuit breaker for incident response
class IncidentResponseStream extends Transform {
    constructor(options = {}) {
        super({ objectMode: true });
        this.incidentLog = options.incidentLog || 'stream-incidents.jsonl';
        this.maxIncidents = options.maxIncidents || 10;
        this.incidents = [];
        this.circuitOpen = false;
        this.onAlert = options.onAlert || (() => {});
    }

    async _logIncident(type, details, record) {
        const incident = {
            id: `INC-${Date.now()}-${Math.random().toString(36).subarray(2, 8)}`,
            type, details,
            fingerprint: record ? createHash('sha256').update(JSON.stringify(record)).digest('hex').subarray(0, 16) : null,
            timestamp: new Date().toISOString(),
            severity: { INJECTION_DETECTED: 'CRITICAL', INTEGRITY_VIOLATION: 'CRITICAL' }[type] || 'MEDIUM',
        };
        this.incidents.push(incident);
        await appendFile(this.incidentLog, JSON.stringify(incident) + '\n');
        if (this.incidents.length >= this.maxIncidents) {
            this.circuitOpen = true;
            this.emit('circuit-open', { totalIncidents: this.incidents.length });
        }
        return incident;
    }

    async _transform(record, encoding, callback) {
        if (this.circuitOpen) return callback(new Error('Circuit breaker open'));
        const str = JSON.stringify(record);
        const issues = [];
        if (/(\{\{|\$\{)/.test(str)) issues.push({ type: 'INJECTION_DETECTED', details: 'Template syntax' });
        if (/[\x00-\x08\x0e-\x1f]/.test(str)) issues.push({ type: 'INJECTION_DETECTED', details: 'Control chars' });
        if (issues.length > 0) {
            for (const issue of issues) this.onAlert(await this._logIncident(issue.type, issue.details, record));
            if (issues.some((i) => i.type === 'INJECTION_DETECTED')) return callback();
        }
        callback(null, record);
    }
}

// Usage
const handler = new IncidentResponseStream({
    incidentLog: '/var/log/security/incidents.jsonl',
    onAlert: (incident) => { if (incident.severity === 'CRITICAL') notifyOpsTeam(incident); },
});
handler.on('circuit-open', ({ totalIncidents }) => process.exit(1));
```

## Real-World Security Architecture: Secure Data Pipeline

```javascript
import { Transform, pipeline } from 'node:stream';
import { createReadStream, createWriteStream } from 'node:fs';

/**
 * Secure Data Pipeline Architecture
 *
 * ┌──────────┐    ┌──────────────┐    ┌──────────────┐    ┌─────────────┐
 * │  Source   │───>│ Size Limiter │───>│  Validator   │───>│  Sanitizer  │
 * └──────────┘    └──────────────┘    └──────────────┘    └─────────────┘
 *                                                                │
 *               ┌──────────────┐    ┌──────────────┐            │
 *               │  Output Sink │<───│ HMAC Signer  │<───┌───────┴──────┐
 *               └──────────────┘    └──────────────┘    │ Classifier   │
 *                        │                              └──────────────┘
 *                        │              ┌─────────────┐
 *                        │              │ Anomaly Det.│──> Audit Log
 *                        │              └─────────────┘
 */

class SecurePipelineBuilder {
    constructor() { this.middlewares = []; }
    addSizeLimit(maxBytes) { this.middlewares.push(new StreamSizeLimiter({ maxBytes })); return this; }
    addSchemaValidation(schema) { this.middlewares.push(new SchemaValidationStream(schema)); return this; }
    addSanitization() { this.middlewares.push(new LogInjectionGuard(), new TemplateInjectionGuard()); return this; }
    addHmacSigning(secret) { this.middlewares.push(new HmacChainSigner(secret)); return this; }
    addMonitoring(opts) { this.middlewares.push(new AnomalyDetector(opts)); return this; }
    build() { return this.middlewares; }
}

const securePipeline = new SecurePipelineBuilder()
    .addSizeLimit(100_000_000)
    .addSchemaValidation({ userId: { type: 'string', required: true }, action: { type: 'string', enum: ['view', 'update'] } })
    .addSanitization().addHmacSigning(process.env.HMAC_SECRET)
    .addMonitoring({ maxBytesPerSecond: 20_000_000, maxBurstPerSecond: 2000 }).build();

for (const mw of securePipeline) {
    if (mw instanceof AnomalyDetector) mw.on('anomaly', (a) => securityOps.alert('stream_anomaly', a));
}

await pipeline(createReadStream('input-events.jsonl'), new JsonParseStream(), ...securePipeline,
    new JsonStringifyStream(), createWriteStream('output-secure.jsonl'));
```

## Best Practices Checklist

- [ ] Model stream threats using STRIDE before implementation
- [ ] Enforce maximum stream size, chunk size, and chunk count limits
- [ ] Apply input validation with schema whitelisting (reject unknown fields)
- [ ] Sanitize control characters and template syntax from all inputs
- [ ] Use certificate pinning for external TLS connections
- [ ] Chain HMAC signatures to detect record reordering or insertion
- [ ] Monitor streams for anomaly patterns (burst, bandwidth, error rate)
- [ ] Implement circuit breakers to halt processing on repeated violations
- [ ] Log all security incidents with severity classification
- [ ] Quarantine suspicious records for manual review
- [ ] Test defenses against known attack patterns regularly

## Cross-References

- See [Stream Encryption, Validation](./01-encryption-validation.md) for encryption and HMAC basics
- See [Stream Data Protection](./02-stream-data-protection.md) for PII and access control patterns
- See [Transform Streams](../03-stream-processing-patterns/01-transform-streams.md) for Transform implementation
- See [Error Handling](../07-stream-error-handling/01-error-patterns.md) for error and recovery patterns

## Next Steps

Continue to [Modern Stream Technologies](../10-modern-stream-technologies/01-express-fastify-integration.md) to integrate stream security with web frameworks.
