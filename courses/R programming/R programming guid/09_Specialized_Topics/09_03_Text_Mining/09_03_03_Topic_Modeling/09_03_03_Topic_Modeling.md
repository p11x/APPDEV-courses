# Topic Modeling in R

## Title
Introduction to Topic Modeling with LDA and the topicmodels Package

## Objectives
- Understand the Latent Dirichlet Allocation (LDA) model
- Prepare text data for topic modeling
- Fit LDA models using the topicmodels package
- Interpret and visualize topic results

## Introduction

Topic modeling discovers abstract topics in a collection of documents. The most common method is Latent Dirichlet Allocation (LDA), which assumes documents are mixtures of topics.

## Installing Required Packages

```r
# Install packages
install.packages("topicmodels")
install.packages("tidytext")
install.packages("tidyverse")
install.packages("_lda")

# Load libraries
library(topicmodels)
library(tidytext)
library(tidyverse)
```

## Preparing Data for Topic Modeling

```r
# Load sample data (using text mining corpus)
library(tm)

# Create a sample corpus
documents <- c(
  "Politics and government policy decisions affect the economy",
  "The federal reserve sets interest rates and monetary policy",
  "Technology companies innovate and create new products",
  "Software programming and data science are growing fields",
  "Sports teams compete in games and win championships",
  "Basketball players score points and dunk the ball",
  "Health and medicine treat patients with diseases",
  "Doctors prescribe medicine and hospitals provide care",
  "Science and research discover new findings",
  "Physics and chemistry study matter and energy"
)

# Create corpus and preprocess
corpus <- VCorpus(VectorSource(documents))

# Preprocessing
corpus <- tm_map(corpus, content_transformer(tolower))
corpus <- tm_map(corpus, removePunctuation)
corpus <- tm_map(corpus, removeNumbers)
corpus <- tm_map(corpus, removeWords, stopwords("english"))
corpus <- tm_map(corpus, stripWhitespace)

# Create document-term matrix
dtm <- DocumentTermMatrix(corpus)
print(dtm)

# Remove sparse terms
dtm_clean <- removeSparseTerms(dtm, 0.8)
print(dtm_clean)
```

## Fitting LDA Models

```r
# Fit LDA model
# k = number of topics
set.seed(123)

lda_model <- LDA(dtm_clean, 
                k = 2,  # 2 topics
                control = list(seed = 123))

# View model
class(lda_model)
names(lda_model)

# Get topic-word distribution
topic_terms <- posterior(lda_model)$terms
print(dim(topic_terms))
print(topic_terms[, 1:5])

# Get document-topic distribution
doc_topics <- posterior(lda_model)$topics
print(doc_topics)

# Access topics using tidy() function
tidy(lda_model, matrix = "beta")
tidy(lda_model, matrix = "gamma")
```

## Exploring Topic Results

```r
# Get top terms per topic
tidy(lda_model, matrix = "beta") %>%
  group_by(topic) %>%
  top_n(5, beta) %>%
  ungroup() %>%
  arrange(topic, -beta)

# Beta: probability of term given topic
beta_terms <- tidy(lda_model, matrix = "beta")

# Find terms with highest beta difference between topics
beta_spread <- beta_terms %>%
  mutate(beta = log(beta)) %>%
  spread(topic, beta) %>%
  mutate(log_ratio = `2` - `1`)

top_terms <- beta_spread %>%
  top_n(10, abs(log_ratio)) %>%
  pull(term)

print(top_terms)

# Gamma: probability of topic given document
gamma_docs <- tidy(lda_model, matrix = "gamma")
print(gamma_docs)
```

## Fitting Multiple Topics

```r
# Try different numbers of topics
k_values <- c(2, 3, 4, 5)
perplexities <- numeric(length(k_values))

for (i in seq_along(k_values)) {
  model <- LDA(dtm_clean, k = k_values[i], 
              control = list(seed = 123))
  perplexities[i] <- perplexity(model, dtm_clean)
}

# Plot perplexity (lower is better)
plot(k_values, perplexities, type = "b",
     xlab = "Number of Topics",
     ylab = "Perplexity",
     main = "Model Selection: Perplexity vs Topics")

# Fit final model with best k
best_k <- k_values[which.min(perplexities)]
final_model <- LDA(dtm_clean, k = best_k, control = list(seed = 123))
```

## Tidyverse Integration

```r
# Using tidy workflow
# Get document-topic probabilities
doc_topics_tidy <- tidy(final_model, matrix = "gamma") %>%
  spread(topic, gamma)

print(doc_topics_tidy)

# Get topic-word probabilities
topic_words_tidy <- tidy(final_model, matrix = "beta")

# Top words visualization
topic_words_tidy %>%
  group_by(topic) %>%
  top_n(10, beta) %>%
  ungroup() %>%
  mutate(term = reorder(term, beta)) %>%
  ggplot(aes(term, beta, fill = factor(topic))) +
  geom_bar(stat = "identity", show.legend = FALSE) +
  facet_wrap(~ topic, scales = "free") +
  coord_flip()
```

## Alternative Topic Models

```r
# CTM (Correlated Topic Model)
ctm_model <- CTM(dtm_clean, k = 2, control = list(seed = 123))
print(ctm_model)

# Access results similarly
tidy(ctm_model, matrix = "beta")
tidy(ctm_model, matrix = "gamma")

# Using packages/LDA with different implementations
# For larger datasets, consider faster alternatives
library(fastTopics)
# fastTopics provides scalable topic modeling
```

## Real-World Example

```r
# Example: Analyze newspaper articles
# Create sample news documents
news_articles <- c(
  # Sports
  "The team won the championship final score was three to one",
  "Player scored thirty points in the basketball game",
  "The soccer match ended in a draw",
  # Technology
  "New smartphone released with improved processor",
  "Computer software update improves security",
  "Artificial intelligence advances machine learning",
  # Politics
  "Congress voted on new legislation today",
  "Election results show candidate winning",
  "Government announces new policy initiative"
)

# Process and create DTM
news_corpus <- VCorpus(VectorSource(news_articles))
news_corpus <- tm_map(news_corpus, content_transformer(tolower))
news_corpus <- tm_map(news_corpus, removePunctuation)
news_corpus <- tm_map(news_corpus, removeWords, stopwords("english"))
news_dtm <- DocumentTermMatrix(news_corpus)

# Fit 3-topic model
news_lda <- LDA(news_dtm, k = 3, control = list(seed = 456))

# Get document-topic assignments
tidy(news_lda, matrix = "gamma") %>%
  mutate(document = rep(1:9, each = 3)) %>%
  spread(topic, gamma)
```

## Summary

- LDA (Latent Dirichlet Allocation) discovers topics in documents
- Preprocess text: lowercase, remove stopwords, create DTM
- Use perplexity to select optimal number of topics
- Beta: term-topic probabilities
- Gamma: document-topic probabilities
- tidytext provides tidy workflow for topic modeling
- Higher perplexity indicates worse model fit
- topicmodels is the primary package for LDA in R