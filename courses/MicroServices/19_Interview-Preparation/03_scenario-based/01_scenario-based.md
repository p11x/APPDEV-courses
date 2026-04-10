# Scenario-Based Microservices Questions

## Overview

Scenario-based questions present real-world situations and evaluate your problem-solving approach. These questions assess how you apply microservices concepts to solve practical challenges.

## Sample Scenarios

### Scenario 1: Service is experiencing high latency

A payment service shows 500ms latency (normally 50ms). How would you diagnose and fix this?

**Approach**:
1. Check recent deployments
2. Review metrics (CPU, memory, DB)
3. Analyze trace data
4. Check for dependency issues
5. Implement fix and monitor

### Scenario 2: Data inconsistency between services

Two services showing different user balance amounts. What's your approach?

**Approach**:
1. Identify affected services
2. Check transaction logs
3. Implement reconciliation
4. Fix the root cause
5. Add safeguards

### Scenario 3: Need to decompose a monolith

Management wants to break a 5-year-old monolith into microservices. How do you start?

**Approach**:
1. Conduct domain analysis
2. Identify bounded contexts
3. Analyze dependencies
4. Choose migration strategy
5. Plan incremental extraction

## Output

```
Scenario Questions: 15
Average Preparation Time: 30 minutes each

Focus Areas:
- Performance Issues: 5
- Data Problems: 4
- Migration: 3
- Security: 3
```
