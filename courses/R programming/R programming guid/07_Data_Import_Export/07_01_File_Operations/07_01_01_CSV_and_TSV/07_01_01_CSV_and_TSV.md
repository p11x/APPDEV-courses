# CSV and TSV File Operations in R

## Learning Objectives

- Master read.csv() and write.csv() functions
- Understand read.table() for flexible text file handling
- Learn fread() from data.table for high-performance reading
- Handle delimiters, headers, and data types
- Apply best practices for large datasets

## Theoretical Background

### File Formats

CSV (Comma-Separated Values) and TSV (Tab-Separated Values) are the most common text-based data exchange formats:

1. **CSV**: Values separated by commas, strings may be quoted
2. **TSV**: Values separated by tabs, cleaner for text with commas
3. **Plain text**: Flexible delimiters via read.table()

### R Functions for Text Files

| Function | Package | Use Case |
|----------|---------|----------|
| read.csv() | base | Standard CSV with defaults |
| read.delim() | base | TSV and custom delimiters |
| read.table() | base | Flexible text file parsing |
| fread() | data.table | High-performance large files |
| write.csv() | base | CSV output |
| write.table() | base | Generic text output |
| fwrite() | data.table | Fast CSV writing |

### Key Parameters

```
read.csv(file, header = TRUE, sep = ",", quote = "\"", 
         stringsAsFactors = FALSE, na.strings = NA,
         colClasses = NULL, nrows = -1, skip = 0)
```

## Code Examples

### Basic Example: Reading CSV Files

```r
# ===== READING CSV FILES =====

cat("===== READ.CSV BASICS =====\n\n")

# Sample CSV content (simulate file)
csv_content <- "name,age,salary,department
Alice,28,55000,Sales
Bob,35,72000,Engineering
Charlie,42,85000,HR
Diana,31,61000,Marketing
Edward,45,92000,Engineering"

# Write temporary file
temp_file <- tempfile(fileext = ".csv")
writeLines(csv_content, temp_file)

# 1. Basic read.csv
cat("1. Basic read.csv:\n")
df1 <- read.csv(temp_file, header = TRUE)
print(df1)
cat("\n")

# 2. Read without header
cat("2. Without header:\n")
df2 <- read.csv(temp_file, header = FALSE)
print(df2)
cat("\n")

# 3. Specify colClasses
cat("3. With colClasses:\n")
df3 <- read.csv(temp_file, colClasses = c("character", "integer", 
                                          "numeric", "character"))
print(df3)
cat("\n")

# 4. Handle missing values
csv_with_na <- "name,age,score
Alice,25,85
Bob,30,
Charlie,,90"
writeLines(csv_with_na, temp_file)

cat("4. Handle NA strings:\n")
df4 <- read.csv(temp_file, na.strings = c("", "NA"))
print(df4)

# Clean up
unlink(temp_file)
```

**Output:**
```
===== READ.CSV BASICS =====

1. Basic read.csv:
      name age salary    department
1   Alice  28  55000        Sales
2     Bob  35  72000  Engineering
3  Charlie  42  85000          HR
4   Diana  31  61000   Marketing
5  Edward  45  92000  Engineering
```

### Standard Example: read.table for Flexible Parsing

```r
# ===== READ.TABLE FLEXIBILITY =====

cat("===== READ.TABLE EXAMPLES =====\n\n")

# Sample TSV content
tsv_content <- "Name\tAge\tScore\tGrade
Alice\t25\t85\tB
Bob\t30\t92\tA
Charlie\t28\t78\tC"

temp_file <- tempfile(fileext = ".tsv")
writeLines(tsv_content, temp_file)

# 1. Read TSV with read.table
cat("1. Read TSV:\n")
df_tsv <- read.table(temp_file, header = TRUE, sep = "\t", 
                      stringsAsFactors = FALSE)
print(df_tsv)
cat("\n")

# 2. Read with custom quote character
csv_quoted <- "name,description
Alice,\"Data Scientist\"
Bob,\"Software Engineer, Senior\""
writeLines(csv_quoted, temp_file)

cat("2. Handle quoted fields:\n")
df_quoted <- read.csv(temp_file, stringsAsFactors = FALSE)
print(df_quoted)
cat("\n")

# 3. Skip rows and limit data
big_csv <- "header1,header2,header3
skip1,skip2,skip3
data1,data2,data3
data4,data5,data6"
writeLines(big_csv, temp_file)

cat("3. Skip rows, nrows:\n")
df_limited <- read.csv(temp_file, skip = 1, nrows = 2)
print(df_limited)
cat("\n")

# 4. StringAsFactors behavior
cat("4. stringsAsFactors = TRUE vs FALSE:\n")
df_factor <- read.csv(temp_file, stringsAsFactors = TRUE)
df_char <- read.csv(temp_file, stringsAsFactors = FALSE)
cat("  With stringsAsFactors (class):", class(df_factor$header1), "\n")
cat("  Without (class):", class(df_char$header1), "\n")

# Clean up
unlink(temp_file)
```

### High-Performance Example: fread()

```r
# ===== FREAD FOR LARGE FILES =====

cat("===== FREAD HIGH PERFORMANCE =====\n\n")

# Note: fread requires data.table package
# install.packages("data.table")

# Create test dataset
set.seed(123)
n <- 10000
large_df <- data.frame(
  id = 1:n,
  name = paste0("Person", 1:n),
  age = sample(20:65, n, replace = TRUE),
  salary = sample(30000:150000, n, replace = TRUE),
  department = sample(c("Sales", "Engineering", "HR", "Marketing"), 
                       n, replace = TRUE),
  score = round(runif(n, 50, 100), 2)
)

# Write to CSV
temp_file <- tempfile(fileext = ".csv")
write.csv(large_df, temp_file, row.names = FALSE)

# 1. fread basics
cat("1. fread basic:\n")
system.time(df_fread <- fread(temp_file))
cat("  Rows read:", nrow(df_fread), "\n\n")

# 2. Select specific columns
cat("2. Select columns:\n")
system.time(df_cols <- fread(temp_file, select = c("id", "name", "salary")))
cat("  Columns:", names(df_cols), "\n\n")

# 3. Skip rows
cat("3. Skip first 5 rows:\n")
df_skip <- fread(temp_file, skip = 5, nrows = 10)
print(head(df_skip, 3))
cat("\n")

# 4. Use colClasses for type optimization
cat("4. Column selection with types:\n")
df_types <- fread(temp_file, colClasses = c("integer", "NULL", 
                                              "numeric", "NULL"))
cat("  Only salary column kept\n")

# 5. fread from command output
cat("5. Use pipe input:\n")
df_pipe <- fread(cmd = "head -20 temp_file.csv")
print(head(df_pipe, 3))

# Clean up
unlink(temp_file)
```

### Real-World Example: Writing CSV Files

```r
# ===== WRITING CSV FILES =====

cat("===== WRITE.CSV EXAMPLES =====\n\n")

# Sample data
employees <- data.frame(
  name = c("Alice", "Bob", "Charlie"),
  age = c(28, 35, 42),
  salary = c(55000, 72000, 85000),
  department = c("Sales", "Engineering", "HR")
)

temp_file <- tempfile(fileext = ".csv")

# 1. Basic write.csv
cat("1. Basic write.csv:\n")
write.csv(employees, temp_file, row.names = FALSE)
cat(readLines(temp_file), sep = "\n")

# 2. Write with append
cat("\n2. Append mode:\n")
new_row <- data.frame(name = "Diana", age = 31, salary = 61000,
                      department = "Marketing")
write.csv(new_row, temp_file, row.names = FALSE, append = TRUE)
cat(readLines(temp_file), sep = "\n")

# 3. write.table for custom delimiter
tsv_file <- tempfile(fileext = ".tsv")
cat("\n3. Write TSV:\n")
write.table(employees, tsv_file, row.names = FALSE, sep = "\t",
            quote = FALSE)
cat(readLines(tsv_file), sep = "\n")

# 4. Using fwrite for large data
cat("\n4. fwrite for speed:\n")
set.seed(123)
large_data <- data.frame(
  x = 1:50000,
  y = rnorm(50000),
  z = sample(letters, 50000, replace = TRUE)
)
system.time(fwrite(large_data, "large_test.csv"))

# Clean up
unlink(temp_file)
unlink(tsv_file)
unlink("large_test.csv")
```

**Output:**
```
===== WRITE.CSV EXAMPLES =====

1. Basic write.csv:
"name","age","salary","department"
"Alice",28,55000,"Sales"
"Bob",35,72000,"Engineering"
"Charlie",42,85000,"HR"
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Always use stringsAsFactors = FALSE**: Prevents automatic factor conversion
2. **Specify colClasses**: Improves memory usage and parsing speed
3. **Use fread for large files**: 5-10x faster than base R functions
4. **Set na.strings explicitly**: Handle missing value representations
5. **Use row.names = FALSE**: Avoids adding row numbers as first column

### Common Pitfalls

1. **Default quote character**: May fail with embedded commas in fields
2. **Header handling**: Check if first row is data or header
3. **Type inference**: Large numeric columns may be read as character
4. **Memory limits**: Use colClasses to limit memory for large files
5. **Encoding issues**: Specify encoding for non-ASCII text

### Performance Comparison

| Method | 10K rows | 100K rows | 1M rows |
|-------|----------|-----------|--------|
| read.csv() | 0.5s | 5s | 60s |
| fread() | 0.1s | 0.8s | 8s |
| write.csv() | 1s | 12s | 180s |
| fwrite() | 0.2s | 2s | 20s |