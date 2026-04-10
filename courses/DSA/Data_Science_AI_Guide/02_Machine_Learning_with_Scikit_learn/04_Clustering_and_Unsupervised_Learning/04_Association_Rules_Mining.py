# Topic: Association Rules Mining
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Association Rules Mining

================================================================================
I. INTRODUCTION
================================================================================

Association Rules Mining is a fundamental unsupervised learning technique used
to discover interesting relationships and patterns in large datasets. Originally
developed for market basket analysis, it has found applications across various
domains including banking, healthcare, web usage mining, and bioinformatics.

The primary goal of association rules mining is to find associations or
correlations between sets of items that appear together frequently in
transactions. For example, in retail, we might discover that customers who
buy bread and milk also tend to buy eggs - this is an association rule.

Key applications include:
- Market Basket Analysis: Understanding which products are purchased together
- Cross-selling strategies in banking (which financial products go together)
- Healthcare: Identifying symptom-disease co-occurrences
- Web usage: Understanding navigation patterns
- Bioinformatics: Gene expression patterns

================================================================================
II. CORE CONCEPTS
================================================================================

1. TRANSACTIONS AND ITEMSETS
--------------------------
- Transaction: A collection of items (e.g., shopping basket)
- Itemset: A set of items
- k-itemset: An itemset containing k items
- Frequent Itemset: Itemset with support >= minimum support threshold

2. SUPPORT MEASURE
-----------------
Support measures how frequently an itemset appears in transactions.
It is calculated as:
    Support(A) = Transactions containing A / Total transactions

Example: If "bread" appears in 200 out of 1000 transactions,
    Support({bread}) = 200/1000 = 0.2 (or 20%)

3. CONFIDENCE MEASURE
--------------------
Confidence measures the likelihood of item B being purchased when item A is purchased.
It is calculated as:
    Confidence(A -> B) = Support(A U B) / Support(A)

Example: If 200 transactions have bread, and 150 of those also have eggs,
    Confidence({bread} -> {eggs}) = 150/200 = 0.75 (or 75%)

4. LIFT MEASURE
--------------
Lift measures how much more likely item B is purchased when item A is purchased,
compared to the baseline likelihood of B. It accounts for the base probability.
    Lift(A -> B) = Confidence(A -> B) / Support(B)
    Lift(A -> B) = Support(A U B) / (Support(A) * Support(B))

- Lift = 1: No association (independent)
- Lift > 1: Positive association (items appear together more than expected)
- Lift < 1: Negative association (items appear together less than expected)

5. CONVICTION MEASURE
--------------------
Conviction measures the strength of the rule:
    Conviction(A -> B) = (1 - Support(B)) / (1 - Confidence(A -> B))

6. LEVERAGE MEASURE
-----------------
Leverage measures the difference between observed and expected co-occurrence:
    Leverage(A -> B) = Support(A U B) - (Support(A) * Support(B))

================================================================================
III. APRIORI ALGORITHM
================================================================================

The Apriori algorithm is the classic algorithm for frequent itemset mining.
It uses the anti-monotonicity property: If an itemset is frequent, then all its
subsets must also be frequent. Conversely, if an itemset is infrequent,
all its supersets are infrequent.

Algorithm Steps:
1. Generate candidate k-itemsets from frequent (k-1)-itemsets
2. Prune candidates whose subsets are not frequent
3. Scan database to count support for candidates
4. Keep candidates meeting minimum support threshold
5. Repeat until no new frequent itemsets found

Optimization Benefits:
- Reduces search space significantly
- Generates only potentially frequent itemsets
- Easy to implement and understand

================================================================================
IV. IMPLEMENTATION
================================================================================
"""

# Import necessary libraries
import numpy as np
import pandas as pd
from sklearn.datasets import make_blobs
from sklearn.preprocessing import StandardScaler
from itertools import combinations
from collections import defaultdict
import warnings
warnings.filterwarnings('ignore')


# ================================================================================
# SECTION 1: DATA GENERATION FUNCTIONS
# ================================================================================

def generate_transaction_data(n_transactions=1000, n_items=15, avg_transaction_size=5):
    """
    Generate synthetic market basket transaction data.
    
    Parameters:
    -----------
    n_transactions : int
        Number of transactions to generate
    n_items : int
        Number of unique items available
    avg_transaction_size : int
        Average number of items per transaction
    
    Returns:
    --------
    transactions : list of lists
        List of transactions, each containing item indices
    item_names : list
        List of item names
    """
    print(f"\n{'='*80}")
    print("DATA GENERATION: Transaction Data")
    print(f"{'='*80}")
    
    # Define realistic item names for various domains
    all_item_names = [
        'Bread', 'Milk', 'Eggs', 'Butter', 'Cheese', 'Yogurt',
        'Coffee', 'Tea', 'Juice', 'Soda', 'Water',
        'Chicken', 'Beef', 'Fish', 'Vegetables', 'Fruits',
        'Rice', 'Pasta', 'Cereal', 'Soup',
        'Detergent', 'Paper Towels', 'Tissue', 'Soap', 'Shampoo'
    ]
    
    # Select n_items items
    item_names = all_item_names[:n_items]
    print(f"Number of unique items: {n_items}")
    print(f"Items: {item_names}")
    
    # Create item probabilities to simulate realistic purchase patterns
    # Some items are more popular than others
    item_probs = np.random.exponential(scale=1.0, size=n_items)
    item_probs = item_probs / item_probs.sum()  # Normalize
    
    # Generate transactions
    transactions = []
    for i in range(n_transactions):
        # Determine transaction size with some randomness
        trans_size = max(1, int(np.random.poisson(avg_transaction_size))
        trans_size = min(trans_size, n_items)
        
        # Select items based on probabilities
        selected_indices = np.random.choice(
            n_items, size=trans_size, replace=False, p=item_probs
        )
        transaction = sorted([item_names[idx] for idx in selected_indices])
        transactions.append(transaction)
    
    # Print statistics
    transaction_lengths = [len(t) for t in transactions]
    print(f"Number of transactions generated: {n_transactions}")
    print(f"Average transaction size: {np.mean(transaction_lengths):.2f}")
    print(f"Min transaction size: {min(transaction_lengths)}")
    print(f"Max transaction size: {max_transaction_lengths}")
    
    return transactions, item_names


# Alternative generator for correlated itemsets (more interesting patterns)
def generate_correlated_transaction_data(n_transactions=1000, n_items=15):
    """
    Generate transaction data with built-in correlations for realistic patterns.
    
    This creates data where certain items are more likely to appear together,
    simulating real-world shopping behavior.
    
    Parameters:
    -----------
    n_transactions : int
        Number of transactions
    n_items : int
        Number of unique items
    
    Returns:
    --------
    transactions : list of lists
        Generated transactions
    """
    print(f"\n{'='*80}")
    print("DATA GENERATION: Correlated Transaction Data")
    print(f"{'='*80}")
    
    # Define item categories
    item_names = [
        'Bread', 'Milk', 'Eggs', 'Butter', 'Cheese', 'Yogurt',
        'Coffee', 'Tea', 'Juice', 'Soda', 'Chicken', 'Beef', 'Fish',
        'Rice', 'Pasta', 'Vegetables'
    ][:n_items]
    
    # Define correlated item groups (these will appear together often)
    # Group 1: Breakfast items
    breakfast_group = ['Bread', 'Butter', 'Eggs', 'Milk', 'Coffee']
    # Group 2: Dinner items
    dinner_group = ['Chicken', 'Beef', 'Rice', 'Vegetables', 'Pasta']
    # Group 3: Healthy items
    healthy_group = ['Milk', 'Yogurt', 'Fruits', 'Vegetables', 'Juice']
    # Group 4: Quick meals (if we have soda/tea)
    if 'Soda' in item_names and 'Tea' in item_names:
        snack_group = ['Bread', 'Cheese', 'Soda', 'Tea']
    
    # Generate transactions
    transactions = []
    for _ in range(n_transactions):
        transaction = []
        
        # Decide which groups to include (more than one possible)
        use_breakfast = np.random.random() < 0.4
        use_dinner = np.random.random() < 0.4
        use_healthy = np.random.random() < 0.3
        
        if use_breakfast:
            # Add breakfast items with some variability
            for item in breakfast_group:
                if np.random.random() < 0.75:
                    transaction.append(item)
        
        if use_dinner:
            for item in dinner_group:
                if np.random.random() < 0.75:
                    transaction.append(item)
        
        if use_healthy:
            for item in healthy_group:
                if np.random.random() < 0.6:
                    transaction.append(item)
        
        # Add at least one random item
        if len(transaction) < 2:
            extra = np.random.choice(item_names, size=1)
            transaction.extend(extra)
        
        # Remove duplicates and sort
        transaction = sorted(set(transaction))
        transactions.append(transaction)
    
    print(f"Generated {len(transactions)} transactions with built-in correlations")
    print(f"Sample transactions: {transactions[:3]}")
    
    return transactions, item_names


def max_transaction_lengths(transactions):
    """Helper function to get max length"""
    return max(len(t) for t in transactions)


# ================================================================================
# SECTION 2: APRIORI ALGORITHM IMPLEMENTATION
# ================================================================================

class TransactionEncoder:
    """
    Encoder for converting transactions to various formats for analysis.
    """
    
    def __init__(self, transactions, item_names=None):
        """
        Initialize with transaction data.
        
        Parameters:
        -----------
        transactions : list of lists
            List of transactions
        item_names : list, optional
            List of unique item names
        """
        self.transactions = transactions
        
        # Get all unique items if item_names not provided
        if item_names is None:
            all_items = set()
            for trans in transactions:
                all_items.update(trans)
            self.item_names = sorted(all_items)
        else:
            self.item_names = item_names
        
        # Create item to index mapping
        self.item_to_idx = {item: idx for idx, item in enumerate(self.item_names)}
        self.n_items = len(self.item_names)
        
        print(f"\nEncoded {len(transactions)} transactions with {self.n_items} unique items")
    
    def to_binary_matrix(self):
        """
        Convert transactions to binary matrix format.
        
        Returns:
        --------
        numpy.ndarray
            Binary matrix of shape (n_transactions, n_items)
        """
        n_transactions = len(self.transactions)
        matrix = np.zeros((n_transactions, self.n_items), dtype=np.int8)
        
        for i, trans in enumerate(self.transactions):
            for item in trans:
                if item in self.item_to_idx:
                    matrix[i, self.item_to_idx[item]] = 1
        
        return matrix
    
    def get_transaction_itemsets(self):
        """
        Get frozensets of items in each transaction.
        
        Returns:
        --------
        list of frozensets
            Each transaction as a frozenset
        """
        return [frozenset(trans) for trans in self.transactions]


def get_itemset_support(itemset, transactions):
    """
    Calculate support for an itemset.
    
    Parameters:
    -----------
    itemset : frozenset
        Itemset to measure support for
    transactions : list of frozensets
        List of transaction itemsets
    
    Returns:
    --------
    float
        Support value between 0 and 1
    """
    if len(itemset) == 0:
        return 1.0
    
    count = sum(1 for trans in transactions if itemset.issubset(trans))
    return count / len(transactions)


def get_all_itemsets(transactions, k):
    """
    Get all unique k-itemsets from transactions.
    
    Parameters:
    -----------
    transactions : list of frozensets
        List of transactions
    k : int
        Size of itemsets to generate
    
    Returns:
    --------
    set of frozensets
        Unique k-itemsets
    """
    itemsets = set()
    for trans in transactions:
        if len(trans) >= k:
            for combo in combinations(trans, k):
                itemsets.add(frozenset(combo))
    return itemsets


def apriori_generate_candidates(frequent_itemsets, k):
    """
    Generate candidate k-itemsets from frequent (k-1)-itemsets.
    
    Parameters:
    -----------
    frequent_itemsets : set of frozensets
        Set of frequent (k-1)-itemsets
    k : int
        Size of candidates to generate
    
    Returns:
    --------
    set of frozensets
        Candidate k-itemsets
    """
    candidates = set()
    frequent_list = list(frequent_itemsets)
    
    for i in range(len(frequent_list)):
        for j in range(i + 1, len(frequent_list)):
            # Get union of two (k-1)-itemsets
            union = frequent_list[i] | frequent_list[j]
            
            # If union has k items, add as candidate
            if len(union) == k:
                candidates.add(union)
    
    return candidates


def apriori_prune(itemset_candidates, frequent_itemsets, k):
    """
    Prune candidates that have infrequent subsets.
    
    Parameters:
    -----------
    itemset_candidates : set of frozensets
        Candidate itemsets to prune
    frequent_itemsets : set of frozensets
        Known frequent itemsets (of size k-1)
    k : int
        Current itemset size
    
    Returns:
    --------
    set of frozensets
        Pruned candidates
    """
    pruned = set()
    
    for candidate in itemset_candidates:
        # Check all (k-1)-subsets
        is_valid = True
        for subset in combinations(candidate, k - 1):
            subset_frozenset = frozenset(subset)
            if subset_frozenset not in frequent_itemsets:
                is_valid = False
                break
        
        if is_valid:
            pruned.add(candidate)
    
    return pruned


def apriori_algorithm(transactions, min_support=0.01, min_itemset_size=1, max_itemset_size=None,
                     verbose=True):
    """
    Implement Apriori algorithm for frequent itemset mining.
    
    Parameters:
    -----------
    transactions : list of lists or list of frozensets
        List of transactions
    min_support : float
        Minimum support threshold (0 to 1)
    min_itemset_size : int
        Minimum size of itemsets to find
    max_itemset_size : int, optional
        Maximum size of itemsets to find
    verbose : bool
        Whether to print progress
    
    Returns:
    --------
    dict
        Dictionary mapping itemset size to frequent itemsets with support
    """
    if verbose:
        print(f"\n{'='*80}")
        print("APRIORI ALGORITHM")
        print(f"{'='*80}")
        print(f"Minimum support: {min_support:.2%}")
        print(f"Minimum itemset size: {min_itemset_size}")
        print(f"Maximum itemset size: {max_itemset_size}")
        print(f"Number of transactions: {len(transactions)}")
    
    # Convert transactions to frozensets if needed
    if not isinstance(transactions[0], frozenset):
        transactions = [frozenset(trans) for trans in transactions]
    
    n_transactions = len(transactions)
    
    # If max_itemset_size not specified, use all items
    if max_itemset_size is None:
        max_itemset_size = max(len(t) for t in transactions)
    
    # Store frequent itemsets by size
    frequent_itemsets = {}
    
    # Step 1: Find frequent 1-itemsets
    if verbose:
        print("\n[Frequent 1-Itemsets]")
    
    item_counts = defaultdict(int)
    for trans in transactions:
        for item in trans:
            item_counts[item] += 1
    
    current_frequent = set()
    for item, count in item_counts.items():
        support = count / n_transactions
        if support >= min_support:
            current_frequent.add(frozenset([item]))
            frequent_itemsets[frozenset([item])] = support
    
    if verbose:
        print(f"Found {len(current_frequent)} frequent 1-itemsets")
    
    # Step 2: Iteratively find frequent k-itemsets
    k = 2
    while current_frequent and k <= max_itemset_size:
        if verbose:
            print(f"\n[Frequent {k}-Itemsets]")
        
        # Generate candidates
        candidates = apriori_generate_candidates(current_frequent, k)
        
        if verbose:
            print(f"Generated {len(candidates)} candidates")
        
        # Prune candidates
        candidates = apriori_prune(candidates, current_frequent, k)
        
        if verbose:
            print(f"After pruning: {len(candidates)} candidates")
        
        # Count support for candidates
        current_frequent = set()
        for candidate in candidates:
            support = get_itemset_support(candidate, transactions)
            if support >= min_support:
                current_frequent.add(candidate)
                frequent_itemsets[candidate] = support
        
        if verbose:
            print(f"Found {len(current_frequent)} frequent {k}-itemsets")
        
        k += 1
    
    if verbose:
        print(f"\nTotal frequent itemsets found: {len(frequent_itemsets)}")
    
    return frequent_itemsets


# ================================================================================
# SECTION 3: ASSOCIATION RULES GENERATION
# ================================================================================

def generate_association_rules(frequent_itemsets, min_confidence=0.5, min_lift=1.0,
                               transactions=None, verbose=True):
    """
    Generate association rules from frequent itemsets.
    
    Parameters:
    -----------
    frequent_itemsets : dict
        Dictionary of frequent itemsets with support values
    min_confidence : float
        Minimum confidence threshold
    min_lift : float
        Minimum lift threshold
    transactions : list of frozensets, optional
        Transaction data for computing metrics
    verbose : bool
        Whether to print progress
    
    Returns:
    --------
    pandas.DataFrame
        DataFrame containing association rules with metrics
    """
    if verbose:
        print(f"\n{'='*80}")
        print("ASSOCIATION RULES GENERATION")
        print(f"{'='*80}")
        print(f"Minimum confidence: {min_confidence:.2%}")
        print(f"Minimum lift: {min_lift:.2f}")
    
    rules = []
    n_transactions = len(transactions) if transactions else 1
    
    # Process each frequent itemset with 2+ items
    for itemset, support in frequent_itemsets.items():
        if len(itemset) < 2:
            continue
        
        # Generate all possible rules from this itemset
        for i in range(1, len(itemset)):
            # Get all combinations of antecedent size i
            for antecedent in combinations(itemset, i):
                antecedent = frozenset(antecedent)
                consequent = itemset - antecedent
                
                # Skip if either side is empty
                if len(antecedent) == 0 or len(consequent) == 0:
                    continue
                
                # Get support values
                support_a = frequent_itemsets.get(antecedent, 0)
                support_b = frequent_itemsets.get(consequent, 0)
                
                if support_a == 0:
                    continue
                
                # Calculate metrics
                confidence = support / support_a
                lift = confidence / support_b if support_b > 0 else 0
                
                # Calculate additional metrics
                leverage = support - (support_a * support_b)
                conviction = (1 - support_b) / (1 - confidence) if confidence < 1 else float('inf')
                
                # Apply filters
                if confidence < min_confidence or lift < min_lift:
                    continue
                
                rules.append({
                    'antecedent': set(antecedent),
                    'consequent': set(consequent),
                    'support': support,
                    'confidence': confidence,
                    'lift': lift,
                    'leverage': leverage,
                    'conviction': conviction,
                    'support_antecedent': support_a,
                    'support_consequent': support_b
                })
    
    # Convert to DataFrame
    if rules:
        rules_df = pd.DataFrame(rules)
        rules_df = rules_df.sort_values('lift', ascending=False)
        
        if verbose:
            print(f"Generated {len(rules_df)} association rules")
        
        return rules_df
    else:
        if verbose:
            print("No rules found meeting the thresholds")
        return pd.DataFrame()


def format_rule(rule_row, show_set=True):
    """
    Format an association rule for display.
    
    Parameters:
    -----------
    rule_row : pd.Series
        Row containing rule metrics
    show_set : bool
        Whether to show detailed set information
    
    Returns:
    --------
    str
        Formatted rule string
    """
    antecedent = ', '.join(sorted(rule_row['antecedent']))
    consequent = ', '.join(sorted(rule_row['consequent']))
    
    if show_set:
        return (f"{{{antecedent}}} -> {{{consequent}}}"
                f" | Support: {rule_row['support']:.4f}, "
                f"Confidence: {rule_row['confidence']:.4f}, "
                f"Lift: {rule_row['lift']:.4f}")
    else:
        return f"{{{antecedent}}} -> {{{consequent}}}"


# ================================================================================
# SECTION 4: EXTENDED METRICS AND EVALUATION
# ================================================================================

def calculate_all_rule_metrics(antecedent, consequent, transactions):
    """
    Calculate comprehensive metrics for an association rule.
    
    Parameters:
    -----------
    antecedent : frozenset or set
        Antecedent items
    consequent : frozenset or set
        Consequent items
    transactions : list of frozensets
        Transaction data
    
    Returns:
    --------
    dict
        Dictionary containing all metrics
    """
    n_transactions = len(transactions)
    
    antecedent = frozenset(antecedent) if not isinstance(antecedent, frozenset) else antecedent
    consequent = frozenset(consequent) if not isinstance(consequent, frozenset) else consequent
    
    # Count transactions
    n_a = sum(1 for t in transactions if antecedent.issubset(t))
    n_b = sum(1 for t in transactions if consequent.issubset(t))
    n_ab = sum(1 for t in transactions if antecedent.issubset(t) and consequent.issubset(t))
    
    # Calculate support
    support_a = n_a / n_transactions
    support_b = n_b / n_transactions
    support_ab = n_ab / n_transactions
    
    # Calculate confidence
    confidence = support_ab / support_a if support_a > 0 else 0
    
    # Calculate lift
    lift = confidence / support_b if support_b > 0 else 0
    
    # Calculate leverage
    leverage = support_ab - (support_a * support_b)
    
    # Calculate conviction
    expected_error_a_to_not_b = (1 - support_b)
    actual_error_a_to_not_b = (1 - confidence)
    conviction = expected_error_a_to_not_b / actual_error_a_to_not_b if actual_error_a_to_not_b > 0 else float('inf')
    
    # Calculate cosine
    cosine = support_ab / np.sqrt(support_a * support_b) if (support_a * support_b) > 0 else 0
    
    # Calculate Jaccard
    union = antecedent | consequent
    n_union = sum(1 for t in transactions if union.issubset(t))
    jaccard = n_ab / n_union if n_union > 0 else 0
    
    # Calculate certainty factor
    cf = (confidence - support_b) / (1 - support_b) if support_b < 1 else 0
    
    return {
        'support_antecedent': support_a,
        'support_consequent': support_b,
        'support_both': support_ab,
        'confidence': confidence,
        'lift': lift,
        'leverage': leverage,
        'conviction': conviction,
        'cosine': cosine,
        'jaccard': jaccard,
        'certainty_factor': cf
    }


def filter_rules_by_metric(rules_df, metric, threshold, direction='above'):
    """
    Filter rules based on a specific metric.
    
    Parameters:
    -----------
    rules_df : pd.DataFrame
        DataFrame containing rules
    metric : str
        Metric column name
    threshold : float
        Threshold value
    direction : str
        'above' or 'below'
    
    Returns:
    --------
    pd.DataFrame
        Filtered rules
    """
    if metric not in rules_df.columns:
        return rules_df
    
    if direction == 'above':
        return rules_df[rules_df[metric] >= threshold]
    else:
        return rules_df[rules_df[metric] <= threshold]


def get_top_rules(rules_df, metric='lift', n=10):
    """
    Get top N rules by a specific metric.
    
    Parameters:
    -----------
    rules_df : pd.DataFrame
        DataFrame containing rules
    metric : str
        Metric to sort by
    n : int
        Number of rules to return
    
    Returns:
    --------
    pd.DataFrame
        Top N rules
    """
    if rules_df.empty or metric not in rules_df.columns:
        return rules_df
    
    return rules_df.nlargest(n, metric)


# ================================================================================
# SECTION 5: BANKING AND FINANCIAL SERVICES EXAMPLE
# ================================================================================

def generate_banking_transaction_data(n_customers=1000):
    """
    Generate synthetic banking customer product holding data.
    
    Simulates which banking products different customer segments hold.
    
    Parameters:
    -----------
    n_customers : int
        Number of customer records
    
    Returns:
    --------
    transactions : list of lists
        List of product holdings per customer
    product_names : list
        List of banking product names
    """
    print(f"\n{'='*80}")
    print("BANKING EXAMPLE: Product Bundle Analysis")
    print(f"{'='*80}")
    
    # Define banking products
    products = [
        'Checking_Account', 'Savings_Account', 'Credit_Card', 'Debit_Card',
        'Personal_Loan', 'Mortgage', 'Auto_Loan', 'Investment_Account',
        'Fixed_Deposit', 'Term_Deposit', 'Home_Insurance', 'Life_Insurance',
        'Health_Insurance', 'Gold_Membership', 'Premium_Card',
        'Business_Account', 'Merchant_Services', 'Wealth_Management'
    ]
    
    # Define correlated product bundles (realistic banking behavior)
    # Basic banking: Most customers start with these
    basic_banking = ['Checking_Account', 'Debit_Card']
    
    # Premium customers: Higher value products
    premium_bundle = ['Premium_Card', 'Gold_Membership', 'Wealth_Management']
    
    # Loan seekers
    loan_bundle = ['Personal_Loan', 'Home_Insurance']
    mortgage_bundle = ['Mortgage', 'Home_Insurance', 'Life_Insurance']
    
    # Investment customers
    investment_bundle = ['Investment_Account', 'Fixed_Deposit', 'Term_Deposit']
    
    # Business customers
    business_bundle = ['Business_Account', 'Merchant_Services']
    
    transactions = []
    for _ in range(n_customers):
        customer_holdings = []
        
        # Assign customer segments
        rand = np.random.random()
        
        if rand < 0.5:
            # Basic customer (50%)
            customer_holdings.extend(basic_banking)
            if np.random.random() < 0.3:
                customer_holdings.append('Savings_Account')
            if np.random.random() < 0.2:
                customer_holdings.append('Credit_Card')
        
        elif rand < 0.7:
            # Loan customer (20%)
            customer_holdings.extend(basic_banking)
            if np.random.random() < 0.6:
                customer_holdings.extend(loan_bundle)
            elif np.random.random() < 0.4:
                customer_holdings.extend(mortgage_bundle)
        
        elif rand < 0.85:
            # Investment customer (15%)
            customer_holdings.extend(basic_banking)
            if np.random.random() < 0.7:
                customer_holdings.extend(investment_bundle)
            if np.random.random() < 0.3:
                customer_holdings.append('Wealth_Management')
        
        elif rand < 0.95:
            # Premium customer (10%)
            customer_holdings.extend(basic_banking)
            if np.random.random() < 0.8:
                customer_holdings.extend(premium_bundle)
            if np.random.random() < 0.5:
                customer_holdings.append('Credit_Card')
        
        else:
            # Business customer (5%)
            if np.random.random() < 0.7:
                customer_holdings.extend(business_bundle)
            if np.random.random() < 0.5:
                customer_holdings.extend(basic_banking)
        
        # Remove duplicates and sort
        customer_holdings = sorted(set(customer_holdings))
        transactions.append(customer_holdings)
    
    print(f"Generated {n_customers} customer product holding records")
    print(f"Products available: {len(products)}")
    print(f"Sample customer holdings: {transactions[:3]}")
    
    return transactions, products


def analyze_banking_rules(rules_df):
    """
    Analyze and interpret banking association rules.
    
    Parameters:
    -----------
    rules_df : pd.DataFrame
        DataFrame containing banking rules
    
    Returns:
    --------
    dict
        Analysis results
    """
    print(f"\n{'='*80}")
    print("BANKING RULES INTERPRETATION")
    print(f"{'='*80}")
    
    analysis = {
        'strongest_associations': [],
        'cross_selling_opportunities': [],
        'product_bundles': [],
        'high_value_rules': []
    }
    
    if rules_df.empty:
        return analysis
    
    # Find strong associations (high lift)
    top_rules = get_top_rules(rules_df, 'lift', 10)
    for _, row in top_rules.iterrows():
        ant = ', '.join(sorted(row['antecedent']))
        cons = ', '.join(sorted(row['consequent']))
        
        rule = {
            'antecedent': ant,
            'consequent': cons,
            'lift': row['lift'],
            'confidence': row['confidence'],
            'support': row['support'],
            'interpretation': ''
        }
        
        # Interpret the rule
        if row['lift'] > 2:
            rule['interpretation'] = f"Strong positive association: Customers with {ant} are {row['lift']:.2f}x more likely to have {cons}"
        elif row['lift'] > 1:
            rule['interpretation'] = f"Weak positive association: Slight increase in likelihood"
        
        analysis['strongest_associations'].append(rule)
        
        # High value rules (for cross-selling)
        if row['confidence'] > 0.5 and row['lift'] > 1.5:
            analysis['cross_selling_opportunities'].append({
                'target': ant,
                'recommendation': cons,
                'success_probability': row['confidence']
            })
    
    return analysis


def perform_banking_analysis(min_support=0.05, min_confidence=0.5):
    """
    Perform complete banking association rules analysis.
    
    Parameters:
    -----------
    min_support : float
        Minimum support threshold
    min_confidence : float
        Minimum confidence threshold
    
    Returns:
    --------
    dict
        Results containing rules and analysis
    """
    # Generate banking data
    transactions, products = generate_banking_transaction_data(n_customers=1000)
    
    # Run Apriori algorithm
    frequent_itemsets = apriori_algorithm(transactions, min_support=min_support, verbose=True)
    
    # Generate rules
    rules_df = generate_association_rules(
        frequent_itemsets, 
        min_confidence=min_confidence,
        min_lift=1.0,
        transactions=transactions,
        verbose=True
    )
    
    # Analyze rules
    analysis = analyze_banking_rules(rules_df)
    
    print(f"\n[Key Findings]")
    print(f"Total frequent itemsets: {len(frequent_itemsets)}")
    print(f"Total association rules: {len(rules_df)}")
    
    return {
        'frequent_itemsets': frequent_itemsets,
        'rules': rules_df,
        'analysis': analysis,
        'transactions': transactions,
        'products': products
    }


# ================================================================================
# SECTION 6: HEALTHCARE EXAMPLE
# ================================================================================

def generate_healthcare_transaction_data(n_patients=1000):
    """
    Generate synthetic healthcare patient symptom and diagnosis data.
    
    Simulates patient records with symptoms, conditions, and treatments.
    
    Parameters:
    -----------
    n_patients : int
        Number of patient records
    
    Returns:
    --------
    transactions : list of lists
        Patient symptom/treatment records
    items : list
        List of symptoms, diagnoses, and treatments
    """
    print(f"\n{'='*80}")
    print("HEALTHCARE EXAMPLE: Symptom Co-occurrence Analysis")
    print(f"{'='*80}")
    
    # Define healthcare items (symptoms, diagnoses, treatments)
    items = [
        'Fever', 'Cough', 'Fatigue', 'Headache', 'Nausea',
        'Chest_Pain', 'Shortness_Breath', 'Body_Aches', 'Sore_Throat',
        'Runny_Nose', 'Congestion', 'Diarrhea', 'Vomiting',
        'High_Blood_Pressure', 'Diabetes', 'Asthma', 'Anxiety',
        'Depression', 'Insomnia', 'Back_Pain', 'Joint_Pain',
        'Vitamin_D', 'Iron_Supplement', 'Pain_Reliever', 'Antibiotic',
        'Blood_Test', 'X_Ray', 'MRI', 'Physical_Therapy',
        'Lifestyle_Counseling', 'Follow_Up', 'Specialist_Referral'
    ]
    
    # Define correlated symptom/treatment groups
    # Common cold symptoms
    cold_group = ['Cough', 'Runny_Nose', 'Sore_Throat', 'Congestion']
    cold_treatment = ['Pain_Reliever', 'Vitamin_C']
    
    # Flu symptoms
    flu_group = ['Fever', 'Body_Aches', 'Fatigue', 'Headache']
    flu_treatment = ['Pain_Reliever', 'Rest', 'Fluid']
    
    # Chronic conditions
    chronic_care = ['High_Blood_Pressure', 'Diabetes']
    chronic_treatment = ['Lifestyle_Counseling', 'Follow_Up', 'Blood_Test']
    
    # Mental health
    mental_health = ['Anxiety', 'Insomnia', 'Fatigue']
    mental_treatment = ['Therapy', 'Lifestyle_Counseling']
    
    # Musculoskeletal
    musculoskeletal = ['Back_Pain', 'Joint_Pain']
    musculoskeletal_treatment = ['Physical_Therapy', 'Pain_Reliever', 'X_Ray']
    
    transactions = []
    for _ in range(n_patients):
        patient_record = []
        
        # Assign patient conditions based on probabilities
        rand = np.random.random()
        
        if rand < 0.35:
            # Respiratory illness (35%)
            patient_record.extend(cold_group)
            if np.random.random() < 0.5:
                patient_record.extend(flu_group)
            patient_record.extend(cold_treatment)
            if np.random.random() < 0.3:
                patient_record.append('Antibiotic')
        
        elif rand < 0.5:
            # Chronic conditions (15%)
            if np.random.random() < 0.6:
                patient_record.extend(chronic_care)
            patient_record.extend(chronic_treatment)
            if np.random.random() < 0.4:
                patient_record.append('Specialist_Referral')
        
        elif rand < 0.7:
            # Mental health (20%)
            patient_record.extend(mental_health)
            patient_record.append('Therapy')
            patient_record.append('Follow_Up')
        
        elif rand < 0.9:
            # Musculoskeletal (20%)
            patient_record.extend(musculoskeletal)
            patient_record.extend(musculoskeletal_treatment)
            if np.random.random() < 0.3:
                if 'MRI' not in patient_record:
                    patient_record.append('MRI')
        
        else:
            # Other/General checkup (10%)
            if np.random.random() < 0.5:
                patient_record.append('Fatigue')
            patient_record.append('Blood_Test')
            patient_record.append('Follow_Up')
        
        # Remove duplicates and sort
        patient_record = sorted(set(patient_record))
        transactions.append(patient_record)
    
    print(f"Generated {n_patients} patient records")
    print(f"Healthcare items tracked: {len(items)}")
    print(f"Sample patient records: {transactions[:3]}")
    
    return transactions, items


def analyze_healthcare_rules(rules_df):
    """
    Analyze and interpret healthcare association rules.
    
    Parameters:
    -----------
    rules_df : pd.DataFrame
        DataFrame containing healthcare rules
    
    Returns:
    --------
    dict
        Analysis results
    """
    print(f"\n{'='*80}")
    print("HEALTHCARE RULES INTERPRETATION")
    print(f"{'='*80}")
    
    analysis = {
        'symptom_syndrome': [],
        'treatment_patterns': [],
        'diagnostic_indicators': [],
        'referral_patterns': []
    }
    
    if rules_df.empty:
        return analysis
    
    # Find symptom co-occurrences
    symptom_keywords = ['Fever', 'Cough', 'Pain', 'Nausea', 'Fatigue', 'Headache']
    treatment_keywords = ['Vitamin', 'Antibiotic', 'Therapy', 'Test', 'Referral']
    
    for _, row in rules_df.iterrows():
        ant_str = ', '.join(row['antecedent'])
        cons_str = ', '.join(row['consequent'])
        
        if any(kw in ant_str for kw in symptom_keywords):
            analysis['symptom_syndrome'].append({
                'symptoms': ant_str,
                'associated': cons_str,
                'lift': row['lift'],
                'confidence': row['confidence']
            })
        
        if any(kw in cons_str for kw in treatment_keywords):
            if any(kw in ant_str for kw in symptom_keywords):
                analysis['treatment_patterns'].append({
                    'condition': ant_str,
                    'treatment': cons_str,
                    'lift': row['lift']
                })
    
    return analysis


def perform_healthcare_analysis(min_support=0.03, min_confidence=0.4):
    """
    Perform complete healthcare association rules analysis.
    
    Parameters:
    -----------
    min_support : float
        Minimum support threshold
    min_confidence : float
        Minimum confidence threshold
    
    Returns:
    --------
    dict
        Results containing rules and analysis
    """
    # Generate healthcare data
    transactions, items = generate_healthcare_transaction_data(n_patients=1000)
    
    # Run Apriori algorithm
    frequent_itemsets = apriori_algorithm(transactions, min_support=min_support, verbose=True)
    
    # Generate rules
    rules_df = generate_association_rules(
        frequent_itemsets,
        min_confidence=min_confidence,
        min_lift=1.0,
        transactions=transactions,
        verbose=True
    )
    
    # Analyze rules
    analysis = analyze_healthcare_rules(rules_df)
    
    print(f"\n[Key Findings]")
    print(f"Total frequent itemsets: {len(frequent_itemsets)}")
    print(f"Total association rules: {len(rules_df)}")
    
    return {
        'frequent_itemsets': frequent_itemsets,
        'rules': rules_df,
        'analysis': analysis,
        'transactions': transactions,
        'items': items
    }


# ================================================================================
# SECTION 7: ADVANCED TOPICS
# ================================================================================

class FPNode:
    """Node for FP-Tree structure"""
    
    def __init__(self, item, count=0, parent=None):
        self.item = item
        self.count = count
        self.parent = parent
        self.children = {}
        self.node_link = None


class FPTree:
    """FP-Tree implementation for frequent pattern mining"""
    
    def __init__(self, transactions, min_support=0.01):
        self.min_support = min_support
        self.n_transactions = len(transactions)
        self.min_count = int(min_support * self.n_transactions)
        
        # Build tree
        self.root = FPNode(None, 0, None)
        self.header_table = {}
        
        for trans in transactions:
            self._add_transaction(trans)
    
    def _add_transaction(self, transaction):
        """Add a transaction to the tree"""
        current = self.root
        
        for item in sorted(transaction):
            if item not in current.children:
                new_node = FPNode(item, 1, current)
                current.children[item] = new_node
                
                # Update header table
                if item not in self.header_table:
                    self.header_table[item] = new_node
                else:
                    # Link to existing node
                    node = self.header_table[item]
                    while node.node_link is not None:
                        node = node.node_link
                    node.node_link = new_node
            
            else:
                current.children[item].count += 1
            
            current = current.children[item]


def mine_fptree(tree, min_support, prefix=[]):
    """
    Mine frequent patterns from FP-Tree.
    
    This is an alternative to Apriori that can be more efficient
    for dense datasets.
    """
    # Base case
    if tree.children is None:
        return []
    
    results = []
    
    # Process each item in header table
    for item in tree.header_table:
        # Generate pattern
        new_pattern = prefix + [item]
        
        # Get conditional pattern base
        pattern_base = tree._get_conditional_pattern_base(item)
        
        # Add to results
        if len(pattern_base) >= tree.min_count:
            results.append((frozenset(new_pattern), pattern_base / tree.n_transactions))
        
        # Mine recursively
        results.extend(mine_fptree(tree, min_support, new_pattern))
    
    return results


def handle_large_transactions_efficiently(transactions, min_support=0.01):
    """
    Alternative implementation using transaction encoding for efficiency.
    
    Parameters:
    -----------
    transactions : list of lists
        Transaction data
    min_support : float
        Minimum support threshold
    
    Returns:
    --------
    dict
        Frequent itemsets with support
    """
    print(f"\n{'='*80}")
    print("EFFICIENT IMPLEMENTATION: Binary Matrix Approach")
    print(f"{'='*80}")
    
    encoder = TransactionEncoder(transactions)
    binary_matrix = encoder.to_binary_matrix()
    
    n_transactions = binary_matrix.shape[0]
    min_count = int(min_support * n_transactions)
    
    print(f"Matrix shape: {binary_matrix.shape}")
    print(f"Min count threshold: {min_count}")
    
    # Use pandas for efficient support calculation
    df = pd.DataFrame(binary_matrix, columns=encoder.item_names)
    
    frequent_itemsets = {}
    
    # Find 1-itemsets
    for col in df.columns:
        support = df[col].sum() / n_transactions
        if support >= min_support:
            frequent_itemsets[frozenset([col])] = support
    
    print(f"Frequent 1-itemsets: {len(frequent_itemsets)}")
    
    return frequent_itemsets


# ================================================================================
# SECTION 8: RULE MINING VARIATIONS
# ================================================================================

def mine_sequential_patterns(transactions, min_support=0.01):
    """
    Mine sequential patterns (for time-ordered data).
    
    This is useful for analyzing customer journeys,
    web navigation sequences, etc.
    """
    print(f"\n{'='*80}")
    print("SEQUENTIAL PATTERN MINING")
    print(f"{'='*80}")
    
    # Simplified implementation
    sequential_rules = []
    
    for i in range(len(transactions) - 1):
        current = frozenset(transactions[i])
        next_seq = frozenset(transactions[i + 1])
        
        if len(current) > 0 and len(next_seq) > 0:
            support = get_itemset_support(current | next_seq, transactions)
            
            if support >= min_support:
                sequential_rules.append({
                    'first': current,
                    'then': next_seq,
                    'support': support
                })
    
    return sequential_rules


def mine_multidimensional_associations(data_df, min_support=0.01):
    """
    Mine associations across multiple dimensions/attributes.
    
    Useful for analyzing categorical data with multiple attributes.
    """
    print(f"\n{'='*80}")
    print("MULTIDIMENSIONAL ASSOCIATION MINING")
    print(f"{'='*80}")
    
    # Convert categorical columns to transactions
    transactions = []
    
    for _, row in data_df.iterrows():
        transaction = [f"{col}={row[col]}" for col in data_df.columns]
        transactions.append(transaction)
    
    return apriori_algorithm(transactions, min_support=min_support)


# ================================================================================
# SECTION 9: VISUALIZATION AND REPORTING
# ================================================================================

def create_rules_summary(rules_df):
    """
    Create a summary report of association rules.
    
    Parameters:
    -----------
    rules_df : pd.DataFrame
        DataFrame containing rules
    
    Returns:
    --------
    str
        Formatted summary report
    """
    print(f"\n{'='*80}")
    print("RULES SUMMARY REPORT")
    print(f"{'='*80}")
    
    if rules_df.empty:
        return "No rules generated."
    
    lines = []
    lines.append(f"Total Rules: {len(rules_df)}")
    lines.append(f"")
    lines.append(f"Support Statistics:")
    lines.append(f"  Min: {rules_df['support'].min():.4f}")
    lines.append(f"  Max: {rules_df['support'].max():.4f}")
    lines.append(f"  Mean: {rules_df['support'].mean():.4f}")
    lines.append(f"")
    lines.append(f"Confidence Statistics:")
    lines.append(f"  Min: {rules_df['confidence'].min():.4f}")
    lines.append(f"  Max: {rules_df['confidence'].max():.4f}")
    lines.append(f"  Mean: {rules_df['confidence'].mean():.4f}")
    lines.append(f"")
    lines.append(f"Lift Statistics:")
    lines.append(f"  Min: {rules_df['lift'].min():.4f}")
    lines.append(f"  Max: {rules_df['lift'].max():.4f}")
    lines.append(f"  Mean: {rules_df['lift'].mean():.4f}")
    lines.append(f"")
    lines.append(f"Top 10 Rules by Lift:")
    
    top_rules = get_top_rules(rules_df, 'lift', 10)
    for i, (_, row) in enumerate(top_rules.iterrows(), 1):
        ant = ', '.join(sorted(row['antecedent']))
        cons = ', '.join(sorted(row['consequent']))
        lines.append(f"  {i}. {{{ant}}} -> {{{cons}}}")
        lines.append(f"     S: {row['support']:.4f}, C: {row['confidence']:.4f}, L: {row['lift']:.4f}")
    
    return '\n'.join(lines)


def export_rules_to_csv(rules_df, filename='association_rules.csv'):
    """
    Export rules to CSV file for further analysis.
    
    Parameters:
    -----------
    rules_df : pd.DataFrame
        DataFrame containing rules
    filename : str
        Output filename
    """
    if not rules_df.empty:
        rules_df.to_csv(filename, index=False)
        print(f"Exported rules to {filename}")


# ================================================================================
# SECTION 10: TESTING AND VALIDATION
# ================================================================================

def run_tests():
    """
    Run comprehensive tests for the association rules implementation.
    """
    print(f"\n{'='*80}")
    print("RUNNING TESTS")
    print(f"{'='*80}")
    
    tests_passed = 0
    tests_failed = 0
    
    # Test 1: Transaction data generation
    try:
        transactions, items = generate_transaction_data(n_transactions=100, n_items=10)
        assert len(transactions) == 100
        assert len(items) == 10
        print("[PASS] Test 1: Transaction data generation")
        tests_passed += 1
    except Exception as e:
        print(f"[FAIL] Test 1: {e}")
        tests_failed += 1
    
    # Test 2: Apriori algorithm
    try:
        transactions, _ = generate_transaction_data(n_transactions=200, n_items=8, avg_transaction_size=3)
        frequent_itemsets = apriori_algorithm(transactions, min_support=0.05, verbose=False)
        assert len(frequent_itemsets) > 0
        print("[PASS] Test 2: Apriori algorithm")
        tests_passed += 1
    except Exception as e:
        print(f"[FAIL] Test 2: {e}")
        tests_failed += 1
    
    # Test 3: Association rules generation
    try:
        transactions, _ = generate_transaction_data(n_transactions=200, n_items=8, avg_transaction_size=3)
        frequent_itemsets = apriori_algorithm(transactions, min_support=0.05, verbose=False)
        rules_df = generate_association_rules(frequent_itemsets, min_confidence=0.5,
                                                transactions=transactions, verbose=False)
        print("[PASS] Test 3: Association rules generation")
        tests_passed += 1
    except Exception as e:
        print(f"[FAIL] Test 3: {e}")
        tests_failed += 1
    
    # Test 4: Transaction encoder
    try:
        transactions, items = generate_transaction_data(n_transactions=100, n_items=10)
        encoder = TransactionEncoder(transactions, items)
        matrix = encoder.to_binary_matrix()
        assert matrix.shape == (100, 10)
        assert matrix.max() <= 1
        assert matrix.min() >= 0
        print("[PASS] Test 4: Transaction encoder")
        tests_passed += 1
    except Exception as e:
        print(f"[FAIL] Test 4: {e}")
        tests_failed += 1
    
    # Test 5: Metrics calculation
    try:
        transactions = [
            ['Bread', 'Milk', 'Eggs'],
            ['Bread', 'Eggs'],
            ['Milk', 'Eggs'],
            ['Bread', 'Milk', 'Eggs', 'Coffee'],
            ['Coffee']
        ]
        frequent_itemsets = apriori_algorithm(transactions, min_support=0.2, verbose=False)
        
        assert len(frequent_itemsets) > 0
        
        support_eggs = frequent_itemsets.get(frozenset(['Eggs']), 0)
        support_bread = frequent_itemsets.get(frozenset(['Bread']), 0)
        support_both = frequent_itemsets.get(frozenset(['Bread', 'Eggs']), 0)
        
        if support_both > 0:
            confidence = support_both / support_bread
            assert confidence > 0
        
        print("[PASS] Test 5: Metrics calculation")
        tests_passed += 1
    except Exception as e:
        print(f"[FAIL] Test 5: {e}")
        tests_failed += 1
    
    # Test 6: Banking example
    try:
        result = perform_banking_analysis(min_support=0.08, min_confidence=0.5)
        assert 'rules' in result
        print("[PASS] Test 6: Banking example")
        tests_passed += 1
    except Exception as e:
        print(f"[FAIL] Test 6: {e}")
        tests_failed += 1
    
    # Test 7: Healthcare example
    try:
        result = perform_healthcare_analysis(min_support=0.05, min_confidence=0.4)
        assert 'rules' in result
        print("[PASS] Test 7: Healthcare example")
        tests_passed += 1
    except Exception as e:
        print(f"[FAIL] Test 7: {e}")
        tests_failed += 1
    
    # Test 8: Correlated transaction data
    try:
        transactions, items = generate_correlated_transaction_data(n_transactions=150, n_items=12)
        frequent_itemsets = apriori_algorithm(transactions, min_support=0.1, verbose=False)
        assert len(frequent_itemsets) > 0
        print("[PASS] Test 8: Correlated transaction data")
        tests_passed += 1
    except Exception as e:
        print(f"[FAIL] Test 8: {e}")
        tests_failed += 1
    
    print(f"\n[TEST SUMMARY]")
    print(f"Tests Passed: {tests_passed}")
    print(f"Tests Failed: {tests_failed}")
    print(f"Total Tests: {tests_passed + tests_failed}")
    
    return tests_passed, tests_failed


# ================================================================================
# SECTION 11: MAIN EXECUTION
# ================================================================================

def main():
    """
    Main function to execute complete association rules mining implementation.
    """
    print("="*80)
    print("ASSOCIATION RULES MINING - COMPREHENSIVE IMPLEMENTATION")
    print("="*80)
    
    # Example 1: Basic transaction analysis
    print("\n" + "="*80)
    print("EXAMPLE 1: Basic Transaction Analysis")
    print("="*80)
    
    transactions, items = generate_transaction_data(
        n_transactions=1000,
        n_items=12,
        avg_transaction_size=4
    )
    
    # Run Apriori
    frequent_itemsets = apriori_algorithm(
        transactions,
        min_support=0.05,
        verbose=True
    )
    
    # Generate rules
    rules_df = generate_association_rules(
        frequent_itemsets,
        min_confidence=0.5,
        min_lift=1.0,
        transactions=transactions,
        verbose=True
    )
    
    # Print summary
    print(create_rules_summary(rules_df))
    
    # Example 2: Banking analysis
    print("\n" + "="*80)
    print("EXAMPLE 2: Banking Product Bundle Analysis")
    print("="*80)
    
    banking_result = perform_banking_analysis(
        min_support=0.05,
        min_confidence=0.5
    )
    
    # Example 3: Healthcare analysis
    print("\n" + "="*80)
    print("EXAMPLE 3: Healthcare Symptom Analysis")
    print("="*80)
    
    healthcare_result = perform_healthcare_analysis(
        min_support=0.03,
        min_confidence=0.4
    )
    
    # Run tests
    print("\n" + "="*80)
    print("RUNNING COMPREHENSIVE TESTS")
    print("="*80)
    
    run_tests()
    
    print("\n" + "="*80)
    print("IMPLEMENTATION COMPLETE")
    print("="*80)
    
    return {
        'frequent_itemsets': frequent_itemsets,
        'rules': rules_df,
        'banking_result': banking_result,
        'healthcare_result': healthcare_result
    }


if __name__ == "__main__":
    results = main()