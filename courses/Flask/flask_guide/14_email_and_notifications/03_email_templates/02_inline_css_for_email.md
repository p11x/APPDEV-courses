<!-- FILE: 14_email_and_notifications/03_email_templates/02_inline_css_for_email.md -->

## Overview

Inline CSS is required for email clients.

## Code Walkthrough

```python
# inline_css.py
# Use premailer or inlining tools
# pip install premailer

from premailer import transform

html_with_inline_css = transform("""
<html>
<style>
    h1 { color: blue; }
</style>
<body>
    <h1>Hello</h1>
</body>
</html>
""")
```

## Next Steps

Continue to [03_email_template_best_practices.md](03_email_template_best_practices.md)
