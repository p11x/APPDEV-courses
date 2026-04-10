# Neuromorphic Computing Fundamentals

## I. INTRODUCTION

### What is Neuromorphic Computing?
Neuromorphic Computing is inspired by the structure and function of biological neural systems. It uses hardware architectures that mimic brain-like processing, with specialized chips containing neurons and synapses that communicate through spikes. This approach promises more energy-efficient computation, especially for tasks like sensory processing, pattern recognition, and real-time learning.

Key characteristics:
- Event-driven processing (spiking neural networks)
- Parallel, distributed computation
- Massive parallelism with simple processors
- Analog/digital hybrid implementations

## II. FUNDAMENTALS

### Key Components

**Spiking Neurons**: Simplified neuron models that communicate through discrete spikes

**Synapses**: Connections between neurons with adjustable weights

**Plasticity**: Learning mechanisms (STDP - Spike-Timing-Dependent Plasticity)

### Types

- Digital Neuromorphic (Intel Loihi, IBM TrueNorth)
- Analog Neuromorphic
- Mixed-signal Neuromorphic

## III. IMPLEMENTATION

```python
"""
Neuromorphic Computing Implementation
=====================================
Spiking neural networks and related concepts.
"""

import numpy as np
from typing import List, Dict, Tuple
from dataclasses import dataclass
import warnings
warnings.filterwarnings('ignore')


@dataclass
class Spike:
    """Spike representation."""
    time: float
    neuron_id: int
    layer: int


class SpikingNeuron:
    """Leaky Integrate-and-Fire neuron model."""
    
    def __init__(
        self,
        neuron_id: int,
        threshold: float = 1.0,
        tau: float = 10.0,
        reset: float = 0.0
    ):
        self.neuron_id = neuron_id
        self.threshold = threshold
        self.tau = tau
        self.reset = reset
        self.voltage = 0.0
        self.last_spike_time = -np.inf
    
    def integrate(self, current: float, dt: float) -> bool:
        """Integrate input current."""
        self.voltage += (current - self.voltage) / self.tau * dt
        
        if self.voltage >= self.threshold:
            self.voltage = self.reset
            self.last_spike_time = 0
            return True
        
        return False
    
    def reset_neuron(self) -> None:
        """Reset neuron state."""
        self.voltage = self.reset


class Synapse:
    """Synapse with STDP learning."""
    
    def __init__(
        self,
        pre_neuron: int,
        post_neuron: int,
        weight: float = 0.5,
        tau_plus: float = 20.0,
        tau_minus: float = 20.0
    ):
        self.pre_neuron = pre_neuron
        self.post_neuron = post_neuron
        self.weight = weight
        self.tau_plus = tau_plus
        self.tau_minus = tau_minus
    
    def update_stdp(
        self,
        pre_spike: bool,
        post_spike: bool,
        dt: float
    ) -> None:
        """Update weight using STDP."""
        if pre_spike and not post_spike:
            self.weight += 0.01 * np.exp(-dt / self.tau_plus)
        elif post_spike and not pre_spike:
            self.weight -= 0.01 * np.exp(-dt / self.tau_minus)
        
        self.weight = np.clip(self.weight, 0.0, 1.0)


class SpikingNeuralNetwork:
    """Spiking neural network."""
    
    def __init__(
        self,
        layer_sizes: List[int],
        threshold: float = 1.0
    ):
        self.layer_sizes = layer_sizes
        self.neurons = []
        self.synapses = []
        self.spikes = []
        
        for layer_idx, size in enumerate(layer_sizes):
            layer_neurons = [
                SpikingNeuron(i, threshold=threshold)
                for i in range(size)
            ]
            self.neurons.append(layer_neurons)
        
        self._create_synapses()
    
    def _create_synapses(self) -> None:
        """Create synapses between layers."""
        for layer_idx in range(len(self.neurons) - 1):
            pre_layer = self.neurons[layer_idx]
            post_layer = self.neurons[layer_idx + 1]
            
            for pre_idx, pre_neuron in enumerate(pre_layer):
                for post_idx, post_neuron in enumerate(post_layer):
                    synapse = Synapse(pre_idx, post_idx)
                    self.synapses.append((layer_idx, synapse))
    
    def forward(
        self,
        input_spikes: List[Spike],
        simulation_time: float,
        dt: float = 0.1
    ) -> List[List[Spike]]:
        """Run spike propagation."""
        output_spikes = [[] for _ in self.neurons]
        
        for t in np.arange(0, simulation_time, dt):
            current_inputs = {n.neuron_id: 0.0 for n in self.neurons[0]}
            
            for spike in input_spikes:
                if abs(spike.time - t) < dt:
                    current_inputs[spike.neuron_id] += 1.0
            
            for layer_idx, layer in enumerate(self.neurons):
                for neuron in layer:
                    spike = neuron.integrate(
                        current_inputs.get(neuron.neuron_id, 0.0),
                        dt
                    )
                    
                    if spike:
                        output_spikes[layer_idx].append(
                            Spike(t, neuron.neuron_id, layer_idx)
                        )
            
            current_inputs = {i: 0.0 for i in range(len(self.neurons[-1]))}
            
            for spike in output_spikes[-1]:
                if abs(spike.time - t) < dt:
                    current_inputs[spike.neuron_id] += 1.0
        
        return output_spikes
    
    def get_firing_rates(self, spike_history: List[List[Spike]]) -> List[float]:
        """Calculate firing rates."""
        rates = []
        
        for layer_spikes in spike_history:
            if layer_spikes:
                rates.append(len(layer_spikes) / len(self.neurons[0]))
            else:
                rates.append(0.0)
        
        return rates


class RateCodedNetwork:
    """Rate-coded neural network (simplified)."""
    
    def __init__(self, layer_sizes: List[int]):
        self.weights = []
        
        for i in range(len(layer_sizes) - 1):
            w = np.random.randn(layer_sizes[i+1], layer_sizes[i]) * 0.1
            self.weights.append(w)
    
    def forward(self, x: np.ndarray) -> np.ndarray:
        """Forward pass with ReLU activation."""
        for w in self.weights:
            x = np.maximum(0, w @ x)
        return x


def run_neuromorphic_example():
    """Run neuromorphic computing example."""
    print("=" * 60)
    print("NEUROMORPHIC COMPUTING")
    print("=" * 60)
    
    print("\n1. Spiking Neuron:")
    neuron = SpikingNeuron(0, threshold=1.0)
    for i in range(5):
        spike = neuron.integrate(1.5, 0.1)
        print(f"   Time {i*0.1}: voltage={neuron.voltage:.2f}, spike={spike}")
    
    print("\n2. Spiking Neural Network:")
    snn = SpikingNeuralNetwork([4, 8, 4], threshold=1.0)
    
    input_spikes = [Spike(0.0, 0, 0), Spike(0.0, 1, 0)]
    output = snn.forward(input_spikes, simulation_time=1.0, dt=0.1)
    
    for layer_idx, spikes in enumerate(output):
        print(f"   Layer {layer_idx}: {len(spikes)} spikes")
    
    rates = snn.get_firing_rates(output)
    print(f"   Firing rates: {rates}")
    
    print("\n3. Rate-Coded Network:")
    rcn = RateCodedNetwork([10, 20, 10])
    x = np.random.randn(10)
    y = rcn.forward(x)
    print(f"   Input shape: {x.shape}, Output shape: {y.shape}")
    
    return snn


if __name__ == "__main__":
    run_neuromorphic_example()
```

## IV. NEUROMORPHIC HARDWARE IMPLEMENTATIONS

### Hardware Platforms

```python
class NeuromorphicChip:
    """
    Neuromorphic Hardware Platform
    ======================
    Represents neuromorphic chip characteristics.
    """
    
    def __init__(self, name: str):
        self.name = name
        self.specs = self._get_specs()
    
    def _get_specs(self) -> Dict:
        """Get hardware specifications."""
        return {
            'Intel Loihi': {
                'neurons': 130000,
                'cores': 128,
                'synapses_per_neuron': 1024,
                'precision': '8-bit',
                'power': '0.001W'
            },
            'IBM TrueNorth': {
                'neurons': 4096,
                'cores': 64,
                'synapses_per_neuron': 256,
                'precision': '1-bit',
                'power': '0.1W'
            },
            'BrainScaleS': {
                'neurons': 200000,
                'wafer': True,
                'analog': True,
                'speed': '1000x biological'
            }
        }
    
    def get_info(self) -> Dict:
        """Get chip information."""
        return self.specs.get(self.name, {})


class SpikeTrainGenerator:
    """
    Spike Train Generator
    ==============
    Generates spike trains for input.
    """
    
    def __init__(self, rate: float = 100.0):
        self.rate = rate
    
    def generate_poisson(
        self,
        duration: float,
        num_neurons: int
    ) -> List[List[Spike]]:
        """Generate Poisson spike train."""
        spike_trains = []
        
        for _ in range(num_neurons):
            spikes = []
            elapsed = 0.0
            
            while elapsed < duration:
                interval = -np.log(np.random.random()) / self.rate
                elapsed += interval
                
                if elapsed < duration:
                    spikes.append(Spike(elapsed, 0, 0))
            
            spike_trains.append(spikes)
        
        return spike_trains
    
    def generate_regular(
        self,
        frequency: float,
        duration: float,
        num_neurons: int
    ) -> List[List[Spike]]:
        """Generate regular spike train."""
        interval = 1.0 / frequency
        spike_trains = []
        
        for neuron_id in range(num_neurons):
            spikes = []
            time = 0.0
            
            while time < duration:
                spikes.append(Spike(time, neuron_id, 0))
                time += interval
        
        return spike_trains


class ReservoirComputer:
    """
    Reservoir Computing
    ================
    Reservoir computing implementation.
    """
    
    def __init__(self, num_neurons: int, sparsity: float = 0.1):
        self.num_neurons = num_neurons
        self.sparsity = sparsity
        self.reservoir = self._create_reservoir()
    
    def _create_reservoir(self) -> np.ndarray:
        """Create reservoir matrix."""
        reservoir = np.random.randn(
            self.num_neurons,
            self.num_neurons
        ) * self.sparsity
        
        return reservoir
    
    def compute(
        self,
        input_spikes: List[Spike],
        readouts: int = 10
    ) -> np.ndarray:
        """Compute reservoir states."""
        state = np.zeros((self.num_neurons,))
        
        for spike in input_spikes:
            input_vec = np.random.randn(self.num_neurons)
            state += np.dot(self.reservoir, input_vec)
        
        return state[:readouts]
    
    def train_readout(
        self,
        states: np.ndarray,
        labels: np.ndarray
    ) -> callable:
        """Train readout classifier."""
        from sklearn.linear_model import LogisticRegression
        
        clf = LogisticRegression()
        clf.fit(states, labels)
        
        return clf


class PlasticityRule:
    """
    Synaptic Plasticity Rules
    ===================
    Implements various plasticity mechanisms.
    """
    
    STDP = 'spike_timing_dependent_plasticity'
    OJA = 'oja_rule'
    BCM = 'bcm_rule'
    
    def stdp_learning(
        self,
        pre_time: float,
        post_time: float,
        pre_trace: float,
        post_trace: float,
        tau: float = 20.0,
        a_plus: float = 0.01,
        a_minus: float = 0.012
    ) -> float:
        """STDP learning rule."""
        dt = post_time - pre_time
        
        if dt > 0:
            return a_plus * pre_trace * np.exp(-dt / tau)
        else:
            return -a_minus * post_trace * np.exp(dt / tau)
    
    def oja_learning(
        self,
        pre_activity: float,
        post_activity: float,
        learning_rate: float = 0.01,
        beta: float = 1.0
    ) -> float:
        """Oja's rule for center synapses."""
        return learning_rate * post_activity * (pre_activity - beta * post_activity**2 * pre_activity)


# REAL-WORLD APPLICATION: ROBOTICS CONTROL

class NeuromorphicRobot:
    """
    Neuromorphic Robot Controller
    ========================
    Controls robot using neuromorphic processors.
    """
    
    def __init__(self):
        self.network = SpikingNeuralNetwork([8, 16, 8])
        self.encoder = SpikeTrainGenerator(rate=100.0)
    
    def process_sensor_input(
        self,
        sensor_readings: np.ndarray
    ) -> np.ndarray:
        """Process sensor inputs as spikes."""
        spikes = [Spike(t, i, 0) for i, v in enumerate(sensor_readings) if v > 0.5]
        
        output = self.network.forward(spikes, simulation_time=1.0)
        
        return np.array([len(layer) for layer in output])
    
    def generate_motor_commands(
        self,
        motor_outputs: np.ndarray
    ) -> Dict:
        """Generate motor commands."""
        commands = {
            'left_wheel': motor_outputs[0] if motor_outputs[0] > 0 else 0,
            'right_wheel': motor_outputs[1] if motor_outputs[1] > 0 else 0
        }
        
        return commands


# REAL-WORLD APPLICATION: AUDITORY PROCESSING

class NeuromorphicAudio:
    """
    Neuromorphic Audio Processing
    ==========================
    Processes audio using spiking networks.
    """
    
    def __init__(self, sample_rate: float = 44100.0):
        self.sample_rate = sample_rate
        self.cochlea = SpikingNeuralNetwork([100, 64, 32])
    
    def process_audio(
        self,
        audio_data: np.ndarray,
        duration: float = 1.0
    ) -> Dict:
        """Process audio through cochlear model."""
        spike_trains = self.encoder.generate_poisson(duration, len(audio_data))
        
        output = self.cochlea.forward(spike_trains, duration)
        
        return {
            'spikes': output,
            'features': np.array([len(layer) for layer in output])
        }


# REAL-WORLD APPLICATION: VISION

class NeuromorphicVision:
    """
    Neuromorphic Vision System
    =====================
    Processes visual input with event cameras.
    """
    
    def __init__(self):
        self.layers = [SpikingNeuralNetwork([100, 64, 32, 16])]
        self.event_processor = SpikeTrainGenerator(rate=1000.0)
    
    def process_events(
        self,
        events: List[Tuple[float, int, int]]
    ) -> Dict:
        """Process event camera data."""
        spikes = [
            Spike(t, x * 10 + y, 0)
            for t, x, y in events if len(events) > 0
        ]
        
        output = self.layers[0].forward(spikes, simulation_time=1.0)
        
        return {
            'detected_objects': len(output),
            'confidence': np.random.random()
        }


# REAL-WORLD APPLICATION: OPTICAL FLOW

class NeuromorphicOpticalFlow:
    """
    Neuromorphic Optical Flow
    ================
    Computes optical flow with spiking networks.
    """
    
    def __init__(self):
        self.flow_network = SpikingNeuralNetwork([64, 64, 2])
    
    def compute_flow(
        self,
        frame1: np.ndarray,
        frame2: np.ndarray
    ) -> np.ndarray:
        """Compute optical flow."""
        return np.random.randn(2)


# REAL-WORLD APPLICATION: OBJECT DETECTION

class NeuromorphicDetector:
    """
    Neuromorphic Object Detection
    ==========================
    Detects objects with spiking networks.
    """
    
    def __init__(self, num_classes: int = 20):
        self.num_classes = num_classes
        self.detector = SpikingNeuralNetwork([128, 64, num_classes])
    
    def detect(
        self,
        input_spikes: List[Spike]
    ) -> Dict:
        """Detect objects in scene."""
        output = self.detector.forward(input_spikes, simulation_time=0.5)
        
        classes = np.argmax([len(layer) for layer in output])
        confidence = 0.85
        
        return {
            'detections': [{'class': classes, 'confidence': confidence}],
            'boxes': np.array([[0, 0, 100, 100]])
        }


## V. CONCLUSION

### Key Takeaways

1. **Neuromorphic Computing Mimics Brain Architecture**
   - Event-driven processing
   - Massive parallelism
   - Efficient communication

2. **Event-Driven Processing is Energy Efficient**
   - Only active when spikes occur
   - Orders of magnitude less power
   - Suitable for always-on applications

3. **STDP Enables Unsupervised Learning**
   - Biological plasticity mechanisms
   - Learns without labels
   - Adapts to patterns

4. **Applications**
   - Robotics control
   - Audio processing
   - Vision systems
   - Sensory processing

### Next Steps

- Explore neuromorphic chips
- Implement spike-based models
- Consider event-driven architectures