# 📦 Dependency Security

## 🎯 What You'll Learn

- Scanning for vulnerabilities
- Pinning dependencies
- Supply chain security

---

## pip audit

```bash
pip install pip-audit
pip-audit
```

---

## Safety

```bash
pip install safety
safety check
```

---

## Pinning Dependencies

```text
# requirements.txt with versions
requests==2.31.0
flask==3.0.0
numpy==1.26.0
```

---

## Hash Checking

```bash
pip install --require-hashes -r requirements.txt
```

---

## Supply Chain Attacks

- **Typosquatting**: Malicious package with similar name
- **Dependency confusion**: Internal package replaced by public
- **Maintainer takeover**: Attacker takes over abandoned package

### Prevention

- Use official package indexes
- Review dependencies before installing
- Use pip-audit/safety regularly

---

## ✅ Summary

- Use pip-audit or safety to check vulnerabilities
- Pin exact versions
- Review dependencies regularly

## 🔗 Further Reading

- [pip-audit](https://pypi.org/project/pip-audit/)
- [Safety](https://pyup.io/safety/)
