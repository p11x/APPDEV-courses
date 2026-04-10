# ♿ Accessibility Testing

## 📋 Overview

Accessibility testing ensures web applications are usable by people with disabilities, including visual, motor, and cognitive impairments.

---

## 🎯 Why Accessibility Matters

- **Legal Compliance**: ADA, WCAG, Section 508
- **Wider Audience**: 15%+ of population has disabilities
- **Better UX**: Benefits all users
- **SEO**: Improved search engine ranking

---

## 🎯 Testing Tools

### Automated Tools

```javascript
// axe-core - accessibility testing
const { axe, getViolations } = require('axe-core');

async function runAccessibilityTest(page) {
    await page.evaluate(() => {
        axe.run(document, (err, results) => {
            if (err) throw err;
            
            if (results.violations.length > 0) {
                console.log('Accessibility violations:');
                results.violations.forEach(v => {
                    console.log(`- ${v.id}: ${v.description}`);
                });
            }
        });
    });
}
```

### Manual Testing Checklist

```javascript
const accessibilityChecklist = {
    keyboard: [
        'All interactive elements focusable',
        'Focus order logical',
        'No keyboard traps',
        'All functions accessible via keyboard'
    ],
    visual: [
        'Color contrast ratio 4.5:1+',
        'Text resizable to 200%',
        'No content relies on color alone',
        'Focus indicators visible'
    ],
    screenReader: [
        'Images have alt text',
        'Forms have labels',
        'Headings in order',
        'ARIA roles correct'
    ]
};
```

---

## 🔗 Related Topics

- [14_DOM_Accessibility_Best_Practices.md](../09_DOM_MANIPULATION/14_DOM_Accessibility_Best_Practices.md)
- [02_Unit_Testing_Master_Class.md](./02_Unit_Testing_Master_Class.md)

---

**Next: [Continuous Integration](./24_Continuous_Integration.md)**