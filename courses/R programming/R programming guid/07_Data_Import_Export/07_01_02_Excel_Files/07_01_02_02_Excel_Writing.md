# Writing Excel Files in R

## Learning Objectives

- Write data frames to Excel format
- Create multi-sheet workbooks
- Format cells (bold, colors, borders)
- Add formulas to Excel files
- Control column widths and sheet names

## Theory

The openxlsx package provides comprehensive Excel file creation capabilities in R. It can create .xlsx files without requiring Excel to be installed. Key functions include createWorkbook(), addWorksheet(), writeData(), and saveWorkbook().

Formatting is handled through style objects created with createStyle(). The package can add headers, merge cells, freeze panes, and set column widths.

## Step-by-Step

1. Create a new workbook with createWorkbook()
2. Add worksheets with addWorksheet()
3. Write data with writeData() or writeDataTable()
4. Apply formatting if needed
5. Save with saveWorkbook()

## Code Examples

### Basic Excel Writing

```r
cat("===== BASIC EXCEL WRITING =====\n\n")

library(openxlsx)

# Create workbook
wb <- createWorkbook()

# Add sheet
addWorksheet(wb, "MyData")

# Write data
writeData(wb, "MyData", mtcars[1:5, ])

# Save
saveWorkbook(wb, "basic_output.xlsx", overwrite = TRUE)
cat("Basic Excel file written\n")
```

### Multi-Sheet Workbook Creation

```r
cat("\n===== MULTI-SHEET WORKBOOK =====\n\n")

# Create workbook with multiple sheets
wb2 <- createWorkbook()

# Sheet 1: Sales data
addWorksheet(wb2, "Sales")
sales_data <- data.frame(
  Month = month.abb[1:6],
  Revenue = c(10000, 12000, 15000, 11000, 13000, 16000),
  Target = rep(12000, 6)
)
writeData(wb2, "Sales", sales_data, startRow = 1)

# Sheet 2: Product data
addWorksheet(wb2, "Products")
products <- data.frame(
  Product = c("Widget A", "Widget B", "Gadget"),
  Price = c(19.99, 29.99, 49.99),
  Stock = c(100, 50, 25)
)
writeData(wb2, "Products", products)

# Sheet 3: Summary
addWorksheet(wb2, "Summary")
summary_data <- data.frame(
  Metric = c("Total Revenue", "Average Revenue"),
  Value = c(sum(sales_data$Revenue), mean(sales_data$Revenue))
)
writeData(wb2, "Summary", summary_data)

saveWorkbook(wb2, "multi_sheet_output.xlsx", overwrite = TRUE)
cat("Multi-sheet workbook written\n")
```

### Cell Formatting

```r
cat("\n===== CELL FORMATTING =====\n\n")

# Create formatting styles
wb3 <- createWorkbook()
addWorksheet(wb3, "Formatted")

# Header style (bold, centered)
header_style <- createStyle(
  fontSize = 14,
  fontColour = "#FFFFFF",
  halign = "center",
  fgFill = "#4F81BD",
  border = "TopBottom",
  borderColour = "#4F81BD",
  textDecoration = "bold"
)

# Number format style
currency_style <- createStyle(numFmt = "$0,000.00")

# Create data with header
df_fmt <- data.frame(
  Item = c("Product A", "Product B", "Product C"),
  Price = c(99.99, 149.99, 199.99),
  Quantity = c(10, 5, 3)
)

# Write header with style
writeData(wb3, "Formatted", x = "Sales Report", startRow = 1)
mergeCells(wb3, "Formatted", rows = 1, cols = 1:3)
addStyle(wb3, "Formatted", header_style, rows = 1, cols = 1:3)

# Write column headers
writeData(wb3, "Formatted", df_fmt, startRow = 3, colNames = TRUE)
addStyle(wb3, "Formatted", header_style, rows = 3, cols = 1:3, gridExpand = TRUE)

# Apply currency format
addStyle(wb3, "Formatted", currency_style, rows = 4:6, cols = 2, gridExpand = TRUE)

# Set column widths
setColWidths(wb3, "Formatted", cols = 1:3, widths = c(15, 12, 12))

saveWorkbook(wb3, "formatted_output.xlsx", overwrite = TRUE)
cat("Formatted Excel file written\n")
```

### Adding Formulas

```r
cat("\n===== FORMULAS IN EXCEL =====\n\n")

wb4 <- createWorkbook()
addWorksheet(wb4, "Calculations")

# Write data
writeData(wb4, "Calculations", x = data.frame(
  Item = c("Item A", "Item B", "Item C", "", "Total"),
  Price = c(100, 200, 150, NA, NA),
  Quantity = c(2, 3, 1, NA, NA)
))

# Add formula for total (row 4 adds Price * Quantity)
writeFormula(wb4, "Calculations", x = "=B2*C2", startRow = 2, startCol = 4)
writeFormula(wb4, "Calculations", x = "=B3*C3", startRow = 3, startCol = 4)
writeFormula(wb4, "Calculations", x = "=B4*C4", startRow = 4, startCol = 4)
writeFormula(wb4, "Calculations", x = "=SUM(D2:D4)", startRow = 5, startCol = 4)

# Header
names(wb4)[[1]] <- c("Item", "Price", "Qty", "Total", "")
writeData(wb4, "Calculations", x = names(wb4)[[1]], rowNames = FALSE)

saveWorkbook(wb4, "formulas_output.xlsx", overwrite = TRUE)
cat("Excel with formulas written\n")
```

## Real-World Example: Financial Report Export

```r
# Real-world: Export financial report to Excel
cat("===== FINANCIAL REPORT EXPORT =====\n\n")

# Create report data
report_wb <- createWorkbook()

# Summary sheet
addWorksheet(report_wb, "Summary")

# Apply styles
title_style <- createStyle(fontSize = 16, textDecoration = "bold", halign = "center")
header_style <- createStyle(fgFill = "#2E75B6", fontColour = "#FFFFFF", textDecoration = "bold")

# Title
writeData(report_wb, "Summary", "Financial Report Q1 2024", startRow = 1)
mergeCells(report_wb, "Summary", rows = 1, cols = 1:4)
addStyle(report_wb, "Summary", title_style, rows = 1, cols = 1)

# Summary data
summary_df <- data.frame(
  Category = c("Revenue", "COGS", "Gross Profit", "Expenses", "Net Income"),
  Amount = c(500000, 200000, 300000, 150000, 150000)
)
writeData(report_wb, "Summary", summary_df, startRow = 3)

# Add header style
addStyle(report_wb, "Summary", header_style, rows = 3, cols = 1:2, gridExpand = TRUE)

# Set column width
setColWidths(report_wb, "Summary", cols = 1:2, widths = c(20, 15))

# Detail sheet
addWorksheet(report_wb, "Details")
detail_df <- data.frame(
  Date = seq(as.Date("2024-01-01"), by = "day", length.out = 10),
  Description = paste("Transaction", 1:10),
  Amount = sample(-500:500, 10)
)
writeData(report_wb, "Details", detail_df)

saveWorkbook(report_wb, "financial_report.xlsx", overwrite = TRUE)
cat("Financial report exported\n")

# Clean up test files
unlink("basic_output.xlsx")
unlink("multi_sheet_output.xlsx")
unlink("formatted_output.xlsx")
unlink("formulas_output.xlsx")
unlink("financial_report.xlsx")
```

## Best Practices

1. Use openxlsx for Excel creation (no external dependencies)
2. Create header styles for clear data boundaries
3. Use formulas for calculated fields (update automatically)
4. Set appropriate column widths for readability
5. Document formatting in code comments

## Exercises

1. Create a workbook with 5 sheets and different data in each
2. Add conditional formatting to Excel cells
3. Create a report with pivot table functionality
4. Freeze header rows in Excel output
5. Add charts to Excel workbook