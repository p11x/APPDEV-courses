# Script Structure in R

## Learning Objectives

- Understand proper R script organization
- Create well-structured R scripts
- Use consistent header sections
- Implement proper code sections
- Document script purpose and usage

## Theory

Well-structured R scripts follow a consistent pattern that improves readability and maintainability. A typical script includes header documentation, setup and configuration, loading libraries, defining functions, main execution, and output handling.

Following a standard structure makes it easier for others (and your future self) to understand and modify the code. It also helps when debugging issues or adding new features.

## Step-by-Step

1. Create script header with metadata
2. Set up configuration and paths
3. Load required libraries
4. Define helper functions
5. Implement main logic
6. Add output and cleanup code

## Code Examples

### Standard Script Template

```r
#!/usr/bin/env Rscript
# =============================================================================
# Script: example_analysis.R
# Description: Performs statistical analysis on sample data
# Author: Your Name
# Created: 2024-01-15
# =============================================================================

# Configuration ------------------------------------------------------------
# Set working directory
setwd(dirname(parent.frame(2)$ofile))

# R configuration
options(
  stringsAsFactors = FALSE,
  scipen = 10,
  digits = 4
)

# Libraries ------------------------------------------------------------
library(stats)
library(graphics)

# Custom functions ------------------------------------------------------

#' Calculate summary statistics
#' @param x Numeric vector
#' @return List with summary statistics
calculate_summary <- function(x) {
  list(
    mean = mean(x, na.rm = TRUE),
    median = median(x, na.rm = TRUE),
    sd = sd(x, na.rm = TRUE),
    n = sum(!is.na(x)),
    missing = sum(is.na(x))
  )
}

# Main execution -------------------------------------------------------

main <- function() {
  cat("Starting analysis...\n\n")
  
  # Sample data
  data <- rnorm(100, mean = 50, sd = 10)
  
  # Calculate summary
  summary <- calculate_summary(data)
  
  cat("Summary Statistics:\n")
  cat("Mean:", summary$mean, "\n")
  cat("Median:", summary$median, "\n")
  cat("SD:", summary$sd, "\n")
  cat("N:", summary$n, "\n")
  
  cat("\nAnalysis complete.\n")
}

# Run main function
if (!interactive()) {
  main()
}
```

### Script with Command-Line Arguments

```r
#!/usr/bin/env Rscript
# =============================================================================
# Script: data_processor.R
# Description: Process data file with optional arguments
# =============================================================================

# Libraries ------------------------------------------------------------
suppressPackageStartupMessages(library(optparse))

# Parse command line options
option_list <- list(
  make_option(c("-i", "--input"), type = "character", 
              help = "Input file path"),
  make_option(c("-o", "--output"), type = "character",
              help = "Output file path"),
  make_option(c("-v", "--verbose"), action = "store_true", default = TRUE,
              help = "Print verbose output [default]")
)

opt <- parse_args(OptionParser(option_list = option_list))

# Main execution -------------------------------------------------------

if (is.null(opt$input)) {
  stop("Input file required. Use --input flag.\n")
}

if (opt$verbose) {
  cat("Reading:", opt$input, "\n")
}

# Process data
data <- read.csv(opt$input)

# Write output if specified
if (!is.null(opt$output)) {
  write.csv(data, opt$output, row.names = FALSE)
  cat("Written to:", opt$output, "\n")
}
```

## Real-World Example: Analysis Script

```r
# Real-world: Statistical analysis script
cat("===== ANALYSIS SCRIPT =====\n\n")

#' Statistical Analysis Script
#' Performs complete analysis pipeline
#' 
#' Usage:
#'   Rscript analyze.R --input data.csv --output results.csv
#'
#' @title Statistical Analysis Pipeline

# Configuration section
PROJECT_DIR <- "C:/projects/analysis"
DATA_DIR <- file.path(PROJECT_DIR, "data")
OUTPUT_DIR <- file.path(PROJECT_DIR, "output")

# Create output directory if needed
if (!dir.exists(OUTPUT_DIR)) {
  dir.create(OUTPUT_DIR, recursive = TRUE)
}

# Libraries section
suppressPackageStartupMessages(library(data.table))

# Functions section
analyze_data <- function(data_file) {
  cat("Loading data from:", data_file, "\n")
  dt <- fread(data_file)
  
  cat("Rows:", nrow(dt), "Columns:", ncol(dt), "\n\n")
  
  # Analysis
  result <- dt[, .(
    mean = mean(value),
    median = median(value),
    sd = sd(value)
  ), by = group]
  
  cat("Results:\n")
  print(result)
  
  result
}

# Main section
main <- function() {
  input_file <- file.path(DATA_DIR, "analysis_data.csv")
  
  if (!file.exists(input_file)) {
    stop("Input file not found: ", input_file)
  }
  
  results <- analyze_data(input_file)
  
  output_file <- file.path(OUTPUT_DIR, "results.csv")
  fwrite(results, output_file)
  
  cat("\nResults saved to:", output_file, "\n")
}

# Execute
main()
```

## Best Practices

1. Always include header with script name, description, author, date
2. Add section dividers (e.g., "=== Libraries ===")
3. Use consistent naming (snake_case recommended)
4. Document functions with roxygen-style comments
5. Include error handling for file operations
6. Add verbose output option for debugging
7. Use command-line arguments for flexibility

## Exercises

1. Create a script template with all sections
2. Add command-line argument parsing to an existing script
3. Write a script that processes multiple input files
4. Implement logging in a script
5. Create a reproducible script with session info