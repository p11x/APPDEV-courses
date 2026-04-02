# MFA Setup

## What You'll Learn

- How to implement TOTP-based MFA
- How to generate and verify QR codes
- How to handle MFA in your auth flow
- How to store MFA secrets securely

## TOTP Implementation

```bash
npm install otplib qrcode speakeasy
```

```ts
// mfa.ts

import { authenticator } from 'otplib';
import QRCode from 'qrcode';

// Generate secret for a user
function generateSecret(email: string) {
  const secret = authenticator.generateSecret();
  const otpauthUrl = authenticator.keyuri(email, 'MyApp', secret);
  return { secret, otpauthUrl };
}

// Generate QR code as data URL
async function generateQR(otpauthUrl: string): Promise<string> {
  return QRCode.toDataURL(otpauthUrl);
}

// Verify TOTP code
function verifyToken(secret: string, token: string): boolean {
  return authenticator.verify({ token, secret });
}

// Recovery codes
function generateRecoveryCodes(count = 10): string[] {
  const codes = [];
  for (let i = 0; i < count; i++) {
    codes.push(crypto.randomBytes(4).toString('hex').toUpperCase());
  }
  return codes;
}
```

## Auth Flow with MFA

```ts
// routes/auth.ts

app.post('/login', async (req, res) => {
  const { email, password } = req.body;

  // Step 1: Verify password
  const user = await db.user.findUnique({ where: { email } });
  if (!user || !await bcrypt.compare(password, user.password)) {
    return res.status(401).json({ error: 'Invalid credentials' });
  }

  // Step 2: Check if MFA is enabled
  if (user.mfaEnabled) {
    // Generate temporary token for MFA step
    const mfaToken = jwt.sign({ userId: user.id, mfa: true }, JWT_SECRET, {
      expiresIn: '5m',
    });

    return res.json({
      requiresMfa: true,
      mfaToken,
    });
  }

  // No MFA — issue session
  const token = jwt.sign({ userId: user.id }, JWT_SECRET);
  res.json({ token });
});

app.post('/verify-mfa', async (req, res) => {
  const { mfaToken, code } = req.body;

  // Verify MFA token
  const decoded = jwt.verify(mfaToken, JWT_SECRET) as { userId: string; mfa: boolean };
  if (!decoded.mfa) {
    return res.status(400).json({ error: 'Invalid MFA token' });
  }

  // Get user's MFA secret
  const user = await db.user.findUnique({ where: { id: decoded.userId } });

  // Verify TOTP code
  const isValid = verifyToken(user.mfaSecret, code);
  if (!isValid) {
    // Check recovery codes
    const recoveryIndex = user.recoveryCodes.indexOf(code);
    if (recoveryIndex === -1) {
      return res.status(401).json({ error: 'Invalid code' });
    }
    // Remove used recovery code
    user.recoveryCodes.splice(recoveryIndex, 1);
    await db.user.update({ where: { id: user.id }, data: { recoveryCodes: user.recoveryCodes } });
  }

  // Issue session token
  const token = jwt.sign({ userId: user.id }, JWT_SECRET);
  res.json({ token });
});
```

## Next Steps

For biometric auth, continue to [Biometric Auth](./02-biometric-auth.md).
