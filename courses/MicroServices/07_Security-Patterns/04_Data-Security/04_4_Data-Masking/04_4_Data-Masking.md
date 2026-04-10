# Data Masking

## Overview

Data masking is a security technique that obscures specific data within a database or data store to prevent unauthorized access to sensitive information. Unlike encryption which completely transforms data, masking typically reveals only partial information while concealing the rest. This allows legitimate users to work with data that appears real but cannot be used for malicious purposes. Data masking is essential for non-production environments, reporting systems, and any scenario where users need to see data structure without accessing actual sensitive values.

In microservices architectures, data masking serves multiple purposes across different contexts. Development and testing environments often require realistic data but must not expose production secrets. Support teams may need to verify user identity without viewing complete account numbers. Analytics systems may require statistical data without exposing individual records. Each of these scenarios demands different masking approaches optimized for specific use cases while maintaining security.

Dynamic masking applies protection at query time, showing different mask levels based on user roles and permissions. Static masking creates permanent masked copies of data for specific environments. Both approaches have their place in comprehensive data security strategies. Organizations must carefully evaluate their masking requirements including performance impact, consistency across systems, and compliance with data protection regulations.

### Key Concepts

**Static Masking**: Creating a permanent masked copy of data that replaces original values with masked versions. This is typically used for test databases, development environments, and data sharing with third parties. Once masked, the data cannot be reverted to original values.

**Dynamic Masking**: Applying masking rules at query time based on user context. The original data remains unchanged in the database, but users see different views based on their privileges. This approach maintains real-time data while protecting sensitive information.

**Character Masking**: Replacing characters with consistent symbols like asterisks or X's. Common patterns include showing only the last four digits of a credit card or masking email addresses while preserving domain information.

**Format-Preserving Masking**: Maintaining the original data format while masking content. A masked credit card number still appears as 16 digits, a masked phone number retains its format, allowing systems expecting formatted data to function properly.

```mermaid
flowchart TD
    A[Data Request] --> B{User Role?}
    B -->|Admin| C[Show Full Data]
    B -->|Support| D[Partial Masking]
    B -->|Analytics| E[Aggregated Data]
    B -->|Public| F[Completely Masked]
    
    C --> G[Return Unmasked]
    D --> H{Masking Rules}
    H -->|Credit Card| I[****-****-****-1234]
    H -->|Email| J[j***@example.com]
    H -->|SSN| K[***-**-1234]
    
    E --> L[Statistical Summary]
    F --> M[Return *****]
    
    N[Static Masking Job] --> O[Extract Data]
    O --> P[Apply Masking Rules]
    P --> Q[Store Masked Copy]
    Q --> R[Verify Masking Applied]
```

## Standard Example

The following example demonstrates implementing data masking in a Node.js microservices environment with static and dynamic masking capabilities, role-based masking, and various masking patterns.

```javascript
const crypto = require('crypto');

const MASKING_TYPE = {
    FULL: 'full',
    PARTIAL: 'partial',
    EMAIL: 'email',
    CREDIT_CARD: 'credit_card',
    PHONE: 'phone',
    SSN: 'ssn',
    NAME: 'name',
    CUSTOM: 'custom',
};

class DataMasker {
    constructor(options = {}) {
        this.defaultMaskChar = options.defaultMaskChar || '*';
        this.showLastChars = options.showLastChars || 4;
        this.showFirstChars = options.showFirstChars || 0;
    }

    maskCreditCard(value) {
        if (!value || typeof value !== 'string') {
            return this.maskString(value);
        }
        
        const cleaned = value.replace(/\D/g, '');
        if (cleaned.length < 13) {
            return this.maskString(value);
        }

        const lastFour = cleaned.slice(-4);
        const masked = this.defaultMaskChar.repeat(cleaned.length - 4);
        
        return masked.slice(0, -4) + lastFour;
    }

    maskEmail(value) {
        if (!value || typeof value !== 'string') {
            return this.maskString(value);
        }

        const [localPart, domain] = value.split('@');
        if (!localPart || !domain) {
            return this.maskString(value);
        }

        const maskedLocal = localPart.length > 2 
            ? localPart[0] + this.defaultMaskChar.repeat(localPart.length - 2) + localPart.slice(-1)
            : this.defaultMaskChar.repeat(localPart.length);

        return `${maskedLocal}@${domain}`;
    }

    maskSSN(value) {
        if (!value || typeof value !== 'string') {
            return this.maskString(value);
        }

        const digits = value.replace(/\D/g, '');
        if (digits.length !== 9) {
            return this.maskString(value);
        }

        return `***-**-${digits.slice(-4)}`;
    }

    maskPhone(value) {
        if (!value || typeof value !== 'string') {
            return this.maskString(value);
        }

        const digits = value.replace(/\D/g, '');
        if (digits.length < 10) {
            return this.maskString(value);
        }

        const lastFour = digits.slice(-4);
        return `(***) ***-${lastFour}`;
    }

    maskName(value, options = {}) {
        if (!value || typeof value !== 'string') {
            return this.maskString(value);
        }

        const parts = value.split(' ');
        const maskedParts = parts.map(part => {
            if (part.length <= 2) {
                return this.defaultMaskChar.repeat(part.length);
            }
            return part[0] + this.defaultMaskChar.repeat(part.length - 2) + part.slice(-1);
        });

        return maskedParts.join(' ');
    }

    maskString(value, options = {}) {
        if (!value || typeof value !== 'string') {
            return value;
        }

        const showFirst = options.showFirstChars || this.showFirstChars;
        const showLast = options.showLastChars || this.showLastChars;
        
        if (value.length <= showFirst + showLast) {
            return this.defaultMaskChar.repeat(value.length);
        }

        const first = value.slice(0, showFirst);
        const middle = this.defaultMaskChar.repeat(value.length - showFirst - showLast);
        const last = value.slice(-showLast);

        return first + middle + last;
    }

    maskCustom(value, config) {
        const { pattern, replacement, regex } = config;
        
        if (regex) {
            return value.replace(new RegExp(regex.pattern, regex.flags), regex.replacement);
        }
        
        if (pattern) {
            return value.replace(new RegExp(pattern), replacement || this.defaultMaskChar);
        }

        return this.maskString(value, config);
    }

    maskValue(value, maskType, options = {}) {
        switch (maskType) {
            case MASKING_TYPE.CREDIT_CARD:
                return this.maskCreditCard(value);
            case MASKING_TYPE.EMAIL:
                return this.maskEmail(value);
            case MASKING_TYPE.SSN:
                return this.maskSSN(value);
            case MASKING_TYPE.PHONE:
                return this.maskPhone(value);
            case MASKING_TYPE.NAME:
                return this.maskName(value, options);
            case MASKING_TYPE.FULL:
                return this.defaultMaskChar.repeat(String(value).length);
            case MASKING_TYPE.PARTIAL:
                return this.maskString(value, options);
            case MASKING_TYPE.CUSTOM:
                return this.maskCustom(value, options.customConfig);
            default:
                return this.maskString(value);
        }
    }
}

class RoleBasedMasking {
    constructor(masker) {
        this.masker = masker;
        this.roleMasks = new Map();
    }

    configureRole(role, fieldMasks) {
        this.roleMasks.set(role, fieldMasks);
    }

    applyRoleMasks(data, role, context = {}) {
        const masks = this.roleMasks.get(role);
        
        if (!masks) {
            return data;
        }

        const maskedData = Array.isArray(data) ? [...data] : { ...data };
        
        for (const [field, maskConfig] of Object.entries(masks)) {
            if (maskedData[field] !== undefined) {
                if (maskConfig.conditions) {
                    let shouldMask = true;
                    for (const [conditionField, conditionValue] of Object.entries(maskConfig.conditions)) {
                        if (maskedData[conditionField] !== conditionValue) {
                            shouldMask = false;
                            break;
                        }
                    }
                    if (!shouldMask) continue;
                }

                if (typeof maskConfig.type === 'function') {
                    maskedData[field] = maskConfig.type(maskedData[field], context);
                } else {
                    maskedData[field] = this.masker.maskValue(
                        maskedData[field],
                        maskConfig.type,
                        maskConfig.options
                    );
                }
            }
        }

        return maskedData;
    }
}

class DynamicMaskingService {
    constructor(options = {}) {
        this.masker = new DataMasker(options);
        this.roleMasking = new RoleBasedMasking(this.masker);
        this.maskRules = new Map();
    }

    initialize() {
        this.roleMasking.configureRole('admin', {});
        
        this.roleMasking.configureRole('support', {
            email: { type: MASKING_TYPE.EMAIL },
            phone: { type: MASKING_TYPE.PHONE },
            creditCard: { type: MASKING_TYPE.CREDIT_CARD },
        });

        this.roleMasking.configureRole('analyst', {
            ssn: { type: MASKING_TYPE.FULL },
            creditCard: { type: MASKING_TYPE.CREDIT_CARD },
            phone: { type: MASKING_TYPE.PHONE },
        });

        this.roleMasking.configureRole('customer', {
            email: { type: MASKING_TYPE.EMAIL },
            phone: { type: MASKING_TYPE.PHONE, options: { showLastChars: 2 } },
        });

        console.log('Dynamic masking service initialized');
    }

    addMaskRule(fieldPath, maskConfig) {
        this.maskRules.set(fieldPath, maskConfig);
    }

    maskObject(data, role, context = {}) {
        return this.roleMasking.applyRoleMasks(data, role, context);
    }

    maskArray(dataArray, role, context = {}) {
        return dataArray.map(item => this.maskObject(item, role, context));
    }

    maskDatabaseResult(result, role, options = {}) {
        if (Array.isArray(result)) {
            return this.maskArray(result, role, options);
        }
        
        if (result && typeof result === 'object') {
            if (result.rows) {
                return { ...result, rows: this.maskArray(result.rows, role, options) };
            }
            return this.maskObject(result, role, options);
        }

        return result;
    }

    createStaticMask(data, rules) {
        const maskedData = JSON.parse(JSON.stringify(data));
        
        const applyRules = (obj, ruleSet) => {
            for (const [field, config] of Object.entries(ruleSet)) {
                if (obj[field] !== undefined) {
                    if (config.nested) {
                        applyRules(obj[field], config.nested);
                    } else {
                        obj[field] = this.masker.maskValue(obj[field], config.type, config.options);
                    }
                }
            }
        };

        applyRules(maskedData, rules);
        
        return maskedData;
    }

    generateTestData(realData, maskRules) {
        const testData = JSON.parse(JSON.stringify(realData));
        
        const maskFields = (obj, rules) => {
            for (const [field, rule] of Object.entries(rules)) {
                if (obj[field] !== undefined) {
                    obj[field] = this.masker.maskValue(obj[field], rule.type, rule.options);
                }
            }
        };

        maskFields(testData, maskRules);
        
        return testData;
    }

    validateMasking(data, role, rules) {
        const masked = this.maskObject(data, role);
        const issues = [];

        for (const [field, rule] of Object.entries(rules)) {
            if (masked[field] === data[field]) {
                issues.push({
                    field: field,
                    expected: 'masked',
                    actual: 'visible',
                    rule: rule.type,
                });
            }
        }

        return {
            valid: issues.length === 0,
            issues: issues,
        };
    }
}

async function demonstrateDataMasking() {
    const maskingService = new DynamicMaskingService();
    maskingService.initialize();

    const userData = {
        id: '12345',
        name: 'John Smith',
        email: 'john.smith@example.com',
        phone: '555-123-4567',
        ssn: '123-45-6789',
        creditCard: '4111111111111111',
        address: '123 Main Street, New York, NY 10001',
    };

    console.log('=== Original Data ===');
    console.log(JSON.stringify(userData, null, 2));

    console.log('\n=== Masked for Admin (Full Access) ===');
    console.log(JSON.stringify(maskingService.maskObject(userData, 'admin'), null, 2));

    console.log('\n=== Masked for Support ===');
    console.log(JSON.stringify(maskingService.maskObject(userData, 'support'), null, 2));

    console.log('\n=== Masked for Analyst ===');
    console.log(JSON.stringify(maskingService.maskObject(userData, 'analyst'), null, 2));

    console.log('\n=== Static Masked for Test Environment ===');
    const staticMaskRules = {
        email: { type: MASKING_TYPE.EMAIL },
        phone: { type: MASKING_TYPE.PHONE },
        ssn: { type: MASKING_TYPE.SSN },
        creditCard: { type: MASKING_TYPE.CREDIT_CARD },
    };
    console.log(JSON.stringify(maskingService.createStaticMask(userData, staticMaskRules), null, 2));

    console.log('\n=== Validation ===');
    const validation = maskingService.validateMasking(userData, 'analyst', {
        ssn: { type: MASKING_TYPE.FULL },
        creditCard: { type: MASKING_TYPE.CREDIT_CARD },
    });
    console.log('Validation result:', validation);
}

if (require.main === module) {
    demonstrateDataMasking().catch(console.error);
}

module.exports = { DataMasker, RoleBasedMasking, DynamicMaskingService, MASKING_TYPE };

## Real-World Examples

### Oracle Data Redaction

Oracle Database provides Data Redaction as a built-in feature that dynamically masks data at query time based on security policies.

```javascript
class OracleDataRedaction {
    constructor() {
        this.redactionPolicies = new Map();
    }

    createRedactionPolicy(tableName, columnName, functionType) {
        const policyName = `redact_${tableName}_${columnName}`;
        
        const policy = {
            tableName: tableName,
            columnName: columnName,
            functionType: functionType,
            expression: 'SYS_CONTEXT(''USERENV'',''SESSION_USER'') != ''ADMIN''',
        };

        this.redactionPolicies.set(policyName, policy);
        
        return `
            BEGIN
                DBMS_REDACT.ADD_POLICY(
                    object_schema => 'APP_USER',
                    object_name => '${tableName}',
                    column_name => '${columnName}',
                    policy_name => '${policyName}',
                    function_type => DBMS_REDACT.${functionType},
                    expression => '${policy.expression}'
                );
            END;
        `;
    }

    applyFullRedaction(tableName, columnName) {
        return this.createRedactionPolicy(tableName, columnName, 'FULL_REDACT_VARCHAR2');
    }

    applyPartialRedaction(tableName, columnName, pattern) {
        return `
            BEGIN
                DBMS_REDACT.ADD_POLICY(
                    object_schema => 'APP_USER',
                    object_name => '${tableName}',
                    column_name => '${columnName}',
                    policy_name => 'redact_${tableName}_${columnName}',
                    function_type => DBMS_REDACT.PARTIAL,
                    function_parameters => '${pattern}'
                );
            END;
        `;
    }

    applyRandomRedaction(tableName, columnName, dataType) {
        return this.createRedactionPolicy(tableName, columnName, 'RANDOM');
    }

    disablePolicy(policyName) {
        return `
            BEGIN
                DBMS_REDACT.DISABLE_POLICY(
                    object_schema => 'APP_USER',
                    object_name => '${policyName}'
                );
            END;
        `;
    }
}
```

### IBM InfoSphere Guardium Data Masking

Guardium provides enterprise-grade data masking with support for static and dynamic masking across multiple database platforms.

```javascript
class GuardiumDataMasking {
    constructor(options = {}) {
        this.guardiumServer = options.server;
        this.guardiumPort = options.port || 8443;
        this.policyName = options.policyName || 'default-masking-policy';
    }

    createMaskingRule(ruleConfig) {
        return {
            name: ruleConfig.name,
            order: ruleConfig.order || 1,
            action: 'MASK',
            fields: ruleConfig.fields.map(field => ({
                name: field.name,
                maskingType: field.maskingType,
                pattern: field.pattern || null,
                position: field.position || null,
            })),
            conditions: {
                userRoles: ruleConfig.allowedRoles || ['admin'],
                queryTypes: ruleConfig.queryTypes || ['SELECT'],
                clientIPs: ruleConfig.allowedIPs || null,
            },
        };
    }

    applyStaticMasking(data, maskConfig) {
        const maskedData = { ...data };

        for (const [field, config] of Object.entries(maskConfig)) {
            if (maskedData[field]) {
                switch (config.type) {
                    case 'FIXED':
                        maskedData[field] = config.value;
                        break;
                    case 'PARTIAL':
                        maskedData[field] = this.partialMask(maskedData[field], config);
                        break;
                    case 'SHUFFLE':
                        maskedData[field] = this.shuffleMask(maskedData[field], maskConfig);
                        break;
                    case 'NULL':
                        maskedData[field] = null;
                        break;
                }
            }
        }

        return maskedData;
    }

    partialMask(value, config) {
        const str = String(value);
        if (!config.showStart || !config.showEnd) return '*'.repeat(str.length);
        
        return str.substring(0, config.showStart) + 
               '*'.repeat(str.length - config.showStart - config.showEnd) + 
               str.substring(str.length - config.showEnd);
    }

    shuffleMask(value, allFields) {
        const values = Object.values(allFields).map(f => f.original).sort(() => Math.random() - 0.5);
        return values[0];
    }

    exportMaskingRules() {
        return {
            policy: this.policyName,
            rules: this.getRules(),
            exportedAt: new Date().toISOString(),
            version: '10.0',
        };
    }
}
```

## Output Statement

Data masking is a critical technique for protecting sensitive information across different environments and user roles. By implementing appropriate masking strategies, organizations can enable development and testing with realistic data while preventing unauthorized access to sensitive information. Both static and dynamic masking have their place in comprehensive data security strategies, with static masking suitable for non-production environments and dynamic masking providing flexible, real-time protection in production systems. The choice of masking approach should align with security requirements, performance considerations, and regulatory obligations.

## Best Practices

**Implement Role-Based Masking**: Define different masking rules for different user roles. Support staff may need partial data for verification, while developers need completely masked fields.

**Choose Format-Preserving Masks**: Maintain data format (credit card numbers, phone numbers) to ensure masked data works with existing validation logic and database schemas.

**Apply Static Masking for Non-Production**: Use static masking for development, testing, and staging environments where data never needs to be reverted.

**Use Dynamic Masking for Production**: Implement dynamic masking in production to apply protection at query time without modifying stored data.

**Protect Masking Configuration**: Masking rules and algorithms should be secured and not exposed to users who should not see them.

**Validate Masking Effectiveness**: Regularly audit masked data to ensure sensitive information is properly protected and no leakage occurs.

**Combine with Encryption**: Use data masking together with encryption for defense-in-depth, especially for the most sensitive fields.