---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Cloud Armor - Practical
Purpose: Real-world deployment scenarios, incident response, and operational best practices
Difficulty: intermediate
Prerequisites: [01_Basic_Cloud_Armor.md]
RelatedFiles: [01_Basic_Cloud_Armor.md], [02_Advanced_Cloud_Armor.md]
UseCase: DDoS protection, WAF rules
CertificationExam: GCP Professional Cloud Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Practical Cloud Armor implementation involves real-world scenarios that address common security incidents, business continuity requirements, and operational challenges. Organizations need to handle attack responses, plan for capacity, integrate with existing security operations, and ensure minimal business impact during security events. This practical guidance helps security teams implement Cloud Armor effectively in production environments.

# WHAT

Practical scenarios covered:

1. **DDoS Attack Response**: Handling volumetric and application-layer attacks
2. **WAF Tuning**: Reducing false positives while maintaining security
3. **Incident Response**: Playbook for security events
4. **Capacity Planning**: Ensuring adequate protection capacity
5. **Operational Monitoring**: Setting up alerts and dashboards
6. **Compliance**: Meeting regulatory requirements
7. **Multi-Environment**: Development, staging, production configurations
8. **Logging & Forensics**: Analyzing attack traffic

# HOW

## Scenario 1: DDoS Attack Response Playbook

```bash
# Step 1: Identify attack - check Cloud Armor logs
gcloud logging read "resource.type=compute_backend_service" \
    --limit=50 --format=json | jq '.[] .jsonPayload .securityPolicy'

# Step 2: Enable managed protection if under attack
gcloud compute security-policies update my-security-policy \
    --managed-protection-tier="HIGH" \
    --description "Emergency: Enable high protection"

# Step 3: Block attacking IPs dynamically
gcloud compute security-policies rules create 9999 \
    --security-policy my-security-policy \
    --expression "source.ip in $ATTACKING_IPS" \
    --action "deny-403" \
    --description "Block detected attackers"

# Step 4: Enable rate limiting
gcloud compute security-policies rules create 9500 \
    --security-policy my-security-policy \
    --expression "rateLimitExceeded(httpRequest.ip, 50, 60)" \
    --action "deny-403" \
    --description "Emergency rate limiting"
```

## Scenario 2: WAF Tuning for Production

```bash
# Test rules in preview mode first
gcloud compute security-policies rules create 5000 \
    --security-policy my-security-policy \
    --expression "evaluatePreconfiguredExpr('sqli-v33-stable').hasMatchingSegments" \
    --action "deny-403" \
    --preview \
    --description "Preview SQLi rule"

# After analyzing logs, update action if no false positives
gcloud compute security-policies rules update 5000 \
    --security-policy my-security-policy \
    --expression "evaluatePreconfiguredExpr('sqli-v33-stable').hasMatchingSegments" \
    --action "deny-403" \
    --description "Enable SQLi rule"
```

## Scenario 3: Multi-Environment Configuration

```bash
# Development environment - monitoring only
gcloud compute security-policies create dev-armor-policy \
    --description "Dev: Monitoring only"
gcloud compute security-policies rules create 1000 \
    --security-policy dev-armor-policy \
    --expression "true" \
    --action "allow" \
    --preview \
    --description "Preview all traffic"

# Staging environment - basic protection
gcloud compute security-policies create staging-armor-policy \
    --description "Staging: Basic protection"
gcloud compute security-policies update staging-armor-policy \
    --enable-layer7-ddos-defenses \
    --sqli-match-conditions="sqli-v33-stable"

# Production - full protection
gcloud compute security-policies create prod-armor-policy \
    --description "Production: Full protection"
gcloud compute security-policies update prod-armor-policy \
    --managed-protection-tier="MIDDLE" \
    --enable-layer7-ddos-defenses
```

# COMMON ISSUES

1. **Attack Detection Delays**: ML-based detection may take 5-15 minutes to identify attacks. Monitor logs actively.

2. **Rule Updates During Attack**: Adding rules during active attack may have propagation delays.

3. **IP Blocking Scope**: Broad IP blocking may impact legitimate users behind NAT.

4. **Logging During Attack**: High traffic volume may delay log ingestion.

5. **Cost Spikes**: Managed Protection High tier can incur significant costs during attacks.

6. **Recovery Time**: After attack subsides, manually downgrade protection tier.

# PERFORMANCE

Production considerations:
- Monitor latency dashboard during rule deployment
- Use preview mode during business off-peak hours
- Enable caching for static content via Cloud CDN
- Consider prewarming for expected traffic spikes

# COMPATIBILITY

Production integrations:
- Cloud Monitoring dashboards for security metrics
- Pub/Sub alerts for security team notification
- SIEM integration via Dataflow/Fluentd
- Terraform for Infrastructure as Code
- Cloud Build for automated deployments

# CROSS-REFERENCES

- **01_Basic.yaml**: Basic configuration and concepts
- **02_Advanced.yaml**: Advanced rules and managed protection
- **Security Incident Response**: Complementary playbook docs
- **Cloud Logging**: Log analysis and forensics
- **Cloud Monitoring**: Alert configuration

# EXAM TIPS

1. Use preview mode for rule testing before enforcement
2. Document all rule changes in change management
3. Test incident response playbooks regularly
4. Monitor false positive rates weekly
5. Keep rule sets updated with latest WAF rules
6. Use labels for environment differentiation
7. Set up budget alerts for Cloud Armor costs
8. Review attack traffic patterns monthly