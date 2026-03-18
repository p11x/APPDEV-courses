<!-- FILE: 15_flask_admin_panel/05_file_management/01_file_admin_view.md -->

## Overview

Add file management to Flask-Admin.

## Installation

```bash
pip install flask-admin[extra]
```

## Code Walkthrough

```python
# file_admin.py
from flask_admin.contrib.fileadmin import FileAdmin
import os

# Add path admin
path = os.path.join(os.path.dirname(__file__), "static")
admin.add_view(FileAdmin(path, "/static/", name="Static Files"))
```
