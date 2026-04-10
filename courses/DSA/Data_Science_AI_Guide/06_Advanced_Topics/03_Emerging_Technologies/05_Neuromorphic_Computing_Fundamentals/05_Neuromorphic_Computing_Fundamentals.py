# Topic: Neuromorphic Computing Fundamentals
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Neuromorphic Computing Fundamentals

I. INTRODUCTION
Neuromorphic computing mimics the brain's architecture using specialized hardware.
This module covers spiking neural networks, leaky integrate-and-fire neurons,
and neuromorphic chip architectures.

II. CORE CONCEPTS
- Spiking neural networks (SNN)
- Leaky integrate-and-fire neurons
- STDP learning rules
- Neuromorphic hardware
- Event-based processing

III. IMPLEMENTATION
"""

import numpy as np
import pandas as pd
from typing import Dict, List, Tuple, Optional, Any
from dataclasses import dataclass
from enum import Enum


class NeuronType(Enum):
    """Types of spiking neurons."""
    LIF = "leaky_integrate_and_fire"
    IF = "integrate_and_fire"
    IZHIKEVICH = "izhikevich"
    HODGKIN_HUXLEY = "hodgkin_huxley"


@dataclass
class Spike:
    """Spike event."""
    time: float
    neuron_id: int
    layer: str


class LeakyIntegrateFire:
    """Leaky Integrate-and-Fire neuron model."""

    def __init__(
        self,
        neuron_id: int,
        v_thresh: float = 1.0,
        v_reset: float = 0.0,
        v_rest: float = 0.0,
        tau_mem: float = 10.0,
        r_mem: float = 1.0,
        tau_ref: float = 2.0
    ):
        self.neuron_id = neuron_id
        self.v_thresh = v_thresh
        self.v_reset = v_reset
        self.v_rest = v_rest
        self.tau_mem = tau_mem
        self.r_mem = r_mem
        self.tau_ref = tau_ref
        
        self.v_mem = v_rest
        self.ref_period = 0.0
        self.spikes: List[Spike] = []

    def update(
        self,
        current_input: float,
        dt: float = 0.1
    ) -> Optional[Spike]:
        """Update neuron state."""
        if self.ref_period > 0:
            self.ref_period -= dt
            return None
        
        dv = (-(self.v_mem - self.v_rest) + self.r_mem * current_input) / self.tau_mem
        
        self.v_mem += dv * dt
        
        if self.v_mem >= self.v_thresh:
            spike = Spike(time=0, neuron_id=self.neuron_id, layer="output")
            self.spikes.append(spike)
            
            self.v_mem = self.v_reset
            self.ref_period = self.tau_ref
            
            return spike
        
        return None

    def reset(self) -> None:
        """Reset neuron state."""
        self.v_mem = self.v_rest
        self.ref_period = 0.0


class SpikingNeuralNetwork:
    """Spiking Neural Network (SNN) implementation."""

    def __init__(self, layers: List[int]):
        self.layers = layers
        self.neurons: Dict[str, List[LeakyIntegrateFire]] = {}
        self.connections: Dict[str, np.ndarray] = {}
        self.spikes: List[Spike] = []
        
        self._build_network()

    def _build_network(self) -> None:
        """Build network architecture."""
        for i, n_neurons in enumerate(self.layers):
            layer_name = f"layer_{i}"
            self.neurons[layer_name] = [
                LeakyIntegrateFire(neuron_id=j)
                for j in range(n_neurons)
            ]
            
            if i > 0:
                prev_size = self.layers[i-1]
                self.connections[layer_name] = np.random.randn(
                    prev_size, n_neurons
                ) * 0.1

    def forward(
        self,
        inputs: np.ndarray,
        duration: float = 100.0,
        dt: float = 0.1
    ) -> Dict[str, Any]:
        """Forward pass through SNN."""
        self.spikes = []
        
        for neuron in self.neurons['layer_0']:
            neuron.reset()
        
        n_timesteps = int(duration / dt)
        
        for t in range(n_timesteps):
            input_spikes = inputs[t % len(inputs)] if t < len(inputs) * 10 else np.zeros(len(inputs[0]))
            
            layer_outputs = []
            for layer_idx, layer_name in enumerate(self.neurons.keys()):
                if layer_idx == 0:
                    for neuron, inp in zip(self.neurons[layer_name], input_spikes):
                        spike = neuron.update(inp, dt)
                        if spike:
                            spike.time = t * dt
                            self.spikes.append(spike)
                else:
                    prev_layer = list(self.neurons.keys())[layer_idx - 1]
                    
                    prev_spikes = np.array([
                        1.0 if s in self.spikes and s.neuron_id == i else 0.0
                        for i in range(len(self.neurons[prev_layer]))
                    ])
                    
                    weighted_input = np.dot(prev_spikes, self.connections[layer_name])
                    
                    for neuron, inp in zip(self.neurons[layer_name], weighted_input):
                        spike = neuron.update(inp, dt)
                        if spike:
                            spike.time = t * dt
                            self.spikes.append(spike)
        
        return {
            'total_spikes': len(self.spikes),
            'spike_neurons': len(set(s.neuron_id for s in self.spikes)),
            'spike_times': [s.time for s in self.spikes]
        }

    def get_firing_rates(self) -> Dict[str, float]:
        """Calculate firing rates for each layer."""
        rates = {}
        
        for layer_name, neurons in self.neurons.items():
            n_spikes = sum(len(n.spikes) for n in neurons)
            n_neurons = len(neurons)
            
            if n_neurons > 0:
                rates[layer_name] = n_spikes / n_neurons
        
        return rates


class STDP:
    """Spike-Timing-Dependent Plasticity learning rule."""

    def __init__(
        self,
        tau_plus: float = 20.0,
        tau_minus: float = 20.0,
        a_plus: float = 0.01,
        a_minus: float = 0.01,
        w_max: float = 1.0,
        w_min: float = 0.0
    ):
        self.tau_plus = tau_plus
        self.tau_minus = tau_minus
        self.a_plus = a_plus
        self.a_minus = a_minus
        self.w_max = w_max
        self.w_min = w_min

    def compute_weight_change(
        self,
        pre_spike_times: List[float],
        post_spike_times: List[float],
        current_weight: float
    ) -> float:
        """Compute weight change based on spike timing."""
        delta_w = 0.0
        
        for pre_time in pre_spike_times:
            for post_time in post_spike_times:
                dt = post_time - pre_time
                
                if dt > 0:
                    delta_w += self.a_plus * np.exp(-dt / self.tau_plus)
                elif dt < 0:
                    delta_w -= self.a_minus * np.exp(dt / self.tau_minus)
        
        new_weight = current_weight + delta_w
        
        return np.clip(new_weight, self.w_min, self.w_max)

    def update_weights(
        self,
        weights: np.ndarray,
        pre_activity: np.ndarray,
        post_activity: np.ndarray
    ) -> np.ndarray:
        """Update all weights in layer."""
        updated_weights = weights.copy()
        
        for i in range(weights.shape[0]):
            for j in range(weights.shape[1]):
                delta = self.compute_weight_change(
                    pre_activity[i],
                    post_activity[j],
                    weights[i, j]
                )
                updated_weights[i, j] += delta
        
        return np.clip(updated_weights, self.w_min, self.w_max)


class EventBasedProcessing:
    """Event-based processing for neuromorphic data."""

    @staticmethod
    def create_events_from_array(
        array: np.ndarray,
        threshold: float = 0.5
    ) -> List[Spike]:
        """Convert array to spike events."""
        events = []
        
        for neuron_idx, values in enumerate(array):
            for time_idx, value in enumerate(values):
                if value > threshold:
                    events.append(Spike(
                        time=float(time_idx),
                        neuron_id=neuron_idx,
                        layer="input"
                    ))
        
        return events

    @staticmethod
    def convert_to_rate(
        events: List[Spike],
        bin_size: float = 1.0,
        n_neurons: int = 100
    ) -> np.ndarray:
        """Convert spike events to firing rates."""
        if not events:
            return np.zeros(n_neurons)
        
        max_time = max(e.time for e in events)
        n_bins = int(max_time / bin_size) + 1
        
        rates = np.zeros((n_bins, n_neurons))
        
        for event in events:
            bin_idx = int(event.time / bin_size)
            if bin_idx < n_bins:
                rates[bin_idx, event.neuron_id] += 1
        
        return rates / bin_size

    @staticmethod
    def filter_events(
        events: List[Spike],
        min_rate: float = 0.1,
        time_window: float = 100.0
    ) -> List[Spike]:
        """Filter events by minimum firing rate."""
        if not events:
            return events
        
        neuron_counts: Dict[int, int] = {}
        
        for event in events:
            neuron_counts[event.neuron_id] = neuron_counts.get(event.neuron_id, 0) + 1
        
        min_spikes = int(min_rate * time_window)
        
        active_neurons = {
            nid for nid, count in neuron_counts.items()
            if count >= min_spikes
        }
        
        return [e for e in events if e.neuron_id in active_neurons]


class NeuromorphicChip:
    """Neuromorphic chip simulation."""

    def __init__(
        self,
        n_cores: int = 4,
        neurons_per_core: int = 256,
        synaptic_weights: int = 1024
    ):
        self.n_cores = n_cores
        self.neurons_per_core = neurons_per_core
        self.synaptic_weights = synaptic_weights
        
        self.core_states: Dict[int, Dict[str, Any]] = {}
        self.energy_consumed: float = 0.0
        
        self._initialize_cores()

    def _initialize_cores(self) -> None:
        """Initialize chip cores."""
        for core_id in range(self.n_cores):
            self.core_states[core_id] = {
                'active_neurons': 0,
                'total_spikes': 0,
                'power_consumption': 0.0
            }

    def process_spikes(
        self,
        spikes: List[Spike]
    ) -> Dict[str, Any]:
        """Process spikes on neuromorphic chip."""
        for spike in spikes:
            core_id = spike.neuron_id % self.n_cores
            
            self.core_states[core_id]['active_neurons'] += 1
            self.core_states[core_id]['total_spikes'] += 1
        
        total_energy = len(spikes) * 0.9
        self.energy_consumed += total_energy
        
        for core in self.core_states.values():
            core['power_consumption'] = core['total_spikes'] * 0.001
        
        return {
            'total_spikes': len(spikes),
            'energy_pj': self.energy_consumed,
            'cores_active': sum(
                1 for s in self.core_states.values()
                if s['active_neurons'] > 0
            )
        }


class RateCoding:
    """Rate-based coding for neuromorphic systems."""

    @staticmethod
    def encode(
        data: np.ndarray,
        time_window: float = 100.0,
        rate_max: float = 1000.0
    ) -> np.ndarray:
        """Encode data as spike rates."""
        data_normalized = (data - data.min()) / (data.max() - data.min() + 1e-10)
        
        rates = data_normalized * rate_max * (time_window / 1000.0)
        
        return rates

    @staticmethod
    def decode(
        spikes: List[Spike],
        time_window: float = 100.0,
        n_neurons: int = 100
    ) -> np.ndarray:
        """Decode spike rates back to data."""
        rates = EventBasedProcessing.convert_to_rate(
            spikes, bin_size=time_window/10, n_neurons=n_neurons
        )
        
        mean_rates = np.mean(rates, axis=0)
        
        return mean_rates


def banking_example():
    """Neuromorphic computing in banking sector."""
    print("\n" + "="*60)
    print("BANKING SECTOR: Neuromorphic Fraud Detection")
    print("="*60)
    
    print("\n1. Spiking Neural Network:")
    
    snn = SpikingNeuralNetwork(layers=[64, 32, 16, 2])
    
    inputs = np.random.rand(1000, 64)
    
    result = snn.forward(inputs, duration=100.0)
    
    print(f"   Layers: {snn.layers}")
    print(f"   Total spikes: {result['total_spikes']}")
    
    print("\n2. Firing Rates:")
    
    rates = snn.get_firing_rates()
    for layer, rate in rates.items():
        print(f"   {layer}: {rate:.2f} Hz")
    
    print("\n3. STDP Learning:")
    
    stdp = STDP(tau_plus=20.0, tau_minus=20.0)
    
    weights = np.random.rand(10, 10) * 0.5
    pre_activity = [[1.0, 2.0], [0.5, 1.5]]
    post_activity = [[1.5], [2.0]]
    
    updated = stdp.update_weights(weights, pre_activity, post_activity)
    print(f"   Weight update applied")
    print(f"   Weight change: {np.sum(updated - weights):.4f}")
    
    print("\n4. Event Processing:")
    
    events = EventBasedProcessing.create_events_from_array(
        np.random.rand(10, 10), threshold=0.5
    )
    print(f"   Events generated: {len(events)}")


def healthcare_example():
    """Neuromorphic computing in healthcare sector."""
    print("\n" + "="*60)
    print("HEALTHCARE SECTOR: Neuromorphic Medical Sensing")
    print("="*60)
    
    print("\n1. Event-based Medical Sensors:")
    
    sensor_data = np.random.rand(100, 64)
    
    events = EventBasedProcessing.create_events_from_array(
        sensor_data, threshold=0.7
    )
    
    print(f"   Sensor channels: {sensor_data.shape[1]}")
    print(f"   Events generated: {len(events)}")
    
    print("\n2. Rate Coding:")
    
    encoded = RateCoding.encode(sensor_data, time_window=50.0)
    print(f"   Encoded rates shape: {encoded.shape}")
    
    decoded = RateCoding.decode(events, time_window=50.0, n_neurons=64)
    print(f"   Decoded shape: {decoded.shape}")
    
    print("\n3. Neuromorphic Chip:")
    
    chip = NeuromorphicChip(n_cores=4, neurons_per_core=256)
    
    spikes = [Spike(time=float(i), neuron_id=i % 100, layer="input") for i in range(1000)]
    
    result = chip.process_spikes(spikes)
    
    print(f"   Cores: {chip.n_cores}")
    print(f"   Energy: {result['energy_pj']:.2f} pJ")
    
    print("\n4. Event Filtering:")
    
    filtered = EventBasedProcessing.filter_events(
        events, min_rate=0.1, time_window=50.0
    )
    print(f"   Filtered events: {len(filtered)}")


def core_implementation():
    """Core implementation."""
    print("\n" + "="*60)
    print("CORE IMPLEMENTATION")
    print("="*60)
    
    print("\n1. LeakyIntegrateFire:")
    neuron = LeakyIntegrateFire(neuron_id=0)
    print(f"   Neuron initialized: ID={neuron.neuron_id}")
    
    print("\n2. SpikingNeuralNetwork:")
    snn = SpikingNeuralNetwork([10, 5, 2])
    print(f"   Layers: {snn.layers}")
    
    print("\n3. STDP:")
    stdp = STDP()
    print("   STDP learning rule ready")
    
    print("\n4. EventBasedProcessing:")
    print("   Event processing ready")
    
    print("\n5. NeuromorphicChip:")
    chip = NeuromorphicChip(n_cores=2)
    print(f"   Cores: {chip.n_cores}")


def main():
    print("="*60)
    print("NEUROMORPHIC COMPUTING FUNDAMENTALS")
    print("="*60)
    
    core_implementation()
    banking_example()
    healthcare_example()
    
    print("\n" + "="*60)
    print("IMPLEMENTATION COMPLETE")
    print("="*60)


if __name__ == "__main__":
    main()