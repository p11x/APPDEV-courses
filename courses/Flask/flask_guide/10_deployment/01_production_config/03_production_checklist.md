<!-- FILE: 10_deployment/01_production_config/03_production_checklist.md -->

## Overview

Deploying a Flask application to production requires careful preparation. This file provides a comprehensive checklist covering security, performance, monitoring, and reliability considerations for production deployment.

## Core Concepts

### Production Readiness

A production-ready Flask application must address:
- Security vulnerabilities
- Performance bottlenecks
- Error handling and logging
- Monitoring and alerting
- Backup and disaster recovery

## Code Walkthrough

### Production Checklist

#### ✅ Security

- [ ] **SECRET_KEY** is strong and from environment variable
- [ ] Debug mode is disabled (`DEBUG = False`)
- [ ] Use HTTPS everywhere (SSL/TLS certificates)
- [ ] Secure cookies: `SESSION_COOKIE_SECURE=True`, `SESSION_COOKIE_HTTPONLY=True`
- [ ] CSRF protection enabled (Flask-WTF or custom)
- [ ] Input validation and sanitization on all user inputs
- [ ] Output escaping in templates (Jinja2 auto-escape by default)
- [ ] Passwords hashed with bcrypt or similar (never store plain text)
- [ ] Rate limiting on authentication endpoints
- [ ] CORS properly configured (not overly permissive)
- [ ] Dependencies updated regularly (use `pip list --outdated`)
- [ ] No sensitive data in logs or error messages
- [ ] Use principle of least privilege for database users
- [ ] Regular security scanning (bandit, safety, etc.)

#### ✅ Performance

- [ ] Use production WSGI server (Gunicorn, uWSGI, not Flask dev server)
- [ ] Enable caching (Flask-Caching, Redis, Memcached)
- [ ] Optimize database queries (use indexes, avoid N+1 queries)
- [ ] Use connection pooling for database connections
- [ ] Compress responses (Gzip middleware)
- [ ] Serve static files efficiently (CDN or web server, not Flask)
- [ ] Minify and bundle CSS/JavaScript assets
- [ ] Use HTTP/2 if possible
- [ ] Implement pagination for large datasets
- [ ] Use asynchronous processing for long-running tasks (Celery, RQ)
- [ ] Monitor response times and throughput

#### ✅ Reliability

- [ ] Application runs as non-root user
- [ ] Proper process management (systemd, supervisord, Docker)
- [ ] Automatic restart on failure
- [ ] Health check endpoints (`/health` or `/ping`)
- [ ] Graceful shutdown handling
- [ ] Logging to external service or rotated files
- [ ] Error tracking (Sentry, Rollbar, etc.)
- [ ] Backup strategy for databases and user uploads
- [ ] Disaster recovery plan tested regularly
- [ ] Scaling strategy (horizontal/vertical)
- [ ] Load balancing configured
- [ ] Database connection limits configured appropriately

#### ✅ Monitoring & Observability

- [ ] Structured logging (JSON format)
- [ ] Key metrics collection (request rates, error rates, latency)
- [ ] Business metrics tracking (signups, conversions, etc.)
- [ ] Alerting on critical errors or performance degradation
- [ ] Application performance monitoring (APM) tools
- [ ] Database query monitoring
- [ ] External API call monitoring
- [ ] User session tracking (anonymized for privacy)
- [ ] Error rate thresholds and notifications
- [ ] Uptime monitoring

#### ✅ Code Quality

- [ ] Automated testing (unit, integration, end-to-end)
- [ ] Code review process for all changes
- [ ] Dependency vulnerability scanning
- [ ] Linting and formatting enforced (flake8, black, isort)
- [ ] Type checking (mypy) if using type hints
- [ ] Documentation kept up to date
- [ ] Dependency licenses reviewed
- [ ] No debug or test code in production builds

#### ✅ Deployment Process

- [ ] Automated deployment pipeline (CI/CD)
- [ ] Blue-green or canary deployment strategy
- [ ] Database migration strategy (backward compatible)
- [ ] Rollback procedure tested and documented
- [ ] Environment parity (dev/staging/prod similar)
- [ ] Configuration managed through environment variables
- [ ] Secrets managed through secure vault (AWS Secrets Manager, HashiCorp Vault, etc.)
- [ ] Deployment notifications (Slack, email, etc.)
- [ ] Post-deployment verification steps
- [ ] Capacity planning for traffic spikes

### Environment-Specific Configuration

```python
# config.py — Environment-specific settings
import os

class BaseConfig:
    """Base configuration."""
    SECRET_KEY = os.environ.get('SECRET_KEY') or 'dev-key-change-in-production'
    SQLALCHEMY_TRACK_MODIFICATIONS = False
    JSON_SORT_KEYS = False

class DevelopmentConfig(BaseConfig):
    """Development configuration."""
    DEBUG = True
    SQLALCHEMY_DATABASE_URI = 'sqlite:///dev.db'
    # Development-specific settings
    TEMPLATES_AUTO_RELOAD = True
    EXPLAIN_TEMPLATE_LOADING = False

class TestingConfig(BaseConfig):
    """Testing configuration."""
    TESTING = True
    SQLALCHEMY_DATABASE_URI = 'sqlite:///:memory:'
    WTF_CSRF_ENABLED = False  # Disable CSRF for testing
    BCRYPT_LOG_ROUNDS = 1  # Fast hashing for tests

class ProductionConfig(BaseConfig):
    """Production configuration."""
    DEBUG = False
    # Use environment variable for database
    SQLALCHEMY_DATABASE_URI = os.environ.get('DATABASE_URL')
    
    # Production security settings
    SESSION_COOKIE_SECURE = True
    SESSION_COOKIE_HTTPONLY = True
    SESSION_COOKIE_SAMESITE = 'Lax'
    PERMANENT_SESSION_LIFETIME = 3600  # 1 hour
    
    # Production performance settings
    SEND_FILE_MAX_AGE_DEFAULT = 31536000  # 1 year for static files
    # Enable compression in production (via web server or middleware)
    
    # Production logging
    LOG_TO_STDOUT = os.environ.get('LOG_TO_STDOUT')

def get_config():
    """Get configuration based on environment."""
    config_name = os.environ.get('FLASK_ENV', 'development')
    config_map = {
        'development': DevelopmentConfig,
        'testing': TestingConfig,
        'production': ProductionConfig
    }
    return config_map.get(config_name, DevelopmentConfig)
```

### Health Check Endpoint

```python
# app.py — Health check endpoint
from flask import Flask, jsonify
import psutil
import time

app = Flask(__name__)

@app.route('/health')
def health_check():
    """Health check endpoint for load balancers and monitoring."""
    checks = {
        'status': 'healthy',
        'timestamp': time.time(),
        'version': '1.0.0',
        'checks': {}
    }
    
    # Check database connectivity
    try:
        from app import db
        db.session.execute('SELECT 1')
        checks['checks']['database'] = {'status': 'healthy'}
    except Exception as e:
        checks['checks']['database'] = {
            'status': 'unhealthy',
            'error': str(e)
        }
        checks['status'] = 'unhealthy'
    
    # Check disk space
    disk_usage = psutil.disk_usage('/')
    if disk_usage.percent > 90:
        checks['checks']['disk'] = {
            'status': 'warning',
            'usage_percent': disk_usage.percent
        }
        if checks['status'] == 'healthy':
            checks['status'] = 'degraded'
    else:
        checks['checks']['disk'] = {'status': 'healthy'}
    
    # Check memory usage
    memory = psutil.virtual_memory()
    if memory.percent > 90:
        checks['checks']['memory'] = {
            'status': 'warning',
            'usage_percent': memory.percent
        }
        if checks['status'] == 'healthy':
            checks['status'] = 'degraded'
    else:
        checks['checks']['memory'] = {'status': 'healthy'}
    
    status_code = 200 if checks['status'] == 'healthy' else 503
    return jsonify(checks), status_code

if __name__ == '__main__':
    app.run()
```

## Common Mistakes

❌ **Using Flask development server in production**
```python
# WRONG — Never use this in production!
if __name__ == '__main__':
    app.run(debug=True)  # Dangerous in production
```

✅ **Correct — Use production WSGI server**
```bash
# CORRECT — Use Gunicorn
gunicorn --workers 4 --bind 0.0.0.0:8000 app:app

# Or uWSGI
uwsgi --socket 0.0.0.0:8000 --protocol=http -w app:app
```

❌ **Ignoring security headers**
```python
# WRONG — Missing important security protections
# No protection against XSS, clickjacking, etc.
```

✅ **Correct — Add security headers**
```python
# CORRECT — Using Flask-Talisman or similar
from flask_talisman import Talisman
talisman = Talisman(app)  # Adds security headers
```

❌ **Not setting up proper logging**
```python
# WRONG — No logs or logs to console only in production
# Makes debugging production issues nearly impossible
```

✅ **Correct — Configure production logging**
```python
# CORRECT
import logging
if not app.debug:
    # Log to file or external service
    file_handler = logging.FileHandler('app.log')
    file_handler.setLevel(logging.WARNING)
    app.logger.addHandler(file_handler)
```

## Quick Reference

| Area | Key Actions |
|------|-------------|
| **Security** | Strong secret, HTTPS, input validation, password hashing |
| **Performance** | Production WSGI, caching, CDN, database optimization |
| **Reliability** | Process management, health checks, backups, monitoring |
| **Deployment** | CI/CD, blue-green deployments, rollback procedures |
| **Monitoring** | Logging, metrics, alerting, APM tools |

## Next Steps

You have completed the Flask Web Application Development Guide! This concludes all 90 files covering Flask development from beginner to production deployment.

Remember to continuously learn and stay updated with Flask's evolving ecosystem. The best way to improve is by building real applications and applying these principles in practice.

Happy coding! 🚀