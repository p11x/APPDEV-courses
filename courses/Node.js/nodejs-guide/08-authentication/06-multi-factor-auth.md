# Multi-Factor Authentication

## What You'll Learn

- What MFA is and why it matters
- TOTP (Time-based One-Time Password) implementation
- QR code generation for authenticator apps

## TOTP Implementation

```bash
npm install otplib qrcode
```

```js
// mfa.js — TOTP setup and verification

import { authenticator } from 'otplib';
import QRCode from 'qrcode';

// Generate a secret for a user
function generateSecret(email) {
  const secret = authenticator.generateSecret();
  const otpauthUrl = authenticator.keyuri(email, 'MyApp', secret);
  return { secret, otpauthUrl };
}

// Generate QR code for authenticator app
async function generateQR(otpauthUrl) {
  return QRCode.toDataURL(otpauthUrl);
}

// Verify a TOTP code
function verifyToken(secret, token) {
  return authenticator.verify({ token, secret });
}

// Setup endpoint
app.post('/auth/mfa/setup', authMiddleware, async (req, res) => {
  const { secret, otpauthUrl } = generateSecret(req.user.email);
  const qrCode = await generateQR(otpauthUrl);

  // Store secret temporarily (confirm with first valid code)
  req.session.mfaSecret = secret;

  res.json({ qrCode, manualEntry: secret });
});

// Verify endpoint
app.post('/auth/mfa/verify', authMiddleware, (req, res) => {
  const { code } = req.body;
  const secret = req.user.mfaSecret || req.session.mfaSecret;

  if (verifyToken(secret, code)) {
    // Enable MFA for user
    req.user.mfaEnabled = true;
    req.user.mfaSecret = secret;
    res.json({ success: true });
  } else {
    res.status(400).json({ error: 'Invalid code' });
  }
});
```

## Next Steps

For deployment, continue to [Chapter 10: Deployment](../../10-deployment/docker/01-dockerfile.md).
