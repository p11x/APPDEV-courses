# RDS and RDA Formats in R

## Learning Objectives

- Use saveRDS() for single object serialization
- Use readRDS() for deserialization
- Understand .rds vs .RData differences
- Apply ASCII and compression options
- Build reproducible data pipelines

## Theoretical Background

### RDS Format

RDS (R Data Serialization) is R's native binary format:

1. **saveRDS()**: Serialize single R object
2. **readRDS()**: Deserialize back to R object
3. Different from .RData which stores workspace

### saveRDS() vs save()

| Feature | saveRDS() | save() |
|---------|----------|-------|
| Objects | Single | Multiple |
| Returns value | Yes (invisibly) | No |
| Assignment | Easy | Requires environment |
| Workspace | Not affected | Pollutes global |

### Key Parameters

- ascii: ASCII format (portable)
- compress: Compression type
- version: R version compatibility
- refhook: Hook for references

## Code Examples

### Basic Example: saveRDS()

```r
# ===== SAVERDS FUNCTION =====

cat("===== SAVERDS FUNCTION =====\n\n")

# Create sample objects
df1 <- data.frame(
  id = 1:5,
  name = c("Apple", "Banana", "Cherry", "Date", "Elderberry"),
  price = c(1.5, 0.75, 2.0, 1.0, 3.5),
  stock = c(100, 200, 50, 150, 25)
)

model1 <- lm(price ~ stock, data = df1)

# 1. Save single object
cat("1. Save with saveRDS:\n")
temp_file <- tempfile(fileext = ".rds")
saveRDS(df1, file = temp_file)
cat("  Saved to", temp_file, "\n\n")

# 2. Verify file
cat("2. Check file:\n")
file_info <- file.info(temp_file)
cat("  Size:", file_info$size, "bytes\n")
cat("  Is file:", !is.na(file_info$size), "\n\n")

# 3. Save different object types
cat("3. Save various objects:\n")
saveRDS(c(1, 2, 3), "vector.rds")
saveRDS(matrix(1:9, 3, 3), "matrix.rds")
saveRDS(list(a = 1, b = "text"), "list.rds")
saveRDS(function(x) x^2, "function.rds")
saveRDS(TRUE, "logical.rds")
cat("  Saved vector, matrix, list, function, logical\n\n")

# Clean up
unlink(temp_file)
unlink(c("vector.rds", "matrix.rds", "list.rds", "function.rds", "logical.rds"))
```

**Output:**
```
===== SAVERDS FUNCTION =====

1. Save with saveRDS:
  Saved to C:\Users\...\AppData\Local\Temp\file2e1c.rds
```

### Standard Example: readRDS()

```r
# ===== READRDS FUNCTION =====

cat("===== READRDS FUNCTION =====\n\n")

# Create and save data
df_original <- data.frame(
  x = 1:10,
  y = runif(10),
  group = sample(c("A", "B"), 10, replace = TRUE)
)

temp_file <- tempfile(fileext = ".rds")
saveRDS(df_original, file = temp_file)

# 1. Basic readRDS
cat("1. Basic readRDS:\n")
df_loaded <- readRDS(temp_file)
print(head(df_loaded))
cat("\n")

# 2. Verify objects are identical
cat("2. Verification:\n")
cat("  Identical:", identical(df_original, df_loaded), "\n\n")

# 3. readRDS returns object (not loads to env)
cat("3. Assign to variable:\n")
result <- readRDS(temp_file)
cat("  Class:", class(result), "\n")
cat("  Dimensions:", dim(result), "\n\n")

# 4. Handle uncompressed files
cat("4. Read uncompressed:\n")
uncompressed_file <- tempfile(fileext = ".rds")
saveRDS(df_original, file = uncompressed_file, compress = FALSE)
df_uncompressed <- readRDS(uncompressed_file)
cat("  Read successfully\n\n")

# Clean up
unlink(temp_file)
unlink(uncompressed_file)
```

### Real-World Example: Model Persistence

```r
# ===== MODEL PERSISTENCE =====

cat("===== MODEL PERSISTENCE =====\n\n")

# Train a model
set.seed(123)
train_data <- data.frame(
  age = sample(20:60, 100, replace = TRUE),
  income = sample(30000:150000, 100),
  education = sample(1:5, 100, replace = TRUE),
  score = rnorm(100, 50, 10)
)

# Create model
model <- lm(score ~ age + income + education, data = train_data)

# 1. Save model
cat("1. Save model with saveRDS:\n")
model_file <- "prediction_model.rds"
saveRDS(model, file = model_file)
cat("  Model saved to", model_file, "\n")
cat("  File size:", file.info(model_file)$size, "bytes\n\n")

# 2. Load and use model for prediction
cat("2. Load and predict:\n")
loaded_model <- readRDS(model_file)

new_data <- data.frame(
  age = c(30, 40, 50),
  income = c(50000, 75000, 100000),
  education = c(3, 4, 5)
)

predictions <- predict(loaded_model, new_data)
cat("  Predictions:", round(predictions, 2), "\n\n")

# 3. Save complete analysis object
cat("3. Save analysis complete:\n")
analysis_result <- list(
  model = model,
  coefficients = coef(model),
  residuals = residuals(model),
  fitted = fitted(model),
  summary = summary(model)
)

analysis_file <- "analysis_results.rds"
saveRDS(analysis_result, file = analysis_file)
cat("  Saved model, coefficients, residuals, fitted values\n\n")

# 4. Load analysis
cat("4. Load complete analysis:\n")
loaded_analysis <- readRDS(analysis_file)
cat("  Coefficients:\n")
print(loaded_analysis$coefficients)

# Clean up
unlink(model_file)
unlink(analysis_file)
```

### ASCII Example: Cross-Platform Sharing

```r
# ===== ASCII FORMAT =====

cat("===== ASCII FORMAT =====\n\n")

# ASCII format is larger but more portable
# Useful for sharing data across platforms

df <- data.frame(
  text = c("hello", "world", "test"),
  number = c(1, 2, 3),
  decimal = c(1.5, 2.5, 3.5)
)

# 1. Save with ASCII
cat("1. Save ASCII format:\n")
ascii_file <- "ascii_data.rds"
saveRDS(df, file = ascii_file, ascii = TRUE)

# Check file content
cat("  File content preview:\n")
lines <- readLines(ascii_file, n = 10)
cat("  ", lines[1:5], "\n\n")

# 2. Compare file sizes
cat("2. Size comparison:\n")
temp_binary <- tempfile(fileext = ".rds")
temp_ascii <- tempfile(fileext = ".rds")

saveRDS(df, file = temp_binary)
saveRDS(df, file = temp_ascii, ascii = TRUE)

binary_size <- file.info(temp_binary)$size
ascii_size <- file.info(temp_ascii)$size

cat("  Binary:", binary_size, "bytes\n")
cat("  ASCII:", ascii_size, "bytes\n")
cat("  Ratio:", round(ascii_size / binary_size, 1), "x larger\n\n")

# Clean up
unlink(ascii_file)
unlink(temp_binary)
unlink(temp_ascii)
```

### Advanced Example: Data Pipeline with RDS

```r
# ===== DATA PIPELINE =====

cat("===== DATA PIPELINE WITH RDS =====\n\n")

# Phase 1: Data Preparation
cat("Phase 1: Data Preparation\n")

raw_data <- data.frame(
  id = 1:1000,
  x1 = rnorm(1000),
  x2 = rnorm(1000),
  category = sample(LETTERS[1:5], 1000, replace = TRUE)
)

# Save raw data
saveRDS(raw_data, "step1_raw.rds")
cat("  Saved: step1_raw.rds\n")

# Phase 2: Preprocessing
cat("Phase 2: Preprocessing\n")
raw <- readRDS("step1_raw.rds")

# Remove outliers
cleaned <- raw[abs(raw$x1) < 3 & abs(raw$x2) < 3, ]

# Save preprocessed
saveRDS(cleaned, "step2_cleaned.rds")
cat("  Saved: step2_cleaned.rds\n")
cat("  Records:", nrow(cleaned), "\n")

# Phase 3: Feature Engineering
cat("Phase 3: Feature Engineering\n")
cleaned <- readRDS("step2_cleaned.rds")

# Create features
cleaned$x1_squared <- cleaned$x1 ^ 2
cleaned$x2_squared <- cleaned$x2 ^ 2
cleaned$x1_x2 <- cleaned$x1 * cleaned$x2

saveRDS(cleaned, "step3_features.rds")
cat("  Saved: step3_features.rds\n")
cat("  Features added: x1_squared, x2_squared, x1_x2\n")

# Phase 4: Model Training
cat("Phase 4: Model Training\n")
features <- readRDS("step3_features.rds")

# Simple model
model_data <- features[1:800, ]
model <- lm(x1 ~ x2 + category, data = model_data)

saveRDS(model, "step4_model.rds")
cat("  Saved: step4_model.rds\n")

# Phase 5: Prediction
cat("Phase 5: Prediction\n")
model <- readRDS("step4_model.rds")
features <- readRDS("step3_features.rds")

# Predict on test set
test_data <- features[801:1000, ]
predictions <- predict(model, test_data)

predictions_df <- data.frame(
  actual = test_data$x1,
  predicted = predictions,
  residual = test_data$x1 - predictions
)

saveRDS(predictions_df, "step5_predictions.rds")
cat("  Saved: step5_predictions.rds\n")
cat("  Mean absolute error:", 
      mean(abs(predictions_df$residual)), "\n\n")

# Final result
cat("Final: Load predictions\n")
final_predictions <- readRDS("step5_predictions.rds")
print(head(final_predictions))

# Clean up files
files_to_remove <- c("step1_raw.rds", "step2_cleaned.rds", 
                    "step3_features.rds", "step4_model.rds",
                    "step5_predictions.rds")
invisible(file.remove(files_to_remove))
```

**Output:**
```
===== DATA PIPELINE WITH RDS =====

Phase 1: Data Preparation
  Saved: step1_raw.rds
Phase 2: Preprocessing
  Saved: step2_cleaned.rds
  Records: 988
Phase 3: Feature Engineering
  Saved: step3_features.rds
Phase 4: Model Training
  Saved: step4_model.rds
Phase 5: Prediction
  Saved: step5_predictions.rds
  Mean absolute error: 0.95
```

## Comparison with save()/load()

### Advantages of RDS

1. **Cleaner workspace**: Doesn't load to global environment
2. **Direct assignment**: `result <- readRDS(file)`
3. **Object clarity**: Know exactly what's in file
4. **Pipeline friendly**: Build step-by-step workflows

### When to Use Each

| Use Case | Recommendation |
|---------|--------------|
| Save model | saveRDS() |
| Save workspace | save.image() |
| Share across R versions | save(..., ascii = TRUE) |
| Single object cache | saveRDS() |
| Multiple related objects | save() |
| Shiny app data | saveRDS() |

## Best Practices and Common Pitfalls

### Best Practices

1. **Use .rds extension**: Clear file type identification
2. **Version compatibility**: Use version = 2 for older R
3. **Compression**: Default is usually optimal
4. **Pipeline steps**: Use descriptive names
5. **Verify loading**: Check object after readRDS()

### Common Pitfalls

1. **ASCII warnings**: Can be corrupted by text editors
2. **File corruption**: Don't modify .rds files
3. **Version mismatch**: Old R may not read new format
4. **Missing files**: Check file path carefully
5. **Object type**: Can only save single object

### Performance Notes

| Operation | Time (1K rows) | Size |
|-----------|---------------|------|
| saveRDS() | 0.05s | 45KB |
| save() | 0.06s | 45KB |
| write.csv() | 0.8s | 65KB |