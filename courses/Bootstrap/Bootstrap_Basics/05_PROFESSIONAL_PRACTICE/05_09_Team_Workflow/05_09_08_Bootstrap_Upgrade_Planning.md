---
title: "Bootstrap Version Upgrade Planning"
module: "Team Workflow"
difficulty: 2
estimated_time: 25
tags: ["upgrades", "migration", "planning", "versioning"]
prerequisites: ["Bootstrap versioning", "Semantic versioning"]
---

## Overview

Bootstrap version upgrades require careful planning to assess impact, identify breaking changes, and coordinate rollout across teams. A structured upgrade process minimizes disruption, ensures compatibility, and maintains quality standards. This guide covers the full upgrade lifecycle: impact assessment, compatibility testing, staged rollout, and validation.

## Basic Implementation

**Impact Assessment**

Before upgrading, catalog all Bootstrap usage in the project.

```bash
# Find all Bootstrap class references
rg "class=\"[^\"]*\b(col-|row|container|btn-|nav-|card|modal|alert|form-|d-|flex-|text-|bg-|p-|m-|g-)" src/ --count

# Find Bootstrap JS imports
rg "from ['\"]bootstrap['\"]" src/

# Find SCSS imports
rg "@import.*bootstrap" src/

# Check current version
npm list bootstrap
```

**Upgrade Assessment Document**

```markdown
## Bootstrap Upgrade: 5.2.x to 5.3.x

### Version Changes
- Current: 5.2.3
- Target: 5.3.3
- Release notes: https://github.com/twbs/bootstrap/releases

### Breaking Changes
| Change | Impact | Files Affected |
|--------|--------|---------------|
| CSS `gap` for grid gutters | Low - fallback exists | 12 grid templates |
| `data-bs-theme` attribute | None - additive feature | - |
| Deprecated `color-bg-subtle` | Medium - used in 8 files | 8 SCSS files |

### New Features to Adopt
- Dark mode via `data-bs-theme`
- CSS `gap` grid gutters
- New `text-bg-{color}` utilities

### Deprecated APIs to Remove
- `$enable-dark-mode` variable (replaced by color modes)
- Old focus ring variables
```

**Testing Plan**

```markdown
## Upgrade Testing Plan

### Phase 1: Build Verification
- [ ] Project compiles without errors
- [ ] No SCSS deprecation warnings
- [ ] CSS bundle size comparison (before/after)
- [ ] JavaScript tree-shaking still works

### Phase 2: Visual Regression
- [ ] Run visual regression tests at all breakpoints
- [ ] Manual review of 10 key pages
- [ ] Verify modal, dropdown, tooltip behavior
- [ ] Check form validation styling

### Phase 3: Accessibility
- [ ] Run axe-core audit on all primary pages
- [ ] Keyboard navigation verification
- [ ] Screen reader test on 3 key pages
- [ ] Focus indicator visibility check

### Phase 4: Cross-Browser
- [ ] Chrome latest
- [ ] Firefox latest
- [ ] Safari latest
- [ ] Edge latest
- [ ] Mobile Safari (iOS)
- [ ] Chrome (Android)
```

## Advanced Variations

**Staged Rollout Strategy**

```markdown
## Rollout Plan

### Stage 1: Development Branch (Week 1)
- Upgrade on feature branch
- Automated test suite passes
- Team lead reviews changes
- Fix any breaking changes

### Stage 2: Staging Environment (Week 2)
- Deploy to staging
- Full QA regression testing
- Visual comparison with production
- Performance benchmarking

### Stage 3: Limited Production (Week 3)
- Feature flag for new version on 10% of traffic
- Monitor error rates and performance
- Collect user feedback
- Rollback plan ready

### Stage 4: Full Production (Week 4)
- Remove feature flag
- Monitor for 48 hours
- Update documentation
- Close upgrade ticket
```

**Rollback Plan**

```markdown
## Rollback Procedure

### Triggers for Rollback
- Visual regression on >5% of pages
- Accessibility score drops below 90
- Performance degradation >20%
- Critical component functionality broken

### Rollback Steps
1. Revert package.json to previous Bootstrap version
2. Run `npm install`
3. Rebuild CSS and JS bundles
4. Deploy previous build artifacts
5. Verify production functionality
6. Document what went wrong
```

**Dependency Coordination**

```markdown
## Upgrade Dependencies

### Related Packages to Update
| Package | Current | Required | Notes |
|---------|---------|----------|-------|
| @popperjs/core | 2.11.6 | 2.11.8 | Required by Bootstrap 5.3 |
| bootstrap-icons | 1.10.0 | 1.11.3 | New icons available |
| @company/ui-components | 2.0.0 | 3.0.0 | Breaking changes |
| @company/theme | 1.5.0 | 2.0.0 | New color mode support |
```

## Best Practices

1. **Read the migration guide** thoroughly before starting the upgrade
2. **Assess impact across all codebases** - not just the primary application
3. **Create a dedicated upgrade branch** - isolate changes from feature work
4. **Run visual regression tests** comparing before and after
5. **Test at all breakpoints** - some changes only affect specific viewports
6. **Verify accessibility** after every upgrade - Bootstrap changes can affect ARIA behavior
7. **Stage the rollout** - dev, staging, then production
8. **Have a rollback plan** ready before deploying
9. **Document the upgrade process** for future reference
10. **Coordinate with all consuming teams** in shared component environments
11. **Update all documentation** to reflect new APIs and patterns
12. **Allocate adequate time** - upgrades always take longer than estimated

## Common Pitfalls

1. **Skipping impact assessment** - breaking changes discovered in production
2. **Not reading release notes** - missing deprecated features and new patterns
3. **Upgrading without testing** - visual regressions go unnoticed
4. **Ignoring peer dependency updates** - Popper.js and other deps fall out of sync
5. **No rollback plan** - stuck with broken production for hours
6. **Upgrading during feature development** - mixing upgrade and feature changes
7. **Not coordinating with other teams** - shared components break across repos
8. **Skipping visual regression testing** - pixel-level changes missed
9. **Forgetting documentation updates** - docs reference old APIs
10. **Underestimating time** - upgrades require more testing than expected

## Accessibility Considerations

Bootstrap upgrades can change ARIA attribute behavior, focus management, and keyboard interactions. Re-run full accessibility audits after every upgrade. Pay special attention to modal focus trapping, dropdown keyboard navigation, and tooltip/screen reader announcements. Compare accessibility test results before and after the upgrade.

## Responsive Behavior

Grid behavior changes are common in Bootstrap upgrades (e.g., CSS `gap` replacing padding-based gutters). Test all layouts at every breakpoint. Verify that column stacking behavior is unchanged. Check that responsive utilities (`d-none`, `d-md-block`) still work as expected. Test on real devices, not just browser DevTools.
