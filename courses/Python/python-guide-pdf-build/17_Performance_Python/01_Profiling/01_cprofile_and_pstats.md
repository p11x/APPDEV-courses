# cProfile and pstats

## What You'll Learn

- Profiling with cProfile
- Analyzing profiling results
- Sorting by different metrics
- Interpreting profiling output

## Prerequisites

- Read [08_packaging_cli_tools.md](../../16_Automation_and_Scripting/03_Advanced_CLI/08_packaging_cli_tools.md) first

## Basic Profiling

```python
# profiling_example.py

import cProfile
import pstats
from io import StringIO


def slow_function():
    total = 0
    for i in range(10000):
        total += i ** 2
    return total


def main():
    for _ in range(100):
        slow_function()


# Profile the code
profiler = cProfile.Profile()
profiler.enable()

main()

profiler.disable()

# Print stats
stats = pstats.Stats(profiler)
stats.sort_stats("cumulative")
stats.print_stats(10)
```

## Command Line Profiling

```bash
# Profile from command line
python -m cProfile -s cumulative script.py
python -m cProfile -o profile.prof script.py
```

## Annotated Full Example

```python
# cprofile_demo.py
"""Complete demonstration of cProfile."""

import cProfile
import pstats
from io import StringIO


def calculate_sum(n: int) -> int:
    return sum(range(n))


def calculate_square_sum(n: int) -> int:
    return sum(i**2 for i in range(n))


def main():
    result1 = calculate_sum(1000000)
    result2 = calculate_square_sum(1000000)
    print(f"Results: {result1}, {result2}")


if __name__ == "__main__":
    profiler = cProfile.Profile()
    profiler.enable()
    
    main()
    
    profiler.disable()
    
    # Print to string
    s = StringIO()
    ps = pstats.Stats(profiler, stream=s).sort_stats("cumulative")
    ps.print_stats(10)
    print(s.getvalue())
```

## Summary

- Profiling with cProfile
- Analyzing profiling results
- Sorting by different metrics

## Next Steps

Continue to **[02_line_profiler.md](./02_line_profiler.md)**
