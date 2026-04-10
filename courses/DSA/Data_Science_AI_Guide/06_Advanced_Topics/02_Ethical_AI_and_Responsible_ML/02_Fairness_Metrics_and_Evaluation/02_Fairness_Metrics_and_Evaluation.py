# Topic: Fairness Metrics and Evaluation
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Fairness Metrics and Evaluation

I. INTRODUCTION
This module provides comprehensive fairness metrics for evaluating machine learning
models across different demographic groups. It covers individual and group fairness
metrics, confusion matrix-based measures, and calibration analysis.

II. CORE CONCEPTS
- Group fairness metrics (demographic parity, equalized odds)
- Individual fairness metrics
- Confusion matrix fairness measures
- Calibration analysis
- Fairness-accuracy tradeoffs

III. IMPLEMENTATION
"""

import numpy as np
import pandas as pd
from typing import Dict, List, Tuple, Optional, Any
from dataclasses import dataclass
from enum import Enum
import math


class FairnessMetricType(Enum):
    """Types of fairness metrics."""
    GROUP_FAIRNESS = "group_fairness"
    INDIVIDUAL_FAIRNESS = "individual_fairness"
    CALIBRATION = "calibration"
    CONFUSION_MATRIX = "confusion_matrix"


@dataclass
class FairnessReport:
    """Comprehensive fairness evaluation report."""
    metric_name: str
    metric_type: FairnessMetricType
    value: float
    threshold: float
    passes: bool
    group_scores: Dict[str, float]
    overall_score: float


class ConfusionMatrixMetrics:
    """Confusion matrix based fairness metrics."""

    def __init__(self):
        self.matrix: Dict[str, int] = {
            'tp': 0, 'fp': 0, 'tn': 0, 'fn': 0
        }

    @staticmethod
    def from_predictions(
        y_true: np.ndarray,
        y_pred: np.ndarray
    ) -> 'ConfusionMatrixMetrics':
        """Create confusion matrix from predictions."""
        cm = ConfusionMatrixMetrics()
        
        cm.matrix['tp'] = np.sum((y_true == 1) & (y_pred == 1))
        cm.matrix['fp'] = np.sum((y_true == 0) & (y_pred == 1))
        cm.matrix['tn'] = np.sum((y_true == 0) & (y_pred == 0))
        cm.matrix['fn'] = np.sum((y_true == 1) & (y_pred == 0))
        
        return cm

    def accuracy(self) -> float:
        """Calculate accuracy."""
        total = sum(self.matrix.values())
        if total == 0:
            return 0.0
        return (self.matrix['tp'] + self.matrix['tn']) / total

    def precision(self) -> float:
        """Calculate precision."""
        tp = self.matrix['tp']
        fp = self.matrix['fp']
        if tp + fp == 0:
            return 0.0
        return tp / (tp + fp)

    def recall(self) -> float:
        """Calculate recall (true positive rate)."""
        tp = self.matrix['tp']
        fn = self.matrix['fn']
        if tp + fn == 0:
            return 0.0
        return tp / (tp + fn)

    def false_positive_rate(self) -> float:
        """Calculate false positive rate."""
        fp = self.matrix['fp']
        tn = self.matrix['tn']
        if fp + tn == 0:
            return 0.0
        return fp / (fp + tn)

    def false_negative_rate(self) -> float:
        """Calculate false negative rate."""
        fn = self.matrix['fn']
        tp = self.matrix['tp']
        if fn + tp == 0:
            return 0.0
        return fn / (fn + tp)

    def specificity(self) -> float:
        """Calculate specificity (true negative rate)."""
        return 1 - self.false_positive_rate()

    def f1_score(self) -> float:
        """Calculate F1 score."""
        p = self.precision()
        r = self.recall()
        if p + r == 0:
            return 0.0
        return 2 * (p * r) / (p + r)


class GroupFairnessEvaluator:
    """Group fairness evaluation metrics."""

    @staticmethod
    def demographic_parity(
        data: pd.DataFrame,
        protected_col: str,
        prediction_col: str
    ) -> FairnessReport:
        """
        Calculate demographic parity (statistical parity).
        
        Measures whether positive prediction rates are equal across groups.
        """
        groups = data[protected_col].unique()
        group_scores = {}
        
        for group in groups:
            group_data = data[data[protected_col] == group]
            positive_rate = group_data[prediction_col].mean()
            group_scores[f"group_{group}"] = positive_rate
        
        overall_positive_rate = data[prediction_col].mean()
        
        rates = list(group_scores.values())
        max_diff = max(abs(r - overall_positive_rate) for r in rates)
        
        passes = max_diff < 0.1
        
        return FairnessReport(
            metric_name="demographic_parity",
            metric_type=FairnessMetricType.GROUP_FAIRNESS,
            value=overall_positive_rate,
            threshold=0.1,
            passes=passes,
            group_scores=group_scores,
            overall_score=overall_positive_rate
        )

    @staticmethod
    def equalized_odds(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str,
        prediction_col: str
    ) -> FairnessReport:
        """
        Calculate equalized odds.
        
        Measures whether TPR and FPR are equal across groups.
        """
        groups = data[protected_col].unique()
        
        all_tpr = []
        all_fpr = []
        
        for group in groups:
            group_data = data[data[protected_col] == group]
            
            cm = ConfusionMatrixMetrics.from_predictions(
                group_data[outcome_col].values,
                group_data[prediction_col].values
            )
            
            tpr = cm.recall()
            fpr = cm.false_positive_rate()
            
            all_tpr.append(tpr)
            all_fpr.append(fpr)
        
        tpr_difference = max(all_tpr) - min(all_tpr)
        fpr_difference = max(all_fpr) - min(all_fpr)
        
        overall_tpr = np.mean(all_tpr)
        overall_fpr = np.mean(all_fpr)
        
        passes = tpr_difference < 0.1 and fpr_difference < 0.1
        
        return FairnessReport(
            metric_name="equalized_odds",
            metric_type=FairnessMetricType.GROUP_FAIRNESS,
            value=(overall_tpr + (1 - overall_fpr)) / 2,
            threshold=0.1,
            passes=passes,
            group_scores={
                f"group_{g}_tpr": tpr for g, tpr in zip(groups, all_tpr)
            },
            overall_score=(overall_tpr + (1 - overall_fpr)) / 2
        )

    @staticmethod
    def equal_opportunity(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str,
        prediction_col: str
    ) -> FairnessReport:
        """
        Calculate equal opportunity.
        
        Measures whether true positive rates are equal across groups.
        """
        groups = data[protected_col].unique()
        
        tpr_scores = {}
        
        for group in groups:
            group_data = data[data[protected_col] == group]
            
            cm = ConfusionMatrixMetrics.from_predictions(
                group_data[outcome_col].values,
                group_data[prediction_col].values
            )
            
            tpr_scores[f"group_{group}"] = cm.recall()
        
        tpr_values = list(tpr_scores.values())
        max_difference = max(tpr_values) - min(tpr_values)
        
        overall_tpr = np.mean(tpr_values)
        
        passes = max_difference < 0.1
        
        return FairnessReport(
            metric_name="equal_opportunity",
            metric_type=FairnessMetricType.GROUP_FAIRNESS,
            value=overall_tpr,
            threshold=0.1,
            passes=passes,
            group_scores=tpr_scores,
            overall_score=overall_tpr
        )

    @staticmethod
    def predictive_equity(
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str,
        prediction_col: str
    ) -> FairnessReport:
        """
        Calculate predictive equity.
        
        Measures whether positive predictive values are equal across groups.
        """
        groups = data[protected_col].unique()
        
        ppv_scores = {}
        
        for group in groups:
            group_data = data[data[protected_col] == group]
            
            cm = ConfusionMatrixMetrics.from_predictions(
                group_data[outcome_col].values,
                group_data[prediction_col].values
            )
            
            ppv_scores[f"group_{group}"] = cm.precision()
        
        ppv_values = [v for v in ppv_scores.values() if not math.isnan(v)]
        
        if len(ppv_values) > 0:
            max_difference = max(ppv_values) - min(ppv_values)
            overall_ppv = np.mean(ppv_values)
        else:
            max_difference = 0
            overall_ppv = 0
        
        passes = max_difference < 0.1
        
        return FairnessReport(
            metric_name="predictive_equity",
            metric_type=FairnessMetricType.GROUP_FAIRNESS,
            value=overall_ppv,
            threshold=0.1,
            passes=passes,
            group_scores=ppv_scores,
            overall_score=overall_ppv
        )


class IndividualFairnessEvaluator:
    """Individual fairness evaluation metrics."""

    @staticmethod
    def consistency_score(
        data: pd.DataFrame,
        features: List[str],
        prediction_col: str,
        n_neighbors: int = 5
    ) -> float:
        """
        Calculate individual fairness through consistency.
        
        Measures whether similar individuals receive similar predictions.
        """
        from sklearn.neighbors import NearestNeighbors
        
        X = data[features].values
        y = data[prediction_col].values
        
        nn = NearestNeighbors(n_neighbors=n_neighbors + 1)
        nn.fit(X)
        
        distances, indices = nn.kneighbors(X)
        
        consistency_scores = []
        
        for i in range(len(X)):
            neighbor_predictions = y[indices[i][1:]]
            
            if len(neighbor_predictions) > 0:
                similarity = 1 - abs(y[i] - np.mean(neighbor_predictions))
                consistency_scores.append(similarity)
        
        return np.mean(consistency_scores) if consistency_scores else 0.0

    @staticmethod
    def theil_index(
        data: pd.DataFrame,
        prediction_col: str,
        weight_col: str = None
    ) -> float:
        """
        Calculate Theil index for individual fairness.
        
        Measures inequality in predictions.
        """
        predictions = data[prediction_col].values
        
        if weight_col:
            weights = data[weight_col].values
        else:
            weights = np.ones(len(predictions))
        
        weights = weights / weights.sum()
        
        mean_prediction = np.sum(weights * predictions)
        
        if mean_prediction == 0:
            return 0.0
        
        theil = np.sum(weights * predictions * np.log(predictions / mean_prediction))
        
        return theil


class CalibrationEvaluator:
    """Calibration analysis for fairness."""

    @staticmethod
    def calculate_calibration(
        data: pd.DataFrame,
        score_col: str,
        outcome_col: str,
        n_bins: int = 10
    ) -> Dict[str, Any]:
        """
        Calculate calibration metrics.
        
        Measures whether predicted probabilities match actual outcomes.
        """
        bins = np.linspace(0, 1, n_bins + 1)
        
        calibration_data = []
        
        for i in range(n_bins):
            bin_mask = (data[score_col] >= bins[i]) & (data[score_col] < bins[i+1])
            bin_data = data[bin_mask]
            
            if len(bin_data) > 0:
                avg_predicted = bin_data[score_col].mean()
                actual_positive = bin_data[outcome_col].mean()
                
                calibration_data.append({
                    'bin_start': bins[i],
                    'bin_end': bins[i+1],
                    'avg_predicted': avg_predicted,
                    'actual_positive': actual_positive,
                    'count': len(bin_data)
                })
        
        calibration_error = 0
        total_count = 0
        
        for cd in calibration_data:
            error = abs(cd['avg_predicted'] - cd['actual_positive'])
            calibration_error += error * cd['count']
            total_count += cd['count']
        
        calibration_error = calibration_error / total_count if total_count > 0 else 0
        
        return {
            'calibration_error': calibration_error,
            'bins': calibration_data
        }

    @staticmethod
    def calibration_by_group(
        data: pd.DataFrame,
        score_col: str,
        outcome_col: str,
        protected_col: str,
        n_bins: int = 10
    ) -> Dict[str, Dict[str, float]]:
        """
        Calculate calibration metrics by protected group.
        
        Measures whether calibration holds across groups.
        """
        groups = data[protected_col].unique()
        
        group_calibration = {}
        
        for group in groups:
            group_data = data[data[protected_col] == group]
            
            calibration = CalibrationEvaluator.calculate_calibration(
                group_data, score_col, outcome_col, n_bins
            )
            
            group_calibration[f"group_{group}"] = {
                'calibration_error': calibration['calibration_error']
            }
        
        return group_calibration


class FairnessEvaluator:
    """Comprehensive fairness evaluation."""

    def __init__(self):
        self.group_evaluator = GroupFairnessEvaluator()
        self.individual_evaluator = IndividualFairnessEvaluator()
        self.calibration_evaluator = CalibrationEvaluator()
        self.reports: List[FairnessReport] = []

    def evaluate_model(
        self,
        data: pd.DataFrame,
        protected_col: str,
        outcome_col: str,
        prediction_col: str,
        score_col: str = None,
        features: List[str] = None
    ) -> Dict[str, Any]:
        """
        Run comprehensive fairness evaluation.
        """
        results = {}
        
        print("\nGroup Fairness Metrics:")
        
        demo_parity = self.group_evaluator.demographic_parity(
            data, protected_col, prediction_col
        )
        results['demographic_parity'] = demo_parity
        print(f"  Demographic Parity: {'PASS' if demo_parity.passes else 'FAIL'}")
        
        eq_odds = self.group_evaluator.equalized_odds(
            data, protected_col, outcome_col, prediction_col
        )
        results['equalized_odds'] = eq_odds
        print(f"  Equalized Odds: {'PASS' if eq_odds.passes else 'FAIL'}")
        
        eq_opp = self.group_evaluator.equal_opportunity(
            data, protected_col, outcome_col, prediction_col
        )
        results['equal_opportunity'] = eq_opp
        print(f"  Equal Opportunity: {'PASS' if eq_opp.passes else 'FAIL'}")
        
        pred_equity = self.group_evaluator.predictive_equity(
            data, protected_col, outcome_col, prediction_col
        )
        results['predictive_equity'] = pred_equity
        print(f"  Predictive Equity: {'PASS' if pred_equity.passes else 'FAIL'}")
        
        if score_col and features:
            print("\nIndividual Fairness Metrics:")
            
            consistency = self.individual_evaluator.consistency_score(
                data, features, prediction_col
            )
            results['consistency'] = consistency
            print(f"  Consistency Score: {consistency:.4f}")
        
        if score_col:
            print("\nCalibration Analysis:")
            
            calibration = self.calibration_evaluator.calculate_calibration(
                data, score_col, outcome_col
            )
            results['calibration'] = calibration
            print(f"  Calibration Error: {calibration['calibration_error']:.4f}")
            
            group_calibration = self.calibration_evaluator.calibration_by_group(
                data, score_col, outcome_col, protected_col
            )
            results['calibration_by_group'] = group_calibration
        
        self.reports = [v for v in results.values() if isinstance(v, FairnessReport)]
        
        return results

    def fairness_summary(self) -> pd.DataFrame:
        """Generate fairness summary report."""
        summary_data = []
        
        for report in self.reports:
            summary_data.append({
                'metric': report.metric_name,
                'type': report.metric_type.value,
                'value': report.value,
                'threshold': report.threshold,
                'passes': report.passes
            })
        
        return pd.DataFrame(summary_data)


def banking_example():
    """Fairness evaluation in banking sector."""
    print("\n" + "="*60)
    print("BANKING SECTOR: Loan Approval Fairness")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 2000
    
    data = pd.DataFrame({
        'gender': np.random.choice(['Male', 'Female'], n_samples),
        'age': np.random.randint(18, 70, n_samples),
        'income': np.random.normal(60000, 20000, n_samples),
        'credit_score': np.random.randint(500, 850, n_samples),
    })
    
    base_approval_prob = (data['credit_score'] - 500) / 350
    gender_effect = data['gender'].map({'Male': 1.0, 'Female': 0.85})
    age_effect = 1 - (data['age'] - 18) / 100
    
    approval_prob = base_approval_prob * gender_effect * age_effect
    
    data['approved'] = (np.random.random(n_samples) < approval_prob).astype(int)
    data['approval_prob'] = approval_prob
    
    print("\nFairness Evaluation:")
    
    evaluator = FairnessEvaluator()
    results = evaluator.evaluate_model(
        data,
        protected_col='gender',
        outcome_col='approved',
        prediction_col='approved',
        score_col='approval_prob',
        features=['income', 'credit_score', 'age']
    )
    
    print("\nGroup-specific Metrics:")
    for group in ['Male', 'Female']:
        group_data = data[data['gender'] == group]
        approval_rate = group_data['approved'].mean()
        print(f"  {group}: Approval Rate = {approval_rate:.3f}")
    
    print("\nConfusion Matrix Metrics by Group:")
    for group in ['Male', 'Female']:
        group_data = data[data['gender'] == group]
        cm = ConfusionMatrixMetrics.from_predictions(
            group_data['approved'].values,
            group_data['approved'].values
        )
        print(f"  {group}: Precision={cm.precision():.3f}, Recall={cm.recall():.3f}")


def healthcare_example():
    """Fairness evaluation in healthcare sector."""
    print("\n" + "="*60)
    print("HEALTHCARE SECTOR: Patient Triage Fairness")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 2000
    
    data = pd.DataFrame({
        'race': np.random.choice(['White', 'Black', 'Asian', 'Hispanic'], n_samples),
        'age': np.random.randint(20, 85, n_samples),
        'bmi': np.random.normal(27, 5, n_samples),
        'severity_score': np.random.uniform(0, 1, n_samples),
    })
    
    base_triage_prob = data['severity_score']
    race_effect = data['race'].map({
        'White': 1.0,
        'Black': 0.75,
        'Asian': 0.95,
        'Hispanic': 0.8
    })
    
    triage_prob = base_triage_prob * race_effect
    
    data['high_risk_predicted'] = (triage_prob > 0.6).astype(int)
    data['actually_high_risk'] = (data['severity_score'] > 0.6).astype(int)
    data['triage_probability'] = triage_prob
    
    print("\nFairness Evaluation:")
    
    evaluator = FairnessEvaluator()
    results = evaluator.evaluate_model(
        data,
        protected_col='race',
        outcome_col='actually_high_risk',
        prediction_col='high_risk_predicted',
        score_col='triage_probability',
        features=['age', 'bmi', 'severity_score']
    )
    
    print("\nGroup-specific Metrics:")
    for group in ['White', 'Black', 'Asian', 'Hispanic']:
        group_data = data[data['race'] == group]
        triage_rate = group_data['high_risk_predicted'].mean()
        actual_rate = group_data['actually_high_risk'].mean()
        print(f"  {group}: Predicted={triage_rate:.3f}, Actual={actual_rate:.3f}")


def core_implementation():
    """Core implementation demonstrating all components."""
    print("\n" + "="*60)
    print("CORE IMPLEMENTATION")
    print("="*60)
    
    print("\n1. Confusion Matrix Metrics:")
    cm = ConfusionMatrixMetrics()
    print("   Accuracy, Precision, Recall, FPR, TNR available")
    
    print("\n2. Group Fairness Evaluator:")
    print("   Demographic parity, Equalized odds, Equal opportunity available")
    
    print("\n3. Individual Fairness Evaluator:")
    print("   Consistency score, Theil index available")
    
    print("\n4. Calibration Evaluator:")
    print("   Calibration analysis and group calibration available")
    
    print("\n5. Fairness Evaluator:")
    print("   Comprehensive model evaluation available")


def main():
    print("="*60)
    print("FAIRNESS METRICS AND EVALUATION")
    print("="*60)
    
    core_implementation()
    banking_example()
    healthcare_example()
    
    print("\n" + "="*60)
    print("IMPLEMENTATION COMPLETE")
    print("="*60)


if __name__ == "__main__":
    main()