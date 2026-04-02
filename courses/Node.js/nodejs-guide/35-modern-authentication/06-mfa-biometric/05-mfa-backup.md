# MFA Backup Codes

## What You'll Learn

- How to generate and display backup codes
- How to enforce backup code generation
- How to regenerate backup codes
- How to handle running out of backup codes

## Implementation

```ts
// When user enables MFA, force backup code generation

app.post('/mfa/enable', authMiddleware, async (req, res) => {
  const userId = req.user.id;
  const user = await db.user.findUnique({ where: { id: userId } });

  if (user.mfaEnabled) {
    return res.status(400).json({ error: 'MFA already enabled' });
  }

  // Generate secret
  const { secret, otpauthUrl } = generateSecret(user.email);
  const qr = await generateQR(otpauthUrl);

  // Generate backup codes
  const codes = generateRecoveryCodes(10);
  await storeRecoveryCodes(userId, codes);

  // Store secret temporarily (enable after verification)
  await db.user.update({
    where: { id: userId },
    data: { mfaSecretTemp: secret },
  });

  res.json({
    qr,
    backupCodes: codes,  // Show once — user must save them
    message: 'Save these backup codes in a safe place!',
  });
});

// Verify first code to complete MFA setup
app.post('/mfa/verify-setup', authMiddleware, async (req, res) => {
  const { code } = req.body;
  const user = await db.user.findUnique({ where: { id: req.user.id } });

  if (!verifyToken(user.mfaSecretTemp, code)) {
    return res.status(400).json({ error: 'Invalid code' });
  }

  // Enable MFA
  await db.user.update({
    where: { id: req.user.id },
    data: {
      mfaEnabled: true,
      mfaSecret: user.mfaSecretTemp,
      mfaSecretTemp: null,
    },
  });

  res.json({ message: 'MFA enabled successfully' });
});

// Regenerate backup codes
app.post('/mfa/regenerate-codes', authMiddleware, async (req, res) => {
  const { currentCode } = req.body;
  const user = await db.user.findUnique({ where: { id: req.user.id } });

  // Require current MFA code to regenerate
  if (!verifyToken(user.mfaSecret, currentCode)) {
    return res.status(401).json({ error: 'Invalid MFA code' });
  }

  const codes = generateRecoveryCodes(10);
  await storeRecoveryCodes(user.id, codes);

  res.json({ backupCodes: codes });
});
```

## UI Best Practices

- Display codes as a grid (easy to screenshot/print)
- Warn user to save codes before closing
- Show codes only once during setup
- Allow downloading codes as a text file
- Show remaining code count in security settings

## Next Steps

This concludes Chapter 35. Return to the [guide index](../../index.html).
