# Risk-Based Authentication and Continuous Authentication

## What You'll Learn

- Risk scoring for authentication decisions
- Adaptive authentication based on risk
- Continuous authentication patterns
- Behavioral analysis for auth

## Risk-Based Authentication

```javascript
class RiskBasedAuth {
    constructor(redis) {
        this.redis = redis;
        this.rules = [
            { check: (ctx) => ctx.isNewDevice, weight: 30, action: 'mfa' },
            { check: (ctx) => ctx.isNewLocation, weight: 25, action: 'mfa' },
            { check: (ctx) => ctx.failedAttempts > 3, weight: 40, action: 'captcha' },
            { check: (ctx) => ctx.isVpn, weight: 10, action: 'allow' },
            { check: (ctx) => ctx.isTor, weight: 50, action: 'block' },
            { check: (ctx) => ctx.timeOfDay === 'unusual', weight: 15, action: 'mfa' },
            { check: (ctx) => ctx.multipleFailedPasswords, weight: 35, action: 'captcha' },
        ];
    }

    async evaluate(context) {
        let totalRisk = 0;
        const triggeredRules = [];

        for (const rule of this.rules) {
            if (rule.check(context)) {
                totalRisk += rule.weight;
                triggeredRules.push(rule);
            }
        }

        const action = totalRisk > 60 ? 'block'
            : totalRisk > 30 ? 'mfa'
            : totalRisk > 15 ? 'captcha'
            : 'allow';

        return { totalRisk, action, triggeredRules };
    }

    async buildContext(userId, req) {
        return {
            userId,
            ip: req.ip,
            userAgent: req.headers['user-agent'],
            isNewDevice: await this.isNewDevice(userId, req),
            isNewLocation: await this.isNewLocation(userId, req.ip),
            failedAttempts: await this.getRecentFailures(req.ip),
            isVpn: await this.checkVpn(req.ip),
            isTor: await this.checkTor(req.ip),
            timeOfDay: this.getTimeCategory(),
            multipleFailedPasswords: await this.hasMultipleFailedPasswords(userId),
        };
    }

    getTimeCategory() {
        const hour = new Date().getHours();
        return (hour >= 2 && hour <= 5) ? 'unusual' : 'normal';
    }
}

// Usage
const riskEngine = new RiskBasedAuth(redis);

app.post('/auth/login', async (req, res) => {
    const { email, password } = req.body;
    const user = await User.findByEmail(email);

    if (!user || !await bcrypt.compare(password, user.passwordHash)) {
        await recordFailedAttempt(req.ip, email);
        return res.status(401).json({ error: 'Invalid credentials' });
    }

    const context = await riskEngine.buildContext(user.id, req);
    const risk = await riskEngine.evaluate(context);

    switch (risk.action) {
        case 'allow':
            res.json({ token: generateToken(user) });
            break;
        case 'captcha':
            res.json({
                requireCaptcha: true,
                captchaChallenge: generateCaptcha(),
                riskLevel: 'medium',
            });
            break;
        case 'mfa':
            res.json({
                requireMfa: true,
                mfaChallenge: generateMfaChallenge(user),
                riskLevel: 'high',
            });
            break;
        case 'block':
            res.status(403).json({
                error: 'Login blocked due to suspicious activity',
                riskLevel: 'critical',
            });
            break;
    }
});
```

## Continuous Authentication

```javascript
class ContinuousAuthEngine {
    constructor(redis) {
        this.redis = redis;
        this.trustScoreThreshold = 0.7;
    }

    async evaluateTrust(session, request) {
        const factors = {
            deviceMatch: await this.checkDevice(session, request),
            locationMatch: await this.checkLocation(session, request),
            behaviorPattern: await this.checkBehavior(session, request),
            timePattern: this.checkTimePattern(session),
        };

        const weights = {
            deviceMatch: 0.3,
            locationMatch: 0.25,
            behaviorPattern: 0.3,
            timePattern: 0.15,
        };

        const score = Object.entries(factors).reduce((sum, [key, value]) => {
            return sum + (value ? weights[key] : 0);
        }, 0);

        return {
            score,
            trusted: score >= this.trustScoreThreshold,
            factors,
            action: score < 0.3 ? 'reauth' : score < 0.7 ? 'step_up' : 'allow',
        };
    }

    async checkDevice(session, request) {
        const fingerprint = this.computeDeviceFingerprint(request);
        return session.deviceFingerprint === fingerprint;
    }

    async checkLocation(session, request) {
        const currentGeo = await this.geoLookup(request.ip);
        const sessionGeo = session.geo;

        if (!sessionGeo) return true;

        const distance = this.haversineDistance(currentGeo, sessionGeo);
        const timeDiff = Date.now() - session.lastActivity;

        // Impossible travel: >500km in <1 hour
        if (distance > 500 && timeDiff < 3600000) return false;
        return true;
    }

    computeDeviceFingerprint(req) {
        const { 'user-agent': ua, 'accept-language': lang } = req.headers;
        return createHash('sha256').update(`${ua}|${lang}`).digest('hex');
    }

    haversineDistance(coord1, coord2) {
        const R = 6371; // Earth radius in km
        const dLat = (coord2.lat - coord1.lat) * Math.PI / 180;
        const dLon = (coord2.lon - coord1.lon) * Math.PI / 180;
        const a = Math.sin(dLat/2)**2 +
            Math.cos(coord1.lat * Math.PI/180) *
            Math.cos(coord2.lat * Math.PI/180) *
            Math.sin(dLon/2)**2;
        return R * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
    }
}

// Middleware: evaluate trust on every request
function continuousAuthMiddleware(engine) {
    return async (req, res, next) => {
        if (!req.session?.userId) return next();

        const trust = await engine.evaluateTrust(req.session, req);

        if (trust.action === 'reauth') {
            req.session.destroy();
            return res.status(401).json({
                error: 'Session trust revoked',
                requireReauth: true,
            });
        }

        if (trust.action === 'step_up') {
            return res.status(403).json({
                error: 'Additional verification required',
                requireMfa: true,
            });
        }

        req.trustScore = trust.score;
        next();
    };
}
```

## Common Mistakes

- Not calibrating risk thresholds (too aggressive or too lax)
- Not explaining to users why they're blocked
- Blocking legitimate users on VPN
- Not updating risk models based on feedback

## Cross-References

- See [Modern Auth](../05-modern-authentication/01-passwordless-sso-apikeys.md) for passwordless
- See [Security](../06-authentication-security/02-csrf-session-protection.md) for hardening
- See [Monitoring](../10-authentication-monitoring/02-audit-trail.md) for threat detection
