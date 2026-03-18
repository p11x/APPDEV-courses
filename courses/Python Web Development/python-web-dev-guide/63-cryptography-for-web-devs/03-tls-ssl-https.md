# TLS/SSL and HTTPS

## What You'll Learn

- Understanding HTTPS
- SSL/TLS certificates
- Using certificates in Python

## Prerequisites

- Completed `02-secure-password-storage.md`

## What Is HTTPS

HTTPS is HTTP over TLS/SSL. It encrypts data in transit between browser and server.

Think of it like sending a letter in a sealed envelope instead of a postcard - anyone can read a postcard, but only the recipient can read a sealed envelope.

## SSL/TLS Certificates

```python
# Generate self-signed certificate (for development)
from cryptography import x509
from cryptography.x509.oid import NameOID
from cryptography.hazmat.primitives import hashes
import datetime

# Generate key
from cryptography.hazmat.primitives.asymmetric import rsa
private_key = rsa.generate_private_key(public_exponent=65537, key_size=2048)

# Create certificate
subject = issuer = x509.Name([
    x509.NameAttribute(NameOID.COUNTRY_NAME, "US"),
    x509.NameAttribute(NameOID.STATE_OR_PROVINCE_NAME, "California"),
    x509.NameAttribute(NameOID.LOCALITY_NAME, "San Francisco"),
    x509.NameAttribute(NameOID.ORGANIZATION_NAME, "Example"),
    x509.NameAttribute(NameOID.COMMON_NAME, "localhost"),
])

cert = (
    x509.CertificateBuilder()
    .subject_name(subject)
    .issuer_name(issuer)
    .public_key(private_key.public_key())
    .serial_number(x509.random_serial_number())
    .not_valid_before(datetime.datetime.utcnow())
    .not_valid_after(datetime.datetime.utcnow() + datetime.timedelta(days=365))
    .sign(private_key, hashes.SHA256())
)

# Save certificate
with open("cert.pem", "wb") as f:
    f.write(cert.public_bytes(serialization.Encoding.PEM))

# Save private key
with open("key.pem", "wb") as f:
    f.write(private_key.private_bytes(
        encoding=serialization.Encoding.PEM,
        format=serialization.PrivateFormat.TraditionalOpenSSL,
        encryption_algorithm=serialization.NoEncryption()
    ))
```

## Using HTTPS in Flask

```python
from flask import Flask

app = Flask(__name__)

@app.route("/")
def index():
    return "Secure!"

# Run with HTTPS (development)
if __name__ == "__main__":
    app.run(
        ssl_context=("cert.pem", "key.pem"),
        host="0.0.0.0",
        port=5000
    )
```

## Using HTTPS in FastAPI

```python
from fastapi import FastAPI
import uvicorn

app = FastAPI()

@app.get("/")
def read_root():
    return {"message": "Secure!"}

# Run with HTTPS
if __name__ == "__main__":
    uvicorn.run(
        app,
        host="0.0.0.0",
        port=5000,
        ssl_keyfile="key.pem",
        ssl_certfile="cert.pem"
    )
```

## Requesting Production Certificates

Let's Encrypt provides free certificates:

```bash
# Using certbot
sudo apt-get install certbot python3-certbot-nginx
sudo certbot --nginx -d example.com -d www.example.com
```

## Certificate Verification

```python
import requests
import ssl

# Verify certificate
response = requests.get("https://example.com", verify=True)

# Use custom CA bundle
response = requests.get("https://example.com", verify="/path/to/ca-bundle.crt")
```

## Summary

- HTTPS encrypts data in transit
- Use certificates for secure communication
- Let's Encrypt provides free certificates

## Next Steps

Continue to `04-data-encryption-at-rest.md`.
