<!-- FILE: 15_flask_admin_panel/04_custom_admin_views/03_custom_actions_on_models.md -->

## Overview

Add custom actions to model views.

## Code Walkthrough

```python
# custom_actions.py
from flask_admin.contrib.sqla import ModelView

class UserAdmin(ModelView):
    # Custom action
    def activate_users(self, ids):
        for id in ids:
            user = self.session.query(User).get(id)
            user.active = True
        self.session.commit()
    
    # Register action
    activate_users.action = ("Activate Users", "Activate selected users")
```
