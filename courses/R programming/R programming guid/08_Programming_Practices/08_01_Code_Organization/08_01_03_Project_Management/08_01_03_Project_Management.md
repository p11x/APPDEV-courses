# Project Management in R

This chapter covers organizing R projects effectively using RStudio projects, the here package, and proper directory structure.

## 1. RStudio Projects

RStudio projects provide a self-contained working directory and maintain session state.

### Creating Projects

```r
# File > New Project
# Or via R console
file.create("my_project.Rproj")

# Project file contents
# Version: 1.0
# RestoreWorkspace: Default
# SaveWorkspace: Default
# AlwaysSaveHistory: Default

# Enable/disable options
# - Restore .RData into workspace at startup
# - Save .RData to workspace on exit
# - Always save history (even if not saving .RData)
```

### Project File Structure

```r
# Standard RStudio project structure
# project_name.Rproj
# R/                          # R code scripts
# data/
#   raw/                      # Raw data, never modify
#   processed/                # Cleaned data
# output/
#   figures/                  # Plots and visualizations
#   tables/                   # Output tables
# docs/                       # Documentation
# README.md                   # Project overview
# LICENSE                     # License file
```

### .Rproj File Options

```r
# Version: 1.0
# RestoreWorkspace: No
# SaveWorkspace: Ask
# AlwaysSaveHistory: Yes
# EnableCodeIndex: Yes
# UnixEncoding: UTF-8

# UseRStudio: Yes
# BuildType: Package
# PackageUseDevtools: Yes
# PackageInstallArgs: --no-multiarch
# PackageR CMD build options: --no-manual
```

## 2. The here Package

The here package creates reliable paths relative to your project root.

### Basic Usage

```r
# Install here if needed
install.packages("here")

# Load package
library(here)

# Initializing in project root
here()

# Current file location
here("data", "raw", "data.csv")

# Equivalent to file.path but relative to project root
here("R", "analysis.R")
```

### Setting Up New Project

```r
# First time setup: tell here about your project root
here::set_here()

# or more commonly
library(here)
here()

# Creates .here file in project root
```

### Building File Paths

```r
# Define paths to project directories
data_dir <- here("data")
raw_dir <- here("data", "raw")
output_dir <- here("output")

# Load data using here
data <- read.csv(here("data", "raw", "survey_data.csv"))

# Save outputs
write.csv(results, here("output", "results.csv"))

# Save plots
ggsave(here("output", "figures", "plot1.png"), plot = p)

# Using with common packages
library(readr)
read_csv(here("data", "raw", "data.csv"))

library(ggplot2)
ggsave(here("output", "figures", "chart.png"))
```

### Dynamic Path Resolution

```r
# Function to build paths dynamically
get_path <- function(...) {
  here(...)
}

# Usage
path_data <- get_path("data", "raw")
path_output <- get_path("output", "results.csv")

# Better: create project-wide constants
project_paths <- list(
  data = here("data"),
  data_raw = here("data", "raw"),
  data_processed = here("data", "processed"),
  output = here("output"),
  output_figures = here("output", "figures"),
  output_tables = here("output", "tables"),
  R = here("R"),
  docs = here("docs")
)

# Use: project_paths$output_figures
```

## 3. Working Directory Management

### Without here (anti-patterns)

```r
# Anti-patterns - avoid these:
setwd("C:/Users/name/project")  # Not portable

read.csv("data.csv")  # Depends on current directory

source("../R/functions.R")  # Fragile relative paths
```

### With here (best practices)

```r
# Best practice: always use here
library(here)

# Read data relative to project root
data <- read.csv(here("data", "raw", "input.csv"))

# Source R files
source(here("R", "utils.R"))

# Save outputs
write.csv(output, here("output", "results.csv"))

# Print paths for debugging
cat("Project root:", here(), "\n")
```

### RStudio Settings for Paths

```r
# Tools > Project Options > General
# - No: Don't preserve .RData
# - Ask: Ask before saving .RData

# Tools > Project Options > Code Editing
# - Enable "Insert
# - native pipe operator"
# - Enable "Auto-indent code after paste"

# Tools > Project Options > Sweave
# - options to set for reports
```

## 4. Directory Structure Best Practices

### Recommended Layout

```r
# Minimal project structure
myproject/
├── myproject.Rproj
├── R/
│   ├── 00_setup.R
│   ├── 01_load_data.R
│   ├── 02_clean_data.R
│   ├── 03_analysis.R
│   └── 04_visualization.R
├── data/
│   ├── raw/
│   └── processed/
├── output/
├── README.md
└── .gitignore
```

### Complete Layout

```r
# research_project/
# ├── ProjectName.Rproj
# ├── R/
# │   ├── 00_libs.R              # Load all libraries
# │   ├── 01_functions.R        # Custom functions
# │   ├── 02_load_data.R        # Data loading
# │   ├── 03_clean_data.R       # Data cleaning
# │   ├── 04_eda.R              # Exploratory analysis
# │   ├── 05_analysis.R         # Main analysis
# │   └── 06_report.R          # Report generation
# ├── data/
# │   ├── raw/                  # Original, immutable data
# │   └── processed/           # Cleaned data
# ├── output/
# │   ├── figures/             # Plots
# │   └── tables/               # Summary tables
# ├── docs/
# │   ├── paper.md
#   └── slides.Rmd
# ├── tests/
# │   └── test_that/
# │       └── test_functions.R
# ├── README.md
# ├── LICENSE
# ├── DESCRIPTION
# └── NAMESPACE
```

### Creating Structure Programmatically

```r
# Create project directories
create_project_structure <- function() {
  dirs <- c("R", "data/raw", "data/processed",
            "output/figures", "output/tables", "docs")
  
  for (dir in dirs) {
    if (!dir.exists(dir)) {
      dir.create(dir, recursive = TRUE)
    }
  }
  
  message("Project structure created")
}

# Run once per project
create_project_structure()
```

## 5. Relative vs Absolute Paths

### Understanding the Difference

```r
# Absolute path - specifies full location
absolute <- "C:/Users/john/Documents/project/data.csv"

# Relative path - relative to current working directory
relative <- "data.csv"

# Using here makes relative to project root
here_relative <- here("data.csv")

# When NOT to use here:
# - In packages (use system.file() instead)
# - When data must be found regardless of working directory

# In package development:
system.file("extdata", "data.csv", package = "mypackage")
```

### Working Directory Functions

```r
# Get current working directory
getwd()
# [1] "C:/Users/john/Documents/project"

# Set working directory
setwd("C:/Users/john/Documents/project")

# In RStudio, projects automatically setwd to project root
# This is one reason to always use projects
```

## 6. renv for Dependency Management

### Setting Up renv

```r
# Install renv
install.packages("renv")

# Initialize in project
renv::init()

# This creates:
# - renv.lock (lockfile)
# - renv/ (private library)

# Snapshot current state
renv::snapshot()

# Restore from lockfile
renv::restore()
```

### Using renv

```r
# Adding packages
library(dplyr)
renv::snapshot()

# Installing packages respects lockfile
renv::restore()

# Update a package
install.packages("ggplot2")
renv::snapshot()

# Check for updates
renv::check()
```

## 7. .gitignore Files

```r
# Typical .gitignore for R projects
# .Rproj.user/
# .Rhistory
# .RData
# .Ruserdata/
# renv/
# renv.lock
# *.Rproj
# !*.Rproj.user
# data/raw/*
# output/*
# docs/*.html
```

## Summary

- Use RStudio projects for self-contained workflows
- Use here() for paths relative to project root
- Maintain clear directory structure
- Consider renv for reproducible dependencies
- Version control with git (.gitignore)
- Never modify raw data - copy to processed
- Document your project with README