# Sentiment Scoring in R

## Learning Objectives

- Score sentiment using lexicons
- Calculate sentiment dimensions
- Use different methods
- Aggregate scores

## Theory

Sentiment scoring calculates sentiment for text by matching words to lexicon scores. Methods include simple word counting, weighted scores (AFINN), and emotion categories (NRC). Results can be aggregated by document or compared across documents.

## Step-by-Guides

### Basic Scoring with AFINN

```r
library(tidytext)
library(dplyr)

# Tokenize and join with AFINN
text_df <- data.frame(
  text = c("I love this product!",
           "This is terrible and bad."),
  doc_id = 1:2
)

text_df |>
  unnest_tokens(word, text) |>
  inner_join(get_sentiments("afinn")) |>
  group_by(doc_id) |>
  summarize(score = sum(value))
```

### Using Bing

```r
# Positive/negative scoring
score_bing <- text_df |>
  unnest_tokens(word, text) |>
  inner_join(get_sentiments("bing")) |>
  count(doc_id, sentiment, wt = n()) |>
  pivot_wider(names_from = sentiment, 
             values_from = n,
             values_fill = 0) |>
  mutate(sentiment = positive - negative)

score_bing
```

## Code Examples

### NRC Emotion Scoring

```r
# Get emotions
emotions <- text_df |>
  unnest_tokens(word, text) |>
  inner_join(get_sentiments("nrc")) |>
  count(doc_id, sentiment) |>
  pivot_wider(names_from = sentiment,
             values_from = n,
             values_fill = 0)

emotions
```

### Proportion Scoring

```r
# Normalize by word count
text_df |>
  unnest_tokens(word, text) |>
  inner_join(get_sentiments("afinn")) |>
  group_by(doc_id) |>
  summarize(
    score = sum(value),
    words = n(),
    norm_score = score / words
  )
```

### Compare Methods

```r
# All three methods
text_df |>
  unnest_tokens(word, text) |>
  group_by(doc_id) |>
  summarize(
    afinn = sum((get_sentiments("afinn") |> 
                   rename(word = word, 
                         afinn = value))$afinn[match(word, 
                                                (get_sentiments("afinn") |> 
                                                    rename(word = word))$word)]),
    bing = n() - 2 * sum((get_sentiments("bing") |> 
                            rename(word = word, 
                                  bing = sentiment))$bing[match(word, 
                                                         (get_sentiments("bing") |> 
                                                             rename(word = word))$word)] == "negative"),
    nrc_positive = sum((get_sentiments("nrc") |> 
                       rename(word = word, 
                             nrc = sentiment))$nrc[match(word, 
                                                    (get_sentiments("nrc") |> 
                                                        rename(word = word))$word)] %in% 
                     c("positive", "joy", "trust", "anticipation"))
  )
```

## Best Practices

1. **Preprocess**: Clean text before scoring.

2. **Consider Context**: Account for negation.

3. **Normalize**: Use proportions for comparison.

4. **Aggregate**: Choose appropriate level.

5. **Validate**: Check on known examples.

## Exercises

1. Score with AFINN.

2. Score with Bing.

3. Score with NRC.

4. Compare methods.

5. Handle negation.

## Additional Resources

- [tidytext](https://www.tidytextmining.com/sentiment.html)
- [Sentiment Analysis in R](https://cran.r-project.org/web/packages/SentimentAnalysis/)
- [syuzhet](https://cran.r-project.org/web/packages/syuzhet/)