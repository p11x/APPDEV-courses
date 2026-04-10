# Reading CSV Files in R

## Learning Objectives

- Understand CSV file formats and their variants
- Use read.csv and read.csv2 functions effectively
- Handle various delimiters and text qualifiers
- Manage header rows and column types
- Address common CSV reading problems

## Theory

CSV (Comma-Separated Values) is the most common data exchange format. CSV files store tabular data in plain text where each line represents a row and fields are separated by commas. TSV (Tab-Separated Values) uses tabs instead of commas.

R provides multiple functions for reading CSV files. The base R functions include read.csv(), read.delim(), and read.table(). The readr package from tidyverse offers read_csv() and read_tsv() with better performance and type inference.

CSV files may have variations: some use semicolons as delimiters (common in European locales), others use different line endings. Text fields containing the delimiter must be quoted.

## Step-by-Step

1. Identify the CSV file format (delimiter, decimal separator, quote character)
2. Determine if the file has a header row
3. Check for missing values representation
4. Choose appropriate reading function
5. Specify column classes if known
6. Handle encoding issues

## Code Examples

### Standard Example: Basic CSV Reading

```r
# Read basic CSV file
cat("===== BASIC CSV READING =====\n\n")

# Create sample CSV
write.csv(mtcars, "sample.csv", row.names = FALSE)

# Read with defaults
data <- read.csv("sample.csv", header = TRUE, sep = ",")
cat("Read", nrow(data), "rows with", ncol(data), "columns\n")
head(data)

# Using read.csv with common options
data2 <- read.csv(
  "sample.csv",
  header = TRUE,
  sep = ",",
  na.strings = c("NA", "", "null"),
  stringsAsFactors = FALSE
)
cat("\nWith na.strings and stringsAsFactors:\n")
str(data2)
```

### Alternative Delimiters

```r
cat("\n===== ALTERNATIVE DELIMITERS =====\n\n")

# Tab-separated
write.table(mtcars, "sample.tsv", row.names = FALSE, sep = "\t", quote = FALSE)
tsv_data <- read.delim("sample.tsv", sep = "\t")
cat("TSV read:", nrow(tsv_data), "rows\n")

# Semicolon-separated (European format)
write.table(mtcars, "sample2.csv", row.names = FALSE, sep = ";", quote = FALSE)
eur_data <- read.csv2("sample2.csv")
cat("Semicolon CSV read:", nrow(eur_data), "rows\n")

# Read with fixed width
cat("\nFixed width format:\n")
fwf_data <- read.fwf("sample.csv", widths = c(10, 10, 10, 10, 10, 10))
```

### Handling Complex CSV Files

```r
cat("\n===== COMPLEX CSV HANDLING =====\n\n")

# Read with column classes specified
data_classed <- read.csv(
  "sample.csv",
  colClasses = c(
    character = "character",
    mpg = "numeric",
    cyl = "integer",
    disp = "numeric",
    hp = "numeric",
    drat = "numeric",
    wt = "numeric",
    qsec = "numeric",
    vs = "integer",
    am = "integer",
    gear = "integer",
    carb = "integer"
  ),
  na.strings = c("NA", "N/A", "")
)
cat("With specified column classes:\n")
str(data_classed)

# Skip rows and nrows limiting
cat("\nSkip and nrows usage:\n")
data_skip <- read.csv("sample.csv", skip = 2, nrows = 5)
cat("Skipped 2 rows, read 5 rows:\n")
print(data_skip)
```

### Using readr Package

```r
cat("\n===== READR PACKAGE =====\n\n")

library(readr)

# Read CSV with readr
data_readr <- read_csv("sample.csv", col_names = TRUE)
cat("readr result:\n")
print(data_readr)

# Specify column types
data_typed <- read_csv(
  "sample.csv",
  col_types = cols(
    mpg = col_double(),
    cyl = col_integer(),
    disp = col_double()
  )
)
cat("\nWith typed columns:")
str(data_typed)
```

## Real-World Example: Import Survey Data

```r
# Real-world: Import survey data with various issues
cat("===== SURVEY DATA IMPORT =====\n\n")

# Simulate survey data with issues
survey_text <- "ID,Age,Income,Comments\n1,25,45000,\"Great product!\"\n2,NA,52000,\"Needs improvement\"\n3,35,NA,\"\"\n4,42,68000,\"Excellent service\"\n5,28,39000,\"NA\"\n"
writeLines(survey_text, "survey.csv")

# Read survey with proper handling
survey <- read.csv(
  "survey.csv",
  header = TRUE,
  na.strings = c("NA", "", "null"),
  stringsAsFactors = FALSE,
  colClasses = c(
    ID = "integer",
    Age = "numeric",
    Income = "numeric",
    Comments = "character"
  )
)
cat("Survey data summary:\n")
print(summary(survey))

# Check for issues
cat("\nMissing values per column:\n")
print(colSums(is.na(survey)))

# Clean the data
survey_clean <- na.omit(survey)
cat("\nAfter removing NA rows:", nrow(survey_clean), "rows remain\n")
```

## Best Practices

1. Always specify colClasses to prevent type coercion issues
2. Use na.strings vector to capture all missing representations
3. Set stringsAsFactors = FALSE to keep character columns as character
4. For large files, use readr::read_csv for better performance
5. Check data after reading with str() and summary()
6. Handle encoding with fileEncoding parameter when needed
7. Use comment.char for files with comment lines

## Exercises

1. Read a CSV file with 10,000 rows and measure read time difference between read.csv and read_csv
2. Create code to automatically detect delimiter in a CSV file
3. Write a function that reads multiple CSV files and combines them
4. Handle a CSV file with different date formats in columns
5. Read only specific columns from a large CSV file efficiently