# PGP and SSH Keys for Authentication

## What You'll Learn
- PGP encryption fundamentals
- SSH key authentication
- Implementing key-based authentication in web apps
- Key management best practices
- Practical implementation examples

## Prerequisites
- Completed `03-rbac-with-fastapi.md` — RBAC implementation
- Understanding of encryption concepts
- Basic command line knowledge

## SSH Key Authentication

SSH keys provide secure passwordless authentication:

```
SSH Key Pair:
├── Public Key (.pub)  → Placed on server (authorized_keys)
└── Private Key        → Kept securely on client
```

### Generating SSH Keys

```bash
# Generate Ed25519 key (recommended)
ssh-keygen -t ed25519 -C "your_email@example.com"

# Generate RSA key (legacy compatibility)
ssh-keygen -t rsa -b 4096 -C "your_email@example.com"

# View your public key
cat ~/.ssh/id_ed25519.pub
```

### SSH Key Authentication in Python

```python
import paramiko
from pathlib import Path

def ssh_key_auth_example():
    """Connect to server using SSH key."""
    
    # Load private key
    private_key_path = Path("~/.ssh/id_ed25519").expanduser()
    private_key = paramiko.Ed255Key.from_private_key_file(private_key_path)
    
    # Create SSH client
    client = paramiko.SSHClient()
    client.set_missing_host_key_policy(paramiko.AutoAddPolicy())
    
    # Connect using key
    client.connect(
        hostname="server.example.com",
        username="deploy",
        pkey=private_key,
        # Don't verify host key in development
        look_for_keys=False,
        allow_agent=False,
    )
    
    # Execute command
    stdin, stdout, stderr = client.exec_command("ls -la")
    print(stdout.read().decode())
    
    client.close()
```

## PGP Encryption

PGP (Pretty Good Privacy) provides:
- **Encryption**: Only intended recipient can read
- **Signing**: Verify sender authenticity
- **Key pairs**: Public and private keys

### Generating PGP Keys

```bash
# Install GPG
# macOS: brew install gnupg
# Ubuntu: sudo apt install gnupg

# Generate key pair
gpg --full-generate-key

# List keys
gpg --list-keys

# Export public key
gpg --armor --export your@email.com > public_key.asc

# Export private key (keep secret!)
gpg --armor --export-secret-keys your@email.com > private_key.asc
```

### PGP with Python

```bash
pip install python-gnupg
```

```python
import gnupg

# Initialize GPG
gpg = gnupg.GPG(gnupghome="/path/to/gnupg/home")

class PGPManager:
    """Manage PGP keys for encryption/decryption."""
    
    def generate_key(self, name: str, email: str, passphrase: str) -> str:
        """Generate a new PGP key pair."""
        
        input_data = gpg.gen_key_input(
            name_real=name,
            name_email=email,
            passphrase=passphrase,
            key_type="RSA",
            key_length=4096,
        )
        
        key = gpg.gen_key(input_data)
        return key.fingerprint
    
    def export_public_key(self, fingerprint: str) -> str:
        """Export public key in ASCII format."""
        public_key = gpg.export_keys(fingerprint)
        return public_key
    
    def encrypt(self, message: str, recipient_fingerprint: str) -> str:
        """Encrypt message for recipient."""
        
        encrypted = gpg.encrypt(
            message,
            recipients=[recipient_fingerprint],
            always_trust=True,
        )
        
        return str(encrypted)
    
    def decrypt(self, encrypted_message: str, passphrase: str) -> str:
        """Decrypt message using private key."""
        
        decrypted = gpg.decrypt(
            encrypted_message,
            passphrase=passphrase,
        )
        
        if not decrypted.ok:
            raise ValueError(f"Decryption failed: {decrypted.status}")
        
        return str(decrypted)
    
    def sign(self, message: str, passphrase: str, key_fingerprint: str) -> str:
        """Sign a message."""
        
        signed = gpg.sign(
            message,
            passphrase=passphrase,
            keyid=key_fingerprint,
            clearsign=True,
        )
        
        return str(signed)
    
    def verify(self, signed_message: str) -> bool:
        """Verify a signed message."""
        
        verified = gpg.verify(signed_message)
        return verified.ok

# Usage
pgp = PGPManager()

# Generate key
key_id = pgp.generate_key("John Doe", "john@example.com", "passphrase123")
print(f"Generated key: {key_id}")

# Encrypt message
public_key = pgp.export_public_key(key_id)
encrypted = pgp.encrypt("Secret message", key_id)
print(f"Encrypted: {encrypted}")

# Decrypt message
decrypted = pgp.decrypt(encrypted, "passphrase123")
print(f"Decrypted: {decrypted}")
```

## Key-Based Authentication in FastAPI

### SSH Key Authentication for API

```python
from fastapi import FastAPI, HTTPException, Header
from pydantic import BaseModel
from typing import Optional
import paramiko

app = FastAPI()

class SSHKey(BaseModel):
    """Model for SSH key authentication."""
    key_id: str
    public_key: str

# Mock database of authorized keys
authorized_keys_db: dict[str, str] = {
    "key-123": "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAA..."
}

def validate_ssh_key(authorization: str = Header(None)) -> str:
    """Validate SSH key from Authorization header."""
    
    if not authorization:
        raise HTTPException(status_code=401, detail="Missing SSH key")
    
    if not authorization.startswith("ssh-key "):
        raise HTTPException(status_code=401, detail="Invalid SSH key format")
    
    key_id = authorization[8:]  # Remove "ssh-key " prefix
    
    if key_id not in authorized_keys_db:
        raise HTTPException(status_code=401, detail="Unknown SSH key")
    
    return key_id

@app.get("/secure-data")
async def get_secure_data(key_id: str = Depends(validate_ssh_key)) -> dict:
    """Endpoint accessible only with valid SSH key."""
    
    return {
        "message": "Access granted via SSH key",
        "key_id": key_id,
        "data": {
            "secret": "value"
        }
    }
```

### PGP-Encrypted API Responses

```python
from fastapi import FastAPI, Depends
from pydantic import BaseModel

app = FastAPI()

pgp_manager = PGPManager()

# Store user keys (in production, use database)
user_keys: dict[str, dict] = {
    "user123": {
        "fingerprint": "ABC123...",
        "passphrase": "user-passphrase",
    }
}

class EncryptedResponse(BaseModel):
    """Encrypted API response."""
    encrypted_data: str

@app.get("/secure-message/{user_id}", response_model=EncryptedResponse)
async def get_encrypted_message(user_id: str) -> EncryptedResponse:
    """Return PGP-encrypted message for user."""
    
    if user_id not in user_keys:
        raise HTTPException(status_code=404, detail="User not found")
    
    user = user_keys[user_id]
    fingerprint = user["fingerprint"]
    
    # Encrypt message
    message = f"Hello user {user_id}! Your secure message."
    encrypted = pgp_manager.encrypt(message, fingerprint)
    
    return EncryptedResponse(encrypted_data=encrypted)
```

## Web-Based Key Management

### Registering SSH Keys

```python
from fastapi import FastAPI, Depends, UploadFile
from pydantic import BaseModel

app = FastAPI()

class SSHKeyRegistration(BaseModel):
    """Register a new SSH key."""
    name: str
    public_key: str

class SSHKeyService:
    """Service for managing SSH keys."""
    
    def validate_public_key(self, public_key: str) -> bool:
        """Validate SSH public key format."""
        
        # Basic validation
        valid_prefixes = [
            "ssh-rsa",
            "ssh-dss", 
            "ssh-ed25519",
            "ecdsa-sha2-nistp",
        ]
        
        key_parts = public_key.strip().split()
        if len(key_parts) < 2:
            return False
        
        key_type = key_parts[0]
        return key_type in valid_prefixes
    
    def register_key(self, user_id: str, key: SSHKeyRegistration) -> dict:
        """Register SSH key for user."""
        
        if not self.validate_public_key(key.public_key):
            raise ValueError("Invalid SSH public key format")
        
        # Store in database (simplified)
        key_id = f"key-{user_id}-{hash(key.public_key)[:8]}"
        
        return {
            "key_id": key_id,
            "name": key.name,
            "fingerprint": self.get_fingerprint(key.public_key),
        }
    
    def get_fingerprint(self, public_key: str) -> str:
        """Get SSH key fingerprint."""
        
        from paramiko import RSAKey
        import base64
        import hashlib
        
        # Parse key (simplified)
        key_data = public_key.split()[1]
        decoded = base64.b64decode(key_data)
        
        # Calculate SHA256 fingerprint
        digest = hashlib.sha256(decoded).digest()
        fingerprint = base64.b64encode(digest).decode()
        
        return f"SHA256:{fingerprint}"

# Usage
@app.post("/keys/register")
async def register_ssh_key(
    key_data: SSHKeyRegistration,
    current_user: User = Depends(get_current_user)
) -> dict:
    """Register new SSH key for user."""
    
    service = SSHKeyService()
    key = service.register_key(current_user.id, key_data)
    
    return key
```

## Production Considerations

- **Key rotation**: Regularly rotate SSH and PGP keys
- **Storage**: Store private keys and passphrases securely
- **Backup**: Backup keys securely (encrypted)
- **Revocation**: Have a process to revoke compromised keys
- **Audit**: Log key usage for security monitoring

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Hardcoding private keys

**Wrong:**
```python
# In source code!
PRIVATE_KEY = "-----BEGIN RSA PRIVATE KEY-----\n..."
```

**Why it fails:** Anyone with code access has the key!

**Fix:**
```python
# Load from environment or secure vault
import os
private_key = os.environ.get("PRIVATE_KEY")
```

### ❌ Mistake 2: Weak passphrases

**Wrong:**
```python
passphrase = "123456"  # Too simple!
```

**Why it fails:** Easily brute-forced.

**Fix:**
```python
import secrets
passphrase = secrets.token_urlsafe(16)  # Strong random
```

### ❌ Mistake 3: No key expiration

**Wrong:**
```python
# Keys never expire
key = generate_key()  # No expiry date
```

**Why it fails:** Old keys become security risks.

**Fix:**
```python
# Set expiration
gpg.gen_key_input(
    passphrase=passphrase,
    expire_date="2y",  # Expires in 2 years
)
```

## Summary

- SSH keys provide secure, passwordless authentication
- PGP encryption ensures message confidentiality and authenticity
- Use SSH keys for server access and API authentication
- Use PGP for encrypted communication and signing
- Store private keys securely, never in source code

## Next Steps

This completes the Auth and Authorization Advanced folder. Continue to `36-realtime-and-streaming/01-websockets-deep-dive.md` to explore real-time communication patterns.
