# Market Basket Analysis

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand market basket analysis and its business applications
- Prepare transaction data for association rule mining
- Interpret and act on discovered rules
- Visualize basket analysis results
- Apply market basket analysis to retail and e-commerce data
- Translate insights into business actions

## Theoretical Background

Market basket analysis is a specific application of association rule mining that analyzes customer purchasing patterns. By understanding which items are frequently purchased together, businesses can make strategic decisions about product placement, promotions, and recommendations.

### Business Applications

1. **Product Placement**: Store layout optimization
2. **Cross-selling**: Recommend related products
3. **Promotions**: Bundle items for discounts
4. **Inventory Management**: Predict demand patterns
5. **Customer Segmentation**: Understand purchase behavior

### Types of Analysis

#### Item-Based Analysis
- Which items are frequently bought together?
- Examples: Diapers → Beer, Bread → Milk

#### Time-Based Analysis
- What items are bought at different times?
- Weekly: Grocery restocking
- Monthly: Bulk buying

#### Customer-Based Analysis
- What types of customers buy similar items?
- Customer segmentation for targeted marketing

### Key Metrics Recap

| Metric | Definition | Business Meaning |
|--------|------------|-----------------|
| Support | P(X ∪ Y) | How common is this combination? |
| Confidence | P(Y\|X) | When X is bought, how often is Y bought? |
| Lift | Confidence / P(Y) | How much does X increase Y's probability? |

### Interesting Rules

A rule is interesting if:
1. **Objective**: Has statistical significance (lift ≠ 1)
2. **Subjective**: Actionable for the business
3. **Unexpected**: Surprising to domain experts

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("arules")
install.packages("arulesViz")
install.packages("tidyverse")
install.packages("reshape2")

library(arules)
library(arulesViz)
library(tidyverse)
library(reshape2)
```

### Step 2: Prepare Transaction Data

```r
# Create realistic retail transaction data
set.seed(42)
n_customers <- 500

# Define product categories and items
products <- list(
  groceries = c("milk", "bread", "eggs", "butter", "cheese"),
  beverages = c("coffee", "tea", "juice", "soda", "water"),
  snacks = c("chips", "cookies", "candy", "nuts"),
  household = c("paper", "soap", "detergent", "trash bags"),
  produce = c("apples", "bananas", "vegetables")
)

# Generate transactions
transactions <- list()

for(i in 1:n_customers) {
  cart <- character(0)
  
  # Each customer buys from 1-4 categories
  n_cats <- sample(1:4, 1)
  selected_cats <- sample(names(products), n_cats)
  
  # Items from selected categories
  for(cat in selected_cats) {
    # Buy 1-3 items from each category
    n_items <- sample(1:3, 1)
    cart <- c(cart, sample(products[[cat]], n_items))
  }
  
  transactions[[i]] <- unique(cart)
}

# Convert to transactions
basket <- as(transactions, "transactions")

# Item frequency
itemFrequencyPlot(basket, topN = 15, type = "absolute",
                  main = "Item Frequency")
```

### Step 3: Apply Association Rules

```r
# Find frequent itemsets first
itemsets <- apriori(basket, 
                    parameter = list(
                      supp = 0.05,
                      target = "frequent itemsets"
                    ))

# Sort by support
inspect(sort(itemsets, by = "support")[1:10])

# Find association rules
rules <- apriori(basket,
                 parameter = list(
                   supp = 0.03,
                   conf = 0.5,
                   target = "rules"
                 ))

# View summary
summary(rules)
# set of 89 rules

# Top rules by lift
top_rules <- sort(rules, by = "lift")[1:15]
inspect(top_rules)
```

### Step 4: Analyze Specific Patterns

```r
# Find rules leading to milk
milk_rules <- apriori(basket,
                      parameter = list(supp = 0.02, conf = 0.4),
                      appearance = list(rhs = "milk"))

inspect(sort(milk_rules, by = "lift"))

# Find rules from bread
bread_rules <- apriori(basket,
                       parameter = list(supp = 0.02, conf = 0.4),
                       appearance = list(lhs = "bread"))

inspect(sort(bread_rules, by = "confidence"))

# Find "anything to snacks" rules
snack_rules <- apriori(basket,
                       parameter = list(supp = 0.03, conf = 0.4),
                       appearance = list(rhs = c("chips", "cookies", "candy")))

inspect(sort(snack_rules, by = "support"))
```

### Step 5: Visualize Basket Analysis

```r
# Scatter plot of rules
plot(rules, 
     measure = c("support", "confidence"),
     shading = "lift")

# Grouped matrix plot
plot(rules, method = "grouped")

# Graph visualization for top rules
top_rules <- sort(rules, by = "lift")[1:20]
plot(top_rules, method = "graph")

# Parallel coordinates
plot(top_rules, method = "paracoord", 
     control = list(type = "items"))
```

## Code Examples

### Example 1: Store Layout Optimization

This example demonstrates using basket analysis for store layout.

```r
# Identify items that should be placed near each other
# Find strong associations

# Get top item pairs
pair_rules <- apriori(basket,
                      parameter = list(
                        supp = 0.05,
                        conf = 0.6,
                        minlen = 2,
                        maxlen = 2
                      ))

# Create adjacency matrix
rules_df <- as(pair_rules, "data.frame")

# Create network
library(igraph)

# Build graph from rules
edges <- rules_df[, c("lhs", "rhs", "lift")]
g <- graph_from_data_frame(edges, directed = FALSE)

# Plot network
plot(g, 
     layout = layout_with_fr,
     vertex.size = 30,
     vertex.color = "lightblue",
     edge.width = E(g)$lift,
     main = "Product Association Network")
```

### Example 2: Recommendation Engine

This example shows building a simple recommendation system.

```r
# For each item, find what else is commonly bought
# Create a simple recommender function

recommend <- function(item, rules_df, top_n = 5) {
  # Find rules where item is on LHS
  related <- rules_df[grepl(item, rules_df$lhs), ]
  
  if(nrow(related) == 0) {
    return("No recommendations available")
  }
  
  # Sort by lift
  related <- related[order(-related$lift), ]
  
  # Extract RHS items
  recs <- unique(related$rhs[1:top_n])
  return(recs)
}

# Test recommendations
recommend("milk", rules_df)
recommend("coffee", rules_df)
recommend("chips", rules_df)

# Create full recommendation table
all_items <- unique(c(rules_df$lhs, rules_df$rhs))
recommendations <- lapply(all_items, function(item) {
  recommend(item, rules_df, 3)
})
names(recommendations) <- all_items

# Display sample recommendations
head(recommendations, 10)
```

### Example 3: Customer Segment Analysis

This example shows analyzing different customer segments.

```r
# Analyze rules by customer type
# Split by customer value (inferred from basket size)

# Get transaction sizes
trans_sizes <- sizes(basket)

# Categorize customers
customer_type <- ifelse(trans_sizes <= 3, "Light",
                        ifelse(trans_sizes <= 6, "Medium", "Heavy"))

# Split and analyze
light_customers <- basket[customer_type == "Light"]
medium_customers <- basket[customer_type == "Medium"]
heavy_customers <- basket[customer_type == "Heavy"]

# Find rules for each segment
rules_light <- apriori(light_customers, parameter = list(supp = 0.05, conf = 0.5))
rules_medium <- apriori(medium_customers, parameter = list(supp = 0.05, conf = 0.5))
rules_heavy <- apriori(heavy_customers, parameter = list(supp = 0.05, conf = 0.5))

# Compare
cat("Light customer rules:", length(rules_light), "\n")
cat("Medium customer rules:", length(rules_medium), "\n")
cat("Heavy customer rules:", length(rules_heavy), "\n")

# Analyze differences
inspect(sort(rules_light, by = "lift")[1:5])
inspect(sort(rules_heavy, by = "lift")[1:5])
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Clean Data**: Remove returns and test transactions
2. **Appropriate Thresholds**: Based on business requirements
3. **Act on Rules**: Not all rules are actionable
4. **Test Recommendations**: A/B test before full rollout
5. **Refresh Regularly**: Customer behavior changes over time

### Common Pitfalls

1. **Too Many Rules**: Overwhelming to analyze
2. **Trivial Rules**: Common sense rules (milk → dairy)
3. **Single Store**: Results may not generalize
4. **Ignoring Business Context**: Rules must be profitable

## Performance Considerations

### Large-Scale Analysis

| Issue | Solution |
|-------|----------|
| Memory | Use sparse matrix representation |
| Speed | Increase support threshold |
| Too many rules | Filter by lift |
| Real-time | Pre-compute rules |

### Business Translation

| Insight | Action |
|---------|--------|
| A → B strong | Bundle together |
| A → B moderate | Cross-sell recommendations |
| Negative lift | Don't stock near each other |
| Time-based | Time promotions appropriately |

## Related Concepts

- **Collaborative Filtering**: User-based recommendations
- **Sequential Pattern Mining**: Order matters
- **Recommendation Systems**: Product suggestions
- **Customer Lifetime Value**: Long-term value analysis

## Exercise Problems

1. **Basic**: Run market basket analysis on grocery data.

2. **Intermediate**: Find rules for specific product categories.

3. **Advanced**: Create a simple recommendation engine.

4. **Real-World Challenge**: Analyze e-commerce transaction data.

5. **Extension**: Compare basket analysis across customer segments.