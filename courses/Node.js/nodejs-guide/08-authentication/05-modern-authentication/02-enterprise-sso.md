# Enterprise Authentication: SAML, LDAP, and Zero-Trust

## What You'll Learn

- SAML 2.0 SSO implementation
- LDAP/Active Directory integration
- Zero-trust authentication architecture
- Enterprise authentication patterns

## SAML 2.0 SSO Implementation

```javascript
import { SAML } from '@node-saml/node-saml';

const saml = new SAML({
    // Service Provider (your app) settings
    issuer: 'https://app.example.com/saml/metadata',
    callbackUrl: 'https://app.example.com/auth/saml/callback',
    
    // Identity Provider settings
    entryPoint: process.env.SAML_ENTRY_POINT,
    cert: process.env.SAML_IDP_CERT,
    
    // Optional
    identifierFormat: 'urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress',
    signatureAlgorithm: 'sha256',
    digestAlgorithm: 'sha256',
});

// SP-initiated SSO
app.get('/auth/saml/login', async (req, res) => {
    try {
        const loginUrl = await saml.getAuthorizeUrlAsync(
            null, // relayState
            null  // options
        );
        res.redirect(loginUrl);
    } catch (err) {
        res.status(500).json({ error: 'SAML login failed' });
    }
});

// Handle SAML response
app.post('/auth/saml/callback', async (req, res) => {
    try {
        const { profile, loggedOut } = await saml.validatePostResponseAsync(req.body);

        if (loggedOut) {
            req.session.destroy();
            return res.redirect('/');
        }

        // Find or create user from SAML profile
        let user = await User.findByEmail(profile.nameID);
        if (!user) {
            user = await User.create({
                email: profile.nameID,
                name: profile.displayName || profile.nameID,
                ssoProvider: 'saml',
                ssoId: profile.nameID,
            });
        }

        // Create session
        req.session.userId = user.id;
        res.redirect('/dashboard');
    } catch (err) {
        console.error('SAML validation error:', err);
        res.redirect('/login?error=saml_failed');
    }
});

// SP metadata endpoint (IdP needs this)
app.get('/auth/saml/metadata', (req, res) => {
    res.type('application/xml');
    res.send(saml.generateServiceProviderMetadata());
});
```

## LDAP/Active Directory Integration

```javascript
import ldap from 'ldapjs';

class LDAPAuthenticator {
    constructor(config) {
        this.url = config.url; // 'ldap://ad.company.com:389'
        this.bindDN = config.bindDN; // 'CN=svc_auth,OU=Service,DC=company,DC=com'
        this.bindPassword = config.bindPassword;
        this.baseDN = config.baseDN; // 'OU=Users,DC=company,DC=com'
        this.searchFilter = config.searchFilter || '(sAMAccountName={{username}})';
    }

    async authenticate(username, password) {
        const client = ldap.createClient({ url: this.url });

        try {
            // Step 1: Bind with service account
            await new Promise((resolve, reject) => {
                client.bind(this.bindDN, this.bindPassword, (err) => {
                    err ? reject(err) : resolve();
                });
            });

            // Step 2: Search for user
            const filter = this.searchFilter.replace('{{username}}', username);
            const user = await new Promise((resolve, reject) => {
                client.search(this.baseDN, {
                    filter,
                    scope: 'sub',
                    attributes: ['dn', 'cn', 'mail', 'memberOf'],
                }, (err, res) => {
                    if (err) return reject(err);

                    let userDN = null;
                    let attrs = {};

                    res.on('searchEntry', (entry) => {
                        userDN = entry.dn;
                        attrs = entry.pojo.attributes.reduce((acc, attr) => {
                            acc[attr.type] = attr.values.length === 1
                                ? attr.values[0]
                                : attr.values;
                            return acc;
                        }, {});
                    });

                    res.on('end', () => {
                        userDN ? resolve({ dn: userDN, ...attrs })
                            : reject(new Error('User not found'));
                    });

                    res.on('error', reject);
                });
            });

            // Step 3: Verify user's password
            await new Promise((resolve, reject) => {
                client.bind(user.dn, password, (err) => {
                    err ? reject(new Error('Invalid password')) : resolve();
                });
            });

            return {
                dn: user.dn,
                name: user.cn,
                email: user.mail,
                groups: Array.isArray(user.memberOf) ? user.memberOf : [user.memberOf],
            };
        } finally {
            client.unbind();
        }
    }
}

// Express middleware
const ldapAuth = new LDAPAuthenticator({
    url: process.env.LDAP_URL,
    bindDN: process.env.LDAP_BIND_DN,
    bindPassword: process.env.LDAP_BIND_PASSWORD,
    baseDN: process.env.LDAP_BASE_DN,
});

app.post('/auth/ldap', async (req, res) => {
    try {
        const profile = await ldapAuth.authenticate(
            req.body.username,
            req.body.password
        );

        let user = await User.findByEmail(profile.email);
        if (!user) {
            user = await User.create({
                email: profile.email,
                name: profile.name,
                ssoProvider: 'ldap',
            });
        }

        const token = generateToken(user);
        res.json({ token, user });
    } catch (err) {
        res.status(401).json({ error: 'Authentication failed' });
    }
});
```

## Zero-Trust Authentication

```javascript
// Zero-trust: verify every request, trust nothing
class ZeroTrustAuth {
    constructor(redis) {
        this.redis = redis;
    }

    async verify(req) {
        const checks = {
            token: await this.verifyToken(req),
            device: await this.verifyDevice(req),
            location: await this.verifyLocation(req),
            behavior: await this.checkBehavior(req),
        };

        const failed = Object.entries(checks)
            .filter(([, result]) => !result.passed);

        return {
            passed: failed.length === 0,
            checks,
            failed: failed.map(([name]) => name),
            riskScore: this.calculateRisk(checks),
        };
    }

    async verifyToken(req) {
        const token = req.headers.authorization?.replace('Bearer ', '');
        if (!token) return { passed: false, reason: 'No token' };

        try {
            const decoded = jwt.verify(token, process.env.JWT_SECRET);
            
            // Check if token is in denylist
            const denied = await this.redis.get(`deny:${decoded.jti}`);
            if (denied) return { passed: false, reason: 'Token revoked' };

            return { passed: true, claims: decoded };
        } catch (err) {
            return { passed: false, reason: err.message };
        }
    }

    async verifyDevice(req) {
        const fingerprint = req.headers['x-device-fingerprint'];
        if (!fingerprint) return { passed: true }; // Optional

        const known = await this.redis.sIsMember(
            `devices:${req.user?.id}`,
            fingerprint
        );
        return { passed: known, reason: known ? null : 'Unknown device' };
    }

    calculateRisk(checks) {
        let risk = 0;
        if (!checks.token.passed) risk += 50;
        if (!checks.device.passed) risk += 20;
        if (!checks.location.passed) risk += 20;
        if (!checks.behavior.passed) risk += 10;
        return risk;
    }
}
```

## Common Mistakes

- Not validating SAML signatures (forged assertions)
- Binding LDAP with user credentials (use service account)
- Not implementing device verification in zero-trust
- Hardcoding LDAP search filters (injection risk)

## Cross-References

- See [OAuth2](../04-oauth2-oidc/01-authorization-code-flow.md) for OAuth flows
- See [Modern Auth](./01-passwordless-sso-apikeys.md) for passwordless
- See [Security](../06-authentication-security/01-security-headers.md) for hardening

## Next Steps

Continue to [Security: CSRF and Session Protection](../06-authentication-security/02-csrf-session-protection.md).
