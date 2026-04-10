# What is R?

## Learning Objectives

- Understand what R programming language is and its primary purposes
- Identify the key characteristics that make R unique among programming languages
- Recognize the main domains where R excels
- Understand the R ecosystem and its package system

## Theoretical Background

R is a programming language and environment specifically designed for statistical computing and graphics. It was created by Ross Ihaka and Robert Gentleman at the University of Auckland, New Zealand, as a statistical implementation of the S language. R provides a wide variety of statistical (linear and nonlinear modelling, classical statistical tests, time-series analysis, classification, clustering) and graphical techniques, and is highly extensible.

### Key Characteristics of R

1. **Statistical Foundation**: R was built by statisticians for statisticians, making it the natural choice for data analysis
2. **Open Source**: R is part of the GNU project, freely available under GNU General Public License
3. **Cross-Platform**: Works on Windows, macOS, Linux, and Unix systems
4. **Interpreted Language**: R executes code line-by-line without compilation
5. **Vectorized Operations**: R performs operations on entire vectors efficiently
6. **Dynamic Typing**: Variables are not declared with specific types
7. **Memory Management**: R handles memory automatically through garbage collection

### The R Ecosystem

The R ecosystem consists of:
- The R core team and language
- CRAN (Comprehensive R Archive Network) - repository of packages
- Bioconductor - packages for biological research
- RStudio and other IDEs
- The tidyverse and other package collections

## Step-by-Step Explanation

### Understanding R's Architecture

R operates as an interpreter that reads commands and executes them immediately. Here's how it works:

1. **User Interface**: You type commands in the R console or R script file
2. **Parser**: R parses the commands into internal representations
3. **Interpreter**: The parsed commands are executed
4. **Output**: Results are displayed or graphics are rendered

### Basic R Session

When you start R, you enter an interactive session:

```
R version 4.3.1 (2023-06-16) -- "Beagle Scouts"
Copyright (C) 2023 The R Foundation for Computational Statistics
...
> 
```

The `>` symbol is the prompt where you enter commands.

## Code Examples

### Standard Example: Hello World in R

```r
# The most basic R command - printing text to the console
# This demonstrates R's print function and string handling

# Method 1: Using print() function
print("Hello, World!")

# Method 2: Using cat() function (better for simple output)
# cat() automatically adds a newline, print() doesn't by default
cat("Hello, World!\n")

# Method 3: Using paste() for string concatenation
greeting <- "Hello"
target <- "World"
message <- paste(greeting, target, sep = ", ")
cat(message, "\n")
```

**Output:**
```
[1] "Hello, World!"
Hello, World!
Hello, World!
```

**Comments:**
- `[1]` in the first output indicates the first element of the output vector
- `print()` returns its input invisibly and is used for side effects
- `cat()` is better for simple output as it handles multiple arguments

### Real-World Example 1: Basic Data Analysis

```r
# Real-world application: Analyzing sales data
# This demonstrates R's statistical capabilities

# Create a numeric vector representing monthly sales (in thousands)
sales <- c(45, 52, 48, 55, 60, 65, 70, 68, 72, 75, 80, 85)

# Calculate basic statistics
mean_sales <- mean(sales)        # Average monthly sales
median_sales <- median(sales)    # Median monthly sales
sd_sales <- sd(sales)            # Standard deviation
min_sales <- min(sales)          # Minimum
max_sales <- max(sales)          # Maximum

# Print summary
cat("Monthly Sales Analysis\n")
cat("======================\n")
cat("Mean:", mean_sales, "thousand dollars\n")
cat("Median:", median_sales, "thousand dollars\n")
cat("Standard Deviation:", sd_sales, "\n")
cat("Range:", min_sales, "to", max_sales, "\n")

# Calculate growth rate
growth_rate <- (last(sales) - first(sales)) / first(sales) * 100
cat("Annual Growth Rate:", growth_rate, "%\n")
```

**Output:**
```
Monthly Sales Analysis
======================
Mean: 64.58333 thousand dollars
Median: 66.5 thousand dollars
Standard Deviation: 12.27811
Range: 45 to 85
Annual Growth Rate: 88.88889 %
```

**Comments:**
- `c()` creates a vector (R's basic data structure)
- Functions like `mean()`, `median()`, `sd()` are built-in statistical functions
- `first()` and `last()` are from dplyr package; base R would use `sales[1]` and `sales[length(sales)]`

### Real-World Example 2: Data Visualization

```r
# Real-world application: Creating a bar chart of product sales
# This demonstrates R's graphics capabilities

# Create sample data
products <- c("Product A", "Product B", "Product C", "Product D", "Product E")
sales_amount <- c(15000, 23000, 18000, 29000, 21000)

# Simple bar chart using base R graphics
barplot(sales_amount,
        names.arg = products,
        col = c("steelblue", "darkorange", "forestgreen", "firebrick", "purple"),
        main = "Product Sales Comparison",
        xlab = "Products",
        ylab = "Sales (in dollars)",
        ylim = c(0, 35000))

# Add value labels on top of bars
text(x = seq_along(sales_amount),
     y = sales_amount + 1000,
     labels = paste0("$", format(sales_amount, big.mark = ",")),
     pos = 3,
     cex = 0.8)
```

**Output:** A bar chart visualization (image would display)

**Comments:**
- `barplot()` is base R's function for creating bar charts
- `seq_along()` generates indices for each element
- `paste0()` concatenates strings without separator
- `format()` adds thousand separators to numbers

## Best Practices and Common Pitfalls

### Best Practices

1. **Use meaningful variable names**: `monthly_sales` is better than `x` or `ms`
2. **Comment your code**: Explain why you're doing something, not what
3. **Use vectorized operations**: Avoid loops when possible for performance
4. **Leverage packages**: Don't reinvent statistical methods
5. **Reproducible research**: Use R Markdown for reports

### Common Pitfalls

1. **Using `=` instead of `<-`**: While `=` works, `<-` is the standard
2. **Forgetting vector recycling**: R recycles shorter vectors, which can cause unexpected results
3. **Indexing from 1**: Unlike Python, R indexes from 1
4. **NA handling**: Always consider how NA values affect your analysis
5. **Package conflicts**: Use `conflicts(detail = TRUE)` to check for conflicts

## Performance Considerations

- R is memory-intensive; work with subsets when possible
- Vectorized operations are much faster than loops
- The `data.table` package offers faster alternatives for large datasets
- Profile your code with `Rprof()` to identify bottlenecks

## Related Concepts and Further Reading

- **R Documentation**: `help(function_name)` or `?function_name`
- **R Journal**: https://journal.r-project.org/
- **R Magazine**: https://r-magazine.com/
- **RStudio Cheatsheets**: https://www.rstudio.com/resources/cheatsheets/

## Exercise Problems

1. **Exercise 1**: Calculate the mean, median, and standard deviation of the vector `c(10, 20, 30, 40, 50)` using R.

2. **Exercise 2**: Create a vector of 100 random numbers between 1 and 100, then find the maximum and minimum values.

3. **Exercise 3**: Write R code to convert temperature from Celsius to Fahrenheit using the formula: F = C * 9/5 + 32

4. **Exercise 4**: Create a simple plot showing the relationship between a company's advertising spend and revenue (create sample data).

5. **Exercise 5**: Research and install the `tidyverse` package, then explore what's included.
