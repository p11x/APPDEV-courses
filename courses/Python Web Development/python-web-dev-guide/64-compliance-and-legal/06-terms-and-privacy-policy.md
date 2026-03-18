# Terms and Privacy Policy

## What You'll Learn

- Privacy policy requirements
- Terms of service
- Implementing legally compliant policies

## Prerequisites

- Completed `05-cookie-compliance.md`

## Privacy Policy Sections

1. Introduction
2. Data Collected
3. How We Use Data
4. Data Sharing
5. Data Security
6. Your Rights
7. Cookies
8. Children's Privacy
9. Changes to Policy
10. Contact Information

## Privacy Policy Generator

```python
from dataclasses import dataclass

@dataclass
class PrivacyPolicy:
    company_name: str
    website: str
    contact_email: str
    data_types: list[str]
    purposes: list[str]
    third_parties: list[str]
    retention_period: str
    user_rights: list[str]

def generate_privacy_policy(policy: PrivacyPolicy) -> str:
    """Generate privacy policy text."""
    return f"""
# Privacy Policy
    
Last updated: 2024-01-15

## 1. Introduction
{policy.company_name} ("we", "us", or "our") operates {policy.website}. 
This Privacy Policy explains how we collect, use, disclose, and safeguard 
your information when you visit our website.

## 2. Data We Collect
We collect the following types of data:
{chr(10).join(f"- {d}" for d in policy.data_types)}

## 3. How We Use Your Data
We use your data for the following purposes:
{chr(10).join(f"- {p}" for d in policy.purposes)}

## 4. Data Sharing
We may share your data with:
{chr(10).join(f"- {t}" for t in policy.third_parties)}

## 5. Data Security
We implement appropriate security measures to protect your data.

## 6. Your Rights
You have the following rights:
{chr(10).join(f"- {r}" for r in policy.user_rights)}

## 7. Contact Us
For questions about this policy, contact us at {policy.contact_email}
"""
```

## Terms of Service Sections

1. Acceptance of Terms
2. Description of Service
3. User Accounts
4. User Conduct
5. Intellectual Property
6. Limitation of Liability
7. Termination
8. Governing Law

## Summary

- Have clear privacy policy
- Update regularly
- Make accessible to users

## Next Steps

Continue to `07-audit-logging.md`.
