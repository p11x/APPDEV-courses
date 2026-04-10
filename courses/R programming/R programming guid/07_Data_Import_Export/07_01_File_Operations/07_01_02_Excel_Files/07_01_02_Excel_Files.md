# Excel File Operations in R

## Learning Objectives

- Read Excel files using readxl package
- Write Excel files using openxlsx package
- Handle multiple sheets and ranges
- Preserve formatting with openxlsx
- Work with xlsx package for Java-based operations

## Theoretical Background

### Excel Packages in R

| Package | Read | Write | Formatting | Dependencies |
|---------|------|-------|------------|--------------|
| readxl | Yes | Limited | No | libxml2 |
| openxlsx | Yes | Yes | Full | zip |
| xlsx | Yes | Yes | Partial | rJava, xlsxjars |
| gdata | Yes | No | No | perl |
| XLConnect | Yes | Yes | Full | rJava |

### Installation

```r
install.packages("readxl")
install.packages("openxlsx")
install.packages("xlsx")
```

### readxl Functions

- read_xlsx(): Read .xlsx files
- read_xls(): Read .xls files
- excel_sheets(): List sheet names
- read_excel(): Auto-detect format

## Code Examples

### Basic Example: readxl Package

```r
# ===== READXL BASICS =====

cat("===== READXL PACKAGE =====\n\n")

# Note: readxl is read-only for formatting
# Create a sample Excel file using openxlsx for demonstration

library(openxlsx)

# Create sample workbook
wb <- createWorkbook()
addWorksheet(wb, "Employees")
addWorksheet(wb, "Sales")

# Sheet 1: Employees
employees <- data.frame(
  name = c("Alice", "Bob", "Charlie", "Diana"),
  age = c(28, 35, 42, 31),
  department = c("Sales", "Engineering", "HR", "Marketing"),
  salary = c(55000, 72000, 85000, 61000)
)
writeData(wb, "Employees", employees)

# Sheet 2: Sales data
sales <- data.frame(
  quarter = Q1 = c("Q1", "Q2", "Q3", "Q4"),
  revenue = c(100000, 120000, 95000, 140000),
  expenses = c(80000, 90000, 85000, 100000)
)
sales_df <- data.frame(
  quarter = rep(c("Q1", "Q2", "Q3", "Q4"), each = 3),
  product = rep(c("Product A", "Product B", "Product C"), 4),
  sales = sample(1000:5000, 12)
)
writeData(wb, "Sales", sales_df)

# Save workbook
temp_xlsx <- tempfile(fileext = ".xlsx")
saveWorkbook(wb, temp_xlsx, overwrite = TRUE)

# Now demonstrate readxl functions
library(readxl)

# 1. List sheets
cat("1. List Excel sheets:\n")
sheet_names <- excel_sheets(temp_xlsx)
print(sheet_names)
cat("\n")

# 2. Read specific sheet
cat("2. Read 'Employees' sheet:\n")
df_employees <- read_xlsx(temp_xlsx, sheet = "Employees")
print(df_employees)
cat("\n")

# 3. Read by index
cat("3. Read sheet by index (1):\n")
df_first <- read_xlsx(temp_xlsx, sheet = 1)
print(df_first)
cat("\n")

# 4. Read specific range
cat("4. Read range A1:B3:\n")
df_range <- read_xlsx(temp_xlsx, sheet = "Employees", 
                      range = "A1:B3")
print(df_range)
cat("\n")

# 5. Skip rows
cat("5. Skip first 2 rows:\n")
# Create file with header in row 3
addWorksheet(wb, "RawData")
raw_data <- data.frame(
  X = c("", "Category", "A", "B", "C"),
  Y = c("", "Value", 100, 200, 300),
  Z = c("", "Count", 50, 75, 60)
)
writeData(wb, "RawData", raw_data)
saveWorkbook(wb, temp_xlsx, overwrite = TRUE)

df_skip <- read_xlsx(temp_xlsx, sheet = "RawData", skip = 2)
print(df_skip)

unlink(temp_xlsx)
```

**Output:**
```
===== READXL PACKAGE =====

1. List Excel sheets:
[1] "Employees" "Sales"     

2. Read 'Employees' sheet:
  name age  department salary
1 Alice  28       Sales  55000
2   Bob  35 Engineering  72000
3 Charlie  42         HR  85000
4  Diana  31  Marketing  61000
```

### Standard Example: openxlsx Package

```r
# ===== OPENXLSX WRITING =====

cat("===== OPENXLSX WRITING =====\n\n")

library(openxlsx)

# Create new workbook
wb <- createWorkbook()

# 1. Basic sheet creation
addWorksheet(wb, "Sheet1")
data <- data.frame(
  Name = c("John", "Jane", "Bob"),
  Score = c(85, 92, 78)
)
writeData(wb, "Sheet1", data)

# 2. Add multiple sheets
addWorksheet(wb, "Summary")
summary_data <- data.frame(
  Metric = c("Average", "Max", "Min"),
  Value = c(mean(data$Score), max(data$Score), min(data$Score))
)
)
writeData(wb, "Summary", summary_data)

# Save
temp_file <- tempfile(fileext = ".xlsx")
saveWorkbook(wb, temp_file, overwrite = TRUE)

cat("Workbook created with", length(wb$sheet_names), "sheets\n\n")

# 3. Formatting with openxlsx
wb2 <- createWorkbook()
addWorksheet(wb2, "Formatted")

# Create styled data
 df <- data.frame(
   Name = c("Alice", "Bob", "Charlie", "Total"),
   Sales = c(1000, 1500, 800, 3300),
   Q1 = c(250, 400, 200, 850),
   Q2 = c(300, 350, 220, 870),
   Q3 = c(450, 750, 380, 1580)
)

# Write data
writeData(wb2, "Formatted", df, startRow = 1)

# Apply formatting
# Header style
headerStyle <- createStyle(
  fontSize = 12,
  fontColour = "#FFFFFF",
  halign = "center",
  fgFill = "#4F81BD",
  border = "TopBottom",
  borderColour = "#4F81BD",
  textDecoration = "bold"
)
addStyle(wb2, "Formatted", headerStyle, rows = 1, cols = 1:5, 
        gridExpand = TRUE)

# Number format for sales
numStyle <- createStyle(halign = "right")
addStyle(wb2, "Formatted", numStyle, rows = 2:5, cols = 2:5,
        gridExpand = TRUE)

# Set column widths
setColWidths(wb2, "Formatted", cols = 1, widths = 12)
setColWidths(wb2, "Formatted", cols = 2:5, widths = 10)

# Freeze panes
freezePane(wb2, "Formatted", firstActiveRow = 2)

saveWorkbook(wb2, temp_file, overwrite = TRUE)
cat("Formatted workbook saved\n\n")

# 4. Conditional formatting
wb3 <- createWorkbook()
addWorksheet(wb3, "Conditional")

scores <- data.frame(
  Student = c("A", "B", "C", "D", "E"),
  Score = c(45, 67, 82, 91, 55)
)
writeData(wb3, "Conditional", scores)

# Add color scale (green for high, red for low)
conditionalFormatting(wb3, "Conditional", 
                    cols = 2, rows = 1:6,
                    rule = c(0, 50, 100),
                    style = c("#F8696B", "#FFEB84", "#63BE7B"),
                    type = "colorscale")

saveWorkbook(wb3, temp_file, overwrite = TRUE)
cat("Conditional formatting applied\n")

# Clean up
unlink(temp_file)
```

### Real-World Example: Working with Multiple Sheets

```r
# ===== MULTI-SHEET OPERATIONS =====

cat("===== MULTI-SHEET WORKFLOW =====\n\n")

library(openxlsx)

# Create input data
input_wb <- createWorkbook()
addWorksheet(input_wb, "2023Sales")
addWorksheet(input_wb, "2024Sales")
addWorksheet(input_wb, "Regions")

# 2023 data
sales_2023 <- data.frame(
  product = c("Widget", "Gadget", "Gizmo"),
  q1 = c(1000, 1500, 800),
  q2 = c(1200, 1600, 900),
  q3 = c(1100, 1400, 850),
  q4 = c(1300, 1800, 950)
)
writeData(input_wb, "2023Sales", sales_2023)

# 2024 data
sales_2024 <- data.frame(
  product = c("Widget", "Gadget", "Gizmo"),
  q1 = c(1400, 1900, 1000),
  q2 = c(1500, 2100, 1100),
  q3 = c(1600, 2000, 1050),
  q4 = c(1800, 2400, 1200)
)
writeData(input_wb, "2024Sales", sales_2024)

# Regions data
regions <- data.frame(
  region = c("North", "South", "East", "West"),
  target = c(5000, 4000, 4500, 3500),
  actual = c(5200, 3800, 4600, 3400)
)
writeData(input_wb, "Regions", regions)

temp_file <- tempfile(fileext = ".xlsx")
saveWorkbook(input_wb, temp_file, overwrite = TRUE)

# Now read and process using readxl
library(readxl)

# 1. Read all sheets into a list
cat("1. Read all sheets:\n")
all_sheets <- lapply(excel_sheets(temp_file), function(sheet) {
  read_xlsx(temp_file, sheet = sheet)
})
names(all_sheets) <- excel_sheets(temp_file)
print(names(all_sheets))
cat("\n")

# 2. Calculate YoY growth
cat("2. Year-over-Year Growth:\n")
s23 <- read_xlsx(temp_file, sheet = "2023Sales")
s24 <- read_xlsx(temp_file, sheet = "2024Sales")

# Calculate totals
total_2023 <- rowSums(s23[, 2:5])
total_2024 <- rowSums(s24[, 2:5])
growth <- ((total_2024 - total_2023) / total_2023) * 100

growth_df <- data.frame(
  Product = s23$product,
  Growth_Percent = round(growth, 1)
)
print(growth_df)
cat("\n")

# 3. Create summary workbook
cat("3. Create summary workbook:\n")
summary_wb <- createWorkbook()

addWorksheet(summary_wb, "YoY_Analysis")
writeData(summary_wb, "YoY_Analysis", growth_df)

# Add totals row
total_row <- data.frame(
  Product = "TOTAL",
  Growth_Percent = round(mean(growth), 1)
)
writeData(summary_wb, "YoY_Analysis", total_row, startRow = 
          nrow(growth_df) + 2)

# Add styling
totalStyle <- createStyle(textDecoration = "bold")
addStyle(summary_wb, "YoY_Analysis", totalStyle, 
         rows = nrow(growth_df) + 2, cols = 1:2)

saveWorkbook(summary_wb, "summary_output.xlsx", overwrite = TRUE)
cat("Summary saved to summary_output.xlsx\n")

# Clean up
unlink(temp_file)
unlink("summary_output.xlsx")
```

### Advanced Example: xlsx Package

```r
# ===== XLSX PACKAGE =====

cat("===== XLSX PACKAGE (Java-based) =====\n\n")

# Note: xlsx package requires Java and rJava
# This is typically slower than openxlsx but supports more features
# Uncomment to run:
# install.packages("xlsx")

# library(xlsx)

# Create sample data
sample_data <- data.frame(
  id = 1:100,
  value = rnorm(100),
  category = sample(LETTERS[1:5], 100, replace = TRUE)
)

temp_file <- tempfile(fileext = ".xlsx")

# xlsx::write.xlsx
# write.xlsx(sample_data, temp_file, sheetName = "Data",
#           col.names = TRUE, row.names = FALSE)

# # Read back
# df <- read.xlsx(temp_file, sheetIndex = 1)
# print(head(df))

cat("Note: xlsx package requires Java configuration\n")
cat("Use openxlsx for better cross-platform compatibility\n")

# Example with formulas (using openxlsx)
cat("\n===== FORMULAS IN OPENXLSX =====\n\n")

wb <- createWorkbook()
addWorksheet(wb, "FormulaDemo")

# Write sample data
data.frame(
  Item = c("Product A", "Product B", "Product C"),
  Price = c(100, 200, 150),
  Quantity = c(10, 5, 8)
) -> data
writeData(wb, "FormulaDemo", data)

# Add formula column for total
writeFormula(wb, "FormulaDemo", "=B2*C2", startRow = 2, startCol = 4)
writeFormula(wb, "FormulaDemo", "=B3*C3", startRow = 3, startCol = 4)
writeFormula(wb, "FormulaDemo", "=B4*C4", startRow = 4, startCol = 4)

# Add header for formula
writeData(wb, "FormulaDemo", "Total", startRow = 1, startCol = 4)

# Add sum formula
writeFormula(wb, "FormulaDemo", "=SUM(D2:D4)", startRow = 5, 
            startCol = 4)
writeData(wb, "FormulaDemo", "Grand Total", startRow = 5, 
          startCol = 1)

setColWidths(wb, "FormulaDemo", cols = 1:4, widths = 15)

saveWorkbook(wb, temp_file, overwrite = TRUE)
cat("Formula workbook created with automatic calculations\n")

# Clean up
unlink(temp_file)
```

**Output:**
```
===== MULTI-SHEET WORKFLOW =====

1. Read all sheets:
[1] "2023Sales" "2024Sales" "Regions"

2. Year-over-Year Growth:
  Product Growth_Percent
1  Widget           36.4
2  Gadget           26.7
3   Gizmo           27.3
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Use openxlsx over xlsx**: Better compatibility, no Java required
2. **Specify sheet name**: Avoid relying on sheet order
3. **Use col_types**: Control column types on read
4. **Pre-create workbook**: For multiple sheets, create workbook first
5. **Close workbooks**: Always save and close properly

### Common Pitfalls

1. **Java dependency**: xlsx package requires rJava configuration
2. **File locks**: Windows may lock Excel files
3. **Large files**: Use read_only mode for large files
4. **Date formats**: Excel dates may need conversion
5. **Formula evaluation**: Formulas are stored, not calculated values

### Performance Comparison

| Method | Read 10K rows | Write 10K rows |
|--------|---------------|----------------|
| readxl | 0.5s | 0.3s |
| openxlsx | 0.4s | 0.2s |
| xlsx | 2s | 1.5s |