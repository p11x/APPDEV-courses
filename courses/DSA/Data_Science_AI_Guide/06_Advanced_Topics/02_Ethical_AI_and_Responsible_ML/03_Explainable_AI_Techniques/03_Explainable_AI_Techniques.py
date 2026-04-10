# Topic: Explainable AI Techniques
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Explainable AI Techniques

I. INTRODUCTION
This module covers various techniques for interpreting and explaining machine learning
models. It includes SHAP values, LIME, feature importance, counterfactuals,
and attention visualization for deep learning.

II. CORE CONCEPTS
- Global vs. local explanations
- Feature importance methods
- SHAP (SHapley Additive exPlanations)
- LIME (Local Interpretable Model-agnostic Explanations)
- Counterfactual explanations
- Attention visualization

III. IMPLEMENTATION
"""

import numpy as np
import pandas as pd
from typing import Dict, List, Tuple, Optional, Any
from dataclasses import dataclass
from enum import Enum
import math


class ExplanationType(Enum):
    """Types of model explanations."""
    GLOBAL = "global"
    LOCAL = "local"
    COUNTERFACTUAL = "counterfactual"
    FEATURE_IMPORTANCE = "feature_importance"


@dataclass
class Explanation:
    """Explanation result."""
    method: str
    feature_names: List[str]
    importance_values: np.ndarray
    local_explanation: bool = False
    instance_index: Optional[int] = None


class FeatureImportanceAnalyzer:
    """Feature importance analysis for ML models."""

    @staticmethod
    def permutation_importance(
        model: Any,
        X: np.ndarray,
        y: np.ndarray,
        feature_names: List[str],
        n_repeats: int = 10,
        scoring_fn: callable = None
    ) -> Dict[str, float]:
        """
        Calculate permutation importance.
        
        Measures importance by shuffling feature values.
        """
        if scoring_fn is None:
            from sklearn.metrics import accuracy_score
            scoring_fn = accuracy_score
        
        baseline_score = scoring_fn(y, model.predict(X))
        
        importances = {name: [] for name in feature_names}
        
        X_permuted = X.copy()
        
        for feature_idx in range(X.shape[1]):
            for _ in range(n_repeats):
                np.random.shuffle(X_permuted[:, feature_idx])
                
                permuted_score = scoring_fn(y, model.predict(X_permuted))
                
                importance = baseline_score - permuted_score
                importances[feature_names[feature_idx]].append(importance)
                
                X_permuted = X.copy()
        
        mean_importances = {
            name: np.mean(values) for name, values in importances.items()
        }
        
        return mean_importances

    @staticmethod
    def tree_based_importance(
        model: Any,
        feature_names: List[str]
    ) -> Dict[str, float]:
        """
        Extract tree-based feature importance.
        
        Works with tree ensemble models.
        """
        if hasattr(model, 'feature_importances_'):
            importances = model.feature_importances_
            
            return {
                name: float(imp) 
                for name, imp in zip(feature_names, importances)
            }
        
        return {}

    @staticmethod
    def calculate_shap_values(
        model: Any,
        X: np.ndarray,
        feature_names: List[str],
        n_samples: int = 100
    ) -> np.ndarray:
        """
        Calculate SHAP values using kernel SHAP approximation.
        
        Uses additive feature attribution method.
        """
        n_features = X.shape[1]
        
        baseline = np.mean(model.predict(X))
        
        shap_values = np.zeros((X.shape[0], n_features))
        
        for i in range(min(n_samples, X.shape[0])):
            instance = X[i]
            
            feature_contributions = []
            
            for j in range(n_features):
                single_feature = np.zeros(n_features)
                single_feature[j] = instance[j]
                
                contribution = model.predict(single_feature.reshape(1, -1))[0] - baseline
                
                feature_contributions.append(contribution)
            
            shap_values[i] = np.array(feature_contributions)
        
        return shap_values

    @staticmethod
    def global_feature_importance(
        shap_values: np.ndarray,
        feature_names: List[str]
    ) -> Dict[str, float]:
        """
        Calculate global feature importance from SHAP values.
        
        Uses mean absolute SHAP values.
        """
        mean_abs_shap = np.mean(np.abs(shap_values), axis=0)
        
        return {
            name: float(imp) 
            for name, imp in zip(feature_names, mean_abs_shap)
        }


class LIMEExplainer:
    """LIME (Local Interpretable Model-agnostic Explanations) implementation."""

    def __init__(self, num_features: int = 10, num_samples: int = 1000):
        self.num_features = num_features
        self.num_samples = num_samples

    def explain_instance(
        self,
        model: Any,
        instance: np.ndarray,
        feature_names: List[str],
        class_names: List[str] = None
    ) -> Explanation:
        """
        Generate local explanation for a single instance.
        
        Perturbs instance and fits simple interpretable model.
        """
        explanations = []
        
        for _ in range(self.num_samples):
            noise = np.random.randn(len(instance)) * 0.1
            perturbed = instance + noise
            
            prob = model.predict(perturbed.reshape(1, -1))[0]
            
            explanations.append((perturbed, prob))
        
        perturbed_X = np.array([exp[0] for exp in explanations])
        perturbed_y = np.array([exp[1] for exp in explanations])
        
        weights = 1 / (np.linalg.norm(perturbed_X - instance, axis=1) + 1e-6)
        
        importance_scores = np.zeros(len(feature_names))
        
        for i in range(len(feature_names)):
            feature_vals = perturbed_X[:, i]
            weighted_cov = np.cov(feature_vals, perturbed_y, aweights=weights)
            
            if not np.isnan(weighted_cov).any() and weighted_cov[0, 0] > 0:
                correlation = weighted_cov[0, 1] / np.sqrt(weighted_cov[0, 0])
                importance_scores[i] = abs(correlation)
        
        top_indices = np.argsort(importance_scores)[-self.num_features:]
        
        top_features = [feature_names[i] for i in top_indices]
        top_importance = importance_scores[top_indices]
        
        return Explanation(
            method="LIME",
            feature_names=top_features,
            importance_values=top_importance,
            local_explanation=True,
            instance_index=0
        )


class CounterfactualExplainer:
    """Counterfactual explanation generator."""

    def __init__(self, epsilon: float = 0.1, max_iterations: int = 100):
        self.epsilon = epsilon
        self.max_iterations = max_iterations

    def find_counterfactuals(
        self,
        model: Any,
        instance: np.ndarray,
        desired_class: int,
        feature_ranges: Dict[str, Tuple[float, float]],
        feature_names: List[str]
    ) -> List[Dict[str, Any]]:
        """
        Find counterfactual examples.
        
        Searches for minimal changes to flip prediction.
        """
        current = instance.copy()
        counterfactuals = []
        
        original_prediction = model.predict(current.reshape(1, -1))[0]
        
        for iteration in range(self.max_iterations):
            current_prediction = model.predict(current.reshape(1, -1))[0]
            
            if current_prediction != original_prediction:
                cf_example = {
                    'instance': current.copy(),
                    'prediction': current_prediction,
                    'changes': {}
                }
                
                for i, name in enumerate(feature_names):
                    if abs(current[i] - instance[i]) > 0.01:
                        cf_example['changes'][name] = {
                            'original': instance[i],
                            'counterfactual': current[i],
                            'change': current[i] - instance[i]
                        }
                
                counterfactuals.append(cf_example)
                
                if len(counterfactuals) >= 3:
                    break
            
            noise = np.random.randn(len(current)) * self.epsilon
            
            for i, name in enumerate(feature_names):
                if name in feature_ranges:
                    low, high = feature_ranges[name]
                    current[i] = np.clip(current[i] + noise[i], low, high)
        
        return counterfactuals

    def generate_whatif_explanations(
        self,
        model: Any,
        instance: np.ndarray,
        feature_names: List[str],
        feature_ranges: Dict[str, Tuple[float, float]]
    ) -> Dict[str, Any]:
        """
        Generate what-if explanations.
        
        Shows how changing each feature affects prediction.
        """
        original_prediction = model.predict(instance.reshape(1, -1))[0]
        
        whatif_results = {}
        
        for i, name in enumerate(feature_names):
            if name not in feature_ranges:
                continue
            
            low, high = feature_ranges[name]
            
            test_points = np.linspace(low, high, 10)
            predictions = []
            
            for test_val in test_points:
                test_instance = instance.copy()
                test_instance[i] = test_val
                pred = model.predict(test_instance.reshape(1, -1))[0]
                predictions.append(pred)
            
            whatif_results[name] = {
                'original_value': instance[i],
                'original_prediction': original_prediction,
                'predictions': list(zip(test_points.tolist(), predictions)),
                'min_prediction': min(predictions),
                'max_prediction': max(predictions)
            }
        
        return whatif_results


class AttentionAnalyzer:
    """Attention visualization for deep learning models."""

    @staticmethod
    def extract_attention_weights(
        attention_matrix: np.ndarray,
        layer_name: str = None
    ) -> Dict[str, Any]:
        """
        Extract and analyze attention weights.
        
        Provides insights into model's focus areas.
        """
        mean_attention = np.mean(attention_matrix, axis=0)
        
        max_attention_idx = np.argmax(mean_attention)
        
        attention_entropy = -np.sum(
            mean_attention * np.log(mean_attention + 1e-10)
        )
        
        normalized_attention = mean_attention / np.sum(mean_attention)
        
        return {
            'layer': layer_name,
            'mean_attention': mean_attention.tolist(),
            'max_attention_index': int(max_attention_idx),
            'attention_entropy': float(attention_entropy),
            'normalized_attention': normalized_attention.tolist()
        }

    @staticmethod
    def visualize_attention_patterns(
        attention_weights: np.ndarray,
        input_tokens: List[str]
    ) -> pd.DataFrame:
        """
        Create attention pattern visualization data.
        
        Shows attention between input tokens.
        """
        if len(input_tokens) != attention_weights.shape[0]:
            input_tokens = [f"token_{i}" for i in range(attention_weights.shape[0])]
        
        attention_df = pd.DataFrame(
            attention_weights,
            index=input_tokens,
            columns=input_tokens
        )
        
        return attention_df

    @staticmethod
    def compute_attention_head_diversity(
        attention_weights: np.ndarray,
        n_heads: int
    ) -> float:
        """
        Compute diversity score for multiple attention heads.
        
        Measures how differently each head attends.
        """
        head_diversities = []
        
        for i in range(n_heads):
            for j in range(i + 1, n_heads):
                cosine_sim = np.dot(
                    attention_weights[i], attention_weights[j]
                ) / (
                    np.linalg.norm(attention_weights[i]) * 
                    np.linalg.norm(attention_weights[j]) + 1e-10
                )
                head_diversities.append(1 - cosine_sim)
        
        return np.mean(head_diversities) if head_diversities else 0.0


class ModelExplanationGenerator:
    """Comprehensive model explanation generator."""

    def __init__(self):
        self.feature_analyzer = FeatureImportanceAnalyzer()
        self.lime_explainer = LIMEExplainer()
        self.counterfactual_explainer = CounterfactualExplainer()
        self.attention_analyzer = AttentionAnalyzer()

    def explain_model(
        self,
        model: Any,
        X: np.ndarray,
        y: np.ndarray,
        feature_names: List[str],
        instance_idx: int = 0
    ) -> Dict[str, Any]:
        """
        Generate comprehensive model explanation.
        """
        results = {}
        
        print("\nGlobal Feature Importance:")
        
        perm_importance = self.feature_analyzer.permutation_importance(
            model, X, y, feature_names, n_repeats=5
        )
        results['permutation_importance'] = perm_importance
        
        sorted_importance = sorted(
            perm_importance.items(), key=lambda x: x[1], reverse=True
        )
        for name, imp in sorted_importance[:5]:
            print(f"  {name}: {imp:.4f}")
        
        print("\nLocal Explanation (LIME):")
        
        instance = X[instance_idx]
        lime_exp = self.lime_explainer.explain_instance(
            model, instance, feature_names
        )
        results['lime'] = {
            'feature_names': lime_exp.feature_names,
            'importance_values': lime_exp.importance_values.tolist()
        }
        
        for name, imp in zip(lime_exp.feature_names, lime_exp.importance_values):
            print(f"  {name}: {imp:.4f}")
        
        return results


def banking_example():
    """Explainable AI in banking sector."""
    print("\n" + "="*60)
    print("BANKING SECTOR: Credit Risk Model Explanation")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 1000
    
    data = pd.DataFrame({
        'income': np.random.normal(60000, 20000, n_samples),
        'credit_score': np.random.randint(500, 850, n_samples),
        'employment_years': np.random.randint(0, 30, n_samples),
        'debt_amount': np.random.normal(10000, 5000, n_samples),
        'num_credit_lines': np.random.randint(1, 10, n_samples),
    })
    
    risk_prob = (
        0.3 * (850 - data['credit_score']) / 350 +
        0.2 * data['debt_amount'] / 20000 +
        0.1 * data['employment_years'] / 30
    )
    
    data['high_risk'] = (risk_prob > 0.5).astype(int)
    
    feature_names = ['income', 'credit_score', 'employment_years', 
                     'debt_amount', 'num_credit_lines']
    
    X = data[feature_names].values
    y = data['high_risk'].values
    
    class SimpleModel:
        def predict(self, X):
            if len(X.shape) == 1:
                X = X.reshape(1, -1)
            risk_score = (
                0.3 * (850 - X[:, 1]) / 350 +
                0.2 * X[:, 3] / 20000 +
                0.1 * X[:, 2] / 30
            )
            return (risk_score > 0.5).astype(int)
        
        def predict_proba(self, X):
            if len(X.shape) == 1:
                X = X.reshape(1, -1)
            risk_score = (
                0.3 * (850 - X[:, 1]) / 350 +
                0.2 * X[:, 3] / 20000 +
                0.1 * X[:, 2] / 30
            )
            return np.column_stack([1 - risk_score, risk_score])
    
    model = SimpleModel()
    
    print("\n1. Feature Importance Analysis:")
    
    perm_importance = FeatureImportanceAnalyzer.permutation_importance(
        model, X, y, feature_names, n_repeats=3
    )
    
    sorted_features = sorted(perm_importance.items(), key=lambda x: x[1], reverse=True)
    print("\n  Top features by importance:")
    for name, imp in sorted_features[:5]:
        print(f"    {name}: {imp:.4f}")
    
    print("\n2. Local Explanation (LIME):")
    
    lime = LIMEExplainer(num_features=3)
    instance = X[0]
    explanation = lime.explain_instance(model, instance, feature_names)
    
    print("  Local explanation for first instance:")
    for name, imp in zip(explanation.feature_names, explanation.importance_values):
        print(f"    {name}: {imp:.4f}")
    
    print("\n3. Counterfactual Explanations:")
    
    feature_ranges = {
        'income': (20000, 150000),
        'credit_score': (500, 850),
        'employment_years': (0, 30),
        'debt_amount': (0, 50000),
        'num_credit_lines': (1, 10)
    }
    
    counterfactuals = CounterfactualExplainer().find_counterfactuals(
        model, instance, 0, feature_ranges, feature_names
    )
    print(f"  Found {len(counterfactuals)} counterfactual examples")


def healthcare_example():
    """Explainable AI in healthcare sector."""
    print("\n" + "="*60)
    print("HEALTHCARE SECTOR: Treatment Recommendation Explanation")
    print("="*60)
    
    np.random.seed(42)
    n_samples = 1000
    
    data = pd.DataFrame({
        'age': np.random.randint(20, 80, n_samples),
        'bmi': np.random.normal(27, 5, n_samples),
        'blood_pressure': np.random.normal(120, 15, n_samples),
        'cholesterol': np.random.normal(200, 30, n_samples),
        'glucose_level': np.random.normal(100, 20, n_samples),
    })
    
    risk_score = (
        0.2 * (data['age'] - 20) / 60 +
        0.2 * (data['bmi'] - 18) / 20 +
        0.2 * (data['blood_pressure'] - 90) / 60 +
        0.2 * (data['cholesterol'] - 150) / 100 +
        0.2 * (data['glucose_level'] - 70) / 60
    )
    
    data['treatment_recommended'] = (risk_score > 0.5).astype(int)
    
    feature_names = ['age', 'bmi', 'blood_pressure', 'cholesterol', 'glucose_level']
    
    X = data[feature_names].values
    y = data['treatment_recommended'].values
    
    class TreatmentModel:
        def predict(self, X):
            if len(X.shape) == 1:
                X = X.reshape(1, -1)
            risk = (
                0.2 * (X[:, 0] - 20) / 60 +
                0.2 * (X[:, 1] - 18) / 20 +
                0.2 * (X[:, 2] - 90) / 60 +
                0.2 * (X[:, 3] - 150) / 100 +
                0.2 * (X[:, 4] - 70) / 60
            )
            return (risk > 0.5).astype(int)
    
    model = TreatmentModel()
    
    print("\n1. Feature Importance Analysis:")
    
    perm_importance = FeatureImportanceAnalyzer.permutation_importance(
        model, X, y, feature_names, n_repeats=3
    )
    
    sorted_features = sorted(perm_importance.items(), key=lambda x: x[1], reverse=True)
    print("\n  Top features by importance:")
    for name, imp in sorted_features[:5]:
        print(f"    {name}: {imp:.4f}")
    
    print("\n2. Attention Pattern Analysis:")
    
    attention_weights = np.random.rand(5, 5)
    attention_weights = attention_weights / attention_weights.sum(axis=1, keepdims=True)
    
    attention_analysis = AttentionAnalyzer.extract_attention_weights(
        attention_weights, layer_name="treatment_attention"
    )
    
    print(f"  Attention entropy: {attention_analysis['attention_entropy']:.4f}")
    print(f"  Max attention index: {attention_analysis['max_attention_index']}")
    
    print("\n3. What-If Analysis:")
    
    whatif_results = CounterfactualExplainer().generate_whatif_explanations(
        model, X[0], feature_names,
        {'age': (20, 80), 'bmi': (15, 40), 'blood_pressure': (80, 160)}
    )
    
    for feature, result in whatif_results.items():
        print(f"  {feature}:")
        print(f"    Original: {result['original_value']:.2f}, Prediction: {result['original_prediction']}")
        print(f"    Range: [{result['min_prediction']}, {result['max_prediction']}]")


def core_implementation():
    """Core implementation demonstrating all components."""
    print("\n" + "="*60)
    print("CORE IMPLEMENTATION")
    print("="*60)
    
    print("\n1. Feature Importance Analyzer:")
    print("   - Permutation importance")
    print("   - Tree-based importance")
    print("   - SHAP values")
    
    print("\n2. LIME Explainer:")
    print("   - Local explanation generation")
    print("   - Perturbation-based interpretation")
    
    print("\n3. Counterfactual Explainer:")
    print("   - Counterfactual search")
    print("   - What-if analysis")
    
    print("\n4. Attention Analyzer:")
    print("   - Attention weight extraction")
    print("   - Pattern visualization")
    
    print("\n5. Model Explanation Generator:")
    print("   - Comprehensive model explanation")


def main():
    print("="*60)
    print("EXPLAINABLE AI TECHNIQUES")
    print("="*60)
    
    core_implementation()
    banking_example()
    healthcare_example()
    
    print("\n" + "="*60)
    print("IMPLEMENTATION COMPLETE")
    print("="*60)


if __name__ == "__main__":
    main()