# ANOVA and MANOVA

## Learning Objectives

- Perform one-way and two-way ANOVA
- Understand MANOVA for multiple dependent variables
- Interpret ANOVA tables

## Code Examples

```r
# ANOVA examples
set.seed(42)

cat("===== ANOVA =====\n\n")

# Create sample data
df <- data.frame(
  treatment = rep(c("A", "B", "C"), each = 10),
  value = c(rnorm(10, mean = 50, sd = 5),
            rnorm(10, mean = 55, sd = 5),
            rnorm(10, mean = 60, sd = 5))
)

# One-way ANOVA
cat("One-way ANOVA:\n")
model <- aov(value ~ treatment, data = df)
print(summary(model))

# Two-way ANOVA
df2 <- data.frame(
  treatment = rep(rep(c("A", "B", "C"), each = 5), 2),
  gender = rep(c("M", "F"), each = 15),
  value = rnorm(30, mean = 50, sd = 10)
)

cat("\nTwo-way ANOVA:\n")
model2 <- aov(value ~ treatment + gender, data = df2)
print(summary(model2))
```

## Best Practices

1. Check ANOVA assumptions (normality, homogeneity)
2. Use post-hoc tests for pairwise comparisons
