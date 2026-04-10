---
Category: AWS Cloud Practitioner
Subcategory: Cloud Fundamentals
Concept: Cloud Concepts
Purpose: Hands-on labs implementing cloud concepts including auto-scaling, load balancing, and cost optimization
Difficulty: intermediate
Prerequisites: 01_Basic_Cloud_Concepts.md, 02_Advanced_Cloud_Concepts.md
RelatedFiles: 01_Basic_Cloud_Concepts.md, 02_Advanced_Cloud_Concepts.md, 03_Practical_EC2.md
UseCase: Building production-ready cloud infrastructure
CertificationExam: AWS Certified Cloud Practitioner - Hands-on practice
LastUpdated: 2025
---

## WHY

Practical implementation solidifies cloud concepts through hands-on experience. This lab-based approach ensures you can actually deploy, scale, and manage cloud resources—not just understand the theory.

### Prerequisites for Hands-On Labs

- AWS Account (free tier eligible)
- AWS CLI configured with credentials
- Basic understanding of command line
- Terraform installed (optional, for IaC examples)

### Lab Environment Setup

```bash
# Verify AWS CLI is installed and configured
aws --version
# Expected: aws-cli/2.x.x

# Verify credentials
aws sts get-caller-identity
# Expected: JSON with your Account, Userarn, UserId

# Set up cost alerts before starting labs
aws cloudwatch put-metric-alarm \
    --alarm-name "MonthlyCostAlarm" \
    --metric-name EstimatedCharges \
    --namespace AWS/Billing \
    --statistic Maximum \
    --period 3600 \
    --threshold 50 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 1 \
    --alarm-actions arn:aws:sns:us-east-1:123456789:aws- cost-alerts
```

## WHAT

### Lab Architecture

We will build:
1. VPC with public and private subnets across 2 AZs
2. Auto Scaling Group with launch template
3. Application Load Balancer
4. RDS Multi-AZ database
5. S3 bucket for static assets
6. CloudFront distribution

### Expected Outcome

- Web application accessible via CloudFront
- Auto scales from 2 to 10 instances under load
- Database with automatic failover
- Static assets served from edge locations
- Total managed via CloudWatch and Auto Scaling

## HOW

### Lab 1: VPC and Networking Setup

```bash
# Step 1: Create VPC
VPC_ID=$(aws ec2 create-vpc \
    --cidr-block 10.0.0.0/16 \
    --query 'Vpc.VpcId' \
    --output text)
echo "Created VPC: $VPC_ID"

# Enable DNS hostnames
aws ec2 modify-vpc-attribute \
    --vpc-id $VPC_ID \
    --enable-dns-hostnames '{"Value": true}'

# Step 2: Create subnets
# Public Subnet A (us-east-1a)
SUBNET_PUB_A=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.1.0/24 \
    --availability-zone us-east-1a \
    --query 'Subnet.SubnetId' \
    --output text)
echo "Public Subnet A: $SUBNET_PUB_A"

# Public Subnet B (us-east-1b)
SUBNET_PUB_B=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.2.0/24 \
    --availability-zone us-east-1b \
    --query 'Subnet.SubnetId' \
    --output text)
echo "Public Subnet B: $SUBNET_PUB_B"

# Private Subnet A
SUBNET_PRIV_A=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.10.0/24 \
    --availability-zone us-east-1a \
    --query 'Subnet.SubnetId' \
    --output text)
echo "Private Subnet A: $SUBNET_PRIV_A"

# Private Subnet B
SUBNET_PRIV_B=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.20.0/24 \
    --availability-zone us-east-1b \
    --query 'Subnet.SubnetId' \
    --output text)
echo "Private Subnet B: $SUBNET_PRIV_B"

# Step 3: Create and attach Internet Gateway
IGW_ID=$(aws ec2 create-internet-gateway \
    --query 'InternetGateway.InternetGatewayId' \
    --output text)
aws ec2 attach-internet-gateway \
    --vpc-id $VPC_ID \
    --internet-gateway-id $IGW_ID
echo "Internet Gateway: $IGW_ID"

# Step 4: Create public route table
RT_PUB=$(aws ec2 create-route-table \
    --vpc-id $VPC_ID \
    --query 'RouteTable.RouteTableId' \
    --output text)
aws ec2 create-route \
    --route-table-id $RT_PUB \
    --destination-cidr-block 0.0.0.0/0 \
    --gateway-id $IGW_ID
aws ec2 associate-route-table \
    --route-table-id $RT_PUB \
    --subnet-id $SUBNET_PUB_A
aws ec2 associate-route-table \
    --route-table-id $RT_PUB \
    --subnet-id $SUBNET_PUB_B
echo "Public Route Table: $RT_PUB"

# Step 5: Create NAT Gateway for private subnets
EIP_ALLOC=$(aws ec2 allocate-address \
    --domain vpc \
    --query 'AllocationId' \
    --output text)
NAT_GW=$(aws ec2 create-nat-gateway \
    --subnet-id $SUBNET_PUB_A \
    --allocation-id $EIP_ALLOC \
    --query 'NatGateway.NatGatewayId' \
    --output text)

# Wait for NAT Gateway to be available
aws ec2 wait nat-gateway-available \
    --nat-gateway-ids $NAT_GW

# Create private route table with NAT Gateway
RT_PRIV=$(aws ec2 create-route-table \
    --vpc-id $VPC_ID \
    --query 'RouteTable.RouteTableId' \
    --output text)
aws ec2 create-route \
    --route-table-id $RT_PRIV \
    --destination-cidr-block 0.0.0.0/0 \
    --nat-gateway-id $NAT_GW
aws ec2 associate-route-table \
    --route-table-id $RT_PRIV \
    --subnet-id $SUBNET_PRIV_A
aws ec2 associate-route-table \
    --route-table-id $RT_PRIV \
    --subnet-id $SUBNET_PRIV_B
echo "NAT Gateway: $NAT_GW"
echo "Private Route Table: $RT_PRIV"

# Save variables for later use
echo "export VPC_ID=$VPC_ID" > vpc_variables.sh
echo "export SUBNET_PUB_A=$SUBNET_PUB_A" >> vpc_variables.sh
echo "export SUBNET_PUB_B=$SUBNET_PUB_B" >> vpc_variables.sh
echo "export SUBNET_PRIV_A=$SUBNET_PRIV_A" >> vpc_variables.sh
echo "export SUBNET_PRIV_B=$SUBNET_PRIV_B" >> vpc_variables.sh
echo "export RT_PUB=$RT_PUB" >> vpc_variables.sh
echo "export RT_PRIV=$RT_PRIV" >> vpc_variables.sh
echo "export IGW_ID=$IGW_ID" >> vpc_variables.sh
```

### Lab 2: Launch Template and Auto Scaling Group

```bash
# Source VPC variables
source vpc_variables.sh

# Step 1: Create security group for web servers
SG_WEB=$(aws ec2 create-security-group \
    --group-name web-server-sg \
    --description "Security group for web servers" \
    --vpc-id $VPC_ID \
    --query 'GroupId' \
    --output text)

# Add inbound rules
aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 80 \
    --cidr 0.0.0.0/0
aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 443 \
    --cidr 0.0.0.0/0
aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 22 \
    --cidr 10.0.0.0/16

echo "Web Security Group: $SG_WEB"

# Step 2: Create IAM role for EC2
aws iam create-role \
    --role-name ec2-web-role \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"Service": "ec2.amazonaws.com"},
            "Action": "sts:AssumeRole"
        }]
    }'

# Attach S3 read policy
aws iam put-role-policy \
    --role-name ec2-web-role \
    --policy-name s3-read \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": ["s3:GetObject"],
            "Resource": "arn:aws:s3:::*/static-assets/*"
        }]
    }'

# Create instance profile
aws iam create-instance-profile \
    --instance-profile-name web-server-profile
aws iam add-role-to-instance-profile \
    --instance-profile-name web-server-profile \
    --role-name ec2-web-role

# Step 3: Create Launch Template
aws ec2 create-launch-template \
    --launch-template-name web-server-lt \
    --version-description "Version 1" \
    --launch-template-data "ImageId=ami-0c55b159cbfafe1f0,InstanceType=t3.micro,KeyName=my-key,SecurityGroupIds=[$SG_WEB],IamInstanceProfile={Arn=arn:aws:iam::$(aws sts get-caller-identity --query Account --output text):instance-profile/web-server-profile},UserData=$(echo -n '#!/bin/bash
yum update -y
yum install -y httpd
systemctl start httpd
systemctl enable httpd
echo "<h1>Hello from $(hostname -f)</h1>" > /var/www/html/index.html' | base64)"

echo "Launch Template created"

# Step 4: Create Auto Scaling Group
aws autoscaling create-auto-scaling-group \
    --auto-scaling-group-name web-server-asg \
    --launch-template "LaunchTemplateName=web-server-lt,Version=1" \
    --min-size 2 \
    --max-size 10 \
    --desired-capacity 2 \
    --vpc-zone-identifier "$SUBNET_PUB_A,$SUBNET_PUB_B" \
    --health-check-type ELB \
    --health-check-grace-period 300

# Add scaling policies
aws autoscaling put-scaling-policy \
    --auto-scaling-group-name web-server-asg \
    --policy-name scale-out \
    --adjustment-type PercentChangeInCapacity \
    --scaling-adjustment 50 \
    --cooldown 300

aws autoscaling put-scaling-policy \
    --auto-scaling-group-name web-server-asg \
    --policy-name scale-in \
    --adjustment-type PercentChangeInCapacity \
    --scaling-adjustment -30 \
    --cooldown 300

echo "Auto Scaling Group created"
```

### Lab 3: Application Load Balancer

```bash
# Step 1: Create security group for ALB
SG_ALB=$(aws ec2 create-security-group \
    --group-name alb-sg \
    --description "Security group for ALB" \
    --vpc-id $VPC_ID \
    --query 'GroupId' \
    --output text)

aws ec2 authorize-security-group-ingress \
    --group-id $SG_ALB \
    --protocol tcp \
    --port 80 \
    --cidr 0.0.0.0/0

aws ec2 authorize-security-group-ingress \
    --group-id $SG_ALB \
    --protocol tcp \
    --port 443 \
    --cidr 0.0.0.0/0

echo "ALB Security Group: $SG_ALB"

# Step 2: Create target group
TG_ARN=$(aws elbv2 create-target-group \
    --name web-tg \
    --protocol HTTP \
    --port 80 \
    --vpc-id $VPC_ID \
    --health-check-path /health \
    --health-check-interval-seconds 30 \
    --healthy-threshold-count 2 \
    --unhealthy-threshold-count 3 \
    --query 'TargetGroups[0].TargetGroupArn' \
    --output text)

echo "Target Group: $TG_ARN"

# Step 3: Create Application Load Balancer
ALB_ARN=$(aws elbv2 create-load-balancer \
    --name web-alb \
    --scheme internet-facing \
    --type application \
    --subnets $SUBNET_PUB_A $SUBNET_PUB_B \
    --security-groups $SG_ALB \
    --query 'LoadBalancers[0].LoadBalancerArn' \
    --output text)

# Wait for ALB to be active
aws elbv2 wait load-balancer-available \
    --load-balancer-arns $ALB_ARN

# Step 4: Create listener
aws elbv2 create-listener \
    --load-balancer-arn $ALB_ARN \
    --protocol HTTP \
    --port 80 \
    --default-actions Type=forward,TargetGroupArn=$TG_ARN

echo "ALB created: $ALB_ARN"

# Step 5: Attach ASG to ALB
aws autoscaling attach-load-balancer-target-groups \
    --auto-scaling-group-name web-server-asg \
    --target-group-arns $TG_ARN
```

### Lab 4: S3 and CloudFront Setup

```bash
# Step 1: Create S3 bucket for static assets
aws s3 mb s3://my-webapp-assets-$RANDOM

# Enable static website hosting
aws s3 website s3://my-webapp-assets \
    --index-document index.html

# Upload sample content
echo "<h1>Static Assets Work!</h1>" | aws s3 cp - s3://my-webapp-assets/index.html

# Set bucket policy for public read
aws s3api put-bucket-policy \
    --bucket my-webapp-assets \
    --policy '{
        "Version": "2012-10-17",
        "Statement": [{
            "Sid": "PublicReadGetObject",
            "Effect": "Allow",
            "Principal": "*",
            "Action": "s3:GetObject",
            "Resource": "arn:aws:s3:::my-webapp-assets/*"
        }]
    }'

echo "S3 bucket created"

# Step 2: Create CloudFront distribution
DISTRIBUTION_ID=$(aws cloudfront create-distribution \
    --origin-domain-name my-webapp-assets.s3.amazonaws.com \
    --default-root-object index.html \
    --query 'Distribution.Id' \
    --output text)

echo "CloudFront Distribution: $DISTRIBUTION_ID"

# Get CloudFront domain
aws cloudfront get-distribution \
    --id $DISTRIBUTION_ID \
    --query 'Distribution.DomainName'
```

### Lab 5: Terraform Equivalent (Infrastructure as Code)

```hcl
# main.tf - Complete infrastructure
provider "aws" {
  region = "us-east-1"
}

# VPC
resource "aws_vpc" "main" {
  cidr_block           = "10.0.0.0/16"
  enable_dns_hostnames = true
  tags = { Name = "main-vpc" }
}

# Subnets
resource "aws_subnet" "public_a" {
  vpc_id                  = aws_vpc.main.id
  cidr_block             = "10.0.1.0/24"
  availability_zone      = "us-east-1a"
  map_public_ip_on_launch = true
}

resource "aws_subnet" "public_b" {
  vpc_id                  = aws_vpc.main.id
  cidr_block             = "10.0.2.0/24"
  availability_zone      = "us-east-1b"
  map_public_ip_on_launch = true
}

resource "aws_subnet" "private_a" {
  vpc_id                  = aws_vpc.main.id
  cidr_block             = "10.0.10.0/24"
  availability_zone      = "us-east-1a"
}

resource "aws_subnet" "private_b" {
  vpc_id                  = aws_vpc.main.id
  cidr_block             = "10.0.20.0/24"
  availability_zone      = "us-east-1b"
}

# Internet Gateway
resource "aws_internet_gateway" "main" {
  vpc_id = aws_vpc.main.id
}

# NAT Gateway
resource "aws_eip" "nat" {
  domain = aws_vpc.main.id
}

resource "aws_nat_gateway" "main" {
  allocation_id = aws_eip.nat.id
  subnet_id     = aws_subnet.public_a.id
}

# Route Tables
resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.main.id
  }
}

resource "aws_route_table" "private" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block     = "0.0.0.0/0"
    nat_gateway_id = aws_nat_gateway.main.id
  }
}

# Security Groups
resource "aws_security_group" "web" {
  name        = "web-sg"
  description = "Security group for web servers"
  vpc_id      = aws_vpc.main.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol   = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Launch Template
resource "aws_launch_template" "web" {
  name_prefix   = "web-lt-"
  image_id      = "ami-0c55b159cbfafe1f0"
  instance_type = "t3.micro"
  key_name      = "my-key"

  network_interfaces {
    subnet_id       = aws_subnet.public_a.id
    security_groups = [aws_security_group.web.id]
  }

  user_data = base64encode(templatefile("user_data.sh", {}))
}

# Auto Scaling Group
resource "aws_autoscaling_group" "web" {
  name                = "web-asg"
  vpc_zone_identifier = [aws_subnet.public_a.id, aws_subnet.public_b.id]

  desired_capacity = 2
  min_size         = 2
  max_size         = 10

  launch_template {
    id = aws_launch_template.web.id
  }

  tag {
    key                 = "Name"
    value               = "web-server"
    propagate_at_launch = true
  }
}

# Application Load Balancer
resource "aws_lb" "web" {
  name               = "web-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.web.id]
  subnets            = [aws_subnet.public_a.id, aws_subnet.public_b.id]
}

resource "aws_lb_target_group" "web" {
  name     = "web-tg"
  port     = 80
  protocol = "HTTP"
  vpc_id   = aws_vpc.main.id
}

resource "aws_lb_listener" "web" {
  load_balancer_arn = aws_lb.web.arn
  port             = 80
  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.web.arn
  }
}

# Attach ASG to ALB
resource "aws_autoscaling_attachment" "asg_attachment" {
  autoscaling_group_name = aws_autoscaling_group.web.name
  lb_target_group_arn   = aws_lb_target_group.web.arn
}
```

## COMMON ISSUES

### 1. Permission Denied Errors

**Problem**: EC2 cannot access S3 or communicate with ALB.

**Solution**: Verify IAM role and security group rules.

### 2. Health Check Failures

**Problem**: Instances fail health checks, causing constant replacement.

**Solution**: Verify:
- /health endpoint returns 200
- Security group allows ALB to reach instances
- User data script completed successfully

### 3. VPC DNS Resolution Issues

**Problem**: Instances cannot resolve internal names.

**Solution**: Enable DNS support in VPC:
```bash
aws ec2 modify-vpc-attribute \
    --vpc-id $VPC_ID \
    --enable-dns-support '{"Value": true}'
```

### 4. Auto Scaling Not Triggering

**Problem**: ASG doesn't scale under load.

**Solution**: Verify:
- CloudWatch metrics are being published
- Scaling policies are attached
- Cooldown period has elapsed

### 5. Cost Overruns

**Problem**: Resources running unexpectedly cost money.

**Solution**: Always set up:
- Cost alerts
- Auto cleanup for dev resources
- AWS Budgets with actions

## VERIFICATION

### Check Resources Created

```bash
# Verify VPC and subnets
aws ec2 describe-vpcs --filters "Name=vpc-id,Values=$VPC_ID"
aws ec2 describe-subnets --filters "Name=vpc-id,Values=$VPC_ID"

# Verify Lambda auto scaling group
aws autoscaling describe-auto-scaling-groups \
    --auto-scaling-group-names web-server-asg

# Verify ALB
aws elbv2 describe-load-balancers --names web-alb

# Verify S3 bucket
aws s3 ls | grep webapp-assets

# Check CloudFront
aws cloudfront list-distributions
```

### Cleanup Commands

```bash
# Delete all lab resources
aws autoscaling delete-auto-scaling-group --force-delete --auto-scaling-group-name web-server-asg
aws elbv2 delete-load-balancer-arn $ALB_ARN --force
aws ec2 delete-launch-template --launch-template-name web-server-lt
aws s3 rb s3://my-webapp-assets --force
aws cloudfront delete-distribution --id $DISTRIBUTION_ID --if-match $(aws cloudfront get-distribution --id $DISTRIBUTION_ID --query 'Distribution.ETag' --output text)

# Delete VPC (wait for NAT Gateway to delete first)
aws ec2 delete-nat-gateway --nat-gateway-id $NAT_GW
sleep 60
aws ec2 delete-subnet --subnet-id $SUBNET_PUB_A
aws ec2 delete-subnet --subnet-id $SUBNET_PUB_B
aws ec2 delete-subnet --subnet-id $SUBNET_PRIV_A
aws ec2 delete-subnet --subnet-id $SUBNET_PRIV_B
aws ec2 delete-route-table --route-table-id $RT_PRIV
aws ec2 delete-route-table --route-table-id $RT_PUB
aws ec2 detach-internet-gateway --internet-gateway-id $IGW_ID --vpc-id $VPC_ID
aws ec2 delete-internet-gateway --internet-gateway-id $IGW_ID
aws ec2 delete-vpc --vpc-id $VPC_ID
```

## CROSS-REFERENCES

### Related Labs

- **Practical EC2**: Deploying to ASG is direct continuation
- **Practical RDS**: For database layer addition
- **Practical VPC**: Additional networking practices
- **Cost Management**: Tracking lab costs

### Multi-Cloud Labs

This lab has equivalents in:
- **Azure**: VM Scale Sets + Application Gateway
- **GCP**: Managed Instance Groups + Cloud Load Balancing

### Prerequisites to Complete

1. Basic Cloud Concepts (01_Basic)
2. Advanced Cloud Concepts (02_Advanced)