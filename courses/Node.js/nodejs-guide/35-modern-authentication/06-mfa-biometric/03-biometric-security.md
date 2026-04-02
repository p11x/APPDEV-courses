# Biometric Security

## What You'll Learn

- How to secure biometric implementations
- How to handle replay attacks
- How to validate attestation
- How to store credentials securely

## Security Considerations

```ts
// Always verify:
// 1. Challenge matches (prevent replay)
// 2. Origin matches (prevent phishing)
// 3. RP ID matches (prevent cross-site)
// 4. Counter increases (prevent cloned authenticators)

const verification = await verifyRegistrationResponse({
  response,
  expectedChallenge: storedChallenge,
  expectedOrigin: 'https://myapp.com',  // Must match exactly
  expectedRPID: 'myapp.com',            // Must match domain
});
```

## Counter Validation

```ts
// Check that the authenticator counter increases
// If counter goes backwards, the credential may be cloned
if (authenticator.counter <= storedCounter) {
  throw new Error('Possible cloned authenticator');
}
```

## Next Steps

For recovery, continue to [MFA Recovery](./04-mfa-recovery.md).
