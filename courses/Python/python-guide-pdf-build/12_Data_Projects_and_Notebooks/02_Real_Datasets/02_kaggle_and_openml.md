# 📂 Kaggle and OpenML

## Kaggle Datasets

```python
# Download from kaggle.com
# 1. Create account
# 2. Get API key
# 3. pip install kaggle
# 4. kaggle datasets download -d <dataset-name>
```

## OpenML

```python
from sklearn.datasets import fetch_openml

# Fetch from OpenML
mnist = fetch_openml('mnist_784', version=1)
X, y = mnist.data, mnist.target
```

## Top Beginner Datasets

1. Titanic Survival
2. House Prices
3. Iris Flowers
4. Titanic - the classic ML dataset
5. COVID-19 data
