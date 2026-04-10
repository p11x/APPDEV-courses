---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Cloud Armor - Basic
Purpose: Protect applications from DDoS attacks and web attacks using Google Cloud's DDoS mitigation and WAF service
Difficulty: beginner
Prerequisites: []
RelatedFiles: [02_Advanced_Cloud_Armor.md], [03_Practical_Cloud_Armor.md]
UseCase: DDoS protection, WAF rules
CertificationExam: GCP Professional Cloud Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud Armor is essential for protecting cloud-native applications against malicious traffic and cyber threats. Without proper protection, applications are vulnerable to DDoS attacks that can overwhelm servers, causing service outages and business disruption. Cloud Armor integrates directly with Google Cloud's global infrastructure, providing edge-level DDoS mitigation that can absorb and scrub traffic before it reaches your application. It also offers Web Application Firewall (WAF) capabilities to protect against common web exploits like SQL injection, cross-site scripting (XSS), and other OWASP Top 10 vulnerabilities.

## 📖 WHAT

Cloud Armor is Google Cloud's built-in DDoS protection and WAF service that operates at the edge of Google's network. It provides:
- Layer 3/4 DDoS protection using Google Cloud's global scale infrastructure
- Layer 7 WAF rules based on OWASP Top 10 threats
- IP rate limiting to prevent brute force attacks
- Geographic access controls (allow/block by region)
- Managed protection tiers (Standard vs Managed)

Cloud Armor policies are attached to backend services or load balancers, and rules are evaluated in order of priority.

## 🔧 HOW

## Example 1: Enable Cloud Armor with Basic DDoS Protection

```bash
# Create a Cloud Armor security policy
gcloud compute security-policies create armor-policy-ddos \
    --description "Basic DDoS protection policy"

# Add a rate-based denial rule to prevent brute force
gcloud compute security-policies rules create 1000 \
    --security-policy armor-policy-ddos \
    --expression "rateLimitExceeded(httpRequest.ip)" \
    --action "deny-403" \
    --description "Rate limit check"

# Attach to backend service
gcloud compute backend-services update my-backend-service \
    --security-policy armor-policy-ddos
```

## Example 2: Configure Geographic Blocking

```bash
# Block traffic from specific regions
gcloud compute security-policies rules create 8000 \
    --security-policy armor-policy-ddos \
    --expression "origin.region_code in ['CN', 'RU']" \
    --action "deny-403" \
    --description "Block high-risk regions"
```

## Example 3: Basic WAF Rule for SQL Injection

```bash
# Enable preconfigured WAF rules for SQL injection protection
gcloud compute security-policies update armor-policy-ddos \
    --enable-layer7-ddos-defenses \
    --sqli-match-conditions="xss-v33-stable" \
    --xss-match-conditions="xss-v33-stable"
```

## ⚠️ COMMON ISSUES

1. **False Positives**: Aggressive WAF rules can block legitimate traffic. Always test rules in preview mode before enforcing.

2. **Latency Impact**: Cloud Armor adds minor latency (1-3ms) for traffic inspection. Monitor application performance after deployment.

3. **Rule Ordering**: Rules are evaluated by priority (lowest first). Ensure more specific rules have lower priority numbers.

4. **Logging Delays**: Security logs may take 1-2 minutes to appear in Cloud Logging. Don't panic if logs are immediate.

5. **Quota Limits**: Each project has limits on security policy rules (default 200). Monitor usage to avoid hitting limits.

## 🏃 PERFORMANCE

Cloud Armor operates at Google's edge locations, providing minimal latency impact (typically 1-3ms additional). The managed protection tier uses machine learning to automatically detect and mitigate attacks. Performance considerations:

- Standard tier: Basic DDoS protection included with Load Balancers
- Managed tier: Advanced ML-based protection with higher capacity
- Regional blocking: Reduces origin requests from blocked regions
- Caching: WAF rules can be cached at the edge for faster evaluation

## 🌐 COMPATIBILITY

Cloud Armor integrates with:
- Google Cloud Load Balancers (HTTP/S, TCP, UDP)
- Cloud CDN for edge security
- Cloud Run and Cloud Functions (via HTTP Load Balancer)
- External HTTP(S) Load Balancer for hybrid/cloud architectures
- Kubernetes Gateway API via Gateway API integrations

Not compatible with:
- Legacy Classic Load Balancers
- Internal TCP/UDP Load Balancers directly (requires HTTP(S) LB)

## 🔗 CROSS-REFERENCES

- **02_Advanced.yaml**: Advanced WAF configurations, DDoS protection tiers, and custom rules
- **03_Practical.yaml**: Real-world deployment scenarios and incident response
- **GCP Security Best Practices**: Complementary to VPC Service Controls and IAM
- **Cloud Load Balancing**: Cloud Armor is deployed with Load Balancers
- **Cloud CDN**: Works together for edge delivery and security

## ✅ EXAM TIPS

1. Cloud Armor policies are region-scoped but can be applied globally
2. Preconfigured WAF rules use OWASP Top 10 as baseline
3. Managed protection tier provides automated DDoS mitigation
4. Always test rules in preview mode before enforcement
5. IP rate limiting uses token bucket algorithm
6. Geographic blocking uses ISO 3166 region codes
7. Cloud Armor logs appear in Cloud Logging with request metadata
8. Cost is based on rule count and protection tier selected