# OAuth 2.0 PKCE, Social Providers, and Token Introspection

## What You'll Learn

- PKCE flow for public clients (SPAs, mobile)
- Social provider integration (Google, GitHub)
- Token introspection implementation
- OAuth 2.0 error handling patterns

## PKCE Flow for Public Clients

```javascript
// Frontend: Generate PKCE challenge
import { createHash, randomBytes } from 'node:crypto';

function generatePKCE() {
    const verifier = randomBytes(32)
        .toString('base64url');
    const challenge = createHash('sha256')
        .update(verifier)
        .digest('base64url');
    return { verifier, challenge };
}

// Store verifier securely (memory, not localStorage)
const { verifier, challenge } = generatePKCE();
sessionStorage.setItem('pkce_verifier', verifier);

// Redirect to authorization server
const authUrl = new URL('https://auth.example.com/authorize');
authUrl.searchParams.set('response_type', 'code');
authUrl.searchParams.set('client_id', CLIENT_ID);
authUrl.searchParams.set('redirect_uri', REDIRECT_URI);
authUrl.searchParams.set('code_challenge', challenge);
authUrl.searchParams.set('code_challenge_method', 'S256');
authUrl.searchParams.set('scope', 'openid profile email');
authUrl.searchParams.set('state', generateState());

window.location.href = authUrl.toString();

// After redirect, exchange code with verifier
async function exchangeCode(code) {
    const verifier = sessionStorage.getItem('pkce_verifier');

    const response = await fetch('/auth/token', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            grant_type: 'authorization_code',
            code,
            client_id: CLIENT_ID,
            redirect_uri: REDIRECT_URI,
            code_verifier: verifier,
        }),
    });

    sessionStorage.removeItem('pkce_verifier');
    return response.json();
}
```

## Social Provider Integration

```javascript
// src/providers/google.js — Google OAuth2 provider
import passport from 'passport';
import { Strategy as GoogleStrategy } from 'passport-google-oauth20';

passport.use(new GoogleStrategy({
    clientID: process.env.GOOGLE_CLIENT_ID,
    clientSecret: process.env.GOOGLE_CLIENT_SECRET,
    callbackURL: '/auth/google/callback',
    scope: ['profile', 'email'],
}, async (accessToken, refreshToken, profile, done) => {
    try {
        // Find or create user
        let user = await User.findByProvider('google', profile.id);

        if (!user) {
            user = await User.create({
                email: profile.emails[0].value,
                name: profile.displayName,
                avatar: profile.photos[0]?.value,
                providers: {
                    google: {
                        id: profile.id,
                        accessToken,
                        refreshToken,
                    },
                },
            });
        } else {
            // Update tokens
            await User.updateProvider(user.id, 'google', {
                accessToken,
                refreshToken,
            });
        }

        done(null, user);
    } catch (err) {
        done(err);
    }
}));

// Routes
app.get('/auth/google',
    passport.authenticate('google', { scope: ['profile', 'email'] })
);

app.get('/auth/google/callback',
    passport.authenticate('google', { failureRedirect: '/login' }),
    (req, res) => {
        const token = generateToken(req.user);
        res.redirect(`/dashboard?token=${token}`);
    }
);
```

```javascript
// src/providers/github.js — GitHub OAuth2 provider
import { Strategy as GitHubStrategy } from 'passport-github2';

passport.use(new GitHubStrategy({
    clientID: process.env.GITHUB_CLIENT_ID,
    clientSecret: process.env.GITHUB_CLIENT_SECRET,
    callbackURL: '/auth/github/callback',
    scope: ['user:email'],
}, async (accessToken, refreshToken, profile, done) => {
    let user = await User.findByProvider('github', profile.id);

    if (!user) {
        user = await User.create({
            email: profile.emails[0].value,
            name: profile.displayName || profile.username,
            avatar: profile.photos[0]?.value,
            providers: { github: { id: profile.id, accessToken } },
        });
    }

    done(null, user);
}));
```

## Token Introspection (RFC 7662)

```javascript
// Resource server validates tokens with auth server
async function introspectToken(token) {
    const response = await fetch(
        process.env.INTROSPECTION_ENDPOINT,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': `Basic ${Buffer.from(
                    `${CLIENT_ID}:${CLIENT_SECRET}`
                ).toString('base64')}`,
            },
            body: new URLSearchParams({
                token,
                token_type_hint: 'access_token',
            }),
        }
    );

    return response.json();
    // { active: true, sub: "123", scope: "read write", exp: 1625000000 }
}

// Middleware using introspection
async function introspectMiddleware(req, res, next) {
    const token = req.headers.authorization?.replace('Bearer ', '');
    if (!token) return res.status(401).json({ error: 'No token' });

    const result = await introspectToken(token);
    if (!result.active) {
        return res.status(401).json({ error: 'Token inactive' });
    }

    req.tokenInfo = result;
    next();
}
```

## OAuth 2.0 Error Handling

```javascript
// Standard OAuth error responses
const oauthErrors = {
    invalid_request: {
        status: 400,
        message: 'Request is missing a required parameter',
    },
    invalid_client: {
        status: 401,
        message: 'Client authentication failed',
    },
    invalid_grant: {
        status: 400,
        message: 'Authorization code is invalid or expired',
    },
    unauthorized_client: {
        status: 403,
        message: 'Client is not authorized for this grant type',
    },
    unsupported_grant_type: {
        status: 400,
        message: 'Grant type is not supported',
    },
    invalid_scope: {
        status: 400,
        message: 'Requested scope is invalid',
    },
};

// Error handler middleware
function oauthErrorHandler(err, req, res, next) {
    if (err.oauthError) {
        const error = oauthErrors[err.oauthError] || oauthErrors.invalid_request;
        return res.status(error.status).json({
            error: err.oauthError,
            error_description: err.message || error.message,
        });
    }
    next(err);
}
```

## Common Mistakes

- Not validating state parameter (CSRF vulnerability)
- Not using PKCE for public clients
- Storing client secret in frontend code
- Not handling provider-specific error formats

## Cross-References

- See [Authorization Code Flow](./01-authorization-code-flow.md) for server-side flow
- See [JWT Refresh](../jwt/04-jwt-refresh-tokens.md) for token management
- See [Security](../06-authentication-security/01-security-headers.md) for hardening

## Next Steps

Continue to [Modern Auth: Enterprise SSO](../05-modern-authentication/02-enterprise-sso.md).
