---
Category: Google Cloud Platform
Subcategory: GCP Fundamentals
Concept: GCP Global Network
Purpose: Hands-on exercises for global network configuration
Difficulty: advanced
Prerequisites: 01_Basic_GCP_Global_Network.md, 02_Advanced_GCP_Global_Network.md
RelatedFiles: 01_Basic_GCP_Global_Network.md, 02_Advanced_GCP_Global_Network.md
UseCase: Global load balancing, CDN configuration, hybrid connectivity
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with GCP's global network is essential for deploying worldwide applications, configuring content delivery, and implementing hybrid cloud connectivity.

### Lab Goals

- Configure global load balancing
- Set up Cloud CDN
- Implement hybrid connectivity

## 📖 WHAT

### Exercise Overview

1. **Global Load Balancing**: Multi-region deployment
2. **Cloud CDN**: Content delivery
3. **Hybrid Cloud**: VPN and Interconnect

## 🔧 HOW

### Exercise 1: Global Load Balancing

```bash
#!/bin/bash
# Configure global HTTP(S) load balancing

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create instance template
gcloud compute instance-templates create lb-template \
    --machine-type=e2-medium \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --metadata=startup-script='#!/bin/bash
echo "Web server on $(hostname)" > /var/www/html/index.html'

# Create regional MIGs
for region in us-central1 us-east1 europe-west1; do
    echo "Creating MIG in $region..."
    gcloud compute instance-groups managed create lb-mig-$region \
        --template=lb-template \
        --size=2 \
        --zone=${region}-a
done

# Create health check
gcloud compute health-checks create lb-health \
    --port=80 \
    --check-interval=30s

# Create backend service
gcloud compute backend-services create lb-backend \
    --protocol=HTTPS \
    --port-name=https \
    --health-checks=lb-health

# Add backends
gcloud compute backend-services add-backend lb-backend \
    --instance-group=lb-mig-us-central1 \
    --instance-group-region=us-central1

gcloud compute backend-services add-backend lb-backend \
    --instance-group=lb-mig-us-east1 \
    --instance-group-region=us-east1

gcloud compute backend-services add-backend lb-backend \
    --instance-group=lb-mig-europe-west1 \
    --instance-group-region=europe-west1

# Create URL map
gcloud compute url-maps create lb-url-map \
    --default-service=lb-backend

# Create SSL certificate
gcloud compute ssl-certificates create lb-ssl \
    --certificate=server.crt \
    --private-key=server.key

# Create target proxy
gcloud compute target-https-proxies create lb-proxy \
    --url-map=lb-url-map \
    --ssl-certificates=lb-ssl

# Create global forwarding rule
gcloud compute forwarding-rules create lb-forwarding \
    --global \
    --target-https-proxy=lb-proxy \
    --ports=443

echo "Global load balancing configured!"
```

### Exercise 2: Cloud CDN Setup

```bash
#!/bin/bash
# Configure Cloud CDN

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create bucket
gsutil mb -l us-central1 gs://my-cdn-bucket

# Upload sample content
echo "Cached content" > index.html
gsutil cp index.html gs://my-cdn-bucket/

# Create backend bucket
gcloud compute backend-buckets create cdn-backend \
    --bucket=my-cdn-bucket \
    --enable-cdn \
    --description="CDN for static content"

# Configure cache settings
gcloud compute backend-buckets update cdn-backend \
    --cache-mode=USE_ORIGIN_HEADERS \
    --default-ttl=3600 \
    --max-ttl=86400 \
    --negative-caching \
    --serve-while-stale=3600

# Create SSL for custom domain
gcloud compute ssl-certificates create cdn-ssl \
    --domains=cdn.example.com \
    --certificate-chain=fullchain.pem \
    --private-key=privkey.pem

# Create URL map for CDN
gcloud compute url-maps create cdn-map \
    --default-backend-bucket=cdn-backend

# Create target proxy
gcloud compute target-https-proxies create cdn-proxy \
    --url-map=cdn-map \
    --ssl-certificates=cdn-ssl

# Create forwarding rule
gcloud compute forwarding-rules create cdn-forward \
    --global \
    --target-https-proxy=cdn-proxy \
    --ports=443

# Configure signed URLs
gcloud compute ssl-certificates describe cdn-ssl

echo "Cloud CDN configured!"
```

### Exercise 3: Hybrid Cloud VPN

```bash
#!/bin/bash
# Configure Cloud VPN

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create VPC
gcloud compute networks create hybrid-vpc \
    --subnet-mode=custom

gcloud compute networks subnets create hybrid-subnet \
    --network=hybrid-vpc \
    --region=us-central1 \
    --range=10.0.0.0/24

# Create Cloud Router
gcloud compute routers create hybrid-router \
    --network=hybrid-vpc \
    --region=us-central1 \
    --asn=65001

# Create VPN tunnel
gcloud compute vpn-tunnels create hybrid-tunnel \
    --peer-address=203.0.113.1 \
    --shared-secret=secret123 \
    --router=hybrid-router \
    --region=us-central1 \
    --ike-version=2

# Configure BGP session
gcloud compute routers add-bgp-peer hybrid-router \
    --region=us-central1 \
    --peer-name=on-prem-peer \
    --interface=interface-0 \
    --ip-address=169.254.0.1 \
    --peer-ip-address=169.254.0.2 \
    --peer-asn=65000

# Verify VPN status
gcloud compute vpn-tunnels describe hybrid-tunnel \
    --region=us-central1

# Check router status
gcloud compute routers get-status hybrid-router \
    --region=us-central1

echo "Hybrid VPN configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| LB not working | Check health check |
| CDN not caching | Check cache headers |
| VPN down | Verify BGP |

### Validation

```bash
# Check LB status
gcloud compute backend-services describe lb-backend

# Check CDN cache
gcloud compute backend-buckets describe cdn-backend

# Check VPN tunnel
gcloud compute vpn-tunnels list
```

## 🌐 COMPATIBILITY

### Integration

- VPC Networking
- Cloud Armor
- Cloud Monitoring

## 🔗 CROSS-REFERENCES

### Related Labs

- VPC Networking
- Cloud Armor
- Cloud Monitoring

### Next Steps

- Add Cloud Armor
- Configure monitoring
- Set up logging

## ✅ EXAM TIPS

- Practice global LB configuration
- Know CDN cache settings
- Understand VPN setup
- Monitor network performance
