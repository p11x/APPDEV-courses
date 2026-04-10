# Topic: Bias Detection and Mitigation
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Bias Detection and Mitigation

I. INTRODUCTION
Bias in machine learning systems can lead to unfair and discriminatory outcomes.
This module covers techniques for detecting various types of bias in datasets
and models, along with strategies for mitigating these biases.

II. CORE CONCEPTS
- Types of bias in ML (selection, confirmation, measurement)
- Bias detection metrics and analysis
- Pre-processing mitigation techniques
- In-processing fairness constraints
- Post-processing calibration methods

III. IMPLEMENTATION
"""

import numpy as np
import pandas as pd
from typing import Dict, List, Tuple, Optional, Any
from dataclasses import dataclass
from enum import Enum
from collections import Counter
import hashlib


class BiasType(Enum):
    """Types of bias in machine learning."""
    SELECTION = "selection"
    CONFIRMATION = "confirmation"
    MEASUREMENT = "measurement"
    PROXY = "proxy"
    AGGREGATION = "aggregation"
    HISTORICAL = "historical"


class ProtectedAttribute(Enum):
    """Protected attributes for fairness analysis."""
    GENDER = "gender"
    RACE = "race"
    AGE = "age"
    RELIGION = "religion"
    DISABILITY = "disability"
    NATIONAL_ORIGIN = "national_origin"
    SEXUAL_ORIENTATION = "sexual_orientation"


@dataclass
class BiasReport:
    """Comprehensive bias detection report."""
    bias_type: BiasType
    protected_attribute: str
    metric_name: str
    value: float
    threshold: float
    severity: str
    description: str
    recommendations: List[str]


class StatisticalParityCalculator:
    """Calculate statistical parity metrics."""

    @staticmethod
    def calculate_statistical_parity(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str
    ) -> Dict[str, float]:
        """
        Calculate statistical parity (demographic parity).
        
        Measures whether the positive outcome rate differs across groups.
        """
        groups = data[protected_col].unique()
        parity_scores = {}
        
        for group in groups:
            group_data = data[data[protected_col] == group]
            positive_rate = group_data[outcome_col].mean()
            parity_scores[f"group_{group}"] = positive_rate
        
        overall_positive_rate = data[outcome_col].mean()
        parity_scores['overall'] = overall_positive_rate
        
        return parity_scores

    @staticmethod
    def calculate_disparate_impact(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str,
        reference_group: str = None
    ) -> float:
        """
        Calculate disparate impact ratio.
        
        Ratio of positive outcome rates between protected and reference groups.
        """
        groups = data[protected_col].unique()
        
        if reference_group is None:
            reference_group = max(groups, key=lambda g: 
                len(data[data[protected_col] == g]))
        
        reference_rate = data[data[protected_col] == reference_group][outcome_col].mean()
        
        if reference_rate == 0:
            return 0.0
        
        disparate_impacts = {}
        for group in groups:
            if group != reference_group:
                group_rate = data[data[protected_col] == group][outcome_col].mean()
                disparate_impacts[group] = group_rate / reference_rate
        
        return min(disparate_impacts.values()) if disparate_impacts else 1.0


class EqualizedOddsCalculator:
    """Calculate equalized odds metrics."""

    @staticmethod
    def calculate_equalized_odds(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str,
        predicted_col: str
    ) -> Dict[str, Dict[str, float]]:
        """
        Calculate equalized odds.
        
        Measures TPR and FPR equality across groups.
        """
        groups = data[protected_col].unique()
        results = {}
        
        for group in groups:
            group_data = data[data[protected_col] == group]
            
            true_positives = ((group_data[predicted_col] == 1) & 
                            (group_data[outcome_col] == 1)).sum()
            false_positives = ((group_data[predicted_col] == 1) & 
                             (group_data[outcome_col] == 0)).sum()
            true_negatives = ((group_data[predicted_col] == 0) & 
                            (group_data[outcome_col] == 0)).sum()
            false_negatives = ((group_data[predicted_col] == 0) & 
                             (group_data[outcome_col] == 1)).sum()
            
            tpr = true_positives / (true_positives + false_negatives) if (true_positives + false_negatives) > 0 else 0
            fpr = false_positives / (false_positives + true_negatives) if (false_positives + true_negatives) > 0 else 0
            
            results[f"group_{group}"] = {
                "tpr": tpr,
                "fpr": fpr,
                "tnr": true_negatives / (true_negatives + false_positives) if (true_negatives + false_positives) > 0 else 0,
                "fnr": false_negatives / (true_positives + false_negatives) if (true_positives + false_negatives) > 0 else 0
            }
        
        return results

    @staticmethod
    def calculate_predictive_equivalence(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str,
        predicted_col: str
    ) -> Dict[str, float]:
        """Calculate predictive parity (PPV equality)."""
        groups = data[protected_col].unique()
        ppv_scores = {}
        
        for group in groups:
            group_data = data[data[protected_col] == group]
            
            true_positives = ((group_data[predicted_col] == 1) & 
                            (group_data[outcome_col] == 1)).sum()
            predicted_positives = (group_data[predicted_col] == 1).sum()
            
            ppv = true_positives / predicted_positives if predicted_positives > 0 else 0
            ppv_scores[f"group_{group}"] = ppv
        
        return ppv_scores


class BiasDetector:
    """Main bias detection class."""

    def __init__(self):
        self.bias_reports: List[BiasReport] = []
        self.stat_parity = StatisticalParityCalculator()
        self.eq_odds = EqualizedOddsCalculator()

    def detect_selection_bias(
        self,
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str
    ) -> List[BiasReport]:
        """Detect selection bias in dataset."""
        reports = []
        
        disparity = self.stat_parity.calculate_disparate_impact(
            data, protected_col, outcome_col
        )
        
        severity = "high" if disparity < 0.8 else "medium" if disparity < 0.9 else "low"
        
        report = BiasReport(
            bias_type=BiasType.SELECTION,
            protected_attribute=protected_col,
            metric_name="disparate_impact",
            value=disparity,
            threshold=0.8,
            severity=severity,
            description=f"Selection bias detected: disparate impact = {disparity:.3f}",
            recommendations=[
                "Review data collection process",
                "Check for missing data patterns across groups",
                "Consider re-weighting or resampling"
            ]
        )
        reports.append(report)
        
        return reports

    def detect_measurement_bias(
        self,
        data: pd.DataFrame,
        protected_col: str,
        feature_cols: List[str]
    ) -> List[BiasReport]:
        """Detect measurement bias (label noise differences)."""
        reports = []
        
        groups = data[protected_col].unique()
        
        for feature in feature_cols:
            group_stds = []
            for group in groups:
                group_data = data[data[protected_col] == group]
                group_stds.append(group_data[feature].std())
            
            if len(group_stds) >= 2:
                std_ratio = min(group_stds) / max(group_stds) if max(group_stds) > 0 else 1
                
                if std_ratio < 0.7:
                    severity = "high" if std_ratio < 0.5 else "medium"
                    report = BiasReport(
                        bias_type=BiasType.MEASUREMENT,
                        protected_attribute=protected_col,
                        metric_name=f"measurement_variance_{feature}",
                        value=std_ratio,
                        threshold=0.7,
                        severity=severity,
                        description=f"Measurement bias: variance ratio = {std_ratio:.3f} for {feature}",
                        recommendations=[
                            f"Review measurement process for {feature}",
                            "Check for differential misclassification",
                            "Consider calibration across groups"
                        ]
                    )
                    reports.append(report)
        
        return reports

    def detect_proxy_bias(
        self,
        data: pd.DataFrame,
        protected_col: str,
        feature_cols: List[str]
    ) -> List[BiasReport]:
        """Detect proxy bias (correlations with protected attributes)."""
        reports = []
        
        correlation_threshold = 0.7
        
        for feature in feature_cols:
            if feature == protected_col:
                continue
            
            try:
                correlation = abs(data[protected_col].astype(float).corr(
                    data[feature].astype(float)
                ))
                
                if correlation > correlation_threshold:
                    severity = "high" if correlation > 0.9 else "medium"
                    report = BiasReport(
                        bias_type=BiasType.PROXY,
                        protected_attribute=protected_col,
                        metric_name=f"proxy_correlation_{feature}",
                        value=correlation,
                        threshold=correlation_threshold,
                        severity=severity,
                        description=f"Proxy bias: {feature} strongly correlated with {protected_col}",
                        recommendations=[
                            f"Consider removing or transforming {feature}",
                            "Use causal inference techniques",
                            "Apply fairness-aware feature selection"
                        ]
                    )
                    reports.append(report)
            except:
                pass
        
        return reports

    def run_full_analysis(
        self,
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str = None,
        predicted_col: str = None,
        feature_cols: List[str] = None
    ) -> List[BiasReport]:
        """Run comprehensive bias detection analysis."""
        all_reports = []
        
        if outcome_col:
            all_reports.extend(
                self.detect_selection_bias(data, protected_col, outcome_col)
            )
        
        if feature_cols:
            all_reports.extend(
                self.detect_measurement_bias(data, protected_col, feature_cols)
            )
            all_reports.extend(
                self.detect_proxy_bias(data, protected_col, feature_cols)
            )
        
        if outcome_col and predicted_col:
            equalized_odds = self.eq_odds.calculate_equalized_odds(
                data, protected_col, outcome_col, predicted_col
            )
            print("\nEqualized Odds Analysis:")
            for group, metrics in equalized_odds.items():
                print(f"  {group}: TPR={metrics['tpr']:.3f}, FPR={metrics['fpr']:.3f}")
        
        self.bias_reports = all_reports
        return all_reports


class BiasMitigation:
    """Bias mitigation techniques."""

    @staticmethod
    def reweighting(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str
    ) -> pd.DataFrame:
        """
        Reweighting technique for bias mitigation.
        
        Adjusts sample weights to achieve statistical parity.
        """
        groups = data[protected_col].unique()
        outcomes = data[outcome_col].unique()
        
        total_samples = len(data)
        
        weights = []
        
        for _, row in data.iterrows():
            group = row[protected_col]
            outcome = row[outcome_col]
            
            group_size = len(data[data[protected_col] == group])
            outcome_size = len(data[data[outcome_col] == outcome])
            joint_size = len(data[(data[protected_col] == group) & 
                                  (data[outcome_col] == outcome)])
            
            expected = (group_size * outcome_size) / total_samples
            
            weight = expected / joint_size if joint_size > 0 else 1.0
            weights.append(weight)
        
        data = data.copy()
        data['sample_weight'] = weights
        
        return data

    @staticmethod
    def disparate_mitigator(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str,
        target_rate: float
    ) -> pd.DataFrame:
        """
        Disparate impact mitigator.
        
        Adjusts predictions to achieve target disparate impact.
        """
        groups = data[protected_col].unique()
        
        reference_group = max(groups, key=lambda g: 
            len(data[data[protected_col] == g]))
        
        reference_rate = data[data[protected_col] == reference_group][outcome_col].mean()
        
        adjusted_data = data.copy()
        
        for group in groups:
            if group == reference_group:
                continue
            
            group_mask = adjusted_data[protected_col] == group
            current_rate = adjusted_data[group_mask][outcome_col].mean()
            
            if current_rate > 0:
                adjustment_factor = (target_rate * reference_rate) / current_rate
                adjustment_factor = min(adjustment_factor, 1.0)
                
                group_indices = adjusted_data[group_mask].index
                for idx in group_indices:
                    if np.random.random() < adjustment_factor:
                        adjusted_data.loc[idx, outcome_col] = 1
                    else:
                        adjusted_data.loc[idx, outcome_col] = 0
        
        return adjusted_data

    @staticmethod
    def threshold_optimization(
        data: pd.DataFrame,
        protected_col: str,
        score_col: str,
        outcome_col: str,
        fairness_constraint: float = 0.8
    ) -> Dict[str, float]:
        """
        Optimize classification thresholds for fairness.
        
        Finds group-specific thresholds that achieve fairness constraint.
        """
        groups = data[protected_col].unique()
        
        thresholds = {}
        
        reference_group = max(groups, key=lambda g: 
            len(data[data[protected_col] == g]))
        
        reference_data = data[data[protected_col] == reference_group]
        
        best_threshold = 0.5
        for threshold in np.arange(0.1, 0.9, 0.05):
            reference_positive_rate = (reference_data[score_col] >= threshold).mean()
            
            if reference_positive_rate > 0:
                thresholds[reference_group] = threshold
                break
        
        for group in groups:
            if group == reference_group:
                continue
            
            group_data = data[data[protected_col] == group]
            
            found_threshold = False
            for threshold in np.arange(0.1, 0.9, 0.05):
                group_positive_rate = (group_data[score_col] >= threshold).mean()
                reference_positive_rate = thresholds.get(reference_group, 0.5)
                
                ratio = group_positive_rate / reference_positive_rate if reference_positive_rate > 0 else 0
                
                if ratio >= fairness_constraint:
                    thresholds[group] = threshold
                    found_threshold = True
                    break
            
            if not found_threshold:
                thresholds[group] = 0.5
        
        return thresholds

    @staticmethod
    def fair_feature_transform(
        data: pd.DataFrame,
        protected_col: str,
        feature_cols: List[str]
    ) -> pd.DataFrame:
        """
        Transform features to remove protected attribute information.
        
        Uses orthogonal transformation.
        """
        transformed = data.copy()
        
        protected_dummies = pd.get_dummies(data[protected_col], prefix='protected')
        
        for feature in feature_cols:
            if feature == protected_col:
                continue
            
            for col in protected_dummies.columns:
                try:
                    from scipy import stats
                    
                    feature_values = data[feature].values.reshape(-1)
                    protected_values = protected_dummies[col].values
                    
                    slope, intercept, _, _, _ = stats.linregress(
                        protected_values, feature_values
                    )
                    
                    predicted = slope * protected_values + intercept
                    residual = feature_values - predicted
                    
                    transformed[f"{feature}_fair"] = residual
                except:
                    transformed[f"{feature}_fair"] = data[feature]
        
        return transformed


def banking_example():
    """Bias detection and mitigation in banking sector."""
    print("\n" + "="*60)
    print("BANKING SECTOR: Credit Decision Fairness")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 1000
    
    data = pd.DataFrame({
        'gender': np.random.choice(['Male', 'Female'], n_samples),
        'age': np.random.randint(18, 70, n_samples),
        'income': np.random.normal(50000, 15000, n_samples),
        'credit_score': np.random.randint(500, 850, n_samples),
        'employment_years': np.random.randint(0, 30, n_samples),
    })
    
    credit_approval_rate = 0.3
    base_approval = np.random.random(n_samples) < credit_approval_rate
    
    gender_bias = data['gender'].map({'Male': 1.2, 'Female': 0.8})
    approval_prob = base_approval * gender_bias
    
    data['credit_approved'] = (approval_prob > 0.5).astype(int)
    
    print("\nBias Detection:")
    detector = BiasDetector()
    
    reports = detector.run_full_analysis(
        data,
        protected_col='gender',
        outcome_col='credit_approved',
        feature_cols=['income', 'credit_score', 'employment_years']
    )
    
    print(f"\nFound {len(reports)} bias issues:")
    for report in reports:
        print(f"  - {report.description}")
        print(f"    Severity: {report.severity}")
        print(f"    Recommendations: {report.recommendations[0]}")
    
    print("\nBias Mitigation:")
    
    print("\n1. Reweighting:")
    weighted_data = BiasMitigation.reweighting(
        data, 'gender', 'credit_approved'
    )
    print(f"   Applied sample weights to achieve statistical parity")
    print(f"   Weight range: [{weighted_data['sample_weight'].min():.3f}, {weighted_data['sample_weight'].max():.3f}]")
    
    print("\n2. Threshold Optimization:")
    data['credit_score_normalized'] = (data['credit_score'] - 500) / 350
    thresholds = BiasMitigation.threshold_optimization(
        data,
        protected_col='gender',
        score_col='credit_score_normalized',
        outcome_col='credit_approved',
        fairness_constraint=0.8
    )
    print(f"   Optimized thresholds:")
    for group, threshold in thresholds.items():
        print(f"     {group}: {threshold:.3f}")


def healthcare_example():
    """Bias detection and mitigation in healthcare sector."""
    print("\n" + "="*60)
    print("HEALTHCARE SECTOR: Treatment Recommendation Fairness")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 1000
    
    data = pd.DataFrame({
        'race': np.random.choice(['White', 'Black', 'Asian', 'Hispanic'], n_samples),
        'age': np.random.randint(20, 80, n_samples),
        'bmi': np.random.normal(27, 5, n_samples),
        'blood_pressure': np.random.normal(120, 15, n_samples),
        'cholesterol': np.random.normal(200, 30, n_samples),
    })
    
    treatment_rate = 0.25
    base_treatment = np.random.random(n_samples) < treatment_rate
    
    race_effect = data['race'].map({
        'White': 1.0, 
        'Black': 0.7, 
        'Asian': 0.9, 
        'Hispanic': 0.75
    })
    treatment_prob = base_treatment * race_effect
    
    data['treatment_recommended'] = (treatment_prob > 0.5).astype(int)
    
    print("\nBias Detection:")
    detector = BiasDetector()
    
    disparity = StatisticalParityCalculator().calculate_disparate_impact(
        data, 'race', 'treatment_recommended'
    )
    print(f"Disparate impact: {disparity:.3f}")
    
    stat_parity = StatisticalParityCalculator().calculate_statistical_parity(
        data, 'race', 'treatment_recommended'
    )
    print(f"\nTreatment rates by race:")
    for key, value in stat_parity.items():
        if key != 'overall':
            print(f"  {key}: {value:.3f}")
    
    print("\nBias Mitigation:")
    
    print("\n1. Disparate Impact Mitigator:")
    mitigated_data = BiasMitigation.disparate_mitigator(
        data, 'race', 'treatment_recommended', target_rate=0.8
    )
    print("   Applied disparate impact mitigation")
    
    print("\n2. Fair Feature Transform:")
    fair_data = BiasMitigation.fair_feature_transform(
        data,
        'race',
        ['bmi', 'blood_pressure', 'cholesterol']
    )
    print("   Transformed features to remove race correlation")


def core_implementation():
    """Core implementation demonstrating all components."""
    print("\n" + "="*60)
    print("CORE IMPLEMENTATION")
    print("="*60)
    
    print("\n1. Statistical Parity Calculator:")
    stat_parity = StatisticalParityCalculator()
    print("   Statistical parity calculator initialized")
    
    print("\n2. Equalized Odds Calculator:")
    eq_odds = EqualizedOddsCalculator()
    print("   Equalized odds calculator initialized")
    
    print("\n3. Bias Detector:")
    detector = BiasDetector()
    print("   Bias detector initialized")
    
    print("\n4. Bias Mitigation:")
    print("   Reweighting technique available")
    print("   Disparate mitigator available")
    print("   Threshold optimization available")
    print("   Fair feature transform available")


def main():
    print("="*60)
    print("BIAS DETECTION AND MITIGATION")
    print("="*60)
    
    core_implementation()
    banking_example()
    healthcare_example()
    
    print("\n" + "="*60)
    print("IMPLEMENTATION COMPLETE")
    print("="*60)


if __name__ == "__main__":
    main()