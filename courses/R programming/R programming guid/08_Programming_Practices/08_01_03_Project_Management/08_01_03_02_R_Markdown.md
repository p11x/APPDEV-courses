# R Markdown: Dynamic Documents for Analysis

## Learning Objectives

- Understand the purpose and structure of R Markdown documents
- Learn to create and customize R Markdown files
- Master code chunks and their options
- Implement dynamic report generation with knitr

## Theory

R Markdown provides a framework for creating dynamic documents that combine narrative text, code, and output. It extends Markdown syntax to include executable R code blocks called "chunks." When rendered, these documents execute the code and include the results inline, producing reproducible research outputs that can be shared as HTML, PDF, or Word documents.

## Step-by-Step Guide

### Creating an R Markdown Document

R Markdown documents can be created through RStudio's New File menu or created directly as .Rmd files with proper YAML header and code chunks:

```r
# Option 1: Using RStudio
# File > New File > R Markdown
# Select output format and title

# Option 2: Using rmarkdown package
library(rmarkdown)
rmarkdown::draft("my_report.Rmd", 
                template = "html_document",
                package = "rmarkdown")
```

### Understanding Document Structure

An R Markdown document consists of three main components: the YAML header, narrative text, and code chunks:

```yaml
---
title: "My Analysis Report"
author: "Jane Doe"
date: "`r Sys.Date()`"
output: html_document
---
```

### YAML Header Options

```yaml
---
title: "Statistical Analysis Report"
author: "John Smith"
date: "`r format(Sys.Date(), '%B %d, %Y')`"
output: 
  html_document:
    theme: united
    highlight: tango
    toc: true
    toc_float: true
    code_folding: show
  pdf_document:
    toc: true
    number_sections: true
---
```

### Code Chunks

```r
```{r setup, include = FALSE}
library(tidyverse)
library(ggplot2)

opts_chunk$set(
  echo = TRUE,
  message = FALSE,
  warning = FALSE,
  fig.align = "center"
)
```

```{r analysis}
# Data analysis code
summary_data <- mtcars |>
  group_by(cyl) |>
  summarize(
    mean_mpg = mean(mpg),
    sd_mpg = sd(mpg),
    .groups = "drop"
  )
summary_data
```
```

### Inline Code

```markdown
The dataset contains `r nrow(mtcars)` observations with 
`r ncol(mtcars)` variables. The average MPG is 
`r round(mean(mtcars$mpg), 2)`.
```

## Code Examples

### Complete R Markdown Document

```markdown
---
title: "Analysis of MTCars Dataset"
author: "Data Analyst"
date: "`r Sys.Date()`"
output: html_document
---

# Introduction

This report analyzes the mtcars dataset to understand the 
relationship between car characteristics and fuel efficiency.

```{r setup, include = FALSE}
library(tidyverse)
library(ggplot2)
opts_chunk$set(echo = TRUE, warning = FALSE, message = FALSE)
```

# Data Summary

```{r data-loading}
data(mtcars)
head(mtcars)
```

# Analysis

## Fuel Efficiency by Cylinder

```{r cylinder-analysis}
mtcars |>
  group_by(cyl) |>
  summarize(
    n = n(),
    mean_mpg = mean(mpg),
    sd_mpg = sd(mpg)
  )
```

## Visualization

```{r plot, fig.cap = "Fuel efficiency by cylinders"}
ggplot(mtcars, aes(x = factor(cyl), y = mpg)) +
  geom_boxplot(fill = "steelblue") +
  labs(x = "Cylinders", y = "Miles per Gallon") +
  theme_minimal()
```

# Conclusion

Cars with fewer cylinders have better fuel efficiency.
```

### Custom Chunk Options

```r
# Hide code, show output
```{r, echo = FALSE}
summary(mtcars$mpg)
```

# Show only plots
```{r, include = FALSE}
ggplot(mtcars, aes(x = wt, y = mpg)) + geom_point()
```

# Cache expensive computations
```{r, cache = TRUE}
slow computation here
```
```

### Parameterized Reports

```yaml
---
title: "Parameterized Report"
output: html_document
params:
  year: 2024
  region: "North America"
---
```

## Best Practices

1. **Organize with Headers**: Use markdown headers for clear hierarchy.

2. **Set Default Options**: Configure chunk options in a setup chunk.

3. **Document While You Work**: Write narrative text as you develop analysis.

4. **Cache Expensive Operations**: Use cache = TRUE for time-consuming code.

5. **Use Child Documents**: Break large documents into reusable parts.

6. **Version Control**: Keep .Rmd files under version control.

7. **Test Rendering**: Render documents regularly to catch errors.

## Exercises

1. Create an R Markdown document with HTML output.

2. Add code chunks with different options (echo, include, cache).

3. Create a parameterized report using YAML params.

4. Convert an existing R script to R Markdown.

5. Add a table of contents and custom styling.

## Additional Resources

- [R Markdown Cookbook](https://bookdown.org/yihui/rmarkdown-cookbook/)
- [R Markdown: The Definitive Guide](https://bookdown.org/yihui/rmarkdown/)
- [knitr Documentation](https://yihui.org/knitr/)