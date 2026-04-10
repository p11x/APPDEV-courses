# Reading Excel Files in R

## Learning Objectives

- Read Excel (.xlsx and .xls) files into R
- Handle multiple sheets inworkbooks
- Specify ranges and headers
- Manage date and time formatting
- Use openxlsx and readxl packages

## Theory

Excel files are binary formats that store data in worksheets. R can read Excel files using several packages: readxl (part of tidyverse), openxlsx, and xlsx. readxl is the most straightforward and reliable, while openxlsx provides more formatting control.

Excel files may contain multiple sheets, formatted cells, merged cells, and formulas. Reading only gets the computed values, not the formulas themselves.

## Step-by-Step

1. Install and load readxl package
2. Use excel_sheets() to list worksheets
3. Read specific sheet with read_excel()
4. Optionally specify range and headers
5. Handle date columns appropriately

## Code Examples

### Basic Excel Reading with readxl

```r
cat("===== BASIC EXCEL READING =====\n\n")

library(readxl)

# Create sample Excel file for demonstration
temp_xlsx <- tempfile(fileext = ".xlsx")

library(openxlsx)
demo_df <- data.frame(
  ID = 1:10,
  Name = paste0("Person", 1:10),
  Score = sample(60:100, 10),
  Date = seq(Sys.Date(), length.out = 10, by = "day")
)
write.xlsx(demo_df, temp_xlsx)

# List all sheets
cat("Sheets in workbook:\n")
print(excel_sheets(temp_xlsx))

# Read first sheet
data1 <- read_excel(temp_xlsx, sheet = 1)
cat("\nSheet 1 read:\n")
print(head(data1))

# Read by sheet name
data2 <- read_excel(temp_xlsx, sheet = "Sheet1")
cat("\nBy name:\n")
print(head(data2))
```

### Specifying Ranges and Headers

```r
cat("\n===== RANGES AND HEADERS =====\n\n")

# Create Excel with header in row 2
demo_df2 <- data.frame(
  A = c("", "ID", 1, 2, 3),
  B = c("", "Name", "A", "B", "C"),
  C = c("Header", "Score", 90, 85, 92)
)

write.xlsx(demo_df2, "header_test.xlsx", sheetName = "Test")

# Read with skip (skip first row)
data_skip <- read_excel("header_test.xlsx", skip = 1)
cat("Skip 1 row:\n")
print(data_skip)

# Read with n_max to limit rows
data_limit <- read_excel(temp_xlsx, n_max = 5)
cat("\nFirst 5 rows only:\n")
print(data_limit)

# Read specific range using col_names
data_range <- read_excel(
  temp_xlsx,
  range = "B1:C5",
  col_names = TRUE
)
cat("\nSpecific range B1:C5:\n")
print(data_range)
```

### Handling Multiple Sheets

```r
cat("\n===== MULTIPLE SHEETS =====\n\n")

# Create workbook with multiple sheets
wb <- createWorkbook()
addWorksheet(wb, "Sales")
addWorksheet(wb, "Products")

writeData(wb, "Sales", data.frame(
  Month = c("Jan", "Feb", "Mar"),
  Sales = c(10000, 12000, 15000)
))
writeData(wb, "Products", data.frame(
  Product = c("A", "B", "C"),
  Price = c(10, 20, 30)
))

saveWorkbook(wb, "multi_sheet.xlsx", overwrite = TRUE)

# Read all sheets into list
all_sheets <- lapply(excel_sheets("multi_sheet.xlsx"), function(sheet) {
  read_excel("multi_sheet.xlsx", sheet = sheet)
})
names(all_sheets) <- excel_sheets("multi_sheet.xlsx")

cat("Sheets read:\n")
print(names(all_sheets))
cat("\nSales sheet:\n")
print(all_sheets$Sales)
```

### Date and Time Handling

```r
cat("\n===== DATE HANDLING =====\n\n")

# Excel stores dates as numbers since 1899-12-30
df_dates <- data.frame(
  Date = as.Date(c("2024-01-01", "2024-02-15", "2024-03-30")),
  Datetime = as.POSIXct(c("2024-01-01 10:30", "2024-02-15 14:45", "2024-03-30 09:00")),
  Numeric = c(45300, 45345, 45389)
)

write.xlsx(df_dates, "dates_test.xlsx")

# Read and detect dates
dates_data <- read_excel("dates_test.xlsx")
cat("Dates read (auto-detected):\n")
print(str(dates_data))

# Read as character to avoid conversion
dates_char <- read_excel("dates_test.xlsx", col_types = "text")
cat("\nAs character:\n")
print(dates_char)
```

## Real-World Example: Import Business Report

```r
# Real-world: Import business report from Excel
cat("===== BUSINESS REPORT IMPORT =====\n\n")

# Simulate business report
business_data <- data.frame(
  Quarter = c("Q1", "Q1", "Q2", "Q2"),
  Region = c("North", "South", "North", "South"),
  Revenue = c(100000, 80000, 120000, 90000),
  Expenses = c(60000, 50000, 70000, 55000),
  Profit = c(40000, 30000, 50000, 35000)
)

write.xlsx(business_data, "business_report.xlsx", overwrite = TRUE)

# Import the report
report <- read_excel("business_report.xlsx")
cat("Business Report Summary:\n")
print(report)

# Calculate totals
total_revenue <- sum(report$Revenue)
total_profit <- sum(report$Profit)
cat("\nTotal Revenue:", total_revenue, "\n")
cat("Total Profit:", total_profit, "\n")

# Clean up
unlink(temp_xlsx)
unlink("header_test.xlsx")
unlink("multi_sheet.xlsx")
unlink("dates_test.xlsx")
unlink("business_report.xlsx")
```

## Best Practices

1. Use readxl for simple, reliable reading
2. Always check sheet names with excel_sheets()
3. Use col_types to prevent unwanted conversions
4. For large files, consider only reading needed columns
5. Test read with first few rows before full import

## Exercises

1. Read an Excel file with 5 worksheets into R
2. Read only columns A, C, and E from a large Excel file
3. Handle Excel files with merged header rows
4. Read Excel files with password protection
5. Create a function to import all Excel files in a directory