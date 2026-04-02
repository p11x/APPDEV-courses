# Firebase Setup

## What You'll Learn

- How to set up Firebase Auth
- How to configure Firebase in Node.js
- How Firebase Auth works
- How Firebase compares to other auth providers

## Setup

```bash
npm install firebase firebase-admin
```

```ts
// lib/firebase.ts — Client SDK

import { initializeApp } from 'firebase/app';
import { getAuth } from 'firebase/auth';

const firebaseConfig = {
  apiKey: process.env.NEXT_PUBLIC_FIREBASE_API_KEY,
  authDomain: process.env.NEXT_PUBLIC_FIREBASE_AUTH_DOMAIN,
  projectId: process.env.NEXT_PUBLIC_FIREBASE_PROJECT_ID,
};

const app = initializeApp(firebaseConfig);
export const auth = getAuth(app);
```

```ts
// lib/firebase-admin.ts — Admin SDK (server-side)

import { initializeApp, cert } from 'firebase-admin/app';
import { getAuth } from 'firebase-admin/auth';

const adminApp = initializeApp({
  credential: cert({
    projectId: process.env.FIREBASE_PROJECT_ID,
    clientEmail: process.env.FIREBASE_CLIENT_EMAIL,
    privateKey: process.env.FIREBASE_PRIVATE_KEY?.replace(/\\n/g, '\n'),
  }),
});

export const adminAuth = getAuth(adminApp);
```

## Auth Methods

```ts
import { signInWithEmailAndPassword, createUserWithEmailAndPassword } from 'firebase/auth';

// Sign up
const userCred = await createUserWithEmailAndPassword(auth, email, password);

// Sign in
const userCred = await signInWithEmailAndPassword(auth, email, password);

// OAuth
import { signInWithPopup, GoogleAuthProvider } from 'firebase/auth';
const provider = new GoogleAuthProvider();
const result = await signInWithPopup(auth, provider);

// Sign out
import { signOut } from 'firebase/auth';
await signOut(auth);
```

## Next Steps

For auth features, continue to [Firebase Auth](./02-firebase-auth.md).
