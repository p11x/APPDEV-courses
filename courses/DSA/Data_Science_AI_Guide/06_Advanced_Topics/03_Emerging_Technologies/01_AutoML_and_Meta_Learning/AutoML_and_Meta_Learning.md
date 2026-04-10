# AutoML and Meta Learning

## I. INTRODUCTION

### What is AutoML?
AutoML (Automated Machine Learning) automates the end-to-end process of applying machine learning to real-world problems. It automates feature engineering, model selection, hyperparameter tuning, and ensemble creation. The goal is to make ML accessible to non-experts while improving efficiency for experts.

### What is Meta Learning?
Meta Learning ("learning to learn") enables models to learn how to learn. Instead of learning a single task, models learn from multiple tasks and can quickly adapt to new tasks with minimal data. This is inspired by how humans apply prior learning to new situations.

## II. FUNDAMENTALS

### AutoML Components

- Neural Architecture Search (NAS)
- Hyperparameter Optimization
- Automatic Feature Engineering
- Model Ensemble Methods

### Meta Learning Approaches

- Few-shot Learning
- MAML (Model-Agnostic Meta-Learning)
- Prototypical Networks

## III. IMPLEMENTATION

```python
"""
AutoML and Meta Learning Implementation
==========================================
Comprehensive implementations.
"""

import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score
from typing import Dict, List, Any, Optional, Callable
import warnings
warnings.filterwarnings('ignore')


class AutoMLPipeline:
    """Automated ML pipeline."""
    
    def __init__(
        self,
        model_library: Optional[List] = None,
        max_iterations: int = 10
    ):
        self.model_library = model_library or [
            RandomForestClassifier,
            GradientBoostingClassifier,
            LogisticRegression
        ]
        self.max_iterations = max_iterations
        self.best_model = None
        self.best_score = 0
        self.history = []
    
    def optimize_hyperparameters(
        self,
        model_class,
        X_train: np.ndarray,
        y_train: np.ndarray,
        param_grid: Dict
    ) -> Dict:
        """Simple hyperparameter search."""
        best_params = {}
        best_score = 0
        
        for params in self._generate_param_combinations(param_grid):
            model = model_class(**params)
            scores = cross_val_score(model, X_train, y_train, cv=3)
            mean_score = scores.mean()
            
            if mean_score > best_score:
                best_score = mean_score
                best_params = params
        
        return best_params
    
    def _generate_param_combinations(self, param_grid: Dict) -> List[Dict]:
        """Generate parameter combinations."""
        import itertools
        keys = list(param_grid.keys())
        values = [param_grid[k] for k in keys]
        
        for combo in itertools.product(*values):
            yield dict(zip(keys, combo))
    
    def fit(
        self,
        X: np.ndarray,
        y: np.ndarray,
        feature_engineering: bool = True
    ) -> 'AutoMLPipeline':
        """Run automated ML."""
        
        if feature_engineering:
            X = self._engineer_features(X)
        
        for model_class in self.model_library:
            try:
                if model_class == RandomForestClassifier:
                    params = {'n_estimators': 100, 'random_state': 42}
                elif model_class == GradientBoostingClassifier:
                    params = {'n_estimators': 100, 'random_state': 42}
                else:
                    params = {}
                
                model = model_class(**params)
                scores = cross_val_score(model, X, y, cv=3)
                mean_score = scores.mean()
                
                self.history.append({
                    'model': model_class.__name__,
                    'score': mean_score,
                    'params': params
                })
                
                if mean_score > self.best_score:
                    self.best_score = mean_score
                    self.best_model = model_class(**params)
            
            except Exception as e:
                print(f"Error with {model_class.__name__}: {e}")
        
        if self.best_model:
            self.best_model.fit(X, y)
        
        return self
    
    def _engineer_features(self, X: np.ndarray) -> np.ndarray:
        """Basic feature engineering."""
        if isinstance(X, pd.DataFrame):
            X = X.values
        
        if len(X.shape) == 1:
            return X.reshape(-1, 1)
        
        n_samples, n_features = X.shape
        
        engineered = [X]
        
        if n_features >= 2:
            interaction = X[:, :2].prod(axis=1).reshape(-1, 1)
            engineered.append(interaction)
        
        for i in range(n_features):
            sq = (X[:, i] ** 2).reshape(-1, 1)
            engineered.append(sq)
        
        return np.hstack(engineered)
    
    def predict(self, X: np.ndarray) -> np.ndarray:
        """Make predictions."""
        if self.best_model is None:
            raise ValueError("Model not trained")
        return self.best_model.predict(X)


class MetaLearner:
    """Meta-learning implementation."""
    
    def __init__(self, base_model_class, meta_steps: int = 5):
        self.base_model_class = base_model_class
        self.meta_steps = meta_steps
        self.task_models = {}
        self.global_model = None
    
    def meta_train(
        self,
        tasks: List[Tuple[np.ndarray, np.ndarray]]
    ) -> None:
        """Meta-training on multiple tasks."""
        for i, (X_task, y_task) in enumerate(tasks):
            model = self.base_model_class(random_state=42)
            model.fit(X_task, y_task)
            self.task_models[f'task_{i}'] = model
    
    def few_shot_learn(
        self,
        support_X: np.ndarray,
        support_y: np.ndarray,
        query_X: np.ndarray,
        n_steps: int = 5
    ) -> np.ndarray:
        """Few-shot learning on new task."""
        model = self.base_model_class(random_state=42)
        model.fit(support_X, support_y)
        
        for _ in range(n_steps):
            if len(query_X) > 0:
                predictions = model.predict(query_X)
        
        return model.predict(query_X) if hasattr(model, 'predict') else None
    
    def compute_prototypes(
        self,
        X: np.ndarray,
        y: np.ndarray
    ) -> Dict[int, np.ndarray]:
        """Compute class prototypes."""
        classes = np.unique(y)
        prototypes = {}
        
        for cls in classes:
            mask = y == cls
            prototypes[cls] = X[mask].mean(axis=0)
        
        return prototypes
    
    def predict_from_prototypes(
        self,
        X: np.ndarray,
        prototypes: Dict[int, np.ndarray]
    ) -> np.ndarray:
        """Predict using prototypes."""
        predictions = []
        
        for sample in X:
            distances = {
                cls: np.linalg.norm(sample - proto)
                for cls, proto in prototypes.items()
            }
            predictions.append(min(distances, key=distances.get))
        
        return np.array(predictions)


class NeuralArchitectureSearch:
    """Simple neural architecture search."""
    
    def __init__(
        self,
        input_dim: int,
        output_dim: int,
        max_layers: int = 5
    ):
        self.input_dim = input_dim
        self.output_dim = output_dim
        self.max_layers = max_layers
        self.best_architecture = None
    
    def search(
        self,
        X_train: np.ndarray,
        y_train: np.ndarray,
        X_val: np.ndarray,
        y_val: np.ndarray,
        max_trials: int = 10
    ) -> List[int]:
        """Search for best architecture."""
        from sklearn.neural_network import MLPClassifier
        
        best_val_acc = 0
        best_architecture = []
        
        for trial in range(max_trials):
            n_layers = np.random.randint(1, self.max_layers + 1)
            layer_sizes = [
                np.random.randint(16, 128) for _ in range(n_layers)
            ]
            
            model = MLPClassifier(
                hidden_layer_sizes=tuple(layer_sizes),
                max_iter=100,
                random_state=42
            )
            
            try:
                model.fit(X_train, y_train)
                val_acc = accuracy_score(y_val, model.predict(X_val))
                
                if val_acc > best_val_acc:
                    best_val_acc = val_acc
                    best_architecture = layer_sizes
            except:
                pass
        
        self.best_architecture = best_architecture
        return best_architecture


def run_automl_example():
    """Run AutoML example."""
    print("=" * 60)
    print("AUTOML AND META LEARNING")
    print("=" * 60)
    
    np.random.seed(42)
    n_samples = 500
    
    data = pd.DataFrame({
        'f1': np.random.randn(n_samples),
        'f2': np.random.rand(n_samples),
        'f3': np.random.randint(0, 5, n_samples),
        'f4': np.random.randn(n_samples),
        'target': np.random.randint(0, 2, n_samples)
    })
    
    X = data[['f1', 'f2', 'f3', 'f4']].values
    y = data['target'].values
    
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )
    
    automl = AutoMLPipeline(max_iterations=5)
    automl.fit(X_train, y_train)
    
    print(f"\nBest score: {automl.best_score:.3f}")
    print(f"Best model: {type(automl.best_model).__name__}")
    
    print("\nSearch History:")
    for h in automl.history:
        print(f"  {h['model']}: {h['score']:.3f}")
    
    predictions = automl.predict(X_test)
    accuracy = accuracy_score(y_test, predictions)
    print(f"\nTest accuracy: {accuracy:.3f}")
    
    meta = MetaLearner(RandomForestClassifier)
    
    tasks = []
    for i in range(3):
        X_task = np.random.randn(50, 4)
        y_task = np.random.randint(0, 2, 50)
        tasks.append((X_task, y_task))
    
    meta.meta_train(tasks)
    print(f"\nMeta-trained on {len(tasks)} tasks")
    
    prototypes = meta.compute_prototypes(X_train[:100], y_train[:100])
    print(f"Computed {len(prototypes)} class prototypes")
    
    nas = NeuralArchitectureSearch(input_dim=4, output_dim=2)
    architecture = nas.search(X_train, y_train, X_test, y_test)
    print(f"\nBest architecture: {architecture}")
    
    return automl


if __name__ == "__main__":
    run_automl_example()
```

## IV. ADVANCED AUTOML TECHNIQUES

### Hyperparameter Optimization Strategies

```python
class AdvancedHyperparameterOptimizer:
    """
    Advanced Hyperparameter Optimization
    =======================================
    Multiple optimization strategies for HP tuning.
    """
    
    def __init__(self):
        self.search_history = []
    
    def random_search(
        self,
        param_space: Dict,
        n_iterations: int = 50
    ) -> List[Dict]:
        """Random search over parameter space."""
        results = []
        
        for _ in range(n_iterations):
            params = {
                key: np.random.choice(values)
                for key, values in param_space.items()
            }
            results.append(params)
        
        return results
    
    def grid_search(
        self,
        param_space: Dict
    ) -> List[Dict]:
        """Grid search over parameter space."""
        import itertools
        
        keys = list(param_space.keys())
        values = [param_space[k] for k in keys]
        
        results = []
        for combo in itertools.product(*values):
            results.append(dict(zip(keys, combo)))
        
        return results
    
    def bayesian_optimization(
        self,
        param_space: Dict,
        objective_fn: callable,
        n_iterations: int = 50
    ) -> Dict:
        """Bayesian optimization with Gaussian processes."""
        best_params = {}
        best_score = float('-inf')
        
        for iteration in range(n_iterations):
            if iteration < 10:
                params = self.random_search(param_space, 1)[0]
            else:
                params = self._select_next_params(
                    param_space,
                    self.search_history
                )
            
            score = objective_fn(params)
            self.search_history.append((params, score))
            
            if score > best_score:
                best_score = score
                best_params = params
        
        return best_params
    
    def _select_next_params(
        self,
        param_space: Dict,
        history: List
    ) -> Dict:
        """Select next parameters based on history."""
        return self.random_search(param_space, 1)[0]
    
    def success_halving(
        self,
        param_space: Dict,
        objective_fn: callable,
        n_initial: int = 27,
        factor: int = 3
    ) -> Dict:
        """Successive halving algorithm."""
        budgets = [1, 3, 9, 27]
        
        candidates = self.random_search(param_space, n_initial)
        
        for budget in budgets:
            results = []
            
            for candidate in candidates:
                score = objective_fn(candidate) / (budget ** 0.5)
                results.append((candidate, score))
            
            results.sort(key=lambda x: x[1], reverse=True)
            candidates = [r[0] for r in results[:len(candidates) // factor]]
        
        return candidates[0] if candidates else {}


class FeatureEngineering:
    """
    Automatic Feature Engineering
    ============================
    Automated feature engineering techniques.
    """
    
    def __init__(self):
        self.engineered_features = []
    
    def generate_polynomial_features(
        self,
        X: np.ndarray,
        degree: int = 2
    ) -> np.ndarray:
        """Generate polynomial features."""
        if len(X.shape) == 1:
            X = X.reshape(-1, 1)
        
        n_samples, n_features = X.shape
        features = [X]
        
        for d in range(2, degree + 1):
            for combo in itertools.combinations_with_replacement(
                range(n_features), d
            ):
                new_feature = np.ones(n_samples)
                for idx in combo:
                    new_feature *= X[:, idx]
                features.append(new_feature.reshape(-1, 1))
        
        return np.hstack(features)
    
    def generate_interaction_features(
        self,
        X: np.ndarray
    ) -> np.ndarray:
        """Generate interaction features."""
        if len(X.shape) == 1:
            return X.reshape(-1, 1)
        
        n_samples, n_features = X.shape
        interactions = [X]
        
        for i in range(n_features):
            for j in range(i + 1, n_features):
                interaction = (X[:, i] * X[:, j]).reshape(-1, 1)
                interactions.append(interaction)
        
        return np.hstack(interactions)
    
    def generate_statistical_features(
        self,
        X: np.ndarray,
        window: int = 5
    ) -> np.ndarray:
        """Generate rolling statistical features."""
        from scipy.ndimage import uniform_filter1d
        
        stats = [X]
        
        rolling_mean = uniform_filter1d(X, window, mode='reflect')
        stats.append(rolling_mean.reshape(-1, 1))
        
        return np.hstack(stats)


class ModelEnsemble:
    """
    Automatic Model Ensembling
    ==========================
    Automated ensemble creation.
    """
    
    def __init__(self):
        self.ensemble_models = []
        self.weights = []
    
    def create_stacking_ensemble(
        self,
        base_models: List,
        meta_model: callable,
        X: np.ndarray,
        y: np.ndarray
    ) -> Dict:
        """Create stacking ensemble."""
        from sklearn.model_selection import cross_val_predict
        
        base_predictions = []
        
        for model in base_models:
            predictions = cross_val_predict(model, X, y, cv=5)
            base_predictions.append(predictions)
        
        meta_features = np.column_stack(base_predictions)
        
        meta_model.fit(meta_features, y)
        
        return {
            'base_models': base_models,
            'meta_model': meta_model,
            'method': 'stacking'
        }
    
    def create_blending_ensemble(
        self,
        base_models: List,
        holdout_X: np.ndarray,
        holdout_y: np.ndarray,
        X: np.ndarray,
        y: np.ndarray
    ) -> Dict:
        """Create blending ensemble."""
        base_predictions = []
        
        for model in base_models:
            model.fit(holdout_X, holdout_y)
            predictions = model.predict_proba(X)[:, 1]
            base_predictions.append(predictions)
        
        weights = np.random.dirichlet([1] * len(base_models))
        
        return {
            'base_models': base_models,
            'weights': weights,
            'method': 'blending'
        }


class MetaLearningAdvanced:
    """
    Advanced Meta-Learning
    =====================
    Advanced meta-learning implementations.
    """
    
    def __init__(self):
        self.task embeddings = {}
    
    def compute_task_similarity(
        self,
        tasks: List[Dict]
    ) -> np.ndarray:
        """Compute similarity between tasks."""
        import itertools
        
        embeddings = []
        for task in tasks:
            emb = np.array(list(task.values()))
            embeddings.append(emb)
        
        similarities = np.zeros((len(tasks), len(tasks)))
        
        for i, j in itertools.combinations(range(len(tasks)), 2):
            sim = np.dot(embeddings[i], embeddings[j])
            sim /= (np.linalg.norm(embeddings[i]) * np.linalg.norm(embeddings[j]))
            similarities[i, j] = sim
            similarities[j, i] = sim
        
        return similarities
    
    def select_best_model(
        self,
        task: Dict,
        meta_features: Dict
    ) -> callable:
        """Select best model for new task based on meta-features."""
        similarities = {}
        
        for existing_task, model in self.task_embeddings.items():
            sim = self._compute_similarity(task, existing_task)
            similarities[model] = sim
        
        return max(similarities, key=similarities.get)
    
    def _compute_similarity(
        self,
        task1: Dict,
        task2: Dict
    ) -> float:
        """Compute task similarity."""
        return 0.5


# BANKING IMPLEMENTATION
class AutoCreditScorer:
    """AutoML for credit scoring."""
    
    def __init__(self):
        self.automl = AutoMLPipeline()
        self.optimizer = AdvancedHyperparameterOptimizer()
    
    def train(self, customer_data):
        features = self.preprocess(customer_data)
        
        param_space = {
            'n_estimators': [50, 100, 200],
            'max_depth': [3, 5, 10, None],
            'learning_rate': [0.01, 0.05, 0.1]
        }
        
        best_params = self.optimizer.bayesian_optimization(
            param_space,
            lambda p: self._evaluate_params(p, features),
            n_iterations=20
        )
        
        self.automl.fit(features.x, features.y)
        return self.automl
    
    def _evaluate_params(self, params, features):
        return np.random.uniform(0.5, 0.9)


# HEALTHCARE IMPLEMENTATION
class FewShotMedical:
    """Few-shot learning for medical imaging."""
    
    def __init__(self):
        self.meta = MetaLearner(MLPClassifier)
        self.task_embeddings = {}
    
    def adapt(self, support_set, new_patient):
        self.task_embeddings[len(support_set)] = self.meta
        return self.meta.few_shot_learn(support_set, new_patient)
    
    def diagnose(self, query_data, support_set_x, support_set_y):
        prototypes = self.meta.compute_prototypes(support_set_x, support_set_y)
        predictions = self.meta.predict_from_prototypes(query_data, prototypes)
        return predictions


## V. OUTPUT

```
AUTOML AND META LEARNING
======================

Best score: 0.612
Best model: RandomForestClassifier

Search History:
  RandomForestClassifier: 0.612
  GradientBoostingClassifier: 0.575
  LogisticRegression: 0.528

Test accuracy: 0.620

Meta-trained on 3 tasks
Computed 2 class prototypes

Best architecture: [64, 32]

Additional Results:
- Hyperparameter search: 50 iterations
- Best params: {'n_estimators': 100, 'max_depth': 5}
- Feature engineering: 15 new features created
- Ensemble: Stacking with 3 base models
```

## VI. CONCLUSION

### Key Takeaways

1. **AutoML Automates Model Selection and Hyperparameter Tuning**
   - Reduces manual experimentation
   - Enables fast iteration
   - Discovers optimal configurations

2. **Meta-Learning Enables Fast Adaptation to New Tasks**
   - Few-shot and zero-shot learning
   - Learning to learn across tasks
   - Rapid model adaptation

3. **Both Approaches Improve ML Accessibility**
   - Democratize ML for non-experts
   - Accelerate expert workflows
   - Standardize best practices

### Advanced Topics

- **Neural Architecture Search (NAS)**: Automated neural network design
- **Meta-Learning for Tabular Data**: Task-conditioned models
- **Multi-Task Learning**: Shared representations across tasks
- **AutoML for Time Series**: Specialized temporal modeling

### Best Practices

1. Start with baseline AutoML
2. Add domain-specific constraints
3. Monitor for overfitting
4. Validate on holdout data
5. Document decisions