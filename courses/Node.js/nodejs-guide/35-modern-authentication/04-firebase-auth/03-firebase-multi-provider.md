# Firebase Multi-Provider Auth

## What You'll Learn

- How to link multiple auth providers to one account
- How to handle provider conflicts
- How to implement account merging
- How to unlink providers

## Linking Providers

```ts
import { linkWithPopup, GoogleAuthProvider, GithubAuthProvider } from 'firebase/auth';

// Link Google to existing account
const googleProvider = new GoogleAuthProvider();
const result = await linkWithPopup(auth.currentUser, googleProvider);

// Link GitHub to existing account
const githubProvider = new GithubAuthProvider();
const result = await linkWithPopup(auth.currentUser, githubProvider);
```

## Handling Conflicts

```ts
import { fetchSignInMethodsForEmail } from 'firebase/auth';

// Check if email already has providers
const methods = await fetchSignInMethodsForEmail(auth, email);

if (methods.length > 0) {
  // Email already exists — ask user to link or sign in
  console.log('Existing providers:', methods);
}
```

## Unlinking

```ts
import { unlink } from 'firebase/auth';

// Remove a provider
await unlink(auth.currentUser, 'google.com');
```

## Next Steps

For security, continue to [Firebase Security](./04-firebase-security.md).
