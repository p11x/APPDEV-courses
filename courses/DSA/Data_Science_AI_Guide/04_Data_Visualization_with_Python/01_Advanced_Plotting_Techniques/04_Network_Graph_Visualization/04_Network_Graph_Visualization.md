# Network Graph Visualization

## I. INTRODUCTION

### What is Network Graph Visualization?

Network graph visualization (also called graph visualization or network mapping) is the practice of representing relational data as mathematical graphs. In this context, a graph consists of nodes (vertices) representing entities and edges (links) representing relationships between those entities.

Network visualization is essential for understanding complex relational structures in social networks, organizational hierarchies, transportation systems, biological networks, and communication infrastructure. It transforms abstract relationship data into intuitive visual formats that reveal clusters, bridges, influencers, and communication patterns.

### Why is Network Graph Visualization Important?

1. **Relational Insight**: Networks reveal connections that aren't visible in tabular data.

2. **Structural Analysis**: Identifies clusters, communities, and key influencers.

3. **Path Finding**: Shows shortest paths and flow patterns.

4. **Influence Analysis**: Identifies central and important nodes.

5. **Pattern Recognition**: Reveals community structures and clusters.

### Prerequisites

- Python programming
- Understanding of basic graph theory
- pandas and numpy for data handling
- NetworkX for graph operations
- matplotlib, plotly, or pyvis for visualization

## II. FUNDAMENTALS

### Key Concepts

**Nodes**: The entities in a network (people, organizations, computers).

**Edges**: The connections between nodes (relationships, interactions).

**Directed vs Undirected**: Edges can have direction (follows) or be bidirectional (friends).

**Weighted Edges**: Edges with values representing strength of relationship.

**Degree**: Number of connections for a node.

**Centrality**: Measure of a node's importance in the network.

**Community**: Group of nodes with dense internal connections.

## III. IMPLEMENTATION

### Step-by-Step Code Examples

```python
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import networkx as nx
import plotly.graph_objects as go
import warnings
warnings.filterwarnings('ignore')

def create_basic_network():
    """Create basic network graph visualization."""
    np.random.seed(42)
    
    # Create a small social network
    edges = [
        ('Alice', 'Bob', 1),
        ('Alice', 'Charlie', 1),
        ('Alice', 'David', 1),
        ('Bob', 'Charlie', 1),
        ('Charlie', 'David', 1),
        ('David', 'Eve', 1),
        ('Eve', 'Frank', 1),
        ('Frank', 'Alice', 1),
        ('Bob', 'Eve', 1),
        ('Charlie', 'Frank', 1)
    ]
    
    G = nx.Graph()
    for e in edges:
        G.add_edge(e[0], e[1], weight=e[2])
    
    pos = nx.spring_layout(G, seed=42)
    
    fig, ax = plt.subplots(figsize=(12, 8))
    
    # Draw nodes
    nx.draw_networkx_nodes(G, pos, node_color='lightblue', 
                       node_size=700, ax=ax)
    
    # Draw edges
    nx.draw_networkx_edges(G, pos, edge_color='gray', 
                        width=2, ax=ax)
    
    # Draw labels
    nx.draw_networkx_labels(G, pos, font_size=12, 
                        font_weight='bold', ax=ax)
    
    ax.set_title('Basic Social Network')
    ax.axis('off')
    
    return fig, G

fig_basic, G_basic = create_basic_network()
plt.show()


def create_weighted_network():
    """Create weighted network showing relationship strength."""
    np.random.seed(42)
    
    # Create edges with weights
    edges = [
        ('Company A', 'Company B', 85),
        ('Company A', 'Company C', 45),
        ('Company B', 'Company C', 70),
        ('Company B', 'Company D', 30),
        ('Company C', 'Company D', 55),
        ('Company D', 'Company E', 90),
        ('Company E', 'Company A', 25)
    ]
    
    G = nx.Graph()
    for e in edges:
        G.add_edge(e[0], e[1], weight=e[2])
    
    pos = nx.circular_layout(G)
    
    fig, ax = plt.subplots(figsize=(12, 8))
    
    # Get edge widths from weights
    edges_list = G.edges(data=True)
    edge_weights = [d['weight'] / 15 for (u, v, d) in edges_list]
    
    # Draw network
    nx.draw_networkx_nodes(G, pos, node_color='lightgreen',
                       node_size=1500, ax=ax)
    nx.draw_networkx_edges(G, pos, width=edge_weights,
                        edge_color='gray', ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=10, ax=ax)
    
    # Add edge labels
    edge_labels = {(u, v): d['weight'] for u, v, d in edges_list}
    nx.draw_networkx_edge_labels(G, pos, edge_labels, font_size=9, ax=ax)
    
    ax.set_title('Business Network (Edge Weights = Transaction Volume)')
    ax.axis('off')
    
    return fig, G

fig_weighted, G_weighted = create_weighted_network()
plt.show()


def create_directed_network():
    """Create directed network showing flow direction."""
    np.random.seed(42)
    
    # Create directed edges representing flow
    edges = [
        ('Marketing', 'Sales', 10),
        ('Marketing', 'Social', 5),
        ('Sales', 'Revenue', 8),
        ('Social', 'Web Traffic', 7),
        ('Web Traffic', 'Leads', 4),
        ('Leads', 'Sales', 3),
        ('Revenue', 'Profit', 10)
    ]
    
    G = nx.DiGraph()
    for e in edges:
        G.add_edge(e[0], e[1], weight=e[2])
    
    pos = nx.spring_layout(G, seed=42)
    
    fig, ax = plt.subplots(figsize=(12, 8))
    
    # Draw with directed arrows
    nx.draw_networkx_nodes(G, pos, node_color='lightyellow',
                       node_size=1500, ax=ax)
    nx.draw_networkx_edges(G, pos, edge_color='blue',
                        width=2, arrows=True,
                        arrowsize=20,
                        connectionstyle='arc,rad=0.1', ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=10, ax=ax)
    
    ax.set_title('Marketing Funnel Flow (Directed)')
    ax.axis('off')
    
    return fig, G

fig_directed, G_directed = create_directed_network()
plt.show()


def create_community_network():
    """Create network with communities/groups."""
    np.random.seed(42)
    
    # Create two communities
    community1 = [f'GroupA_{i}' for i in range(1, 6)]
    community2 = [f'GroupB_{i}' for i in range(1, 6)]
    
    G = nx.Graph()
    
    # Add nodes
    for n in community1 + community2:
        G.add_node(n)
    
    # Dense connections within communities
    for i, n1 in enumerate(community1):
        for n2 in community1[i+1:]:
            G.add_edge(n1, n2, weight=1)
    
    for i, n1 in enumerate(community2):
        for n2 in community2[i+1:]:
            G.add_edge(n1, n2, weight=1)
    
    # Sparse connections between communities (bridge)
    G.add_edge('GroupA_1', 'GroupB_1', weight=0)
    
    # Position
    pos = nx.spring_layout(G, seed=42)
    
    fig, ax = plt.subplots(figsize=(12, 8))
    
    # Different colors for communities
    colors = ['lightblue' if 'GroupA' in n else 'lightcoral' 
              for n in G.nodes()]
    
    nx.draw_networkx_nodes(G, pos, node_color=colors,
                       node_size=800, ax=ax)
    nx.draw_networkx_edges(G, pos, edge_color='gray',
                        width=1.5, ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=9, ax=ax)
    
    ax.set_title('Network Communities')
    ax.axis('off')
    
    return fig, G

fig_comm, G_comm = create_community_network()
plt.show()


def create_interactive_network():
    """Create interactive network with Plotly."""
    np.random.seed(42)
    
    # Sample organization network
    edges = [
        ('CEO', 'VP Eng'), ('CEO', 'VP Sales'), ('CEO', 'VP Ops'),
        ('VP Eng', 'Eng Lead'), ('VP Eng', 'Eng Lead 2'),
        ('VP Sales', 'Sales Lead'), ('VP Sales', 'Marketing Lead'),
        ('VP Ops', 'HR Lead'), ('VP Ops', 'Finance Lead'),
        ('Eng Lead', 'Dev 1'), ('Eng Lead', 'Dev 2'),
        ('Eng Lead 2', 'Dev 3'), ('Eng Lead 2', 'Dev 4'),
        ('Sales Lead', 'AE 1'), ('Sales Lead', 'AE 2'),
        ('Marketing Lead', 'MM 1'), ('Marketing Lead', 'MM 2')
    ]
    
    G = nx.DiGraph()
    for e in edges:
        G.add_edge(e[0], e[1])
    
    # Get positions from spring layout
    pos = nx.spring_layout(G, seed=42, k=2)
    
    # Create edge traces
    edge_x = []
    edge_y = []
    for edge in G.edges():
        x0, y0 = pos[edge[0]]
        x1, y1 = pos[edge[1]]
        edge_x.extend([x0, x1, None])
        edge_y.extend([y0, y1, None])
    
    edge_trace = go.Scatter(
        x=edge_x, y=edge_y,
        mode='lines',
        line=dict(width=1.5, color='gray'),
        hoverinfo='none'
    )
    
    # Create node trace
    node_x = [pos[n][0] for n in G.nodes()]
    node_y = [pos[n][1] for n in G.nodes()]
    
    node_trace = go.Scatter(
        x=node_x, y=node_y,
        mode='markers+text',
        text=[n for n in G.nodes()],
        textposition='top center',
        marker=dict(
            size=20,
            color='lightblue',
            line=dict(width=2, color='darkblue')
        ),
        hoverinfo='text'
    )
    
    fig = go.Figure(data=[edge_trace, node_trace])
    
    fig.update_layout(
        title='Organization Chart',
        showlegend=False,
        hovermode='closest',
        xaxis=dict(showgrid=False, zeroline=False),
        yaxis=dict(showgrid=False, zeroline=False)
    )
    
    return fig, G

fig_interactive, G_interactive = create_interactive_network()
fig_interactive.show()
```

## IV. APPLICATIONS

### Standard Example: Social Network Analysis

```python
def social_network_analysis():
    """Comprehensive social network analysis."""
    np.random.seed(42)
    
    # Create a realistic social network
    n_people = 30
    names = [f'Person_{i}' for i in range(1, n_people + 1)]
    
    G = nx.barabasi_albert_graph(n_people, 2)
    G = nx.relabel_nodes(G, {i: names[i] for i in range(n_people)})
    
    # Calculate centrality measures
    degree_centrality = nx.degree_centrality(G)
    betweenness = nx.betweenness_centrality(G)
    pagerank = nx.pagerank(G)
    
    # Create visualization
    fig, axes = plt.subplots(1, 2, figsize=(16, 7))
    
    # Network plot
    ax1 = axes[0]
    pos = nx.spring_layout(G, seed=42, k=1.5)
    
    # Node sizes based on degree
    node_sizes = [degree_centrality[n] * 3000 + 100 for n in G.nodes()]
    
    # Node colors based on pagerank
    node_colors = [pagerank[n] for n in G.nodes()]
    
    nx.draw_networkx_nodes(G, pos, node_color=node_colors,
                         node_size=node_sizes, ax=ax1,
                         cmap=plt.cm.RdYlBu)
    nx.draw_networkx_edges(G, pos, alpha=0.3, ax=ax1)
    nx.draw_networkx_labels(G, pos, font_size=7, ax=ax1)
    
    ax1.set_title('Social Network\n(Node Color: PageRank, Size: Degree)')
    ax1.axis('off')
    
    # Centrality comparison
    ax2 = axes[1]
    
    # Get top 10 by different metrics
    top_degree = sorted(degree_centrality.items(), key=lambda x: x[1], reverse=True)[:10]
    top_between = sorted(betweenness.items(), key=lambda x: x[1], reverse=True)[:10]
    top_pagerank = sorted(pagerank.items(), key=lambda x: x[1], reverse=True)[:10]
    
    metrics = ['Degree', 'Betweenness', 'PageRank']
    top_nodes = list(set([n for n, _ in top_degree + top_between + top_pagerank]))[:10]
    
    x = np.arange(len(top_nodes))
    width = 0.25
    
    for i, (metric, top_list) in enumerate(zip(metrics, [top_degree, top_between, top_pagerank])):
        values = [dict(top_list).get(n, 0) for n in top_nodes]
        normalized = [v / max(values) if max(values) > 0 else 0 for v in values]
        ax2.barh(x + i * width, normalized, width, label=metric)
    
    ax2.set_yticks(x + width)
    ax2.set_yticklabels(top_nodes, fontsize=8)
    ax2.set_xlabel('Normalized Centrality')
    ax2.set_title('Top 10 Influencers by Different Metrics')
    ax2.legend()
    
    plt.tight_layout()
    
    print("=" * 50)
    print("NETWORK ANALYSIS SUMMARY")
    print("=" * 50)
    print(f"Nodes: {G.number_of_nodes()}")
    print(f"Edges: {G.number_of_edges()}")
    print(f"Avg Degree: {sum(dict(G.degree()).values()) / G.number_of_nodes():.1f}")
    
    return fig, G

fig_social, G_social = social_network_analysis()
plt.show()
```

### Real-world Example 1: Banking - Customer Relationship Network

```python
def banking_network():
    """Example: Banking customer relationship network."""
    np.random.seed(42)
    
    # Create bank-customer network
    G = nx.Graph()
    
    # Add bank hub
    G.add_node('Central Bank', type='hub', size=5000)
    
    # Add branch nodes
    branches = ['Branch_A', 'Branch_B', 'Branch_C', 'Branch_D']
    for b in branches:
        G.add_node(b, type='branch', size=2000)
        G.add_edge('Central Bank', b, weight=100)
    
    # Add customer nodes to branches
    for branch_idx, branch in enumerate(branches):
        for c in range(5):
            cust = f'Customer_{branch_idx}_{c}'
            G.add_node(cust, type='customer', size=500)
            G.add_edge(branch, cust, weight=np.random.randint(10, 50))
    
    # Add some customer-customer connections (transactions)
    np.random.seed(42)
    for _ in range(10):
        n1, n2 = np.random.choice(list(G.nodes()), 2, replace=False)
        if G.has_edge(n1, n2) == False and 'Customer' in n1 and 'Customer' in n2:
            G.add_edge(n1, n2, weight=np.random.randint(5, 30))
    
    # Calculate and visualize
    fig, ax = plt.subplots(figsize=(14, 10))
    
    # Custom positions
    pos = nx.spring_layout(G, seed=42, k=2)
    
    # Adjust hub position
    pos['Central Bank'] = np.array([0, 0])
    for i, branch in enumerate(branches):
        angle = 2 * np.pi * i / len(branches)
        pos[branch] = np.array([np.cos(angle), np.sin(angle)]) * 1.5
    
    # Color by type
    colors = []
    sizes = []
    for n in G.nodes():
        if n == 'Central Bank':
            colors.append('gold')
            sizes.append(3000)
        elif 'Branch' in n:
            colors.append('lightblue')
            sizes.append(1500)
        else:
            colors.append('lightgreen')
            sizes.append(500)
    
    nx.draw_networkx_nodes(G, pos, node_color=colors, node_size=sizes, ax=ax)
    nx.draw_networkx_edges(G, pos, alpha=0.3, ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=8, ax=ax)
    
    ax.set_title('Bank Customer Network Relationship')
    ax.axis('off')
    
    # Add legend
    from matplotlib.patches import Patch
    legend_elements = [
        Patch(facecolor='gold', label='Central Bank'),
        Patch(facecolor='lightblue', label='Branch'),
        Patch(facecolor='lightgreen', label='Customer')
    ]
    ax.legend(handles=legend_elements, loc='lower right')
    
    return fig, G

fig_bank_net, G_bank_net = banking_network()
plt.show()
```

### Real-world Example 2: Healthcare - Patient Care Network

```python
def healthcare_network():
    """Example: Patient referral network."""
    np.random.seed(42)
    
    G = nx.DiGraph()
    
    # Add healthcare providers
    providers = [
        ('GP_Dr_Smith', 'Primary Care', 100),
        ('Hospital_Central', 'Hospital', 200),
        ('Specialist_Cardio', 'Cardiologist', 80),
        ('Specialist_Ortho', 'Orthopedic', 75),
        ('Specialist_Neuro', 'Neurologist', 60),
        ('Lab_Alpha', 'Diagnostics', 150),
        ('Pharmacy_Beta', 'Pharmacy', 120)
    ]
    
    for p, specialty, patients in providers:
        G.add_node(p, specialty=specialty, size=patients * 10)
    
    # Add patient nodes
    for i in range(20):
        G.add_node(f'Patient_{i}', type='patient')
    
    # Add edges (referrals and visits)
    referral_pairs = [
        ('GP_Dr_Smith', 'Hospital_Central'),
        ('GP_Dr_Smith', 'Specialist_Cardio'),
        ('GP_Dr_Smith', 'Specialist_Ortho'),
        ('GP_Dr_Smith', 'Lab_Alpha'),
        ('Hospital_Central', 'Specialist_Neuro'),
        ('Specialist_Cardio', 'Lab_Alpha'),
        ('Specialist_Ortho', 'Lab_Alpha'),
        ('Specialist_Neuro', 'Lab_Alpha'),
    ]
    
    for src, dst in referral_pairs:
        G.add_edge(src, dst, weight=np.random.randint(5, 30))
    
    # Patient-provider edges
    np.random.seed(42)
    for i in range(20):
        gp = np.random.choice(['GP_Dr_Smith'])
        hospital = np.random.choice(['Hospital_Central', 'Lab_Alpha'], p=[0.3, 0.7])
        G.add_edge(f'Patient_{i}', gp, weight=1)
        if hospital:
            G.add_edge(f'Patient_{i}', hospital, weight=1)
    
    # Visualize
    fig, ax = plt.subplots(figsize=(14, 10))
    
    pos = nx.spring_layout(G, seed=42, k=2)
    
    # Color by type
    colors = []
    sizes = []
    for n in G.nodes():
        if G.nodes[n].get('specialty') == 'Primary Care':
            colors.append('lightblue')
            sizes.append(1500)
        elif G.nodes[n].get('specialty') == 'Hospital':
            colors.append('red')
            sizes.append(2000)
        elif G.nodes[n].get('specialty') == 'Cardiologist':
            colors.append('orange')
            sizes.append(1000)
        elif G.nodes[n].get('specialty') == 'Diagnostics':
            colors.append('purple')
            sizes.append(1000)
        elif 'Patient' in n:
            colors.append('lightgreen')
            sizes.append(400)
        else:
            colors.append('gray')
            sizes.append(800)
    
    nx.draw_networkx_nodes(G, pos, node_color=colors, node_size=sizes, ax=ax)
    nx.draw_networkx_edges(G, pos, alpha=0.3, ax=ax,
                       arrows=True, arrowsize=15)
    nx.draw_networkx_labels(G, pos, font_size=8, ax=ax)
    
    ax.set_title('Patient Care Referral Network')
    ax.axis('off')
    
    return fig, G

fig_health_net, G_health_net = healthcare_network()
plt.show()
```

## V. OUTPUT_RESULTS

### Expected Outputs

1. **Basic Network**: Shows connections between entities.

2. **Weighted Network**: Shows relationship strength through edge thickness.

3. **Directed Network**: Shows flow with arrows.

4. **Community Network**: Shows clusters with different colors.

5. **Interactive**: Plotly visualization with hover info.

6. **Social Analysis**: Centrality rankings and comparisons.

7. **Banking**: Hub-and-spoke customer relationship model.

8. **Healthcare**: Referral network showing patient flow.

## VI. VISUALIZATION

### Flow Chart

```
+--------------------+
|  DEFINE NODES     |----> Entities
+--------------------+
          |
          v
+--------------------+
|  DEFINE EDGES    |----> Relationships
+--------------------+
          |
          v
+--------------------+
|  CHOOSE LAYOUT   |----> spring/circular
+--------------------+
          |
          v
+--------------------+
|  VISUALIZE       |----> matplotlib/plotly
+--------------------+
```

## VII. CONCLUSION

### Key Takeaways

1. Networks reveal hidden relationships in data.

2. Centrality identifies key nodes.

3. Communities show cluster structure.

4. Interactive tools enhance exploration.

5. Domain-specific networks model real-world systems.

### Further Reading

- NetworkX Documentation
- Plotly Network Graphs
- Community Detection Algorithms