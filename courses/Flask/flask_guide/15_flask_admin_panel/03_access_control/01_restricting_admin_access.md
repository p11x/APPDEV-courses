<!-- FILE: 15_flask_admin_panel/03_access_control/01_restricting_admin_access.md -->

## Overview

Restrict admin access to authorized users.

## Code Walkthrough

```python
# access_control.py
from flask_admin import Admin
from flask_admin.contrib.sqla import ModelView
from flask_login import current_user

class SecureModelView(ModelView):
    def is_accessible(self):
        return current_user.is_authenticated
    
    def inaccessible_callback(self, name, **kwargs):
        return redirect("/login")
```
