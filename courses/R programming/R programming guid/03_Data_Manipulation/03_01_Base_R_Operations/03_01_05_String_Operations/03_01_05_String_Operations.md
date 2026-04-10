# String Operations in R

## Learning Objectives

- Perform string manipulation in base R
- Use string functions effectively
- Apply regular expressions
- Handle character data

## Theoretical Background

### String Functions in R

Base R provides comprehensive string operations through:

1. **paste()** - Concatenate strings
2. **substr()** - Extract substrings
3. **grep()** - Pattern matching
4. **gsub()** - Pattern replacement
5. **nchar()** - String length

## Code Examples

### Standard Example: String Functions

```r
# ===== STRING OPERATIONS =====

cat("===== BASIC STRING FUNCTIONS =====\n\n")

# paste() - concatenate with separator
first <- "Hello"
last <- "World"
cat("paste():", paste(first, last), "\n")
cat("paste0():", paste0(first, last), "\n")

# substr() - extract substring
text <- "Hello World"
cat("\nsubstr():\n")
cat("  text:", text, "\n")
cat("  substr(text, 1, 5):", substr(text, 1, 5), "\n")
cat("  substr(text, 7, 11):", substr(text, 7, 11), "\n")

# nchar() - string length
cat("\nnchar():\n")
cat("  nchar('Hello'):", nchar("Hello"), "\n")

# toupper() / tolower()
cat("\ntoupper() / tolower():\n")
cat("  toupper('Hello'):", toupper("Hello"), "\n")
cat("  tolower('HELLO'):", tolower("HELLO"), "\n")

# paste with collapse
cat("\npaste with collapse:\n")
words <- c("Hello", "World", "from", "R")
cat("  paste(words, collapse = ' '):", paste(words, collapse = " "), "\n")
```

**Output:**
```
===== BASIC STRING FUNCTIONS =====

paste(): Hello World
paste0(): HelloWorld

substr():
  text: Hello World
  substr(text, 1, 5): Hello
```

### Real-World Example: Name Processing

```r
# Real-world: Processing customer names
cat("===== CUSTOMER NAME PROCESSING =====\n\n")

names <- c("JOHN SMITH", "Mary Johnson", "robert brown", 
           "PATRICIA DAVIS", "Michael Wilson")

cat("Original names:\n")
print(names)

# 1. Standardize case
cat("\n1. Standardized to title case:\n")
standardized <- tolower(names)
standardized <- gsub("(^|[[:space:]])([[:alpha:]])", 
                    "\\1\\U\\2", standardized, perl = TRUE)
print(standardized)

# 2. Extract first and last names
cat("\n2. First and last names:\n")
parts <- strsplit(names, " ")
first_names <- sapply(parts, `[`, 1)
last_names <- sapply(parts, `[`, 2)
cat("  First names:", first_names, "\n")
cat("  Last names:", last_names, "\n")

# 3. Create email addresses
cat("\n3. Email addresses:\n")
emails <- paste0(tolower(first_names), ".", tolower(last_names), 
                "@company.com")
cat(" ", emails, "\n")

# 4. Search for patterns
cat("\n4. Find names with 'son':\n")
pattern <- "son"
matches <- grep(pattern, names, ignore.case = TRUE)
cat("  Names containing 'son':\n")
for (i in matches) {
  cat("   ", names[i], "\n")
}
```

## Best Practices and Common Pitfalls

### Best Patterns

1. Use paste0() for no-separator concatenation
2. Use gregexpr() for multiple matches
3. Use fixed = TRUE for literal matching

### Common Issues

1. Forgetting collapse in paste()
2. Not escaping special characters
3. String encoding issues
