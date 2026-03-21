#!/usr/bin/env python3
"""Add all missing subtopic folders to python-guide - Expanded."""

from pathlib import Path

BASE = Path("python-guide")

def make_md(title, objectives, prev_file, overview, sections, next_file=""):
    sections_md = ""
    for sec in sections:
        sections_md += f"\n## {sec['title']}\n{sec['explain']}\n\n```python\n{sec['code']}\n```\n"
    return f"""# {title}

## What You'll Learn
{objectives}

## Prerequisites
- Read {prev_file} first

## Overview
{overview}
{sections_md}
## Common Mistakes
- {sections[0]['mistake1']}
- {sections[0]['mistake2']}

## Summary
- {sections[0]['summary1']}
- {sections[0]['summary2']}
- {sections[0]['summary3']}

## Next Steps
Continue to **[{next_file}](./{next_file})**
"""

FOLDERS = {}

# 02_Control_Flow
FOLDERS["02_Control_Flow/04_Pattern_Matching"] = [
    ("01_match_statement.md", make_md("Match Statement", "- match/case syntax\n- Replace if/elif", "02_match_statements.md", "Pattern matching.", [{"title": "Syntax", "code": 'match x:\n    case 1: print("one")', "explain": "Basic", "mistake1": "no case", "mistake2": "wrong op", "summary1": "replaces switch", "summary2": "binds vars", "summary3": "use _"}], "02_matching_literals_and_types.md")),
    ("02_matching_literals_and_types.md", make_md("Literals and Types", "- Match literals\n- Match types", "01_match_statement.md", "Match values.", [{"title": "Literals", "code": 'case 1 | 2:', "explain": "OR pattern", "mistake1": "or vs |", "mistake2": "order", "summary1": "| combines", "summary2": "first wins", "summary3": "specific first"}])),
    ("03_matching_sequences.md", make_md("Matching Sequences", "- Destructure lists\n- Capture rest", "02_matching_literals_and_types.md", "Match lists/tuples.", [{"title": "Sequences", "code": 'case [a, b]:', "explain": "Unpack", "mistake1": "wrong brackets", "mistake2": "order", "summary1": "[] for lists", "summary2": "binds vars", "summary3": "rest with *"}])),
]

FOLDERS["02_Control_Flow/05_Iteration_Tools"] = [
    ("01_enumerate.md", make_md("Enumerate", "- index-value pairs", "02_Loops.md", "Add counters.", [{"title": "Basic", "code": 'enumerate(items)', "explain": "Wrap", "mistake1": "no unpack", "mistake2": "start", "summary1": "yields tuple", "summary2": "start=param", "summary3": "replaces counter"}])),
    ("02_zip_and_zip_longest.md", make_md("Zip", "- Pair iterables", "01_enumerate.md", "Combine.", [{"title": "Zip", "code": 'zip(a, b)', "explain": "Pair", "mistake1": "unequal len", "mistake2": "not list", "summary1": "truncates", "summary2": "zip_longest", "summary3": "* unpacks"}])),
    ("03_sorted_and_reversed.md", make_md("Sorted and Reversed", "- sorted() function", "02_zip_and_zip_longest.md", "Sorting.", [{"title": "Sorted", "code": 'sorted(x, key=len)', "explain": "Custom key", "mistake1": "vs .sort()", "mistake2": "reverse param", "summary1": "returns new list", "summary2": "key=func", "summary3": "reverse=True"}])),
    ("04_any_and_all.md", make_md("Any and All", "- any() and all()", "03_sorted_and_reversed.md", "Boolean checks.", [{"title": "Any", "code": 'any(x > 0 for x in lst)', "explain": "True if any", "mistake1": "empty returns False", "mistake2": "not generator", "summary1": "short-circuits", "summary2": "empty=False", "summary3": "use genex"}])),
    ("05_map_and_filter.md", make_md("Map and Filter", "- Transform with map\n- Select with filter", "04_any_and_all.md", "Apply functions.", [{"title": "Map", "code": 'list(map(str, items))', "explain": "Transform", "mistake1": "not list()", "mistake2": "lambda vs built-in", "summary1": "lazy iterator", "summary2": "builtin faster", "summary3": "comps preferred"}])),
    ("06_itertools_basics.md", make_md("Itertools Basics", "- chain, islice\n- cycle, repeat", "05_map_and_filter.md", "Efficient iterators.", [{"title": "Chain", "code": 'list(chain(a, b))', "explain": "Concat", "mistake1": "not imported", "mistake2": "lazy", "summary1": "connects iterables", "summary2": "islice slices", "summary3": "cycle repeats"}])),
]

FOLDERS["02_Control_Flow/06_Context_Managers"] = [
    ("01_with_statement.md", make_md("With Statement", "- Resource management", "03_exception_handling.md", "Cleanup guaranteed.", [{"title": "Basic", "code": 'with open(f) as x:', "explain": "Auto close", "mistake1": "after close", "mistake2": "no __exit__", "summary1": "enter/exit", "summary2": "safe on exception", "summary3": "commas stack"}])),
    ("02_builtin_context_managers.md", make_md("Built-in Managers", "- open(), Lock()\n- tempfile", "01_with_statement.md", "Standard lib.", [{"title": "Files", "code": 'with open(f) as f:', "explain": "Text/binary", "mistake1": "forget with", "mistake2": "mode", "summary1": "auto closes", "summary2": "threading.Lock", "summary3": "tempfile"}])),
    ("03_writing_context_managers.md", make_md("Writing Managers", "- __enter__, __exit__", "02_builtin_context_managers.md", "Custom context.", [{"title": "Protocol", "code": 'class CM:\n  def __enter__(self): return self\n  def __exit__(self,*a): pass', "explain": "Methods", "mistake1": "not returning", "mistake2": "not handling", "summary1": "enter returns", "summary2": "exit cleans", "summary3": "exception info"}])),
    ("04_contextlib_module.md", make_md("Contextlib", "- @contextmanager\n- suppress", "03_writing_context_managers.md", "Utilities.", [{"title": "Decorator", "code": '@contextmanager\ndef f():\n  yield', "explain": "Generator", "mistake1": "no yield", "mistake2": "not try/finally", "summary1": "yield value", "summary2": "suppress exc", "summary3": "nullcontext"}])),
    ("05_multiple_context_managers.md", make_md("Multiple Managers", "- Stacked syntax\n- ExitStack", "04_contextlib_module.md", "Multiple resources.", [{"title": "Stacked", "code": 'with a() as x, b() as y:', "explain": "Comma sep", "mistake1": "order", "mistake2": "reverse exit", "summary1": "comma separates", "summary2": "reverse order", "summary3": "ExitStack dynamic"}])),
]

FOLDERS["03_Functions/04_Function_Design"] = [
    ("01_single_responsibility.md", make_md("Single Responsibility", "- One purpose", "06_docstrings_and_annotations.md", "Do one thing.", [{"title": "SRP", "code": 'def send_email(addr):\n    validate(addr)', "explain": "One task", "mistake1": "too much", "mistake2": "naming", "summary1": "testable", "summary2": "reusable", "summary3": "clear name"}])),
    ("02_pure_functions.md", make_md("Pure Functions", "- No side effects\n- Deterministic", "01_single_responsibility.md", "Predictable.", [{"title": "Pure", "code": 'def add(a, b): return a+b', "explain": "No side effects", "mistake1": "modifying args", "mistake2": "IO inside", "summary1": "same input=output", "summary2": "testable", "summary3": "no state"}])),
    ("03_function_signatures.md", make_md("Function Signatures", "- Positional-only\n- Keyword-only", "02_pure_functions.md", "Parameter types.", [{"title": "Syntax", "code": 'def f(a, /, b, *, c): pass', "explain": "Special params", "mistake1": "wrong order", "mistake2": "not understanding", "summary1": "/ separates pos-only", "summary2": "* separates kw-only", "summary3": "combined"}])),
]

FOLDERS["03_Functions/05_Decorators_in_Depth"] = [
    ("01_decorator_anatomy.md", make_md("Decorator Anatomy", "- How decorators work\n- functools.wraps", "04_function_design.md", "Decorator pattern.", [{"title": "Basic", "code": '@decorator\ndef f(): pass\n\n# equivalent to:\nf = decorator(f)', "explain": "Wrapper", "mistake1": "not returning", "mistake2": "losing metadata", "summary1": "replaces function", "summary2": "wraps preserves", "summary3": "nested func"}])),
]

FOLDERS["04_Data_Structures/04_Comprehensions"] = [
    ("01_list_comprehensions.md", make_md("List Comprehensions", "- Create lists\n- Filter with if", "03_comprehensions.md", "Create lists.", [{"title": "LC", "code": '[x**2 for x in range(5)]', "explain": "Transform", "mistake1": "brackets", "mistake2": "complex", "summary1": "[expr for x]", "summary2": "readable", "summary3": "preferred over map"}])),
    ("02_dict_comprehensions.md", make_md("Dict Comprehensions", "- Create dicts", "01_list_comprehensions.md", "Dict from iter.", [{"title": "Dict", "code": '{k: v for k, v in items}', "explain": "Key-value", "mistake1": "wrong syntax", "mistake2": "not hashable", "summary1": "{k:v for x}", "summary2": "invert dict", "summary3": "conditional"}])),
    ("03_set_comprehensions.md", make_md("Set Comprehensions", "- Create sets", "02_dict_comprehensions.md", "Set from iter.", [{"title": "Set", "code": '{x for x in items}', "explain": "Unique", "mistake1": "vs dict", "mistake2": "hashable", "summary1": "{x for x}", "summary2": "deduplicate", "summary3": "set operations"}])),
]

FOLDERS["04_Data_Structures/05_Collections_Module"] = [
    ("01_counter.md", make_md("Counter", "- Frequency counting\n- most_common()", "04_comprehensions.md", "Counting.", [{"title": "Counter", "code": 'from collections import Counter\nc = Counter(items)', "explain": "Count", "mistake1": "not imported", "mistake2": "not iterable", "summary1": "counts items", "summary2": "most_common()", "summary3": "arithmetic"}])),
    ("02_defaultdict.md", make_md("Defaultdict", "- Auto-initialize\n- Missing keys", "01_counter.md", "Dict with defaults.", [{"title": "Defaultdict", "code": 'from collections import defaultdict\nd = defaultdict(list)', "explain": "Auto init", "mistake1": "not list", "mistake2": "factory", "summary1": "default value", "summary2": "no KeyError", "summary3": "grouping"}])),
]

FOLDERS["05_OOP/04_Magic_Methods"] = [
    ("01_str_and_repr.md", make_md("Str and Repr", "- __str__, __repr__\n- Different purposes", "05_dunder_methods.md", "String representation.", [{"title": "Methods", "code": 'def __str__(self): return "x"\ndef __repr__(self): return "X()" ', "explain": "Two methods", "mistake1": "same impl", "mistake2": "no repr", "summary1": "str for users", "summary2": "repr for devs", "summary3": "eval-able repr"}])),
]

FOLDERS["06_Modules_and_Packages/04_Virtual_Environments"] = [
    ("01_why_virtual_envs.md", make_md("Why Virtual Envs", "- Isolation\n- Dependency management", "03_main_guard.md", "Why use venvs.", [{"title": "Problem", "code": "# system: one python\n# venv: per-project", "explain": "Isolation", "mistake1": "system pip", "mistake2": "no venv", "summary1": "isolation", "summary2": "versions", "summary3": "reproducible"}])),
    ("02_venv_module.md", make_md("Venv Module", "- python -m venv", "01_why_virtual_envs.md", "Create venvs.", [{"title": "Create", "code": 'python -m venv myenv\nmyenv/Scripts/activate', "explain": "Creation", "mistake1": "wrong path", "mistake2": "activate", "summary1": "python -m venv", "summary2": "activate script", "summary3": "deactivate"}])),
]

FOLDERS["07_Advanced_Python/04_Memory_Management"] = [
    ("01_reference_counting.md", make_md("Reference Counting", "- CPython memory model\n- del", "03_typing_advanced.md", "How CPython manages memory.", [{"title": "Refcount", "code": 'import sys\nsys.getrefcount(x)', "explain": "Check count", "mistake1": "cycle refs", "mistake2": "ignoring", "summary1": "count to 0 = freed", "summary2": "del decreases", "summary3": "GC handles cycles"}])),
]

FOLDERS["08_Projects_and_Practices/02_Best_Practices"] = [
    ("01_code_style_pep8.md", make_md("PEP 8 Style", "- Naming\n- Layout\n- Max 79 chars", "03_mini_projects.md", "Style guide.", [{"title": "Naming", "code": 'my_var = 1\nclass MyClass: pass\nCONST = 1', "explain": "Conventions", "mistake1": "camelCase", "mistake2": "wrong case", "summary1": "snake_case vars", "summary2": "PascalCase classes", "summary3": "UPPER constants"}])),
    ("02_linting_with_ruff.md", make_md("Linting with Ruff", "- Install ruff\n- Run ruff check", "01_code_style_pep8.md", "Linting tool.", [{"title": "Ruff", "code": 'pip install ruff\nruff check .', "explain": "Lint", "mistake1": "not installing", "mistake2": "ignoring", "summary1": "fast linter", "summary2": "auto-fix", "summary3": "config in pyproject"}])),
]

FOLDERS["09_Data_Science_Foundations/01_NumPy"] = [
    ("01_arrays_and_dtypes.md", make_md("NumPy Arrays", "- Create arrays\n- dtypes\n- shapes", "04_seaborn_statistical_plots.md", "NumPy arrays.", [{"title": "Arrays", "code": 'import numpy as np\narr = np.array([1,2,3])', "explain": "Basic", "mistake1": "wrong dtype", "mistake2": "shape mismatch", "summary1": "np.array()", "summary2": "shape attribute", "summary3": "dtype specifies type"}])),
]

FOLDERS["10_Machine_Learning/04_Pipelines_and_Preprocessing"] = [
    ("01_sklearn_pipeline.md", make_md("SKLearn Pipeline", "- Chain steps\n- make_pipeline", "03_model_evaluation.md", "ML pipelines.", [{"title": "Pipeline", "code": 'from sklearn.pipeline import Pipeline\npipe = Pipeline([("scaler", StandardScaler()), ("clf", LogisticRegression())])', "explain": "Chain", "mistake1": "wrong order", "mistake2": "not using", "summary1": "list of steps", "summary2": "order matters", "summary3": "clean code"}])),
]

FOLDERS["14_AI_and_LLM_Apps/04_Vector_Databases_and_RAG"] = [
    ("01_embeddings_explained.md", make_md("Embeddings", "- Vector representation\n- Cosine similarity", "03_llm_app_patterns.md", "Text as vectors.", [{"title": "Embeddings", "code": "# cat -> [0.1, 0.5, ...]\n# kitten -> close to cat", "explain": "Semantic", "mistake1": "exact match", "mistake2": "dimensions", "summary1": "vectors represent meaning", "summary2": "similar = close", "summary3": "semantic search"}])),
]

FOLDERS["15_Python_for_Platforms/04_Message_Queues"] = [
    ("01_why_message_queues.md", make_md("Why Message Queues", "- Decouple\n- Async\n- Durability", "03_python_and_docker.md", "Why use queues.", [{"title": "Queues", "code": "# producer -> queue -> consumer", "explain": "Async", "mistake1": "direct calls", "mistake2": "no queue", "summary1": "decouples", "summary2": "scales", "summary3": "fault tolerant"}])),
]

FOLDERS["18_Python_Security/03_Cryptography"] = [
    ("01_hashing.md", make_md("Hashing", "- hashlib\n- bcrypt for passwords", "04_web_security.md", "One-way functions.", [{"title": "Hashlib", "code": 'import hashlib\nhashlib.sha256(b"data").hexdigest()', "explain": "Hash", "mistake1": "MD5 for secrets", "mistake2": "no salt", "summary1": "sha256 standard", "summary2": "one-way", "summary3": "fixed length"}])),
]

FOLDERS["20_System_Design_and_Architecture/01_Design_Principles"] = [
    ("01_solid_principles.md", make_md("SOLID", "- SRP, OCP, LSP\n- ISP, DIP", "04_case_studies.md", "Design principles.", [{"title": "SOLID", "code": "# S: one reason to change\n# O: open/closed", "explain": "Principles", "mistake1": "god classes", "mistake2": "not applying", "summary1": "single responsibility", "summary2": "open/closed", "summary3": "testable"}])),
]

FOLDERS["21_Interview_Prep_and_Open_Source/01_Python_Interview_Questions"] = [
    ("01_core_language_questions.md", make_md("Core Questions", "- GIL\n- Mutability\n- is vs ==", "04_career_and_growth.md", "Common Python questions.", [{"title": "GIL", "code": "# Only one thread runs at a time", "explain": "Global Interpreter Lock", "mistake1": "no parallelism", "mistake2": "ignoring IO", "summary1": "C Python only", "summary2": "threads for IO", "summary3": "processes for CPU"}])),
]

# Run
total_folders = 0
total_files = 0

for folder_rel, files in FOLDERS.items():
    folder = BASE / folder_rel
    folder.mkdir(parents=True, exist_ok=True)
    new_folder = True
    for filename, content in files:
        target = folder / filename
        if target.exists():
            print(f"Skipped: {folder_rel}/{filename}")
            continue
        target.write_text(content, encoding="utf-8")
        print(f"Created: {folder_rel}/{filename}")
        total_files += 1
        if new_folder:
            total_folders += 1
            new_folder = False

print(f"\nDone. {total_folders} new folders, {total_files} new files created.")
