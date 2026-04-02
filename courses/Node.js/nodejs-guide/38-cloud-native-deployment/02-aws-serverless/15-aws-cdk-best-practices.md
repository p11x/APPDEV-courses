# AWS CDK Best Practices

## What You'll Learn

- Production-ready CDK patterns
- Security hardening guidelines
- Performance optimization strategies
- Testing and deployment best practices

---

## Layer 1: Best Practices

### Security

| Practice | Implementation |
|----------|----------------|
| Least privilege | Granular IAM policies |
| Encryption | Enable at rest/transit |
| Private networking | VPC with private subnets |
| Secrets | Use Secrets Manager |

### Performance

- Enable CDK diagnostics
- Use lazy imports for large constructs
- Implement stack dependencies

---

## Layer 2: Code Patterns

### Production Stack

```typescript
export class ProductionStack extends Stack {
  constructor(scope: App, id: string) {
    super(scope, id, { 
      terminationProtection: true,
      analytics: true
    });
    
    this.node.addValidation(() => validateStack(this));
  }
}

function validateStack(stack: Stack): ValidationError[] {
  const errors: ValidationError[] = [];
  
  const table = Table.fromTableName(stack, 'Table', 'users');
  if (!table.encryption) {
    errors.push({ message: 'Table should be encrypted' });
  }
  
  return errors;
}
```

---

## Next Steps

Continue to [CDK vs Terraform](./16-aws-cdk-vs-terraform.md)