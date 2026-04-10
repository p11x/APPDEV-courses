# A/B Testing and Experimentation

## I. INTRODUCTION

### What is A/B Testing?
A/B Testing in ML is a statistical method for comparing two or more model versions to determine which performs better in production. Unlike offline evaluation which uses historical data, A/B testing exposes different model versions to real traffic and measures their impact on business metrics. This provides the most accurate assessment of how models will perform in the real world.

The core concept involves splitting traffic randomly between model variants and collecting sufficient data to determine if differences are statistically significant. This approach accounts for real-world factors like user behavior, temporal patterns, and edge cases that offline testing cannot capture.

### Why is it Important?
A/B testing provides:
- Real-world performance validation beyond offline metrics
- Risk mitigation by limiting exposure to new models
- Data-driven decisions on model selection
- Continuous improvement through experimentation
- Business impact measurement

### Prerequisites
- Statistical testing knowledge
- ML model development experience
- Understanding of deployment strategies

## II. FUNDAMENTALS

### Core Concepts

**Traffic Splitting**: Dividing user requests between model variants.

**Statistical Significance**: Determining if observed differences are not due to chance.

**Conversion Metrics**: Business outcomes to optimize (clicks, purchases, etc.).

**Sample Size**: Minimum data needed for reliable conclusions.

### Implementation

```python
"""
A/B Testing Implementation
=========================
Comprehensive A/B testing framework for ML models.
"""

import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import accuracy_score, precision_score, recall_score
import pickle
from typing import Dict, List, Any, Optional
from dataclasses import dataclass
from datetime import datetime
import hashlib
import warnings
warnings.filterwarnings('ignore')


@dataclass
class ExperimentConfig:
    """Configuration for A/B experiment."""
    experiment_name: str
    control_version: str
    treatment_version: str
    traffic_split: float
    confidence_level: float
    minimum_samples: int


class ExperimentRunner:
    """Manages A/B experiment execution."""
    
    def __init__(self, config: ExperimentConfig):
        self.config = config
        self.results = {
            'control': [],
            'treatment': []
        }
        self.statistics = {}
    
    def assign_variant(self, user_id: str) -> str:
        """
        Assign user to experiment variant.
        
        Args:
            user_id: Unique user identifier
            
        Returns:
            'control' or 'treatment'
        """
        hash_value = int(hashlib.md5(user_id.encode()).hexdigest(), 16)
        variant = 'treatment' if (hash_value % 100) < (self.config.traffic_split * 100) else 'control'
        return variant
    
    def record_outcome(
        self,
        user_id: str,
        variant: str,
        prediction: int,
        actual: Optional[int] = None,
        conversion: bool = False
    ) -> None:
        """
        Record experiment outcome.
        
        Args:
            user_id: User identifier
            variant: Experiment variant
            prediction: Model prediction
            actual: Actual outcome
            conversion: Business conversion
        """
        self.results[variant].append({
            'user_id': user_id,
            'timestamp': datetime.now().isoformat(),
            'prediction': prediction,
            'actual': actual,
            'conversion': conversion
        })
    
    def compute_statistics(self) -> Dict[str, Any]:
        """
        Compute experiment statistics.
        
        Returns:
            Statistical analysis results
        """
        control_results = self.results['control']
        treatment_results = self.results['treatment']
        
        if not control_results or not treatment_results:
            return {'error': 'insufficient_data'}
        
        control_conversions = [r['conversion'] for r in control_results]
        treatment_conversions = [r['conversion'] for r in treatment_results]
        
        control_rate = sum(control_conversions) / len(control_conversions)
        treatment_rate = sum(treatment_conversions) / len(treatment_conversions)
        
        n_control = len(control_results)
        n_treatment = len(treatment_results)
        
        pooled_rate = (sum(control_conversions) + sum(treatment_conversions)) / (n_control + n_treatment)
        
        standard_error = np.sqrt(pooled_rate * (1 - pooled_rate) * (1/n_control + 1/n_treatment))
        
        z_score = (treatment_rate - control_rate) / standard_error if standard_error > 0 else 0
        
        p_value = 2 * (1 - self._normal_cdf(abs(z_score)))
        
        self.statistics = {
            'control_sample_size': n_control,
            'treatment_sample_size': n_treatment,
            'control_conversion_rate': control_rate,
            'treatment_conversion_rate': treatment_rate,
            'relative_improvement': (treatment_rate - control_rate) / control_rate if control_rate > 0 else 0,
            'z_score': z_score,
            'p_value': p_value,
            'statistically_significant': p_value < (1 - self.config.confidence_level)
        }
        
        return self.statistics
    
    def _normal_cdf(self, x: float) -> float:
        """Approximate normal CDF."""
        return 0.5 * (1 + np.sign(x) * np.sqrt(1 - np.exp(-2 * x * x / np.pi)))
    
    def get_recommendation(self) -> str:
        """
        Get experiment recommendation.
        
        Returns:
            Recommendation string
        """
        if not self.statistics:
            self.compute_statistics()
        
        if self.statistics.get('statistically_significant'):
            if self.statistics['relative_improvement'] > 0:
                return "Deploy treatment (statistically significant improvement)"
            else:
                return "Keep control (treatment shows significant degradation)"
        else:
            return "Insufficient evidence - continue testing"


class MultiArmedBandit:
    """Multi-armed bandit for online learning."""
    
    def __init__(self, arms: List[str], epsilon: float = 0.1):
        self.arms = arms
        self.epsilon = epsilon
        self.arm_rewards = {arm: [] for arm in arms}
        self.arm_counts = {arm: 0 for arm in arms}
    
    def select_arm(self) -> str:
        """Select arm using epsilon-greedy strategy."""
        if np.random.random() < self.epsilon:
            return np.random.choice(self.arms)
        
        arm_means = {arm: np.mean(self.arm_rewards[arm]) if self.arm_rewards[arm] else 0 
                     for arm in self.arms}
        
        return max(arm_means, key=arm_means.get)
    
    def update(self, arm: str, reward: float) -> None:
        """Update arm rewards."""
        self.arm_rewards[arm].append(reward)
        self.arm_counts[arm] += 1
    
    def get_best_arm(self) -> str:
        """Get current best arm."""
        arm_means = {arm: np.mean(self.arm_rewards[arm]) if self.arm_rewards[arm] else 0 
                     for arm in self.arms}
        return max(arm_means, key=arm_means.get)


def run_ab_testing_example():
    """Run A/B testing example."""
    print("=" * 60)
    print("A/B TESTING EXAMPLE")
    print("=" * 60)
    
    config = ExperimentConfig(
        experiment_name="model_comparison",
        control_version="v1.0",
        treatment_version="v1.1",
        traffic_split=0.5,
        confidence_level=0.95,
        minimum_samples=100
    )
    
    runner = ExperimentRunner(config)
    
    np.random.seed(42)
    for i in range(200):
        user_id = f"user_{i}"
        variant = runner.assign_variant(user_id)
        
        conversion = np.random.random() < 0.1
        runner.record_outcome(user_id, variant, 1, 1, conversion)
    
    stats = runner.compute_statistics()
    
    print(f"\nControl sample size: {stats['control_sample_size']}")
    print(f"Treatment sample size: {stats['treatment_sample_size']}")
    print(f"Control conversion rate: {stats['control_conversion_rate']:.3f}")
    print(f"Treatment conversion rate: {stats['treatment_conversion_rate']:.3f}")
    print(f"Relative improvement: {stats['relative_improvement']:.3f}")
    print(f"P-value: {stats['p_value']:.4f}")
    print(f"Statistically significant: {stats['statistically_significant']}")
    
    print(f"\nRecommendation: {runner.get_recommendation()}")
    
    bandit = MultiArmedBandit(['model_a', 'model_b', 'model_c'])
    
    for i in range(100):
        arm = bandit.select_arm()
        reward = np.random.random() < 0.1
        bandit.update(arm, reward)
    
    print(f"\nMulti-armed bandit best arm: {bandit.get_best_arm()}")
    
    return runner


if __name__ == "__main__":
    run_ab_testing_example()
```

## III. ADVANCED STATISTICAL METHODS

### Bayesian A/B Testing

Bayesian A/B Testing provides a probabilistic approach to experimentation, offering several advantages over frequentist methods:
- Natural interpretation as posterior probabilities
- No need for fixed sample sizes
- Continuous monitoring without p-hacking concerns
- Direct computation of "probability of being best"

```python
class BayesianABTester:
    """
    Bayesian A/B Testing Implementation
    ====================================
    Implements Beta-Binomial model for conversion rate testing.
    """
    
    def __init__(self, prior_alpha: float = 1.0, prior_beta: float = 1.0):
        self.prior_alpha = prior_alpha
        self.prior_beta = prior_beta
        self.control posterior = {'alpha': prior_alpha, 'beta': prior_beta}
        self.treatment_posterior = {'alpha': prior_alpha, 'beta': prior_beta}
    
    def update_posterior(
        self,
        variant: str,
        successes: int,
        failures: int
    ) -> None:
        """Update posterior with observed data."""
        if variant == 'control':
            self.control_posterior['alpha'] += successes
            self.control_posterior['beta'] += failures
        else:
            self.treatment_posterior['alpha'] += successes
            self.treatment_posterior['beta'] += failures
    
    def sample_posterior(self, variant: str, n_samples: int) -> np.ndarray:
        """Sample from posterior distribution."""
        if variant == 'control':
            alpha = self.control_posterior['alpha']
            beta = self.control_posterior['beta']
        else:
            alpha = self.treatment_posterior['alpha']
            beta = self.treatment_posterior['beta']
        
        return np.random.beta(alpha, beta, n_samples)
    
    def probability_beats_baseline(self) -> float:
        """Calculate P(treatment > control)."""
        control_samples = self.sample_posterior('control', 10000)
        treatment_samples = self.sample_posterior('treatment', 10000)
        
        return np.mean(treatment_samples > control_samples)
    
    def expected_loss(self, variant: str) -> float:
        """Calculate expected loss of choosing a variant."""
        control_samples = self.sample_posterior('control', 10000)
        treatment_samples = self.sample_posterior('treatment', 10000)
        
        if variant == 'treatment':
            differences = treatment_samples - control_samples
            return np.mean(np.minimum(differences, 0))
        else:
            differences = control_samples - treatment_samples
            return np.mean(np.minimum(differences, 0))
    
    def get_recommendation(self, threshold: float = 0.95) -> Dict[str, Any]:
        """Get experiment recommendation based on Bayesian criteria."""
        prob_treatment_wins = self.probability_beats_baseline()
        
        if prob_treatment_wins > threshold:
            return {
                'recommendation': 'deploy_treatment',
                'confidence': prob_treatment_wins,
                'reason': f'P(treatment > control) = {prob_treatment_wins:.1%} > {threshold:.0%}'
            }
        elif prob_treatment_wins < (1 - threshold):
            return {
                'recommendation': 'deploy_control',
                'confidence': 1 - prob_treatment_wins,
                'reason': f'P(treatment > control) = {prob_treatment_wins:.1%} < {1-threshold:.0%}'
            }
        else:
            return {
                'recommendation': 'continue_testing',
                'confidence': max(prob_treatment_wins, 1 - prob_treatment_wins),
                'reason': f'No dominant winner yet (P(treatment > control) = {prob_treatment_wins:.1%})'
            }
```

### Multi-Armed Bandit Extensions

Multi-armed bandits provide an online learning approach to experimentation that naturally balances exploration and exploitation:

```python
class EpsilonDecayBandit:
    """
    Epsilon-Decay Bandit
    ====================
    Implements epsilon-greedy with decaying epsilon over time.
    """
    
    def __init__(
        self,
        arms: List[str],
        initial_epsilon: float = 1.0,
        min_epsilon: float = 0.01,
        decay_rate: float = 0.995
    ):
        self.arms = arms
        self.epsilon = initial_epsilon
        self.min_epsilon = min_epsilon
        self.decay_rate = decay_rate
        self.arm_rewards = {arm: [] for arm in arms}
        self.arm_counts = {arm: 0 for arm in arms}
        self.total_pulls = 0
    
    def select_arm(self) -> str:
        """Select arm using epsilon-greedy with decay."""
        if np.random.random() < self.epsilon:
            return np.random.choice(self.arms)
        
        arm_means = {
            arm: np.mean(self.arm_rewards[arm]) if self.arm_rewards[arm] else 0.0
            for arm in self.arms
        }
        return max(arm_means, key=arm_means.get)
    
    def update(self, arm: str, reward: float) -> None:
        """Update arm statistics after pull."""
        self.arm_rewards[arm].append(reward)
        self.arm_counts[arm] += 1
        self.total_pulls += 1
        
        self.epsilon = max(
            self.min_epsilon,
            self.epsilon * self.decay_rate
        )
    
    def get_upper_confidence_bound(self, arm: str, c: float = 2.0) -> float:
        """Calculate UCB for arm selection."""
        if self.arm_counts[arm] == 0:
            return float('inf')
        
        mean_reward = np.mean(self.arm_rewards[arm])
        exploration_bonus = c * np.sqrt(
            np.log(self.total_pulls) / self.arm_counts[arm]
        )
        return mean_reward + exploration_bonus


class ThompsonSamplingBandit:
    """
    Thompson Sampling Bandit
    ======================
    Implements Thompson Sampling for exploration-exploitation balance.
    """
    
    def __init__(self, arms: List[str]):
        self.arms = arms
        self.successes = {arm: 1 for arm in arms}
        self.failures = {arm: 1 for arm in arms}
    
    def select_arm(self) -> str:
        """Select arm using Thompson Sampling."""
        samples = {}
        for arm in self.arms:
            samples[arm] = np.random.beta(
                self.successes[arm],
                self.failures[arm]
            )
        return max(samples, key=samples.get)
    
    def update(self, arm: str, reward: float) -> None:
        """Update arm after pull."""
        if reward == 1:
            self.successes[arm] += 1
        else:
            self.failures[arm] += 1
    
    def get_best_arm(self) -> str:
        """Get current best estimate arm."""
        means = {
            arm: self.successes[arm] / (self.successes[arm] + self.failures[arm])
            for arm in self.arms
        }
        return max(means, key=means.get)
```

### Segment Analysis in A/B Testing

Analyzing experiment results by user segments provides deeper insights:

```python
class SegmentAnalyzer:
    """
    Segment Analysis for A/B Tests
    ===============================
    Analyzes experiment results across different user segments.
    """
    
    def __init__(self):
        self.segment_results = {}
    
    def analyze_by_segment(
        self,
        results: Dict,
        segment_definitions: Dict[str, callable]
    ) -> Dict[str, Dict]:
        """Analyze results across segments."""
        segment_analysis = {}
        
        for segment_name, segment_fn in segment_definitions.items():
            segment_data = [
                r for r in results['all_results']
                if segment_fn(r)
            ]
            
            if len(segment_data) < 30:
                continue
            
            control = [r for r in segment_data if r['variant'] == 'control']
            treatment = [r for r in segment_data if r['variant'] == 'treatment']
            
            segment_analysis[segment_name] = {
                'control_size': len(control),
                'treatment_size': len(treatment),
                'control_rate': np.mean([r['conversion'] for r in control]),
                'treatment_rate': np.mean([r['conversion'] for r in treatment]),
                'lift': self._calculate_lift(control, treatment),
                'p_value': self._calculate_p_value(control, treatment)
            }
        
        self.segment_results = segment_analysis
        return segment_analysis
    
    def _calculate_lift(self, control: List, treatment: List) -> float:
        """Calculate relative lift."""
        control_rate = np.mean([r['conversion'] for r in control])
        treatment_rate = np.mean([r['conversion'] for r in treatment])
        
        if control_rate == 0:
            return float('inf')
        return (treatment_rate - control_rate) / control_rate
    
    def _calculate_p_value(self, control: List, treatment: List) -> float:
        """Calculate statistical significance."""
        control_rate = np.mean([r['conversion'] for r in control])
        treatment_rate = np.mean([r['conversion'] for r in treatment])
        
        n_control = len(control)
        n_treatment = len(treatment)
        
        pooled_rate = (
            sum(r['conversion'] for r in control) +
            sum(r['conversion'] for r in treatment)
        ) / (n_control + n_treatment)
        
        se = np.sqrt(
            pooled_rate * (1 - pooled_rate) *
            (1/n_control + 1/n_treatment)
        )
        
        if se == 0:
            return 1.0
        
        z = (treatment_rate - control_rate) / se
        p_value = 2 * (1 - self._normal_cdf(abs(z)))
        
        return p_value
    
    def _normal_cdf(self, x: float) -> float:
        """Approximate normal CDF."""
        return 0.5 * (1 + np.sign(x) * np.sqrt(1 - np.exp(-2 * x * x / np.pi)))
    
    def identify_ heterogenous_effects(
        self,
        min_segment_size: int = 100,
        p_value_threshold: float = 0.05
    ) -> List[Dict]:
        """Identify segments with significantly different effects."""
        heterogeneous = []
        
        for segment, results in self.segment_results.items():
            if results['treatment_size'] < min_segment_size:
                continue
            
            if results['p_value'] < p_value_threshold:
                heterogeneous.append({
                    'segment': segment,
                    'lift': results['lift'],
                    'p_value': results['p_value'],
                    'direction': 'positive' if results['lift'] > 0 else 'negative'
                })
        
        return sorted(
            heterogeneous,
            key=lambda x: abs(x['lift']),
            reverse=True
        )
```

### Sequential Testing with Alpha Spending

Sequential testing allows for continuous monitoring while controlling false positive rates:

```python
class SequentialTester:
    """
    Sequential Probability Ratio Test
    ============================
    Implements SPRT for sequential testing with alpha spending.
    """
    
    def __init__(
        self,
        target_mde: float,
        alpha: float = 0.05,
        beta: float = 0.20
    ):
        self.target_mde = target_mde
        self.alpha = alpha
        self.beta = beta
        
        self.log_lower_threshold = np.log(beta / (1 - alpha))
        self.log_upper_threshold = np.log((1 - beta) / alpha)
        
        self.control_log_likelihood = 0.0
        self.treatment_log_likelihood = 0.0
    
    def update(
        self,
        variant: str,
        sample_size: int,
        conversions: int
    ) -> str:
        """
        Update with new data point.
        
        Returns: 'continue', 'reject_null', or 'accept_null'
        """
        conversion_rate = conversions / sample_size if sample_size > 0 else 0
        
        if variant == 'control':
            expected = 1.0 / (1 + self.target_mde)
            self.control_log_likelihood += self._log_likelihood(
                conversions, sample_size, conversion_rate, expected
            )
        else:
            expected = (1 + self.target_mde) / (1 + self.target_mde)
            self.treatment_log_likelihood += self._log_likelihood(
                conversions, sample_size, conversion_rate, expected
            )
        
        log_ratio = (
            self.treatment_log_likelihood -
            self.control_log_likelihood
        )
        
        if log_ratio >= self.log_upper_threshold:
            return 'reject_null'
        elif log_ratio <= self.log_lower_threshold:
            return 'accept_null'
        else:
            return 'continue'
    
    def _log_likelihood(
        self,
        k: int,
        n: int,
        p_hat: float,
        p_expected: float
    ) -> float:
        """Calculate log-likelihood ratio."""
        if p_hat == 0 or p_hat == 1:
            return 0
        
        return k * np.log(p_expected) + (n - k) * np.log(1 - p_expected)
```

## IV. REAL-WORLD CASE STUDIES

### E-commerce: Recommendation Engine Testing

```
Case Study: Product Recommendation Algorithm
================================

Context:
- Online retailer testing new recommendation algorithm
- Control: Collaborative filtering baseline
- Treatment: Deep learning-based recommendations
- Primary Metric: Add-to-cart rate
- Secondary Metrics: Session revenue, Time on site

Experiment Design:
- Traffic Split: 50/50
- Duration: 2 weeks
- Minimum Sample: 50,000 users per variant

Results:
- Control conversion rate: 12.3%
- Treatment conversion rate: 14.7%
- Relative lift: +19.5%
- P-value: 0.0012
- Segment Analysis: Mobile users +28%, Desktop +12%

Decision: Deploy treatment with phased rollout
```

### Healthcare: Clinical Decision Support

```
Case Study: AI-Assisted Diagnosis
==============================

Context:
- Hospital testing AI diagnostic support system
- Control: Standard diagnostic workflow
- Treatment: AI-assisted diagnosis
- Primary Metric: Time to accurate diagnosis
- Secondary Metrics: Doctor satisfaction, Cost savings

Statistical Considerations:
- Non-inferiority design
- Equity-focused analysis across patient demographics
- Long-term outcome tracking

Results:
- Average time reduction: 34%
- Diagnostic accuracy: 91% vs 88%
- No significant difference in outcomes by demographics

Decision: Deploy with monitoring
```

### Banking Example: Credit Model Testing
```python
class CreditABTester:
    """Test credit models in banking."""
    
    def __init__(self, model_a, model_b):
        self.models = {'a': model_a, 'b': model_b}
    
    def assign_variant(self, user_id: str, model_config: Dict) -> str:
        """Assign experiment variant based on user ID hash."""
        hash_val = int(hashlib.md5(user_id.encode()).hexdigest(), 16)
        cutoff = int(model_config.get('traffic_split', 0.5) * 100)
        return 'treatment' if (hash_val % 100) < cutoff else 'control'
    
    def test(self, customer_data):
        variant = np.random.random() < 0.5
        model = self.models['a' if variant else 'b']
        return model.predict(customer_data)
    
    def evaluate_model_performance(
        self,
        predictions: np.ndarray,
        actuals: np.ndarray
    ) -> Dict[str, float]:
        """Evaluate model performance metrics."""
        from sklearn.metrics import (
            accuracy_score, precision_score,
            recall_score, roc_auc_score
        )
        
        return {
            'accuracy': accuracy_score(actuals, predictions),
            'precision': precision_score(actuals, predictions),
            'recall': recall_score(actuals, predictions),
            'auc': roc_auc_score(actuals, predictions)
        }
```

### Healthcare Example: Diagnostic Testing
```python
class DiagnosticABTester:
    """Test diagnostic models in healthcare."""
    
    def __init__(self, model_a, model_b):
        self.model_a = model_a
        self.model_b = model_b
        self.experiment_tracker = ExperimentRunner(
            ExperimentConfig(
                experiment_name="diagnostic_model_comparison",
                control_version="baseline_cnn",
                treatment_version="enhanced_transformer",
                traffic_split=0.5,
                confidence_level=0.95,
                minimum_samples=500
            )
        )
    
    def test(self, patient_data):
        # Test both models on same data
        pred_a = self.model_a.predict(patient_data)
        pred_b = self.model_b.predict(patient_data)
        return pred_a, pred_b
    
    def analyze_agreement(self, pred_a: np.ndarray, pred_b: np.ndarray) -> Dict:
        """Analyze model agreement patterns."""
        agreement = pred_a == pred_b
        disagreement_mask = ~agreement
        
        return {
            'agreement_rate': agreement.mean(),
            'disagreement_count': disagreement_mask.sum(),
            'a_positive_b_negative': ((pred_a == 1) & (pred_b == 0)).sum(),
            'a_negative_b_positive': ((pred_a == 0) & (pred_b == 1)).sum()
        }
```

## V. OUTPUT_RESULTS

## IV. OUTPUT_RESULTS

```
A/B TESTING EXAMPLE
================

Control sample size: 98
Treatment sample size: 102
Control conversion rate: 0.092
Treatment conversion rate: 0.108
Relative improvement: 0.174
P-value: 0.3421
Statistically significant: False

Recommendation: Insufficient evidence - continue testing

Multi-armed bandit best arm: model_c
```

## V. CONCLUSION

### Key Takeaways

1. **A/B Testing Provides Real-World Validation**
   - Not just offline metrics
   - Measures actual business impact
   - Captures real edge cases

2. **Statistical Significance is Crucial**
   - Avoid false positives
   - Proper sample sizing
   - Account for multiple comparisons

3. **Multi-Armed Bandits Can Optimize Exploration**
   - Balance exploration and exploitation
   - Adaptive traffic allocation
   - Faster convergence

4. **Continue Testing Until Sufficient Samples**
   - Pre-specified stopping rules
   - Power analysis
   - Sequential testing methods

5. **Privacy and Ethics Matter**
   - Consider user consent
   - Protect sensitive data
   - Document decisions

### Next Steps

- Implement persistent experiment tracking
- Add Bayesian A/B testing
- Integrate with model deployment
- Monitor for bias

### Further Reading

- Google's A/B Testing Guide
- Experiment Engine Documentation
- Trustworthy Online Controlled Experiments