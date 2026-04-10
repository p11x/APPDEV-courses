# Feature Engineering

## Learning Objectives

- Perform feature transformation
- Create new features
- Handle categorical variables

## Code Examples

```r
# Feature engineering

cat("===== FEATURE ENGINEERING =====\n\n")

# Create sample data
df <- data.frame(
  age = 25:35,
  salary = c(35000, 40000, 45000, 50000, 55000, 60000, 
             65000, 70000, 75000, 80000, 85000),
  education = sample(c("High School", "Bachelor", "Master"), 11, replace = TRUE)
)

# Create new features
df$age_squared <- df$age^2
df$salary_per_age <- df$salary / df$age
df$high_earner <- ifelse(df$salary > 60000, 1, 0)

cat("Created features:\n")
print(df)

# One-hot encoding
cat("\nOne-hot encoding of education:\n")
model.matrix(~ education - 1, df)
```

## Best Practices

1. Create meaningful features
2. Avoid data leakage
3. Document feature transformations
