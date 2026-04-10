# Text Cleaning in R

## Learning Objectives

- Clean and preprocess text data
- Handle whitespace and case
- Remove punctuation and numbers
- Use regular expressions
- Normalize text

## Theory

Text cleaning is the first step in text analysis. Common tasks include: converting to lowercase, removing whitespace, removing punctuation, handling special characters, and normalization. The base R functions like tolower(), gsub(), and stringr package functions handle these tasks.

Regular expressions provide powerful pattern matching for text manipulation. Tools like quanteda and tm offer specialized text processing functions.

## Step-by-Step

1. Load or parse text data
2. Convert case consistently
3. Remove unwanted characters
4. Handle whitespace
5. Validate cleaning results

## Code Examples

### Basic Text Cleaning

```r
cat("===== BASIC CLEANING =====\n\n")

# Sample text
text <- "  This is a SAMPLE text! 123 \n with multiple spaces   "

cat("Original:\n")
cat("'", text, "'\n")

# Trim whitespace
text_trim <- trimws(text)
cat("\nTrimmed:\n")
cat("'", text_trim, "'\n")

# Lowercase
text_lower <- tolower(text_trim)
cat("\nLowercase:\n")
cat("'", text_lower, "'\n")

# Remove numbers
text_no_num <- gsub("[0-9]", "", text_lower)
cat("\nNo numbers:\n")
cat("'", text_no_num, "'\n")
```

### Removing Punctuation

```r
cat("\n===== PUNCTUATION =====\n\n")

text <- "Hello, World! How are you? I'm fine..."

# Base R approach
text_no_punct <- gsub("[[:punct:]]", "", text)
cat("No punctuation:\n")
cat("'", text_no_punct, "'\n")

# Using quantifier
text_clean <- gsub("[^[:alnum:][:space:]]", "", text)
cat("\nAlphanumeric only:\n")
cat("'", text_clean, "'\n")
```

### Regular Expressions

```r
cat("\n===== REGEX =====\n\n")

text <- "Contact: email@example.com or phone at 555-123-4567"

# Email
cat("Emails:", gsub(".*([a-z]+@[a-z.]+).*", "\\1", text), "\n")

# Phone numbers
cat("Phone:", gsub("[0-9]{3}-[0-9]{3}-[0-9]{4}", "XXX-XXX-XXXX", text), "\n")

# Multiple spaces
text_multi <- "This   has    many     spaces"
text_fixed <- gsub("\\s+", " ", text_multi)
cat("Fixed spaces:\n")
cat("'", text_fixed, "'\n")
```

### Using stringr

```r
cat("\n===== STRINGR =====\n\n")

library(stringr)

text <- "  The QUICK brown fox!  "

cat("str_trim:", str_trim(text), "\n")
cat("str_to_lower:", str_to_lower(text), "\n")
cat("str_remove numbers:", str_remove_all(text, "[0-9]"), "\n")
cat("str_count words:", str_count(text, "\\w+"), "\n")
cat("str_extract digits:", str_extract_all(text, "[0-9]+"), "\n")
```

### Multiple Text Cleaning

```r
cat("\n===== BATCH CLEANING =====\n\n")

# Clean multiple texts
texts <- c("  FIRST text!  ", "Second... TEXT", "third TEXT!!")

clean_text <- function(text) {
  text %>%
    trimws() %>%
    tolower() %>%
    gsub("[[:punct:]]", "", .) %>%
    gsub("\\s+", " ", .)
}

cat("Cleaned texts:\n")
for (t in texts) {
  cat("'", clean_text(t), "'\n")
}
```

## Real-World Example: Social Media Text

```r
# Real-world: Clean social media posts
cat("===== SOCIAL MEDIA CLEANING =====\n\n")

# Sample tweets
tweets <- c(
  "@user1 Great #product!!! http://example.com 555",
  "Love this!!! Sooo coool https://t.co/abc #AMAZING",
  "NOT good :-( http://link.com #bad"
)

# Clean tweet function
clean_tweet <- function(tweet) {
  tweet %>%
    tolower() %>%
    gsub("http[^ ]*", "", .) %>%  # remove URLs
    gsub("#[[:alnum:]]+", "", .) %>%  # remove hashtags
    gsub("@[[:alnum:]]+", "", .) %>%  # remove mentions
    gsub("[0-9]", "", .) %>%  # remove numbers
    gsub("[[:punct:]]", " ", .) %>%  # replace punct with space
    gsub("\\s+", " ", .) %>%  # collapse spaces
    trimws()
}

cat("Original:\n")
for (t in tweets) cat("'", t, "'\n")

cat("\nCleaned:\n")
for (t in tweets) cat("'", clean_tweet(t), "'\n")
```

## Best Practices

1. Validate cleaning at each step
2. Document all transformations
3. Preserve original text
4. Handle Unicode characters
5. Use consistent patterns
6. Test with sample data

## Exercises

1. Clean a corpus of text documents
2. Extract URLs from text
3. Remove stopwords
4. Handle accented characters
5. Create a cleaning pipeline