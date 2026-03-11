# Cloud Deployment to AWS/Azure

## Concept Title and Overview

Cloud deployment is the process of hosting and running applications on cloud infrastructure platforms instead of on-premises servers. This lesson covers deploying Spring Boot applications to Amazon Web Services (AWS) and Microsoft Azure, two of the most popular cloud platforms used in enterprise environments.

## Real-World Importance and Context

Cloud deployment provides scalability, reliability, and global reach for modern applications. Major cloud platforms include:
- **Amazon Web Services (AWS)** - The most comprehensive cloud platform
- **Microsoft Azure** - Enterprise-friendly cloud with strong Windows integration
- **Google Cloud Platform (GCP)** - Strong in data analytics and machine learning

### Why Deploy to the Cloud?

| Benefit | Description |
|---------|-------------|
| **Scalability** | Automatically scale up/down based on traffic |
| **Reliability** | Built-in redundancy and failover |
| **Global Reach** | Deploy closer to users worldwide |
| **Cost Efficiency** | Pay only for what you use |
| **Managed Services** | Reduce operational overhead |

---

## Key Concepts and Definitions

### 1. Platform as a Service (PaaS)
A cloud computing model where the provider manages the underlying infrastructure, allowing developers to focus on application code.

### 2. Infrastructure as a Service (IaaS)
Virtualized computing resources over the internet, giving you more control over infrastructure.

### 3. Elastic Beanstalk
AWS's PaaS offering that automatically handles deployment, capacity provisioning, load balancing, and health monitoring.

### 4. Azure App Service
Microsoft's fully managed platform for building, deploying, and scaling web apps.

### 5. Container Registry
A storage location for container images (e.g., AWS ECR, Azure Container Registry).

### 6. Environment Variables
Configuration values that change based on the deployment environment (development, staging, production).

---

## Detailed Step-by-Step Explanation

### AWS Deployment Options

#### Option 1: AWS Elastic Beanstalk (Recommended for Beginners)

Elastic Beanstalk is the easiest way to deploy Spring Boot applications on AWS. It handles all the infrastructure details automatically.

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    AWS ELASTIC BEANSTALK                                 │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  Upload your application                                                │
│  (JAR/WAR file or Docker)                                              │
│           │                                                             │
│           ▼                                                             │
│  ┌─────────────────────────────────────────────────────────────────┐ │
│  │  Elastic Beanstalk                                               │ │
│  │  • Automatically provisions infrastructure                      │ │
│  │  • Manages capacity, load balancing, scaling                    │ │
│  │  • Handles deployment                                           │ │
│  └─────────────────────────────────────────────────────────────────┘ │
│           │                                                             │
│           ▼                                                             │
│  Your application is live!                                              │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

**Step-by-Step Deployment:**

1. **Prepare Your Application**
   ```bash
   # Build the JAR file
   ./mvnw clean package -DskipTests
   
   # Verify the JAR exists
   ls -la target/*.jar
   ```

2. **Install EB CLI**
   ```bash
   pip install awsebcli
   ```

3. **Initialize EB CLI**
   ```bash
   # Navigate to your project directory
   cd task-manager-backend
   
   # Initialize EB with Java
   eb init -p java-17 task-manager
   
   # Follow prompts:
   # - Select region
   # - Choose application name
   # - Set up SSH (optional)
   ```

4. **Create Environment and Deploy**
   ```bash
   # Create production environment
   eb create production-env --instance-type t3.small --database --database-engine mysql
   
   # Or deploy to existing environment
   eb deploy production-env
   ```

5. **Configure Environment Variables**
   ```bash
   # Set environment variables
   eb setenv DB_HOST=mydb.xxx.us-east-1.rds.amazonaws.com \
             DB_PASSWORD=your_secure_password \
             JWT_SECRET=your_jwt_secret
   ```

6. **Open Your Application**
   ```bash
   eb open
   ```

#### Option 2: AWS ECS (Elastic Container Service)

For Docker-based deployments, use ECS with Fargate.

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    AWS ECS DEPLOYMENT                                    │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐                 │
│  │  Developer  │───▶│    ECR      │───▶│    ECS      │                 │
│  │    Push     │    │  Registry   │    │  Fargate    │                 │
│  └─────────────┘    └─────────────┘    └─────────────┘                 │
│                                                 │                       │
│                                                 ▼                       │
│                                        ┌─────────────┐                 │
│                                        │  Application│                 │
│                                        │   Running   │                 │
│                                        └─────────────┘                 │
└─────────────────────────────────────────────────────────────────────────┘
```

**ECS Deployment Steps:**

1. **Create ECR Repository**
   ```bash
   # Create repository
   aws ecr create-repository --repository-name task-manager-backend
   
   # Login to ECR
   aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin your-account.dkr.ecr.us-east-1.amazonaws.com
   ```

2. **Build and Push Docker Image**
   ```bash
   # Build image
   docker build -t task-manager-backend .
   
   # Tag for ECR
   docker tag task-manager-backend:latest your-account.dkr.ecr.us-east-1.amazonaws.com/task-manager-backend:latest
   
   # Push to ECR
   docker push your-account.dkr.ecr.us-east-1.amazonaws.com/task-manager-backend:latest
   ```

3. **Create Task Definition (task-definition.json)**
   ```json
   {
     "family": "task-manager-backend",
     "networkMode": "awsvpc",
     "requiresCompatibilities": ["FARGATE"],
     "cpu": "512",
     "memory": "1024",
     "executionRoleArn": "arn:aws:iam::123456789:role/ecsTaskExecutionRole",
     "containerDefinitions": [
       {
         "name": "backend",
         "image": "123456789.dkr.ecr.us-east-1.amazonaws.com/task-manager-backend:latest",
         "essential": true,
         "portMappings": [
           {
             "containerPort": 8080,
             "protocol": "tcp"
           }
         ],
         "environment": [
           {
             "name": "SPRING_PROFILES_ACTIVE",
             "value": "prod"
           }
         ],
         "secrets": [
           {
             "name": "DB_PASSWORD",
             "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:db-password"
           }
         ]
       }
     ]
   }
   ```

4. **Register Task and Create Service**
   ```bash
   # Register task definition
   aws ecs register-task-definition --cli-input-json file://task-definition.json
   
   # Create ECS service
   aws ecs create-service \
     --cluster task-manager-cluster \
     --service-name task-manager-backend \
     --task-definition task-manager-backend:1 \
     --desired-count 2 \
     --launch-type FARGATE \
     --network-configuration "awsvpcConfiguration={subnets=[subnet-xxx,subnet-yyy],securityGroups=[sg-zzz]}"
   ```

---

### Azure Deployment Options

#### Option 1: Azure App Service (Recommended for Beginners)

Azure App Service is a fully managed platform for web applications.

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    AZURE APP SERVICE                                     │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                      Azure App Service                          │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐              │   │
│  │  │  Instance 1 │  │  Instance 2 │  │  Instance 3 │              │   │
│  │  │  (Tomcat)   │  │  (Tomcat)   │  │  (Tomcat)   │              │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘              │   │
│  │         │               │               │                       │   │
│  │         └───────────────┼───────────────┘                       │   │
│  │                         ▼                                         │   │
│  │                  Load Balancer                                   │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                              │                                           │
│                              ▼                                           │
│                    Your Application URL                                 │
└─────────────────────────────────────────────────────────────────────────┘
```

**Step-by-Step Deployment:**

1. **Login to Azure**
   ```bash
   # Login via browser
   az login
   
   # Set subscription (if multiple)
   az account set --subscription "Your Subscription Name"
   ```

2. **Create Resource Group**
   ```bash
   # Create resource group
   az group create --name task-manager-rg --location eastus
   
   # Verify
   az group show --name task-manager-rg
   ```

3. **Create App Service Plan**
   ```bash
   # Create App Service Plan (Pricing tier)
   az appservice plan create \
     --name task-manager-plan \
     --resource-group task-manager-rg \
     --sku B1 \  # Basic tier; use S1 for Standard
     --is-linux
   ```

4. **Create Web App**
   ```bash
   # Create the web app
   az webapp create \
     --name task-manager-app \
     --resource-group task-manager-rg \
     --plan task-manager-plan \
     --runtime "JAVA|17-java17" \
     --deployment-local-git
   ```

5. **Configure Application Settings**
   ```bash
   # Set environment variables
   az webapp config appsettings set \
     --name task-manager-app \
     --resource-group task-manager-rg \
     --settings \
       SPRING_PROFILES_ACTIVE=prod \
       DB_HOST=mydb.mysql.database.azure.com \
       DB_PASSWORD=@YourSecurePassword123
   ```

6. **Deploy Your Application**
   
   **Option A: Using Git**
   ```bash
   # Get deployment URL
   az webapp deployment source config-local-git \
     --name task-manager-app \
     --resource-group task-manager-rg
   
   # Add Azure as remote
   git remote add azure <deployment-url>
   
   # Push to Azure
   git push azure main
   ```
   
   **Option B: Using Maven**
   ```bash
   # Add Azure plugin to pom.xml
   # Then deploy
   mvn azure-webapp:deploy
   ```
   
   **Option C: Using Azure CLI**
   ```bash
   # Package first
   ./mvnw clean package -DskipTests
   
   # Deploy JAR
   az webapp deploy \
     --name task-manager-app \
     --resource-group task-manager-rg \
     --src-path target/task-manager-0.0.1-SNAPSHOT.jar
   ```

7. **Verify Deployment**
   ```bash
   # Browse to app
   az webapp show --name task-manager-app --resource-group task-manager-rg --query "defaultHostName"
   
   # View logs
   az webapp log tail --name task-manager-app --resource-group task-manager-rg
   ```

#### Option 2: Azure Container Instances (ACI)

For container-based deployments:

```bash
# Create container instance
az container create \
  --resource-group task-manager-rg \
  --name task-manager-backend \
  --image myregistry.azurecr.io/task-manager-backend:latest \
  --cpu 1.0 \
  --memory 1.5 \
  --port 8080 \
  --environment-variables SPRING_PROFILES_ACTIVE=prod \
  --secure-environment-variables DB_PASSWORD=YourSecurePassword \
  --acr-username $(az acr credential show -n myregistry --query username -o tsv) \
  --acr-password $(az acr credential show -n myregistry --query passwords[0].value -o tsv)
```

---

### Database Configuration for Cloud Deployment

#### AWS RDS (Relational Database Service)

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    AWS RDS CONFIGURATION                                 │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────────┐                                                    │
│  │  Spring Boot    │                                                    │
│  │  Application    │                                                    │
│  └────────┬────────┘                                                    │
│           │ JDBC Connection                                            │
│           ▼                                                            │
│  ┌─────────────────┐                                                    │
│  │   AWS RDS       │                                                    │
│  │  ┌───────────┐  │                                                    │
│  │  │  MySQL/   │  │                                                    │
│  │  │ PostgreSQL│  │                                                    │
│  │  └───────────┘  │                                                    │
│  └─────────────────┘                                                    │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

**application.properties for AWS RDS:**
```properties
# AWS RDS Configuration
spring.datasource.url=jdbc:mysql://mydb.xxx.us-east-1.rds.amazonaws.com:3306/taskmanager?useSSL=false&allowPublicKeyRetrieval=true&serverTimezone=UTC
spring.datasource.username=admin
spring.datasource.password=${DB_PASSWORD}
spring.datasource.driver-class-name=com.mysql.cj.jdbc.Driver

# JPA/Hibernate
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=false
spring.jpa.properties.hibernate.dialect=org.hibernate.dialect.MySQL8Dialect

# Connection Pool
spring.datasource.hikari.maximum-pool-size=10
spring.datasource.hikari.minimum-idle=5
spring.datasource.hikari.connection-timeout=20000
spring.datasource.hikari.idle-timeout=300000
spring.datasource.hikari.max-lifetime=1200000
```

#### Azure Database for MySQL/PostgreSQL

```properties
# Azure Database Configuration
spring.datasource.url=jdbc:mysql://mydb.mysql.database.azure.com:3306/taskmanager?useSSL=true&serverTimezone=UTC&requireSSL=true
spring.datasource.username=admin@mydb
spring.datasource.password=${DB_PASSWORD}
spring.datasource.driver-class-name=com.mysql.cj.jdbc.Driver

# JPA/Hibernate
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=false
spring.jpa.properties.hibernate.dialect=org.hibernate.dialect.MySQL8Dialect
```

---

### Environment-Specific Configuration

#### Spring Profiles
Use Spring profiles for different environments:

```properties
# application-prod.properties
server.port=8080
server.compression.enabled=true

# Database - AWS
spring.datasource.url=jdbc:mysql://prod-db.xxx.rds.amazonaws.com:3306/taskmanager

# Logging
logging.level.root=INFO
logging.level.com.taskmanager=DEBUG

# Security
jwt.expiration=86400000
```

#### Deployment Configuration Files

```
src/
└── main/
    ├── resources/
    │   ├── application.properties      # Default config
    │   ├── application-dev.properties  # Development
    │   ├── application-prod.properties # Production
    │   └── application.yml             # Alternative format
    └── docker/
        └── Dockerfile
```

---

### SSL/TLS Configuration

#### AWS Certificate Manager

```bash
# Request certificate (done in AWS Console)
# Then configure Elastic Beanstalk
eb setenv SERVER_SSL_ENABLED=true
```

#### Azure App Service (Let's Encrypt or Custom)

```bash
# Add custom domain with SSL (Azure Portal)
# Or use Azure Let's Encrypt extension
az webapp config ssl bind \
  --certificate-thumbprint <thumbprint> \
  --name task-manager-app \
  --resource-group task-manager-rg
```

---

### Monitoring and Logging

#### AWS CloudWatch
```bash
# View logs
aws logs describe-log-groups --log-group-name-prefix /aws/elasticbeanstalk

# Stream logs
aws logs tail /aws/elasticbeanstalk/task-manager/prod.log --follow
```

#### Azure Application Insights
```xml
<!-- Add to pom.xml -->
<dependency>
    <groupId>com.microsoft.azure</groupId>
    <artifactId>applicationinsights-spring-boot-starter</artifactId>
    <version>2.6.4</version>
</dependency>
```

```properties
# application.properties
spring.applicationinsights.connection.string=${APPLICATIONINSIGHTS_CONNECTION_STRING}
```

---

## Docker Compose for Cloud

```yaml
version: '3.8'

services:
  backend:
    image: myregistry/task-manager-backend:latest
    deploy:
      replicas: 2
      resources:
        limits:
          cpus: '0.5'
          memory: 512M
        reservations:
          cpus: '0.25'
          memory: 256M
    environment:
      - SPRING_PROFILES_ACTIVE=prod
      - DB_HOST=database
    depends_on:
      - database
    networks:
      - app-network

  database:
    image: mysql:8.0
    deploy:
      resources:
        limits:
          cpus: '0.5'
          memory: 1G
    environment:
      MYSQL_ROOT_PASSWORD: ${DB_ROOT_PASSWORD}
      MYSQL_DATABASE: taskmanager
      MYSQL_USER: appuser
      MYSQL_PASSWORD: ${DB_PASSWORD}
    volumes:
      - mysql-data:/var/lib/mysql
    networks:
      - app-network
    healthcheck:
      test: ["CMD", "mysqladmin", "ping", "-h", "localhost"]
      interval: 10s
      timeout: 5s
      retries: 5

  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf:ro
    depends_on:
      - backend
    networks:
      - app-network

networks:
  app-network:
    driver: bridge

volumes:
  mysql-data:
```

---

## CI/CD Integration with Cloud

### GitHub Actions - AWS Elastic Beanstalk

```yaml
name: Deploy to AWS Elastic Beanstalk

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Set up JDK 17
        uses: actions/setup-java@v3
        with:
          java-version: '17'
          distribution: 'temurin'
          
      - name: Build with Maven
        run: ./mvnw clean package -DskipTests
        
      - name: Configure AWS credentials
        uses: aws-actions/configure-aws-credentials@v2
        with:
          aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
          aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
          aws-region: us-east-1
          
      - name: Deploy to EB
        run: |
          zip -r deploy.zip target/*.jar .ebextensions
          aws elasticbeanstalk create-application-version \
            --application-name task-manager \
            --version-label v1 \
            --source-bundle S3Bucket=${{ secrets.S3_BUCKET }},S3Key=deploy.zip
          aws elasticbeanstalk update-environment \
            --environment-name production-env \
            --version-label v1
```

### GitHub Actions - Azure App Service

```yaml
name: Deploy to Azure

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Set up JDK 17
        uses: actions/setup-java@v3
        with:
          java-version: '17'
          distribution: 'temurin'
          
      - name: Build with Maven
        run: ./mvnw clean package -DskipTests
        
      - name: Login to Azure
        uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}
          
      - name: Deploy to Azure Web App
        uses: azure/webapps-deploy@v3
        with:
          app-name: task-manager-app
          package: target/*.jar
          
      - name: Azure logout
        run: |
          az logout
```

---

## Practice Questions

### Multiple Choice Questions

1. **Which AWS service is a Platform as a Service (PaaS) offering?**
   - A) EC2
   - B) S3
   - C) Elastic Beanstalk
   - D) Lambda
   
   **Answer: C** - Elastic Beanstalk is AWS's PaaS offering that manages infrastructure automatically.

2. **What is the default port for Spring Boot applications?**
   - A) 80
   - B) 443
   - C) 8080
   - D) 3000
   
   **Answer: C** - Spring Boot applications run on port 8080 by default.

3. **Which Azure service is used to deploy containerized applications?**
   - A) Azure App Service
   - B) Azure Container Instances
   - C) Azure Functions
   - D) Azure Storage
   
   **Answer: B** - Azure Container Instances (ACI) is designed for container deployments.

4. **What Spring profile should be used for production deployments?**
   - A) development
   - B) test
   - C) prod
   - D) default
   
   **Answer: C** - The `prod` profile is conventionally used for production environments.

### Fill in the Blanks

5. **AWS RDS stands for ____________________________________________________.**
   
   **Answer:** Amazon Relational Database Service

6. **The Azure CLI command to create a resource group is ______________________.**
   
   **Answer:** `az group create`

7. **To store sensitive configuration in Azure, you should use ______________________.**
   
   **Answer:** Azure Key Vault or secure environment variables

### Hands-On Exercises

8. **Exercise 1:** Deploy your Task Manager Spring Boot application to AWS Elastic Beanstalk with the following requirements:
   - Create an EB environment with MySQL database
   - Configure environment variables for database connection
   - Deploy the application and verify it's accessible

9. **Exercise 2:** Deploy the same application to Azure App Service:
   - Create a resource group and App Service plan
   - Configure application settings
   - Deploy using Azure CLI or GitHub Actions

10. **Exercise 3:** Set up CI/CD pipeline:
    - Create a GitHub Actions workflow
    - Configure automatic deployment to your chosen cloud platform
    - Test the pipeline by pushing code changes

---

## Summary

Cloud deployment transforms how applications are hosted and managed, offering unparalleled scalability, reliability, and cost efficiency. This lesson covered the essential concepts and practical implementations for deploying Spring Boot applications to both AWS and Azure platforms.

### Key Takeaways

1. **AWS Elastic Beanstalk** provides the easiest path to deploy Spring Boot applications on AWS with automatic infrastructure management.

2. **Azure App Service** offers enterprise-ready deployment with strong integration and management capabilities.

3. **Docker containers** enable consistent deployments across different cloud platforms and environments.

4. **Spring profiles** allow you to maintain environment-specific configurations for development, staging, and production.

5. **CI/CD integration** automates the deployment process, reducing manual errors and enabling rapid iteration.

6. **Database services** like AWS RDS and Azure Database provide managed database solutions that integrate seamlessly with your deployed applications.

7. **Environment variables** are crucial for storing sensitive information like database passwords and API keys securely.

### Next Steps

- Practice deploying sample applications to both AWS and Azure
- Explore more advanced topics like Kubernetes deployment (covered in Topic 33)
- Learn about cloud cost optimization techniques
- Implement monitoring and alerting for your deployed applications

### Additional Resources

- [AWS Elastic Beanstalk Documentation](https://docs.aws.amazon.com/elastic-beanstalk/)
- [Azure App Service Documentation](https://docs.microsoft.com/azure/app-service/)
- [Spring Boot Cloud Deploy Guide](https://spring.io/guides/gs/spring-boot-aws/)

---

**Congratulations!** You have completed the Cloud Deployment lesson. You now have the skills to deploy your full-stack Angular + Spring Boot applications to production cloud environments.
