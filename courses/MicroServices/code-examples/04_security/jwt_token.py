"""
JWT Token Generation and Verification

This module provides JWT (JSON Web Token) functionality:
- Token generation with claims
- Token verification with validation
- Token refresh
- Token revocation
- FastAPI integration

Usage:
    # Generate token
    token = JWTHandler.encode(
        {"user_id": "123", "role": "admin"},
        expires_in=3600
    )
    
    # Verify token
    payload = JWTHandler.decode(token)
"""

import logging
from datetime import datetime, timedelta
from typing import Any, Dict, List, Optional, Set
from dataclasses import dataclass

from jose import jwt, JWTError
from passlib.context import CryptContext


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


# Configuration
JWT_SECRET_KEY = "your-secret-key-change-in-production"
JWT_ALGORITHM = "HS256"
JWT_EXPIRATION_MINUTES = 60
JWT_REFRESH_EXPIRATION_DAYS = 7


@dataclass
class TokenPayload:
    """JWT token payload structure."""
    sub: str  # Subject (user ID)
    exp: int  # Expiration timestamp
    iat: int  # Issued at timestamp
    roles: List[str] = None
    permissions: List[str] = None
    metadata: Dict[str, Any] = None


class JWTError(Exception):
    """Base exception for JWT operations."""
    pass


class JWTExpiredError(JWTError):
    """Token has expired."""
    pass


class JWTInvalidError(JWTError):
    """Token is invalid."""
    pass


class JWTHandler:
    """
    Handler for JWT token operations.
    
    Supports:
    - Token generation with custom claims
    - Token verification
    - Token refresh
    - Blacklist for revocation
    """
    
    def __init__(
        self,
        secret_key: str = JWT_SECRET_KEY,
        algorithm: str = JWT_ALGORITHM,
    ):
        self.secret_key = secret_key
        self.algorithm = algorithm
        self._blacklist: Set[str] = set()
    
    def encode(
        self,
        subject: str,
        roles: Optional[List[str]] = None,
        permissions: Optional[List[str]] = None,
        metadata: Optional[Dict[str, Any]] = None,
        expires_in: int = JWT_EXPIRATION_MINUTES * 60,
        refresh: bool = False,
    ) -> str:
        """
        Generate a JWT token.
        
        Args:
            subject: User ID or subject identifier
            roles: User roles
            permissions: User permissions
            metadata: Additional custom claims
            expires_in: Expiration in seconds
            refresh: Whether this is a refresh token
            
        Returns:
            Encoded JWT token string
        """
        now = datetime.utcnow()
        
        # Build payload
        payload = {
            "sub": subject,
            "iat": int(now.timestamp()),
            "exp": int((now + timedelta(seconds=expires_in)).timestamp()),
            "type": "refresh" if refresh else "access",
        }
        
        # Add custom claims
        if roles:
            payload["roles"] = roles
        
        if permissions:
            payload["permissions"] = permissions
        
        if metadata:
            payload["metadata"] = metadata
        
        # Encode token
        token = jwt.encode(payload, self.secret_key, algorithm=self.algorithm)
        
        logger.info(f"Generated token for subject: {subject}")
        
        return token
    
    def decode(
        self,
        token: str,
        verify_exp: bool = True,
    ) -> Dict[str, Any]:
        """
        Decode and verify a JWT token.
        
        Args:
            token: JWT token string
            verify_exp: Whether to verify expiration
            
        Returns:
            Decoded payload
            
        Raises:
            JWTInvalidError: If token is invalid
            JWTExpiredError: If token has expired
        """
        # Check blacklist
        if token in self._blacklist:
            raise JWTInvalidError("Token has been revoked")
        
        try:
            payload = jwt.decode(
                token,
                self.secret_key,
                algorithms=[self.algorithm],
                options={"verify_exp": verify_exp},
            )
            
            return payload
        
        except jwt.ExpiredSignatureError:
            raise JWTExpiredError("Token has expired")
        
        except JWTError as e:
            raise JWTInvalidError(f"Invalid token: {e}")
    
    def refresh_token(
        self,
        token: str,
    ) -> str:
        """
        Refresh an access token using a refresh token.
        
        Args:
            token: Current refresh token
            
        Returns:
            New access token
        """
        # Verify the refresh token
        payload = self.decode(token, verify_exp=True)
        
        if payload.get("type") != "refresh":
            raise JWTInvalidError("Not a refresh token")
        
        # Extract claims for new token
        return self.encode(
            subject=payload["sub"],
            roles=payload.get("roles"),
            permissions=payload.get("permissions"),
            metadata=payload.get("metadata"),
        )
    
    def revoke_token(self, token: str):
        """
        Revoke a token (add to blacklist).
        
        Args:
            token: Token to revoke
        """
        self._blacklist.add(token)
        logger.info("Token revoked")
    
    def verify_role(
        self,
        token: str,
        required_role: str,
    ) -> bool:
        """
        Verify that a token has a specific role.
        
        Args:
            token: JWT token
            required_role: Required role name
            
        Returns:
            True if role is present
        """
        payload = self.decode(token)
        roles = payload.get("roles", [])
        return required_role in roles
    
    def verify_permission(
        self,
        token: str,
        required_permission: str,
    ) -> bool:
        """
        Verify that a token has a specific permission.
        
        Args:
            token: JWT token
            required_permission: Required permission name
            
        Returns:
            True if permission is present
        """
        payload = self.decode(token)
        permissions = payload.get("permissions", [])
        return required_permission in permissions


class PasswordHandler:
    """
    Handler for password hashing and verification.
    
    Uses bcrypt for secure password hashing.
    """
    
    def __init__(self):
        self.context = CryptContext(schemes=["bcrypt"], deprecated="auto")
    
    def hash_password(self, password: str) -> str:
        """
        Hash a password.
        
        Args:
            password: Plain text password
            
        Returns:
            Hashed password
        """
        return self.context.hash(password)
    
    def verify_password(
        self,
        plain_password: str,
        hashed_password: str,
    ) -> bool:
        """
        Verify a password against its hash.
        
        Args:
            plain_password: Plain text password
            hashed_password: Hashed password
            
        Returns:
            True if password matches
        """
        return self.context.verify(plain_password, hashed_password)


# Example FastAPI dependency
def get_current_user(token: str) -> Dict[str, Any]:
    """
    FastAPI dependency to get current user from token.
    
    Usage:
        @app.get("/protected")
        async def protected_route(user: dict = Depends(get_current_user)):
            return user
    """
    handler = JWTHandler()
    return handler.decode(token)


class TokenValidator:
    """
    Validator for JWT tokens in FastAPI.
    
    Provides dependency injection and error handling.
    """
    
    def __init__(self):
        self.jwt_handler = JWTHandler()
    
    def __call__(self, authorization: str = None) -> Dict[str, Any]:
        """Validate token from Authorization header."""
        if not authorization:
            raise JWTError("Authorization header missing")
        
        # Extract token from "Bearer <token>"
        if authorization.startswith("Bearer "):
            token = authorization[7:]
        else:
            token = authorization
        
        return self.jwt_handler.decode(token)


# Example usage
def main():
    """Demonstrate JWT operations."""
    
    # Create handler
    jwt_handler = JWTHandler()
    password_handler = PasswordHandler()
    
    # Generate token
    token = jwt_handler.encode(
        subject="user-123",
        roles=["admin", "user"],
        permissions=["read", "write"],
        metadata={"email": "user@example.com"},
    )
    print(f"Generated token: {token[:50]}...")
    
    # Verify token
    payload = jwt_handler.decode(token)
    print(f"Decoded payload: {payload}")
    
    # Verify role
    print(f"Has admin role: {jwt_handler.verify_role(token, 'admin')}")
    print(f"Has superadmin role: {jwt_handler.verify_role(token, 'superadmin')}")
    
    # Verify permission
    print(f"Has write permission: {jwt_handler.verify_permission(token, 'write')}")
    
    # Password hashing
    password = "securePassword123"
    hashed = password_handler.hash_password(password)
    print(f"Password hash: {hashed}")
    
    print(f"Password verification: {password_handler.verify_password(password, hashed)}")
    print(f"Wrong password: {password_handler.verify_password('wrong', hashed)}")


if __name__ == "__main__":
    main()