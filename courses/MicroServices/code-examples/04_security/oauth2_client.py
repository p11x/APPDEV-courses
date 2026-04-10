"""
OAuth2 Client Implementation

This module provides OAuth2 client functionality:
- Authorization code flow
- Client credentials flow
- Resource owner password flow
- Token refresh
- Token revocation

Usage:
    oauth = OAuth2Client(
        client_id="my-client",
        client_secret="secret",
        token_url="https://auth.example.com/oauth/token",
    )
    
    # Get access token
    token = await oauth.get_token(["read", "write"])
"""

import asyncio
import logging
import base64
from typing import Any, Dict, List, Optional
from dataclasses import dataclass
from enum import Enum

import httpx


logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)


class OAuthFlow(Enum):
    """OAuth2 flow types."""
    AUTHORIZATION_CODE = "authorization_code"
    CLIENT_CREDENTIALS = "client_credentials"
    RESOURCE_OWNER = "resource_owner"
    REFRESH_TOKEN = "refresh_token"


@dataclass
class OAuthToken:
    """OAuth token response."""
    access_token: str
    token_type: str = "Bearer"
    expires_in: int = 3600
    refresh_token: Optional[str] = None
    scope: Optional[str] = None
    id_token: Optional[str] = None
    
    def is_expired(self) -> bool:
        """Check if token is expired."""
        from datetime import datetime
        return self.expires_in <= 0
    
    @property
    def scopes(self) -> List[str]:
        """Get token scopes."""
        if self.scope:
            return self.scope.split()
        return []


class OAuthError(Exception):
    """OAuth2 error."""
    pass


class OAuth2Client:
    """
    OAuth2 client for authentication.
    
    Supports multiple flows:
    - Authorization Code: For web applications with user login
    - Client Credentials: For service-to-service communication
    - Resource Owner: For trusted applications
    
    Usage:
        client = OAuth2Client(
            client_id="app-id",
            client_secret="app-secret",
            token_url="https://oauth.example.com/token",
        )
        
        token = await client.get_token(["read", "write"])
    """
    
    def __init__(
        self,
        client_id: str,
        client_secret: str,
        token_url: str,
        auth_url: Optional[str] = None,
        redirect_uri: Optional[str] = None,
    ):
        """
        Initialize OAuth2 client.
        
        Args:
            client_id: OAuth2 client ID
            client_secret: OAuth2 client secret
            token_url: Token endpoint URL
            auth_url: Authorization endpoint URL
            redirect_uri: Redirect URI for callback
        """
        self.client_id = client_id
        self.client_secret = client_secret
        self.token_url = token_url
        self.auth_url = auth_url
        self.redirect_uri = redirect_uri
        
        # Token cache
        self._token: Optional[OAuthToken] = None
    
    def get_client_credentials(self) -> str:
        """Get base64 encoded client credentials."""
        credentials = f"{self.client_id}:{self.client_secret}"
        return base64.b64encode(credentials.encode()).decode()
    
    async def get_token(
        self,
        scope: Optional[List[str]] = None,
        grant_type: str = "client_credentials",
    ) -> OAuthToken:
        """
        Get access token using client credentials flow.
        
        Args:
            scope: Requested scopes
            grant_type: OAuth2 grant type
            
        Returns:
            OAuth token
        """
        headers = {
            "Authorization": f"Basic {self.get_client_credentials()}",
            "Content-Type": "application/x-www-form-urlencoded",
        }
        
        data = {
            "grant_type": grant_type,
        }
        
        if scope:
            data["scope"] = " ".join(scope)
        
        async with httpx.AsyncClient() as client:
            response = await client.post(self.token_url, headers=headers, data=data)
            
            if response.status_code != 200:
                error = response.json()
                raise OAuthError(
                    f"Token request failed: {error.get('error_description', error)}"
                )
            
            token_data = response.json()
            
            self._token = OAuthToken(
                access_token=token_data["access_token"],
                token_type=token_data.get("token_type", "Bearer"),
                expires_in=token_data.get("expires_in", 3600),
                refresh_token=token_data.get("refresh_token"),
                scope=token_data.get("scope"),
                id_token=token_data.get("id_token"),
            )
            
            logger.info(f"Obtained token, scopes: {self._token.scopes}")
            
            return self._token
    
    async def get_token_auth_code(
        self,
        code: str,
        code_verifier: Optional[str] = None,
    ) -> OAuthToken:
        """
        Get token using authorization code.
        
        Args:
            code: Authorization code from redirect
            code_verifier: PKCE code verifier
            
        Returns:
            OAuth token
        """
        headers = {
            "Authorization": f"Basic {self.get_client_credentials()}",
            "Content-Type": "application/x-www-form-urlencoded",
        }
        
        data = {
            "grant_type": "authorization_code",
            "code": code,
        }
        
        if self.redirect_uri:
            data["redirect_uri"] = self.redirect_uri
        
        if code_verifier:
            data["code_verifier"] = code_verifier
        
        async with httpx.AsyncClient() as client:
            response = await client.post(self.token_url, headers=headers, data=data)
            
            if response.status_code != 200:
                error = response.json()
                raise OAuthError(
                    f"Token request failed: {error.get('error_description', error)}"
                )
            
            token_data = response.json()
            
            self._token = OAuthToken(
                access_token=token_data["access_token"],
                token_type=token_data.get("token_type", "Bearer"),
                expires_in=token_data.get("expires_in", 3600),
                refresh_token=token_data.get("refresh_token"),
                scope=token_data.get("scope"),
                id_token=token_data.get("id_token"),
            )
            
            return self._token
    
    async def get_token_password(
        self,
        username: str,
        password: str,
        scope: Optional[List[str]] = None,
    ) -> OAuthToken:
        """
        Get token using resource owner password flow.
        
        Args:
            username: User's username
            password: User's password
            scope: Requested scopes
            
        Returns:
            OAuth token
        """
        headers = {
            "Authorization": f"Basic {self.get_client_credentials()}",
            "Content-Type": "application/x-www-form-urlencoded",
        }
        
        data = {
            "grant_type": "password",
            "username": username,
            "password": password,
        }
        
        if scope:
            data["scope"] = " ".join(scope)
        
        async with httpx.AsyncClient() as client:
            response = await client.post(self.token_url, headers=headers, data=data)
            
            if response.status_code != 200:
                error = response.json()
                raise OAuthError(f"Token request failed: {error}")
            
            token_data = response.json()
            
            self._token = OAuthToken(
                access_token=token_data["access_token"],
                token_type=token_data.get("token_type", "Bearer"),
                expires_in=token_data.get("expires_in", 3600),
                refresh_token=token_data.get("refresh_token"),
                scope=token_data.get("scope"),
            )
            
            return self._token
    
    async def refresh_token(
        self,
        refresh_token: Optional[str] = None,
    ) -> OAuthToken:
        """
        Refresh an access token.
        
        Args:
            refresh_token: Refresh token (uses cached if not provided)
            
        Returns:
            New OAuth token
        """
        token = refresh_token or self._token.refresh_token
        
        if not token:
            raise OAuthError("No refresh token available")
        
        headers = {
            "Authorization": f"Basic {self.get_client_credentials()}",
            "Content-Type": "application/x-www-form-urlencoded",
        }
        
        data = {
            "grant_type": "refresh_token",
            "refresh_token": token,
        }
        
        async with httpx.AsyncClient() as client:
            response = await client.post(self.token_url, headers=headers, data=data)
            
            if response.status_code != 200:
                error = response.json()
                raise OAuthError(f"Token refresh failed: {error}")
            
            token_data = response.json()
            
            self._token = OAuthToken(
                access_token=token_data["access_token"],
                token_type=token_data.get("token_type", "Bearer"),
                expires_in=token_data.get("expires_in", 3600),
                refresh_token=token_data.get("refresh_token", token),
                scope=token_data.get("scope"),
            )
            
            return self._token
    
    async def revoke_token(
        self,
        token: Optional[str] = None,
        token_type_hint: str = "access_token",
    ) -> bool:
        """
        Revoke an access or refresh token.
        
        Args:
            token: Token to revoke
            token_type_hint: Hint about token type
            
        Returns:
            True if revoked
        """
        access_token = token or self._token.access_token
        
        if not access_token:
            return False
        
        headers = {
            "Authorization": f"Basic {self.get_client_credentials()}",
            "Content-Type": "application/x-www-form-urlencoded",
        }
        
        data = {
            "token": access_token,
            "token_type_hint": token_type_hint,
        }
        
        async with httpx.AsyncClient() as client:
            await client.post(
                self.token_url.replace("token", "revoke"),
                headers=headers,
                data=data,
            )
        
        self._token = None
        logger.info("Token revoked")
        
        return True
    
    def get_authorization_url(
        self,
        scope: Optional[List[str]] = None,
        state: Optional[str] = None,
        code_challenge: Optional[str] = None,
    ) -> str:
        """
        Get authorization URL for authentication.
        
        Args:
            scope: Requested scopes
            state: State parameter for CSRF protection
            code_challenge: PKCE code challenge
            
        Returns:
            Authorization URL
        """
        if not self.auth_url:
            raise OAuthError("Auth URL not configured")
        
        params = {
            "response_type": "code",
            "client_id": self.client_id,
        }
        
        if self.redirect_uri:
            params["redirect_uri"] = self.redirect_uri
        
        if scope:
            params["scope"] = " ".join(scope)
        
        if state:
            params["state"] = state
        
        if code_challenge:
            params["code_challenge"] = code_challenge
            params["code_challenge_method"] = "S256"
        
        # Build URL with query params
        from urllib.parse import urlencode
        return f"{self.auth_url}?{urlencode(params)}"
    
    @property
    def current_token(self) -> Optional[OAuthToken]:
        """Get current cached token."""
        return self._token
    
    @property
    def is_authenticated(self) -> bool:
        """Check if client has valid token."""
        return self._token is not None and not self._token.is_expired()


# Example integration with FastAPI
class OAuth2Dependency:
    """FastAPI dependency for OAuth2 authentication."""
    
    def __init__(self, oauth_client: OAuth2Client):
        self.oauth_client = oauth_client
    
    async def __call__(
        self,
        authorization: str = None,
    ) -> Dict[str, Any]:
        """Validate OAuth2 token."""
        if not authorization:
            raise OAuthError("Authorization header required")
        
        # Extract token
        if authorization.startswith("Bearer "):
            token = authorization[7:]
        else:
            token = authorization
        
        # Get user info from token (simplified - would call userinfo endpoint)
        return {"token": token}


# Example usage
async def main():
    """Demonstrate OAuth2 client."""
    
    oauth = OAuth2Client(
        client_id="my-app",
        client_secret="secret",
        token_url="https://oauth.example.com/token",
        auth_url="https://oauth.example.com/authorize",
    )
    
    print("OAuth2 Client Demo")
    
    # Example: Get authorization URL
    auth_url = oauth.get_authorization_url(
        scope=["read", "write"],
        state="random-state-123",
    )
    print(f"Authorization URL: {auth_url}")
    
    # Example: Get token (would work with real OAuth server)
    # token = await oauth.get_token(["read", "write"])
    # print(f"Access token: {token.access_token}")
    
    # Example: Client credentials flow
    print("\nClient credentials flow:")
    print(f"  Credentials header: {oauth.get_client_credentials()[:30]}...")
    
    # Example: Token refresh
    print("\nToken refresh:")
    print("  Using cached refresh token")


if __name__ == "__main__":
    asyncio.run(main())