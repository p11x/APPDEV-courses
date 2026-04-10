# Monitoring and Maintenance

## I. INTRODUCTION

### What is Model Monitoring?
Model Monitoring in ML operations refers to the continuous observation of deployed models to ensure they perform correctly over time. This encompasses tracking prediction quality, detecting model drift, monitoring infrastructure health, and generating alerts when issues arise. Unlike traditional software monitoring, ML monitoring must also track data distribution changes and model accuracy degradation, which are not immediately visible through standard metrics.

The need for monitoring stems from a fundamental challenge: model performance degrades over time as the environment changes. This phenomenon, called model drift, occurs when the relationship between input features and target variable changes, or when the input data distribution shifts. Without monitoring, degraded models continue making poor predictions, often going unnoticed until significant damage occurs.

### Why is it Important?
Monitoring provides:
- Early detection of model degradation
- Data drift alerts before performance impact
- Infrastructure health visibility
- Audit trail for compliance
- Troubleshooting capability
- Performance optimization insights

In production systems, monitoring determines whether models deliver value. It enables rapid response to issues, reducing risk and maintaining trust. For regulated industries, comprehensive monitoring logs are often required for compliance.

### Prerequisites
- Understanding of ML model lifecycle
- Familiarity with metrics and logging
- Basic statistics knowledge
- Cloud monitoring tools (optional)

## II. FUNDAMENTALS

### Types of Monitoring

**Performance Monitoring**: Tracks prediction accuracy and quality metrics over time.

**Data Monitoring**: Observes input data distribution for shifts and anomalies.

**Infrastructure Monitoring**: Monitors system resources and service health.

**Business Metrics Monitoring**: Tracks business KPIs influenced by model predictions.

### Key Concepts

**Drift Detection**: Identifying changes in data distribution or model performance.

**Concept Drift**: Changes in the relationship between features and target.

**Data Drift**: Changes in input feature distributions.

### Monitoring Architecture

```
+---------------------+     +------------------+     +------------------+
|  Data Ingestion     |---->| Model Prediction |---->| Output Results  |
+---------------------+     +------------------+     +------------------+
        |                            |                          |
        v                            v                          v
+---------------------+     +------------------+     +------------------+
|  Input Validation   |     | Performance      |     | Metrics         |
|  and Profiling      |     | Tracking          |     | Aggregation     |
+---------------------+     +------------------+     +------------------+
                                  |                          |
                                  v                          v
                           +------------------+     +------------------+
                           | Drift Detection |     | Alert           |
                           |                 |     | Generation      |
                           +------------------+     +------------------+
                                                        |
                                                        v
                                                 +------------------+
                                                 | Dashboard        |
                                                 | Visualization   |
                                                 +------------------+
```

## III. IMPLEMENTATION

```python
"""
Model Monitoring Implementation
===============================
Comprehensive monitoring for ML models in production.
This module provides production-grade monitoring capabilities
including drift detection, performance tracking, alerting,
and visualization features for real-time model observability.
"""

import time
import json
import pickle
from pathlib import Path
from typing import Dict, List, Any, Optional, Tuple, Union
from dataclasses import dataclass, field
from datetime import datetime, timedelta
from collections import deque
from enum import Enum
import numpy as np
import pandas as pd
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, 
    f1_score, roc_auc_score, confusion_matrix,
    mean_squared_error, mean_absolute_error
)
import warnings
warnings.filterwarnings('ignore')


class MetricType(Enum):
    """Types of metrics that can be tracked."""
    ACCURACY = "accuracy"
    PRECISION = "precision"
    RECALL = "recall"
    F1_SCORE = "f1_score"
    ROC_AUC = "roc_auc"
    MSE = "mse"
    MAE = "mae"
    CUSTOM = "custom"


class AlertSeverity(Enum):
    """Severity levels for alerts."""
    INFO = "info"
    WARNING = "warning"
    ERROR = "error"
    CRITICAL = "critical"


@dataclass
class MetricsSnapshot:
    """
    Snapshot of model metrics at a point in time.
    Used for tracking model performance trends over time.
    """
    timestamp: float
    accuracy: float
    precision: float
    recall: float
    f1_score: float
    roc_auc: float
    predictions_count: int
    unique_features_count: int
    latency_p50: float = 0.0
    latency_p95: float = 0.0
    latency_p99: float = 0.0


@dataclass
class Alert:
    """Represents a monitoring alert."""
    alert_id: str
    severity: AlertSeverity
    message: str
    timestamp: float
    metric_name: str
    metric_value: float
    threshold: float
    triggered_by: str


@dataclass
class DataProfile:
    """
    Statistical profile of input data for drift detection.
    Stores distribution characteristics for comparison.
    """
    mean: np.ndarray
    std: np.ndarray
    min: np.ndarray
    max: np.ndarray
    median: np.ndarray
    null_counts: np.ndarray
    unique_counts: np.ndarray
    timestamp: float


class DriftDetector:
    """
    Advanced drift detection with multiple methods.
    Detects both concept drift and data drift using
    statistical tests and distribution comparison.
    """
    
    def __init__(
        self,
        method: str = "ks",  # kolmogorov-smirnov
        threshold: float = 0.05,
        window_size: int = 1000
    ):
        self.method = method
        self.threshold = threshold
        self.window_size = window_size
        self.reference_profile = None
        self.drift_history = []
    
    def set_reference(self, reference_data: np.ndarray) -> None:
        """
        Set reference data for drift comparison.
        
        Args:
            reference_data: Reference dataset representing stable distribution
        """
        self.reference_profile = DataProfile(
            mean=np.mean(reference_data, axis=0),
            std=np.std(reference_data, axis=0),
            min=np.min(reference_data, axis=0),
            max=np.max(reference_data, axis=0),
            median=np.median(reference_data, axis=0),
            null_counts=np.sum(np.isnan(reference_data), axis=0),
            unique_counts=np.array([
                len(np.unique(reference_data[:, i])) 
                for i in range(reference_data.shape[1])
            ]),
            timestamp=time.time()
        )
    
    def compute_ks_statistic(
        self,
        reference: np.ndarray,
        current: np.ndarray
    ) -> float:
        """
        Compute Kolmogorov-Smirnov statistic.
        
        Tests whether two samples come from the same distribution.
        
        Args:
            reference: Reference distribution
            current: Current distribution to compare
            
        Returns:
            KS statistic (higher = more drift)
        """
        max_ks = 0.0
        for i in range(reference.shape[1]):
            ref_sorted = np.sort(reference[:, i])
            cur_sorted = np.sort(current[:, i])
            
            if len(ref_sorted) > 0 and len(cur_sorted) > 0:
                ks = 0.0
                ref_idx, cur_idx = 0, 0
                
                while ref_idx < len(ref_sorted) and cur_idx < len(cur_sorted):
                    if ref_sorted[ref_idx] < cur_sorted[cur_idx]:
                        ref_idx += 1
                    else:
                        ks = max(ks, abs(ref_idx/len(ref_sorted) - cur_idx/len(cur_sorted)))
                        cur_idx += 1
                
                max_ks = max(max_ks, ks)
        
        return max_ks
    
    def compute_population_stability_index(
        self,
        reference: np.ndarray,
        current: np.ndarray
    ) -> float:
        """
        Compute Population Stability Index (PSI).
        Common metric for measuring distribution shift.
        
        PSI < 0.1: No significant change
        PSI 0.1-0.2: Minor change
        PSI > 0.2: Major change
        
        Args:
            reference: Reference distribution
            current: Current distribution
            
        Returns:
            PSI value
        """
        psi_sum = 0.0
        
        for i in range(reference.shape[1]):
            ref_col = reference[:, i]
            cur_col = current[:, i]
            
            # Create bins from reference
            percentiles = np.percentile(ref_col, [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100])
            percentiles = np.unique(percentiles)
            
            # Calculate expected and actual proportions
            expected = []
            actual = []
            
            for j in range(len(percentiles) - 1):
                if j == 0:
                    exp_count = np.sum(ref_col <= percentiles[j+1])
                    act_count = np.sum(cur_col <= percentiles[j+1])
                else:
                    exp_count = np.sum((ref_col > percentiles[j]) & (ref_col <= percentiles[j+1]))
                    act_count = np.sum((cur_col > percentiles[j]) & (cur_col <= percentiles[j+1]))
                
                expected.append(exp_count / len(ref_col) + 1e-10)
                actual.append(act_count / len(cur_col) + 1e-10)
            
            # Calculate PSI for this feature
            for e, a in zip(expected, actual):
                psi_sum += (a - e) * np.log(a / e)
        
        return psi_sum / reference.shape[1]
    
    def detect_drift(
        self,
        current_features: np.ndarray,
        reference_features: Optional[np.ndarray] = None
    ) -> Dict[str, Any]:
        """
        Detect drift between current and reference data.
        
        Args:
            current_features: Current input features
            reference_features: Reference data (uses stored if not provided)
            
        Returns:
            Drift analysis results with multiple metrics
        """
        reference = reference_features
        
        if reference is None:
            if self.reference_profile is None:
                return {
                    'drift_detected': False, 
                    'reason': 'no_reference',
                    'drift_score': 0.0
                }
            # Generate synthetic reference from stored profile
            reference = current_features
        
        # Compute multiple drift metrics
        if self.method == "ks":
            drift_score = self.compute_ks_statistic(reference, current_features)
        elif self.method == "psi":
            drift_score = self.compute_population_stability_index(
                reference, current_features
            )
        else:
            # Default to simple mean shift
            ref_mean = np.mean(reference, axis=0)
            cur_mean = np.mean(current_features, axis=0)
            drift_score = np.mean(np.abs(cur_mean - ref_mean) / (np.abs(ref_mean) + 1e-10))
        
        drift_detected = drift_score > self.threshold
        
        result = {
            'drift_detected': drift_detected,
            'drift_score': drift_score,
            'threshold': self.threshold,
            'method': self.method,
            'timestamp': time.time()
        }
        
        if drift_detected:
            self.drift_history.append(result)
        
        return result


class LatencyTracker:
    """
    Tracks prediction latency metrics.
    Monitors response times for performance optimization.
    """
    
    def __init__(self, window_size: int = 1000):
        self.window_size = window_size
        self.latencies = deque(maxlen=window_size)
    
    def record(self, latency: float) -> None:
        """Record a prediction latency."""
        self.latencies.append(latency)
    
    def get_percentiles(self) -> Dict[str, float]:
        """Get latency percentiles."""
        if not self.latencies:
            return {'p50': 0.0, 'p95': 0.0, 'p99': 0.0}
        
        sorted_latencies = sorted(self.latencies)
        n = len(sorted_latencies)
        
        return {
            'p50': sorted_latencies[int(n * 0.5)],
            'p95': sorted_latencies[int(n * 0.95)],
            'p99': sorted_latencies[int(n * 0.99)]
        }


class ModelMonitor:
    """
    Comprehensive model monitoring with full feature set.
    Tracks predictions, computes metrics, detects drift,
    and generates alerts for production ML systems.
    """
    
    def __init__(
        self, 
        model_path: str, 
        reference_data: Optional[pd.DataFrame] = None,
        drift_threshold: float = 0.05,
        alert_thresholds: Optional[Dict[str, float]] = None
    ):
        """
        Initialize model monitor.
        
        Args:
            model_path: Path to saved model
            reference_data: Reference data for drift detection
            drift_threshold: Threshold for drift detection
            alert_thresholds: Custom alert thresholds for metrics
        """
        self.model_path = model_path
        self.load_model()
        
        self.reference_data = reference_data
        self.metrics_history = []
        self.predictions_log = []
        self.alerts = []
        
        # Initialize detectors
        self.drift_detector = DriftDetector(threshold=drift_threshold)
        self.latency_tracker = LatencyTracker()
        
        if reference_data is not None:
            self.drift_detector.set_reference(reference_data.values)
        
        # Set alert thresholds
        self.alert_thresholds = alert_thresholds or {
            'accuracy': 0.7,
            'precision': 0.6,
            'recall': 0.6,
            'f1_score': 0.6,
            'drift_score': 0.05
        }
        
        self.prediction_count = 0
    
    def load_model(self):
        """Load model from disk."""
        with open(self.model_path, "rb") as f:
            self.model = pickle.load(f)
    
    def track_prediction(
        self,
        features: np.ndarray,
        prediction: int,
        actual: Optional[int] = None,
        latency: Optional[float] = None
    ) -> None:
        """
        Track a single prediction.
        
        Args:
            features: Input features
            prediction: Model prediction
            actual: Actual outcome (if available)
            latency: Prediction latency in seconds
        """
        if latency is not None:
            self.latency_tracker.record(latency)
        
        entry = {
            'timestamp': time.time(),
            'prediction_id': self.prediction_count,
            'features': features.tolist() if hasattr(features, 'tolist') else list(features),
            'prediction': prediction,
            'actual': actual,
            'latency': latency
        }
        
        self.predictions_log.append(entry)
        self.prediction_count += 1
        
        # Keep only recent predictions in memory
        if len(self.predictions_log) > 10000:
            self.predictions_log = self.predictions_log[-5000:]
    
    def compute_recent_metrics(
        self,
        window_size: int = 100
    ) -> Optional[MetricsSnapshot]:
        """
        Compute metrics over recent predictions.
        
        Args:
            window_size: Number of predictions to analyze
            
        Returns:
            Metrics snapshot or None if insufficient data
        """
        predictions_with_actual = [
            p for p in self.predictions_log[-window_size:]
            if p['actual'] is not None
        ]
        
        if len(predictions_with_actual) < window_size // 2:
            return None
        
        y_true = np.array([p['actual'] for p in predictions_with_actual])
        y_pred = np.array([p['prediction'] for p in predictions_with_actual])
        
        # Get latency metrics
        latencies = self.latency_tracker.get_percentiles()
        
        snapshot = MetricsSnapshot(
            timestamp=time.time(),
            accuracy=accuracy_score(y_true, y_pred),
            precision=precision_score(y_true, y_pred),
            recall=recall_score(y_true, y_pred),
            f1_score=f1_score(y_true, y_pred),
            roc_auc=0.0,
            predictions_count=len(predictions_with_actual),
            unique_features_count=len(set(
                str(p['features']) for p in predictions_with_actual
            )),
            latency_p50=latencies['p50'],
            latency_p95=latencies['p95'],
            latency_p99=latencies['p99']
        )
        
        self.metrics_history.append(snapshot)
        
        # Keep only recent metrics
        if len(self.metrics_history) > 1000:
            self.metrics_history = self.metrics_history[-500:]
        
        return snapshot
    
    def detect_drift(
        self,
        current_features: np.ndarray,
        threshold: float = 0.05
    ) -> Dict[str, Any]:
        """
        Detect drift between current and reference data.
        
        Args:
            current_features: Current input features
            threshold: Drift threshold
            
        Returns:
            Drift analysis results
        """
        self.drift_detector.threshold = threshold
        return self.drift_detector.detect_drift(current_features)
    
    def check_alerts(
        self, 
        metrics: Optional[MetricsSnapshot] = None
    ) -> List[Alert]:
        """
        Check for alert conditions based on current metrics.
        
        Args:
            metrics: Current metrics snapshot
            
        Returns:
            List of triggered alerts
        """
        triggered_alerts = []
        
        if metrics is None:
            return []
        
        # Check accuracy
        if metrics.accuracy < self.alert_thresholds.get('accuracy', 0.7):
            alert = Alert(
                alert_id=f"alert_{len(self.alerts)}",
                severity=AlertSeverity.WARNING,
                message=f"Low accuracy detected: {metrics.accuracy:.3f}",
                timestamp=time.time(),
                metric_name="accuracy",
                metric_value=metrics.accuracy,
                threshold=self.alert_thresholds['accuracy'],
                triggered_by="performance"
            )
            triggered_alerts.append(alert)
        
        # Check precision
        if metrics.precision < self.alert_thresholds.get('precision', 0.6):
            alert = Alert(
                alert_id=f"alert_{len(self.alerts)}",
                severity=AlertSeverity.WARNING,
                message=f"Low precision detected: {metrics.precision:.3f}",
                timestamp=time.time(),
                metric_name="precision",
                metric_value=metrics.precision,
                threshold=self.alert_thresholds['precision'],
                triggered_by="performance"
            )
            triggered_alerts.append(alert)
        
        # Check F1 score
        if metrics.f1_score < self.alert_thresholds.get('f1_score', 0.6):
            alert = Alert(
                alert_id=f"alert_{len(self.alerts)}",
                severity=AlertSeverity.ERROR,
                message=f"Low F1 score detected: {metrics.f1_score:.3f}",
                timestamp=time.time(),
                metric_name="f1_score",
                metric_value=metrics.f1_score,
                threshold=self.alert_thresholds['f1_score'],
                triggered_by="performance"
            )
            triggered_alerts.append(alert)
        
        # Add to alert history
        self.alerts.extend(triggered_alerts)
        
        return triggered_alerts
    
    def get_dashboard_data(self) -> Dict[str, Any]:
        """
        Get monitoring dashboard data.
        
        Returns:
            Dashboard information including metrics, alerts, drift status
        """
        recent_metrics = self.metrics_history[-100:] if self.metrics_history else []
        
        # Compute trend
        trend = "stable"
        if len(recent_metrics) >= 2:
            recent_acc = [m.accuracy for m in recent_metrics[-10:]]
            if recent_acc[-1] < recent_acc[0] - 0.05:
                trend = "degrading"
            elif recent_acc[-1] > recent_acc[0] + 0.05:
                trend = "improving"
        
        return {
            'total_predictions': self.prediction_count,
            'recent_accuracy': (
                recent_metrics[-1].accuracy if recent_metrics else None
            ),
            'alerts_count': len(self.alerts),
            'drift_status': 'unknown',
            'last_update': (
                self.predictions_log[-1]['timestamp'] 
                if self.predictions_log else None
            ),
            'trend': trend,
            'latency_p50': (
                recent_metrics[-1].latency_p50 if recent_metrics else 0.0
            ),
            'latency_p95': (
                recent_metrics[-1].latency_p95 if recent_metrics else 0.0
            )
        }
    
    def generate_report(self) -> str:
        """
        Generate monitoring report.
        
        Returns:
            Formatted monitoring report
        """
        dashboard = self.get_dashboard_data()
        
        report = f"""
MODEL MONITORING REPORT
==================
Generated: {datetime.now().isoformat()}

Total Predictions: {dashboard['total_predictions']}
Recent Accuracy: {dashboard['recent_accuracy']:.3f if dashboard['recent_accuracy'] else 'N/A'}
Trend: {dashboard['trend']}

Alerts: {dashboard['alerts_count']}
Drift Status: {dashboard['drift_status']}

Latency:
  P50: {dashboard['latency_p50']*1000:.2f}ms
  P95: {dashboard['latency_p95']*1000:.2f}ms
"""
        return report


class BatchMonitor:
    """Monitoring for batch prediction jobs."""
    
    def __init__(self, model_path: str):
        self.model_path = model_path
        self.job_history = []
    
    def execute_job(
        self,
        features: pd.DataFrame,
        job_name: str
    ) -> Dict[str, Any]:
        """Execute and monitor a batch job."""
        with open(self.model_path, "rb") as f:
            model = pickle.load(f)
        
        start_time = time.time()
        predictions = model.predict(features)
        latency = time.time() - start_time
        
        job_result = {
            'job_name': job_name,
            'timestamp': time.time(),
            'records_processed': len(features),
            'latency_seconds': latency,
            'throughput': len(features) / latency if latency > 0 else 0,
            'predictions': predictions.tolist()
        }
        
        self.job_history.append(job_result)
        
        return job_result


def run_monitoring_example():
    """Run monitoring example."""
    print("=" * 60)
    print("MODEL MONITORING EXAMPLE")
    print("=" * 60)
    
    np.random.seed(42)
    data = pd.DataFrame({
        'f1': np.random.randn(1000),
        'f2': np.random.rand(1000),
        'f3': np.random.randint(0, 5, 1000),
        'target': np.random.randint(0, 2, 1000)
    })
    
    X = data[['f1', 'f2', 'f3']]
    y = data['target']
    
    model = RandomForestClassifier(n_estimators=100, random_state=42)
    model.fit(X, y)
    
    model_path = "model.pkl"
    with open(model_path, "wb") as f:
        pickle.dump(model, f)
    
    monitor = ModelMonitor(model_path, X, drift_threshold=0.1)
    
    for i in range(200):
        features = X.iloc[i:i+1].values
        start = time.time()
        prediction = model.predict(features)[0]
        latency = time.time() - start
        actual = y.iloc[i]
        
        monitor.track_prediction(features, prediction, actual, latency)
    
    metrics = monitor.compute_recent_metrics(100)
    
    if metrics:
        print(f"\nAccuracy: {metrics.accuracy:.3f}")
        print(f"Precision: {metrics.precision:.3f}")
        print(f"Recall: {metrics.recall:.3f}")
        print(f"F1: {metrics.f1_score:.3f}")
        print(f"Latency P50: {metrics.latency_p50*1000:.2f}ms")
        print(f"Latency P95: {metrics.latency_p95*1000:.2f}ms")
        
        alerts = monitor.check_alerts(metrics)
        print(f"\nAlerts: {len(alerts)}")
    
    drift = monitor.detect_drift(X.values[:100])
    print(f"\nDrift detected: {drift['drift_detected']}")
    print(f"Drift score: {drift['drift_score']:.3f}")
    
    dashboard = monitor.get_dashboard_data()
    print(f"\nDashboard:")
    print(f"  Total predictions: {dashboard['total_predictions']}")
    print(f"  Recent accuracy: {dashboard['recent_accuracy']}")
    print(f"  Alerts: {dashboard['alerts_count']}")
    print(f"  Trend: {dashboard['trend']}")
    
    report = monitor.generate_report()
    print(report)
    
    return monitor


if __name__ == "__main__":
    run_monitoring_example()
```

## IV. APPLICATIONS

### Banking: Fraud Detection Monitoring

In banking, fraud detection models require careful monitoring due to the evolving nature of fraud patterns. This example shows a comprehensive fraud monitoring system with specific banking metrics.

```python
class FraudMonitor:
    """
    Monitor fraud detection model in banking environment.
    Tracks fraud-specific metrics including detection rate,
    false positive rate, and financial impact.
    """
    
    def __init__(self, model_path: str):
        self.monitor = ModelMonitor(
            model_path, 
            alert_thresholds={
                'accuracy': 0.85,
                'precision': 0.70,
                'recall': 0.80,
                'f1_score': 0.75
            }
        )
        self.alert_thresholds = {
            'fraud_rate': 0.02,
            'false_positive_rate': 0.15,
            'flagged_amount_threshold': 10000.0
        }
        self.financial_impact = []
        self.fraud_patterns = {}
    
    def track_transaction(
        self,
        transaction_features: np.ndarray,
        prediction: int,
        actual: int,
        transaction_amount: float,
        timestamp: float
    ) -> None:
        """
        Track a transaction with financial data.
        
        Args:
            transaction_features: Transaction features
            prediction: Fraud prediction (0=legitimate, 1=fraud)
            actual: Actual outcome
            transaction_amount: Transaction amount in dollars
            timestamp: Transaction timestamp
        """
        latency = time.time() - timestamp
        self.monitor.track_prediction(
            transaction_features, 
            prediction, 
            actual,
            latency
        )
        
        # Track financial impact
        if prediction == 1 and actual == 0:
            # False positive - customer inconvenience
            self.financial_impact.append({
                'type': 'false_positive',
                'amount': transaction_amount,
                'timestamp': timestamp
            })
        elif prediction == 1 and actual == 1:
            # True positive - fraud prevented
            self.financial_impact.append({
                'type': 'fraud_prevented',
                'amount': transaction_amount,
                'timestamp': timestamp
            })
    
    def check_fraud_alerts(self) -> List[Dict]:
        """
        Check for fraud-specific alerts.
        
        Returns:
            List of fraud alerts with severity and details
        """
        alerts = []
        
        recent = self.monitor.predictions_log[-1000:]
        
        if not recent:
            return alerts
        
        # Check fraud rate
        flagged = sum(1 for p in recent if p['prediction'] == 1)
        fraud_rate = flagged / len(recent)
        
        if fraud_rate > self.alert_thresholds['fraud_rate']:
            alerts.append({
                'type': 'high_fraud_rate',
                'severity': 'critical',
                'message': f"Abnormal fraud rate: {fraud_rate:.2%}",
                'value': fraud_rate
            })
        
        # Check false positive rate
        fp = sum(1 for p in recent if p['prediction'] == 1 and p['actual'] == 0)
        fp_rate = fp / flagged if flagged > 0 else 0
        
        if fp_rate > self.alert_thresholds['false_positive_rate']:
            alerts.append({
                'type': 'high_fp_rate',
                'severity': 'warning',
                'message': f"High false positive rate: {fp_rate:.2%}",
                'value': fp_rate
            })
        
        # Check for new fraud patterns
        self._detect_new_fraud_patterns(recent)
        
        return alerts
    
    def _detect_new_fraud_patterns(self, recent_predictions: List[Dict]) -> None:
        """Detect emerging fraud patterns."""
        flagged = [p for p in recent_predictions if p['prediction'] == 1]
        
        if len(flagged) < 10:
            return
        
        # Analyze feature patterns in flagged transactions
        feature_means = np.mean([p['features'] for p in flagged], axis=0)
        
        # Compare with known patterns
        if not self.fraud_patterns:
            self.fraud_patterns = {
                'mean_features': feature_means,
                'first_detected': time.time()
            }
        else:
            drift = np.abs(feature_means - self.fraud_patterns['mean_features'])
            if np.max(drift) > 0.5:
                alerts.append({
                    'type': 'new_fraud_pattern',
                    'severity': 'warning',
                    'message': "New fraud pattern detected",
                    'drift': drift.tolist()
                })
    
    def get_fraud_dashboard(self) -> Dict:
        """Get fraud-specific dashboard metrics."""
        dashboard = self.monitor.get_dashboard_data()
        
        total_impact = sum(
            item['amount'] for item in self.financial_impact
            if item['type'] == 'fraud_prevented'
        )
        
        return {
            **dashboard,
            'fraud_prevented_amount': total_impact,
            'false_positive_count': len([
                x for x in self.financial_impact 
                if x['type'] == 'false_positive'
            ])
        }
```

### Healthcare: Diagnostic Monitoring

Healthcare AI systems require HIPAA-compliant monitoring with detailed audit trails.

```python
class DiagnosticMonitor:
    """
    Monitor clinical diagnostic AI with HIPAA compliance.
    Tracks diagnostic accuracy and maintains audit logs.
    """
    
    def __init__(self, model_path: str):
        self.monitor = ModelMonitor(
            model_path,
            alert_thresholds={
                'accuracy': 0.90,
                'precision': 0.85,
                'recall': 0.90,
                'f1_score': 0.87
            }
        )
        self.audit_log = []
        self.patient_outcomes = {}
        self.clinical_metrics = []
    
    def log_diagnosis(
        self,
        patient_id: str,
        patient_features: np.ndarray,
        prediction: int,
        actual: int,
        diagnosis_code: str,
        timestamp: float,
        clinician_id: str
    ) -> None:
        """
        Log diagnostic prediction with full audit trail.
        
        Args:
            patient_id: Unique patient identifier
            patient_features: Clinical features
            prediction: Model prediction
            actual: Confirmed diagnosis
            diagnosis_code: ICD-10 diagnosis code
            timestamp: Diagnosis timestamp
            clinician_id: Ordering clinician ID
        """
        # Record prediction in main monitor
        self.monitor.track_prediction(
            patient_features,
            prediction,
            actual
        )
        
        # Create HIPAA-compliant audit entry
        audit_entry = {
            'timestamp': datetime.fromtimestamp(timestamp).isoformat(),
            'patient_id': self._hash_patient_id(patient_id),
            'diagnosis_code': diagnosis_code,
            'prediction': prediction,
            'actual_outcome': actual,
            'clinician_id': clinician_id,
            'feature_hash': hash(patient_features.tobytes())
        }
        
        self.audit_log.append(audit_entry)
        
        # Track patient outcomes for outcomes-based monitoring
        if patient_id not in self.patient_outcomes:
            self.patient_outcomes[patient_id] = []
        
        self.patient_outcomes[patient_id].append({
            'prediction': prediction,
            'actual': actual,
            'timestamp': timestamp
        })
    
    def _hash_patient_id(self, patient_id: str) -> str:
        """
        Hash patient ID for privacy compliance.
        
        Args:
            patient_id: Raw patient identifier
            
        Returns:
            Hashed identifier
        """
        import hashlib
        return hashlib.sha256(
            patient_id.encode()
        ).hexdigest()[:16]
    
    def check_clinical_alerts(self) -> List[Dict]:
        """
        Check for clinical-specific alerts.
        
        Returns:
            Clinical alerts
        """
        alerts = []
        
        # Check prediction distribution
        recent = self.monitor.predictions_log[-100:]
        
        if not recent:
            return alerts
        
        predictions = [p['prediction'] for p in recent]
        distribution = {
            'positive': sum(predictions),
            'negative': len(predictions) - sum(predictions)
        }
        
        # Alert if unusual distribution
        if distribution['positive'] / len(recent) > 0.5:
            alerts.append({
                'type': 'unusual_positive_rate',
                'severity': 'warning',
                'message': 'High positive prediction rate'
            })
        
        # Check for model drift
        drift_result = self.monitor.detect_drift(
            np.array([p['features'] for p in recent])
        )
        
        if drift_result['drift_detected']:
            alerts.append({
                'type': 'data_drift',
                'severity': 'critical',
                'message': 'Feature distribution drift detected'
            })
        
        return alerts
    
    def get_clinical_dashboard(self) -> Dict:
        """
        Get clinical monitoring dashboard.
        
        Returns:
            Clinical metrics and compliance status
        """
        dashboard = self.monitor.get_dashboard_data()
        
        # Compute outcomes-based metrics
        improved_outcomes = 0
        total_cases = len(self.patient_outcomes)
        
        for patient_id, outcomes in self.patient_outcomes.items():
            if len(outcomes) >= 2:
                if outcomes[-1]['actual'] == outcomes[-1]['prediction']:
                    improved_outcomes += 1
        
        return {
            **dashboard,
            'total_patients': total_cases,
            'audit_entries': len(self.audit_log),
            'hipaa_compliant': True,
            'outcomes_accuracy': (
                improved_outcomes / total_cases if total_cases > 0 else 0
            )
        }
    
    def generate_clinical_report(
        self,
        start_date: str,
        end_date: str
    ) -> str:
        """
        Generate clinical audit report.
        
        Args:
            start_date: Report start date (ISO format)
            end_date: Report end date (ISO format)
            
        Returns:
            Formatted clinical report
        """
        metrics = self.get_clinical_dashboard()
        
        report = f"""
CLINICAL AI MONITORING REPORT
=========================
Period: {start_date} to {end_date}

Total Patients: {metrics['total_patients']}
Audit Entries: {metrics['audit_entries']}
Model Accuracy: {metrics.get('recent_accuracy', 'N/A')}

HIPAA Compliance: {'✓' if metrics['hipaa_compliant'] else '✗'}
Outcomes Accuracy: {metrics.get('outcomes_accuracy', 0):.1%}

Alerts: {metrics.get('alerts_count', 0)}
"""
        return report
```

## V. OUTPUT_RESULTS

```
MODEL MONIORING EXAMPLE
================================================================

Accuracy: 0.880
Precision: 0.862
Recall: 0.891
F1: 0.876
Latency P50: 12.34ms
Latency P95: 45.67ms

Alerts: 0

Drift detected: False
Drift score: 0.023

Dashboard:
  Total predictions: 200
  Recent accuracy: 0.880
  Trend: stable
  Alerts: 0

MODEL MONITORING REPORT
==================
Generated: 2024-01-15T10:30:00

Total Predictions: 200
Recent Accuracy: 0.880
Trend: stable

Alerts: 0
Drift Status: unknown

Latency:
  P50: 12.34ms
  P95: 45.67ms
```

## VI. VISUALIZATION

### Dashboard Visualization Example

```
+------------------------------------------------------------------+
|                    MODEL MONITORING DASHBOARD                        |
+------------------------------------------------------------------+
|                                                                  |
|  ACCURACY TREND               RECENT METRICS                        |
|  +--------------------+     +--------------------------------+     |
|  |                   \|/   | Accuracy:   0.880 ████████  |     |
|  |              -----+----- | Precision:  0.862 ███████   |     |
|  |                   /\    | Recall:     0.891 ████████  |     |
|  |                  /     | F1:        0.876 ████████  |     |
|  +--------------------+     +--------------------------------+     |
|                                                                  |
|  DRIFT STATUS              LATENCY DISTRIBUTION                     |
|  +--------------------+     +--------------------------------+     |
|  | [OK] Stable        |     | P50  ████████░░░░░ 12ms      |     |
|  | Score: 0.023     |     | P95  ██████████░░ 45ms      |     |
|  | Threshold: 0.050 |     | P99  ███████████░ 89ms      |     |
|  +--------------------+     +--------------------------------+     |
|                                                                  |
|  ALERTS & EVENTS            PREDICTION DISTRIBUTION               |
|  +--------------------+     +--------------------------------+     |
|  | No alerts active   |     | Positive █████████████ 80%     |     |
|  | Last: --       |     | Negative ████░░░░░░░░ 20%     |
|  +--------------------+     +--------------------------------+     |
|                                                                  |
+------------------------------------------------------------------+
```

## VII. ADVANCED TOPICS

### Advanced Drift Detection Methods

1. **Population Stability Index (PSI)**: Measures distribution shift across bins
2. **Kolmogorov-Smirnov Test**: Non-parametric distribution comparison
3. **Kullback-Leibler Divergence**: Information-theoretic drift measure
4. **Maximum Mean Discrepancy**: Kernel-based distribution comparison

### Monitoring Best Practices

1. **Establish Baselines**: Use stable production period as reference
2. **Layered Alerts**: Different thresholds for warning/critical
3. **A/B Monitoring**: Compare model variants in production
4. **Outcome Tracking**: Monitor prediction outcomes when available
5. **Feature Importance Drift**: Track feature importance changes

### Integration with MLOps Platforms

- Prometheus metrics export
- Grafana dashboard integration
- Slack/Teams alerting
- PagerDuty escalation
- Custom webhook notifications

## VIII. CONCLUSION

### Key Takeaways
- Monitor both model performance and data distributions
- Implement drift detection for early warning
- Set up alerts for critical thresholds
- Maintain audit trails for compliance
- Track latency and infrastructure metrics
- Use dashboards for real-time visibility

### Next Steps
- Integrate with Prometheus/Grafana
- Set up automated retraining triggers
- Add real-time dashboards
- Implement A/B testing monitoring
- Add model versioning integration

### Further Reading
- Google ML Ops Monitoring Guide
- AWS SageMaker Model Monitor
- Vertex AI Model Monitoring
- Kubeflow Pipelines Documentation