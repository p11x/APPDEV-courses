# Attribute-Based Access Control (ABAC)

## Overview

ABAC provides fine-grained access control based on attributes of the user, resource, action, and environment.

## Implementation

```python
# Example 1: ABAC implementation
from fastapi import FastAPI, Depends, HTTPException
from pydantic import BaseModel
from typing import Any, Dict

app = FastAPI()

class Policy(BaseModel):
    """Access control policy"""
    name: str
    effect: str  # "allow" or "deny"
    conditions: Dict[str, Any]

class ABACEngine:
    """Attribute-Based Access Control engine"""

    def __init__(self):
        self.policies: list[Policy] = []

    def add_policy(self, policy: Policy):
        self.policies.append(policy)

    def evaluate(
        self,
        subject: dict,  # User attributes
        resource: dict,  # Resource attributes
        action: str,     # Action being performed
        environment: dict = None  # Environment attributes
    ) -> bool:
        """Evaluate access based on attributes"""
        context = {
            "subject": subject,
            "resource": resource,
            "action": action,
            "environment": environment or {}
        }

        for policy in self.policies:
            if self._match_conditions(policy.conditions, context):
                return policy.effect == "allow"

        return False  # Deny by default

    def _match_conditions(self, conditions: dict, context: dict) -> bool:
        """Check if conditions match context"""
        for key, value in conditions.items():
            if key not in context:
                return False
            if context[key] != value:
                return False
        return True

abac = ABACEngine()

# Add policies
abac.add_policy(Policy(
    name="admin-full-access",
    effect="allow",
    conditions={"subject.role": "admin"}
))

abac.add_policy(Policy(
    name="owner-access",
    effect="allow",
    conditions={"subject.id": "resource.owner_id"}
))

def check_access(action: str, resource: dict):
    """Dependency to check access"""
    def checker(user: dict = Depends(get_current_user)):
        if not abac.evaluate(user, resource, action):
            raise HTTPException(403, "Access denied")
        return user
    return checker

@app.get("/documents/{doc_id}")
async def get_document(
    doc_id: int,
    user: dict = Depends(check_access("read", {"id": doc_id}))
):
    """Get document with ABAC check"""
    return {"doc_id": doc_id}
```

## Summary

ABAC provides flexible, fine-grained access control based on attributes.

## Next Steps

Continue learning about:
- [Permission Decorators](./04_permission_decorators.md)
- [Resource Ownership](./06_resource_ownership.md)
