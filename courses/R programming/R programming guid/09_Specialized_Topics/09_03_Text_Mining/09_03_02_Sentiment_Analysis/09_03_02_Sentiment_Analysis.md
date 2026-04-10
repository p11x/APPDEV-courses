# Sentiment Analysis in R

## Title
Sentiment Analysis Using syuzhet and tidytext Packages

## Objectives
- Understand sentiment analysis concepts and methods
- Perform sentiment scoring using dictionary-based approaches
- Implement lexicon-based sentiment analysis
- Visualize sentiment trends in text data

## Introduction

Sentiment analysis determines the emotional tone or opinion expressed in text. R provides several packages for sentiment analysis, including syuzhet, tidytext, and sentimentr.

## Installing Required Packages

```r
# Install packages
install.packages("syuzhet")
install.packages("tidytext")
install.packages("dplyr")
install.packages("ggplot2")

# Load libraries
library(syuzhet)
library(tidytext)
library(dplyr)
library(ggplot2)
```

## Basic Sentiment Analysis with syuzhet

```r
# Simple sentiment scoring using get_sentiment()
# Sample text
text <- c(
  "I love this product! It is amazing and wonderful.",
  "This is terrible. I hate it so much.",
  "The product is okay, nothing special.",
  "Great experience! Highly recommend!"
)

# Get sentiment using different methods
# Method 1: Syuzhet (default)
sentiment_syuzhet <- get_sentiment(text, method = "syuzhet")
print(sentiment_syuzhet)

# Method 2: Bing (binary positive/negative)
sentiment_bing <- get_sentiment(text, method = "bing")
print(sentiment_bing)

# Method 3: AFINN (scored -5 to +5)
sentiment_afinn <- get_sentiment(text, method = "afinn")
print(sentiment_afinn)

# Method 4: NRC (emotions)
sentiment_nrc <- get_sentiment(text, method = "nrc")
print(sentiment_nrc)
```

## Sentiment by Sentences

```r
# Get sentiment for each sentence in a paragraph
text_paragraph <- "The product is amazing! I love everything about it. 
However, the shipping was terrible. The package arrived damaged. 
But the customer service was excellent and resolved my issue. 
Overall, I am happy with my purchase."

# Get sentence-level sentiment
sentiment_by_sentence <- get_sentences(text_paragraph) %>%
  get_sentiment(method = "syuzhet")

print(sentiment_by_sentence)

# Plot sentiment trajectory
sentiment_df <- data.frame(
  sentence = 1:length(sentiment_by_sentence),
  sentiment = sentiment_by_sentence
)

ggplot(sentiment_df, aes(x = sentence, y = sentiment)) +
  geom_line() +
  geom_smooth() +
  geom_hline(yintercept = 0, linetype = "dashed", color = "gray") +
  labs(title = "Sentiment Trajectory",
       x = "Sentence",
       y = "Sentiment Score")
```

## tidytext Sentiment Analysis

```r
# Using tidytext with lexicons
# Create sample tidy text
sample_text <- data.frame(
  line = 1:3,
  text = c("I love happy people and joyful moments",
           "This makes me sad and angry",
           "It is a wonderful and beautiful day")
)

# Tokenize and get bing sentiment
sentiment_tidy <- sample_text %>%
  unnest_tokens(word, text) %>%
  inner_join(get_sentiments("bing"), by = "word")

print(sentiment_tidy)

# Count positive vs negative
sentiment_tidy %>%
  count(sentiment)

# Get sentiment score
sentiment_tidy %>%
  group_by(line) %>%
  summarise(sentiment_score = sum(ifelse(sentiment == "positive", 1, -1)))
```

## Using NRC Emotion Lexicon

```r
# Get emotions using NRC lexicon
text_emotions <- c(
  "I am so happy and excited about this!",
  "This makes me feel sad and angry.",
  "The weather is wonderful and peaceful today."
)

# Get emotion categories
emotions <- get_nrc_sentiment(text_emotions)
print(emotions)

# Visualize emotions
emotion_df <- data.frame(
  emotion = names(emotions),
  count = colSums(emotions)
)

ggplot(emotion_df, aes(x = reorder(emotion, count), y = count)) +
  geom_bar(stat = "identity", fill = "steelblue") +
  coord_flip() +
  labs(title = "Emotion Distribution",
       x = "Emotion",
       y = "Count")
```

## Sentiment Analysis on Larger Text

```r
# Example: Analyze movie reviews
# Create sample reviews
reviews <- data.frame(
  id = 1:10,
  text = c(
    "Absolutely loved this movie! Best I've ever seen.",
    "Terrible plot, boring characters, wasted time.",
    "It was okay, nothing special but not bad either.",
    "Amazing cinematography and great acting!",
    "Very disappointing. Expected much more.",
    "Great storytelling and wonderful performances.",
    "The movie was dull and too long.",
    "Enjoyable experience, would recommend to friends.",
    "Not impressed at all. Very poor quality.",
    "Fantastic! A masterpiece in every way."
  )
)

# Calculate sentiment scores
reviews$sentiment <- get_sentiment(reviews$text, method = "syuzhet")
reviews$afinn_score <- get_sentiment(reviews$text, method = "afinn")

# View results
print(reviews)

# Categorize sentiment
reviews$category <- case_when(
  reviews$afinn_score > 0 ~ "Positive",
  reviews$afinn_score < 0 ~ "Negative",
  TRUE ~ "Neutral"
)

# Count by category
reviews %>%
  count(category)
```

## Word-Level Sentiment Analysis

```r
# Analyze which words contribute to sentiment
text <- "I love this amazing product. It is fantastic and wonderful.
But the quality is poor and the service was terrible."

# Get sentiment by word
word_sentiment <- get_sentences(text) %>%
  get_sentiment(method = "syuzhet", 
                transform = TRUE) %>%
  get_words()

# Get words with sentiment scores
word_scores <- get_sentences(text) %>%
  get_sentiment(method = "syuzhet") %>%
  get_tokens()

# Find most positive and negative words
text_df <- data.frame(
  word = c("love", "amazing", "fantastic", "poor", "terrible"),
  sentiment = get_sentiment(c("love", "amazing", "fantastic", "poor", "terrible"))
)
print(word_scores)
```

## Visualizing Sentiment Patterns

```r
# Sentiment distribution
sentiment_values <- get_sentiment(reviews$text, method = "syuzhet")

ggplot(data.frame(score = sentiment_values), 
       aes(x = score)) +
  geom_histogram(binwidth = 0.5, fill = "coral") +
  geom_vline(xintercept = 0, color = "black", linetype = "dashed") +
  labs(title = "Distribution of Sentiment Scores",
       x = "Sentiment Score",
       y = "Frequency")

# Sentiment over text (word by word)
text_example <- "The movie started great but became boring. 
Then the ending was terrible and disappointing."

# Word-by-word sentiment
word_sent <- text_example %>%
  strsplit(split = " ") %>%
  unlist() %>%
  get_sentiment(method = "syuzhet")

plot(word_sent, type = "l", 
     xlab = "Word Position", 
     ylab = "Sentiment",
     main = "Sentiment Progression")
```

## Summary

- syuzhet provides multiple sentiment methods: syuzhet, bing, afinn, nrc
- get_sentiment() scores text on a spectrum
- NRC returns emotion categories (anger, joy, sadness, etc.)
- tidytext integrates with dplyr for tidy sentiment analysis
- Sentiment can be aggregated by sentence, paragraph, or document
- Visualize sentiment distribution and trajectory over text
- Choose appropriate lexicon based on your text domain