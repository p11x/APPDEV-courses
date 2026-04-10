# Trace Sampling Strategies

## Overview

Trace sampling strategies determine which traces are recorded and stored. In high-throughput systems, recording every trace would overwhelm storage and processing. Effective sampling ensures representative traces while controlling costs.

Sampling decisions must balance observability needs with resource constraints. Important traces (errors, slow requests) should always be captured, while normal traces can be sampled to reduce volume. The sampling strategy should consider both backend load and debugging needs.

Several sampling approaches address different scenarios. The best approach depends on traffic patterns, error rates, and analysis requirements.

## Sampling Types

Multiple sampling strategies address different needs.

**Head Sampling**: Decides whether to sample at the start of a trace. Simple but may drop traces that become interesting later.

**Tail Sampling**: Captures complete traces for specific conditions after the trace completes. Ensures errors and slow requests are always captured.

**Probabilistic Sampling**: Samples a fixed percentage of traces. Simple to implement and predictable.

**Deterministic Sampling**: Uses trace ID hash to decide sampling. Ensures consistent sampling for the same trace.

**Adaptive Sampling**: Adjusts sampling rate based on system load or error rates.

## Sampling Architecture

```mermaid
flowchart TB
    subgraph Application["Application"]
        HS[Head Sampler]
    end
    
    subgraph Collector["Collector"]
        TS[Tail Sampler]
        Filter[Filter]
        Buffer[Buffer]
    end
    
    subgraph Decision["Decision Logic"]
        Check[Check Conditions]
        Rules[Rules Engine]
    end
    
    subgraph Storage["Storage"]
        Full[(Full Storage)]
        Sample[(Sampled Storage)]
    end
    
    Request -> HS
    HS --> Collector
    TS --> Check
    Check --> Rules
    Rules --> Buffer
    Buffer --> Full
    Buffer --> Sample
```

Sampling flows from application through collectors with tail sampling for important traces.

## Java Implementation

```java
import io.opentelemetry.api.trace.Sampler;
import io.opentelemetry.api.trace.Span;
import io.opentelemetry.api.trace.SpanContext;
import io.opentelemetry.sdk.trace.SamplerProbability;
import io.opentelemetry.sdk.trace.sampling.*;
import io.opentelemetry.sdk.trace.data.SpanData;
import io.opentelemetry.sdk.trace.export SpanExporter;
import io.opentelemetry.sdk.trace.export.BatchSpanProcessor;
import io.opentelemetry.sdk.common.AttributeValue;
import java.util.List;
import java.util.Map;
import java.util.Random;

public class TraceSamplingExample {
    
    public static class CustomSampler implements Sampler {
        private final Sampler baseSampler;
        private final double errorRate;
        private final long slowThresholdMs;
        private final Random random;
        
        public CustomSampler(double probability, double errorRate, 
                            long slowThresholdMs) {
            this.baseSampler = Sampler.probability(probability);
            this.errorRate = errorRate;
            this.slowThresholdMs = slowThresholdMs;
            this.random = new Random();
        }
        
        @Override
        public boolean shouldSample(io.opentelemetry.sdk.trace.ReadWriteSpan span) {
            SpanContext context = span.getSpanContext();
            
            if (isError(span) || isSlow(span)) {
                return true;
            }
            
            String traceId = context.getTraceId();
            int hash = Math.abs(traceId.hashCode());
            return (hash % 100) < (baseSampler.getProbability() * 100);
        }
        
        private boolean isError(io.opentelemetry.sdk.trace.ReadWriteSpan span) {
            return span.getStatus().getStatusCode() == 
                   io.opentelemetry.api.trace.StatusCode.ERROR;
        }
        
        private boolean isSlow(io.opentelemetry.sdk.trace.ReadWriteSpan span) {
            return span.getEndTimeUnixNano() - span.getStartTimeUnixNano() >
                   slowThresholdMs * 1_000_000;
        }
        
        @Override
        public String getDescription() {
            return "CustomSampler";
        }
    }
    
    public static class AlwaysSampleForErrors implements Sampler {
        private final Sampler baseSampler;
        
        public AlwaysSampleForErrors(Sampler baseSampler) {
            this.baseSampler = baseSampler;
        }
        
        @Override
        public boolean shouldSample(io.opentelemetry.sdk.trace.ReadWriteSpan rootSpan) {
            if (rootSpan.getStatus().getStatusCode() == 
                io.opentelemetry.api.trace.StatusCode.ERROR) {
                return true;
            }
            
            return baseSampler.shouldSample(rootSpan);
        }
        
        @Override
        public String getDescription() {
            return "AlwaysSampleForErrors";
        }
    }
    
    public static class TailBasedSampler implements Sampler {
        private final Sampler baseSampler;
        private final List<String> errorConditions;
        private final List<String> slowConditions;
        
        public TailBasedSampler(Sampler baseSampler) {
            this.baseSampler = baseSampler;
            this.errorConditions = List.of("error", "exception", "failed");
            this.slowConditions = List.of("timeout", "slow");
        }
        
        @Override
        public boolean shouldSample(io.opentelemetry.sdk.trace.ReadWriteSpan span) {
            if (isImportant(span)) {
                return true;
            }
            
            return baseSampler.shouldSample(span);
        }
        
        private boolean isImportant(io.opentelemetry.sdk.trace.ReadWriteSpan span) {
            Map<String, AttributeValue> attributes = span.getAttributes();
            
            for (Map.Entry<String, AttributeValue> entry : 
                 attributes.entrySet()) {
                String key = entry.getKey();
                Object value = entry.getValue();
                
                for (String condition : errorConditions) {
                    if (key.contains(condition) || 
                        value.toString().contains(condition)) {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        @Override
        public String getDescription() {
            return "TailBasedSampler";
        }
    }
    
    public static class AdaptiveSampler implements Sampler {
        private volatile double currentProbability = 1.0;
        private volatile double targetProbability = 0.1;
        private final double minProbability = 0.01;
        private final double maxProbability = 1.0;
        
        private volatile long errorCount = 0;
        private volatile long totalCount = 0;
        
        public void recordOutcome(boolean isError) {
            totalCount++;
            if (isError) errorCount++;
            
            double errorRate = (double) errorCount / (double) totalCount;
            
            if (errorRate > 0.05) {
                currentProbability = maxProbability;
            } else if (errorRate > 0.01) {
                currentProbability = 0.5;
            } else {
                currentProbability = targetProbability;
            }
            
            if (totalCount > 10000) {
                errorCount = 0;
                totalCount = 0;
            }
        }
        
        @Override
        public boolean shouldSample(io.opentelemetry.sdk.trace.ReadWriteSpan span) {
            double probability = currentProbability;
            return Math.random() < probability;
        }
        
        @Override
        public String getDescription() {
            return "AdaptiveSampler-" + currentProbability;
        }
    }
    
    public static class DeterministicSampler implements Sampler {
        private final double probability;
        
        public DeterministicSampler(double probability) {
            this.probability = probability;
        }
        
        @Override
        public boolean shouldSample(io.opentelemetry.sdk.trace.ReadWriteSpan span) {
            String traceId = span.getSpanContext().getTraceId();
            
            int hash = 0;
            for (int i = 0; i < traceId.length(); i++) {
                hash = 31 * hash + traceId.charAt(i);
            }
            
            return (Math.abs(hash) % 100) < (probability * 100);
        }
        
        @Override
        public String getDescription() {
            return "DeterministicSampler-" + probability;
        }
    }
    
    public static class CompositeSampler implements Sampler {
        private final List<Sampler> samplers;
        
        public CompositeSampler(List<Sampler> samplers) {
            this.samplers = samplers;
        }
        
        @Override
        public boolean shouldSample(io.opentelemetry.sdk.trace.ReadWriteSpan span) {
            for (Sampler sampler : samplers) {
                if (sampler.shouldSample(span)) {
                    return true;
                }
            }
            return false;
        }
        
        @Override
        public String getDescription() {
            return "CompositeSampler";
        }
    }
    
    public static Sampler createSampler(String type, double probability) {
        switch (type) {
            case "probabilistic":
                return new DeterministicSampler(probability);
            case "always":
                return Sampler.alwaysOn();
            case "head":
                return new AlwaysSampleForErrors(Sampler.probability(probability));
            default:
                return Sampler.probability(probability);
        }
    }
    
    public static void main(String[] args) {
        Sampler headSampler = new DeterministicSampler(0.1);
        
        CustomSampler customSampler = new CustomSampler(0.1, 0.5, 1000);
        
        System.out.println("Sampling configured: " + 
                         customSampler.getDescription());
    }
}


class SamplingConfig {
    
    public static class Builder {
        private String type = "deterministic";
        private double probability = 0.1;
        private double errorRate = 0.5;
        private long slowThresholdMs = 1000;
        
        public Builder setType(String type) {
            this.type = type;
            return this;
        }
        
        public Builder setProbability(double probability) {
            this.probability = probability;
            return this;
        }
        
        public Builder setErrorRate(double errorRate) {
            this.errorRate = errorRate;
            return this;
        }
        
        public Builder setSlowThresholdMs(long slowThresholdMs) {
            this.slowThresholdMs = slowThresholdMs;
            return this;
        }
        
        public Sampler build() {
            return new TraceSamplingExample.CustomSampler(
                probability, errorRate, slowThresholdMs
            );
        }
    }
}
```

## Python Implementation

```python
import random
import time
import threading
from typing import Dict, List, Optional, Callable
from dataclasses import dataclass
from enum import Enum
from opentracing import Tracer
from opentracing.samplers import Sampler


class SamplingStrategy(Enum):
    PROBABILISTIC = "probabilistic"
    TAIL_BASED = "tail_based"
    ADAPTIVE = "adaptive"
    DETERMINISTIC = "deterministic"


class BaseSampler(Sampler):
    """Base sampler interface."""
    
    def should_sample(self, trace_id: str, span_data: Dict) -> bool:
        raise NotImplementedError


class ProbabilisticSampler(BaseSampler):
    """Probabilistic sampling."""
    
    def __init__(self, probability: float):
        self.probability = probability
    
    def should_sample(self, trace_id: str, span_data: Dict) -> bool:
        return random.random() < self.probability


class DeterministicSampler(BaseSampler):
    """Deterministic sampling based on trace ID hash."""
    
    def __init__(self, probability: float):
        self.probability = probability
    
    def should_sample(self, trace_id: str, span_data: Dict) -> bool:
        hash_value = hash(trace_id)
        return (abs(hash_value) % 100) < (self.probability * 100)


class TailBasedSampler(BaseSampler):
    """Tail-based sampling for errors and slow requests."""
    
    def __init__(self, base_sampler: BaseSampler, 
                 error_conditions: List[str] = None,
                 slow_threshold_ms: int = 1000):
        self.base_sampler = base_sampler
        self.error_conditions = error_conditions or [
            'error', 'exception', 'failed'
        ]
        self.slow_threshold_ms = slow_threshold_ms
    
    def should_sample(self, trace_id: str, span_data: Dict) -> bool:
        if self._is_error(span_data) or self._is_slow(span_data):
            return True
        return self.base_sampler.should_sample(trace_id, span_data)
    
    def _is_error(self, span_data: Dict) -> bool:
        tags = span_data.get('tags', {})
        for key, value in tags.items():
            if any(cond in str(key).lower() or 
                 cond in str(value).lower() 
                 for cond in self.error_conditions):
                return True
        return False
    
    def _is_slow(self, span_data: Dict) -> bool:
        duration = span_data.get('duration_ms', 0)
        return duration >= self.slow_threshold_ms


class AdaptiveSampler(BaseSampler):
    """Adaptive sampling based on error rate."""
    
    def __init__(self, target_probability: float = 0.1,
                 min_probability: float = 0.01,
                 max_probability: float = 1.0):
        self.target_probability = target_probability
        self.min_probability = min_probability
        self.max_probability = max_probability
        
        self._current_probability = 1.0
        self._error_count = 0
        self._total_count = 0
        self._lock = threading.Lock()
    
    def should_sample(self, trace_id: str, span_data: Dict) -> bool:
        with self._lock:
            self._total_count += 1
            
            if span_data.get('is_error'):
                self._error_count += 1
            
            self._adjust_probability()
            
            return random.random() < self._current_probability
    
    def _adjust_probability(self):
        if self._total_count < 100:
            return
        
        error_rate = self._error_count / self._total_count
        
        if error_rate > 0.05:
            self._current_probability = self.max_probability
        elif error_rate > 0.01:
            self._current_probability = 0.5
        else:
            self._current_probability = self.target_probability
        
        if self._total_count > 10000:
            self._error_count = 0
            self._total_count = 0


class CompositeSampler(BaseSampler):
    """Composite sampling with multiple strategies."""
    
    def __init__(self, samplers: List[BaseSampler]):
        self.samplers = samplers
    
    def should_sample(self, trace_id: str, span_data: Dict) -> bool:
        for sampler in self.samplers:
            if sampler.should_sample(trace_id, span_data):
                return True
        return False


class ConfigurableSampler(BaseSampler):
    """Configurable sampler from configuration."""
    
    def __init__(self, config: Dict):
        self._config = config
        self._build_sampler()
    
    def _build_sampler(self):
        strategy = self._config.get('strategy', 'deterministic')
        
        if strategy == SamplingStrategy.PROBABILISTIC.value:
            probability = self._config.get('probability', 0.1)
            self._sampler = ProbabilisticSampler(probability)
        
        elif strategy == SamplingStrategy.DETERMINISTIC.value:
            probability = self._config.get('probability', 0.1)
            self._sampler = DeterministicSampler(probability)
        
        elif strategy == SamplingStrategy.TAIL_BASED.value:
            base = DeterministicSampler(
                self._config.get('probability', 0.1)
            )
            self._sampler = TailBasedSampler(
                base,
                error_conditions=self._config.get('error_conditions'),
                slow_threshold_ms=self._config.get('slow_threshold_ms', 1000)
            )
        
        elif strategy == SamplingStrategy.ADAPTIVE.value:
            self._sampler = AdaptiveSampler(
                target_probability=self._config.get('probability', 0.1)
            )
        
        else:
            self._sampler = DeterministicSampler(0.1)
    
    def should_sample(self, trace_id: str, span_data: Dict) -> bool:
        return self._sampler.should_sample(trace_id, span_data)


def create_sampler(config: Dict) -> BaseSampler:
    """Create sampler from configuration."""
    return ConfigurableSampler(config)


if __name__ == "__main__":
    config = {
        'strategy': 'tail_based',
        'probability': 0.1,
        'error_conditions': ['error', 'exception'],
        'slow_threshold_ms': 500
    }
    
    sampler = create_sampler(config)
    
    test_data = [
        {'trace_id': 'abc123', 'tags': {'status': 'ok'}, 
         'duration_ms': 100, 'is_error': False},
        {'trace_id': 'def456', 'tags': {'error': 'true'}, 
         'duration_ms': 100, 'is_error': True},
    ]
    
    for data in test_data:
        result = sampler.should_sample(data['trace_id'], data)
        print(f"Sample {data['trace_id']}: {result}")
```

## Real-World Examples

**Google Cloud** uses tail-based sampling to capture all error traces while sampling 0.1% of normal traffic.

**AWS X-Ray** uses adaptive sampling that increases capture during elevated error rates.

**DataDog** implements intelligent sampling that prioritizes error and slow traces.

## Output Statement

Organizations implementing trace sampling can expect: significant storage savings (often 90%+ reduction); maintained error visibility through tail-based sampling; predictable costs regardless of traffic; and complete traces for debugging through sampling strategies.

Trace sampling enables production-scale distributed tracing with sustainable resource costs.

## Best Practices

1. **Always Sample Errors**: Use tail-based sampling to capture all error traces.

2. **Capture Slow Requests**: Always capture requests exceeding latency thresholds.

3. **Use Deterministic Sampling**: Use trace ID hash for consistent sampling decisions.

4. **Implement Adaptive Sampling**: Increase sampling rate during elevated error rates.

5. **Configure Per-Service**: Adjust sampling based on service traffic and importance.

6. **Monitor Sampling Impact**: Track sampling effectiveness regularly.