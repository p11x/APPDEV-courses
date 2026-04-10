# CSV Options and Advanced Settings

## Learning Objectives

- Understand all available CSV reading/writing parameters
- Handle encoding and locale settings
- Configure memory and performance options
- Work with text encoding and conversions

## Theory

The read.csv and write.csv functions have many parameters controlling how data is parsed and written. Key parameters include sep (field delimiter), dec (decimal separator), na.strings (missing value representations), colClasses (column type specifications), and encoding options.

For international data, locale settings are critical. Different countries use different decimal separators (period vs comma) and date formats. The readr package provides cleaner API design and better defaults.

## Step-by-Step

1. Identify data format requirements
2. Set appropriate separator and quote characters
3. Configure locale for regional formats
4. Handle encoding properly
5. Test with edge cases

## Code Examples

### Separator and Delimiter Options

```r
cat("===== SEPARATOR OPTIONS =====\n\n")

data <- data.frame(
  a = 1:3,
  b = c("x", "y", "z"),
  c = c(1.1, 2.2, 3.3)
)

# Comma separator (default for read.csv)
write.csv(data, "opt_comma.csv", row.names = FALSE, quote = TRUE)

# Tab separator
write.table(data, "opt_tab.tsv", row.names = FALSE, sep = "\t")

# Semicolon separator
write.csv2(data, "opt_semi.csv", row.names = FALSE)

# Reading back with different separators
cat("Reading tab-separated:\n")
tab_data <- read.delim("opt_tab.tsv", sep = "\t")
print(tab_data)

cat("\nReading semicolon CSV:\n")
semi_data <- read.csv2("opt_semi.csv")
print(semi_data)
```

### Locale and Encoding Settings

```r
cat("\n===== LOCALE SETTINGS =====\n\n")

# Set locale for different regions
# European locale uses comma as decimal separator
Sys.setlocale("LC_ALL", "German_Germany")

# Create data with locale-specific values
df_locale <- data.frame(
  value = c(1.5, 2.5, 3.5),
  date = as.Date(c("2024-01-15", "2024-02-20", "2024-03-25"))
)

# Write with locale settings
write.csv2(df_locale, "locale_de.csv", row.names = FALSE)

# Using readr with locale
library(readr)
df_en <- read_csv("opt_comma.csv", locale = locale(decimal_mark = "."))
cat("English locale read:\n")
print(df_en)

# Using read.csv2 for European format
df_eu <- read.csv2("locale_de.csv")
cat("\nEuropean format read:\n")
print(df_eu)
```

### File Encoding Options

```r
cat("\n===== ENCODING OPTIONS =====\n\n")

# UTF-8 encoding
write.csv(data, "utf8_file.csv", row.names = FALSE, fileEncoding = "UTF-8")

# Read with encoding specified
utf8_data <- read.csv("utf8_file.csv", fileEncoding = "UTF-8")
cat("UTF-8 encoded file read\n")

# Handle Latin-1 encoding
latin_data <- read.csv("opt_comma.csv", fileEncoding = "latin1")

# Native encoding
native_data <- read.csv("opt_comma.csv", encoding = "native.enc")
```

### Performance and Memory Options

```r
cat("\n===== PERFORMANCE OPTIONS =====\n\n")

# For large files, read in chunks
chunk_size <- 1000
con <- file("opt_comma.csv", "r")
chunk_num <- 1

while(TRUE) {
  chunk <- read.csv(con, nrows = chunk_size, header = if(chunk_num == 1) TRUE else FALSE)
  if(nrow(chunk) == 0) break
  cat("Chunk", chunk_num, ":", nrow(chunk), "rows\n")
  chunk_num <- chunk_num + 1
  if(nrow(chunk) < chunk_size) break
}
close(con)

# Using nrows and skip for partial reads
partial_data <- read.csv("opt_comma.csv", nrows = 10)
cat("\nFirst 10 rows read:\n")
print(partial_data)

# Using comment.char to skip comment lines
```

## Real-World Example: Multi-Format Data Export

```r
# Real-world: Export the same data in multiple formats
cat("===== MULTI-FORMAT EXPORT =====\n\n")

# Sample data
export_data <- data.frame(
  ID = 1:100,
  Name = paste0("Person", 1:100),
  Score = runif(100, 0, 100),
  Date = seq(as.Date("2024-01-01"), length.out = 100, by = "day")
)

# Export to different formats
write.csv(export_data, "export_comma.csv", row.names = FALSE)
write.csv2(export_data, "export_semi.csv", row.names = FALSE)
write.table(export_data, "export_tab.tsv", row.names = FALSE, sep = "\t")

# Using readr
library(readr)
write_csv(export_data, "export_readr.csv")
write_tsv(export_data, "export_readr.tsv")

# Verify file sizes
cat("File sizes:\n")
cat("CSV:", file.info("export_comma.csv")$size, "bytes\n")
cat("TSV:", file.info("export_tab.tsv")$size, "bytes\n")
cat("readr CSV:", file.info("export_readr.csv")$size, "bytes\n")
```

## Best Practices

1. Use consistent encoding (UTF-8 recommended)
2. Test with sample data before full export
3. For large files, consider chunked processing
4. Document the format used in code comments
5. Use readr for cleaner API and better defaults
6. Always specify fileEncoding for international data

## Exercises

1. Create a function that auto-detects CSV format
2. Write code to convert between different CSV variants
3. Optimize a CSV export for a 1 million row dataset
4. Handle CSV files with embedded newlines in fields
5. Write code to validate CSV output format