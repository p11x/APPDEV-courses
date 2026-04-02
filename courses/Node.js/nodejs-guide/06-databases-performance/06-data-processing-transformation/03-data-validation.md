# Data Validation and Cleaning Patterns

## What You'll Learn

- Schema-based data validation
- Data cleaning and normalization
- Validation pipeline implementation
- Error collection and reporting
- Data quality metrics

## Validation Framework

```javascript
class DataValidator {
    constructor() {
        this.rules = new Map();
    }

    addRule(field, validator) {
        if (!this.rules.has(field)) {
            this.rules.set(field, []);
        }
        this.rules.get(field).push(validator);
        return this;
    }

    validate(record) {
        const errors = [];

        for (const [field, validators] of this.rules) {
            for (const validator of validators) {
                const error = validator(field, record[field], record);
                if (error) {
                    errors.push({ field, message: error, value: record[field] });
                }
            }
        }

        return {
            valid: errors.length === 0,
            errors,
            record,
        };
    }

    validateBatch(records) {
        const results = records.map(r => this.validate(r));
        return {
            total: records.length,
            valid: results.filter(r => r.valid).length,
            invalid: results.filter(r => !r.valid).length,
            results,
        };
    }

    clean(records) {
        return records.filter(r => this.validate(r).valid);
    }
}

// Built-in validators
const validators = {
    required: (field, value) =>
        value == null || value === '' ? `${field} is required` : null,

    string: (field, value) =>
        typeof value !== 'string' ? `${field} must be a string` : null,

    number: (field, value) =>
        typeof value !== 'number' || isNaN(value) ? `${field} must be a number` : null,

    email: (field, value) =>
        !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value) ? `${field} must be a valid email` : null,

    minLength: (min) => (field, value) =>
        String(value).length < min ? `${field} must be at least ${min} characters` : null,

    maxLength: (max) => (field, value) =>
        String(value).length > max ? `${field} must be at most ${max} characters` : null,

    range: (min, max) => (field, value) =>
        value < min || value > max ? `${field} must be between ${min} and ${max}` : null,

    pattern: (regex, message) => (field, value) =>
        !regex.test(value) ? message || `${field} format is invalid` : null,

    unique: (existingValues) => (field, value) =>
        existingValues.has(value) ? `${field} must be unique` : null,
};

// Usage
const userValidator = new DataValidator()
    .addRule('name', validators.required)
    .addRule('name', validators.minLength(2))
    .addRule('name', validators.maxLength(100))
    .addRule('email', validators.required)
    .addRule('email', validators.email)
    .addRule('age', validators.number)
    .addRule('age', validators.range(0, 150));

const results = userValidator.validateBatch(users);
console.log(`Valid: ${results.valid}, Invalid: ${results.invalid}`);
```

## Data Cleaning

```javascript
class DataCleaner {
    constructor() {
        this.transformers = [];
    }

    addTransformer(transformer) {
        this.transformers.push(transformer);
        return this;
    }

    clean(records) {
        return records.map(record => {
            let cleaned = { ...record };
            for (const transformer of this.transformers) {
                cleaned = transformer(cleaned);
            }
            return cleaned;
        });
    }
}

const cleaner = new DataCleaner()
    .addTransformer(r => ({
        ...r,
        name: r.name?.trim(),
        email: r.email?.trim().toLowerCase(),
    }))
    .addTransformer(r => ({
        ...r,
        phone: r.phone?.replace(/\D/g, ''),
    }))
    .addTransformer(r => ({
        ...r,
        age: r.age ? parseInt(r.age) : null,
    }))
    .addTransformer(r => ({
        ...r,
        country: r.country?.toUpperCase(),
    }))
    .addTransformer(r => {
        const { _temp, _unused, ...clean } = r;
        return clean;
    });

const cleanedUsers = cleaner.clean(rawUsers);
```

## Validation Pipeline for ETL

```javascript
import { Transform } from 'node:stream';

function createValidationStream(validator) {
    const valid = [];
    const invalid = [];

    return new Transform({
        objectMode: true,
        transform(record, encoding, callback) {
            const result = validator.validate(record);
            if (result.valid) {
                valid.push(record);
                callback(null, record);
            } else {
                invalid.push({ record, errors: result.errors });
                callback(); // Skip invalid records
            }
        },
        flush(callback) {
            this.push({ _stats: { valid: valid.length, invalid: invalid.length, invalidRecords: invalid } });
            callback();
        },
    });
}
```

## Best Practices Checklist

- [ ] Validate data at ingestion boundaries
- [ ] Use schema-based validation (Zod, Joi, etc.)
- [ ] Collect all errors, not just the first one
- [ ] Clean data before validation
- [ ] Log validation failures for analysis
- [ ] Implement data quality metrics
- [ ] Use streaming validation for large datasets

## Cross-References

- See [ETL Pipelines](./02-etl-pipelines.md) for data pipelines
- See [Streaming Data](./01-streaming-data.md) for stream processing
- See [Security](../07-database-security-implementation/01-connection-security.md) for input sanitization

## Next Steps

Continue to [Data Migration](./04-data-migration.md) for migration patterns.
