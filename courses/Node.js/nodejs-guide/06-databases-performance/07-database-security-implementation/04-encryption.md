# Database Encryption and Data Protection

## What You'll Learn

- Encryption at rest and in transit
- Application-level field encryption
- Key management strategies
- Data masking and tokenization
- GDPR and compliance patterns

## Field-Level Encryption

```javascript
import { createCipheriv, createDecipheriv, randomBytes, scryptSync } from 'node:crypto';

class FieldEncryption {
    constructor(secretKey) {
        this.algorithm = 'aes-256-gcm';
        this.key = Buffer.from(secretKey, 'hex');
        if (this.key.length !== 32) throw new Error('Key must be 32 bytes (64 hex chars)');
    }

    encrypt(plaintext) {
        const iv = randomBytes(16);
        const cipher = createCipheriv(this.algorithm, this.key, iv);
        let encrypted = cipher.update(plaintext, 'utf8', 'hex');
        encrypted += cipher.final('hex');
        const authTag = cipher.getAuthTag();

        return `${iv.toString('hex')}:${authTag.toString('hex')}:${encrypted}`;
    }

    decrypt(encryptedData) {
        const [ivHex, authTagHex, encrypted] = encryptedData.split(':');
        const iv = Buffer.from(ivHex, 'hex');
        const authTag = Buffer.from(authTagHex, 'hex');
        const decipher = createDecipheriv(this.algorithm, this.key, iv);
        decipher.setAuthTag(authTag);

        let decrypted = decipher.update(encrypted, 'hex', 'utf8');
        decrypted += decipher.final('utf8');
        return decrypted;
    }
}

class EncryptedRepository {
    constructor(pool, encryptionKey) {
        this.pool = pool;
        this.encryption = new FieldEncryption(encryptionKey);
        this.encryptedFields = new Set();
    }

    addEncryptedField(field) {
        this.encryptedFields.add(field);
        return this;
    }

    async save(table, data) {
        const encrypted = { ...data };
        for (const field of this.encryptedFields) {
            if (encrypted[field]) {
                encrypted[field] = this.encryption.encrypt(String(encrypted[field]));
            }
        }

        const fields = Object.keys(encrypted);
        const placeholders = fields.map((_, i) => `$${i + 1}`);
        const values = Object.values(encrypted);

        const { rows } = await this.pool.query(
            `INSERT INTO ${table} (${fields.join(', ')}) VALUES (${placeholders.join(', ')}) RETURNING *`,
            values
        );

        return this.decryptRow(rows[0]);
    }

    async findById(table, id) {
        const { rows } = await this.pool.query(
            `SELECT * FROM ${table} WHERE id = $1`,
            [id]
        );
        return rows[0] ? this.decryptRow(rows[0]) : null;
    }

    decryptRow(row) {
        const decrypted = { ...row };
        for (const field of this.encryptedFields) {
            if (decrypted[field]) {
                try {
                    decrypted[field] = this.encryption.decrypt(decrypted[field]);
                } catch {
                    // Field might not be encrypted (mixed data)
                }
            }
        }
        return decrypted;
    }
}

// Usage
const repo = new EncryptedRepository(pool, process.env.ENCRYPTION_KEY)
    .addEncryptedField('ssn')
    .addEncryptedField('credit_card');

await repo.save('customers', {
    name: 'John Doe',
    ssn: '123-45-6789',
    credit_card: '4111111111111111',
});
```

## Data Masking

```javascript
class DataMasker {
    static maskEmail(email) {
        const [user, domain] = email.split('@');
        const maskedUser = user[0] + '*'.repeat(Math.max(user.length - 2, 1)) + user[user.length - 1];
        return `${maskedUser}@${domain}`;
    }

    static maskPhone(phone) {
        const digits = phone.replace(/\D/g, '');
        return '*'.repeat(digits.length - 4) + digits.slice(-4);
    }

    static maskSSN(ssn) {
        return `***-**-${ssn.slice(-4)}`;
    }

    static maskCreditCard(card) {
        return '*'.repeat(card.length - 4) + card.slice(-4);
    }

    static maskName(name) {
        return name[0] + '*'.repeat(name.length - 1);
    }

    static maskObject(obj, fields) {
        const masked = { ...obj };
        const maskFunctions = {
            email: this.maskEmail,
            phone: this.maskPhone,
            ssn: this.maskSSN,
            credit_card: this.maskCreditCard,
            name: this.maskName,
        };

        for (const [field, fn] of Object.entries(maskFunctions)) {
            if (masked[field] && fields.includes(field)) {
                masked[field] = fn(masked[field]);
            }
        }

        return masked;
    }
}
```

## Key Rotation

```javascript
class KeyRotationManager {
    constructor(pool, currentKey) {
        this.pool = pool;
        this.encryption = new FieldEncryption(currentKey);
        this.currentKeyId = 'current';
    }

    async rotateKey(oldKey, newKey) {
        const oldEncryption = new FieldEncryption(oldKey);
        const newEncryption = new FieldEncryption(newKey);

        let lastId = 0;
        let totalRotated = 0;

        while (true) {
            const { rows } = await this.pool.query(
                'SELECT id, ssn, credit_card FROM customers WHERE id > $1 ORDER BY id LIMIT 1000',
                [lastId]
            );

            if (rows.length === 0) break;

            for (const row of rows) {
                const updates = { id: row.id };

                if (row.ssn) {
                    const decrypted = oldEncryption.decrypt(row.ssn);
                    updates.ssn = newEncryption.encrypt(decrypted);
                }

                if (row.credit_card) {
                    const decrypted = oldEncryption.decrypt(row.credit_card);
                    updates.credit_card = newEncryption.encrypt(decrypted);
                }

                await this.pool.query(
                    'UPDATE customers SET ssn = $1, credit_card = $2 WHERE id = $3',
                    [updates.ssn, updates.credit_card, updates.id]
                );
            }

            lastId = rows[rows.length - 1].id;
            totalRotated += rows.length;
            console.log(`Key rotation: ${totalRotated} records processed`);
        }

        return totalRotated;
    }
}
```

## Best Practices Checklist

- [ ] Encrypt sensitive fields at application level
- [ ] Use TLS for database connections
- [ ] Store encryption keys in a secrets manager
- [ ] Implement key rotation strategy
- [ ] Mask data in non-production environments
- [ ] Use tokenization for payment data
- [ ] Audit encryption key access
- [ ] Test decryption after key rotation

## Cross-References

- See [Connection Security](./01-connection-security.md) for TLS setup
- See [Access Control](./02-access-control.md) for permissions
- See [Audit Logging](./02-access-control.md#audit-logging) for compliance

## Next Steps

Continue to [Performance Testing](../08-performance-testing-benchmarking/01-load-testing.md) for testing.
