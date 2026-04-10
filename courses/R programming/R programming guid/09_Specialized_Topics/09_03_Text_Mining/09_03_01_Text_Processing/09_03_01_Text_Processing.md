# Text Processing in R

## Title
Text Mining Basics and Corpus Creation with the tm Package

## Objectives
- Create and manage text corpora using the tm package
- Perform text preprocessing operations
- Apply transformations like stemming and stopword removal
- Create document-term matrices for analysis

## Introduction

The tm package provides a comprehensive framework for text mining in R. It enables the creation and manipulation of text corpora, along with various preprocessing functions.

## Installing and Loading the tm Package

```r
# Install required packages
install.packages("tm")
install.packages("quanteda")

# Load library
library(tm)
```

## Creating a Corpus

A corpus is a collection of text documents in R.

```r
# Method 1: Create corpus from vectors
docs <- c(
  "This is the first document.",
  "The second document is longer.",
  "Document three is also quite interesting.",
  "This is document number four."
)

# Create corpus from character vector
corpus <- VCorpus(VectorSource(docs))

# Inspect corpus
corpus
summary(corpus)

# Access individual documents
inspect(corpus[[1]])
as.character(corpus[[1]])

# Method 2: Create corpus from data frame
df <- data.frame(
  id = 1:3,
  text = c("First document content",
           "Second document with different content",
           "Third document here"),
  author = c("John", "Jane", "Bob")
)
corpus_df <- VCorpus(DataframeSource(df))
inspect(corpus_df[[1]])
```

## Loading External Text Files

```r
# Create a temporary directory with sample files
temp_dir <- tempdir()
writeLines(c("Document one content here", 
             "Second document text", 
             "Third document more content"), 
           file.path(temp_dir, c("doc1.txt", "doc2.txt", "doc3.txt")))

# Load directory of text files
file_corpus <- VCorpus(DirSource(temp_dir))
summary(file_corpus)

# Clean up
unlink(temp_dir, recursive = TRUE)
```

## Text Transformations

```r
# Sample corpus for transformations
corpus <- VCorpus(VectorSource(c(
  "This is the FIRST document!!!",
  "The SECOND document contains numbers 12345",
  "Document THREE has UPPERCASE text"
)))

# Convert to lowercase
corpus <- tm_map(corpus, content_transformer(tolower))
as.character(corpus[[1]])

# Remove punctuation
corpus <- tm_map(corpus, removePunctuation)
as.character(corpus[[2]])

# Remove numbers
corpus <- tm_map(corpus, removeNumbers)
as.character(corpus[[2]])

# Remove whitespace
corpus <- tm_map(corpus, stripWhitespace)
as.character(corpus[[1]])
```

## Stopwords Removal

```r
# View built-in stopwords
stopwords("english")
length(stopwords("english"))

# Remove stopwords
corpus <- VCorpus(VectorSource(c(
  "This is a sample document with common words",
  "Another document with different words"
)))
corpus <- tm_map(corpus, content_transformer(tolower))
corpus <- tm_map(corpus, removeWords, stopwords("english"))
as.character(corpus[[1]])

# Add custom stopwords
custom_stopwords <- c("custom", "words")
corpus <- tm_map(corpus, removeWords, custom_stopwords)

# Remove specific words
corpus <- tm_map(corpus, removeWords, c("document", "text"))
```

## Stemming and Lemmatization

```r
# Stemming - reduce words to root form
corpus <- VCorpus(VectorSource(c(
  "running runners ran",
  "programming programs program"
)))

corpus <- tm_map(corpus, stemDocument)
as.character(corpus[[1]])

# Stem completion - convert stems back to words
corpus_complete <- tm_map(corpus, stemCompletion, 
                         dictionary = c("running", "programming"))
as.character(corpus_complete[[1]])
```

## Creating Document-Term Matrix

A document-term matrix represents the frequency of terms in documents.

```r
# Sample corpus
corpus <- VCorpus(VectorSource(c(
  "I love programming in R",
  "R is great for data science",
  "I love data and programming"
)))

# Create document-term matrix
dtm <- DocumentTermMatrix(corpus)
print(dtm)
inspect(dtm)

# Transpose (term-document matrix)
tdm <- TermDocumentMatrix(corpus)
inspect(tdm)

# Access specific elements
dtm[1, "programming"]
dtm[2, "r"]

# Term frequencies
colSums(as.matrix(dtm))
rowSums(as.matrix(dtm))
```

## Filtering and Operations on DTM

```r
# Remove sparse terms (terms that appear in few documents)
dtm2 <- removeSparseTerms(dtm, 0.5)
print(dtm2)

# Find frequent terms
findFreqTerms(dtm, 3)

# Find associations with specific terms
findAssocs(dtm, "love", 0.5)

# Weighting (TF-IDF)
dtm_tfidf <- DocumentTermMatrix(corpus, 
                                control = list(weighting = weightTfIdf))
inspect(dtm_tfidf)
```

## Working with Multiple Documents

```r
# Create corpus from multiple files with metadata
meta_data <- data.frame(
  id = c("doc1", "doc2", "doc3"),
  author = c("Smith", "Jones", "Brown"),
  date = as.Date(c("2020-01-01", "2020-01-02", "2020-01-03"))
)

texts <- c("First document text content",
           "Second document different text",
           "Third document more content")

# Create corpus with metadata
corpus <- VCorpus(VectorSource(texts))
meta(corpus) <- meta_data

# Access metadata
meta(corpus, "author")
meta(corpus)

# Get document identifiers
doc_id(corpus)
```

## Summary

- VCorpus and PCorpus are the main corpus classes in tm
- Text transformations include tolower, removePunctuation, removeNumbers
- removeWords removes stopwords and custom words
- stemDocument reduces words to their root form
- DocumentTermMatrix creates the fundamental text representation
- TF-IDF weighting helps identify important terms
- Sparse term removal helps reduce dimensionality