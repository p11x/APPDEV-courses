# itertools Module

## What You'll Learn

- itertools.chain, islice, product, combinations, permutations, groupby

## Prerequisites

- Read [01_map_filter_reduce.md](./01_map_filter_reduce.md) first

## Common itertools Functions

```python
# chain() - chain iterables together
from itertools import chain

result = list(chain([1, 2], [3, 4], [5]))
# [1, 2, 3, 4, 5]

# islice() - slice iterator
from itertools import islice

result = list(islice(range(10), 2, 8, 2))
# [2, 4, 6]

# product() - cartesian product
from itertools import product

result = list(product([1, 2], ['a', 'b']))
# [(1, 'a'), (1, 'b'), (2, 'a'), (2, 'b')]

# combinations() - combinations without replacement
from itertools import combinations

result = list(combinations([1, 2, 3], 2))
# [(1, 2), (1, 3), (2, 3)]

# permutations() - all orderings
from itertools import permutations

result = list(permutations([1, 2, 3], 2))
# [(1, 2), (1, 3), (2, 1), (2, 3), (3, 1), (3, 2)]

# groupby() - group consecutive items
from itertools import groupby

data = [(1, 'a'), (1, 'b'), (2, 'c')]
for key, group in groupby(data, lambda x: x[0]):
    print(key, list(group))
```

## Summary

itertools provides efficient iterator building blocks.

## Next Steps

Continue to **[03_functools.md](./03_functools.md)**
