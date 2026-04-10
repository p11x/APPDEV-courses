# Fairness Metrics and Evaluation

## I. INTRODUCTION

### What are Fairness Metrics?
Fairness metrics are quantitative measures that assess whether ML models treat different demographic groups equitably. These metrics provide numerical indicators of model fairness, enabling comparison across models and tracking fairness improvements over time. Different metrics capture different notions of fairness, and the appropriate metric depends on the specific application and legal requirements.

Understanding fairness metrics is crucial because:
- They provide objective, comparable measures
- They enable automated fairness testing in CI/CD
- They help identify specific groups affected by bias
- They support regulatory compliance documentation

## II. FAIRNESS TAXONOMY AND THEORY

### Types of Fairness

**Group Fairness**: Equal outcomes across groups - aims to ensure that positive outcomes are distributed equally across demographic groups.

**Individual Fairness**: Similar individuals get similar outcomes - ensures that similar individuals receive similar predictions.

**Counterfactual Fairness**: Flipping protected attribute doesn't change outcome - asks "would the decision be different if the protected attribute were different?"

**Procedural Fairness**: Ensures that the process used to make decisions is fair and consistent.

**Substantive Fairness**: Focuses on achieving equal outcomes in the long run.

### Fairness-Accuracy Trade-offs

There's often a mathematical trade-off between different fairness metrics and accuracy. Understanding this is crucial:

```
Fairness-Accuracy Trade-off Visualization
====================================

        Accuracy
          ^
          |      * accuracy-only model
    0.95 |     /\
          |    /  \
          |   /    \    * fair model
    0.90 |  /      \  /\
          | /        \/  \
          |/          \  *\
    0.85 |            \  / \
          |             \/   \
          +-----------------+--->
            Statistical False Positive Rate
                 Parity
```

Key observations:
- Perfect fairness and maximum accuracy are often mutually exclusive
- The degree of trade-off depends on base rates across groups
- Trade-offs can be minimized with targeted interventions

### Key Fairness Metrics - Detailed Explanations

#### Demographic Parity (Statistical Parity Difference)

Demographic parity requires that the positive outcome rate be equal across groups:

```
Demographic Parity = P(Label=1|Group=A) - P(Label=1|Group=B)

Interpretation:
- = 0: Perfect demographic parity (fair)
- > 0: Group A has higher positive rate
- < 0: Group B has higher positive rate
```

#### Equalized Odds

Equalized odds requires that both true positive rate (TPR) and false positive rate (FPR) be equal across groups:

```
TPR_Group = P(Prediction=1|Actual=1, Group)
FPR_Group = P(Prediction=1|Actual=0, Group)

Equalized Odds: TPR_A = TPR_B AND FPR_A = FPR_B
```

#### Equal Opportunity (TPR Parity)

A simplified version focusing only on TPR equality:

```
Equal Opportunity = TPR_A - TPR_B

This is particularly important when:
- Positive outcomes are desired (e.g., hiringqualified candidates)
- Missing positives has high cost
```

#### Predictive Parity

Requires that predictive values be equal across groups:

```
PPV_Group = P(Actual=1|Prediction=1, Group)
NPV_Group = P(Actual=0|Prediction=0, Group)

Predictive Parity: PPV_A = PPV_B AND NPV_A = NPV_B
```

#### Calibration

Ensures calibrated probabilities are equal across groups:

```
Calibration: P(Actual=1|Predicted=p, Group) = p

For all p in [0,1] and all groups.
```

### Extended Implementations

### Implementation with Detailed Metrics

```python
"""
Fairness Metrics Implementation
===============================
Comprehensive fairness metric computation.
"""

import numpy as np
import pandas as pd
from typing import Dict, List, Tuple
from dataclasses import dataclass
from collections import defaultdict


@dataclass
class FairnessReport:
    """Complete fairness evaluation."""
    statistical_parity: float
    equal_opportunity: float
    average_odds: float
    disparate_impact: float
    theil_index: float


@dataclass
class GroupMetrics:
    """Metrics for a single group."""
    base_rate: float
    positive_rate: float
    tpr: float
    fpr: float
    ppv: float
    npv: float
    accuracy: float


class ComprehensiveFairnessEvaluator:
    """
    Comprehensive Fairness Evaluation
    ==============================
    Detailed fairness metrics computation.
    """
    
    def __init__(self, protected_attr: str):
        self.protected_attr = protected_attr
        self.group_metrics = {}
        self.thresholds = {
            'demographic_parity': 0.1,
            'equal_opportunity': 0.1,
            'disparate_impact': 0.8,
            'theil_index': 0.1
        }
    
    def compute_detailed_metrics(
        self,
        y_true: np.ndarray,
        y_pred: np.ndarray,
        y_proba: np.ndarray,
        groups: np.ndarray
    ) -> Dict[str, Dict]:
        """Compute detailed metrics for each group."""
        unique_groups = np.unique(groups)
        all_metrics = {}
        
        for group in unique_groups:
            mask = groups == group
            
            y_true_g = y_true[mask]
            y_pred_g = y_pred[mask]
            y_proba_g = y_proba[mask]
            
            tp = ((y_pred_g == 1) & (y_true_g == 1)).sum()
            tn = ((y_pred_g == 0) & (y_true_g == 0)).sum()
            fp = ((y_pred_g == 1) & (y_true_g == 0)).sum()
            fn = ((y_pred_g == 0) & (y_true_g == 1)).sum()
            
            n_positive = (y_true_g == 1).sum()
            n_negative = (y_true_g == 0).sum()
            
            all_metrics[group] = {
                'base_rate': n_positive / len(y_true_g),
                'positive_rate': y_pred_g.mean(),
                'tpr': tp / n_positive if n_positive > 0 else 0,
                'fpr': fp / n_negative if n_negative > 0 else 0,
                'ppv': tp / (tp + fp) if (tp + fp) > 0 else 0,
                'npv': tn / (tn + fn) if (tn + fn) > 0 else 0,
                'accuracy': (tp + tn) / len(y_true_g)
            }
        
        self.group_metrics = all_metrics
        return all_metrics
    
    def evaluate(
        self,
        y_true: np.ndarray,
        y_pred: np.ndarray,
        y_proba: np.ndarray,
        groups: np.ndarray
    ) -> FairnessReport:
        """Compute all fairness metrics."""
        
        return FairnessReport(
            statistical_parity=self._statistical_parity(y_pred, groups),
            equal_opportunity=self._equal_opportunity(y_true, y_pred, groups),
            average_odds=self._average_odds(y_true, y_pred, groups),
            disparate_impact=self._disparate_impact(y_pred, groups),
            theil_index=self._theil_index(y_true, y_proba)
        )
    
    def _statistical_parity(self, y_pred: np.ndarray, groups: np.ndarray) -> float:
        """Selection rate difference between groups."""
        unique_groups = np.unique(groups)
        if len(unique_groups) < 2:
            return 1.0
        
        rates = [y_pred[groups == g].mean() for g in unique_groups]
        return abs(rates[0] - rates[1])
    
    def _equal_opportunity(self, y_true: np.ndarray, y_pred: np.ndarray, groups: np.ndarray) -> float:
        """True positive rate difference."""
        unique_groups = np.unique(groups)
        
        tprs = []
        for g in unique_groups:
            mask = groups == g
            positives = y_true[mask] == 1
            if positives.sum() > 0:
                tprs.append(y_pred[mask & (y_true == 1)].mean())
        
        return abs(tprs[0] - tprs[1]) if len(tprs) == 2 else 0
    
    def _average_odds(self, y_true: np.ndarray, y_pred: np.ndarray, groups: np.ndarray) -> float:
        """Average of TPR and FPR differences."""
        unique_groups = np.unique(groups)
        
        tprs, fprs = [], []
        for g in unique_groups:
            mask = groups == g
            if (y_true[mask] == 1).sum() > 0:
                tprs.append(y_pred[mask & (y_true == 1)].mean())
            if (y_true[mask] == 0).sum() > 0:
                fprs.append(y_pred[mask & (y_true == 0)].mean())
        
        tpr_diff = abs(tprs[0] - tprs[1]) if len(tprs) == 2 else 0
        fpr_diff = abs(fprs[0] - fprs[1]) if len(fprs) == 2 else 0
        
        return (tpr_diff + fpr_diff) / 2
    
    def _disparate_impact(self, y_pred: np.ndarray, groups: np.ndarray) -> float:
        """Ratio of positive rates."""
        unique_groups = np.unique(groups)
        if len(unique_groups) < 2:
            return 1.0
        
        rates = [y_pred[groups == g].mean() for g in unique_groups]
        return min(rates) / max(rates) if max(rates) > 0 else 1.0
    
    def _theil_index(self, y_true: np.ndarray, y_proba: np.ndarray) -> float:
        """Entropy-based inequality measure."""
        epsilon = 1e-10
        normalized = np.clip(y_proba, epsilon, 1 - epsilon)
        return np.mean(normalized * np.log(normalized))
    
    def fairness_violations(
        self,
        y_true: np.ndarray,
        y_pred: np.ndarray,
        y_proba: np.ndarray,
        groups: np.ndarray
    ) -> List[Dict]:
        """Identify which fairness criteria are violated."""
        report = self.evaluate(y_true, y_pred, y_proba, groups)
        violations = []
        
        if report.statistical_parity > self.thresholds['demographic_parity']:
            violations.append({
                'metric': 'demographic_parity',
                'value': report.statistical_parity,
                'threshold': self.thresholds['demographic_parity'],
                'type': 'group_fairness'
            })
        
        if report.equal_opportunity > self.thresholds['equal_opportunity']:
            violations.append({
                'metric': 'equal_opportunity',
                'value': report.equal_opportunity,
                'threshold': self.thresholds['equal_opportunity'],
                'type': 'error_rate_fairness'
            })
        
        if report.disparate_impact < self.thresholds['disparate_impact']:
            violations.append({
                'metric': 'disparate_impact',
                'value': report.disparate_impact,
                'threshold': self.thresholds['disparate_impact'],
                'type': 'group_fairness'
            })
        
        return violations


class FairnessOptimizer:
    """
    Fairness Optimization
    ===================
    Techniques to improve model fairness.
    """
    
    def __init__(self):
        self.constrained_model = None
    
    def reweight(
        self,
        X: np.ndarray,
        y: np.ndarray,
        groups: np.ndarray
    ) -> Dict[str, float]:
        """Reweight samples to achieve fairness."""
        unique_groups = np.unique(groups)
        
        counts = {g: (groups == g).sum() for g in unique_groups}
        
        base_rate = y.mean()
        
        weights = {}
        for group in unique_groups:
            group_mask = groups == group
            group_base_rate = y[group_mask].mean()
            
            if group_base_rate > 0:
                weights[group] = base_rate / group_base_rate
            else:
                weights[group] = 1.0
        
        return weights
    
    def optimize_threshold_per_group(
        self,
        X: np.ndarray,
        y_true: np.ndarray,
        y_proba: np.ndarray,
        groups: np.ndarray,
        fair_metric: str = 'equal_opportunity'
    ) -> Dict[str, float]:
        """Optimize decision threshold per group."""
        unique_groups = np.unique(groups)
        
        thresholds = {}
        for group in unique_groups:
            mask = groups == group
            group_probas = y_proba[mask]
            group_labels = y_true[mask]
            
            best_threshold = 0.5
            best_metric_value = float('inf')
            
            for threshold in np.arange(0.1, 0.9, 0.05):
                preds = (group_probas >= threshold).astype(int)
                
                if preds.sum() > 0:
                    tpr = preds[group_labels == 1].mean()
                    
                    if abs(tpr - 0.8) < best_metric_value:
                        best_metric_value = abs(tpr - 0.8)
                        best_threshold = threshold
            
            thresholds[group] = best_threshold
        
        return thresholds
    
    def adversarial_debiasing(
        self,
        X: np.ndarray,
        y: np.ndarray,
        groups: np.ndarray,
        epochs: int = 100
    ) -> np.ndarray:
        """Apply adversarial debiasing."""
        n_samples, n_features = X.shape
        
        X_debiased = X.copy()
        
        for epoch in range(epochs):
            group_predictions = np.zeros(len(groups))
            
            for i, x in enumerate(X_debiased):
                group_predictions[i] = self._predict_group(x, groups[i])
            
            correction = np.zeros(n_features)
            for i in range(n_samples):
                if group_predictions[i] != groups[i]:
                    correction += X_debiased[i]
            
            correction /= n_samples
            X_debiased -= 0.01 * correction
        
        return X_debiased
    
    def _predict_group(self, x: np.ndarray, group: int) -> int:
        """Predict protected group (simplified)."""
        return group


class FairnessTestingPipeline:
    """
    Fairness Testing Pipeline
    =====================
    End-to-end fairness testing.
    """
    
    def __init__(self):
        self.evaluator = ComprehensiveFairnessEvaluator('group')
        self.optimizer = FairnessOptimizer()
        self.results = []
    
    def test_model(
        self,
        model,
        X_test: np.ndarray,
        y_test: np.ndarray,
        groups_test: np.ndarray,
        sensitive_features: List[str]
    ) -> Dict:
        """Full fairness test of a model."""
        y_pred = model.predict(X_test)
        y_proba = model.predict_proba(X_test)[:, 1]
        
        detailed_metrics = self.evaluator.compute_detailed_metrics(
            y_test, y_pred, y_proba, groups_test
        )
        
        fairness_report = self.evaluator.evaluate(
            y_test, y_pred, y_proba, groups_test
        )
        
        violations = self.evaluator.fairness_violations(
            y_test, y_pred, y_proba, groups_test
        )
        
        return {
            'group_metrics': detailed_metrics,
            'fairness_report': fairness_report,
            'violations': violations,
            'recommendations': self._generate_recommendations(violations)
        }
    
    def _generate_recommendations(
        self,
        violations: List[Dict]
    ) -> List[str]:
        """Generate improvement recommendations."""
        recommendations = []
        
        if any(v['metric'] == 'demographic_parity' for v in violations):
            recommendations.append(
                "Consider adjusting decision thresholds per demographic group"
            )
        
        if any(v['metric'] == 'equal_opportunity' for v in violations):
            recommendations.append(
                "Review feature engineering for potential proxy discrimination"
            )
        
        if any(v['metric'] == 'disparate_impact' for v in violations):
            recommendations.append(
                "Apply reweighting or post-processing to equalize positive rates"
            )
        
        if not recommendations:
            recommendations.append("Model passes all fairness tests")
        
        return recommendations


def run_fairness_example():
    """Run fairness evaluation."""
    print("=" * 50)
    print("FAIRNESS METRICS EVALUATION")
    print("=" * 50)
    
    np.random.seed(42)
    y_true = np.random.randint(0, 2, 500)
    y_pred = np.random.randint(0, 2, 500)
    y_proba = np.random.rand(500)
    groups = np.random.choice(['A', 'B'], 500)
    
    evaluator = FairnessEvaluator('group')
    report = evaluator.evaluate(y_true, y_pred, y_proba, groups)
    
    print(f"\nStatistical Parity: {report.statistical_parity:.3f}")
    print(f"Equal Opportunity: {report.equal_opportunity:.3f}")
    print(f"Average Odds: {report.average_odds:.3f}")
    print(f"Disparate Impact: {report.disparate_impact:.3f}")
    print(f"Theil Index: {report.theil_index:.3f}")
    
    return report


if __name__ == "__main__":
    run_fairness_example()
```

## IV. CONCLUSION

### Key Takeaways

1. **Multiple Fairness Metrics Capture Different Aspects**
   - Statistical Parity: Group fairness in outcomes
   - Equal Opportunity: Equal TPR across groups
   - Disparate Impact: Ratio-based fairness

2. **Choose Metrics Based on Legal and Ethical Requirements**
   - GDPR: Non-discrimination requirements
   - EEOC: Disparate impact analysis
   - Industry-specific guidelines

3. **Regular Evaluation Ensures Ongoing Fairness**
   - Test during development
   - Monitor in production
   - Re-evaluate when data changes

### Next Steps

- Implement fairness testing in CI/CD
- Document fairness decisions
- Regular fairness audits

### Further Reading

- Fairness and Machine Learning (Barrett & Wu)
- AI Fairness Tutorial (ICML 2024)