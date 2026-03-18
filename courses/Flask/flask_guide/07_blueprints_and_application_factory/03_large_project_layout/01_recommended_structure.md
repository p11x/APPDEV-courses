<!-- FILE: 07_blueprints_and_application_factory/03_large_project_layout/01_recommended_structure.md -->

## Overview

A proper project structure scales with your application. This file shows a recommended folder structure for medium-to-large Flask applications.

## Core Concepts

### Recommended Structure

```
myapp/
├── app/
│   ├── __init__.py       # Application factory
│   ├── models.py        # Database models
│   ├── routes/          # Blueprint routes
│   │   ├── __init__.py
│   │   ├── main.py
│   │   └── auth.py
│   ├── templates/       # Templates
│   └── static/          # Static files
├── config.py            # Configuration
├── requirements.txt     # Dependencies
└── run.py              # Entry point
```

## Next Steps

Now organize your project. Continue to [02_separating_concerns.md](02_separating_concerns.md) to learn separating concerns.