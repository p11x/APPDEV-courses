# 🐍 Python Programming Guide — Master Index

> From Absolute Beginner to Production-Ready Developer

**Python Version:** 3.12+ | **Total Folders:** 21 | **Total Files:** 200+

---

## 📍 How to Use This Guide

- Read folders in order for a structured learning path
- Jump to any folder if you already know earlier material
- Each file lists prerequisites so you can verify readiness
- Complete all "🚀 Challenge" sections — they cement learning

---

## 🗺️ Complete Learning Roadmap

### Phase 1 — Foundation (Folders 01–08) ~6 weeks

| Folder | Topic | What You'll Learn |
|--------|-------|-------------------|
| [01_Foundations](./01_Foundations/) | Variables, types, operators | Basic Python syntax, data types, operations |
| [02_Control_Flow](./02_Control_Flow/) | Conditionals, loops, exceptions | Program flow control, error handling |
| [03_Functions](./03_Functions/) | Functions, decorators, closures | Code organization, higher-order functions |
| [04_Data_Structures](./04_Data_Structures/) | Lists, dicts, dataclasses | Data organization, algorithms |
| [05_OOP](./05_OOP/) | Classes, inheritance, protocols | Object-oriented programming |
| [06_Modules_and_Packages](./06_Modules_and_Packages/) | Imports, packaging, pip | Code distribution, dependencies |
| [07_Advanced_Python](./07_Advanced_Python/) | Generators, async, typing | Advanced language features |
| [08_Projects_and_Practices](./08_Projects_and_Practices/) | Testing, projects | Real-world application building |

**After Phase 1:** You can write standalone Python scripts, understand OOP, and build basic applications.

### Phase 2 — Data Science & ML (Folders 09–12) ~6 weeks

| Folder | Topic | What You'll Learn |
|--------|-------|-------------------|
| [09_Data_Science_Foundations](./09_Data_Science_Foundations/) | NumPy, Pandas, Visualization | Data manipulation, analysis |
| [10_Machine_Learning](./10_Machine_Learning/) | ML algorithms, scikit-learn | Machine learning fundamentals |
| [11_Deep_Learning_Intro](./11_Deep_Learning_Intro/) | Neural networks, PyTorch | Deep learning basics |
| [12_Data_Projects_and_Notebooks](./12_Data_Projects_and_Notebooks/) | Jupyter, end-to-end projects | Real data science workflows |

**After Phase 2:** You can analyze datasets, build ML models, and create data visualizations.

### Phase 3 — Cutting-Edge & AI (Folders 13–16) ~6 weeks

| Folder | Topic | What You'll Learn |
|--------|-------|-------------------|
| [13_Cutting_Edge_Python](./13_Cutting_Edge_Python/) | Python 3.12/3.13, metaclasses | Latest Python features |
| [14_AI_and_LLM_Apps](./14_AI_and_LLM_Apps/) | Claude API, chatbots, RAG | AI application development |
| [15_Python_for_Platforms](./15_Python_for_Platforms/) | FastAPI, SQLAlchemy, Docker | Web services and deployment |
| [16_Automation_and_Scripting](./16_Automation_and_Scripting/) | File automation, Playwright | Workflow automation |

**After Phase 3:** You can build AI-powered apps, deploy web services, and automate workflows.

### Phase 4 — Professional Skills (Folders 17–21) ~5 weeks

| Folder | Topic | What You'll Learn |
|--------|-------|-------------------|
| [17_Performance_Python](./17_Performance_Python/) | Profiling, Numba, Cython | Code optimization |
| [18_Python_Security](./18_Python_Security/) | Secrets, encryption, secure coding | Security best practices |
| [19_Advanced_Web_Scraping](./19_Advanced_Web_Scraping/) | ETL, pipelines, scraping projects | Data collection at scale |
| [20_System_Design_and_Architecture](./20_System_Design_and_Architecture/) | SOLID, patterns, refactoring | Software architecture |
| [21_Interview_Prep_and_Open_Source](./21_Interview_Prep_and_Open_Source/) | Coding challenges, OSS contribution | Career preparation |

**After Phase 4:** You're a production-ready Python developer prepared for professional work and open source contributions.

---

## 📦 Master Dependencies List

Install all packages used across this guide:

### Phase 1: Stdlib Only
```bash
# No external packages needed!
```

### Phase 2: Data Science & ML
```bash
pip install numpy pandas matplotlib seaborn plotly scikit-learn torch torchvision
```

### Phase 3: Cutting-Edge & AI
```bash
pip install anthropic python-dotenv fastapi uvicorn sqlalchemy aiosqlite alembic \
         playwright httpx beautifulsoup4 rich typer click schedule tenacity \
         pydantic-settings feedparser
```

### Phase 4: Performance & Security
```bash
pip install pytest pytest-cov ruff black mypy \
         bcrypt argon2-cffi cryptography cython numba pip-audit
```

### Complete Install (All at Once)
```bash
pip install numpy pandas matplotlib seaborn plotly scikit-learn torch torchvision \
         anthropic python-dotenv fastapi uvicorn sqlalchemy aiosqlite alembic \
         playwright httpx beautifulsoup4 rich typer click schedule tenacity \
         pydantic-settings feedparser pytest pytest-cov ruff black mypy \
         bcrypt argon2-cffi cryptography cython numba pip-audit
```

---

## 🏆 Projects Built in This Guide

| # | Project | Folder | Tech Stack |
|---|---------|--------|------------|
| 1 | To-Do CLI App | 08 | argparse, dataclasses, Rich |
| 2 | Web Scraper | 16 | httpx, BeautifulSoup |
| 3 | Data Analyzer | 12 | Pandas, Matplotlib |
| 4 | REST API | 15 | FastAPI, Pydantic |
| 5 | Chatbot | 14 | Claude API, Rich |
| 6 | Price Tracker | 19 | SQLite, Rich, scheduling |
| 7 | News Aggregator | 19 | RSS, deduplication, categorization |
| 8 | Dataset Builder | 19 | Async scraping, Parquet |
| 9 | Finance Tracker | 20 | Hexagonal architecture |

---

## ⚡ Quick Reference Cards

### Python 3.12+ Syntax Cheat Sheet

```python
# Type hints (3.12+)
x: int = 5
y: list[int] = [1, 2, 3]

# F-strings (3.12+)
name = "Alice"
age = 30
print(f"{name} is {age}")

# Pattern matching (3.10+)
match value:
    case 1: print("one")
    case _: print("other")

# | Union types (3.10+)
def process(x: int | str) -> int | str: ...

# Structural pattern matching
match point:
    case (x, y): print(f"Point at {x}, {y}")

# Walrus operator
if (n := len(data)) > 10:
    print(f"Got {n} items")
```

### Big-O Complexity Table

| Operation | List | Dict | Set |
|-----------|------|------|-----|
| Access | O(1) | O(1) | N/A |
| Search | O(n) | O(1)* | O(1)* |
| Insert | O(1) | O(1) | O(1) |
| Delete | O(n) | O(1)* | O(1) |
| Sort | O(n log n) | N/A | N/A |

*Average case; O(n) worst case

### Common Design Patterns Decision Guide

| Situation | Use |
|-----------|------|
| Single instance needed | Singleton |
| Multiple similar objects | Factory |
| Complex construction | Builder |
| Swap algorithms at runtime | Strategy |
| Notify multiple objects | Observer |
| Wrap incompatible interface | Adapter |
| Simple interface over complex | Facade |

---

## 🔗 Essential External Resources

### Official Documentation
- [Python Docs](https://docs.python.org/3/) — Official language reference
- [Real Python](https://realpython.com/) — In-depth tutorials
- [PyPI](https://pypi.org/) — Package index

### Practice Platforms
- [LeetCode](https://leetcode.com/) — Coding challenges
- [HackerRank](https://www.hackerrank.com/) — Algorithm practice
- [Project Euler](https://projecteuler.net/) — Math/programming problems

### Community
- [r/learnpython](https://reddit.com/r/learnpython) — Beginner questions
- [Python Discord](https://discord.gg/python) — Real-time help
- [Stack Overflow](https://stackoverflow.com/questions/tagged/python) — Q&A

### News & Updates
- [Python Weekly](https://pythonweekly.com/) — Weekly newsletter
- [PyCoder's Weekly](https://pycoders.com/) — Articles and tutorials
- [Planet Python](https://planetpython.org/) — Blog aggregator

---

## 📚 Folder Structure Overview

```
python-guide/
├── 01_Foundations/          # Basics
├── 02_Control_Flow/         # Logic
├── 03_Functions/            # Functions
├── 04_Data_Structures/     # Data
├── 05_OOP/                  # OOP
├── 06_Modules_and_Packages/ # Distribution
├── 07_Advanced_Python/     # Advanced features
├── 08_Projects_and_Practices/ # Projects
├── 09_Data_Science_Foundations/ # NumPy/Pandas
├── 10_Machine_Learning/     # ML
├── 11_Deep_Learning_Intro/ # Neural networks
├── 12_Data_Projects_and_Notebooks/ # Data projects
├── 13_Cutting_Edge_Python/  # Python 3.12+
├── 14_AI_and_LLM_Apps/      # AI development
├── 15_Python_for_Platforms/ # Web & deployment
├── 16_Automation_and_Scripting/ # Automation
├── 17_Performance_Python/   # Optimization
├── 18_Python_Security/       # Security
├── 19_Advanced_Web_Scraping/ # Scraping
├── 20_System_Design_and_Architecture/ # Architecture
└── 21_Interview_Prep_and_Open_Source/ # Career
```

---

**Start your journey at [01_Foundations/01_Getting_Started/](./01_Foundations/01_Getting_Started/)**

*Happy coding! 🐍*
