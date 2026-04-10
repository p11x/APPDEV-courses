# Feature Encoding Methods

## Learning Objectives

By the end of this chapter, students will be able to:
- Understand different categorical encoding methods
- Implement one-hot encoding for nominal categories
- Apply label encoding and ordinal encoding
- Use target encoding for high-cardinality features
- Choose appropriate encoding method for different algorithms
- Handle data leakage in encoding

## Theoretical Background

Machine learning algorithms typically require numerical input. Categorical encoding transforms categorical features into numerical representations. Choosing the right encoding method is crucial for model performance.

### Types of Categorical Variables

#### Nominal Categories
Categories with no inherent order:
- Colors (red, blue, green)
- Countries (USA, UK, France)
- Types (sedan, SUV, truck)

#### Ordinal Categories
Categories with meaningful order:
- Education level (high school, bachelor's, master's)
- Size (small, medium, large)
- Rating (poor, fair, good, excellent)

### Common Encoding Methods

#### One-Hot Encoding (Dummy Variables)

Creates binary columns for each category:
- Works well for nominal categories
- Can cause high dimensionality for many categories
- No information about ordering (intentional for nominal)

#### Label Encoding

Assigns integer to each category:
- Simple but implies ordinal relationship
- Works with tree-based algorithms
- Can mislead linear models

#### Ordinal Encoding

Uses integers preserving order:
- For ordinal categories
- Preserves meaningful relationships

#### Target Encoding

Replaces categories with mean of target:
- Handles high cardinality
- Can cause overfitting
- Requires careful regularization

## Step-by-Step Implementation

### Step 1: Install and Load Required Packages

```r
install.packages("caret")
install.packages("dplyr")
install.packages("fastDummies")

library(caret)
library(dplyr)
library(fastDummies)
```

### Step 2: Label Encoding

```r
# Create sample data with categorical features
set.seed(42)
n <- 100

sample_data <- data.frame(
  color = factor(sample(c("red", "blue", "green"), n, replace = TRUE)),
  size = factor(sample(c("small", "medium", "large"), n, replace = TRUE)),
  category = factor(sample(c("A", "B", "C", "D"), n, replace = TRUE)),
  value = rnorm(n)
)

# Method 1: Using base R
sample_data$color_code <- as.integer(sample_data$color)
print(head(sample_data[, c("color", "color_code")]))

# Method 2: Using dplyr
sample_data <- sample_data %>%
  mutate(color_label = as.integer(factor(color, levels = unique(color))))

# Check encoding
unique(sample_data[, c("color", "color_code")])
```

### Step 3: One-Hot Encoding

```r
# Using fastDummies package
sample_onehot <- dummy_cols(sample_data, 
                            select_columns = c("color", "category"),
                            remove_first_dummy = TRUE)

# View result
head(sample_onehot)

# Using caret
dummies <- dummyVars(~ color + size, data = sample_data)
onehot_encoded <- predict(dummies, sample_data)
head(onehot_encoded)

# Manual one-hot encoding
manual_onehot <- model.matrix(~ color - 1, data = sample_data)
head(manual_onehot)
```

### Step 4: Ordinal Encoding

```r
# Define order for ordinal variable
size_order <- c("small" = 1, "medium" = 2, "large" = 3)

# Apply ordinal encoding
sample_data$size_ordinal <- sapply(sample_data$size, function(x) {
  size_order[x]
})

# Verify
unique(sample_data[, c("size", "size_ordinal")])

# Using factor levels for ordinal encoding
sample_data$size_ordinal2 <- as.integer(factor(sample_data$size, 
                                                levels = c("small", "medium", "large")))
```

### Step 5: Target Encoding

```r
# Create sample data for target encoding
set.seed(123)
n <- 200

target_data <- data.frame(
  category = factor(sample(paste0("cat_", 1:20), n, replace = TRUE)),
  value1 = rnorm(n),
  value2 = rnorm(n),
  target = rbinom(n, 1, 0.5)
)

# Add relationship
target_data$target[target_data$category %in% paste0("cat_", 1:5)] <- 1

# Calculate target mean per category
category_means <- target_data %>%
  group_by(category) %>%
  summarise(target_mean = mean(target), .groups = "drop")

print(category_means)

# Apply target encoding
target_data_encoded <- target_data %>%
  left_join(category_means, by = "category")

head(target_data_encoded)

# With smoothing (regularization)
global_mean <- mean(target_data$target)

# Add count for smoothing
category_stats <- target_data %>%
  group_by(category) %>%
  summarise(
    count = n(),
    cat_mean = mean(target),
    .groups = "drop"
  ) %>%
  mutate(smoothed = (count * cat_mean + 10 * global_mean) / (count + 10))

target_data_encoded <- target_data %>%
  left_join(category_stats[, c("category", "smoothed")], by = "category")
```

### Step 6: Encoding with caret

```r
# Using caret's dummyVars for one-hot encoding
set.seed(42)

# Create recipe
dummy_recipe <- recipe(~ color + size + category, data = sample_data) %>%
  step_dummy(all_nominal())

# Prepare recipe
prepped <- prep(dummy_recipe)

# Bake (transform)
encoded_data <- bake(prepped, new_data = NULL)
print(encoded_data)

# Using step_encoding for ordinal
ordinal_recipe <- recipe(~ size + value, data = sample_data) %>%
  step_ordinal encode(size, mapping = c(small = 1, medium = 2, large = 3))
```

## Code Examples

### Example 1: Encoding for Machine Learning

This example demonstrates proper encoding for ML models.

```r
# Create sample classification data
set.seed(456)
n <- 300

ml_data <- data.frame(
  color = factor(sample(c("red", "blue", "green", "yellow"), n, replace = TRUE)),
  size = factor(sample(c("S", "M", "L", "XL"), n, replace = TRUE)),
  material = factor(sample(c("cotton", "polyester", "wool"), n, replace = TRUE)),
  price = runif(n, 20, 200),
  quality_score = runif(n, 1, 10),
  sold = factor(sample(c("yes", "no"), n, prob = c(0.6, 0.4), replace = TRUE))
)

# Split
train_idx <- sample(1:n, n * 0.7)
train_df <- ml_data[train_idx, ]
test_df <- ml_data[-train_idx, ]

# Method: Create encoding using training data only
# One-hot encode nominal features
dummies <- dummyVars(sold ~ color + material, data = train_df)

train_encoded <- predict(dummies, train_df)
test_encoded <- predict(dummies, test_df)

# Combine with other features
train_final <- cbind(train_encoded, train_df[, c("size", "price", "quality_score")])
test_final <- cbind(test_encoded, test_df[, c("size", "price", "quality_score")])

# Convert size to ordinal
train_final$size_ord <- as.integer(factor(train_df$size, levels = c("S", "M", "L", "XL")))
test_final$size_ord <- as.integer(factor(test_df$size, levels = c("S", "M", "L", "XL")))

# Train model
model <- train(sold ~ ., data = train_final, method = "glm", family = "binomial")
predictions <- predict(model, test_final)
```

### Example 2: High Cardinality Encoding

This example shows target encoding for high-cardinality features.

```r
# Create high-cardinality data
set.seed(789)
n <- 500

high_card_data <- data.frame(
  product_id = factor(sample(paste0("prod_", 1:50), n, replace = TRUE)),
  user_id = factor(sample(paste0("user_", 1:100), n, replace = TRUE)),
  category = factor(sample(paste0("cat_", 1:30), n, replace = TRUE)),
  price = runif(n, 10, 500),
  purchased = factor(sample(c("yes", "no"), n, replace = TRUE))
)

# Add relationships
high_card_data$purchased[high_card_data$category %in% paste0("cat_", 1:10)] <- "yes"
high_card_data$purchased[high_card_data$price > 400] <- "yes"

# Split
train_idx <- sample(1:n, n * 0.7)
train_hc <- high_card_data[train_idx, ]
test_hc <- high_card_data[-train_idx, ]

# Target encoding for high-cardinality features
# Calculate means from training data only
category_means <- train_hc %>%
  group_by(category) %>%
  summarise(
    n = n(),
    purchase_rate = mean(as.numeric(purchased) - 1),
    .groups = "drop"
  )

# Smoothing parameter
global_rate <- mean(as.numeric(train_hc$purchased) - 1)
k <- 10  # smoothing parameter

category_means <- category_means %>%
  mutate(
    smoothed_rate = (n * purchase_rate + k * global_rate) / (n + k)
  )

# Apply to test data
test_hc <- test_hc %>%
  left_join(category_means[, c("category", "smoothed_rate")], by = "category") %>%
  mutate(smoothed_rate = ifelse(is.na(smoothed_rate), global_rate, smoothed_rate))

# Use for modeling
train_hc <- train_hc %>%
  left_join(category_means[, c("category", "smoothed_rate")], by = "category")
```

## Best Practices and Common Pitfalls

### Best Practices

1. **Never Encode Target**: Target encoding should use target, but carefully
2. **Use Training Statistics Only**: Calculate encodings from training data only
3. **Handle Unseen Categories**: Assign default value for test set categories not in training
4. **Check for Leakage**: Target encoding can cause leakage if not done properly
5. **Consider Algorithm**: Different algorithms handle encodings differently

### Common Pitfalls

1. **Data Leakage**: Using all data for encoding calculation
2. **Too Many Dummies**: Creating too many features for high-cardinality
3. **Ignoring Ordinal Order**: Not using ordinal encoding when appropriate
4. **Linear Model Issues**: Label encoding implies order to linear models

## Performance Considerations

### Method Selection

| Method | Use Case | Cardinality | Model Types |
|--------|----------|-------------|-------------|
| One-Hot | Nominal | Low | All |
| Label | Ordinal | Any | Tree-based |
| Target | High | High | Most |
| Embedding | Very High | Very High | Neural Networks |

### Computational Notes

- One-hot: O(categories) new columns
- Target encoding: O(1) new columns but careful computation
- Memory increases with cardinality for one-hot

## Related Concepts

- **Feature Scaling**: Often needed after encoding
- **Embedding**: Learned representations
- **Frequency Encoding**: Using counts instead of target
- **Hashing**: For extremely high cardinality

## Exercise Problems

1. **Basic**: Apply one-hot encoding to a categorical feature.

2. **Intermediate**: Implement target encoding with smoothing.

3. **Advanced**: Build pipeline with proper encoding to avoid leakage.

4. **Real-World Challenge**: Handle high-cardinality features in real data.

5. **Extension**: Compare different encoding methods for a model.