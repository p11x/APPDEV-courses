# Save and Load RData in R

## Learning Objectives

- Use save() to save R objects to file
- Use load() to restore saved objects
- Understand .RData workspace files
- Apply save.image() for complete workspace
- Handle compression and version compatibility

## Theoretical Background

### Save/Load Functions

R provides three main approaches for saving and loading data:

1. **save()**: Save specific objects to file
2. **load()**: Load saved .RData files
3. **save.image()**: Save entire workspace

### File Formats

| Format | Extension | Contents | Size |
|--------|-----------|----------|------|
| .RData | .RData | Multiple objects | Compressed |
| .rda | .rda | Same as .RData | Compressed |
| .rds | .rds | Single object | Variable |

### Compression

- compress = TRUE: Uses gzip compression
- compress = FALSE: No compression, faster
- compression_level: 1-9 (1=fastest, 9=smallest)

## Code Examples

### Basic Example: save() Function

```r
# ===== SAVE FUNCTION =====

cat("===== SAVE FUNCTION =====\n\n")

# Create sample objects
vector1 <- c(1, 2, 3, 4, 5)
matrix1 <- matrix(1:9, nrow = 3)
df1 <- data.frame(
  name = c("Alice", "Bob", "Charlie"),
  age = c(25, 30, 35),
  score = c(85, 92, 78)
)
list1 <- list(
  data = df1,
  summary = "Example list"
)

# Save to file
temp_file <- tempfile(fileext = ".RData")

# 1. Save single object
cat("1. Save single object:\n")
save(df1, file = temp_file)
cat("  Saved df1 to", temp_file, "\n\n")

# 2. Save multiple objects
cat("2. Save multiple objects:\n")
save(vector1, matrix1, df1, file = temp_file)
cat("  Saved 3 objects\n\n")

# 3. Save with custom compression
cat("3. Save with compression:\n")
save(df1, file = temp_file, compress = TRUE)
cat("  Compression enabled\n\n")

# 4. Save with ASCII (for sharing)
cat("4. Save with ASCII format:\n")
# ASCII is larger but more portable
# save(df1, file = temp_file, ascii = TRUE)
cat("  ASCII format (larger file, portable)\n\n")

# 5. Save list of objects by name
cat("5. Save objects by name list:\n")
save(list = c("vector1", "matrix1"), file = temp_file)
cat("  Saved objects in list\n\n")

# Clean up
unlink(temp_file)
```

**Output:**
```
===== SAVE FUNCTION =====

1. Save single object:
  Saved df1 to C:\Users\...\AppData\Local\Temp\file2e1c.RData
```

### Standard Example: load() Function

```r
# ===== LOAD FUNCTION =====

cat("===== LOAD FUNCTION =====\n\n")

# Prepare data
df1 <- data.frame(
  id = 1:5,
  name = c("A", "B", "C", "D", "E"),
  value = c(10, 20, 30, 40, 50)
)
vector1 <- c("red", "blue", "green")
list1 <- list(x = 1:3, y = c("a", "b", "c"))

# Save data
temp_file <- tempfile(fileext = ".RData")
save(df1, vector1, list1, file = temp_file)

# 1. Basic load
cat("1. Basic load():\n")
load(temp_file)
cat("  Loaded objects:", ls(), "\n\n")

# Clean up after load test
rm(df1, vector1, list1)

# 2. Load into specified environment
cat("2. Load to custom environment:\n")
env <- new.env()
load(temp_file, envir = env)
cat("  Objects in environment:\n")
print(ls(envir = env))
cat("\n")

# 3. Load returns object names
cat("3. Load return value:\n")
loaded_names <- load(temp_file)
cat("  Loaded objects (returned):", loaded_names, "\n\n")

# Clean up
unlink(temp_file)
```

### Real-World Example: save.image()

```r
# ===== SAVE.IMAGE FUNCTION =====

cat("===== SAVE.IMAGE FUNCTION =====\n\n")

# save.image() saves entire workspace to .RData

# Create multiple objects in workspace
df_employees <- data.frame(
  id = 1:10,
  name = paste0("Employee", 1:10),
  department = sample(c("Sales", "Engineering", "HR"), 10, replace = TRUE),
  salary = sample(40000:100000, 10)
)

df_sales <- data.frame(
  month = c("Jan", "Feb", "Mar", "Apr"),
  revenue = c(100000, 120000, 95000, 140000),
  expenses = c(80000, 90000, 85000, 100000)
)

vector_settings <- c(theme = "dark", version = "1.0", debug = TRUE)
list_config <- list(
  api_key = "secret",
  timeout = 30,
  retries = 3
)

# 1. Save entire workspace
cat("1. Save entire workspace:\n")
save.image(".RData_backup.RData")
cat("  Saved to .RData_backup.RData\n")
cat("  Current workspace size:", file.info(".RData_backup.RData")$size, "bytes\n\n")

# 2. Save with custom name
cat("2. Save with custom filename:\n")
save.image(file = "project_backup.RData")
cat("  Saved to project_backup.RData\n\n")

# 3. Load workspace
cat("3. Load workspace:\n")
# Start fresh
rm(list = ls())
load(".RData_backup.RData")
cat("  Loaded workspace, objects:", length(ls()), "\n")
print(head(df_employees))

# Clean up
unlink(".RData_backup.RData")
unlink("project_backup.RData.RData")
file.remove("project_backup.RData")
```

### Advanced Example: Compression and Performance

```r
# ===== COMPRESSION OPTIONS =====

cat("===== COMPRESSION COMPARISON =====\n\n")

# Create large dataset for comparison
set.seed(123)
large_df <- data.frame(
  id = 1:10000,
  value = rnorm(10000),
  group = sample(LETTERS[1:10], 10000, replace = TRUE),
  score = runif(10000)
)

cat("Dataset: 10,000 rows, 4 columns\n\n")

# Test different compression levels
results <- data.frame(
  compression = c("FALSE", "TRUE (default)", "gzip level 1", "gzip level 6", "gzip level 9")
)

test_compression <- function(compress_level) {
  temp_file <- tempfile()
  save(large_df, file = temp_file, compress = compress_level)
  size <- file.info(temp_file)$size
  unlink(temp_file)
  return(size)
}

results$size_kb <- c(
  test_compression(FALSE),
  test_compression(TRUE),
  test_compression(1),
  test_compression(6),
  test_compression(9)
) / 1024

results$size_kb <- round(results$size_kb, 1)
print(results)

cat("\nCompression ratio: ", 
      round((1 - results$size_kb[5] / results$size_kb[1]) * 100, 1), 
      "%\n\n")

# 2. Version compatibility
cat("2. R version compatibility:\n")
temp_file <- tempfile()
# save to older R version format
save(large_df, file = temp_file, version = 2)
cat("  Saved with version = 2 (R < 3.5.0)\n")
cat("  Current default version = 3\n")

unlink(temp_file)
```

### Practical Example: Data Pipeline

```r
# ===== DATA PIPELINE PATTERN =====

cat("===== DATA PIPELINE =====\n\n")

# Common pattern: Process, Save, Load

# Step 1: Process data
process_data <- function(raw_data) {
  df <- raw_data
  df$processed <- df$value * 2
  df$category <- ifelse(df$value > 0, "positive", "negative")
  return(df)
}

# Create sample raw data
raw_data <- data.frame(
  id = 1:100,
  value = rnorm(100)
)

# Step 2: Process
processed <- process_data(raw_data)

# Step 3: Save intermediate result
intermediate_file <- "processed_data.RData"
save(processed, file = intermediate_file)
cat("Saved processed data to", intermediate_file, "\n")

# Later, reload for analysis
analysis <- function(data_file) {
  load(data_file)
  summary(processed)
}

cat("Analysis results:\n")
analysis_result <- analysis(intermediate_file)
print(analysis_result)

# Clean up
unlink(intermediate_file)
```

**Output:**
```
===== COMPRESSION COMPARISON =====

Dataset: 10,000 rows, 4 columns

  compression size_kb
1       FALSE   312.5
2    TRUE (default)   125.8
3    gzip level 1   152.3
4    gzip level 6   125.7
5    gzip level 9   117.4

Compression ratio: 62.4%
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use descriptive filenames**: Include date and purpose
2. **Use compression**: Default is sufficient
3. **Version = 2**: For sharing with older R versions
4. **Load into environment**: Avoid polluting global env
5. **Check existence**: Verify file before loading

### Common Pitfalls

1. **Object overwriting**: load() overwrites existing objects
2. **Path issues**: Use absolute paths in scripts
3. **Large files**: Consider RDS for single objects
4. **Encoding**: ASCII for cross-platform text
5. **Corruption**: Don't modify .RData files manually

### Comparison with RDS

| Feature | save()/load() | saveRDS()/readRDS() |
|---------|--------------|-------------------|
| Multiple objects | Yes | No |
| Single object | No | Yes |
| Returns object names | Yes | No |
| Direct assignment | No | Yes |