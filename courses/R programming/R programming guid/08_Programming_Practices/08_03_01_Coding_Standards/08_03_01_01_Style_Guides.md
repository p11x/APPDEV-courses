# Style Guides in R

## Learning Objectives

- Understand the importance of coding style
- Learn common R style guides
- Apply consistent formatting
- Build maintainable code

## Theory

Consistent code style improves readability and maintainability. While R doesn't enforce style, adhering to style guides makes code easier to understand and share. The tidyverse style guide is widely adopted in the R community.

## Step-by-Step Guide

### Tidyverse Style Guide Basics

```r
# File names: snake_case, descriptive
# Good: 01_02_03_predict_values.R
# Bad: predict.R, pred.R, myPreds.R

# Object names: snake_case, descriptive
# Good: patient_id, mean_income
# Bad: patientid, x, myVar

# Function names: verbs
# Good: calculate_mean(), filter_data()
# Bad: mean(), DataFilter()
```

### Spacing

```r
# Spaces around operators
x <- 1 + 2
y <- x * 3

# No spaces in function calls
mean(x)
filter(df, condition)

# Spaces after commas
c(1, 2, 3)
mean(x, na.rm = TRUE)
```

### Line Length

```r
# Keep lines under 80 characters
# Break long function calls
result <- data |>
  filter(condition) |>
  group_by(group) |>
  summarize(mean = mean(value))
```

### Indentation

```r
# Use two spaces for indentation
# Never use tabs

if (condition) {
  do_something()
  do_something_else()
}

# In data pipelines, indent after %>%
result <- data |>
  filter(condition) |>
  mutate(new_var = old_var * 2)
```

## Code Examples

### Good vs Bad Code Style

```r
# Bad style
x<-c(1,2,3 );mean(x,TRUE)

# Good style
x <- c(1, 2, 3)
mean(x, na.rm = TRUE)

# Bad: Non-descriptive names
x<-c(1,2,3);y<-mean(x)

# Good: Descriptive names
values <- c(1, 2, 3)
mean_value <- mean(values)

# Bad: Nested code
result<-ifelse(test,a,ifelse(test2,b,ifelse(test3,c,d)))

# Good: Clear structure
result <- case_when(
  test ~ a,
  test2 ~ b,
  test3 ~ c,
  TRUE ~ d
)
```

### Using styler Package

```r
# Install and use styler
library(styler)

# Style a file
style_file("R/my_file.R")

# Style all files in a package
style_pkg()

# Apply tidyverse style
style_file("R/my_file.R", style = "tidyverse")
```

### Using lintr

```r
library(lintr)

# Lint a file
lint("R/my_file.R")

# Common linters
linters <- with_defaults(
  line_length_linter(80),
  object_usage_linter = NULL,
  trailing_whitespace_linter = NULL
)

lint("R/my_file.R", linters = linters)
```

## Best Practices

1. **Consistency**: Pick a style and stick with it.

2. **Descriptive Names**: Use meaningful names.

3. **Readable Code**: Use spacing and line breaks.

4. **Tools**: Use styler and lintr.

5. **Review**: Code review for style issues.

## Exercises

1. Read the tidyverse style guide.

2. Use styler to format your code.

3. Add lintr checks to your project.

4. Create project-specific styling rules.

5. Review code with style in mind.

## Additional Resources

- [Tidyverse Style Guide](https://style.tidyverse.org/)
- [Google R Style Guide](https://google.github.io/styleguide/Rguide.html)
- [styler Package](https://styler.r-lib.org/)
- [lintr Package](https://github.com/r-lib/lintr)