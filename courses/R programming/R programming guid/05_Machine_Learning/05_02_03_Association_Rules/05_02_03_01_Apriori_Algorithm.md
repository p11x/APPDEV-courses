# Apriori Algorithm for Association Rule Mining

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand the fundamentals of association rule mining
- Implement the Apriori algorithm in R using the `arules` package
- Interpret support, confidence, and lift metrics
- Generate and filter association rules
- Apply the algorithm to real-world market basket data
- Understand the limitations and extensions of Apriori

## Theoretical Background

Association rule mining discovers interesting relationships among items in large transaction datasets. Originally developed for market basket analysis (e.g., "customers who buy diapers often also buy beer"), it has applications in web usage mining, fraud detection, and more.

### Key Concepts

#### Transaction Database

A collection of transactions, where each transaction is a set of items:
```
T1: {bread, milk}
T2: {bread, diapers, beer, eggs}
T3: {milk, diapers, beer, cola}
T4: {bread, milk, diapers, beer}
T5: {bread, milk, cola}
```

#### Itemset

A set of items. A k-itemset contains k items:
- 1-itemset: {bread}
- 2-itemset: {bread, milk}
- 3-itemset: {bread, milk, diapers}

#### Support

Fraction of transactions containing an itemset:
$$support(X) = \frac{|{t \in T: X \subseteq t}|}{|T|}$$

For example, support({bread, milk}) = 3/5 = 0.6 (60% of transactions)

#### Confidence

Conditional probability that a transaction containing X also contains Y:
$$confidence(X \rightarrow Y) = \frac{support(X \cup Y)}{support(X)}$$

For example, confidence({bread} → {milk}) = support({bread, milk}) / support({bread}) = 0.6 / 0.8 = 0.75

#### Lift

How much more likely Y is purchased when X is purchased:
$$lift(X \rightarrow Y) = \frac{confidence(X \rightarrow Y)}{support(Y)}$$

- lift > 1: Positive association
- lift = 1: No association
- lift < 1: Negative association

### The Apriori Principle

The Apriori principle states: **If an itemset is frequent, then all its subsets must also be frequent.**

Conversely: **If an itemset is infrequent, all its supersets must be infrequent.**

This principle allows pruning the search space significantly.

### The Apriori Algorithm

1. **Generate Frequent 1-itemsets**: Find all items meeting minimum support
2. **Generate Candidate k-itemsets**: Join frequent (k-1)-itemsets
3. **Prune Candidates**: Remove itemsets containing infrequent subsets
4. **Count Support**: Calculate support for remaining candidates
5. **Repeat**: Steps 2-4 until no frequent itemsets found

### Limitations

- Can generate many rules
- Sensitive to threshold settings
- Doesn't consider order or quantity
- Sparse data may have few associations

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("arules")
install.packages("arulesViz")
install.packages("ggplot2")

library(arules)
library(arulesViz)
library(ggplot2)
```

### Step 2: Prepare Transaction Data

```r
# Create sample transaction data
# Each row is a transaction with items
transactions <- list(
  c("bread", "milk"),
  c("bread", "diapers", "beer", "eggs"),
  c("milk", "diapers", "beer", "cola"),
  c("bread", "milk", "diapers", "beer"),
  c("bread", "milk", "cola"),
  c("bread", "diapers", "cola"),
  c("milk", "diapers", "cola"),
  c("bread", "diapers")
)

# Convert to transaction format
trans <- as(transactions, "transactions")

# Inspect
inspect(trans)
#   items                       
# 1 {bread, milk}              
# 2 {bread, diapers, beer, eggs}
# ...
```

### Step 3: Apply Apriori Algorithm

```r
# Run Apriori with minimum support and confidence
set.seed(42)
rules <- apriori(trans, 
                parameter = list(
                  supp = 0.3,      # Minimum support
                  conf = 0.6,      # Minimum confidence
                  target = "rules"
                ))

# View rules
inspect(rules)
#     lhs           rhs     support confidence     lift
# 1 {bread}     => {milk}    0.625      0.833  1.111 
# 2 {diapers}   => {beer}    0.500      0.714  1.190 
# ...

# Summary
summary(rules)
# set of 12 rules
```

### Step 4: Analyze Rules

```r
# Sort by lift
rules_sorted <- sort(rules, by = "lift", decreasing = TRUE)
inspect(head(rules_sorted, 5))

# Filter rules with specific items
# Rules leading to beer
beer_rules <- apriori(trans, 
                      parameter = list(supp = 0.2, conf = 0.5),
                      appearance = list(rhs = "beer"))
inspect(beer_rules)

# Rules from diapers
diapers_rules <- apriori(trans, 
                        parameter = list(supp = 0.3, conf = 0.6),
                        appearance = list(lhs = "diapers"))
inspect(diapers_rules)
```

### Step 5: Visualize Rules

```r
# Plot rules as scatter plot
plot(rules, 
     measure = c("support", "confidence"),
     shading = "lift")

# Grouped matrix plot
plot(rules, method = "grouped")

# Graph-based plot
plot(rules, method = "graph")

# Parallel coordinates plot
plot(rules, method = "paracoord")
```

## Code Examples

### Example 1: Grocery Store Data

This example demonstrates Apriori on grocery transaction data.

```r
# Create synthetic grocery data
set.seed(123)
n_transactions <- 200

# Common item combinations
grocery_data <- list()

for(i in 1:n_transactions) {
  items <- character(0)
  
  # Bread and milk often together
  if(runif(1) < 0.6) items <- c(items, "bread")
  if(runif(1) < 0.5) items <- c(items, "milk")
  
  # Beer and diapers often together
  if(runif(1) < 0.3) items <- c(items, "diapers")
  if("diapers" %in% items && runif(1) < 0.7) items <- c(items, "beer")
  
  # Coffee
  if(runif(1) < 0.4) items <- c(items, "coffee")
  
  # Vegetables
  if(runif(1) < 0.5) items <- c(items, "vegetables")
  
  # Fruits
  if(runif(1) < 0.4) items <- c(items, "fruits")
  
  # Add some randomness
  if(length(items) == 0) items <- sample(c("bread", "milk", "coffee"), 1)
  
  grocery_data[[i]] <- unique(items)
}

# Convert to transactions
grocery_trans <- as(grocery_data, "transactions")

# Item frequency plot
itemFrequencyPlot(grocery_trans, topN = 10)

# Apply Apriori
set.seed(42)
grocery_rules <- apriori(grocery_trans, 
                         parameter = list(supp = 0.1, conf = 0.5))

# Top rules by lift
grocery_sorted <- sort(grocery_rules, by = "lift")
inspect(head(grocery_sorted, 10))
```

### Example 2: Online Retail Data

This example shows analysis of online retail transactions.

```r
# Create synthetic e-commerce data
set.seed(456)
n <- 300

# Simulate customer purchase patterns
retail_data <- list()

for(i in 1:n) {
  items <- c()
  
  # User behavior clusters
  cluster <- sample(1:3, 1, prob = c(0.4, 0.35, 0.25))
  
  if(cluster == 1) {
    # Electronics shopper
    items <- c(items, sample(c("laptop", "mouse", "keyboard", "headphones"), 
                              size = sample(1:3, 1), replace = FALSE))
  } else if(cluster == 2) {
    # Clothing shopper  
    items <- c(items, sample(c("shirt", "pants", "shoes", "jacket"), 
                            size = sample(1:3, 1), replace = FALSE))
  } else {
    # Books shopper
    items <- c(items, sample(c("novel", "textbook", "magazine"), 
                            size = sample(1:2, 1), replace = FALSE))
  }
  
  retail_data[[i]] <- items
}

# Convert to transactions
retail_trans <- as(retail_data, "transactions")

# Run Apriori
set.seed(42)
retail_rules <- apriori(retail_trans, 
                        parameter = list(supp = 0.15, conf = 0.6))

# Top rules
inspect(sort(retail_rules, by = "lift")[1:10])
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Start with High Support**: Filter to frequent items first
2. **Filter Rules**: Focus on actionable, interesting rules
3. **Check Lift**: Only consider rules with lift > 1
4. **Use Domain Knowledge**: Interpret rules in business context

### Common Pitfalls

1. **Low Support Threshold**: Generates too many rules
2. **Ignoring Lift**: Rules may be spurious
3. **Large Itemsets**: Can be computationally expensive
4. **Not Considering Business**: Rules must be actionable

## Performance Considerations

### Computational Complexity

- **Time**: O(m × n × k) where m = itemsets, n = transactions, k = average size
- **Space**: O(n × average_transaction_size)

### Parameter Selection

| Parameter | Low Value | High Value |
|-----------|-----------|-----------|
| Support | More rules, slower | Fewer rules, faster |
| Confidence | More rules, less reliable | Fewer rules, more reliable |

## Related Concepts

- **Eclat Algorithm**: Different frequent itemset mining approach
- **FP-Growth**: More efficient for dense datasets
- **Association Rules**: The output format
- **Sequential Patterns**: Considers order of items

## Exercise Problems

1. **Basic**: Apply Apriori to simple transaction data.

2. **Intermediate**: Find rules with different support/confidence thresholds.

3. **Advanced**: Visualize and interpret rule networks.

4. **Real-World Challenge**: Analyze grocery store transactions.

5. **Extension**: Compare with Eclat algorithm.