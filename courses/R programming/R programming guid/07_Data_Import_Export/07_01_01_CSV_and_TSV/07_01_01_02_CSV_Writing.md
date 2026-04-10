# Writing CSV Files in R

## Learning Objectives

- Write data frames to CSV format
- Control delimiters and quoting behavior
- Handle row names and headers properly
- Manage NA values in output
- Optimize for large data exports

## Theory

Writing CSV files in R is the reverse operation of reading. The primary functions are write.csv(), write.csv2(), write.table(), and the readr equivalents write_csv(), write_tsv().

Key considerations include: preserving data types (all become character when written), handling special characters (quotes, delimiters, newlines), controlling line endings, and managing missing values.

## Step-by-Step

1. Prepare data: check types, handle missing values
2. Choose writing function based on delimiter needs
3. Configure quote handling
4. Set header row options
5. Specify line ending if needed
6. Verify output file

## Code Examples

### Basic CSV Writing

```r
cat("===== BASIC CSV WRITING =====\n\n")

# Create sample data
df <- data.frame(
  id = 1:5,
  name = c("Alice", "Bob", "Carol", "Dave", "Eve"),
  score = c(95, 87, 92, 78, 88),
  passed = c(TRUE, TRUE, TRUE, FALSE, TRUE)
)

# Write CSV with defaults
write.csv(df, "output_basic.csv", row.names = FALSE)
cat("Basic CSV written\n")

# Write with quotes around character columns
write.csv(df, "output_quoted.csv", row.names = FALSE, quote = TRUE)
cat("CSV with quotes written\n")

# Using write.table for more control
write.table(
  df,
  "output_table.csv",
  row.names = FALSE,
  sep = ",",
  na = "NA",
  quote = TRUE
)
cat("Using write.table\n")
```

### Writing with Different Delimiters

```r
cat("\n===== DIFFERENT DELIMITERS =====\n\n")

# Tab-separated
write.table(df, "output.tsv", row.names = FALSE, sep = "\t", na = "NA")
cat("TSV written\n")

# Semicolon-separated
write.csv2(df, "output_eu.csv", row.names = FALSE)
cat("European format CSV written\n")

# Pipe-separated
write.table(df, "output_pipe.csv", row.names = FALSE, sep = "|", na = "NA")
cat("Pipe-separated file written\n")
```

### Controlling Quote Behavior

```r
cat("\n===== QUOTE CONTROL =====\n\n")

# Data with special characters
df_special <- data.frame(
  text = c('Hello, World', 'She said "hi"', "It's fine", "Normal text"),
  value = 1:4
)

# Write without quoting (only if needed)
write.csv(df_special, "no_extra_quotes.csv", row.names = FALSE, quote = FALSE)
cat("Written without extra quotes\n")

# Force quoting all fields
write.csv(df_special, "all_quotes.csv", row.names = FALSE, quote = TRUE)
cat("Written with all fields quoted\n")

# Using qmethod for escaping
write.csv(df_special, "escaped.csv", row.names = FALSE, qmethod = "double")
cat("Written with doublequote escaping\n")
```

### Handling NA Values

```r
cat("\n===== NA VALUE HANDLING =====\n\n")

df_na <- data.frame(
  x = c(1, 2, NA, 4, 5),
  y = c("a", NA, "c", "d", "e"),
  z = c(TRUE, FALSE, TRUE, NA, FALSE)
)

# Default NA handling
write.csv(df_na, "na_default.csv", row.names = FALSE)
cat("Default NA handling\n")

# Custom NA representation
write.csv(df_na, "na_custom.csv", row.names = FALSE, na = "MISSING")
cat("Custom NA string: MISSING\n")

# Empty string for NA
write.csv(df_na, "na_empty.csv", row.names = FALSE, na = "")
cat("Empty string for NA\n")
```

## Real-World Example: Export Analysis Results

```r
# Real-world: Export statistical analysis results
cat("===== EXPORT ANALYSIS RESULTS =====\n\n")

# Run analysis on mtcars
results <- by(mtcars, mtcars$cyl, function(x) {
  data.frame(
    count = nrow(x),
    mean_mpg = mean(x$mpg),
    mean_wt = mean(x$wt),
    sd_mpg = sd(x$mpg),
    sd_wt = sd(x$wt)
  )
})
results_df <- do.call(rbind, results)
results_df$cyl <- as.numeric(rownames(results_df))
rownames(results_df) <- NULL

print(results_df)

# Export with proper formatting
write.csv(
  results_df,
  "analysis_results.csv",
  row.names = FALSE,
  na = "NA",
  quote = FALSE
)
cat("\nAnalysis exported to analysis_results.csv\n")
```

## Best Practices

1. Always use row.names = FALSE unless row names are needed
2. Check for special characters in data before writing
3. Use consistent NA representation
4. Consider readr::write_csv for better performance
5. Verify output with read.csv after writing
6. Use append = TRUE for incremental writes

## Exercises

1. Write a data frame with 10,000 rows and compare performance
2. Export data with mixed date formats to CSV
3. Create code to append new data to existing CSV
4. Write a function that exports multiple data frames to separate CSV files
5. Export data ensuring proper handling of quotes in text fields