<!-- FILE: 15_flask_admin_panel/03_access_control/02_role_based_permissions.md -->

## Overview

Implement role-based permissions in Flask-Admin.

## Code Walkthrough

```python
# role_permissions.py
from flask_admin.contrib.sqla import ModelView
from flask_login import current_user

class RoleAdmin(ModelView):
    def is_accessible(self):
        return current_user.is_authenticated and current_user.role == "admin"

class UserAdmin(ModelView):
    def is_accessible(self):
        return current_user.is_authenticated and current_user.role in ["admin", "moderator"]
```
