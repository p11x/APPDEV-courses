# 🎬 Movie Recommender System

## 🛠️ Setup

```python
pip install pandas scikit-learn rich
```

## Full Code

```python
import pandas as pd
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
from rich.console import Console
from rich.table import Table

# Sample movie data
movies: pd.DataFrame = pd.DataFrame({
    "title": [
        "The Matrix", "Inception", "Interstellar", "The Dark Knight",
        "Toy Story", "Finding Nemo", "Shrek", "The Lion King",
        "Iron Man", "Thor", "Guardians of the Galaxy"
    ],
    "genre": [
        "Sci-Fi Action", "Sci-Fi Action Adventure", "Sci-Fi Drama",
        "Action Drama", "Animation Comedy", "Animation Family",
        "Animation Comedy Fantasy", "Animation Family Drama",
        "Action Sci-Fi", "Action Sci-Fi Fantasy", "Action Sci-Fi Comedy"
    ],
    "description": [
        "A computer hacker learns about the nature of reality",
        "A thief enters dreams to steal secrets",
        "Astronauts travel through a wormhole",
        "A hero fights crime in Gotham",
        "A toy comes to life",
        "A fish searches for his son",
        "An ogre goes on an adventure",
        "A lion prince flees his kingdom",
        "A billionaire becomes a superhero",
        "A god wielding a hammer",
        "A group of misfits save the galaxy"
    ]
})

# Combine features
movies["features"] = movies["genre"] + " " + movies["description"]

# TF-IDF vectorization
vectorizer: TfidfVectorizer = TfidfVectorizer(stop_words="english")
tfidf_matrix = vectorizer.fit_transform(movies["features"])

# Cosine similarity
cosine_sim = cosine_similarity(tfidf_matrix, tfidf_matrix)

def get_recommendations(title: str) -> list[str]:
    """Get movie recommendations based on title."""
    # Find movie index
    idx = movies[movies["title"] == title].index[0]
    
    # Get similarity scores
    sim_scores = list(enumerate(cosine_sim[idx]))
    
    # Sort by similarity
    sim_scores = sorted(sim_scores, key=lambda x: x[1], reverse=True)
    
    # Get top 5 (excluding self)
    movie_indices = [i[0] for i in sim_scores[1:6]]
    return movies["title"].iloc[movie_indices].tolist()

# Display recommendations
console = Console()

table = Table(title="🎬 Movie Recommendations")
table.add_column("Movie", style="cyan")
table.add_column("Similar Movies", style="green")

for movie in ["The Matrix", "Toy Story", "Iron Man"]:
    recs = get_recommendations(movie)
    table.add_row(movie, ", ".join(recs))

console.print(table)
```

## Output

```
        Movie Recommendations
┌───────────────┬──────────────────────────────────┐
│ Movie         │ Similar Movies                   │
├───────────────┼──────────────────────────────────┤
│ The Matrix    │ Inception, Interstellar, ...    │
│ Toy Story     │ Finding Nemo, Shrek, ...       │
│ Iron Man      │ Thor, Guardians of the Galaxy  │
└───────────────┴──────────────────────────────────┘
```

## 🚀 Challenge

- Add user ratings data
- Use collaborative filtering
- Add content-based filtering with more features
