# R Projects: Organized Data Science Workflows

## Learning Objectives

- Understand the purpose and structure of R Projects
- Learn how to create and manage R Projects in RStudio
- Master working directory management within projects
- Implement best practices for project organization

## Theory

R Projects (.Rproj files) are a foundational tool for organizing R work. They provide a self-contained working environment that stores project-specific settings and maintains a consistent working directory. Each project maintains its own history, workspace, and development tools, making them essential for reproducible research and collaborative work.

## Step-by-Step Guide

### Creating an R Project

R Projects can be created through RStudio or directly in R. The .Rproj file stores all project-specific configuration and serves as the entry point for opening the project.

```r
# Option 1: Using RStudio
# File > New Project > New Directory > New Project
# This creates a directory with an .Rproj file

# Option 2: Using the usethis package
library(usethis)
create_project("path/to/my_project")
```

### Understanding the .Rproj File

The .Rproj file contains project-specific settings that control RStudio behavior and package development tools:

```
Version: 1
RestoreWorkspace: Default
SaveWorkspace: Default
AlwaysSaveHistory: Default

EnableCodeIndexing: Yes
UseSpacesForTab: Yes
NumSpacesForTab: 2
Encoding: UTF-8

RnwWeave: Sweave
LaTeX: PDFLaTeX

AutoAppendNewline: Yes
StripTrailingWhitespace: Yes

BuildType: Package
PackageUseDevtools: Yes
```

### Working Directory Management

When you open an R Project, the working directory automatically sets to the project root. This provides consistent file paths throughout your analysis:

```r
# Get the project root directory
here::here()

# Build paths relative to project root
data_file <- here::here("data", "raw", "dataset.csv")
output_file <- here::here("outputs", "results.rds")

# Load data using project-relative paths
data <- readr::read_csv(here::here("data", "raw", "survey_data.csv"))
```

### Project Structure

A well-organized R Project follows a consistent directory structure:

```
myproject/
├── myproject.Rproj
├── DESCRIPTION
├── NAMESPACE
├── R/
│   └── functions.R
├── data/
│   ├── raw/
│   └── processed/
├── inst/
│   └── extdata/
├── man/
├── tests/
│   └── testthat/
├── vignettes/
├── README.md
├── LICENSE
└── .gitignore
```

## Code Examples

### Setting Up a New Project with Package Development

```r
# Create a new package project with complete setup
library(usethis)
create_package("mypackage")

# Add common development infrastructure
use_git()
use_readme_md()
use_license("mit")
use_testthat()
use_rcpp()
```

### Managing Relative Paths in Projects

```r
# Using the here package for robust path management
library(here)

# Define project structure
project_dirs <- c(
  data = "data",
  output = "output",
  scripts = "R",
  figs = "figures"
)

# Check all directories exist
walk(project_dirs, ~dir.create(here(.x), showWarnings = FALSE))

# Load data files
load_data <- function(filename) {
  readRDS(here("data", filename))
}

# Save outputs
save_results <- function(obj, filename) {
  saveRDS(obj, here("output", filename))
}
```

### Version Control Integration

```r
# Initialize git repository
library(usethis)
use_git()

# Check git status
system("git status")

# Make initial commit
system("git add .")
system("git commit -m 'Initial commit'")

# Connect to GitHub
use_github(auth_token = Sys.getenv("GITHUB_TOKEN"))
```

## Best Practices

1. **One Project Per Task**: Each analysis or package should live in its own project.

2. **Consistent Structure**: Follow a standard directory layout across all projects.

3. **Use here()**: Always use the here package for relative paths instead of setwd().

4. **Version Control**: Initialize Git from the start of every project.

5. **Document Everything**: Maintain a README that explains project purpose and structure.

6. **Reproducible Environments**: Use renv or packrat for dependency management.

7. **Avoid Absolute Paths**: Never hardcode absolute paths that won't work on other machines.

## Exercises

1. Create a new R Project for a data analysis task.

2. Set up a proper directory structure with data, R, and output folders.

3. Practice using here() for file paths in your project.

4. Initialize a Git repository and make your first commit.

5. Create an R script that processes data using relative paths.

## Additional Resources

- [RStudio Projects](https://support.rstudio.com/hc/en-us/articles/200526207-Using-RStudio-Projects)
- [usethis Package](https://usethis.r-lib.org/)
- [here Package](https://here.r-lib.org/)