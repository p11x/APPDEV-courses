# Capacity Planning, Cost Optimization & Continuous Improvement

> **Chapter:** 10 - Deployment | **Section:** 12 - Deployment Best Practices
>
> **Previous:** [02 - Automation & Troubleshooting](./02-automation-troubleshooting.md)
>
> **Return to:** [Chapter 10 Overview](../README.md)

---

## Table of Contents

1. [Capacity Planning Deep Dive](#capacity-planning-deep-dive)
2. [Auto-Scaling Policy Design](#auto-scaling-policy-design)
3. [Cost Optimization (FinOps)](#cost-optimization-finops)
4. [Cloud Provider Cost Management](#cloud-provider-cost-management)
5. [Right-Sizing Analysis](#right-sizing-analysis)
6. [Spot/Preemptible Instance Strategies](#spot-preemptible-instance-strategies)
7. [Reserved Capacity Planning](#reserved-capacity-planning)
8. [Deployment Continuous Improvement](#deployment-continuous-improvement)
9. [DORA Metrics Implementation](#dora-metrics-implementation)
10. [Deployment Maturity Model](#deployment-maturity-model)
11. [Technology Radar for Deployment Tools](#technology-radar-for-deployment-tools)
12. [Chapter Summary](#chapter-summary)
13. [Cross-References](#cross-references)

---

## Capacity Planning Deep Dive

### Load Testing for Capacity

```javascript
// capacity/load-test.js - Artillery-based capacity testing
const artillery = require('artillery');

module.exports = {
  config: {
    target: 'https://api.example.com',
    phases: [
      // Phase 1: Warm-up
      { duration: 60, arrivalRate: 10, name: 'warm-up' },
      // Phase 2: Baseline load
      { duration: 120, arrivalRate: 50, name: 'baseline' },
      // Phase 3: Peak load simulation
      { duration: 180, arrivalRate: 200, name: 'peak' },
      // Phase 4: Stress test (beyond expected)
      { duration: 120, arrivalRate: 500, name: 'stress' },
      // Phase 5: Recovery observation
      { duration: 60, arrivalRate: 50, name: 'recovery' },
    ],
    plugins: {
      'metrics-by-endpoint': {
        useOnlyRequestNames: true,
      },
    },
    ensure: {
      p95: 500, // 95th percentile under 500ms
      maxErrorRate: 0.01, // Less than 1% errors
    },
  },
  scenarios: [
    {
      name: 'API Read Operations',
      weight: 70,
      flow: [
        { get: { url: '/api/v1/items', name: 'List Items' } },
        { think: 1 },
        {
          get: {
            url: '/api/v1/items/{{ $randomInt(1, 1000) }}',
            name: 'Get Item',
          },
        },
        { think: 2 },
      ],
    },
    {
      name: 'API Write Operations',
      weight: 20,
      flow: [
        {
          post: {
            url: '/api/v1/items',
            name: 'Create Item',
            json: {
              name: 'Test Item {{ $randomInt(1, 100000) }}',
              description: 'Load test item',
            },
          },
        },
        { think: 3 },
      ],
    },
    {
      name: 'Search Operations',
      weight: 10,
      flow: [
        {
          get: {
            url: '/api/v1/search?q=test&page={{ $randomInt(1, 10) }}',
            name: 'Search',
          },
        },
      ],
    },
  ],
};
```

### Capacity Calculator

```javascript
// capacity/calculator.js - Capacity planning calculator
class CapacityCalculator {
  constructor(metrics) {
    this.metrics = metrics;
  }

  /**
   * Calculate required instances based on traffic projections
   */
  calculateRequiredInstances(projection) {
    const {
      expectedRPS,        // Requests per second
      avgResponseTime,    // Average response time in ms
      targetUtilization,  // Target CPU utilization (0-1)
      headroom,           // Safety headroom multiplier
      peakMultiplier,     // Peak traffic multiplier
    } = projection;

    // Calculate based on Little's Law: N = λ × W
    // N = number of concurrent requests
    // λ = arrival rate (RPS)
    // W = response time (seconds)
    const concurrentRequests = expectedRPS * (avgResponseTime / 1000);

    // Apply peak multiplier
    const peakConcurrent = concurrentRequests * peakMultiplier;

    // Calculate instances needed per core capacity
    const requestsPerInstance = this.metrics.requestsPerCore || 100;
    const coresPerInstance = this.metrics.coresPerInstance || 2;

    const instancesForLoad = Math.ceil(
      peakConcurrent / (requestsPerInstance * coresPerInstance)
    );

    // Apply utilization target and headroom
    const instancesWithUtilization = Math.ceil(
      instancesForLoad / targetUtilization
    );
    const instancesWithHeadroom = Math.ceil(
      instancesWithUtilization * headroom
    );

    return {
      baselineInstances: instancesForLoad,
      withUtilizationTarget: instancesWithUtilization,
      recommendedInstances: instancesWithHeadroom,
      details: {
        expectedConcurrentRequests: concurrentRequests,
        peakConcurrentRequests: peakConcurrent,
        requestsPerInstance: requestsPerInstance * coresPerInstance,
        effectiveCapacity: instancesWithHeadroom * requestsPerInstance * coresPerInstance,
      },
    };
  }

  /**
   * Calculate storage requirements
   */
  calculateStorageNeeds(dataProfile) {
    const {
      recordsPerDay,
      avgRecordSizeBytes,
      retentionDays,
      indexOverheadMultiplier,
      replicationFactor,
    } = dataProfile;

    const dailyGrowth = recordsPerDay * avgRecordSizeBytes;
    const totalData = dailyGrowth * retentionDays;
    const withIndexes = totalData * indexOverheadMultiplier;
    const withReplication = withIndexes * replicationFactor;

    return {
      dailyGrowthBytes: dailyGrowth,
      dailyGrowthGB: (dailyGrowth / (1024 ** 3)).toFixed(2),
      totalStorageGB: (withReplication / (1024 ** 3)).toFixed(2),
      projectedYearlyGB: ((dailyGrowth * 365 * indexOverheadMultiplier * replicationFactor) / (1024 ** 3)).toFixed(2),
    };
  }

  /**
   * Calculate bandwidth requirements
   */
  calculateBandwidth(traffic) {
    const {
      requestsPerSecond,
      avgRequestSizeKB,
      avgResponseSizeKB,
      peakMultiplier,
    } = traffic;

    const avgBandwidthMbps =
      (requestsPerSecond * (avgRequestSizeKB + avgResponseSizeKB) * 8) / 1000;
    const peakBandwidthMbps = avgBandwidthMbps * peakMultiplier;

    return {
      avgBandwidthMbps: avgBandwidthMbps.toFixed(2),
      peakBandwidthMbps: peakBandwidthMbps.toFixed(2),
      monthlyTransferGB: (
        (avgBandwidthMbps * 86400 * 30) /
        8 /
        1024
      ).toFixed(2),
    };
  }

  /**
   * Forecast growth over time
   */
  forecastGrowth(currentMetrics, growthRate, months = 12) {
    const forecast = [];
    let currentRPS = currentMetrics.requestsPerSecond;
    let currentStorage = currentMetrics.storageGB;
    let currentInstances = currentMetrics.instances;

    for (let month = 1; month <= months; month++) {
      currentRPS *= 1 + growthRate.monthly / 100;
      currentStorage *= 1 + growthRate.storageMonthly / 100;

      const requiredInstances = this.calculateRequiredInstances({
        expectedRPS: currentRPS,
        avgResponseTime: currentMetrics.avgResponseTime,
        targetUtilization: 0.7,
        headroom: 1.3,
        peakMultiplier: currentMetrics.peakMultiplier || 2,
      });

      forecast.push({
        month,
        projectedRPS: Math.round(currentRPS),
        projectedStorageGB: currentStorage.toFixed(2),
        requiredInstances: requiredInstances.recommendedInstances,
        estimatedCost: this.estimateMonthlyCost(
          requiredInstances.recommendedInstances,
          currentStorage
        ),
      });
    }

    return forecast;
  }

  estimateMonthlyCost(instances, storageGB) {
    const computeCostPerInstance = 80; // USD per month
    const storageCostPerGB = 0.10;    // USD per GB per month

    return (
      instances * computeCostPerInstance +
      storageGB * storageCostPerGB
    ).toFixed(2);
  }
}

// Usage example
const calculator = new CapacityCalculator({
  requestsPerCore: 150,
  coresPerInstance: 2,
});

const currentMetrics = {
  requestsPerSecond: 500,
  avgResponseTime: 120,
  peakMultiplier: 3,
  storageGB: 250,
  instances: 4,
};

const forecast = calculator.forecastGrowth(
  currentMetrics,
  { monthly: 15, storageMonthly: 10 },
  12
);

console.log('12-Month Capacity Forecast:');
console.table(forecast);

module.exports = { CapacityCalculator };
```

---

## Auto-Scaling Policy Design

### Predictive Scaling

```javascript
// scaling/predictive-scaler.js - ML-based predictive scaling
const { CloudWatchClient, GetMetricStatisticsCommand } = require('@aws-sdk/client-cloudwatch');
const { AutoScalingClient, SetDesiredCapacityCommand } = require('@aws-sdk/client-auto-scaling');

class PredictiveScaler {
  constructor(config) {
    this.cloudwatch = new CloudWatchClient({ region: config.region });
    this.autoscaling = new AutoScalingClient({ region: config.region });
    this.asgName = config.asgName;
    this.namespace = config.namespace || 'AWS/ApplicationELB';
  }

  /**
   * Analyze historical traffic patterns and predict future load
   */
  async analyzeTrafficPattern(days = 30) {
    const endTime = new Date();
    const startTime = new Date(endTime.getTime() - days * 24 * 60 * 60 * 1000);

    const command = new GetMetricStatisticsCommand({
      Namespace: this.namespace,
      MetricName: 'RequestCount',
      Dimensions: [{ Name: 'LoadBalancer', Value: this.config.loadBalancer }],
      StartTime: startTime,
      EndTime: endTime,
      Period: 3600, // 1 hour periods
      Statistics: ['Sum'],
    });

    const response = await this.cloudwatch.send(command);
    const datapoints = response.Datapoints.sort(
      (a, b) => new Date(a.Timestamp) - new Date(b.Timestamp)
    );

    // Build hourly pattern model
    const hourlyPattern = this.buildHourlyPattern(datapoints);
    const dailyPattern = this.buildDailyPattern(datapoints);

    return {
      hourlyPattern,
      dailyPattern,
      prediction: this.predictNextWeek(hourlyPattern, dailyPattern),
    };
  }

  buildHourlyPattern(datapoints) {
    const hourlyAvg = new Array(24).fill(0);
    const hourlyCount = new Array(24).fill(0);

    for (const dp of datapoints) {
      const hour = new Date(dp.Timestamp).getUTCHours();
      hourlyAvg[hour] += dp.Sum;
      hourlyCount[hour]++;
    }

    return hourlyAvg.map((sum, i) =>
      hourlyCount[i] > 0 ? Math.round(sum / hourlyCount[i]) : 0
    );
  }

  buildDailyPattern(datapoints) {
    const dailyAvg = new Array(7).fill(0);
    const dailyCount = new Array(7).fill(0);

    for (const dp of datapoints) {
      const day = new Date(dp.Timestamp).getDay();
      dailyAvg[day] += dp.Sum;
      dailyCount[day]++;
    }

    return dailyAvg.map((sum, i) =>
      dailyCount[i] > 0 ? Math.round(sum / dailyCount[i]) : 0
    );
  }

  predictNextWeek(hourlyPattern, dailyPattern) {
    const predictions = [];
    const now = new Date();

    for (let day = 0; day < 7; day++) {
      const targetDate = new Date(now.getTime() + day * 24 * 60 * 60 * 1000);
      const dayOfWeek = targetDate.getDay();
      const dailyMultiplier = dailyPattern[dayOfWeek] / (dailyPattern.reduce((a, b) => a + b, 0) / 7);

      for (let hour = 0; hour < 24; hour++) {
        const predictedRPS = Math.round(hourlyPattern[hour] * dailyMultiplier);
        predictions.push({
          timestamp: new Date(
            targetDate.getFullYear(),
            targetDate.getMonth(),
            targetDate.getDate(),
            hour
          ).toISOString(),
          predictedRPS,
          recommendedInstances: this.rpsToInstances(predictedRPS),
        });
      }
    }

    return predictions;
  }

  rpsToInstances(rps, rpsPerInstance = 100) {
    const baseInstances = Math.ceil(rps / rpsPerInstance);
    const withHeadroom = Math.ceil(baseInstances * 1.3);
    return Math.max(2, Math.min(withHeadroom, 50)); // Min 2, max 50
  }

  /**
   * Schedule scaling actions based on predictions
   */
  async scheduleScaling(predictions) {
    for (const prediction of predictions) {
      const targetTime = new Date(prediction.timestamp);
      const now = new Date();

      if (targetTime <= now) continue;

      const preWarmTime = new Date(targetTime.getTime() - 10 * 60 * 1000); // 10 min before

      if (preWarmTime > now) {
        const delay = preWarmTime.getTime() - now.getTime();

        setTimeout(async () => {
          console.log(`Pre-warming: Setting capacity to ${prediction.recommendedInstances} for ${prediction.timestamp}`);
          await this.setCapacity(prediction.recommendedInstances);
        }, delay);
      }
    }
  }

  async setCapacity(desiredCapacity) {
    const command = new SetDesiredCapacityCommand({
      AutoScalingGroupName: this.asgName,
      DesiredCapacity: desiredCapacity,
      HonorCooldown: true,
    });

    await this.autoscaling.send(command);
  }
}

module.exports = { PredictiveScaler };
```

### Target Tracking Policy

```yaml
# scaling/target-tracking-policy.yaml
# AWS Auto Scaling - Target Tracking Configuration
Resources:
  NodeAppScalingPolicy:
    Type: AWS::AutoScaling::ScalingPolicy
    Properties:
      AutoScalingGroupName: !Ref NodeAppASG
      PolicyType: TargetTrackingScaling
      TargetTrackingConfiguration:
        PredefinedMetricSpecification:
          PredefinedMetricType: ASGAverageCPUUtilization
        TargetValue: 70.0
        ScaleInCooldown: 300
        ScaleOutCooldown: 60
        DisableScaleIn: false

  RequestCountScalingPolicy:
    Type: AWS::AutoScaling::ScalingPolicy
    Properties:
      AutoScalingGroupName: !Ref NodeAppASG
      PolicyType: TargetTrackingScaling
      TargetTrackingConfiguration:
        PredefinedMetricSpecification:
          PredefinedMetricType: ALBRequestCountPerTarget
          ResourceLabel: !Sub '${ALB.FullName}/${TargetGroup.FullName}'
        TargetValue: 1000.0
        ScaleInCooldown: 300
        ScaleOutCooldown: 60

  CustomMetricScalingPolicy:
    Type: AWS::AutoScaling::ScalingPolicy
    Properties:
      AutoScalingGroupName: !Ref NodeAppASG
      PolicyType: TargetTrackingScaling
      TargetTrackingConfiguration:
        CustomizedMetricSpecification:
          MetricName: EventLoopLatency
          Namespace: NodeJS/Application
          Statistic: p95
          Unit: Milliseconds
          Dimensions:
            - Name: Application
              Value: node-api
        TargetValue: 50.0
        ScaleInCooldown: 300
        ScaleOutCooldown: 60
```

### Step Scaling with Custom Metrics

```javascript
// scaling/step-scaler.js - Step scaling based on custom metrics
const { CloudWatchClient, PutMetricAlarmCommand } = require('@aws-sdk/client-cloudwatch');
const { AutoScalingClient, PutScalingPolicyCommand } = require('@aws-sdk/client-auto-scaling');

class StepScaler {
  constructor(config) {
    this.cloudwatch = new CloudWatchClient({ region: config.region });
    this.autoscaling = new AutoScalingClient({ region: config.region });
    this.asgName = config.asgName;
  }

  async createStepScalingPolicies() {
    // Scale out policy (add instances)
    await this.autoscaling.send(new PutScalingPolicyCommand({
      AutoScalingGroupName: this.asgName,
      PolicyName: 'cpu-scale-out',
      PolicyType: 'StepScaling',
      AdjustmentType: 'ChangeInCapacity',
      StepAdjustments: [
        { MetricIntervalLowerBound: 0, MetricIntervalUpperBound: 10, ScalingAdjustment: 1 },
        { MetricIntervalLowerBound: 10, MetricIntervalUpperBound: 20, ScalingAdjustment: 2 },
        { MetricIntervalLowerBound: 20, ScalingAdjustment: 4 },
      ],
      Cooldown: 60,
    }));

    // Scale in policy (remove instances)
    await this.autoscaling.send(new PutScalingPolicyCommand({
      AutoScalingGroupName: this.asgName,
      PolicyName: 'cpu-scale-in',
      PolicyType: 'StepScaling',
      AdjustmentType: 'ChangeInCapacity',
      StepAdjustments: [
        { MetricIntervalUpperBound: 0, MetricIntervalLowerBound: -10, ScalingAdjustment: -1 },
        { MetricIntervalUpperBound: -10, ScalingAdjustment: -2 },
      ],
      Cooldown: 300,
    }));

    // Create alarms
    await this.createAlarm('cpu-high', 'CPUUtilization', 'GreaterThanThreshold', 70, 'cpu-scale-out');
    await this.createAlarm('cpu-critical', 'CPUUtilization', 'GreaterThanThreshold', 85, 'cpu-scale-out');
    await this.createAlarm('cpu-low', 'CPUUtilization', 'LessThanThreshold', 30, 'cpu-scale-in');
  }

  async createAlarm(alarmName, metric, operator, threshold, policyName) {
    await this.cloudwatch.send(new PutMetricAlarmCommand({
      AlarmName: `${this.asgName}-${alarmName}`,
      MetricName: metric,
      Namespace: 'AWS/EC2',
      Statistic: 'Average',
      Period: 60,
      EvaluationPeriods: 2,
      Threshold,
      ComparisonOperator: operator,
      AlarmActions: [
        `arn:aws:autoscaling:${process.env.AWS_REGION}:${process.env.AWS_ACCOUNT_ID}:scalingPolicy:${policyName}`,
      ],
      Dimensions: [{ Name: 'AutoScalingGroupName', Value: this.asgName }],
    }));
  }
}

module.exports = { StepScaler };
```

---

## Cost Optimization (FinOps)

### Cost Allocation Tags Strategy

```yaml
# cost/allocation-tags.yaml
# Comprehensive tagging strategy for cost allocation
Tags:
  # Required cost allocation tags
  - Key: 'cost:center'
    Description: 'Finance cost center code'
    Required: true
    Example: 'CC-ENG-4501'

  - Key: 'project'
    Description: 'Project identifier'
    Required: true
    Example: 'ecommerce-platform'

  - Key: 'environment'
    Description: 'Deployment environment'
    Required: true
    AllowedValues: ['production', 'staging', 'development', 'sandbox']

  - Key: 'team'
    Description: 'Owning team'
    Required: true
    Example: 'platform-engineering'

  - Key: 'service'
    Description: 'Service name'
    Required: true
    Example: 'payment-api'

  # Optional enrichment tags
  - Key: 'owner'
    Description: 'Technical owner email'
    Required: false

  - Key: 'business-unit'
    Description: 'Business unit for chargeback'
    Required: false

  - Key: 'data-classification'
    Description: 'Data sensitivity level'
    AllowedValues: ['public', 'internal', 'confidential', 'restricted']

# Terraform tagging module
resource "aws_default_tags" "cost_allocation" {
  tags = {
    "cost:center"       = var.cost_center
    "project"           = var.project_name
    "environment"       = var.environment
    "team"              = var.team_name
    "managed-by"        = "terraform"
    "last-updated"      = timestamp()
  }
}
```

### Budget Alert System

```javascript
// cost/budget-alerts.js - Multi-channel budget monitoring
const { CostExplorerClient, GetCostAndUsageCommand } = require('@aws-sdk/client-cost-explorer');
const { SNSClient, PublishCommand } = require('@aws-sdk/client-sns');

class BudgetMonitor {
  constructor(config) {
    this.costExplorer = new CostExplorerClient({ region: 'us-east-1' });
    this.sns = new SNSClient({ region: config.region });
    this.budgets = config.budgets;
    this.alertTopic = config.alertTopicArn;
  }

  async checkBudgets() {
    for (const budget of this.budgets) {
      const currentSpend = await this.getCurrentSpend(budget);
      const percentUsed = (currentSpend / budget.limit) * 100;

      if (percentUsed >= 100) {
        await this.sendAlert(budget, currentSpend, 'EXCEEDED');
      } else if (percentUsed >= budget.alertThresholds.critical) {
        await this.sendAlert(budget, currentSpend, 'CRITICAL');
      } else if (percentUsed >= budget.alertThresholds.warning) {
        await this.sendAlert(budget, currentSpend, 'WARNING');
      } else if (percentUsed >= budget.alertThresholds.info) {
        await this.sendAlert(budget, currentSpend, 'INFO');
      }
    }
  }

  async getCurrentSpend(budget) {
    const now = new Date();
    const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);

    const response = await this.costExplorer.send(new GetCostAndUsageCommand({
      TimePeriod: {
        Start: startOfMonth.toISOString().split('T')[0],
        End: now.toISOString().split('T')[0],
      },
      Granularity: 'MONTHLY',
      Metrics: ['UnblendedCost'],
      Filter: budget.filter || undefined,
      GroupBy: budget.groupBy || undefined,
    }));

    return response.ResultsByTime.reduce((total, period) => {
      return total + parseFloat(period.Total.UnblendedCost.Amount);
    }, 0);
  }

  async sendAlert(budget, currentSpend, severity) {
    const percentUsed = ((currentSpend / budget.limit) * 100).toFixed(1);
    const remaining = (budget.limit - currentSpend).toFixed(2);
    const daysInMonth = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).getDate();
    const daysElapsed = new Date().getDate();
    const projectedSpend = (currentSpend / daysElapsed) * daysInMonth;

    const message = {
      severity,
      budget: budget.name,
      limit: `$${budget.limit}`,
      currentSpend: `$${currentSpend.toFixed(2)}`,
      percentUsed: `${percentUsed}%`,
      remaining: `$${remaining}`,
      projectedMonthlySpend: `$${projectedSpend.toFixed(2)}`,
      daysRemaining: daysInMonth - daysElapsed,
      timestamp: new Date().toISOString(),
    };

    await this.sns.send(new PublishCommand({
      TopicArn: this.alertTopic,
      Subject: `[${severity}] Budget Alert: ${budget.name} at ${percentUsed}%`,
      Message: JSON.stringify(message, null, 2),
    }));
  }

  async getCostBreakdown(startDate, endDate, granularity = 'SERVICE') {
    const response = await this.costExplorer.send(new GetCostAndUsageCommand({
      TimePeriod: { Start: startDate, End: endDate },
      Granularity: 'MONTHLY',
      Metrics: ['UnblendedCost', 'UsageQuantity'],
      GroupBy: [{ Type: 'DIMENSION', Key: granularity }],
    }));

    return response.ResultsByTime.map((period) => ({
      period: period.TimePeriod,
      services: period.Groups.map((group) => ({
        name: group.Keys[0],
        cost: parseFloat(group.Metrics.UnblendedCost.Amount),
        usage: parseFloat(group.Metrics.UsageQuantity.Amount),
      })).sort((a, b) => b.cost - a.cost),
    }));
  }
}

// Configuration example
const monitor = new BudgetMonitor({
  region: 'us-east-1',
  alertTopicArn: 'arn:aws:sns:us-east-1:123456789:budget-alerts',
  budgets: [
    {
      name: 'Production Monthly',
      limit: 5000,
      alertThresholds: { info: 50, warning: 75, critical: 90 },
      filter: {
        Dimensions: { Key: 'TAG:environment', Values: ['production'] },
      },
    },
    {
      name: 'Development Monthly',
      limit: 1000,
      alertThresholds: { info: 60, warning: 80, critical: 95 },
      filter: {
        Dimensions: { Key: 'TAG:environment', Values: ['development'] },
      },
    },
  ],
});

module.exports = { BudgetMonitor };
```

---

## Cloud Provider Cost Management

### AWS Cost Optimization

```javascript
// cost/aws-optimizer.js - AWS-specific cost optimization
const { CostExplorerClient, GetRightsizingRecommendationCommand } = require('@aws-sdk/client-cost-explorer');
const { PricingClient, GetProductsCommand } = require('@aws-sdk/client-pricing');
const { EC2Client, DescribeInstancesCommand, DescribeSpotPriceHistoryCommand } = require('@aws-sdk/client-ec2');

class AWSCostOptimizer {
  constructor(region = 'us-east-1') {
    this.costExplorer = new CostExplorerClient({ region });
    this.pricing = new PricingClient({ region: 'us-east-1' }); // Pricing API only in us-east-1
    this.ec2 = new EC2Client({ region });
  }

  async getRightsizingRecommendations() {
    const response = await this.costExplorer.send(new GetRightsizingRecommendationCommand({
      Service: 'AmazonEC2',
      Configuration: {
        RecommendationTarget: 'SAME_INSTANCE_FAMILY',
        BenefitsConsidered: true,
      },
    }));

    return response.RightsizingRecommendations.map((rec) => ({
      currentInstance: rec.CurrentInstance,
      recommendation: rec.RightsizingType,
      details: rec.ModifyRecommendationDetail || rec.TerminateRecommendationDetail,
      estimatedMonthlySavings: rec.EstimatedMonthlySavings,
    }));
  }

  async analyzeUnusedResources() {
    // Find unattached EBS volumes
    // Find unused Elastic IPs
    // Find idle load balancers
    // Find old snapshots
    return {
      unattachedVolumes: await this.findUnattachedVolumes(),
      unusedEIPs: await this.findUnusedEIPs(),
      idleLoadBalancers: await this.findIdleLoadBalancers(),
    };
  }

  async getSpotInstanceSavings(instanceType, days = 30) {
    const endTime = new Date();
    const startTime = new Date(endTime.getTime() - days * 24 * 60 * 60 * 1000);

    const response = await this.ec2.send(new DescribeSpotPriceHistoryCommand({
      InstanceTypes: [instanceType],
      StartTime: startTime,
      EndTime: endTime,
      ProductDescriptions: ['Linux/UNIX'],
    }));

    const prices = response.SpotPriceHistory.map((h) => parseFloat(h.SpotPrice));
    const avgPrice = prices.reduce((a, b) => a + b, 0) / prices.length;

    // Get on-demand price for comparison
    const onDemandPrice = await this.getOnDemandPrice(instanceType);

    return {
      instanceType,
      avgSpotPrice: avgPrice.toFixed(4),
      onDemandPrice: onDemandPrice.toFixed(4),
      savingsPercent: (((onDemandPrice - avgPrice) / onDemandPrice) * 100).toFixed(1),
      monthlySavings: ((onDemandPrice - avgPrice) * 730).toFixed(2), // ~730 hours/month
    };
  }

  async getOnDemandPrice(instanceType) {
    const response = await this.pricing.send(new GetProductsCommand({
      ServiceCode: 'AmazonEC2',
      Filters: [
        { Type: 'TERM_MATCH', Field: 'instanceType', Value: instanceType },
        { Type: 'TERM_MATCH', Field: 'operatingSystem', Value: 'Linux' },
        { Type: 'TERM_MATCH', Field: 'tenancy', Value: 'Shared' },
        { Type: 'TERM_MATCH', Field: 'preInstalledSw', Value: 'NA' },
        { Type: 'TERM_MATCH', Field: 'capacitystatus', Value: 'Used' },
      ],
    }));

    const product = JSON.parse(response.PriceList[0]);
    const onDemand = product.terms.OnDemand;
    const priceDimensions = Object.values(onDemand)[0].priceDimensions;
    return parseFloat(Object.values(priceDimensions)[0].pricePerUnit.USD);
  }

  generateCostReport() {
    return {
      sections: [
        'Current Month Spend by Service',
        'Month-over-Month Trend',
        'Top 10 Cost Drivers',
        'Rightsizing Opportunities',
        'Reserved Instance Coverage',
        'Savings Plan Utilization',
        'Spot Instance Savings',
        'Storage Optimization',
      ],
    };
  }
}

module.exports = { AWSCostOptimizer };
```

### GCP Cost Management

```javascript
// cost/gcp-optimizer.js - GCP-specific cost optimization
class GCPCostOptimizer {
  constructor(projectId) {
    this.projectId = projectId;
  }

  async getCostBreakdown(startDate, endDate) {
    // Using Cloud Billing API
    const { CloudBillingClient } = require('@google-cloud/billing');
    const billing = new CloudBillingClient();

    // Get project billing info
    const [billingInfo] = await billing.getProjectBillingInfo({
      name: `projects/${this.projectId}`,
    });

    return {
      billingAccount: billingInfo.billingAccountName,
      isEnabled: billingInfo.billingEnabled,
    };
  }

  async getCommittedUseDiscountRecommendations() {
    // Analyze usage patterns for CUD recommendations
    return {
      recommendations: [
        {
          resource: 'n2-standard-4',
          currentSpend: 500,
          recommendedCommitment: '1-year',
          estimatedSavings: 175, // 35% savings
        },
      ],
    };
  }

  async analyzePreemptibleSavings() {
    return {
      preemptibleDiscount: '60-91%',
      considerations: [
        'Max 24-hour runtime',
        'Can be terminated at any time',
        'Not suitable for stateful workloads',
        'Great for batch processing and CI/CD',
      ],
    };
  }
}

module.exports = { GCPCostOptimizer };
```

### Azure Cost Management

```javascript
// cost/azure-optimizer.js - Azure-specific cost optimization
class AzureCostOptimizer {
  constructor(subscriptionId) {
    this.subscriptionId = subscriptionId;
  }

  async getCostManagementData(scope, startDate, endDate) {
    // Using Azure Cost Management API
    const { CostManagementClient } = require('@azure/arm-costmanagement');
    // Implementation would query cost data
    return { scope, period: { startDate, endDate } };
  }

  async getReservedInstanceRecommendations() {
    return {
      recommendations: [
        {
          vmSize: 'Standard_D4s_v5',
          term: '1-year',
          savingsPercentage: 40,
          estimatedAnnualSavings: 2400,
        },
      ],
    };
  }

  async getSpotInstanceAnalysis() {
    return {
      spotDiscount: 'Up to 90%',
      evictionPolicy: 'Deallocate or Delete',
      maxLifetime: 'Unlimited (with possible eviction)',
      suitableFor: [
        'Development/test environments',
        'Batch processing',
        'CI/CD pipelines',
        'Non-critical workloads',
      ],
    };
  }
}

module.exports = { AzureCostOptimizer };
```

---

## Right-Sizing Analysis

```javascript
// cost/rightsizing-analyzer.js - Automated right-sizing analysis
const os = require('os');

class RightSizingAnalyzer {
  constructor() {
    this.samples = [];
    this.sampleInterval = null;
  }

  startSampling(intervalMs = 60000) {
    this.sampleInterval = setInterval(() => {
      this.samples.push({
        timestamp: new Date().toISOString(),
        cpuUsage: this.getCPUUsage(),
        memoryUsage: this.getMemoryUsage(),
        eventLoopDelay: this.getEventLoopDelay(),
      });

      // Keep last 24 hours of samples
      const maxSamples = (24 * 60 * 60 * 1000) / intervalMs;
      if (this.samples.length > maxSamples) {
        this.samples.shift();
      }
    }, intervalMs);
  }

  stopSampling() {
    if (this.sampleInterval) {
      clearInterval(this.sampleInterval);
    }
  }

  getCPUUsage() {
    const cpus = os.cpus();
    let totalIdle = 0;
    let totalTick = 0;

    for (const cpu of cpus) {
      for (const type in cpu.times) {
        totalTick += cpu.times[type];
      }
      totalIdle += cpu.times.idle;
    }

    return {
      idlePercent: (totalIdle / totalTick) * 100,
      usedPercent: 100 - (totalIdle / totalTick) * 100,
      cores: cpus.length,
    };
  }

  getMemoryUsage() {
    const total = os.totalmem();
    const free = os.freemem();
    const used = total - free;

    return {
      totalMB: Math.round(total / 1024 / 1024),
      usedMB: Math.round(used / 1024 / 1024),
      freeMB: Math.round(free / 1024 / 1024),
      usedPercent: ((used / total) * 100).toFixed(1),
    };
  }

  getEventLoopDelay() {
    const start = process.hrtime.bigint();
    return new Promise((resolve) => {
      setImmediate(() => {
        const delay = Number(process.hrtime.bigint() - start) / 1e6;
        resolve({ delayMs: delay });
      });
    });
  }

  generateRecommendation() {
    if (this.samples.length < 60) {
      return { error: 'Insufficient data. Need at least 60 samples.' };
    }

    const cpuSamples = this.samples.map((s) => s.cpuUsage.usedPercent);
    const memSamples = this.samples.map((s) => parseFloat(s.memoryUsage.usedPercent));

    const cpuAvg = cpuSamples.reduce((a, b) => a + b, 0) / cpuSamples.length;
    const cpuP95 = cpuSamples.sort((a, b) => a - b)[Math.floor(cpuSamples.length * 0.95)];
    const cpuMax = Math.max(...cpuSamples);

    const memAvg = memSamples.reduce((a, b) => a + b, 0) / memSamples.length;
    const memP95 = memSamples.sort((a, b) => a - b)[Math.floor(memSamples.length * 0.95)];

    const recommendation = {
      currentResources: {
        cpuCores: os.cpus().length,
        memoryMB: Math.round(os.totalmem() / 1024 / 1024),
      },
      utilization: {
        cpu: { avg: cpuAvg.toFixed(1), p95: cpuP95.toFixed(1), max: cpuMax.toFixed(1) },
        memory: { avg: memAvg.toFixed(1), p95: memP95.toFixed(1) },
      },
      recommendation: null,
      estimatedSavings: 0,
    };

    // Right-sizing logic
    if (cpuAvg < 20 && cpuP95 < 40) {
      recommendation.recommendation = 'DOWNSIZE';
      recommendation.details = 'CPU utilization consistently low. Consider reducing CPU allocation.';
      recommendation.suggestedCores = Math.max(1, Math.ceil(os.cpus().length / 2));
    } else if (cpuP95 > 80) {
      recommendation.recommendation = 'UPSIZE';
      recommendation.details = 'CPU utilization approaching limits. Consider increasing CPU allocation.';
      recommendation.suggestedCores = os.cpus().length * 2;
    } else {
      recommendation.recommendation = 'RIGHT-SIZED';
      recommendation.details = 'Current CPU allocation is appropriate.';
    }

    if (memAvg < 30 && memP95 < 50) {
      recommendation.memoryRecommendation = 'DOWNSIZE';
      recommendation.suggestedMemoryMB = Math.round(os.totalmem() / 1024 / 1024 / 2);
    } else if (memP95 > 85) {
      recommendation.memoryRecommendation = 'UPSIZE';
      recommendation.suggestedMemoryMB = Math.round(os.totalmem() / 1024 / 1024 * 1.5);
    } else {
      recommendation.memoryRecommendation = 'RIGHT-SIZED';
    }

    return recommendation;
  }
}

module.exports = { RightSizingAnalyzer };
```

---

## Spot/Preemptible Instance Strategies

```javascript
// cost/spot-strategy.js - Spot instance management
class SpotInstanceManager {
  constructor(config) {
    this.config = config;
    this.interruptionHandlers = [];
  }

  /**
   * Setup graceful spot instance interruption handling
   */
  setupInterruptionHandler() {
    // AWS spot instance interruption notice (2 minutes warning)
    const checkInterruption = async () => {
      try {
        const response = await fetch(
          'http://169.254.169.254/latest/meta-data/spot/instance-action',
          { timeout: 1000 }
        );

        if (response.ok) {
          const action = await response.json();
          console.log('Spot interruption notice received:', action);

          for (const handler of this.interruptionHandlers) {
            await handler(action);
          }

          // Initiate graceful shutdown
          await this.gracefulShutdown(action);
        }
      } catch {
        // No interruption notice - normal operation
      }
    };

    // Check every 5 seconds
    setInterval(checkInterruption, 5000);
  }

  /**
   * Register handlers for pre-interruption tasks
   */
  onInterruption(handler) {
    this.interruptionHandlers.push(handler);
  }

  async gracefulShutdown(action) {
    console.log(`Graceful shutdown initiated. Action: ${action.action}, Time: ${action.time}`);

    // 1. Stop accepting new requests
    process.emit('SIGTERM');

    // 2. Wait for in-flight requests to complete
    await this.drainConnections(30000); // 30 second drain timeout

    // 3. Save state/checkpoint
    await this.saveCheckpoint();

    // 4. Deregister from load balancer
    await this.deregisterFromLB();

    console.log('Graceful shutdown complete');
  }

  async drainConnections(timeoutMs) {
    return new Promise((resolve) => {
      const timeout = setTimeout(resolve, timeoutMs);
      // Implementation would track active connections
      // and resolve when all are complete
    });
  }

  async saveCheckpoint() {
    // Save application state for resumption on new instance
    console.log('Saving checkpoint...');
  }

  async deregisterFromLB() {
    // Deregister from load balancer
    console.log('Deregistering from load balancer...');
  }

  /**
   * Calculate optimal spot bid price
   */
  calculateBidStrategy(spotHistory) {
    const prices = spotHistory.map((h) => h.price);
    const avg = prices.reduce((a, b) => a + b, 0) / prices.length;
    const max = Math.max(...prices);
    const p95 = prices.sort((a, b) => a - b)[Math.floor(prices.length * 0.95)];

    return {
      avgPrice: avg.toFixed(4),
      maxPrice: max.toFixed(4),
      p95Price: p95.toFixed(4),
      recommendedBid: (p95 * 1.1).toFixed(4), // 10% above P95
      onDemandPrice: spotHistory[0]?.onDemandPrice,
      savingsPercent: (((spotHistory[0]?.onDemandPrice - avg) / spotHistory[0]?.onDemandPrice) * 100).toFixed(1),
    };
  }
}

module.exports = { SpotInstanceManager };
```

---

## Reserved Capacity Planning

```javascript
// cost/reserved-planner.js - Reserved capacity planning tool
class ReservedCapacityPlanner {
  constructor() {
    this.instanceTypes = [];
    this.usageHistory = [];
  }

  addInstanceType(type) {
    this.instanceTypes.push(type);
  }

  analyzeUsagePatterns(monthlyUsage) {
    this.usageHistory = monthlyUsage;

    return {
      alwaysOnInstances: this.findAlwaysOnInstances(monthlyUsage),
      variableInstances: this.findVariableInstances(monthlyUsage),
      burstInstances: this.findBurstInstances(monthlyUsage),
    };
  }

  findAlwaysOnInstances(usage) {
    // Instances running >95% of the time across all months
    return usage.filter((m) => m.utilizationPercent > 95);
  }

  findVariableInstances(usage) {
    // Instances with variable usage (50-95%)
    return usage.filter((m) => m.utilizationPercent >= 50 && m.utilizationPercent <= 95);
  }

  findBurstInstances(usage) {
    // Instances used <50% of the time (burst/sporadic)
    return usage.filter((m) => m.utilizationPercent < 50);
  }

  calculateReservedInstanceRecommendations(patterns) {
    const recommendations = [];

    // 1-year reserved for always-on instances
    for (const instance of patterns.alwaysOnInstances) {
      recommendations.push({
        instanceType: instance.type,
        quantity: instance.count,
        term: '1-year',
        paymentOption: 'partial-upfront',
        onDemandCost: instance.monthlyCost * 12,
        reservedCost: instance.monthlyCost * 12 * 0.6, // ~40% savings
        annualSavings: instance.monthlyCost * 12 * 0.4,
        confidence: 'high',
      });
    }

    // Savings Plans for variable instances
    for (const instance of patterns.variableInstances) {
      recommendations.push({
        instanceType: 'Flexible',
        hourlyCommitment: instance.avgHourlySpend,
        term: '1-year',
        paymentOption: 'no-upfront',
        estimatedSavings: instance.monthlyCost * 12 * 0.3, // ~30% savings
        confidence: 'medium',
      });
    }

    // On-demand / spot for burst instances
    for (const instance of patterns.burstInstances) {
      recommendations.push({
        instanceType: instance.type,
        strategy: 'Spot + On-Demand fallback',
        estimatedSpotSavings: instance.monthlyCost * 0.7, // ~70% savings on spot
        confidence: 'medium',
      });
    }

    return recommendations;
  }

  generatePurchasePlan(recommendations, budget) {
    let totalCost = 0;
    const plan = [];

    // Sort by savings percentage
    const sorted = [...recommendations].sort(
      (a, b) => (b.annualSavings || b.estimatedSavings || 0) - (a.annualSavings || a.estimatedSavings || 0)
    );

    for (const rec of sorted) {
      const cost = rec.reservedCost || rec.hourlyCommitment * 8760 || 0;
      if (totalCost + cost <= budget) {
        plan.push(rec);
        totalCost += cost;
      }
    }

    return {
      plan,
      totalAnnualCost: totalCost,
      totalAnnualSavings: plan.reduce((sum, r) => sum + (r.annualSavings || r.estimatedSavings || 0), 0),
      budgetRemaining: budget - totalCost,
    };
  }
}

module.exports = { ReservedCapacityPlanner };
```

---

## Deployment Continuous Improvement

### Deployment Frequency Metrics

```javascript
// metrics/deployment-metrics.js - Track and analyze deployment patterns
class DeploymentMetrics {
  constructor() {
    this.deployments = [];
  }

  record(deployment) {
    this.deployments.push({
      id: deployment.id || Date.now().toString(),
      timestamp: new Date().toISOString(),
      environment: deployment.environment,
      version: deployment.version,
      status: deployment.status, // 'success' | 'failure' | 'rollback'
      durationMs: deployment.durationMs,
      leadTimeMs: deployment.leadTimeMs, // Commit to deploy time
      triggeredBy: deployment.triggeredBy,
      commitHash: deployment.commitHash,
    });
  }

  /**
   * Calculate deployment frequency (deploys per day)
   */
  getDeploymentFrequency(days = 30) {
    const cutoff = new Date(Date.now() - days * 24 * 60 * 60 * 1000);
    const recentDeploys = this.deployments.filter(
      (d) => new Date(d.timestamp) >= cutoff && d.environment === 'production'
    );

    return {
      totalDeployments: recentDeploys.length,
      deploymentsPerDay: (recentDeploys.length / days).toFixed(2),
      successRate: (
        (recentDeploys.filter((d) => d.status === 'success').length / recentDeploys.length) *
        100
      ).toFixed(1),
    };
  }

  /**
   * Calculate lead time for changes
   */
  getLeadTimeStats() {
    const productionDeploys = this.deployments.filter(
      (d) => d.environment === 'production' && d.leadTimeMs
    );

    if (productionDeploys.length === 0) return null;

    const leadTimes = productionDeploys.map((d) => d.leadTimeMs).sort((a, b) => a - b);

    return {
      medianMs: leadTimes[Math.floor(leadTimes.length / 2)],
      p90Ms: leadTimes[Math.floor(leadTimes.length * 0.9)],
      avgMs: Math.round(leadTimes.reduce((a, b) => a + b, 0) / leadTimes.length),
      medianHours: (leadTimes[Math.floor(leadTimes.length / 2)] / 3600000).toFixed(1),
    };
  }

  /**
   * Calculate Mean Time to Recovery (MTTR)
   */
  getMTTR() {
    const failures = this.deployments
      .filter((d) => d.status === 'failure' || d.status === 'rollback')
      .sort((a, b) => new Date(a.timestamp) - new Date(b.timestamp));

    if (failures.length === 0) return { mttrMinutes: 0, failureCount: 0 };

    const recoveryTimes = [];

    for (const failure of failures) {
      const failureTime = new Date(failure.timestamp).getTime();
      // Find next successful deploy
      const recovery = this.deployments.find(
        (d) =>
          d.environment === failure.environment &&
          d.status === 'success' &&
          new Date(d.timestamp).getTime() > failureTime
      );

      if (recovery) {
        recoveryTimes.push(new Date(recovery.timestamp).getTime() - failureTime);
      }
    }

    if (recoveryTimes.length === 0) return { mttrMinutes: 'N/A', failureCount: failures.length };

    const avgRecoveryMs = recoveryTimes.reduce((a, b) => a + b, 0) / recoveryTimes.length;

    return {
      mttrMinutes: (avgRecoveryMs / 60000).toFixed(1),
      failureCount: failures.length,
      recoveryCount: recoveryTimes.length,
    };
  }

  /**
   * Calculate Change Failure Rate
   */
  getChangeFailureRate(days = 30) {
    const cutoff = new Date(Date.now() - days * 24 * 60 * 60 * 1000);
    const recent = this.deployments.filter(
      (d) => new Date(d.timestamp) >= cutoff && d.environment === 'production'
    );

    if (recent.length === 0) return { rate: 0, total: 0 };

    const failures = recent.filter((d) => d.status === 'failure' || d.status === 'rollback');

    return {
      rate: ((failures.length / recent.length) * 100).toFixed(1),
      total: recent.length,
      failures: failures.length,
    };
  }

  /**
   * Get deployment duration trends
   */
  getDurationTrends(days = 30) {
    const cutoff = new Date(Date.now() - days * 24 * 60 * 60 * 1000);
    const recent = this.deployments.filter(
      (d) => new Date(d.timestamp) >= cutoff && d.status === 'success'
    );

    if (recent.length === 0) return null;

    const durations = recent.map((d) => d.durationMs).sort((a, b) => a - b);

    return {
      avgMinutes: (durations.reduce((a, b) => a + b, 0) / durations.length / 60000).toFixed(1),
      medianMinutes: (durations[Math.floor(durations.length / 2)] / 60000).toFixed(1),
      p95Minutes: (durations[Math.floor(durations.length * 0.95)] / 60000).toFixed(1),
      minMinutes: (durations[0] / 60000).toFixed(1),
      maxMinutes: (durations[durations.length - 1] / 60000).toFixed(1),
    };
  }

  /**
   * Generate full metrics dashboard data
   */
  getDashboard() {
    return {
      deploymentFrequency: this.getDeploymentFrequency(),
      leadTime: this.getLeadTimeStats(),
      mttr: this.getMTTR(),
      changeFailureRate: this.getChangeFailureRate(),
      durationTrends: this.getDurationTrends(),
    };
  }
}

module.exports = { DeploymentMetrics };
```

---

## DORA Metrics Implementation

```javascript
// metrics/dora-metrics.js - DORA metrics framework implementation
class DORAMetrics {
  constructor(deploymentStore, incidentStore) {
    this.deployments = deploymentStore;
    this.incidents = incidentStore;
  }

  /**
   * Calculate all four DORA metrics
   */
  async calculateAll(periodDays = 90) {
    return {
      deploymentFrequency: await this.deploymentFrequency(periodDays),
      leadTimeForChanges: await this.leadTimeForChanges(periodDays),
      changeFailureRate: await this.changeFailureRate(periodDays),
      timeToRestore: await this.timeToRestore(periodDays),
      performanceTier: null, // Calculated below
    };
  }

  /**
   * Deployment Frequency: How often code is deployed to production
   */
  async deploymentFrequency(days) {
    const deployments = await this.deployments.getProductionDeploys(days);
    const perDay = deployments.length / days;

    let tier;
    if (perDay >= 1) tier = 'Elite';       // On-demand / multiple per day
    else if (perDay >= 1 / 7) tier = 'High'; // Weekly
    else if (perDay >= 1 / 30) tier = 'Medium'; // Monthly
    else tier = 'Low';                        // Less than monthly

    return {
      metric: 'Deployment Frequency',
      value: perDay.toFixed(3),
      unit: 'deploys/day',
      displayValue: `${deployments.length} deployments in ${days} days`,
      tier,
      benchmark: {
        elite: 'On-demand (multiple per day)',
        high: 'Weekly',
        medium: 'Monthly',
        low: 'Less than monthly',
      },
    };
  }

  /**
   * Lead Time for Changes: Time from commit to production
   */
  async leadTimeForChanges(days) {
    const deployments = await this.deployments.getProductionDeploys(days);
    const leadTimes = deployments
      .filter((d) => d.leadTimeMs)
      .map((d) => d.leadTimeMs)
      .sort((a, b) => a - b);

    if (leadTimes.length === 0) return { metric: 'Lead Time for Changes', value: 'N/A' };

    const medianMs = leadTimes[Math.floor(leadTimes.length / 2)];
    const medianHours = medianMs / 3600000;

    let tier;
    if (medianHours < 1) tier = 'Elite';        // Less than 1 hour
    else if (medianHours < 24) tier = 'High';   // Less than 1 day
    else if (medianHours < 168) tier = 'Medium'; // Less than 1 week
    else tier = 'Low';                            // More than 1 week

    return {
      metric: 'Lead Time for Changes',
      value: medianMs,
      displayValue: medianHours < 1
        ? `${(medianMs / 60000).toFixed(0)} minutes`
        : `${medianHours.toFixed(1)} hours`,
      tier,
      benchmark: {
        elite: '< 1 hour',
        high: '< 1 day',
        medium: '< 1 week',
        low: '> 1 week',
      },
    };
  }

  /**
   * Change Failure Rate: Percentage of deployments causing failures
   */
  async changeFailureRate(days) {
    const deployments = await this.deployments.getProductionDeploys(days);
    const failures = deployments.filter(
      (d) => d.status === 'failure' || d.status === 'rollback'
    );

    const rate = deployments.length > 0
      ? (failures.length / deployments.length) * 100
      : 0;

    let tier;
    if (rate <= 5) tier = 'Elite';       // 0-5%
    else if (rate <= 10) tier = 'High';  // 6-10%
    else if (rate <= 15) tier = 'Medium'; // 11-15%
    else tier = 'Low';                     // > 15%

    return {
      metric: 'Change Failure Rate',
      value: rate.toFixed(1),
      unit: '%',
      displayValue: `${failures.length} failures / ${deployments.length} deployments (${rate.toFixed(1)}%)`,
      tier,
      benchmark: {
        elite: '0-5%',
        high: '6-10%',
        medium: '11-15%',
        low: '> 15%',
      },
    };
  }

  /**
   * Time to Restore Service: How long it takes to restore after failure
   */
  async timeToRestore(days) {
    const incidents = await this.incidents.getResolvedIncidents(days);

    if (incidents.length === 0) {
      return { metric: 'Time to Restore Service', value: 'N/A', tier: 'Elite' };
    }

    const resolutionTimes = incidents
      .map((i) => i.resolvedAt - i.createdAt)
      .sort((a, b) => a - b);

    const medianMs = resolutionTimes[Math.floor(resolutionTimes.length / 2)];
    const medianHours = medianMs / 3600000;

    let tier;
    if (medianHours < 1) tier = 'Elite';       // Less than 1 hour
    else if (medianHours < 24) tier = 'High';  // Less than 1 day
    else if (medianHours < 168) tier = 'Medium'; // Less than 1 week
    else tier = 'Low';                            // More than 1 week

    return {
      metric: 'Time to Restore Service',
      value: medianMs,
      displayValue: medianHours < 1
        ? `${(medianMs / 60000).toFixed(0)} minutes`
        : `${medianHours.toFixed(1)} hours`,
      tier,
      benchmark: {
        elite: '< 1 hour',
        high: '< 1 day',
        medium: '< 1 week',
        low: '> 1 week',
      },
    };
  }

  /**
   * Determine overall DORA performance tier
   */
  calculatePerformanceTier(metrics) {
    const tiers = [
      metrics.deploymentFrequency.tier,
      metrics.leadTimeForChanges.tier,
      metrics.changeFailureRate.tier,
      metrics.timeToRestore.tier,
    ];

    const tierScore = { Elite: 4, High: 3, Medium: 2, Low: 1 };
    const avgScore = tiers.reduce((sum, t) => sum + tierScore[t], 0) / tiers.length;

    if (avgScore >= 3.5) return 'Elite';
    if (avgScore >= 2.5) return 'High';
    if (avgScore >= 1.5) return 'Medium';
    return 'Low';
  }
}

module.exports = { DORAMetrics };
```

---

## Deployment Maturity Model

```markdown
# Deployment Maturity Model

## Level 1: Initial (Ad Hoc)
- Manual deployments via SSH
- No standardized process
- Deployments cause downtime
- Rollback is manual file restoration
- No deployment metrics tracked

**Key Indicators:**
- Deploy frequency: Monthly or less
- Lead time: Days to weeks
- Change failure rate: > 25%
- MTTR: Hours to days

## Level 2: Repeatable
- Basic deployment scripts exist
- Some documentation of procedures
- Manual testing before deployment
- Basic health checks implemented
- Environment parity improving

**Key Indicators:**
- Deploy frequency: Weekly
- Lead time: 1-3 days
- Change failure rate: 15-25%
- MTTR: 1-4 hours

**Actions to Advance:**
1. Implement CI/CD pipeline
2. Add automated tests to deployment flow
3. Create runbooks for common issues
4. Implement basic monitoring

## Level 3: Defined
- CI/CD pipeline for all environments
- Automated testing in pipeline
- Standard deployment process documented
- Blue-green or rolling deployments
- Centralized logging

**Key Indicators:**
- Deploy frequency: Multiple times per week
- Lead time: Hours to 1 day
- Change failure rate: 10-15%
- MTTR: 30 min - 1 hour

**Actions to Advance:**
1. Implement canary deployments
2. Add automated rollback
3. Implement feature flags
4. Add deployment metrics dashboard

## Level 4: Measured
- Comprehensive deployment metrics (DORA)
- Automated canary analysis
- Self-healing infrastructure
- Chaos engineering practices
- Cost optimization automated

**Key Indicators:**
- Deploy frequency: Daily
- Lead time: < 4 hours
- Change failure rate: 5-10%
- MTTR: < 30 minutes

**Actions to Advance:**
1. Implement progressive delivery
2. Add AI-powered anomaly detection
3. Automate capacity planning
4. Implement deployment risk scoring

## Level 5: Optimized
- Continuous deployment on every commit
- Fully automated progressive delivery
- Predictive scaling and capacity planning
- Zero-downtime deployments guaranteed
- Cost optimization is continuous

**Key Indicators:**
- Deploy frequency: Multiple per day
- Lead time: < 1 hour
- Change failure rate: < 5%
- MTTR: < 15 minutes

**Characteristics:**
- Deployments are boring and routine
- Failures are caught before production
- Recovery is automatic
- Engineering focus on feature delivery
```

---

## Technology Radar for Deployment Tools

```markdown
# Deployment Technology Radar

## Adopt (Proven, Recommended)
| Tool | Category | Use Case |
|---|---|---|
| **Docker** | Containerization | Application packaging and isolation |
| **Kubernetes** | Orchestration | Container orchestration at scale |
| **GitHub Actions** | CI/CD | Pipeline automation |
| **Terraform** | IaC | Infrastructure provisioning |
| **Prometheus + Grafana** | Monitoring | Metrics collection and visualization |

## Trial (Worth Pursuing)
| Tool | Category | Use Case |
|---|---|---|
| **ArgoCD** | GitOps | Kubernetes deployment via Git |
| **Pulumi** | IaC | IaC with real programming languages |
| **Crossplane** | IaC | Kubernetes-native infrastructure |
| **Backstage** | Developer Portal | Service catalog and templates |
| **Keptn** | CD | Cloud-native delivery orchestration |

## Assess (Worth Exploring)
| Tool | Category | Use Case |
|---|---|---|
| **Dagger** | CI/CD | Portable pipeline engine |
| **Acorn** | Packaging | Simpler container packaging |
| **Timoni** | Package Manager | Kubernetes package management |
| **CUE** | Configuration | Type-safe configuration language |

## Hold (Proceed with Caution)
| Tool | Category | Reason |
|---|---|---|
| **Jenkins** | CI/CD | Maintenance overhead, newer alternatives available |
| **Ansible for Deploy** | Configuration | Better suited for provisioning than deployments |
| **Custom Scripts** | Automation | Fragile, hard to maintain at scale |
```

---

## Chapter Summary

### Key Takeaways

This chapter covered the complete lifecycle of deploying Node.js applications to production:

1. **Pre-Deployment Preparation**: Comprehensive checklists ensure nothing is missed before going live.

2. **Automation & Scripting**: Idempotent scripts with proper error handling make deployments reliable and repeatable.

3. **Rollback Strategies**: Automated rollback with health verification provides safety nets for failed deployments.

4. **Database Migrations**: The expand-contract pattern enables zero-downtime schema changes.

5. **Documentation & Runbooks**: Standardized documentation ensures knowledge transfer and consistent operations.

6. **Notification & Approval**: Integrating with communication tools keeps stakeholders informed and provides governance.

7. **Troubleshooting**: Decision trees and diagnostic scripts reduce mean time to resolution.

8. **Debugging**: Node.js inspector, heap snapshots, and CPU profiling enable deep production debugging.

9. **Capacity Planning**: Load testing and forecasting models ensure infrastructure scales with demand.

10. **Cost Optimization**: FinOps practices and cloud-specific tools minimize infrastructure spend.

11. **Continuous Improvement**: DORA metrics and maturity models provide frameworks for measuring and improving deployment practices.

### Recommended Next Steps

| Current State | Recommended Action |
|---|---|
| No CI/CD pipeline | Start with GitHub Actions for basic automation |
| Manual deployments | Create shell scripts for repeatable deployments |
| No rollback plan | Implement automated rollback with health checks |
| No deployment metrics | Implement DORA metrics tracking |
| High change failure rate | Add canary deployments and automated testing |
| High infrastructure cost | Implement right-sizing analysis and reserved capacity |

---

## Cross-References

### Related Sections in This Chapter

| Section | Topic |
|---|---|
| [01 - Deployment Checklist](./01-deployment-checklist.md) | Pre-deployment verification |
| [02 - Automation & Troubleshooting](./02-automation-troubleshooting.md) | Deployment scripting and debugging |

### Related Chapters

| Chapter | Topic | Relevance |
|---|---|---|
| [05 - Express Framework](../../05-express-framework/) | Web framework | Health check endpoints |
| [06 - Databases & Performance](../../06-databases-performance/) | Database patterns | Migration strategies |
| [09 - Testing](../../09-testing/) | Testing | Pre-deployment testing |
| [19 - Security & Rate Limiting](../../19-security-rate-limiting/) | Security | Deployment security practices |
| [26 - CI/CD GitHub Actions](../../26-cicd-github-actions/) | CI/CD | Pipeline integration |
| [33 - Observability](../../33-observability-monitoring/) | Monitoring | Production monitoring |
| [38 - Cloud Native Deployment](../../38-cloud-native-deployment/) | Cloud deployment | Advanced cloud patterns |

### External Resources

- [DORA State of DevOps Reports](https://dora.dev/)
- [FinOps Foundation](https://www.finops.org/)
- [AWS Well-Architected Framework](https://aws.amazon.com/architecture/well-architected/)
- [Google SRE Book](https://sre.google/sre-book/table-of-contents/)
- [Martin Fowler - Deployment Patterns](https://martinfowler.com/tags/deployment%20pipeline.html)

---

*Last updated: 2026-04-01*
