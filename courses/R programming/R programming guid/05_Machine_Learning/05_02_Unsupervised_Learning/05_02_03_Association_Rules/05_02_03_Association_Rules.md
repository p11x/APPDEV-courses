# Association Rules

## Learning Objectives

- Understand association rule mining
- Perform market basket analysis

## Code Examples

```r
# Association rules

cat("===== ASSOCIATION RULES =====\n\n")

# Example: Market basket analysis
library(arules)

# Sample transaction data
trans <- list(
  c("bread", "milk", "eggs"),
  c("bread", "diaper", "beer"),
  c("milk", "diaper", "beer", "cola"),
  c("bread", "milk", "diaper", "beer"),
  c("bread", "cola")
)
trans <- as(trans, "transactions")

# Mine rules
cat("Mining association rules:\n")
rules <- apriori(trans, parameter = list(supp = 0.3, conf = 0.6))
inspect(rules)
```

## Best Practices

1. Set appropriate support and confidence
2. Interpret lift values
