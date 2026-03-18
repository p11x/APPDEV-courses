# Data Encryption at Rest

## What You'll Learn

- Encrypting database fields
- Encrypting files
- Using envelope encryption

## Prerequisites

- Completed `03-tls-ssl-https.md`

## Encrypting Database Fields

```python
from cryptography.fernet import Fernet
from dataclasses import dataclass
import base64

@dataclass
class FieldEncryptor:
    """Encrypt and decrypt database fields."""
    key: bytes
    
    def __post_init__(self):
        self.cipher = Fernet(self.key)
    
    def encrypt(self, value: str) -> str:
        """Encrypt a string value."""
        encrypted = self.cipher.encrypt(value.encode())
        return base64.b64encode(encrypted).decode()
    
    def decrypt(self, encrypted_value: str) -> str:
        """Decrypt an encrypted value."""
        decoded = base64.b64decode(encrypted_value.encode())
        decrypted = self.cipher.decrypt(decoded)
        return decrypted.decode()

# Usage
encryptor = FieldEncryptor(Fernet.generate_key())

# Encrypt sensitive data before saving
ssn_encrypted = encryptor.encrypt("123-45-6789")
print(f"Encrypted: {ssn_encrypted}")

# Decrypt when reading
ssn_decrypted = encryptor.decrypt(ssn_encrypted)
print(f"Decrypted: {ssn_decrypted}")
```

## Field-Level Encryption in SQLAlchemy

```python
from sqlalchemy import Column, String, create_engine
from sqlalchemy.dialects.postgresql import ARRAY
from sqlalchemy.ext.declarative import declarative_base
from cryptography.fernet import Fernet

Base = declarative_base()

class User(Base):
    __tablename__ = "users"
    
    id = Column(String, primary_key=True)
    email = Column(String)
    ssn_encrypted = Column(String)  # Encrypted field
    
    def set_ssn(self, ssn: str, cipher: Fernet):
        """Set encrypted SSN."""
        self.ssn_encrypted = cipher.encrypt(ssn.encode()).decode()
    
    def get_ssn(self, cipher: Fernet) -> str:
        """Get decrypted SSN."""
        if not self.ssn_encrypted:
            return None
        return cipher.decrypt(self.ssn_encrypted.encode()).decode()
```

## Encrypting Files

```python
from cryptography.fernet import Fernet
import os
import shutil

def encrypt_file(input_path: str, output_path: str, key: bytes) -> None:
    """Encrypt a file."""
    cipher = Fernet(key)
    
    with open(input_path, "rb") as f_in:
        data = f_in.read()
        encrypted = cipher.encrypt(data)
    
    with open(output_path, "wb") as f_out:
        f_out.write(encrypted)

def decrypt_file(input_path: str, output_path: str, key: bytes) -> None:
    """Decrypt a file."""
    cipher = Fernet(key)
    
    with open(input_path, "rb") as f_in:
        encrypted = f_in.read()
        decrypted = cipher.decrypt(encrypted)
    
    with open(output_path, "wb") as f_out:
        f_out.write(decrypted)

# Usage
key = Fernet.generate_key()
encrypt_file("secret.txt", "secret.enc", key)
decrypt_file("secret.enc", "decrypted.txt", key)
```

## Envelope Encryption

```python
from cryptography.fernet import Fernet
import secrets

class EnvelopeEncryption:
    """Use different keys for different data."""
    
    def __init__(self, master_key: bytes):
        self.master_key = master_key
        self.cipher = Fernet(master_key)
    
    def generate_data_key(self) -> bytes:
        """Generate a new data key."""
        return Fernet.generate_key()
    
    def encrypt_with_key(self, data: str, data_key: bytes) -> tuple:
        """Encrypt data with a specific key."""
        cipher = Fernet(data_key)
        encrypted_data = cipher.encrypt(data.encode())
        
        # Encrypt the data key with master key
        encrypted_key = self.cipher.encrypt(data_key)
        
        return encrypted_data, encrypted_key
    
    def decrypt_with_key(self, encrypted_data: bytes, encrypted_key: bytes) -> str:
        """Decrypt data using encrypted key."""
        # Decrypt the data key
        data_key = self.cipher.decrypt(encrypted_key)
        
        # Decrypt the data
        cipher = Fernet(data_key)
        return cipher.decrypt(encrypted_data).decode()

# Usage
master_key = Fernet.generate_key()
envelope = EnvelopeEncryption(master_key)

data_key = envelope.generate_data_key()
encrypted_data, encrypted_key = envelope.encrypt_with_key("secret data", data_key)
decrypted = envelope.decrypt_with_key(encrypted_data, encrypted_key)
```

## Summary

- Encrypt sensitive database fields
- Encrypt files before storing
- Use envelope encryption for scalability

## Next Steps

Continue to `05-api-security-headers.md`.
