# What is Jenkins?

## What this covers

This guide explains what Jenkins is, the concepts of CI/CD (Continuous Integration/Continuous Deployment), and how Jenkins fits into a modern DevOps workflow. You'll learn why Jenkins has become one of the most popular automation servers in the software industry.

## CI/CD: The Problem Jenkins Solves

### The Old Way: Manual Deployments

Imagine a team of developers working on a web application. Each developer writes code on their own computer. When they think their code is ready, they manually:

1. Copy their code changes to a shared server
2. Manually run tests on that server
3. Manually restart the application
4. Hope nothing breaks in production

This approach is error-prone, time-consuming, and stressful. Mistakes happen when humans perform repetitive tasks manually.

### CI/CD: Automated Software Delivery

CI/CD is a methodology that automates the entire process of getting code from a developer's laptop to production:

- **Continuous Integration (CI)**: Developers merge their code changes frequently (multiple times per day). Each merge triggers an automated build and test process to detect problems early.

- **Continuous Deployment (CD)**: After code passes all tests, it's automatically deployed to production or staging environments without manual intervention.

- **Continuous Delivery**: Similar to Continuous Deployment, but a human may manually approve the final production deployment step.

### Jenkins in the CI/CD Pipeline

Jenkins is an **open-source automation server** that orchestrates this entire CI/CD pipeline. Think of Jenkins as a robot assistant that:

- Watches for code changes in your repository
- Downloads the latest code
- Runs your build commands (compiling, bundling, packaging)
- Executes your tests
- Deploys the application to servers
- Sends notifications about the results

## Jenkins as a Robot Assistant

Here's an analogy that helps visualize Jenkins:

> **Jenkins is like a tireless robot assistant sitting in your server room.**
> 
> You give this robot a checklist of tasks to perform (defined in a Jenkinsfile). Whenever you push new code to your repository, the robot:
> 1. Receives a notification (webhook)
> 2. Fetches your latest code
> 3. Follows your checklist step-by-step
> 4. Reports back with the results (success or failure)
> 5. Never gets tired, never forgets a step, and works 24/7

This robot doesn't just run one type of task—it can:
- Compile code in any programming language
- Run test suites
- Build Docker containers
- Deploy to AWS, Azure, Kubernetes, or any cloud
- Send Slack messages, emails, or create GitHub PR comments
- Run security scans
- Generate documentation
- And much more, thanks to thousands of plugins

## Key Jenkins Concepts

### Jobs and Builds

A **job** (or "project") is a configuration that tells Jenkins what to do. Each time Jenkins executes a job, it creates a **build**. Build #1, Build #2, etc. Each build has:
- A unique build number
- A build log showing what happened
- Artifacts (files produced by the build)
- A status: SUCCESS, FAILURE, UNSTABLE, or ABORTED

### Pipelines

A **pipeline** is a more sophisticated way to define CI/CD workflows. Instead of a simple list of commands, a pipeline defines stages (like "Build", "Test", "Deploy") with conditions, parallel execution, and more.

Modern Jenkins pipelines are defined in a **Jenkinsfile**—a text file stored in your source code repository alongside your code. This is called "Pipeline as Code" and provides version control for your automation.

### Agents and Executors

Jenkins can distribute work across multiple machines using **agents** (also called "nodes"). An **executor** is a slot where a build can run. A Jenkins master can have multiple executors, and each agent can provide additional executors.

### Plugins

Jenkins has over 1,800 plugins that extend its functionality. Plugins integrate Jenkins with:
- Version control systems (Git, Subversion, Mercurial)
- Build tools (Maven, Gradle, npm, CMake)
- Cloud platforms (AWS, Azure, Google Cloud)
- Containers (Docker, Kubernetes)
- Testing frameworks (JUnit, TestNG, Selenium)
- Communication tools (Slack, Discord, Teams, Email)
- Security tools (SonarQube, OWASP)

## Why Jenkins?

Jenkins has been the #1 choice for CI/CD for over 15 years because:

1. **Free and Open Source**: No licensing costs
2. **Plugin Ecosystem**: 1,800+ plugins for virtually any integration
3. **Cross-Platform**: Runs on Windows, Linux, macOS, and more
4. **Flexible**: Can be simple (one job) or enterprise-grade (distributed builds)
5. **Strong Community**: Extensive documentation, tutorials, and support
6. **Pipeline as Code**: Jenkinsfiles version control your automation
7. **Industry Standard**: Most DevOps jobs require Jenkins experience

## Next Steps

Now that you understand what Jenkins is and why it matters, continue to:

- **[Install Jenkins on Ubuntu](02-install-on-ubuntu.md)** - Set up Jenkins on a Linux server
- **[Install Jenkins with Docker](03-install-with-docker.md)** - Run Jenkins in a container
- **[Complete the Setup Wizard](04-initial-setup-wizard.md)** - Configure your new Jenkins instance
