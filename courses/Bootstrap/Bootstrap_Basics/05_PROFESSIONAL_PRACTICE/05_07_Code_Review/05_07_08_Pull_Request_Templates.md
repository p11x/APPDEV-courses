---
title: "Pull Request Templates for Bootstrap Changes"
module: "Code Review"
difficulty: 1
estimated_time: 15
tags: ["pull-request", "templates", "workflow", "review"]
prerequisites: ["Git basics", "Bootstrap fundamentals"]
---

## Overview

Pull request templates standardize how Bootstrap changes are proposed, reviewed, and merged. A well-structured template ensures that reviewers have the context needed to evaluate changes efficiently. It enforces consistent documentation of what changed, why it changed, and how it was tested. Templates reduce back-and-forth communication and catch issues before code reaches the main branch.

## Basic Implementation

**Bootstrap PR Template Structure**

Create a `.github/pull_request_template.md` file with sections covering change description, Bootstrap-specific considerations, and testing evidence.

```markdown
## Summary
<!-- Brief description of the change -->

## Type of Change
- [ ] New Bootstrap component
- [ ] Modified existing component
- [ ] Bootstrap version update
- [ ] Custom SCSS changes
- [ ] Utility class modifications

## Bootstrap Details
**Components affected:** <!-- e.g., modal, card, navbar -->
**Bootstrap version:** <!-- e.g., 5.3.2 -->

## Screenshots
| Mobile (375px) | Tablet (768px) | Desktop (1440px) |
|----------------|----------------|-------------------|
| ![mobile]() | ![tablet]() | ![desktop]() |

## Accessibility Checklist
- [ ] ARIA attributes added/verified
- [ ] Keyboard navigation tested
- [ ] Color contrast meets WCAG AA
- [ ] Screen reader tested

## Testing
- [ ] Tested at all breakpoints
- [ ] Cross-browser tested (Chrome, Firefox, Safari)
- [ ] No Bootstrap console errors
```

**Component Change Template**

For changes to specific Bootstrap components, use a focused template.

```markdown
## Component: [Name]
**Change type:** New | Modified | Removed
**Reason:** <!-- Why this change is needed -->

## Before/After
<!-- Screenshots or code snippets showing the difference -->

## Impact
- [ ] No breaking changes to existing usage
- [ ] Documentation updated
- [ ] Migration guide provided (if breaking)

## Review Checklist
- [ ] Follows Bootstrap markup conventions
- [ ] Custom classes use project prefix
- [ ] Responsive at all breakpoints
- [ ] Accessibility preserved
```

**Bug Fix PR Template**

```markdown
## Bug Description
<!-- What was broken -->

## Root Cause
<!-- Why it was broken -->

## Fix Applied
<!-- What was changed to fix it -->

## Regression Prevention
- [ ] Test added to prevent recurrence
- [ ] Related components checked for same issue
```

## Advanced Variations

**Automated PR Checks Configuration**

Configure GitHub Actions to validate Bootstrap-specific requirements on every PR.

```yaml
# .github/workflows/pr-checks.yml
name: PR Checks
on: [pull_request]
jobs:
  bootstrap-lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: npm ci
      - run: npm run lint:css
      - run: npm run test:accessibility
      - run: npm run build:css
      - name: Check CSS size
        run: |
          SIZE=$(wc -c < dist/css/app.css)
          if [ $SIZE -gt 200000 ]; then
            echo "CSS bundle exceeds 200KB: $SIZE bytes"
            exit 1
          fi
```

**Review Assignment Rules**

Assign reviewers based on the type of Bootstrap change.

```yaml
# .github/CODEOWNERS
# Bootstrap component changes
/src/components/ @team/frontend-reviewers
/src/scss/ @team/design-system-team
/bootstrap.config.js @team/design-system-team
```

**PR Size Guidelines**

```markdown
## PR Size Guide
- **Small (< 100 lines):** Single component change, fast review
- **Medium (100-400 lines):** Multiple components, thorough review needed
- **Large (> 400 lines):** Consider splitting into smaller PRs

If your PR is large, explain why it cannot be split:
<!-- Reason for large PR size -->
```

## Best Practices

1. **Always include before/after screenshots** for visual changes
2. **List affected Bootstrap components** explicitly
3. **Specify the Bootstrap version** used for testing
4. **Include responsive screenshots** at mobile, tablet, and desktop widths
5. **Mark accessibility checklist items** as verified, not just checked
6. **Link to design specs** or Figma files when applicable
7. **Provide a testing URL** (Vercel/Netlify preview) for reviewers
8. **Describe the "why" not just the "what"** in the summary
9. **Tag appropriate reviewers** based on change type
10. **Reference related issues** using GitHub keywords (fixes #123)
11. **Keep PRs focused** - one logical change per PR
12. **Document any Bootstrap variable changes** with before/after values

## Common Pitfalls

1. **Skipping screenshots** - reviewers cannot assess visual changes without them
2. **Vague descriptions** - "fixed styling" provides no useful context
3. **Missing breakpoint testing** - changes look good on desktop only
4. **Not listing affected components** - reviewers don't know what to focus on
5. **Forgetting accessibility checks** - only testing visual appearance
6. **Large PRs without explanation** - 500+ line PRs overwhelm reviewers
7. **No link to design reference** - reviewers cannot validate against intended design
8. **Missing Bootstrap version** - changes may behave differently across versions
9. **Not testing in multiple browsers** - issues specific to Safari or Firefox go unnoticed
10. **Bypassing template sections** - leaving sections empty defeats the template's purpose

## Accessibility Considerations

Every PR template should include an accessibility section that is mandatory for UI changes. Reviewers must verify ARIA attributes, keyboard navigation, and screen reader compatibility. Include links to accessibility testing tools (axe DevTools, WAVE) in the template. Require at least one reviewer with accessibility knowledge for component-level changes.

## Responsive Behavior

PR templates should require screenshots at three minimum breakpoints: mobile (375px), tablet (768px), and desktop (1440px). For navigation or layout changes, include screenshots of the transition states. Document any breakpoint-specific behavior that reviewers should verify. Include a testing matrix checklist that confirms all Bootstrap breakpoints were tested.
