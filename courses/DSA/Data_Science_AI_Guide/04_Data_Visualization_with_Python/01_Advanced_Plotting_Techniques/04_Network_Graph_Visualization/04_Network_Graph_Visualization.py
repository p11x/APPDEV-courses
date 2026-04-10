# Topic: Network Graph Visualization
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Network Graph Visualization

I. INTRODUCTION
This module covers network graph visualization techniques including
basic graphs, directed graphs, weighted graphs, and interactive
network visualizations.

II. CORE CONCEPTS
- Graph theory fundamentals
- Node and edge visualization
- Layout algorithms
- Graph metrics and centrality

III. IMPLEMENTATION
- NetworkX for graph creation
- matplotlib for visualization
- plotly for interactive graphs

IV. EXAMPLES
- Banking: Transaction networks, customer relationships
- Healthcare: Drug interactions, disease transmission

V. OUTPUT RESULTS
- Network visualizations

VI. TESTING
- Graph algorithms

VII. ADVANCED TOPICS
- Large network optimization
- Community detection

VIII. CONCLUSION
Best practices for network visualization
"""

import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
import pandas as pd
import networkx as nx
import plotly.graph_objects as go
import warnings
warnings.filterwarnings('ignore')

plt.style.use('seaborn-v0_8-whitegrid')


def generate_random_graph(n_nodes=50, n_edges=100, seed=42):
    """Generate random graph data."""
    np.random.seed(seed)
    G = nx.gnm_random_graph(n_nodes, n_edges, seed=seed)
    
    for node in G.nodes():
        G.nodes[node]['weight'] = np.random.randint(1, 100)
    
    for u, v in G.edges():
        G.edges[u, v]['weight'] = np.random.randint(1, 10)
    
    return G


def generate_scale_free_graph(n_nodes=50, seed=42):
    """Generate scale-free graph."""
    np.random.seed(seed)
    G = nx.barabasi_albert_graph(n_nodes, 2, seed=seed)
    
    for node in G.nodes():
        G.nodes[node]['size'] = np.random.randint(10, 100)
    
    return G


def generate_community_graph(n_communities=3, nodes_per_community=20):
    """Generate graph with communities."""
    np.random.seed(42)
    
    G = nx.Graph()
    
    for i in range(n_communities):
        for j in range(nodes_per_community):
            G.add_node(i * nodes_per_community + j, community=i)
    
    for i in range(n_communities):
        nodes = [i * nodes_per_community + j for j in range(nodes_per_community)]
        
        for u in nodes:
            for v in nodes:
                if u < v and np.random.random() < 0.3:
                    G.add_edge(u, v)
        
        if i < n_communities - 1:
            cross_edges = np.random.randint(1, 5)
            for _ in range(cross_edges):
                u = np.random.choice(nodes)
                v = np.random.choice([(i+1) * nodes_per_community + j for j in range(nodes_per_community)])
                G.add_edge(u, v)
    
    return G


def create_basic_network_graph(G, title='Network Graph'):
    """Create basic network visualization."""
    fig, ax = plt.subplots(figsize=(12, 10))
    
    pos = nx.spring_layout(G, k=0.5, iterations=50)
    
    node_sizes = [G.nodes[node].get('weight', 50) for node in G.nodes()]
    
    nx.draw_networkx_nodes(G, pos, node_size=node_sizes, node_color='steelblue',
                         alpha=0.7, ax=ax)
    nx.draw_networkx_edges(G, pos, alpha=0.3, edge_color='gray', ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=8, ax=ax)
    
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.axis('off')
    
    plt.tight_layout()
    return fig


def create_directed_graph():
    """Create directed graph visualization."""
    G = nx.DiGraph()
    
    edges = [(0, 1), (0, 2), (1, 2), (1, 3), (2, 3), (3, 4), (4, 0)]
    G.add_edges_from(edges)
    
    fig, ax = plt.subplots(figsize=(10, 8))
    
    pos = nx.spring_layout(G)
    
    nx.draw_networkx_nodes(G, pos, node_color='steelblue', node_size=700,
                         alpha=0.7, ax=ax)
    nx.draw_networkx_edges(G, pos, edge_color='red', alpha=0.7,
                         arrows=True, arrowsize=20, ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=12, ax=ax)
    
    ax.set_title('Directed Network Graph', fontsize=14, fontweight='bold')
    ax.axis('off')
    
    plt.tight_layout()
    return fig


def create_weighted_graph():
    """Create weighted graph visualization."""
    G = nx.Graph()
    
    nodes = ['A', 'B', 'C', 'D', 'E', 'F']
    G.add_nodes_from(nodes)
    
    edges = [('A', 'B', 4), ('A', 'C', 2), ('B', 'C', 1), ('B', 'D', 3),
            ('C', 'D', 5), ('C', 'E', 6), ('D', 'E', 2), ('D', 'F', 4),
            ('E', 'F', 3)]
    G.add_weighted_edges_from(edges)
    
    fig, ax = plt.subplots(figsize=(10, 8))
    
    pos = nx.circular_layout(G)
    
    weights = [G.edges[u, v]['weight'] for u, v in G.edges()]
    max_weight = max(weights)
    normalized_weights = [w / max_weight * 10 + 1 for w in weights]
    
    nx.draw_networkx_nodes(G, pos, node_color='steelblue', node_size=800,
                         alpha=0.7, ax=ax)
    nx.draw_networkx_edges(G, pos, width=normalized_weights, edge_color='gray',
                         alpha=0.7, ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=12, ax=ax)
    nx.draw_networkx_edge_labels(G, pos, {(u, v): G.edges[u, v]['weight'] for u, v in G.edges()},
                          font_size=10, ax=ax)
    
    ax.set_title('Weighted Network Graph', fontsize=14, fontweight='bold')
    ax.axis('off')
    
    plt.tight_layout()
    return fig


def create_community_graph(G, title='Community Network'):
    """Create community graph with colors."""
    fig, ax = plt.subplots(figsize=(12, 10))
    
    pos = nx.spring_layout(G, k=0.5, iterations=50)
    
    communities = {}
    for node in G.nodes():
        community = G.nodes[node].get('community', 0)
        if community not in communities:
            communities[community] = []
        communities[community].append(node)
    
    colors = ['steelblue', 'red', 'green', 'orange', 'purple']
    
    for community, nodes in communities.items():
        nx.draw_networkx_nodes(G, pos, nodelist=nodes,
                         node_color=colors[community % len(colors)],
                         node_size=200, alpha=0.7, ax=ax)
    
    nx.draw_networkx_edges(G, pos, alpha=0.2, edge_color='gray', ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=8, ax=ax)
    
    ax.set_title(title, fontsize=14, fontweight='bold')
    ax.axis('off')
    
    plt.tight_layout()
    return fig


def create_ego_graph():
    """Create ego network visualization."""
    G = generate_random_graph(30, 50)
    
    ego_node = 0
    ego = nx.ego_graph(G, ego_node, radius=2)
    
    fig, ax = plt.subplots(figsize=(12, 10))
    
    pos = nx.spring_layout(G, k=0.5, iterations=50)
    
    non_ego_nodes = [n for n in G.nodes() if n != ego_node]
    nx.draw_networkx_nodes(G, pos, nodelist=non_ego_nodes, node_color='lightgray',
                         node_size=100, alpha=0.5, ax=ax)
    
    nx.draw_networkx_nodes(G, pos, nodelist=[ego_node], node_color='red',
                         node_size=500, alpha=0.9, ax=ax)
    
    nx.draw_networkx_nodes(G, pos, nodelist=list(ego.nodes()),
                         node_color='steelblue', node_size=200, alpha=0.7, ax=ax)
    
    nx.draw_networkx_edges(G, pos, alpha=0.2, edge_color='gray', ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=8, ax=ax)
    
    ax.set_title(f'Ego Network (Node {ego_node})', fontsize=14, fontweight='bold')
    ax.axis('off')
    
    plt.tight_layout()
    return fig


def create_interactive_network(G, title='Interactive Network'):
    """Create interactive network using Plotly."""
    pos = nx.spring_layout(G, k=0.5, iterations=50)
    
    edge_x = []
    edge_y = []
    for edge in G.edges():
        x0, y0 = pos[edge[0]]
        x1, y1 = pos[edge[1]]
        edge_x.extend([x0, x1, None])
        edge_y.extend([y0, y1, None])
    
    edge_trace = go.Scatter(
        x=edge_x, y=edge_y,
        line=dict(width=0.5, color='#888'),
        hoverinfo='none',
        mode='lines')
    
    node_x = [pos[node][0] for node in G.nodes()]
    node_y = [pos[node][1] for node in G.nodes()]
    
    node_trace = go.Scatter(
        x=node_x, y=node_y,
        mode='markers',
        hoverinfo='text',
        marker=dict(
            showscale=True,
            colorscale='YlOrRd',
            size=10,
            color=[G.nodes[node].get('weight', 50) for node in G.nodes()],
            colorbar=dict(thickness=15, title='Node Size', xanchor='left', titleside='right'),
            line_width=2))
    
    node_text = [f'Node {node}<br>Weight: {G.nodes[node].get("weight", 50)}'
                for node in G.nodes()]
    node_trace.text = node_text
    
    fig = go.Figure(data=[edge_trace, node_trace],
                   layout=go.Layout(
                       title=title,
                       titlefont_size=16,
                       showlegend=False,
                       hovermode='closest',
                       margin=dict(b=20, l=5, r=5, t=40),
                       xaxis=dict(showgrid=False, zeroline=False, showticklabels=False),
                       yaxis=dict(showgrid=False, zeroline=False, showticklabels=False)))
    
    return fig


def create_graph_metrics(G, title='Graph Metrics Dashboard'):
    """Create dashboard showing graph metrics."""
    fig, axes = plt.subplots(2, 2, figsize=(14, 12))
    
    degrees = [G.degree(n) for n in G.nodes()]
    axes[0, 0].hist(degrees, bins=20, color='steelblue', edgecolor='white')
    axes[0, 0].set_xlabel('Degree')
    axes[0, 0].set_ylabel('Frequency')
    axes[0, 0].set_title('Degree Distribution')
    axes[0, 0].grid(True, alpha=0.3)
    
    degree_centrality = nx.degree_centrality(G)
    cent_values = list(degree_centrality.values())
    axes[0, 1].hist(cent_values, bins=20, color='red', edgecolor='white')
    axes[0, 1].set_xlabel('Centrality')
    axes[0, 1].set_ylabel('Frequency')
    axes[0, 1].set_title('Degree Centrality Distribution')
    axes[0, 1].grid(True, alpha=0.3)
    
    betweenness = nx.betweenness_centrality(G)
    betw_values = list(betweenness.values())
    axes[1, 0].hist(betw_values, bins=20, color='green', edgecolor='white')
    axes[1, 0].set_xlabel('Centrality')
    axes[1, 0].set_ylabel('Frequency')
    axes[1, 0].set_title('Betweenness Centrality Distribution')
    axes[1, 0].grid(True, alpha=0.3)
    
    clustering = nx.clustering(G)
    clust_values = list(clustering.values())
    axes[1, 1].hist(clust_values, bins=20, color='orange', edgecolor='white')
    axes[1, 1].set_xlabel('Coefficient')
    axes[1, 1].set_ylabel('Frequency')
    axes[1, 1].set_title('Clustering Coefficient Distribution')
    axes[1, 1].grid(True, alpha=0.3)
    
    fig.suptitle(title, fontsize=16, fontweight='bold')
    plt.tight_layout()
    
    return fig


def create_layered_graph():
    """Create layered graph visualization."""
    L = 3
    G = nx.DiGraph()
    
    layers = [[], [], []]
    for i in range(L):
        nodes_in_layer = 4
        for j in range(nodes_in_layer):
            node_id = i * nodes_in_layer + j
            G.add_node(node_id, layer=i)
            layers[i].append(node_id)
    
    for i in range(L - 1):
        for src in layers[i]:
            for dst in layers[i + 1]:
                if np.random.random() < 0.4:
                    G.add_edge(src, dst)
    
    for i in range(L):
        for j, u in enumerate(layers[i]):
            for v in layers[i][j+1:]:
                if np.random.random() < 0.2:
                    G.add_edge(u, v)
    
    fig, ax = plt.subplots(figsize=(14, 10))
    
    pos = {}
    y_positions = [(L - 1 - i) * 2 for i in range(L)]
    for i, layer in enumerate(layers):
        x_positions = np.linspace(-3, 3, len(layer))
        for j, node in enumerate(layer):
            pos[node] = (x_positions[j], y_positions[i])
    
    layer_colors = ['steelblue', 'green', 'red']
    for i, layer in enumerate(layers):
        nx.draw_networkx_nodes(G, pos, nodelist=layer,
                             node_color=layer_colors[i],
                             node_size=500, alpha=0.7, ax=ax)
    
    nx.draw_networkx_edges(G, pos, alpha=0.3, edge_color='gray',
                         arrows=True, arrowsize=15, ax=ax)
    nx.draw_networkx_labels(G, pos, font_size=10, ax=ax)
    
    layer_names = ['Input', 'Hidden', 'Output']
    for i, name in enumerate(layer_names):
        ax.annotate(name, xy=(0, (L - 1 - i) * 2), xytext=(-5, 0),
                   textcoords='offset points', fontsize=12,
                   fontweight='bold', va='center')
    
    ax.set_title('Layered Neural Network', fontsize=14, fontweight='bold')
    ax.axis('off')
    
    plt.tight_layout()
    return fig


def core_implementation():
    """Core network visualization implementation."""
    G = generate_random_graph(30, 50)
    scale_free = generate_scale_free_graph(30)
    communities = generate_community_graph(3, 15)
    
    results = {}
    
    results['basic'] = create_basic_network_graph(G, 'Random Network')
    results['directed'] = create_directed_graph()
    results['weighted'] = create_weighted_graph()
    results['community'] = create_community_graph(communities)
    results['ego'] = create_ego_graph()
    results['metrics'] = create_graph_metrics(G)
    
    return results


def banking_example():
    """Banking application - Transaction Network."""
    np.random.seed(123)
    
    G = nx.DiGraph()
    
    n_accounts = 30
    for i in range(n_accounts):
        G.add_node(i, balance=np.random.randint(1000, 100000),
                   account_type=np.random.choice(['Checking', 'Savings', 'Investment']))
    
    for i in range(n_accounts):
        n_transactions = np.random.randint(0, 5)
        for _ in range(n_transactions):
            dest = np.random.randint(0, n_accounts)
            if dest != i:
                amount = np.random.randint(100, 10000)
                G.add_edge(i, dest, amount=amount)
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1)
    pos = nx.spring_layout(G, k=0.5)
    
    account_types = {'Checking': 'blue', 'Savings': 'green', 'Investment': 'red'}
    colors = [account_types[G.nodes[n].get('account_type', 'Checking')] for n in G.nodes()]
    balances = [G.nodes[n].get('balance', 10000) for n in G.nodes()]
    
    nx.draw_networkx_nodes(G, pos, node_color=colors, node_size=[b/500 for b in balances],
                          alpha=0.7, ax=ax1)
    nx.draw_networkx_edges(G, pos, alpha=0.3, edge_color='gray',
                          arrows=True, arrowsize=10, ax=ax1)
    ax1.set_title('Transaction Network by Account Type')
    ax1.axis('off')
    
    ax2 = fig.add_subplot(2, 3, 2)
    degrees = [G.out_degree(n) for n in G.nodes()]
    ax2.hist(degrees, bins=15, color='steelblue', edgecolor='white')
    ax2.set_xlabel('Out-Degree')
    ax2.set_ylabel('Frequency')
    ax2.set_title('Outgoing Transactions')
    ax2.grid(True, alpha=0.3)
    
    ax3 = fig.add_subplot(2, 3, 3)
    in_degrees = [G.in_degree(n) for n in G.nodes()]
    ax3.hist(in_degrees, bins=15, color='green', edgecolor='white')
    ax3.set_xlabel('In-Degree')
    ax3.set_ylabel('Frequency')
    ax3.set_title('Incoming Transactions')
    ax3.grid(True, alpha=0.3)
    
    ax4 = fig.add_subplot(2, 3, 4)
    balances = [G.nodes[n].get('balance', 10000) for n in G.nodes()]
    ax4.scatter([G.out_degree(n) for n in G.nodes()], balances,
               c='red', alpha=0.5, s=50)
    ax4.set_xlabel('Out-Degree')
    ax4.set_ylabel('Account Balance ($)')
    ax4.set_title('Activity vs Balance')
    ax4.grid(True, alpha=0.3)
    
    ax5 = fig.add_subplot(2, 3, 5)
    amounts = [G.edges[u, v].get('amount', 1000) for u, v in G.edges()]
    ax5.hist(amounts, bins=20, color='orange', edgecolor='white')
    ax5.set_xlabel('Transaction Amount ($)')
    ax5.set_ylabel('Frequency')
    ax5.set_title('Transaction Amount Distribution')
    ax5.grid(True, alpha=0.3)
    
    ax6 = fig.add_subplot(2, 3, 6)
    centrality = nx.degree_centrality(G)
    high_centrality = [(n, c) for n, c in centrality.items() if c > 0.3]
    if high_centrality:
        nodes, cents = zip(*high_centrality)
        ax6.bar(nodes, cents, color='purple')
    ax6.set_xlabel('Node ID')
    ax6.set_ylabel('Centrality')
    ax6.set_title('High Centrality Nodes')
    ax6.grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    print("=" * 60)
    print("TRANSACTION NETWORK ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Nodes (Accounts): {G.number_of_nodes()}")
    print(f"Total Edges (Transactions): {G.number_of_edges()}")
    print(f"Average Degree: {sum(dict(G.degree()).values()) / G.number_of_nodes():.2f}")
    print(f"Total Transaction Volume: ${sum(amounts):,}")
    print(f"Average Transaction: ${np.mean(amounts):,.0f}")
    
    return fig, G


def healthcare_example():
    """Healthcare application - Drug Interaction Network."""
    np.random.seed(456)
    
    G = nx.Graph()
    
    drugs = ['Aspirin', 'Ibuprofen', 'Acetaminophen', 'Warfarin', 'Insulin',
             'Metformin', 'Lisinopril', 'Atorvastatin', 'Amoxicillin', 'Omeprazole']
    
    for drug in drugs:
        G.add_node(drug, uses=np.random.randint(100, 10000),
                   side_effects=np.random.randint(0, 10))
    
    interactions = [
        ('Aspirin', 'Warfarin', 5), ('Aspirin', 'Ibuprofen', 3),
        ('Metformin', 'Insulin', 4), ('Lisinopril', 'Warfarin', 2),
        ('Atorvastatin', 'Metformin', 1), ('Amoxicillin', 'Omeprazole', 2),
        ('Aspirin', 'Acetaminophen', 1), ('Insulin', 'Metformin', 3)
    ]
    
    for d1, d2, severity in interactions:
        G.add_edge(d1, d2, severity=severity)
    
    fig = plt.figure(figsize=(18, 14))
    
    ax1 = fig.add_subplot(2, 3, 1)
    pos = nx.spring_layout(G, k=1, iterations=50)
    
    uses = [G.nodes[drug].get('uses', 1000) for drug in drugs]
    node_sizes = [u / 50 for u in uses]
    
    nx.draw_networkx_nodes(G, pos, node_color='steelblue',
                         node_size=node_sizes, alpha=0.7, ax=ax1)
    nx.draw_networkx_edges(G, pos, alpha=0.5, edge_color='gray', ax=ax1)
    nx.draw_networkx_labels(G, pos, font_size=10, ax=ax1)
    ax1.set_title('Drug Interaction Network')
    ax1.axis('off')
    
    ax2 = fig.add_subplot(2, 3, 2)
    degrees = [G.degree(drug) for drug in drugs]
    ax2.barh(drugs, degrees, color='steelblue')
    ax2.set_xlabel('Number of Interactions')
    ax2.set_title('Drug Interactions')
    ax2.invert_yaxis()
    ax2.grid(True, alpha=0.3)
    
    ax3 = fig.add_subplot(2, 3, 3)
    categories = {}
    for drug in drugs:
        uses = G.nodes[drug].get('uses', 1000)
        category = 'High' if uses > 5000 else 'Medium' if uses > 2000 else 'Low'
        if category not in categories:
            categories[category] = 0
        categories[category] += 1
    
    colors = {'High': 'red', 'Medium': 'orange', 'Low': 'green'}
    ax3.pie(list(categories.values()), labels=list(categories.keys()),
            colors=[colors[c] for c in categories.keys()],
            autopct='%1.1f%%', startangle=90)
    ax3.set_title('Drug Usage Categories')
    
    ax4 = fig.add_subplot(2, 3, 4)
    severity_weights = []
    for u, v in G.edges():
        severity = G.edges[u, v].get('severity', 1)
        severity_weights.append(severity)
    
    edge_list = list(G.edges())
    combined = list(zip(edge_list, severity_weights))
    combined.sort(key=lambda x: x[1], reverse=True)
    
    labels = [f'{e[0][0]}-{e[0][1]}' for e, s in combined[:5]]
    values = [s for e, s in combined[:5]]
    
    ax4.barh(labels, values, color='red')
    ax4.set_xlabel('Severity')
    ax4.set_title('Top Drug Interactions')
    ax4.invert_yaxis()
    ax4.grid(True, alpha=0.3)
    
    ax5 = fig.add_subplot(2, 3, 5)
    centrality = nx.degree_centrality(G)
    cent_values = list(centrality.values())
    drug_names = list(centrality.keys())
    ax5.bar(drug_names, cent_values, color='purple')
    ax5.set_xlabel('Drug')
    ax5.set_ylabel('Centrality')
    ax5.set_title('Drug Degree Centrality')
    plt.sca(ax5)
    plt.xticks(rotation=45, ha='right')
    ax5.grid(True, alpha=0.3)
    
    ax6 = fig.add_subplot(2, 3, 6)
    cluster_coef = nx.clustering(G)
    coefs = list(cluster_coef.values())
    drugs_list = list(cluster_coef.keys())
    ax6.scatter(drugs_list, coefs, c='green', s=100)
    ax6.set_xlabel('Drug')
    ax6.set_ylabel('Clustering Coefficient')
    ax6.set_title('Drug Clustering')
    plt.sca(ax6)
    plt.xticks(rotation=45, ha='right')
    ax6.grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    print("=" * 60)
    print("DRUG INTERACTION NETWORK ANALYSIS")
    print("=" * 60)
    print(f"\nTotal Drugs: {G.number_of_nodes()}")
    print(f"Total Interactions: {G.number_of_edges()}")
    print(f"Network Density: {nx.density(G):.3f}")
    print(f"Average Clustering: {np.mean(list(cluster_coef.values())):.3f}")
    print(f"\nMost Central Drug: {max(centrality, key=centrality.get)}")
    print(f"Most Connected Drug: {max(G.degree(), key=lambda x: x[1])[0]}")
    
    return fig, G


def main():
    """Main execution function."""
    print("Executing Network Graph Visualization implementation")
    print("=" * 60)
    
    G = generate_random_graph(30, 50)
    
    create_basic_network_graph(G, 'Random Network')
    create_directed_graph()
    create_weighted_graph()
    create_ego_graph()
    create_layered_graph()
    
    plt.show()
    
    return G


if __name__ == "__main__":
    main()