# Basic Plots in R

## Learning Objectives

- Create basic plots in base R
- Use plot(), hist(), boxplot()
- Customize basic visualizations

## Code Examples

```r
# Basic plots in R

cat("===== BASIC PLOTS =====\n\n")

# Scatter plot
x <- 1:10
y <- c(2, 4, 3, 5, 6, 8, 7, 9, 10, 11)

cat("1. Scatter plot:\n")
plot(x, y, main = "Scatter Plot", xlab = "X", ylab = "Y")

cat("\n2. Line plot:\n")
plot(x, y, type = "l", main = "Line Plot")

cat("\n3. Bar plot:\n")
barplot(c(10, 20, 15, 25), names.arg = c("A", "B", "C", "D"))

cat("\n4. Histogram:\n")
hist(rnorm(100), main = "Histogram")

cat("\n5. Box plot:\n")
boxplot(mtcars$mpg, main = "Box Plot")
```

## Best Practices

1. Choose appropriate plot type
2. Add clear labels
3. Consider visualization purpose
