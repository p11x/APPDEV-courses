# Performance Regression Detection

## What You'll Learn

- Baseline establishment and management
- Statistical regression detection
- Automated regression testing
- Performance trend analysis
- Alert thresholds and notifications

## Baseline Manager

```javascript
class BaselineManager {
    constructor(options = {}) {
        this.baselines = new Map();
        this.storagePath = options.storagePath || './performance-baselines.json';
        this.deviationThreshold = options.deviationThreshold || 20; // percent
    }

    recordBaseline(name, metrics) {
        const baseline = {
            name,
            metrics: {
                meanLatency: metrics.meanLatency,
                p95Latency: metrics.p95Latency,
                p99Latency: metrics.p99Latency,
                throughput: metrics.throughput,
                errorRate: metrics.errorRate,
                memoryUsageMB: metrics.memoryUsageMB,
            },
            recordedAt: new Date().toISOString(),
            samples: metrics.sampleCount || 1,
        };

        this.baselines.set(name, baseline);
        return baseline;
    }

    async saveBaselines() {
        const data = Object.fromEntries(this.baselines);
        const fs = await import('node:fs/promises');
        await fs.writeFile(this.storagePath, JSON.stringify(data, null, 2));
    }

    async loadBaselines() {
        try {
            const fs = await import('node:fs/promises');
            const data = JSON.parse(await fs.readFile(this.storagePath, 'utf-8'));
            for (const [name, baseline] of Object.entries(data)) {
                this.baselines.set(name, baseline);
            }
        } catch {
            console.log('No existing baselines found');
        }
    }

    compare(name, currentMetrics) {
        const baseline = this.baselines.get(name);
        if (!baseline) return { status: 'no_baseline' };

        const comparisons = {};
        const regressions = [];

        for (const [key, baseValue] of Object.entries(baseline.metrics)) {
            const currentValue = currentMetrics[key];
            if (currentValue == null) continue;

            let changePercent;
            if (key === 'throughput') {
                // Lower throughput is worse
                changePercent = ((baseValue - currentValue) / baseValue) * 100;
            } else if (key === 'errorRate') {
                // Higher error rate is worse
                changePercent = ((currentValue - baseValue) / (baseValue || 0.01)) * 100;
            } else {
                // Higher latency/memory is worse
                changePercent = ((currentValue - baseValue) / baseValue) * 100;
            }

            comparisons[key] = {
                baseline: baseValue,
                current: currentValue,
                change: +changePercent.toFixed(1),
                regressed: changePercent > this.deviationThreshold,
            };

            if (changePercent > this.deviationThreshold) {
                regressions.push(key);
            }
        }

        return {
            status: regressions.length > 0 ? 'regressed' : 'ok',
            regressions,
            comparisons,
        };
    }
}
```

## Statistical Regression Detector

```javascript
class RegressionDetector {
    constructor(options = {}) {
        this.windowSize = options.windowSize || 50;
        this.significanceLevel = options.significanceLevel || 0.05;
        this.minSamples = options.minSamples || 20;
        this.history = new Map();
    }

    record(metricName, value) {
        if (!this.history.has(metricName)) {
            this.history.set(metricName, []);
        }

        const samples = this.history.get(metricName);
        samples.push({ value, timestamp: Date.now() });

        if (samples.length > this.windowSize * 3) {
            this.history.set(metricName, samples.slice(-this.windowSize * 2));
        }
    }

    detect(metricName) {
        const samples = this.history.get(metricName);
        if (!samples || samples.length < this.minSamples * 2) {
            return { status: 'insufficient_data' };
        }

        const midpoint = Math.floor(samples.length / 2);
        const baseline = samples.slice(0, midpoint).map(s => s.value);
        const current = samples.slice(midpoint).map(s => s.value);

        const baselineMean = baseline.reduce((a, b) => a + b) / baseline.length;
        const currentMean = current.reduce((a, b) => a + b) / current.length;

        const baselineStd = this.stdDev(baseline);
        const currentStd = this.stdDev(current);

        // Cohen's d effect size
        const pooledStd = Math.sqrt(
            ((baseline.length - 1) * baselineStd ** 2 + (current.length - 1) * currentStd ** 2) /
            (baseline.length + current.length - 2)
        );

        const effectSize = (currentMean - baselineMean) / pooledStd;
        const percentChange = ((currentMean - baselineMean) / baselineMean) * 100;

        // Detect trend
        const trend = this.detectTrend(current);

        return {
            metricName,
            baseline: {
                mean: +baselineMean.toFixed(3),
                std: +baselineStd.toFixed(3),
                samples: baseline.length,
            },
            current: {
                mean: +currentMean.toFixed(3),
                std: +currentStd.toFixed(3),
                samples: current.length,
            },
            percentChange: +percentChange.toFixed(1),
            effectSize: +effectSize.toFixed(3),
            trend,
            status: Math.abs(effectSize) > 0.5 && percentChange > 20
                ? 'regressed'
                : Math.abs(effectSize) > 0.2 && percentChange > 10
                    ? 'warning'
                    : 'ok',
        };
    }

    detectTrend(values) {
        if (values.length < 10) return 'insufficient_data';

        // Simple linear regression
        const n = values.length;
        const x = Array.from({ length: n }, (_, i) => i);
        const sumX = x.reduce((a, b) => a + b);
        const sumY = values.reduce((a, b) => a + b);
        const sumXY = x.reduce((acc, xi, i) => acc + xi * values[i], 0);
        const sumX2 = x.reduce((acc, xi) => acc + xi * xi, 0);

        const slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);

        if (slope > 0.1) return 'increasing';
        if (slope < -0.1) return 'decreasing';
        return 'stable';
    }

    stdDev(values) {
        const mean = values.reduce((a, b) => a + b) / values.length;
        const squaredDiffs = values.map(v => (v - mean) ** 2);
        return Math.sqrt(squaredDiffs.reduce((a, b) => a + b) / values.length);
    }

    getAllStatuses() {
        const statuses = {};
        for (const [name] of this.history) {
            statuses[name] = this.detect(name);
        }
        return statuses;
    }
}
```

## Automated Regression Report

```javascript
class RegressionReporter {
    constructor(detector, baselineManager) {
        this.detector = detector;
        this.baselineManager = baselineManager;
    }

    generateReport(currentMetrics) {
        const report = {
            timestamp: new Date().toISOString(),
            metrics: {},
            regressions: [],
            warnings: [],
            summary: { total: 0, passed: 0, regressed: 0, warning: 0 },
        };

        for (const [name, value] of Object.entries(currentMetrics)) {
            this.detector.record(name, value);
            const detection = this.detector.detect(name);
            report.metrics[name] = detection;
            report.summary.total++;

            if (detection.status === 'regressed') {
                report.regressions.push({ metric: name, ...detection });
                report.summary.regressed++;
            } else if (detection.status === 'warning') {
                report.warnings.push({ metric: name, ...detection });
                report.summary.warning++;
            } else {
                report.summary.passed++;
            }
        }

        return report;
    }

    formatReport(report) {
        let output = `\n=== Performance Regression Report ===\n`;
        output += `Timestamp: ${report.timestamp}\n\n`;
        output += `Summary: ${report.summary.passed} passed, ${report.summary.regressed} regressed, ${report.summary.warning} warnings\n\n`;

        if (report.regressions.length > 0) {
            output += `REGRESSIONS:\n`;
            for (const r of report.regressions) {
                output += `  - ${r.metric}: ${r.percentChange}% change (${r.trend})\n`;
            }
        }

        if (report.warnings.length > 0) {
            output += `WARNINGS:\n`;
            for (const w of report.warnings) {
                output += `  - ${w.metric}: ${w.percentChange}% change (${w.trend})\n`;
            }
        }

        return output;
    }
}
```

## Best Practices Checklist

- [ ] Establish baselines for all critical metrics
- [ ] Store baselines in version control
- [ ] Run regression detection in CI/CD pipeline
- [ ] Set appropriate deviation thresholds (15-25%)
- [ ] Monitor trends, not just point-in-time values
- [ ] Generate regression reports for each deployment
- [ ] Alert on statistical significance, not just averages
- [ ] Maintain historical performance data

## Cross-References

- See [Database Profiling](../02-database-performance-optimization/06-database-monitoring-profiling.md) for DB profiling
- See [Memory/CPU Monitoring](./02-memory-cpu-monitoring.md) for resource monitoring
- See [Performance Testing](../08-performance-testing-benchmarking/01-load-testing.md) for testing

## Next Steps

Continue to [Performance Testing and Debugging](./05-testing-debugging.md) for testing strategies.
