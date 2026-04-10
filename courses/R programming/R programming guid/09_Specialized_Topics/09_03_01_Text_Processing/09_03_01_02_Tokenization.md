# Tokenization in Text Processing

## Learning Objectives

- Understand tokenization concepts
- Use quanteda for tokenization
- Create different token types
- Apply token filters

## Theory

Tokenization splits text into smaller units (tokens). Common token types include words, sentences, and n-grams. The quanteda package provides comprehensive text processing capabilities. Tokenization is the first step in most text analysis workflows.

## Step-by-Step Guide

### Basic Tokenization

```r
library(quanteda)

# Create corpus
texts <- c("The quick brown fox.", 
             "A fox is quick!", 
             "Foxes are amazing.")

corp <- corpus(texts)

# Tokenize (word level)
tokens(corp, what = "word")

# Character level
tokens(corp, what = "character")
```

### Using tidytext

```r
library(tidytext)

# Tokenize by word
text_df <- data.frame(text = texts)

text_df |>
  unnest_tokens(word, text)

# By sentence
text_df |>
  unnest_tokens(sentence, text, token = "sentences")
```

## Code Examples

### N-grams

```r
library(quanteda)

# Bigrams
tokens_ngram <- tokens(corp, what = "word", ngrams = 2)

# Skip-grams
tokens_skip <- tokens(corp, what = "word", ngrams = 2, skip = 1)
```

### Custom Tokenization

```r
# Custom patterns
tokens_custom <- tokens(corp, 
                   what = "word",
                   remove_punct = TRUE,
                   remove_symbols = TRUE,
                   remove_numbers = TRUE,
                   remove_separators = TRUE)

# Remove stopwords
tokens_no_stop <- tokens_remove(tokens_custom, 
                          stopwords("en"))
```

### tidytext N-grams

```r
# Bigrams with tidytext
text_df |>
  unnest_tokens(word, text) |>
  count(word, sort = TRUE) |>
  filter(n > 1)
```

## Best Practices

1. **Preprocess First**: Clean text before tokenizing.

2. **Choose Token Type**: Match to analysis needs.

3. **Remove Noise**: Filter stopwords and punctuation.

4. **Consider N-grams**: Capture phrases when needed.

5. **Test Results**: Examine tokens after processing.

## Exercises

1. Tokenize sample text by words.

2. Create bigrams.

3. Remove stopwords.

4. Use tidytext.tokenizers.

5. Compare tokenization methods.

## Additional Resources

- [quanteda](https://quanteda.io/)
- [tidytext](https://cran.r-project.org/web/packages/tidytext/)
- [Text Mining with R](https://www.tidytextmining.com/)