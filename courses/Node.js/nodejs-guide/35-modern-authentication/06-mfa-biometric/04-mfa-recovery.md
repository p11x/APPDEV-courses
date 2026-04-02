# MFA Recovery

## What You'll Learn

- How to implement recovery codes
- How to handle lost authenticator devices
- How to verify identity for recovery
- How to reset MFA securely

## Recovery Codes

```ts
// Generate recovery codes during MFA setup

function generateRecoveryCodes(count = 10): string[] {
  const codes = [];
  for (let i = 0; i < count; i++) {
    // Each code is 8 characters, easy to type
    codes.push(crypto.randomBytes(4).toString('hex').toUpperCase());
  }
  return codes;
}

// Store hashed recovery codes
async function storeRecoveryCodes(userId: string, codes: string[]) {
  const hashed = await Promise.all(
    codes.map((code) => bcrypt.hash(code, 10))
  );
  await db.user.update({
    where: { id: userId },
    data: { recoveryCodes: hashed },
  });
}

// Verify recovery code
async function verifyRecoveryCode(userId: string, code: string): Promise<boolean> {
  const user = await db.user.findUnique({ where: { id: userId } });

  for (let i = 0; i < user.recoveryCodes.length; i++) {
    if (await bcrypt.compare(code, user.recoveryCodes[i])) {
      // Remove used code
      user.recoveryCodes.splice(i, 1);
      await db.user.update({
        where: { id: userId },
        data: { recoveryCodes: user.recoveryCodes },
      });
      return true;
    }
  }
  return false;
}
```

## Next Steps

For backup, continue to [MFA Backup](./05-mfa-backup.md).
