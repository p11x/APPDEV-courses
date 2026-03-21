# Profiling with Clinic.js

## 📌 What You'll Learn

- Using clinic.js doctor/flame/bubbleprof
- Reading flame graphs
- Finding bottlenecks

## 💻 Code Example

```bash
# Install clinic
npm install -g clinic

# Run with doctor
clinic doctor -- node server.js

# Run with flame
clinic flame -- node server.js

# Run with bubbleprof
clinic bubbleprof -- node server.js
```

## ✅ Quick Recap

- Use clinic.js for performance profiling
- doctor finds issues automatically
- flame shows where time is spent
- bubbleprof shows async operations
