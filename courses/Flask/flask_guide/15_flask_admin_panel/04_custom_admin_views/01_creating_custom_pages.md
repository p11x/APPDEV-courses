<!-- FILE: 15_flask_admin_panel/04_custom_admin_views/01_creating_custom_pages.md -->

## Overview

Create custom admin pages beyond model views.

## Code Walkthrough

```python
# custom_pages.py
from flask_admin import AdminIndexView
from flask import render_template_string

class MyAdminIndexView(AdminIndexView):
    @expose("/")
    def index(self):
        return render_template_string("""
            <html>
            <body>
                <h1>Custom Dashboard</h1>
                <p>Welcome to the admin panel!</p>
            </body>
            </html>
        """)

admin = Admin(index_view=MyAdminIndexView())
```
