<!-- FILE: 15_flask_admin_panel/04_custom_admin_views/02_adding_dashboard_widgets.md -->

## Overview

Add dashboard widgets to your admin.

## Code Walkthrough

```python
# dashboard_widgets.py
from flask_admin import AdminIndexView
from flask import render_template

class DashboardView(AdminIndexView):
    def index(self):
        stats = {
            "users": get_user_count(),
            "orders": get_order_count(),
            "revenue": get_revenue()
        }
        return self.render("admin/dashboard.html", stats=stats)
```
