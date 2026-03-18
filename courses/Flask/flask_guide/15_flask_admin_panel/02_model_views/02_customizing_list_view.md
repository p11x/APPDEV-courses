<!-- FILE: 15_flask_admin_panel/02_model_views/02_customizing_list_view.md -->

## Overview

Customize the list view in Flask-Admin.

## Code Walkthrough

```python
# custom_list.py
from flask_admin.contrib.sqla import ModelView

class UserAdmin(ModelView):
    # Columns to display
    column_list = ("id", "username", "email", "created_at")
    
    # Searchable columns
    column_searchable_list = ("username", "email")
    
    # Filterable columns
    column_filters = ("created_at", "role")
    
    # Sortable columns
    column_sortable_list = ("username", "created_at")
```

## Next Steps

Continue to [03_customizing_form_view.md](03_customizing_form_view.md)
