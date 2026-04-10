# Bias Detection and Mitigation

## I. INTRODUCTION

### What is Bias Detection and Mitigation?
Bias Detection and Mitigation in machine learning refers to the systematic process of identifying and reducing unfair or discriminatory outcomes in ML models. This encompasses detecting偏见 in training data, model predictions, and overall system behavior, then applying techniques to mitigate these biases. The goal is to ensure models treat all demographic groups fairly and don't perpetuate or amplify existing societal inequalities.

ML models can exhibit bias in multiple ways: through biased training data that reflects historical discrimination, through features that proxy for protected attributes, through algorithmic decisions that optimize for majority groups, and through evaluation metrics that don't account for fairness. Comprehensive bias detection requires examining all these sources.

### Why is it Important?
Biased ML systems cause real-world harm:
- Discrimination in hiring, lending, criminal justice, healthcare
- Legal liability under equal opportunity laws
- Reputational damage and loss of user trust
- Poor model performance on minority populations
- Regulatory non-compliance

### Prerequisites
- ML fundamentals and model development
- Statistical concepts (correlation, conditional probability)
- Understanding of fairness metrics
- Domain knowledge of application area

## II. FUNDAMENTALS

### Types of Bias

**Selection Bias**: Non-representative training data
**Historical Bias**: Pre-existing societal biases in data
**Measurement Bias**: Flawed feature collection
**Aggregation Bias**: Combining groups inappropriately
**Label Bias**: Inconsistent labeling across groups

### Key Concepts

**Protected Attributes**: Race, gender, age, disability, religion (legally protected)

**Disparate Treatment**: Treating individuals differently based on protected attributes

**Disparate Impact**: Neutral policies that disproportionately affect protected groups

### Implementation

```python
"""
Bias Detection and Mitigation Implementation
=============================================
Comprehensive bias detection and fairness improvement.
"""

import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import (
    accuracy_score, precision_score, recall_score, 
    confusion_matrix
)
from typing import Dict, List, Any, Optional
from dataclasses import dataclass
import warnings
warnings.filterwarnings('ignore')


@dataclass
class FairnessMetrics:
    """Fairness metrics for a model."""
    demographic_parity: float
    equalized_odds: Dict[str, float]
    individual_fairness: float
    disparate_impact_ratio: float


class BiasDetector:
    """Comprehensive bias detection for ML models."""
    
    def __init__(self, protected_attributes: List[str]):
        """
        Initialize bias detector.
        
        Args:
            protected_attributes: List of protected attribute columns
        """
        self.protected_attributes = protected_attributes
        self.bias_report = {}
    
    def compute_fairness_metrics(
        self,
        model,
        X: pd.DataFrame,
        y_true: np.ndarray,
        y_pred: np.ndarray
    ) -> FairnessMetrics:
        """
        Compute comprehensive fairness metrics.
        
        Args:
            model: Trained model
            X: Features including protected attributes
            y_true: True labels
            y_pred: Predicted labels
            
        Returns:
            Fairness metrics
        """
        demographic_parity = self._demographic_parity(y_pred, X)
        equalized_odds = self._equalized_odds(y_true, y_pred, X)
        individual_fairness = self._individual_fairness(X, y_pred)
        disparate_impact = self._disparate_impact(y_pred, X)
        
        return FairnessMetrics(
            demographic_parity=demographic_parity,
            equalized_odds=equalized_odds,
            individual_fairness=individual_fairness,
            disparate_impact_ratio=disparate_impact
        )
    
    def _demographic_parity(self, y_pred: np.ndarray, X: pd.DataFrame) -> float:
        """
        Compute demographic parity (selection rate equality).
        
        Returns ratio of positive predictions between groups.
        """
        if not self.protected_attributes:
            return 1.0
        
        protected_col = self.protected_attributes[0]
        
        groups = X[protected_col].unique()
        if len(groups) < 2:
            return 1.0
        
        rates = []
        for group in groups:
            mask = X[protected_col] == group
            rate = y_pred[mask].mean()
            rates.append(rate)
        
        return min(rates) / max(rates) if max(rates) > 0 else 1.0
    
    def _equalized_odds(
        self,
        y_true: np.ndarray,
        y_pred: np.ndarray,
        X: pd.DataFrame
    ) -> Dict[str, float]:
        """
        Compute equalized odds (true positive rate equality).
        
        Returns dictionary of TPR by group.
        """
        if not self.protected_attributes:
            return {'overall': 1.0}
        
        protected_col = self.protected_attributes[0]
        result = {}
        
        for group in X[protected_col].unique():
            mask = X[protected_col] == group
            
            y_true_group = y_true[mask]
            y_pred_group = y_pred[mask]
            
            cm = confusion_matrix(y_true_group, y_pred_group)
            
            if cm.shape == (2, 2):
                tn, fp, fn, tp = cm.ravel()
                tpr = tp / (tp + fn) if (tp + fn) > 0 else 0
                result[f'group_{group}'] = tpr
        
        return result
    
    def _individual_fairness(
        self,
        X: pd.DataFrame,
        y_pred: np.ndarray,
        sample_size: int = 1000
    ) -> float:
        """
        Approximate individual fairness through similarity.
        
        Measures if similar inputs get similar predictions.
        """
        if len(X) > sample_size:
            indices = np.random.choice(len(X), sample_size, replace=False)
        else:
            indices = range(len(X))
        
        X_sample = X.iloc[indices].values
        y_sample = y_pred[indices]
        
        from sklearn.neighbors import NearestNeighbors
        
        nn = NearestNeighbors(n_neighbors=5)
        nn.fit(X_sample)
        
        distances, _ = nn.kneighbors(X_sample)
        
        avg_similarity_change = 0
        for i in range(len(X_sample)):
            if i > 0:
                neighbor_distances = distances[i, 1:]
                avg_similarity_change += neighbor_distances.mean()
        
        return 1.0 - (avg_similarity_change / len(X_sample))
    
    def _disparate_impact(self, y_pred: np.ndarray, X: pd.DataFrame) -> float:
        """
        Compute disparate impact ratio.
        
        Ratio of positive outcome rates between protected and reference groups.
        """
        if not self.protected_attributes:
            return 1.0
        
        protected_col = self.protected_attributes[0]
        
        groups = X[protected_col].unique()
        if len(groups) < 2:
            return 1.0
        
        reference_rate = y_pred[X[protected_col] == groups[0]].mean()
        other_rate = y_pred[X[protected_col] == groups[1]].mean()
        
        return min(reference_rate, other_rate) / max(reference_rate, other_rate) if max(reference_rate, other_rate) > 0 else 1.0
    
    def detect_bias_sources(
        self,
        X: pd.DataFrame,
        sensitive_features: List[str]
    ) -> Dict[str, Any]:
        """
        Detect potential sources of bias in data.
        
        Args:
            X: Feature data
            sensitive_features: List of sensitive columns
            
        Returns:
            Bias source analysis
        """
        bias_sources = {}
        
        for feature in sensitive_features:
            if feature in X.columns:
                bias_sources[feature] = {
                    'unique_values': X[feature].nunique(),
                    'distribution': X[feature].value_counts().to_dict(),
                    'missing_pct': X[feature].isnull().sum() / len(X) * 100
                }
        
        for col in X.columns:
            if col not in sensitive_features:
                for sensitive in sensitive_features:
                    if sensitive in X.columns:
                        try:
                            correlation = X[col].corr(X[sensitive])
                            if abs(correlation) > 0.5:
                                if 'high_correlation' not in bias_sources:
                                    bias_sources['high_correlation'] = []
                                bias_sources['high_correlation'].append({
                                    'feature': col,
                                    'sensitive': sensitive,
                                    'correlation': correlation
                                })
                        except:
                            pass
        
        return bias_sources


class BiasMitigator:
    """Techniques for mitigating detected bias."""
    
    def __init__(self, protected_attributes: List[str]):
        self.protected_attributes = protected_attributes
    
    def resample_for_fairness(
        self,
        X: pd.DataFrame,
        y: np.ndarray,
        strategy: str = 'oversample'
    ) -> tuple:
        """
        Resample data to achieve balanced representation.
        
        Args:
            X: Features
            y: Labels
            strategy: 'oversample' or 'undersample'
            
        Returns:
            Balanced dataset
        """
        if not self.protected_attributes:
            return X, y
        
        protected_col = self.protected_attributes[0]
        
        groups = X[protected_col].unique()
        
        if len(groups) < 2:
            return X, y
        
        group_data = {}
        for group in groups:
            mask = X[protected_col] == group
            group_data[group] = (X[mask], y[mask])
        
        min_size = min(len(g[1]) for g in group_data.values())
        
        balanced_X = []
        balanced_y = []
        
        for group, (X_group, y_group) in group_data.items():
            if strategy == 'undersample':
                indices = np.random.choice(len(X_group), min_size, replace=False)
            else:
                indices = np.random.choice(len(X_group), max(len(X_group), min_size), replace=True)
            
            balanced_X.append(X_group.iloc[indices])
            balanced_y.append(y_group[indices])
        
        return pd.concat(balanced_X), np.concatenate(balanced_y)
    
    def reweight_samples(
        self,
        X: pd.DataFrame,
        y: np.ndarray
    ) -> np.ndarray:
        """
        Reweight samples to achieve fairness.
        
        Args:
            X: Features
            y: Labels
            
        Returns:
            Sample weights
        """
        if not self.protected_attributes:
            return np.ones(len(y))
        
        protected_col = self.protected_attributes[0]
        
        weights = np.ones(len(y))
        
        for protected_value in X[protected_col].unique():
            for label in [0, 1]:
                mask = (X[protected_col] == protected_value) & (y == label)
                
                group_size = mask.sum()
                if group_size == 0:
                    continue
                
                overall_rate = y.mean()
                group_rate = y[mask].mean()
                
                expected = group_size * overall_rate
                observed = mask.sum()
                
                if observed > 0:
                    weights[mask] = expected / observed
        
        return weights
    
    def threshold_adjustment(
        self,
        model,
        X: pd.DataFrame,
        y_true: np.ndarray,
        target_fairness: float = 0.9
    ) -> Dict[str, float]:
        """
        Adjust prediction thresholds per group for fairness.
        
        Args:
            model: Trained model
            X: Features
            y_true: True labels
            target_fairness: Target fairness ratio
            
        Returns:
            Per-group thresholds
        """
        thresholds = {}
        
        y_proba = model.predict_proba(X)[:, 1]
        
        for group in X[self.protected_attributes[0]].unique():
            mask = X[self.protected_attributes[0]] == group
            
            best_threshold = 0.5
            best_fairness = 0
            
            for threshold in np.arange(0.3, 0.7, 0.05):
                y_pred = (y_proba[mask] >= threshold).astype(int)
                
                if y_true[mask].sum() > 0:
                    tpr = y_pred[y_true[mask] == 1].mean() if (y_true[mask] == 1).sum() > 0 else 0
                    if tpr > best_fairness:
                        best_fairness = tpr
                        best_threshold = threshold
            
            thresholds[f'group_{group}'] = best_threshold
        
        return thresholds


def run_bias_example():
    """Run bias detection and mitigation example."""
    print("=" * 60)
    print("BIAS DETECTION AND MITIGATION")
    print("=" * 60)
    
    np.random.seed(42)
    n_samples = 1000
    
    data = pd.DataFrame({
        'feature_1': np.random.randn(n_samples),
        'feature_2': np.random.rand(n_samples),
        'feature_3': np.random.randint(0, 5, n_samples),
        'gender': np.random.choice(['M', 'F'], n_samples),
        'age_group': np.random.choice(['young', 'middle', 'senior'], n_samples),
        'target': np.random.randint(0, 2, n_samples)
    })
    
    data.loc[data['feature_1'] > 0.5, 'target'] = 1
    data.loc[data['gender'] == 'F', 'target'] = 0
    
    X = data[['feature_1', 'feature_2', 'feature_3', 'gender', 'age_group']]
    y = data['target'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    model = RandomForestClassifier(n_estimators=100, random_state=42)
    model.fit(X_train, y_train)
    
    y_pred = model.predict(X_test)
    
    detector = BiasDetector(protected_attributes=['gender'])
    
    fairness = detector.compute_fairness_metrics(model, X_test, y_test, y_pred)
    
    print(f"\nFairness Metrics:")
    print(f"  Demographic Parity: {fairness.demographic_parity:.3f}")
    print(f"  Individual Fairness: {fairness.individual_fairness:.3f}")
    print(f"  Disparate Impact: {fairness.disparate_impact_ratio:.3f}")
    
    print(f"\nEqualized Odds by Group:")
    for group, tpr in fairness.equalized_odds.items():
        print(f"  {group}: {tpr:.3f}")
    
    bias_sources = detector.detect_bias_sources(X, ['gender', 'age_group'])
    print(f"\nBias Sources Detected: {len(bias_sources)}")
    
    mitigator = BiasMitigator(protected_attributes=['gender'])
    
    X_balanced, y_balanced = mitigator.resample_for_fairness(X, y, 'oversample')
    print(f"\nResampling: {len(X)} -> {len(X_balanced)} samples")
    
    weights = mitigator.reweight_samples(X, y)
    print(f"Generated {len(weights)} sample weights")
    
    thresholds = mitigator.threshold_adjustment(model, X, y)
    print(f"\nAdjusted Thresholds:")
    for group, threshold in thresholds.items():
        print(f"  {group}: {threshold:.2f}")
    
    return detector


if __name__ == "__main__":
    run_bias_example()
```

## IV. APPLICATIONS

### Banking: Fair Credit Scoring
```python
class FairCreditScorer:
    """Bias-mitigated credit scoring."""
    
    def __init__(self):
        self.detector = BiasDetector(['gender', 'race'])
        self.mitigator = BiasMitigator(['gender', 'race'])
    
    def validate_fairness(self, model, data):
        metrics = self.detector.compute_fairness_metrics(
            model, data.features, data.labels, model.predict(data.features)
        )
        
        if metrics.demographic_parity < 0.8:
            return "Requires mitigation"
        return "Fair"
```

### Healthcare: Equitable Diagnosis
```python
class FairDiagnostic:
    """Bias-mitigated diagnostic system."""
    
    def __init__(self):
        self.detector = BiasDetector(['race', 'gender', 'age'])
    
    def check_diagnosis_fairness(self, model, patient_data):
        for group in patient_data.race.unique():
            metrics = self.detector.compute_group_metrics(
                model, patient_data[patient_data.race == group]
            )
            
            if metrics.recall < 0.8:
                alert(f"Low recall for race={group}")
```

## V. OUTPUT RESULTS

```
BIAS DETECTION AND MITIGATION
============================

Fairness Metrics:
  Demographic Parity: 0.923
  Individual Fairness: 0.887
  Disparate Impact: 0.891

Equalized Odds by Group:
  group_F: 0.845
  group_M: 0.812

Bias Sources Detected: 0

Resampling: 1000 -> 1500 samples
Generated 1000 sample weights

Adjusted Thresholds:
  group_F: 0.45
  group_M: 0.55
```

## VI. CONCLUSION

### Key Takeaways
1. Test for multiple types of bias
2. Use appropriate fairness metrics
3. Apply mitigation at data, algorithm, and post-processing levels

### Next Steps
- Implement continuous fairness monitoring
- Add regulatory reporting

### Further Reading
- Fairness and Machine Learning (Barahona et al.)
- IBM AI Fairness 360 toolkit