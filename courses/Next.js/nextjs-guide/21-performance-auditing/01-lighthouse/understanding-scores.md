# Understanding Lighthouse Scores

## What You'll Learn
- Understand each Lighthouse metric
- Know what makes a good score
- Interpret the results

## Prerequisites
- Basic understanding of web performance

## Do I Need This Right Now?
Lighthouse scores help you understand how well your app performs. Knowing what each score means helps you prioritize improvements.

## Concept Explained Simply

Lighthouse is like a report card for your website. It grades you on different aspects: speed, accessibility, best practices, SEO. Each category gets a score from 0-100.

## The Six Categories

### 1. Performance (0-100)
Measures how fast your page loads and becomes interactive.

- **0-49:** Poor
- **50-89:** Needs Improvement
- **90-100:** Good

### 2. Accessibility (0-100)
How usable your site is for people with disabilities.

- **0-49:** Poor
- **50-89:** Needs Improvement
- **90-100:** Good

### 3. Best Practices (0-100)
Whether your site follows security and coding best practices.

- **0-49:** Poor
- **50-89:** Needs Improvement
- **90-100:** Good

### 4. SEO (0-100)
How well search engines can index your page.

- **0-49:** Poor
- **50-89:** Needs Improvement
- **90-100:** Good

### 5. Progressive Web App (0-100)
Whether your app can work offline and has PWA features.

### 6. Carbon Footprint (grams)
Environmental impact of your page's energy usage.

## Key Metrics Explained

### Largest Contentful Paint (LCP)
Time for the largest content to become visible.

| Time | Rating |
|------|--------|
| < 2.5s | Good |
| 2.5-4s | Needs Improvement |
| > 4s | Poor |

### First Input Delay (FID)
Time between user interaction and browser response.

| Time | Rating |
|------|--------|
| < 100ms | Good |
| 100-300ms | Needs Improvement |
| > 300ms | Poor |

### Cumulative Layout Shift (CLS)
How much the page layout shifts unexpectedly.

| Score | Rating |
|-------|--------|
| < 0.1 | Good |
| 0.1-0.25 | Needs Improvement |
| > 0.25 | Poor |

### Interaction to Next Paint (INP)
Time for the page to respond to user interactions.

| Time | Rating |
|------|--------|
| < 200ms | Good |
| 200-500ms | Needs Improvement |
| > 500ms | Poor |

## Running Lighthouse

### In Chrome DevTools

1. Open Chrome DevTools (F12)
2. Click the "Lighthouse" tab
3. Choose categories to audit
4. Click "Analyze page load"

### From Command Line

```bash
# Install lighthouse CLI
npm install -g lighthouse

# Run lighthouse
lighthouse https://your-site.com --output=html --output-path=report.html
```

### In CI/CD

```yaml
# .github/workflows/lighthouse.yml
name: Lighthouse

on: [push]

jobs:
  lighthouse:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
      - run: npm ci
      - run: npm run build
      - name: Run Lighthouse
        uses: foo-software/lighthouse-check-action@master
        with:
          urls: https://your-production-url.com
          budgetPath: ./lighthouse-budget.json
```

## Common Scores

| Metric | Good | Needs Work | Poor |
|--------|------|------------|------|
| Performance | 90+ | 50-89 | <50 |
| Accessibility | 90+ | 50-89 | <50 |
| Best Practices | 90+ | 50-89 | <50 |
| SEO | 90+ | 50-89 | <50 |

## Summary
- Lighthouse gives scores 0-100 for each category
- Performance is most important for user experience
- Key metrics: LCP, FID, CLS, INP
- Run in Chrome DevTools or CLI
- Integrate into CI/CD for continuous monitoring
- Aim for 90+ in all categories

## Next Steps
- [fixing-common-issues.md](./fixing-common-issues.md) — Fixing performance issues
