# Security Compliance

## What You'll Learn

- Common compliance frameworks
- SOC 2, ISO 27001
- Compliance checklist

## Prerequisites

- Completed `03-privacy-by-design.md`

## Common Frameworks

| Framework | Focus | Industry |
|-----------|-------|----------|
| SOC 2 | Security, Availability | SaaS |
| ISO 27001 | Information Security | General |
| PCI DSS | Payment Card Data | Finance |
| HIPAA | Healthcare Data | Healthcare |

## SOC 2 Trust Principles

1. **Security** - Protection against unauthorized access
2. **Availability** - System operational
3. **Processing Integrity** - Data processing is accurate
4. **Confidentiality** - Data kept confidential
5. **Privacy** - Personal data protected

## Security Controls

```python
from dataclasses import dataclass
from typing import Literal

@dataclass
class SecurityControl:
    name: str
    category: str
    status: Literal["implemented", "planned", "not_applicable"]
    evidence: str | None = None

CONTROLS = [
    SecurityControl(
        "Access Control",
        "Security",
        "implemented",
        "IAM policy document"
    ),
    SecurityControl(
        "Encryption at Rest",
        "Security",
        "implemented",
        "KMS encryption keys"
    ),
    SecurityControl(
        "Incident Response",
        "Availability",
        "implemented",
        "IRP document"
    ),
]
```

## Compliance Checklist

- [ ] Access control policies
- [ ] Encryption for data at rest
- [ ] Encryption for data in transit
- [ ] Regular security testing
- [ ] Employee training
- [ ] Incident response plan
- [ ] Data backup procedures
- [ ] Vendor management
- [ ] Regular audits

## Summary

- Understand compliance frameworks
- Implement security controls
- Document everything

## Next Steps

Continue to `05-cookie-compliance.md`.
