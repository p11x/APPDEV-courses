# Sentiment Lexicons in R

## Learning Objectives

- Understand sentiment lexicons
- Use AFINN lexicon
- Apply Bing and NRC lexicons
- Create custom lexicon

## Theory

Sentiment lexicons map words to sentiment scores or categories (positive/negative). Common lexicons include AFINN (scores -5 to 5), Bing (positive/negative), and NRC (emotion categories). The tidytext package provides access to these.

## Step-by-Step Guide

### Loading Lexicons

```r
library(tidytext)

# AFINN lexicon
afinn <- get_sentiments("afinn")

# Bing lexicon
bing <- get_sentiments("bing")

# NRC lexicon
nrc <- get_sentiments("nrc")
```

### Examining Lexicons

```r
# View AFINN scores
head(afinn)

# Words in Bing
head(bing)

# NRC emotions
unique(nrc$sentiment)
```

## Code Examples

### Custom Lexicon

```r
# Create custom lexicon
custom_lexicon <- data.frame(
  word = c("good", "great", "bad", "terrible"),
  sentiment = c("positive", "positive", 
               "negative", "negative"),
  score = c(3, 4, -3, -5)
)
```

### Loughran Lexicon

```r
# For business text
loughran <- get_sentiments("loughran")

# Check categories
unique(loughran$sentiment)
```

### Lexicon Comparison

```r
# Compare different lexicons
library(dplyr)

common_words <- c("good", "bad", "great", 
               "terrible", "excellent")

afinn |> filter(word %in% common_words)
bing |> filter(word %in% common_words)
nrc |> filter(word %in% common_words)
```

## Best Practices

1. **Match Lexicon**: Choose lexicon matching domain.

2. **Check Coverage**: See how many words match.

3. **Combine**: Use multiple lexicons for robustness.

4. **Validate**: Check on known examples.

5. **Consider Intensity**: Some lexicons have scores.

## Exercises

1. Load AFINN lexicon.

2. Load Bing lexicon.

3. Compare lexicons.

4. Create custom lexicon.

5. Validate on samples.

## Additional Resources

- [Sentiment Analysis](https://www.tidytextmining.com/sentiment.html)
- [AFINN](https://github.com/fnielsen/afinn)
- [NRC Emotion Lexicon](http://saifmohammad.com/WebPages/NRC-Emotion-Lexicon.htm)