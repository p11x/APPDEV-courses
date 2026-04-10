# Factors in R

## Learning Objectives

- Understand what factors are in R and why they exist
- Create and manipulate factors
- Understand factor levels and ordering
- Apply factors in statistical analysis and data visualization

## Theoretical Background

### What is a Factor?

A factor is a categorical data type in R that takes a limited number of different values. It is stored internally as an integer vector with a set of labels (levels). Factors are essential for statistical modeling where categorical variables are common.

### Why Use Factors?

1. **Memory efficiency**: Stores integers instead of repeated strings
2. **Categorical analysis**: Essential for ANOVA, regression with categories
3. **Data validation**: Prevents typos in categorical data
4. **Ordered categories**: Supports ordinal data

### Factor Properties

- **Levels**: Unique possible values
- **Ordered**: Can be ordered or unordered
- **Labels**: Optional display labels

## Code Examples

### Standard Example: Factor Creation and Manipulation

```r
# ===== FACTOR CREATION IN R =====

cat("===== CREATING FACTORS =====\n\n")

# 1. Creating factors with factor()
# Basic factor from character vector
colors <- c("Red", "Blue", "Green", "Blue", "Red", "Green", "Blue")
color_factor <- factor(colors)

cat("Original vector:", colors, "\n")
cat("As factor:", color_factor, "\n")
cat("Levels:", levels(color_factor), "\n")
cat("Class:", class(color_factor), "\n")

# 2. Specifying level order
# Define explicit order for categories
satisfaction <- c("Low", "High", "Medium", "High", "Low", "Medium", "High")
satisfaction_factor <- factor(satisfaction, 
                               levels = c("Low", "Medium", "High"),
                               ordered = TRUE)

cat("\n\nOrdered factor:\n")
cat("Factor:", satisfaction_factor, "\n")
cat("Is ordered:", is.ordered(satisfaction_factor), "\n")

# 3. Factor with labels
# Using labels to change display values
gender_codes <- c(1, 2, 1, 1, 2, 1, 2)
gender_factor <- factor(gender_codes, 
                        levels = c(1, 2),
                        labels = c("Male", "Female"))

cat("\n\nFactor with labels:\n")
cat("Factor:", gender_factor, "\n")

# 4. Checking factor properties
cat("\n===== FACTOR PROPERTIES =====\n\n")
cat("nlevels (number of levels):", nlevels(color_factor), "\n")
cat("levels():", levels(color_factor), "\n")
cat("table() frequencies:\n")
print(table(color_factor))

# 5. Adding new levels
# This is useful when you know all possible values
animals <- factor(c("dog", "cat", "bird"))
# Add a new level that doesn't appear in data
levels(animals) <- c(levels(animals), "fish")
# Now we can add fish to the vector
animals[3] <- "fish"
cat("\n\nAfter adding level:\n")
print(animals)
```

**Output:**
```
===== CREATING FACTORS =====

Original vector: Red Blue Green Blue Red Green Blue
As factor: Red Blue Green Blue Red Green Blue
Levels: Blue Green Red
Class: factor
```

**Comments:**
- Factors are stored as integers with level mappings
- Default level order is alphabetical
- Use `ordered = TRUE` for ordinal data

### Real-World Example 1: Survey Response Analysis

```r
# Real-world application: Analyzing survey categorical data
# This demonstrates factors in survey analysis

# Create survey response data
survey_data <- data.frame(
  respondent_id = 1:10,
  age_group = factor(c("25-34", "18-24", "35-44", "45-54", "25-34",
                     "18-24", "35-44", "45-54", "25-34", "18-24"),
                   levels = c("18-24", "25-34", "35-44", "45-54", "55-64"),
                   ordered = TRUE),
  education = factor(c("Bachelor", "High School", "Master", "Bachelor",
                      "High School", "PhD", "Master", "Bachelor",
                      "High School", "PhD"),
                    levels = c("High School", "Bachelor", "Master", "PhD"),
                    ordered = TRUE),
  satisfaction = factor(c("Satisfied", "Neutral", "Dissatisfied", "Satisfied",
                         "Satisfied", "Neutral", "Satisfied", "Dissatisfied",
                         "Neutral", "Satisfied")),
  response_time = c(5, 3, 7, 8, 6, 9, 4, 5, 6, 7)
)

cat("===== SURVEY DATA WITH FACTORS =====\n\n")
print(survey_data)

# Analyze satisfaction by education
cat("\n===== SATISFACTION BY EDUCATION =====\n\n")
satisfaction_table <- table(survey_data$education, survey_data$satisfaction)
print(satisfaction_table)

# Proportions
cat("\nProportions by education:\n")
print(prop.table(satisfaction_table, margin = 1))

# Cross-tabulation with chi-square test
cat("\n===== CHI-SQUARE TEST =====\n\n")
chi_sq <- chisq.test(satisfaction_table)
cat("Chi-square statistic:", round(chi_sq$statistic, 3), "\n")
cat("p-value:", round(chi_sq$p.value, 4), "\n")

# Aggregate by age group
cat("\n===== RESPONSE TIME BY AGE GROUP =====\n\n")
agg_response <- aggregate(response_time ~ age_group, 
                         data = survey_data,
                         FUN = mean)
print(agg_response)

# Reorder factor levels by response time
cat("\n===== REORDERED BY RESPONSE =====\n\n")
age_order <- order(agg_response$response_time)
ordered_levels <- levels(survey_data$age_group)[age_order]
cat("Age groups ordered by response time:\n")
cat(" ", paste(ordered_levels, collapse = " < "), "\n")
```

**Output:**
```
===== SURVEY DATA WITH FACTORS =====

   respondent_id age_group education ...
```

**Comments:**
- Ordered factors preserve meaningful order
- `table()` creates frequency counts
- Chi-square test works with factor data

### Real-World Example 2: Experimental Design with Factors

```r
# Real-world application: Analyzing experimental data with factors
# This demonstrates factors in designed experiments

# Create experimental data
# Simulate a drug trial with treatment groups
set.seed(42)

experiment <- data.frame(
  subject = 1:60,
  # Treatment: 3 groups (Placebo, Drug A, Drug B)
  treatment = factor(rep(c("Placebo", "Drug A", "Drug B"), each = 20)),
  # Response variable (blood pressure reduction)
  bp_reduction = c(
    rnorm(20, mean = 2, sd = 3),    # Placebo
    rnorm(20, mean = 8, sd = 4),    # Drug A
    rnorm(20, mean = 12, sd = 5)    # Drug B
  ),
  # Patient characteristic
  gender = factor(rep(rep(c("Male", "Female"), each = 10), 3))
)

cat("===== DRUG TRIAL DATA =====\n\n")
print(head(experiment))

# Summary by treatment
cat("\n===== SUMMARY BY TREATMENT =====\n\n")
treatment_summary <- experiment %>%
  group_by(treatment) %>%
  summarise(
    n = n(),
    mean_bp = mean(bp_reduction),
    sd_bp = sd(bp_reduction),
    min_bp = min(bp_reduction),
    max_bp = max(bp_reduction)
  )
print(treatment_summary)

# Two-way analysis by treatment and gender
cat("\n===== TWO-WAY SUMMARY =====\n\n")
two_way <- experiment %>%
  group_by(treatment, gender) %>%
  summarise(
    n = n(),
    mean_bp = mean(bp_reduction),
    sd_bp = sd(bp_reduction)
  )
print(two_way)

# ANOVA analysis
cat("\n===== ONE-WAY ANOVA =====\n\n")
model <- aov(bp_reduction ~ treatment, data = experiment)
anova_table <- summary(model)
print(anova_table)

# Create interaction plot
cat("\n===== INTERACTION PLOT =====\n\n")
# Calculate means for each combination
interaction_means <- experiment %>%
  group_by(treatment, gender) %>%
  summarise(mean = mean(bp_reduction))

print(interaction_means)

# Visualize (text-based representation)
cat("\nVisual representation:\n")
cat("Treatment vs Gender Effect:\n")
for (treatment in levels(experiment$treatment)) {
  cat("\n", treatment, ":\n")
  sub_data <- interaction_means[interaction_means$treatment == treatment, ]
  for (i in 1:nrow(sub_data)) {
    bar_width <- floor(sub_data$mean[i] / 2)
    cat("   ", sub_data$gender[i], ": ", 
        paste(rep("█", max(0, bar_width)), collapse = ""), 
        sprintf(" (%.1f)", sub_data$mean[i]), "\n")
  }
}
```

**Output:**
```
===== DRUG TRIAL DATA =====

   subject treatment bp_reduction gender
1        1   Placebo      3.408661   Male
```

**Comments:**
- Factors are essential for experimental design
- ANOVA requires factor variables
- Interaction effects are easy to analyze

## Best Practices and Common Pitfalls

### Best Practices

1. **Use ordered factors**: For ordinal data
2. **Set levels explicitly**: Control the order
3. **Check with is.factor()**: Verify factor type
4. **Use droplevels()**: Remove unused levels

### Common Pitfalls

1. **StringAsFactors warning**: Modern R recommends FALSE
2. **New levels**: Will cause errors if unexpected values appear
3. **Dropping levels**: Subsetting can drop levels

## Performance Considerations

- Factors are more memory-efficient than character
- `forcats` package has useful factor functions
- Use `gtools::reorder.factor()` for complex reordering

## Related Concepts and Further Reading

- `?factor`, `?levels`, `?droplevels`
- `forcats` package documentation
- R for Data Science - Factors

## Exercise Problems

1. **Exercise 1**: Create an ordered factor for days of the week.

2. **Exercise 2**: Convert a character vector to a factor.

3. **Exercise 3**: Add a new level to an existing factor.

4. **Exercise 4**: Use table() to count factor frequencies.

5. **Exercise 5**: Reorder factor levels by frequency.
