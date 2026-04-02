# Infrastructure as Code with Terraform

## What You'll Learn

- Terraform comprehensive setup
- AWS infrastructure provisioning
- Docker Compose for development
- Kubernetes manifests management
- IaC best practices

## Terraform AWS Setup

```hcl
# terraform/main.tf
terraform {
  required_version = ">= 1.5"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  backend "s3" {
    bucket         = "my-terraform-state"
    key            = "prod/terraform.tfstate"
    region         = "us-east-1"
    dynamodb_table = "terraform-locks"
    encrypt        = true
  }
}

provider "aws" {
  region = var.aws_region

  default_tags {
    tags = {
      Project     = var.project_name
      Environment = var.environment
      ManagedBy   = "terraform"
    }
  }
}

# ─────────────── VPC ───────────────
module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "5.0"

  name = "${var.project_name}-${var.environment}"
  cidr = "10.0.0.0/16"

  azs             = ["${var.aws_region}a", "${var.aws_region}b", "${var.aws_region}c"]
  private_subnets = ["10.0.1.0/24", "10.0.2.0/24", "10.0.3.0/24"]
  public_subnets  = ["10.0.101.0/24", "10.0.102.0/24", "10.0.103.0/24"]

  enable_nat_gateway   = true
  single_nat_gateway   = var.environment != "production"
  enable_dns_hostnames = true
}

# ─────────────── ECS Cluster ───────────────
resource "aws_ecs_cluster" "main" {
  name = "${var.project_name}-${var.environment}"

  setting {
    name  = "containerInsights"
    value = "enabled"
  }
}

resource "aws_ecs_task_definition" "app" {
  family                   = "${var.project_name}-app"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = var.task_cpu
  memory                   = var.task_memory
  execution_role_arn       = aws_iam_role.ecs_execution.arn
  task_role_arn            = aws_iam_role.ecs_task.arn

  container_definitions = jsonencode([{
    name  = "app"
    image = "${aws_ecr_repository.app.repository_url}:${var.image_tag}"

    portMappings = [{
      containerPort = 3000
      protocol      = "tcp"
    }]

    environment = [
      { name = "NODE_ENV", value = var.environment },
      { name = "PORT", value = "3000" },
    ]

    secrets = [
      { name = "DATABASE_URL", valueFrom = aws_secretsmanager_secret.db_url.arn },
      { name = "JWT_SECRET", valueFrom = aws_secretsmanager_secret.jwt_secret.arn },
    ]

    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group"         = "/ecs/${var.project_name}"
        "awslogs-region"        = var.aws_region
        "awslogs-stream-prefix" = "app"
      }
    }

    healthCheck = {
      command     = ["CMD-SHELL", "curl -f http://localhost:3000/health || exit 1"]
      interval    = 30
      timeout     = 5
      retries     = 3
      startPeriod = 60
    }
  }])
}

resource "aws_ecs_service" "app" {
  name            = "${var.project_name}-app"
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.app.arn
  desired_count   = var.desired_count
  launch_type     = "FARGATE"

  network_configuration {
    subnets          = module.vpc.private_subnets
    security_groups  = [aws_security_group.app.id]
    assign_public_ip = false
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.app.arn
    container_name   = "app"
    container_port   = 3000
  }

  deployment_circuit_breaker {
    enable   = true
    rollback = true
  }

  deployment_configuration {
    maximum_percent         = 200
    minimum_healthy_percent = 100
  }
}

# ─────────────── RDS Database ───────────────
resource "aws_db_instance" "main" {
  identifier     = "${var.project_name}-${var.environment}"
  engine         = "postgres"
  engine_version = "15"
  instance_class = var.db_instance_class

  allocated_storage     = 20
  max_allocated_storage = 100
  storage_encrypted     = true

  db_name  = var.db_name
  username = var.db_username
  password = var.db_password

  db_subnet_group_name   = aws_db_subnet_group.main.name
  vpc_security_group_ids = [aws_security_group.db.id]

  backup_retention_period = var.environment == "production" ? 30 : 7
  multi_az                = var.environment == "production"
  skip_final_snapshot     = var.environment != "production"

  tags = {
    Name = "${var.project_name}-db"
  }
}

# ─────────────── Variables ───────────────
# terraform/variables.tf
variable "aws_region" {
  default = "us-east-1"
}

variable "project_name" {
  default = "my-node-app"
}

variable "environment" {
  type    = string
  default = "staging"
}

variable "image_tag" {
  type = string
}

variable "task_cpu" {
  default = 256
}

variable "task_memory" {
  default = 512
}

variable "desired_count" {
  default = 2
}

variable "db_instance_class" {
  default = "db.t3.micro"
}

variable "db_name" {
  default = "myapp"
}

variable "db_username" {
  sensitive = true
}

variable "db_password" {
  sensitive = true
}

# ─────────────── Outputs ───────────────
# terraform/outputs.tf
output "app_url" {
  value = "https://${aws_lb.main.dns_name}"
}

output "ecr_repository_url" {
  value = aws_ecr_repository.app.repository_url
}

output "db_endpoint" {
  value     = aws_db_instance.main.endpoint
  sensitive = true
}
```

## Docker Compose for Development

```yaml
# docker-compose.yml
version: '3.8'

services:
  app:
    build:
      context: .
      dockerfile: Dockerfile
      target: development
    ports:
      - "3000:3000"
      - "9229:9229" # Debug port
    volumes:
      - .:/app
      - /app/node_modules
    environment:
      - NODE_ENV=development
      - DATABASE_URL=postgresql://user:pass@postgres:5432/myapp_dev
      - REDIS_URL=redis://redis:6379
    depends_on:
      postgres:
        condition: service_healthy
      redis:
        condition: service_started
    command: npm run dev

  postgres:
    image: postgres:15-alpine
    ports:
      - "5432:5432"
    environment:
      POSTGRES_DB: myapp_dev
      POSTGRES_USER: user
      POSTGRES_PASSWORD: pass
    volumes:
      - postgres-data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U user -d myapp_dev"]
      interval: 5s
      timeout: 5s
      retries: 5

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis-data:/data

  test:
    build:
      context: .
      dockerfile: Dockerfile
      target: test
    environment:
      - DATABASE_URL=postgresql://user:pass@postgres-test:5432/myapp_test
    depends_on:
      postgres-test:
        condition: service_healthy
    command: npm test

  postgres-test:
    image: postgres:15-alpine
    environment:
      POSTGRES_DB: myapp_test
      POSTGRES_USER: user
      POSTGRES_PASSWORD: pass
    tmpfs:
      - /var/lib/postgresql/data

volumes:
  postgres-data:
  redis-data:
```

## Best Practices Checklist

- [ ] Use remote state with locking (S3 + DynamoDB)
- [ ] Separate state per environment
- [ ] Use modules for reusable infrastructure
- [ ] Tag all resources consistently
- [ ] Use sensitive variables for secrets
- [ ] Run `terraform plan` before `apply`
- [ ] Store Terraform in version control
- [ ] Use Docker Compose for local development

## Cross-References

- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for patterns
- See [Kubernetes](../03-container-orchestration/01-kubernetes-patterns.md) for K8s
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for automation
- See [Security](../09-deployment-security/01-security-scanning.md) for hardening

## Next Steps

Continue to [Container Security](../07-container-security/01-image-scanning.md).
