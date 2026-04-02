# Biometric Authentication

## What You'll Learn

- How WebAuthn works for biometric auth
- How to implement passkeys
- How biometric auth improves security
- How to handle biometric registration and verification

## What Is WebAuthn?

WebAuthn is a W3C standard for passwordless authentication using biometrics (fingerprint, face) or security keys (YubiKey). **Passkeys** are WebAuthn credentials synced across devices.

## Implementation with SimpleWebAuthn

```bash
npm install @simplewebauthn/server @simplewebauthn/browser
```

```ts
// webauthn.ts — Server-side

import {
  generateRegistrationOptions,
  verifyRegistrationResponse,
  generateAuthenticationOptions,
  verifyAuthenticationResponse,
} from '@simplewebauthn/server';

const rpName = 'My App';
const rpID = 'localhost';
const origin = `http://${rpID}:3000`;

// Generate registration options
async function startRegistration(userId: string) {
  const user = await db.user.findUnique({ where: { id: userId } });

  const options = await generateRegistrationOptions({
    rpName,
    rpID,
    userName: user.email,
    attestationType: 'none',
    authenticatorSelection: {
      residentKey: 'preferred',
      userVerification: 'preferred',
    },
  });

  // Store challenge in session
  await db.challenge.create({ data: { userId, challenge: options.challenge } });

  return options;
}

// Verify registration
async function finishRegistration(userId: string, response: any) {
  const challenge = await db.challenge.findFirst({ where: { userId } });

  const verification = await verifyRegistrationResponse({
    response,
    expectedChallenge: challenge.challenge,
    expectedOrigin: origin,
    expectedRPID: rpID,
  });

  if (verification.verified) {
    // Store credential
    await db.credential.create({
      data: {
        userId,
        credentialID: verification.registrationInfo!.credentialID,
        publicKey: Buffer.from(verification.registrationInfo!.credentialPublicKey),
        counter: verification.registrationInfo!.counter,
      },
    });
  }

  return verification.verified;
}
```

## Next Steps

For security, continue to [Biometric Security](./03-biometric-security.md).
