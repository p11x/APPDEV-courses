# Feature Flags

## 📌 What You'll Learn

- What feature flags are and why they're used
- Implementing feature flags in Express
- Gradual rollouts and percentage-based targeting
- Using feature flags for A/B testing

## 🧠 Concept Explained (Plain English)

**Feature flags** let you turn features on/off without deploying new code. Instead of shipping code that might cause issues, you wrap new features in flags that can be toggled instantly.

**Benefits:**
- Deploy any time, release when ready
- Instant rollback if issues arise
- Gradual rollouts to percentage of users
- A/B testing capabilities

**Common use cases:**
- Kill switches for problematic features
- Beta/alpha testing programs  
- Percentage rollouts
- A/B testing
- Legacy feature toggling

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// Feature flag store (use Redis in production)
const featureFlags = new Map([
  ['new-checkout', { enabled: true, rollout: 100 }],
  ['beta-dashboard', { enabled: true, rollout: 20 }],
  ['dark-mode', { enabled: true, rollout: 50 }],
  ['ai-recommendations', { enabled: false, rollout: 0 }]
]);

// User attributes for targeting
const userAttributes = new Map();


// ============================================
// Feature Flag Service
// ============================================

class FeatureFlagService {
  isEnabled(flag, userId = null) {
    const flagConfig = featureFlags.get(flag);
    if (!flagConfig || !flagConfig.enabled) {
      return false;
    }
    
    // No rollout percentage = 100%
    if (!flagConfig.rollout) {
      return true;
    }
    
    // If no user, use random
    if (!userId) {
      return Math.random() * 100 < flagConfig.rollout;
    }
    
    // Deterministic rollout based on user ID
    const hash = this.hash(userId + flag);
    return hash < flagConfig.rollout;
  }
  
  hash(str) {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      hash = ((hash << 5) - hash) + str.charCodeAt(i);
      hash |= 0;
    }
    return Math.abs(hash) % 100;
  }
  
  getAllFlags(userId = null) {
    const flags = {};
    for (const [key, config] of featureFlags) {
      flags[key] = this.isEnabled(key, userId);
    }
    return flags;
  }
}

const flags = new FeatureFlagService();


// ============================================
// Middleware to add flags to requests
// ============================================

app.use((req, res, next) => {
  const userId = req.headers['x-user-id'];
  
  // Attach flag checker to request
  req.features = {
    isEnabled: (flag) => flags.isEnabled(flag, userId),
    getAll: () => flags.getAllFlags(userId)
  };
  
  next();
});


// ============================================
// Routes
// ============================================

app.get('/health', (req, res) => res.json({ status: 'ok' }));

app.get('/api/features', (req, res) => {
  res.json(req.features.getAll());
});

// Feature-gated routes
app.get('/checkout/new', (req, res) => {
  if (!req.features.isEnabled('new-checkout')) {
    return res.redirect('/checkout/legacy');
  }
  res.json({ checkout: 'New checkout UI' });
});

app.get('/dashboard/beta', (req, res) => {
  if (!req.features.isEnabled('beta-dashboard')) {
    return res.status(403).json({ error: 'Not yet available' });
  }
  res.json({ dashboard: 'Beta dashboard' });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 10-15 | Flag store | Configuration for each feature |
| 27-47 | FeatureFlagService | Logic for checking flags and rollouts |
| 53-63 | Middleware | Attaches flag checker to requests |
| 70-85 | Routes | Using feature flags |

## ✅ Quick Recap

- Feature flags enable dynamic feature control
- Use rollout percentages for gradual rollouts
- Deterministic hashing ensures consistent user experience
- Flags can drive A/B testing

## 🔗 What's Next

This completes Section 12. Moving to [Section 13: Advanced API Patterns](./../../13_Advanced_API_Patterns/01_Alternative_Protocols/01_graphql-with-express.md).
