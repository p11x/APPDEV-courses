# OAuth2 Social Login

## What You'll Learn

- How OAuth2 works
- Implementing Google login with passport
- Handling OAuth callbacks
- Storing social login data

## OAuth2 Flow

```
1. User clicks "Login with Google"
2. App redirects to Google login page
3. User authenticates with Google
4. Google redirects back with authorization code
5. App exchanges code for access token
6. App uses token to get user profile
```

## Implementation

```bash
npm install passport passport-google-oauth20 express-session
```

```js
// auth/oauth.js — Google OAuth2 setup

import passport from 'passport';
import { Strategy as GoogleStrategy } from 'passport-google-oauth20';

passport.use(new GoogleStrategy({
  clientID: process.env.GOOGLE_CLIENT_ID,
  clientSecret: process.env.GOOGLE_CLIENT_SECRET,
  callbackURL: '/auth/google/callback',
}, async (accessToken, refreshToken, profile, done) => {
  // Find or create user
  let user = await db.user.findUnique({
    where: { googleId: profile.id },
  });

  if (!user) {
    user = await db.user.create({
      data: {
        googleId: profile.id,
        email: profile.emails[0].value,
        name: profile.displayName,
        avatar: profile.photos[0]?.value,
      },
    });
  }

  done(null, user);
}));

passport.serializeUser((user, done) => done(null, user.id));
passport.deserializeUser(async (id, done) => {
  const user = await db.user.findUnique({ where: { id } });
  done(null, user);
});
```

```js
// routes/auth.js — OAuth routes

import { Router } from 'express';
import passport from 'passport';

const router = Router();

// Start OAuth flow
router.get('/google',
  passport.authenticate('google', { scope: ['profile', 'email'] })
);

// Handle callback
router.get('/google/callback',
  passport.authenticate('google', { failureRedirect: '/login' }),
  (req, res) => {
    res.redirect('/dashboard');
  }
);

// Logout
router.post('/logout', (req, res) => {
  req.logout(() => {
    res.redirect('/');
  });
});

export default router;
```

## Next Steps

For session management, continue to [Session Management](../sessions/05-session-management.md).
