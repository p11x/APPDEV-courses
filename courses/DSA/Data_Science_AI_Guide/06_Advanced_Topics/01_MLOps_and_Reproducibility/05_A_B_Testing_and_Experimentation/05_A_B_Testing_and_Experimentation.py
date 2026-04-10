# Topic: A/B Testing and Experimentation
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for A/B Testing and Experimentation

I. INTRODUCTION
A/B testing is a critical methodology for data-driven decision making in machine
learning systems. This module covers statistical foundations, experimental design,
multi-armed bandits, and deployment strategies for production ML systems.

II. CORE CONCEPTS
- Statistical foundations of hypothesis testing
- Experimental design and randomization
- Sample size calculation and power analysis
- Multi-armed bandits and adaptive testing
- Causal inference in ML systems

III. IMPLEMENTATION
"""

import numpy as np
import pandas as pd
from scipy import stats
from typing import Dict, List, Tuple, Optional, Callable
from dataclasses import dataclass
from enum import Enum
import hashlib
import time
from abc import ABC, abstractmethod


class TestType(Enum):
    """Types of statistical tests."""
    T_TEST = "t_test"
    Z_TEST = "z_test"
    CHI_SQUARE = "chi_square"
    MANN_WHITNEY = "mann_whitney"
    WILCOXON = "wilcoxon"


class VariantType(Enum):
    """Experiment variant types."""
    CONTROL = "control"
    TREATMENT = "treatment"


@dataclass
class ExperimentConfig:
    """Configuration for A/B test experiment."""
    experiment_name: str
    control_variant: str
    treatment_variant: str
    metric_name: str
    significance_level: float = 0.05
    power: float = 0.80
    minimum_detectable_effect: float = 0.05
    two_sided: bool = True


@dataclass
class ExperimentResult:
    """Results from an A/B test experiment."""
    experiment_name: str
    control_mean: float
    treatment_mean: float
    control_std: float
    treatment_std: float
    control_size: int
    treatment_size: int
    test_statistic: float
    p_value: float
    confidence_interval: Tuple[float, float]
    significant: bool
    effect_size: float
    power_achieved: float


class StatisticalFoundation:
    """Statistical foundations for A/B testing."""

    @staticmethod
    def calculate_sample_size(
        baseline_conversion: float,
        minimum_detectable_effect: float,
        significance_level: float = 0.05,
        power: float = 0.80
    ) -> int:
        """
        Calculate required sample size for A/B test.
        
        Uses the formula for two-proportion z-test.
        """
        alpha = significance_level
        beta = 1 - power
        
        p1 = baseline_conversion
        p2 = baseline_conversion * (1 + minimum_detectable_effect)
        
        p_pooled = (p1 + p2) / 2
        
        z_alpha = stats.norm.ppf(1 - alpha / 2)
        z_beta = stats.norm.ppf(1 - beta)
        
        numerator = (z_alpha * np.sqrt(2 * p_pooled * (1 - p_pooled)) + 
                     z_beta * np.sqrt(p1 * (1 - p1) + p2 * (1 - p2))) ** 2
        denominator = (p2 - p1) ** 2
        
        return int(np.ceil(numerator / denominator))

    @staticmethod
    def calculate_power(
        effect_size: float,
        sample_size: int,
        significance_level: float = 0.05
    ) -> float:
        """Calculate statistical power given effect size and sample size."""
        z_alpha = stats.norm.ppf(1 - significance_level / 2)
        z_beta = effect_size * np.sqrt(sample_size) - z_alpha
        return stats.norm.cdf(z_beta)

    @staticmethod
    def cohens_d(group1: np.ndarray, group2: np.ndarray) -> float:
        """Calculate Cohen's d effect size."""
        n1, n2 = len(group1), len(group2)
        var1, var2 = np.var(group1, ddof=1), np.var(group2, ddof=1)
        
        pooled_std = np.sqrt(((n1 - 1) * var1 + (n2 - 1) * var2) / (n1 + n2 - 2))
        return (np.mean(group1) - np.mean(group2)) / pooled_std

    @staticmethod
    def confidence_interval(
        group1: np.ndarray,
        group2: np.ndarray,
        confidence: float = 0.95
    ) -> Tuple[float, float]:
        """Calculate confidence interval for difference in means."""
        mean1, mean2 = np.mean(group1), np.mean(group2)
        n1, n2 = len(group1), len(group2)
        
        se1 = np.std(group1, ddof=1) / np.sqrt(n1)
        se2 = np.std(group2, ddof=1) / np.sqrt(n2)
        
        se_diff = np.sqrt(se1**2 + se2**2)
        
        t_value = stats.t.ppf((1 + confidence) / 2, n1 + n2 - 2)
        
        diff = mean1 - mean2
        margin = t_value * se_diff
        
        return (diff - margin, diff + margin)


class Randomization:
    """Randomization strategies for experiments."""

    @staticmethod
    def hash_based_assignment(
        user_id: str,
        experiment_id: str,
        num_variants: int = 2
    ) -> int:
        """Assign user to variant using consistent hashing."""
        combined = f"{user_id}:{experiment_id}"
        hash_value = int(hashlib.md5(combined.encode()).hexdigest(), 16)
        return hash_value % num_variants

    @staticmethod
    def stratified_sampling(
        data: pd.DataFrame,
        strata_columns: List[str],
        treatment_ratio: float = 0.5
    ) -> pd.DataFrame:
        """Perform stratified sampling for balanced groups."""
        np.random.seed(42)
        
        for col in strata_columns:
            groups = data.groupby(col)
            
            treatment_indices = []
            control_indices = []
            
            for name, group in groups:
                indices = group.index.tolist()
                np.random.shuffle(indices)
                
                split = int(len(indices) * treatment_ratio)
                treatment_indices.extend(indices[:split])
                control_indices.extend(indices[split:])
            
            data = data.copy()
            data.loc[treatment_indices, 'variant'] = 'treatment'
            data.loc[control_indices, 'variant'] = 'control'
        
        return data

    @staticmethod
    def bucket_assignment(
        user_id: str,
        num_buckets: int = 100
    ) -> int:
        """Assign user to a bucket for experimentation."""
        hash_value = int(hashlib.sha256(user_id.encode()).hexdigest(), 16)
        return hash_value % num_buckets


class ABTestRunner:
    """Main A/B test execution class."""

    def __init__(self, config: ExperimentConfig):
        self.config = config
        self.stat_foundation = StatisticalFoundation()
        self.control_data: List[float] = []
        self.treatment_data: List[float] = []

    def add_observation(
        self,
        variant: str,
        value: float,
        user_id: Optional[str] = None
    ) -> None:
        """Add an observation to the experiment."""
        if variant == self.config.control_variant:
            self.control_data.append(value)
        elif variant == self.config.treatment_variant:
            self.treatment_data.append(value)

    def run_ttest(
        self,
        two_sided: bool = True
    ) -> ExperimentResult:
        """Run independent samples t-test."""
        control = np.array(self.control_data)
        treatment = np.array(self.treatment_data)
        
        t_stat, p_value = stats.ttest_ind(control, treatment, equal_var=False)
        
        effect_size = self.stat_foundation.cohens_d(control, treatment)
        
        ci = self.stat_foundation.confidence_interval(control, treatment)
        
        n_control = len(control)
        n_treatment = len(treatment)
        
        power = self.stat_foundation.calculate_power(
            abs(effect_size),
            min(n_control, n_treatment),
            self.config.significance_level
        )
        
        return ExperimentResult(
            experiment_name=self.config.experiment_name,
            control_mean=np.mean(control),
            treatment_mean=np.mean(treatment),
            control_std=np.std(control, ddof=1),
            treatment_std=np.std(treatment, ddof=1),
            control_size=n_control,
            treatment_size=n_treatment,
            test_statistic=t_stat,
            p_value=p_value,
            confidence_interval=ci,
            significant=p_value < self.config.significance_level,
            effect_size=effect_size,
            power_achieved=power
        )

    def run_ztest(self) -> ExperimentResult:
        """Run two-proportion z-test."""
        control = np.array(self.control_data)
        treatment = np.array(self.treatment_data)
        
        p1 = np.mean(control)
        p2 = np.mean(treatment)
        
        n1, n2 = len(control), len(treatment)
        
        p_pooled = (p1 * n1 + p2 * n2) / (n1 + n2)
        
        se = np.sqrt(p_pooled * (1 - p_pooled) * (1/n1 + 1/n2))
        
        z_stat = (p2 - p1) / se
        p_value = 2 * (1 - stats.norm.cdf(abs(z_stat)))
        
        effect_size = (p2 - p1) / np.sqrt(p_pooled * (1 - p_pooled))
        
        ci = ((p2 - p1) - 1.96 * se, (p2 - p1) + 1.96 * se)
        
        power = self.stat_foundation.calculate_power(
            abs(effect_size),
            min(n1, n2),
            self.config.significance_level
        )
        
        return ExperimentResult(
            experiment_name=self.config.experiment_name,
            control_mean=p1,
            treatment_mean=p2,
            control_std=np.std(control, ddof=1),
            treatment_std=np.std(treatment, ddof=1),
            control_size=n1,
            treatment_size=n2,
            test_statistic=z_stat,
            p_value=p_value,
            confidence_interval=ci,
            significant=p_value < self.config.significance_level,
            effect_size=effect_size,
            power_achieved=power
        )


class MultiArmedBandit:
    """Multi-armed bandit implementation for adaptive experimentation."""

    def __init__(
        self,
        n_arms: int,
        epsilon: float = 0.1,
        algorithm: str = "epsilon_greedy"
    ):
        self.n_arms = n_arms
        self.epsilon = epsilon
        self.algorithm = algorithm
        self.counts = [0] * n_arms
        self.values = [0.0] * n_arms
        self.total_count = 0

    def select_arm(self) -> int:
        """Select an arm based on the chosen algorithm."""
        if self.algorithm == "epsilon_greedy":
            return self._epsilon_greedy()
        elif self.algorithm == "ucb":
            return self._ucb()
        elif self.algorithm == "thompson":
            return self._thompson_sampling()
        else:
            return self._epsilon_greedy()

    def _epsilon_greedy(self) -> int:
        """Epsilon-greedy arm selection."""
        if np.random.random() < self.epsilon:
            return np.random.randint(self.n_arms)
        
        max_value = max(self.values)
        best_arms = [i for i, v in enumerate(self.values) if v == max_value]
        return np.random.choice(best_arms)

    def _ucb(self) -> int:
        """Upper Confidence Bound arm selection."""
        total = sum(self.counts)
        if total == 0:
            return np.random.randint(self.n_arms)
        
        ucb_values = []
        for i in range(self.n_arms):
            if self.counts[i] == 0:
                ucb_values.append(float('inf'))
            else:
                bonus = np.sqrt(2 * np.log(total) / self.counts[i])
                ucb_values.append(self.values[i] + bonus)
        
        return np.argmax(ucb_values)

    def _thompson_sampling(self) -> int:
        """Thompson sampling with Beta distribution."""
        samples = []
        for i in range(self.n_arms):
            alpha = self.counts[i] * self.values[i] + 1
            beta = self.counts[i] * (1 - self.values[i]) + 1
            samples.append(np.random.beta(alpha, beta))
        
        return np.argmax(samples)

    def update(self, arm: int, reward: float) -> None:
        """Update arm values based on observed reward."""
        self.counts[arm] += 1
        n = self.counts[arm]
        
        value = self.values[arm]
        self.values[arm] = ((n - 1) / n) * value + (1 / n) * reward
        self.total_count += 1

    def get_best_arm(self) -> int:
        """Get the currently estimated best arm."""
        return np.argmax(self.values)


class SequentialTesting:
    """Sequential testing for online experiments."""

    def __init__(
        self,
        significance_level: float = 0.05,
        power: float = 0.80,
        maximum_sample_size: int = 100000
    ):
        self.alpha = significance_level
        self.beta = 1 - power
        self.max_n = maximum_sample_size
        
        self.alpha_spending = self._get_ocb_alpha_spending()
        self.current_n = 0
        self.z_scores: List[float] = []

    def _get_ocb_alpha_spending(self) -> Callable:
        """O'Brien-Fleming alpha spending function."""
        def alpha_spending(t: float) -> float:
            if t <= 0 or t > 1:
                return 0
            return 2 - 2 * stats.norm.ppf(1 - self.alpha / 2 * t)
        return alpha_spending

    def add_observation(
        self,
        control_value: float,
        treatment_value: float
    ) -> Dict:
        """Add paired observations and calculate current statistics."""
        self.current_n += 1
        
        diff = treatment_value - control_value
        self.z_scores.append(diff)
        
        cumulative_mean = np.mean(self.z_scores)
        cumulative_std = np.std(self.z_scores, ddof=1) / np.sqrt(self.current_n)
        
        z_stat = cumulative_mean / cumulative_std if cumulative_std > 0 else 0
        
        t = self.current_n / self.max_n
        alpha_boundary = self.alpha_spending(t)
        
        critical_value = stats.norm.ppf(1 - alpha_boundary / 2)
        
        return {
            'n': self.current_n,
            'z_statistic': z_stat,
            'critical_value': critical_value,
            'stop_early': abs(z_stat) > critical_value,
            'continue': abs(z_stat) <= critical_value and self.current_n < self.max_n
        }


def banking_example():
    """A/B testing example for banking/fintech applications."""
    print("\n" + "="*60)
    print("BANKING SECTOR: Credit Card Offer Optimization")
    print("="*60)
    
    config = ExperimentConfig(
        experiment_name="Credit_Card_Offer_v2",
        control_variant="standard_offer",
        treatment_variant="premium_offer",
        metric_name="application_conversion_rate",
        significance_level=0.05,
        power=0.80,
        minimum_detectable_effect=0.10
    )
    
    sample_size = StatisticalFoundation.calculate_sample_size(
        baseline_conversion=0.05,
        minimum_detectable_effect=0.10,
        significance_level=0.05,
        power=0.80
    )
    print(f"\nRequired sample size per variant: {sample_size}")
    
    np.random.seed(42)
    control_conversion = 0.05
    treatment_conversion = 0.065
    
    control_results = np.random.binomial(1, control_conversion, sample_size)
    treatment_results = np.random.binomial(1, treatment_conversion, sample_size)
    
    test_runner = ABTestRunner(config)
    for val in control_results:
        test_runner.add_observation("standard_offer", val)
    for val in treatment_results:
        test_runner.add_observation("premium_offer", val)
    
    result = test_runner.run_ztest()
    
    print(f"\nExperiment Results:")
    print(f"  Control conversion rate: {result.control_mean:.4f} ({result.control_mean*100:.2f}%)")
    print(f"  Treatment conversion rate: {result.treatment_mean:.4f} ({result.treatment_mean*100:.2f}%)")
    print(f"  Sample size - Control: {result.control_size}")
    print(f"  Sample size - Treatment: {result.treatment_size}")
    print(f"  Test statistic: {result.test_statistic:.4f}")
    print(f"  P-value: {result.p_value:.6f}")
    print(f"  95% CI: ({result.confidence_interval[0]:.4f}, {result.confidence_interval[1]:.4f})")
    print(f"  Effect size: {result.effect_size:.4f}")
    print(f"  Power achieved: {result.power_achieved:.4f}")
    print(f"  Significant: {result.significant}")
    
    bandit = MultiArmedBandit(n_arms=3, epsilon=0.1, algorithm="thompson")
    arm_names = ["cashback", "travel", "balance_transfer"]
    
    print(f"\nMulti-Armed Bandit Test:")
    for i in range(500):
        arm = bandit.select_arm()
        reward = np.random.binomial(1, 0.1 + arm * 0.05)
        bandit.update(arm, reward)
    
    print(f"  Best offer type: {arm_names[bandit.get_best_arm()]}")
    print(f"  Arm selection counts: {bandit.counts}")


def healthcare_example():
    """A/B testing example for healthcare applications."""
    print("\n" + "="*60)
    print("HEALTHCARE SECTOR: Treatment Protocol Optimization")
    print("="*60)
    
    config = ExperimentConfig(
        experiment_name="Treatment_Protocol_v2",
        control_variant="standard_protocol",
        treatment_variant="new_protocol",
        metric_name="patient_outcome_score",
        significance_level=0.05,
        power=0.80,
        minimum_detectable_effect=0.15
    )
    
    np.random.seed(42)
    
    control_outcomes = np.random.normal(75, 10, 200)
    treatment_outcomes = np.random.normal(80, 10, 200)
    
    test_runner = ABTestRunner(config)
    for val in control_outcomes:
        test_runner.add_observation("standard_protocol", val)
    for val in treatment_outcomes:
        test_runner.add_observation("new_protocol", val)
    
    result = test_runner.run_ttest()
    
    print(f"\nExperiment Results:")
    print(f"  Control mean outcome: {result.control_mean:.2f} (SD: {result.control_std:.2f})")
    print(f"  Treatment mean outcome: {result.treatment_mean:.2f} (SD: {result.treatment_std:.2f})")
    print(f"  Sample size - Control: {result.control_size}")
    print(f"  Sample size - Treatment: {result.treatment_size}")
    print(f"  T-statistic: {result.test_statistic:.4f}")
    print(f"  P-value: {result.p_value:.6f}")
    print(f"  95% CI: ({result.confidence_interval[0]:.2f}, {result.confidence_interval[1]:.2f})")
    print(f"  Cohen's d: {result.effect_size:.4f}")
    print(f"  Power achieved: {result.power_achieved:.4f}")
    print(f"  Significant: {result.significant}")
    
    print(f"\nSequential Testing (simulated patient outcomes):")
    seq_test = SequentialTesting(significance_level=0.05, maximum_sample_size=100)
    
    for i in range(50):
        control_val = np.random.normal(75, 10)
        treatment_val = np.random.normal(80, 10)
        result_dict = seq_test.add_observation(control_val, treatment_val)
        
        if result_dict['stop_early']:
            print(f"  Stopped at n={result_dict['n']} with z={result_dict['z_statistic']:.4f}")
            break
    else:
        print(f"  Completed full run at n={seq_test.current_n}")


def core_implementation():
    """Core implementation demonstrating all components."""
    print("\n" + "="*60)
    print("CORE IMPLEMENTATION")
    print("="*60)
    
    print("\n1. Statistical Foundation:")
    sample_size = StatisticalFoundation.calculate_sample_size(
        baseline_conversion=0.10,
        minimum_detectable_effect=0.05,
        significance_level=0.05,
        power=0.80
    )
    print(f"   Sample size for 5% MDE: {sample_size}")
    
    print("\n2. Randomization:")
    user_id = "user_12345"
    experiment_id = "exp_001"
    variant = Randomization.hash_based_assignment(user_id, experiment_id, 2)
    print(f"   User {user_id} assigned to variant {variant}")
    
    print("\n3. A/B Test Runner:")
    config = ExperimentConfig(
        experiment_name="test_exp",
        control_variant="A",
        treatment_variant="B",
        metric_name="conversion"
    )
    runner = ABTestRunner(config)
    runner.add_observation("A", 0.1)
    runner.add_observation("B", 0.15)
    print("   A/B test runner initialized and observations added")
    
    print("\n4. Multi-Armed Bandit:")
    bandit = MultiArmedBandit(n_arms=4, epsilon=0.2, algorithm="epsilon_greedy")
    for _ in range(100):
        arm = bandit.select_arm()
        reward = np.random.random()
        bandit.update(arm, reward)
    print(f"   Bandit completed with arm counts: {bandit.counts}")


def main():
    print("="*60)
    print("A/B TESTING AND EXPERIMENTATION")
    print("="*60)
    
    core_implementation()
    banking_example()
    healthcare_example()
    
    print("\n" + "="*60)
    print("IMPLEMENTATION COMPLETE")
    print("="*60)


if __name__ == "__main__":
    main()
