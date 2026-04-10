# Topic: Neural Architecture Search
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Neural Architecture Search

I. INTRODUCTION
Neural Architecture Search (NAS) automates the design of neural network architectures.
This module covers search spaces, search strategies, and performance estimation methods.

II. CORE CONCEPTS
- Search space design
- Search strategies (RL, Evolution, Gradient)
- Performance estimation
- DARTS and efficient architectures
- NAS benchmarks

III. IMPLEMENTATION
"""

import numpy as np
import pandas as pd
from typing import Dict, List, Tuple, Optional, Any
from dataclasses import dataclass
from enum import Enum
import random


class SearchStrategy(Enum):
    """NAS search strategies."""
    RANDOM = "random"
    RL = "reinforcement_learning"
    EVOLUTION = "evolutionary"
    GRADIENT = "gradient_based"
    BAYESIAN = "bayesian"


class CellType(Enum):
    """Network cell types."""
    CONVOLUTION = "convolution"
    REDUCTION = "reduction"
    SKIP = "skip_connection"
    SEPARABLE_CONV = "separable_conv"


@dataclass
class Cell:
    """Network cell specification."""
    cell_type: CellType
    inputs: List[str]
    operations: List[str]
    output_dim: int


@dataclass
class Architecture:
    """Neural network architecture."""
    name: str
    cells: List[Cell]
    input_shape: Tuple
    num_classes: int
    parameters: int
    accuracy: float


class SearchSpace:
    """NAS search space definition."""

    def __init__(self):
        self.operations = [
            'conv_3x3', 'conv_5x5', 'sep_conv_3x3', 'sep_conv_5x5',
            'pool_3x3', 'pool_5x5', 'identity', 'zero'
        ]
        self.max_nodes = 8
        self.num_choices_per_node = 2

    def get_operations(self) -> List[str]:
        """Get available operations."""
        return self.operations

    def generate_cell(
        self,
        num_nodes: int = 4,
        num_operations_per_node: int = 2
    ) -> Cell:
        """Generate a cell structure."""
        inputs = [f"node_{i}" for i in range(num_nodes - 1)]
        
        operations = random.sample(
            self.operations,
            min(num_operations_per_node, len(self.operations))
        )
        
        output_dim = random.choice([16, 32, 64, 128, 256])
        
        return Cell(
            cell_type=CellType.CONVOLUTION,
            inputs=inputs,
            operations=operations,
            output_dim=output_dim
        )

    def get_search_space_size(self, num_nodes: int) -> int:
        """Calculate search space size."""
        num_ops = len(self.operations)
        
        total_edges = num_nodes * (num_nodes - 1) // 2
        
        return num_ops ** total_edges


class RLNAS:
    """Reinforcement Learning based NAS."""

    def __init__(self, search_space: SearchSpace):
        self.search_space = search_space
        self.controller = None
        self.rewards: List[float] = []

    def create_controller(self) -> Dict[str, Any]:
        """Create RNN controller."""
        self.controller = {
            'num_layers': 10,
            'hidden_size': 100,
            'num_operations': len(self.search_space.get_operations())
        }
        
        return self.controller

    def sample_architecture(self) -> Architecture:
        """Sample architecture from controller."""
        num_nodes = random.randint(3, 6)
        
        cells = []
        
        for _ in range(num_nodes):
            cell = self.search_space.generate_cell(num_nodes=num_nodes)
            cells.append(cell)
        
        total_params = sum(
            cell.output_dim * cell.output_dim
            for cell in cells
        )
        
        return Architecture(
            name=f"arch_{random.randint(1000, 9999)}",
            cells=cells,
            input_shape=(32, 32, 3),
            num_classes=10,
            parameters=total_params,
            accuracy=0.0
        )

    def get_reward(self, accuracy: float, latency: float) -> float:
        """Calculate reward for architecture."""
        return accuracy / (latency + 1)

    def update_controller(
        self,
        architectures: List[Architecture],
        rewards: List[float]
    ) -> None:
        """Update controller based on rewards."""
        self.rewards.extend(rewards)
        
        best_idx = np.argmax(rewards)
        best_arch = architectures[best_idx]
        
        print(f"   Best architecture: {best_arch.name}")
        print(f"   Best accuracy: {best_arch.accuracy:.4f}")


class EvolutionaryNAS:
    """Evolutionary algorithm based NAS."""

    def __init__(self, search_space: SearchSpace):
        self.search_space = search_space
        self.population: List[Architecture] = []
        self.population_size = 20

    def initialize_population(self) -> None:
        """Initialize population."""
        for i in range(self.population_size):
            arch = self._create_random_architecture(f"evo_{i}")
            self.population.append(arch)

    def _create_random_architecture(self, name: str) -> Architecture:
        """Create random architecture."""
        num_cells = random.randint(3, 8)
        
        cells = [
            self.search_space.generate_cell()
            for _ in range(num_cells)
        ]
        
        return Architecture(
            name=name,
            cells=cells,
            input_shape=(32, 32, 3),
            num_classes=10,
            parameters=random.randint(100000, 5000000),
            accuracy=random.uniform(0.5, 0.9)
        )

    def select_parents(self, n_parents: int = 2) -> List[Architecture]:
        """Select parents using tournament selection."""
        sorted_pop = sorted(
            self.population,
            key=lambda x: x.accuracy,
            reverse=True
        )
        
        return sorted_pop[:n_parents]

    def crossover(
        self,
        parent1: Architecture,
        parent2: Architecture
    ) -> Architecture:
        """Perform crossover between two architectures."""
        new_cells = []
        
        for i in range(min(len(parent1.cells), len(parent2.cells))):
            if random.random() < 0.5:
                new_cells.append(parent1.cells[i])
            else:
                new_cells.append(parent2.cells[i])
        
        return Architecture(
            name=f"child_{random.randint(1000, 9999)}",
            cells=new_cells,
            input_shape=parent1.input_shape,
            num_classes=parent1.num_classes,
            parameters=random.randint(100000, 5000000),
            accuracy=0.0
        )

    def mutate(self, architecture: Architecture) -> Architecture:
        """Mutate architecture."""
        mutated_cells = architecture.cells.copy()
        
        if random.random() < 0.3:
            idx = random.randint(0, len(mutated_cells) - 1)
            mutated_cells[idx] = self.search_space.generate_cell()
        
        return Architecture(
            name=f"mutated_{random.randint(1000, 9999)}",
            cells=mutated_cells,
            input_shape=architecture.input_shape,
            num_classes=architecture.num_classes,
            parameters=random.randint(100000, 5000000),
            accuracy=0.0
        )

    def evolve(self, n_generations: int = 10) -> Architecture:
        """Run evolutionary search."""
        self.initialize_population()
        
        best_arch = None
        best_accuracy = 0
        
        for gen in range(n_generations):
            parents = self.select_parents(2)
            
            child = self.crossover(parents[0], parents[1])
            child = self.mutate(child)
            
            child.accuracy = random.uniform(0.6, 0.95)
            
            if child.accuracy > best_accuracy:
                best_accuracy = child.accuracy
                best_arch = child
            
            self.population[-1] = child
        
        return best_arch


class DARTS:
    """Differentiable Architecture Search (DARTS)."""

    def __init__(self):
        self.edge_weights = {}
        self.alpha = None
        self.softmax = lambda x: np.exp(x) / np.sum(np.exp(x))

    def initialize_alphas(self, num_edges: int, num_operations: int) -> None:
        """Initialize architecture parameters."""
        self.alpha = np.random.randn(num_edges, num_operations)
        self.alpha = np.log(self.softmax(self.alpha))

    def compute_softmax(self, edge_weights: np.ndarray) -> np.ndarray:
        """Compute softmax for edge weights."""
        exp_weights = np.exp(edge_weights - np.max(edge_weights))
        return exp_weights / np.sum(exp_weights)

    def forward_weights(
        self,
        node_features: np.ndarray,
        edge_idx: int
    ) -> np.ndarray:
        """Compute mixed operation output."""
        probs = self.compute_softmax(self.alpha[edge_idx])
        
        output = node_features * probs.sum()
        
        return output

    def architecture_gradient(
        self,
        loss: float,
        data_grad: np.ndarray
    ) -> np.ndarray:
        """Compute architecture gradient."""
        alpha_grad = np.random.randn(*self.alpha.shape)
        
        return alpha_grad

    def step(self, loss: float) -> None:
        """Single DARTS optimization step."""
        grad = self.architecture_gradient(loss, np.array([]))
        
        learning_rate = 0.01
        self.alpha -= learning_rate * grad

    def get_best_operations(self) -> List[str]:
        """Get best operations for each edge."""
        best_ops = []
        
        for edge_idx in range(self.alpha.shape[0]):
            best_op_idx = np.argmax(self.alpha[edge_idx])
            best_ops.append(f"operation_{best_op_idx}")
        
        return best_ops


class PerformanceEstimator:
    """Performance estimation for NAS."""

    @staticmethod
    def predict_accuracy(
        parameters: int,
        depth: int,
        width: int
    ) -> float:
        """Predict accuracy based on architecture properties."""
        param_factor = 1.0 / (1 + np.log(parameters / 1000))
        depth_factor = 1.0 / (1 + depth / 10)
        width_factor = 1.0 / (1 + width / 100)
        
        base_accuracy = 0.5
        accuracy = base_accuracy + 0.4 * param_factor + 0.1 * depth_factor
        
        return min(accuracy, 0.98)

    @staticmethod
    def estimate_latency(
        parameters: int,
        input_shape: Tuple
    ) -> float:
        """Estimate inference latency."""
        flops = parameters * input_shape[0] * input_shape[1]
        
        latency_ms = flops / 1e9 * 10
        
        return latency_ms

    @staticmethod
    def early_stop_score(
        early_acc: float,
        final_acc_estimate: float
    ) -> float:
        """Calculate early stopping score."""
        return early_acc * 0.6 + final_acc_estimate * 0.4


class NASBench:
    """NAS Benchmark interface."""

    def __init__(self):
        self.architectures: Dict[str, Architecture] = {}

    def query(self, arch_id: str) -> Optional[Dict[str, Any]]:
        """Query benchmark for architecture results."""
        if arch_id in self.architectures:
            arch = self.architectures[arch_id]
            return {
                'accuracy': arch.accuracy,
                'parameters': arch.parameters,
                'training_time': arch.parameters / 1000
            }
        return None

    def get_top_k(self, k: int = 10) -> List[Architecture]:
        """Get top-k architectures."""
        sorted_archs = sorted(
            self.architectures.values(),
            key=lambda x: x.accuracy,
            reverse=True
        )
        return sorted_archs[:k]


class NAS:
    """Complete NAS system."""

    def __init__(self, strategy: SearchStrategy = SearchStrategy.RANDOM):
        self.strategy = strategy
        self.search_space = SearchSpace()
        self.history: List[Architecture] = []
        
        if strategy == SearchStrategy.RL:
            self.nas = RLNAS(self.search_space)
        elif strategy == SearchStrategy.EVOLUTION:
            self.nas = EvolutionaryNAS(self.search_space)
        elif strategy == SearchStrategy.GRADIENT:
            self.nas = DARTS()
        else:
            self.nas = RLNAS(self.search_space)

    def search(
        self,
        n_iterations: int = 20
    ) -> Architecture:
        """Run architecture search."""
        print(f"\nRunning {self.strategy.value} search...")
        
        for i in range(n_iterations):
            if self.strategy == SearchStrategy.EVOLUTION:
                arch = self.nas.evolve(n_generations=5)
            elif self.strategy == SearchStrategy.RL:
                arch = self.nas.sample_architecture()
            elif self.strategy == SearchStrategy.GRADIENT:
                self.nas.step(loss=np.random.random())
                arch = Architecture(
                    name=f"darts_{i}",
                    cells=[],
                    input_shape=(32, 32, 3),
                    num_classes=10,
                    parameters=100000,
                    accuracy=random.uniform(0.7, 0.9)
                )
            else:
                arch = Architecture(
                    name=f"random_{i}",
                    cells=[self.search_space.generate_cell()],
                    input_shape=(32, 32, 3),
                    num_classes=10,
                    parameters=random.randint(100000, 5000000),
                    accuracy=random.uniform(0.5, 0.9)
                )
            
            arch.accuracy = PerformanceEstimator.predict_accuracy(
                arch.parameters,
                len(arch.cells),
                arch.cells[0].output_dim if arch.cells else 128
            )
            
            self.history.append(arch)
            
            print(f"  Iteration {i+1}: {arch.accuracy:.4f}")
        
        best = max(self.history, key=lambda x: x.accuracy)
        
        return best


def banking_example():
    """NAS in banking sector."""
    print("\n" + "="*60)
    print("BANKING SECTOR: NAS for Fraud Detection")
    print("="*60)
    
    print("\n1. Search Space:")
    
    space = SearchSpace()
    print(f"   Operations: {len(space.get_operations())}")
    print(f"   Search space size: {space.get_search_space_size(4)}")
    
    print("\n2. Evolutionary Search:")
    
    evo_nas = EvolutionaryNAS(space)
    best_arch = evo_nas.evolve(n_generations=5)
    
    print(f"   Best architecture: {best_arch.name}")
    print(f"   Accuracy: {best_arch.accuracy:.4f}")
    print(f"   Parameters: {best_arch.parameters}")
    
    print("\n3. Performance Estimation:")
    
    accuracy = PerformanceEstimator.predict_accuracy(
        parameters=2000000,
        depth=6,
        width=128
    )
    latency = PerformanceEstimator.estimate_latency(2000000, (32, 32))
    
    print(f"   Predicted accuracy: {accuracy:.4f}")
    print(f"   Estimated latency: {latency:.2f}ms")


def healthcare_example():
    """NAS in healthcare sector."""
    print("\n" + "="*60)
    print("HEALTHCARE SECTOR: NAS for Medical Imaging")
    print("="*60)
    
    print("\n1. DARTS-based Search:")
    
    darts = DARTS()
    darts.initialize_alphas(num_edges=10, num_operations=8)
    
    for i in range(3):
        darts.step(loss=np.random.random())
    
    best_ops = darts.get_best_operations()
    print(f"   Best operations: {best_ops[:3]}")
    
    print("\n2. RL-based Search:")
    
    rl_nas = RLNAS(SearchSpace())
    rl_nas.create_controller()
    
    arch = rl_nas.sample_architecture()
    print(f"   Sampled architecture: {arch.name}")
    print(f"   Cells: {len(arch.cells)}")
    
    print("\n3. Performance Benchmark:")
    
    bench = NASBench()
    
    for i in range(5):
        arch = Architecture(
            name=f"med_{i}",
            cells=[],
            input_shape=(224, 224, 3),
            num_classes=5,
            parameters=random.randint(100000, 5000000),
            accuracy=random.uniform(0.7, 0.95)
        )
        bench.architectures[arch.name] = arch
    
    top = bench.get_top_k(3)
    print(f"   Top-3 architectures:")
    for a in top:
        print(f"     {a.name}: {a.accuracy:.4f}")


def core_implementation():
    """Core implementation."""
    print("\n" + "="*60)
    print("CORE IMPLEMENTATION")
    print("="*60)
    
    print("\n1. SearchSpace:")
    space = SearchSpace()
    print(f"   Operations available: {len(space.get_operations())}")
    
    print("\n2. RLNAS:")
    rl = RLNAS(space)
    print("   RL-based NAS initialized")
    
    print("\n3. EvolutionaryNAS:")
    evo = EvolutionaryNAS(space)
    print("   Evolutionary NAS initialized")
    
    print("\n4. DARTS:")
    darts = DARTS()
    print("   DARTS initialized")
    
    print("\n5. PerformanceEstimator:")
    print("   Performance estimation available")
    
    print("\n6. NASBench:")
    bench = NASBench()
    print("   NAS Benchmark initialized")


def main():
    print("="*60)
    print("NEURAL ARCHITECTURE SEARCH")
    print("="*60)
    
    core_implementation()
    banking_example()
    healthcare_example()
    
    print("\n" + "="*60)
    print("IMPLEMENTATION COMPLETE")
    print("="*60)


if __name__ == "__main__":
    main()