# Supabase Multi-Factor Auth

## What You'll Learn

- How to enable MFA with Supabase
- How to implement TOTP with Supabase
- How to verify MFA codes

## Setup

```ts
// Enable MFA factor
const { data, error } = await supabase.auth.mfa.enroll({
  factorType: 'totp',
  friendlyName: 'My Authenticator',
});

// data.totp.qr_code contains the QR code SVG
// data.totp.uri contains the otpauth:// URI

// Challenge (after user enters code)
const { data: challengeData, error: challengeError } =
  await supabase.auth.mfa.challenge({ factorId: data.id });

// Verify
const { data: verifyData, error: verifyError } =
  await supabase.auth.mfa.verify({
    factorId: data.id,
    challengeId: challengeData.id,
    code: '123456',  // User's TOTP code
  });
```

## Next Steps

For NextAuth, continue to [NextAuth Setup](../03-nextauth-v4/01-nextauth-setup.md).
