# Firebase Auth

## What You'll Learn

- How to implement Firebase Auth
- How to use email/password and OAuth
- How to handle auth state in React
- How to protect routes

## Email/Password

```ts
import { createUserWithEmailAndPassword, signInWithEmailAndPassword } from 'firebase/auth';
import { auth } from '../lib/firebase.js';

// Sign up
const userCredential = await createUserWithEmailAndPassword(auth, email, password);
console.log(userCredential.user);

// Sign in
const userCredential = await signInWithEmailAndPassword(auth, email, password);
console.log(userCredential.user);

// Sign out
import { signOut } from 'firebase/auth';
await signOut(auth);
```

## OAuth

```ts
import { signInWithPopup, GoogleAuthProvider, GithubAuthProvider } from 'firebase/auth';

const googleProvider = new GoogleAuthProvider();
const result = await signInWithPopup(auth, googleProvider);

const githubProvider = new GithubAuthProvider();
const result = await signInWithPopup(auth, githubProvider);
```

## Auth State

```tsx
import { onAuthStateChanged } from 'firebase/auth';
import { useEffect, useState } from 'react';

function useAuth() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const unsubscribe = onAuthStateChanged(auth, (user) => {
      setUser(user);
      setLoading(false);
    });

    return () => unsubscribe();
  }, []);

  return { user, loading };
}
```

## Next Steps

For multi-provider, continue to [Firebase Multi-Provider](./03-firebase-multi-provider.md).
