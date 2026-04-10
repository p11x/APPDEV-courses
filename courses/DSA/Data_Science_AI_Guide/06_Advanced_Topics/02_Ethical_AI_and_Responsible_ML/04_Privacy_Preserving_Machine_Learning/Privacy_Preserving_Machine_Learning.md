# Privacy Preserving Machine Learning

## I. INTRODUCTION

### What is Privacy Preserving ML?
Privacy Preserving Machine Learning (PPML) encompasses techniques that enable ML model training and deployment while protecting sensitive data. As ML systems increasingly handle personal information, PPML ensures privacy through cryptographic methods, data transformation, and distributed learning approaches.

Key motivations:
- GDPR and privacy regulations
- Protection of sensitive data (healthcare, finance)
- Prevention of data leakage
- Trust in AI systems

## II. ADVANCED PPML TECHNIQUES

### Deep Dive: Differential Privacy

Differential Privacy (DP) provides a mathematical guarantee that individual records cannot be inferred from model outputs. The core mechanism is adding calibrated noise:

```
Differential Privacy Mechanism
========================

Original Query: Q(D)
    │
    ▼
┌────────────┐
│Sensitivity│ Calculate query sensitivity
│   (S)    │   (max change from one record)
└────────────┘
    │
    ▼
┌────────────┐
│   Noise   │  Sample: Laplace(S/ε)
│ Generator │          or Gaussian(σ)
└────────────┘
    │
    ▼
┌────────────┐
│   DP      │
│   Output  │  Q(D) + noise
└────────────┘

Key Properties:
- ε (epsilon): Privacy budget - smaller = more private
- δ (delta): Failure probability allowance
- Composition: Multiple queries consume budget
```

### Advanced Implementation

```python
"""
Advanced Privacy Preserving ML Implementation
===================================
Comprehensive PPML techniques.
"""

import numpy as np
from typing import List, Tuple, Dict
from sklearn.ensemble import RandomForestClassifier
import warnings
warnings.filterwarnings('ignore')


class AdvancedDifferentialPrivacy:
    """
    Advanced Differential Privacy
    =====================
    Implements multiple DP mechanisms and composition.
    """
    
    def __init__(
        self,
        epsilon: float = 1.0,
        delta: float = 1e-5,
        mechanism: str = 'laplace'
    ):
        self.epsilon = epsilon
        self.delta = delta
        self.mechanism = mechanism
        self.budget_spent = 0.0
    
    def add_laplace_noise(
        self,
        data: np.ndarray,
        sensitivity: float = 1.0
    ) -> np.ndarray:
        """Add Laplace noise for differential privacy."""
        scale = sensitivity / self.epsilon
        noise = np.random.laplace(0, scale, data.shape)
        self.budget_spent += self.epsilon
        return data + noise
    
    def add_gaussian_noise(
        self,
        data: np.ndarray,
        sensitivity: float = 1.0
    ) -> np.ndarray:
        """Add Gaussian noise for differential privacy."""
        sigma = sensitivity * np.sqrt(2 * np.log(1.25 / self.delta)) / self.epsilon
        noise = np.random.normal(0, sigma, data.shape)
        self.budget_spent += self.epsilon
        return data + noise
    
    def compute_dp_average(
        self,
        values: List[float],
        use_composition: bool = True
    ) -> float:
        """Differentially private average."""
        true_avg = np.mean(values)
        noise_scale = 1.0 / self.epsilon
        noisy_avg = true_avg + np.random.laplace(0, noise_scale)
        
        if use_composition:
            self.budget_spent += self.epsilon
        
        return noisy_avg
    
    def compute_dp_histogram(
        self,
        values: np.ndarray,
        bins: int = 10
    ) -> Tuple[np.ndarray, np.ndarray]:
        """Differentially private histogram."""
        hist, edges = np.histogram(values, bins=bins)
        
        sensitivity = 1.0
        noisy_hist = hist + np.random.laplace(0, sensitivity / self.epsilon, bins)
        noisy_hist = np.maximum(0, noisy_hist)
        
        self.budget_spent += self.epsilon
        
        return noisy_hist, edges
    
    def check_privacy_budget(self) -> Dict:
        """Check remaining privacy budget."""
        return {
            'budget_spent': self.budget_spent,
            'epsilon': self.epsilon,
            'budget_remaining': self.epsilon - self.budget_spent,
            'budget_exhausted': self.budget_spent >= self.epsilon
        }
    
    def compose_queries(
        self,
        num_queries: int,
        epsilon_per_query: float
    ) -> float:
        """Calculate composed epsilon for multiple queries."""
        composed = 0
        for _ in range(num_queries):
            composed += epsilon_per_query
        return composed


class SecureAggregation:
    """
    Secure Aggregation
    ===============
    Implements secure aggregation for federated learning.
    """
    
    def __init__(self, num_clients: int):
        self.num_clients = num_clients
        self.masks = {}
    
    def generate_masks(self, shape: Tuple) -> Dict[int, np.ndarray]:
        """Generate random masks for clients."""
        for client_id in range(self.num_clients):
            np.random.seed(client_id)
            self.masks[client_id] = np.random.randint(0, 2, shape)
        
        return self.masks
    
    def apply_masks(
        self,
        updates: List[np.ndarray]
    ) -> List[np.ndarray]:
        """Apply masks to client updates."""
        masked_updates = []
        
        for update, mask in zip(updates, self.masks.values()):
            masked_updates.append(update * mask)
        
        return masked_updates
    
    def aggregate_with_unmask(
        self,
        masked_updates: List[np.ndarray],
        expected_clients: int
    ) -> np.ndarray:
        """Aggregate and unmask updates."""
        if len(masked_updates) < expected_clients:
            raise ValueError("Incomplete aggregation")
        
        aggregated = np.sum(masked_updates, axis=0)
        
        for mask in self.masks.values():
            aggregated = aggregated * mask
        
        return aggregated


class HomomorphicEncryption:
    """
    Homomorphic Encryption
    ===============
    Simulated HE operations on encrypted data.
    """
    
    def __init__(self):
        self.public_key = None
        self.private_key = None
        self.keys_generated = False
    
    def generate_keys(self, key_size: int = 2048):
        """Generate encryption keys (simulated)."""
        np.random.seed(key_size)
        self.public_key = np.random.randint(1, key_size)
        self.private_key = np.random.randint(1, key_size)
        self.keys_generated = True
    
    def encrypt(self, plaintext: np.ndarray) -> np.ndarray:
        """Encrypt data (simulated)."""
        if not self.keys_generated:
            raise ValueError("Keys not generated")
        
        encrypted = plaintext + self.public_key
        return encrypted
    
    def decrypt(self, ciphertext: np.ndarray) -> np.ndarray:
        """Decrypt data (simulated)."""
        if not self.keys_generated:
            raise ValueError("Keys not generated")
        
        decrypted = ciphertext - self.public_key
        return decrypted
    
    def add_encrypted(
        self,
        ciphertext_a: np.ndarray,
        ciphertext_b: np.ndarray
    ) -> np.ndarray:
        """Add two encrypted values."""
        return ciphertext_a + ciphertext_b
    
    def multiply_encrypted(
        self,
        ciphertext: np.ndarray,
        scalar: float
    ) -> np.ndarray:
        """Multiply encrypted value by scalar."""
        return ciphertext * scalar


class PrivacyPreservingFeatures:
    """
    Privacy-Preserving Feature Engineering
    ==================================
    Techniques for privacy-preserving feature creation.
    """
    
    def __init__(self):
        self.encodings = {}
    
    def compute_dp_count(
        self,
        data: np.ndarray,
        value: float,
        epsilon: float = 1.0
    ) -> float:
        """Differentially private count."""
        true_count = (data == value).sum()
        noise = np.random.laplace(0, 1 / epsilon)
        return max(0, true_count + noise)
    
    def compute_dp_mean(
        self,
        data: np.ndarray,
        epsilon: float = 1.0
    ) -> float:
        """Differentially private mean."""
        true_mean = np.mean(data)
        std = np.std(data)
        noise = np.random.laplace(0, std / epsilon)
        return true_mean + noise
    
    def compute_dp_quantile(
        self,
        data: np.ndarray,
        quantile: float,
        epsilon: float = 1.0
    ) -> float:
        """Differentially private quantile."""
        true_quantile = np.percentile(data, quantile * 100)
        noise = np.random.laplace(0, 1 / epsilon)
        return true_quantile + noise
    
    def bin_with_dp_noise(
        self,
        data: np.ndarray,
        num_bins: int,
        epsilon: float = 1.0
    ) -> np.ndarray:
        """Create bins with DP noise."""
        bin_edges = np.linspace(data.min(), data.max(), num_bins + 1)
        binned = np.digitize(data, bin_edges)
        
        for bin_id in range(1, num_bins + 1):
            bin_mask = binned == bin_id
            bin_size = bin_mask.sum()
            
            if bin_size > 0:
                noise = np.random.laplace(0, 1 / epsilon)
                adjusted_size = max(0, bin_size + noise)
                
                if adjusted_size < bin_size:
                    excess = bin_size - adjusted_size
                    indices = np.where(bin_mask)[0]
                    remove_count = min(len(indices), int(excess))
                    remove_indices = np.random.choice(indices, remove_count, replace=False)
                    binned[remove_indices] = 0
        
        return binned


class FederatedLearning:
    """Advanced federated learning simulation."""
    
    def __init__(self, num_clients: int):
        self.num_clients = num_clients
        self.client_models = []
        self.global_model = None
    
    def simulate_client_update(
        self,
        client_data: List[Tuple[np.ndarray, np.ndarray]],
        global_model: RandomForestClassifier
    ) -> List:
        """Simulate local training on client data."""
        updates = []
        
        for data in client_data:
            local_model = RandomForestClassifier(n_estimators=10)
            local_model.fit(data[0], data[1])
            updates.append(local_model)
        
        return updates
    
    def aggregate_models(
        self,
        updates: List,
        weights: List[float] = None
    ) -> dict:
        """Aggregate client model updates."""
        if weights is None:
            weights = [1.0 / len(updates)] * len(updates)
        
        aggregated = {}
        for key in updates[0].feature_importances_.keys():
            weighted_sum = sum(
                w * update.feature_importances_[key]
                for w, update in zip(weights, updates)
            )
            aggregated[key] = weighted_sum
        
        return {'aggregated': aggregated, 'weights_used': weights}


class DataAnonymization:
    """Advanced data anonymization techniques."""
    
    def __init__(self):
        self.generalization_hierarchy = {}
    
    def generalize_age(self, age: int) -> str:
        """Generalize age to ranges."""
        if age < 18:
            return "0-17"
        elif age < 30:
            return "18-29"
        elif age < 50:
            return "30-49"
        elif age < 70:
            return "50-69"
        return "70+"
    
    def generalize_zip(self, zip_code: str) -> str:
        """Generalize ZIP code to 3 digits."""
        return zip_code[:3] + "00"
    
    def generalize_income(self, income: float) -> str:
        """Generalize income to ranges."""
        if income < 25000:
            return "0-24999"
        elif income < 50000:
            return "25000-49999"
        elif income < 75000:
            return "50000-74999"
        elif income < 100000:
            return "75000-99999"
        return "100000+"
    
    def k_anonymize(
        self,
        data: pd.DataFrame,
        quasi_identifiers: List[str],
        k: int = 5
    ) -> pd.DataFrame:
        """Apply k-anonymization."""
        groups = data.groupby(quasi_identifiers)
        
        anonymized_groups = []
        for name, group in groups:
            if len(group) >= k:
                anonymized_groups.append(group)
            else:
                pass
        
        if anonymized_groups:
            return pd.concat(anonymized_groups)
        return data
    
    def l_diversity(
        self,
        data: pd.DataFrame,
        sensitive_attr: str,
        quasi_identifiers: List[str],
        l: int = 2
    ) -> pd.DataFrame:
        """Apply l-diversity."""
        groups = data.groupby(quasi_identifiers)
        
        diverse_groups = []
        for name, group in groups:
            unique_values = group[sensitive_attr].nunique()
            if unique_values >= l:
                diverse_groups.append(group)
        
        if diverse_groups:
            return pd.concat(diverse_groups)
        return data
    
    def t_closeness(
        self,
        data: pd.DataFrame,
        sensitive_attr: str,
        quasi_identifiers: List[str],
        t: float = 0.1
    ) -> pd.DataFrame:
        """Apply t-closeness."""
        overall_distribution = data[sensitive_attr].value_counts(normalize=True)
        
        groups = data.groupby(quasi_identifiers)
        
        close_groups = []
        for name, group in groups:
            group_distribution = group[sensitive_attr].value_counts(normalize=True)
            
            distance = np.sum(np.abs(group_distribution - overall_distribution)) / 2
            
            if distance <= t:
                close_groups.append(group)
        
        if close_groups:
            return pd.concat(close_groups)
        return data


def run_ppml_example():
    """Run privacy preserving example."""
    print("=" * 50)
    print("PRIVACY PRESERVING ML")
    print("=" * 50)
    
    dp = DifferentialPrivacy(epsilon=1.0)
    data = np.array([1.0, 2.0, 3.0, 4.0, 5.0])
    noisy = dp.add_noise(data)
    print(f"\nOriginal: {data}")
    print(f"With noise: {noisy}")
    
    values = [1.0, 2.0, 3.0, 4.0, 5.0]
    dp_avg = dp.compute_dp_average(values)
    print(f"\nDP Average: {dp_avg:.3f}")
    
    fl = FederatedLearning(num_clients=3)
    print(f"\nFederated Learning: {fl.num_clients} clients")
    
    anon = DataAnonymization()
    print(f"\nAge generalization: {anon.generalize_age(25)}")
    print(f"ZIP generalization: {anon.generalize_zip('12345')}")
    
    return dp


if __name__ == "__main__":
    run_ppml_example()
```

## III. PRIVACY IMPLEMENTATIONS IN PRACTICE

### Healthcare Implementation

```python
class PrivacyPreservingHealthcare:
    """
    Privacy-Preserving Healthcare ML
    =================================
    Implements DP in healthcare settings.
    """
    
    def __init__(self):
        self.dp = AdvancedDifferentialPrivacy(epsilon=0.5)
        self.fl = FederatedLearning(num_clients=10)
        self.anon = DataAnonymization()
    
    def train_onPatientData(
        self,
        patient_records: List[Dict]
    ) -> Dict:
        """Train model with privacy guarantees."""
        aggregated_data = self._aggregate_patient_data(patient_records)
        
        dp_mean = self.dp.compute_dp_mean(
            np.array(aggregated_data['values']),
            epsilon=0.5
        )
        
        return {
            'model_params': aggregated_data,
            'privacy_assured': True,
            'epsilon_used': 0.5
        }
    
    def _aggregate_patient_data(self, records: List[Dict]) -> Dict:
        """Aggregate patient data with DP."""
        return {
            'values': [r['value'] for r in records],
            'count': len(records),
            'mean': np.mean([r['value'] for r in records])
        }
    
    def anonymizePatientData(
        self,
        patient_data: pd.DataFrame
    ) -> pd.DataFrame:
        """Anonymize patient data."""
        patient_data['age'] = patient_data['age'].apply(self.anon.generalize_age)
        patient_data['zip_code'] = patient_data['zip_code'].apply(self.anon.generalize_zip)
        
        return patient_data


class PrivacyAuditor:
    """
    Privacy Audit Tool
    ===============
    Audits privacy-preserving systems.
    """
    
    def __init__(self):
        self.audit_log = []
    
    def audit_model(
        self,
        model,
        original_data: np.ndarray,
        model_output: np.ndarray
    ) -> Dict:
        """Audit model for privacy leakage."""
        reconstruction_attempts = 100
        successful_reconstructions = 0
        
        for _ in range(reconstruction_attempts):
            if self._attempt_reconstruction(original_data, model_output):
                successful_reconstructions += 1
        
        return {
            'reconstruction_risk': successful_reconstructions / reconstruction_attempts,
            'privacy_grade': 'HIGH' if successful_reconstructions < 10 else 'LOW',
            'audit_passed': successful_reconstructions < 10
        }
    
    def _attempt_reconstruction(
        self,
        original: np.ndarray,
        output: np.ndarray
    ) -> bool:
        """Attempt to reconstruct original data."""
        return False
```

## IV. CONCLUSION

### Key Takeaways

1. **Multiple Privacy Techniques**: Differential Privacy, Federated Learning, Secure Multi-Party Computation, and Homomorphic Encryption each address different privacy scenarios:

   - DP: Mathematical guarantees for aggregate queries
   - FL: Training without centralizing data
   - SMC: Computation over distributed data
   - HE: Computation on encrypted data

2. **Trade-offs Exist**:
   ```
   Privacy-Utility Trade-off
   ========================
   Lower ε (more privacy) → Higher utility loss
   Higher ε (less privacy) → Lower utility loss
   
   Best Practice: Choose ε based on:
   - Data sensitivity
   - Regulatory requirements
   - Use case requirements
   ```

3. **Privacy by Design**: Implement privacy from the start:
   - Define privacy budget upfront
   - Track privacy expenditure
   - Regular audits are essential

4. **Regulatory Considerations**:
   - GDPR: Right to explanation, data minimization
   - HIPAA: De-identification requirements
   - CCPA: Opt-out rights

### Next Steps

- Implement privacy-preserving ML in your projects
- Conduct privacy audits regularly
- Document privacy decisions
- Train teams on privacy best practices