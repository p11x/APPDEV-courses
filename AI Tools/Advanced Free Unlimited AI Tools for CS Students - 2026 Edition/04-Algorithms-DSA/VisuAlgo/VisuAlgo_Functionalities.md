# VisuAlgo - Functionalities

## Core Capabilities

### Algorithm Animation Engine

VisuAlgo provides real-time visualization of algorithm execution with the following capabilities:

1. **Step-by-Step Execution**
   - Execute algorithms one operation at a time
   - Track state changes after each step
   - Display current operation being performed

2. **Variable State Tracking**
   - Show current values of all variables
   - Highlight active data elements
   - Display array indices and pointers

3. **Pseudocode Synchronization**
   - Highlight current line in pseudocode
   - Show which code segment is executing
   - Display execution context

### User Input Handling

| Input Type | Description | Limits |
|------------|-------------|--------|
| Random Data | Generate random arrays/graphs | 5-30 elements |
| Custom Input | Enter specific values | Depends on visualization |
| Preset Examples | Load common test cases | Multiple options per algorithm |

### Visualization Modes

#### 1. Standard Mode
- Single visualization panel
- Basic animation controls
- Essential information display

#### 2. Split Mode
- Code panel on left
- Visualization on right
- Synchronized highlighting

#### 3. Comparison Mode (Some Algorithms)
- Two algorithms side by side
- Same input data
- Performance comparison

## Learning Features

### Complexity Information

Each visualization displays:
- Time complexity (Best/Average/Worst)
- Space complexity
- When each complexity applies

### Concept Explanations

- Brief description of algorithm purpose
- Use cases and applications
- Advantages and disadvantages

### Practice Integration

Students can use VisuAlgo to:
- Verify their algorithm understanding
- Debug their own implementations
- Prepare for viva voce exams

## Technical Implementation

### Browser Requirements

- Modern web browser (Chrome, Firefox, Safari, Edge)
- JavaScript enabled
- No plugins required

### Performance Characteristics

| Dataset Size | Responsiveness | Memory Usage |
|--------------|----------------|--------------|
| Small (5-10) | Instant | Low |
| Medium (11-20) | Fast | Moderate |
| Large (21-30) | Slight delay | Higher |

---

*Back to [Algorithms DSA README](../README.md)*