# API Key Pattern
  ## Overview
  API keys are simple, static credentials used to identify and authenticate applications, services, or users accessing APIs. Unlike OAuth 2.0 tokens or JWTs, API keys do not contain user context or expiration by default—they are typically long-lived credentials that identify the calling application rather than the end user. This simplicity makes API keys particularly suitable for server-to-server communication, microservices authentication, and rate-limiting purposes in distributed systems.
  
  The API key pattern has been a fundamental security mechanism in web services since the early days of web APIs. It works by generating a unique identifier (the API key) that is associated with an application or user account. When making API requests, the client includes this key, typically in an HTTP header or as a query parameter. The server validates the key against its database of registered keys and allows or denies access based on the key's status and associated permissions.
  
  One of the primary advantages of API keys is their simplicity of implementation and use. They do not require complex OAuth flows, token refresh mechanisms, or cryptographic validation. This makes them ideal for scenarios where the overhead of more complex authentication mechanisms is not justified, such as internal service communication or public APIs that need simple rate limiting. Additionally, API keys can be easily revoked and rotated without affecting users' primary credentials.
  
  However, API keys have significant security limitations that must be understood. Because they are static credentials, if compromised, they provide ongoing access until revoked. They also do not provide any information about the user context or authorization level beyond what is associated with the key itself. For these reasons, API keys should be used appropriately—in combination with other security mechanisms for sensitive operations, and with proper safeguards to detect and respond to compromise.
  
  ### Types of API Keys
  
  There are several types of API keys, each suited to different use cases. **Public API keys** are exposed in client-side code and are used to identify applications for rate limiting and analytics. These keys typically have very limited permissions and cannot access sensitive data. Examples include JavaScript API keys for web applications that need to make requests to public endpoints.
  
  **Private API keys** are kept confidential and used for server-side authentication. These keys typically have broader permissions and can access sensitive endpoints. They must never be exposed in client-side code or version control. Private keys should be stored securely and rotated periodically.
  
  **Secret keys** are used in combination with API keys to create a shared secret between the client and server. The client signs requests using the secret key, and the server validates the signature. This adds a layer of security beyond simple key-based identification by ensuring that requests cannot be forged even if the API key is intercepted.
  
  ### Key Management Lifecycle
  
  The API key lifecycle includes several phases: creation, distribution, rotation, and revocation. During creation, keys are generated using cryptographically secure random number generators and associated with appropriate permissions and rate limits. Distribution involves securely delivering the key to the authorized party—never through unencrypted channels. Rotation involves periodically replacing old keys with new ones to limit the exposure window if a key is compromised. Revocation is the process of invalidating a key when it is compromised, no longer needed, or when a user leaves the organization.
  
  ```mermaid
  sequenceDiagram
      participant Client as API Client
      participant API as API Gateway
      participant Auth as Auth Service
      participant DB as Key Database

      Note over Client: 1. Generate or retrieve API key
      Client->>Auth: 2. Request API key (app_id, purpose)
      Auth->>DB: 3. Store new key with permissions
      DB-->>Auth: 4. Confirm storage
      Auth-->>Client: 5. Return API Key
      
      Note over Client: 6. Make API request with key
      Client->>API: 7. GET /api/resource (X-API-Key: key_abc123)
      
      Note over API: 8. Validate API key
      API->>DB: 9. Lookup key and permissions
      DB-->>API: 10. Return key info (active, rate_limit, scopes)
      
      alt Key Valid and Active
          API->>API: 11. Check rate limit
          API->>Client: 12. Return Resource (200 OK)
      else Key Invalid or Expired
          API->>Client: 12. Return Error (401 Unauthorized)
      end
      
      Note over Auth: 13. Rotate key (scheduled or on-demand)
      Auth->>DB: 14. Generate new key, mark old as rotating
      DB-->>Auth: 15. Confirm rotation
      Auth-->>Client: 16. Notify new key (secure channel)
  ```
  
  ## Standard Example
  
  The following implementation provides a complete API key management system including key generation, validation, rate limiting, and automatic rotation. This system is designed for microservices environments where API keys are used for service-to-service authentication and access control.
  
  ```javascript
  const crypto = require('crypto');
  const express = require('express');
  const rateLimit = require('express-rate-limit');
  
  const app = express();
  app.use(express.json());
  
  const apiKeyStore = new Map();
  const rateLimitStore = new Map();
  
  const config = {
      keyPrefix: 'sk_live_',
      keyLength: 32,
      secretLength: 64,
      rotationPeriod: 90 * 24 * 60 * 60 * 1000,
      maxKeysPerApp: 5,
      defaultRateLimit: { windowMs: 60000, max: 100 },
  };
  
  function generateSecureKey() {
      const bytes = crypto.randomBytes(config.keyLength);
      return config.keyPrefix + bytes.toString('hex');
  }
  
  function generateSecret() {
      const bytes = crypto.randomBytes(config.secretLength);
      return bytes.toString('hex');
  }
  
  function hashKey(key) {
      return crypto.createHash('sha256').update(key).digest('hex');
  }
  
  function createApiKey(appId, options = {}) {
      const existingKeys = [...apiKeyStore.values()].filter(k => k.appId === appId);
      
      if (existingKeys.length >= config.maxKeysPerApp) {
          throw new Error(`Maximum number of API keys reached for app: ${appId}`);
      }
  
      const now = Date.now();
      const key = generateSecureKey();
      const keyHash = hashKey(key);
      const secret = generateSecret();
  
      const apiKeyData = {
          id: crypto.randomUUID(),
          appId: appId,
          keyHash: keyHash,
          keyPrefix: key.substring(0, 12) + '...',
          secretHash: hashKey(secret),
          name: options.name || 'Default',
          permissions: options.permissions || ['read'],
          rateLimit: options.rateLimit || config.defaultRateLimit,
          createdAt: now,
          lastUsedAt: null,
          expiresAt: options.expiresAt || (now + config.rotationPeriod),
          rotatedAt: null,
          rotatedFrom: null,
          active: true,
          metadata: options.metadata || {},
      };
  
      apiKeyStore.set(keyHash, apiKeyData);
  
      return {
          id: apiKeyData.id,
          key: key,
          secret: secret,
          name: apiKeyData.name,
          permissions: apiKeyData.permissions,
          expiresAt: apiKeyData.expiresAt,
      };
  }
  
  function validateApiKey(key, secret = null) {
      if (!key) {
          return { valid: false, reason: 'API key is required' };
      }
  
      const keyHash = hashKey(key);
      const keyData = apiKeyStore.get(keyHash);
  
      if (!keyData) {
          return { valid: false, reason: 'Invalid API key' };
      }
  
      if (!keyData.active) {
          return { valid: false, reason: 'API key has been deactivated' };
      }
  
      if (keyData.expiresAt && Date.now() > keyData.expiresAt) {
          return { valid: false, reason: 'API key has expired' };
      }
  
      if (secret) {
          const secretHash = hashKey(secret);
          if (secretHash !== keyData.secretHash) {
              return { valid: false, reason: 'Invalid secret' };
          }
      }
  
      keyData.lastUsedAt = Date.now();
  
      return {
          valid: true,
          keyData: keyData,
          permissions: keyData.permissions,
          rateLimit: keyData.rateLimit,
      };
  }
  
  function rotateApiKey(keyId) {
      for (const [key, keyData] of apiKeyStore.entries()) {
          if (keyData.id === keyId) {
              const newKeyData = createApiKey(keyData.appId, {
                  name: keyData.name + ' (Rotated)',
                  permissions: keyData.permissions,
                  rateLimit: keyData.rateLimit,
                  expiresAt: Date.now() + config.rotationPeriod,
              });
  
              keyData.active = false;
              keyData.rotatedAt = Date.now();
              keyData.rotatedTo = newKeyData.id;
  
              return {
                  rotated: true,
                  oldKey: { id: keyData.id, active: false },
                  newKey: newKeyData,
              };
          }
      }
  
      return { rotated: false, reason: 'Key not found' };
  }
  
  function revokeApiKey(keyId) {
      for (const [key, keyData] of apiKeyStore.entries()) {
          if (keyData.id === keyId) {
              keyData.active = false;
              keyData.revokedAt = Date.now();
              return { revoked: true, keyId: keyId };
          }
      }
  
      return { revoked: false, reason: 'Key not found' };
  }
  
  function listApiKeys(appId) {
      const keys = [...apiKeyStore.values()]
          .filter(k => k.appId === appId)
          .map(k => ({
              id: k.id,
              name: k.name,
              keyPrefix: k.keyPrefix,
              permissions: k.permissions,
              createdAt: k.createdAt,
              lastUsedAt: k.lastUsedAt,
              expiresAt: k.expiresAt,
              active: k.active,
          }));
  
      return keys;
  }
  
  const apiKeyLimiter = rateLimit({
      keyGenerator: (req) => {
          const key = req.headers['x-api-key'];
          return key ? hashKey(key) : req.ip;
      },
      windowMs: 60000,
      max: 100,
      standardHeaders: true,
      legacyHeaders: false,
      handler: (req, res) => {
          res.status(429).json({
              error: 'Rate limit exceeded',
              retryAfter: 60,
          });
      },
  });
  
  function apiKeyAuthMiddleware(req, res, next) {
      const apiKey = req.headers['x-api-key'];
      const apiSecret = req.headers['x-api-secret'];
  
      if (!apiKey) {
          return res.status(401).json({ error: 'API key is required' });
      }
  
      const validation = validateApiKey(apiKey, apiSecret);
  
      if (!validation.valid) {
          return res.status(401).json({ error: validation.reason });
      }
  
      const windowMs = validation.rateLimit.windowMs || 60000;
      const maxRequests = validation.rateLimit.max || 100;
      const keyHash = hashKey(apiKey);
  
      const now = Date.now();
      const record = rateLimitStore.get(keyHash) || { count: 0, resetTime: now + windowMs };
  
      if (now > record.resetTime) {
          record.count = 0;
          record.resetTime = now + windowMs;
      }
  
      record.count++;
  
      if (record.count > maxRequests) {
          return res.status(429).json({
              error: 'Rate limit exceeded',
              retryAfter: Math.ceil((record.resetTime - now) / 1000),
          });
      }
  
      rateLimitStore.set(keyHash, record);
  
      req.apiKeyData = validation.keyData;
      req.rateLimitRemaining = maxRequests - record.count;
      res.set('X-RateLimit-Limit', maxRequests);
      res.set('X-RateLimit-Remaining', maxRequests - record.count);
      res.set('X-RateLimit-Reset', Math.ceil(record.resetTime / 1000));
  
      next();
  }
  
  app.post('/api/keys', (req, res) => {
      const { appId, name, permissions, rateLimit, expiresAt } = req.body;
  
      if (!appId) {
          return res.status(400).json({ error: 'appId is required' });
      }
  
      try {
          const keyData = createApiKey(appId, { name, permissions, rateLimit, expiresAt });
          res.status(201).json(keyData);
      } catch (error) {
          res.status(400).json({ error: error.message });
      }
  });
  
  app.get('/api/keys', apiKeyAuthMiddleware, (req, res) => {
      const keys = listApiKeys(req.apiKeyData.appId);
      res.json({ keys: keys });
  });
  
  app.post('/api/keys/:id/rotate', apiKeyAuthMiddleware, (req, res) => {
      const { id } = req.params;
      const result = rotateApiKey(id);
  
      if (result.rotated) {
          res.json(result);
      } else {
          res.status(404).json({ error: result.reason });
      }
  });
  
  app.post('/api/keys/:id/revoke', apiKeyAuthMiddleware, (req, res) => {
      const { id } = req.params;
      const result = revokeApiKey(id);
  
      if (result.revoked) {
          res.json({ success: true, message: 'API key revoked' });
      } else {
          res.status(404).json({ error: result.reason });
      }
  });
  
  app.get('/api/protected', apiKeyAuthMiddleware, (req, res) => {
      res.json({
          message: 'Access granted',
          apiKey: req.apiKeyData.name,
          permissions: req.apiKeyData.permissions,
      });
  });
  
  app.post('/api/sign-request', (req, body) => {
      const { keyId, secret, data } = body;
  
      const validation = validateApiKey(keyId);
      if (!validation.valid) {
          return res.status(401).json({ error: validation.reason });
      }
  
      const signature = crypto
          .createHmac('sha256', validation.keyData.secretHash)
          .update(JSON.stringify(data))
          .digest('hex');
  
      res.json({ signature });
  });
  
  module.exports = {
      createApiKey,
      validateApiKey,
      rotateApiKey,
      revokeApiKey,
      listApiKeys,
      apiKeyAuthMiddleware,
      hashKey,
  };
  ```
  
  ## Real-World Examples
  
  ### Auth0 API Key Implementation
  Auth0 provides API key management as part of their broader authentication platform. While Auth0 primarily uses OAuth 2.0 and OIDC, they also support API keys for legacy integrations and specific use cases. Their implementation includes key rotation, rate limiting, and fine-grained permissions.
  
  ```javascript
  const { ManagementClient } = require('auth0');
  
  const auth0Client = new ManagementClient({
      domain: process.env.AUTH0_DOMAIN || 'your-tenant.auth0. com',
      clientId: process.env.AUTH0_CLIENT_ID,
      clientSecret: process.env.AUTH0_CLIENT__SECRET,
      scope: 'create:client_grants read:client_grants delete:client_grants',
  });
  
  async function createAuth0ApiKey(clientId, scopes, label) {
      try {
          const clientGrant = await auth0Client.createClientGrant({
              client_id: clientId,
              audience: process.env.API_AUDIENCE,
              scope: scopes,
          });
  
          return {
              id: clientGrant.data.id,
              clientId: clientGrant.data.client_id,
              audience: clientGrant.data.audience,
              scope: clientGrant.data.scope,
              createdAt: clientGrant.data.created_at,
          };
      } catch (error) {
          console.error('Failed to create API key:', error);
          throw error;
      }
  }
  
  async function listAuth0ApiKeys(clientId) {
      try {
          const clientGrants = await auth0. client.getClientGrants({ client_id: clientId });
          return clientGrants.data;
      } catch (error) {
          console.error('Failed to list API keys:', error);
          throw error;
      }
  }
  
  async function rotateAuth0ApiKey(grantId) {
      await auth0Client.deleteClientGrant({ id: grantId });
      return { rotated: true, message: 'Old grant removed, create new one' };
  }
  
  async function validateAuth0ApiKey(validationToken) {
      const response = await fetch(`https://${process.env.AUTH0_DOMAIN}/userinfo`, {
          headers: {
              'Authorization': `Bearer ${validationToken}`,
          },
      });
  
      if (!response.ok) {
          return { valid: false };
      }
  
      return { valid: true, user: await response.json() };
  }
  
  module.exports = {
      createAuth0ApiKey,
      listAuth0ApiKeys,
      rotateAuth0ApiKey,
      validateAuth0ApiKey,
  };
  ```
  
  ### Okta API Key Implementation
  Okta provides API token management for administrative operations and supports OAuth 2.0 for API access. Their API tokens are used for Okta API calls and can be scoped to specific administrative roles. Okta's implementation includes comprehensive audit logging and token lifetime management.
  
  ```javascript
  const { Client } = require('@okta/okta-sdk-nodejs');
  
  const oktaClient = new Client({
      orgUrl: process.env.OKTA_ORG_URL || 'https://your-org..okta.com',
      token: process.env.OKTA_API_TOKEN,
      requestExecutor: {
          headers: {
              'User-Agent': 'my-app/1.0.0',
          },
      },
  });
  
  async function createOktaApiToken(name, expiresAt, permissions) {
      try {
          const token = await oktaClient.createApiToken({
              name: name,
              expiresAt: expiresAt,
              status: 'ACTIVE',
          });
  
          return {
              id: token.id,
              token: token.token,
              name: token.name,
              created: token.created,
              expires: token.expires,
          };
      } catch (error) {
          console.error('Failed to create Okta API token:', error);
          throw error;
      }
  }
  
  async function listOktaApiTokens() {
      try {
          const tokens = await oktaClient.listApiTokens();
          const tokenList = [];
  
          for await (const token of tokens) {
              tokenList.push({
                  id: token.id,
                  name: token.name,
                  created: token.created,
                  expires: token.expires,
                  lastUsed: token.lastUsed,
              });
          }
  
          return tokenList;
      } catch (error) {
          console.error('Failed to list Okta API tokens:', error);
          throw error;
      }
  }
  
  async function revokeOktaApiToken(tokenId) {
      try {
          await oktaClient.deleteApiToken({ id: tokenId });
          return { revoked: true };
      } catch (error) {
          console.error('Failed to revoke Okta API token:', error);
          throw error;
      }
  }
  
  function validateOktaToken(token) {
      return oktaClient.getOktaSignInSession(token)
          .then(session => ({ valid: true, session: session }))
          .catch(() => ({ valid: false }));
  }
  
  module.exports = {
      createOktaApiToken,
      listOktaApiTokens,
      revokeOktaApiToken,
      validateOktaToken,
  };
  ```
  
  ### Google Cloud API Key Implementation
  Google Cloud provides API keys with extensive configuration options including application restrictions (HTTP, Android, iOS), API restrictions, and quota settings. Google API keys are designed to work seamlessly with Google's API ecosystem while providing granular control over usage and security.
  
  ```javascript
  const {google} = require('googleapis');
  
  const apiKeysClient = new google.cloudresourcemanager('v3');
  const {ProjectsClient} = require('@google-cloud/resource-manager');
  
  const projectId = process.env.GOOGLE_PROJECT_ID;
  
  async function createGoogleApiKey(name, restrictions) {
      try {
          const parent = `projects/${projectId}`;
  
          const request = {
              parent: parent,
              description: name,
              restrictions: restrictions || {
                  apiTargets: [
                      {service: 'cloudresourcemanager.googleapis.com'}
                  ],
                  browserKeyRestrictions: {
                      allowedReferrers: ['*.example.com/*']
                  },
              },
              displayName: name,
          };
  
          const response = await apiKeysClient.projects.apiKeys.create(request);
  
          return {
              name: response.data.name,
              key: response.data.keyString,
              displayName: response.data.displayName,
          };
      } catch (error) {
          console.error('Failed to create Google API key:', error);
          throw error;
      }
  }
  
  async function getGoogleApiKey(keyId) {
      try {
          const request = { name: `projects/${projectId}/locations/global/keys/${keyId}` };
          const response = await apiKeysClient.projects.apiKeys.get(request);
  
          return {
              name: response.data.name,
              displayName: response.data.displayName,
              restrictions: response.data.restrictions,
              state: response.data.state,
              createTime: response.data.createTime,
          };
      } catch (error) {
          console.error('Failed to get Google API key:', error);
          throw error;
      }
  }
  
  async function restrictGoogleApiKey(keyId, restrictions) {
      try {
          const request = {
              name: `projects/${projectId}/locations/global/keys/${keyId}`,
              updateMask: 'restrictions',
              resource: {
                  restrictions: restrictions,
              },
          };
  
          const response = await apiKeysClient.projects.apiKeys.patch(request);
  
          return { updated: true, restrictions: response.data.restrictions };
      } catch (error) {
          console.error('Failed to restrict Google API key:', error);
          throw error;
      }
  }
  
  async function auditGoogleApiKeys() {
      const client = new ProjectsClient();
      const [project] = await client.getProject({ name: `projects/${projectId}` });
  
      const request = {
          parent: `projects/${projectId}/locations/-`,
      };
  
      const keys = [];
      const response = await apiKeysClient.projects.apiKeys.list(request);
  
      for (const key of response[0].keys || []) {
          keys.push({
              name: key.name,
              displayName: key.displayName,
              state: key.state,
              createTime: key.createTime,
          });
      }
  
      return keys;
  }
  
  module.exports = {
      createGoogleApiKey,
      getGoogleApiKey,
      restrictGoogleApiKey,
      auditGoogleApiKeys,
  };
  ```
  
  ## Output Statement
  
  The API key pattern provides a straightforward mechanism for application-level authentication in microservices architectures. While less sophisticated than OAuth 2.0 or JWT, API keys excel in scenarios requiring simple, performant authentication for server-to-server communication, public APIs, and rate limiting. The pattern's simplicity makes it easy to implement and manage, but security requires careful attention to key distribution, rotation, and revocation. Organizations should implement API keys alongside other security measures for sensitive operations and choose appropriate key types based on the use case—public keys for identification and rate limiting, secret keys for authenticated requests.
  
  ## Best Practices
  
  **Never Expose API Keys in Client-Side Code**: Public API keys are sometimes necessary for client-side applications, but they must be restricted to non-sensitive operations only. Never include private or secret API keys in JavaScript code, mobile app bundles, or anywhere they could be extracted by attackers. Use server-side proxies for any operations requiring sensitive keys.
  
  **Implement Key Rotation**: Rotate API keys periodically to limit the impact of potential key compromise. The rotation period depends on the sensitivity of the data accessed—more sensitive systems require more frequent rotation. Implement automated rotation with grace periods to allow clients to migrate to new keys without service interruption.
  
  **Use Application and API Restrictions**: Configure restrictions on API keys to limit their use to specific applications, IP addresses, referrers, or APIs. Google Cloud and other providers support these restrictions natively. This limits the damage if a key is compromised because attackers cannot use it from unauthorized locations or for unrestricted APIs.
  
  **Implement Comprehensive Logging**: Log all API key usage including creation, rotation, revocation, and access attempts. This enables security monitoring and incident response. Log sufficient context (IP address, user agent, timestamp) to support forensic analysis if keys are compromised.
  
  **Use Separate Keys for Different Environments**: Maintain separate API keys for development, staging, and production environments. This allows you to revoke development keys without affecting production systems and provides better isolation for security incidents. Different keys also enable different rate limits and restrictions per environment.
  
  **Implement Rate Limiting**: Configure rate limits per API key to prevent abuse and protect your services from excessive usage. Rate limiting should be based on the expected usage pattern for each key and should include both per-minute and per-day limits. Use separate limits for different types of operations if needed.
  
  **Secure Key Storage**: Store API keys securely using environment variables, secret management systems (HashiCorp Vault, AWS Secrets Manager), or encrypted configuration files. Never commit keys to version control, log files, or error messages. Use secrets scanning tools in CI/CD pipelines to detect accidentally committed keys.
  
  **Implement Key Expiration**: Set expiration dates for all API keys and implement processes for reviewing and rotating expiring keys. Keys without expiration dates provide ongoing access that can be forgotten and exploited. Automatic expiration provides defense in depth against forgotten keys.
  
  **Provide Secure Key Delivery**: Deliver API keys to users through secure channels. Email is generally not secure for key delivery. Use HTTPS for web-based key delivery, encrypted file formats for bulk key delivery, and consider one-time key reveal mechanisms where users must authenticate to view their keys.
  
  **Implement Revocation Capability**: Ensure you can quickly revoke compromised or misused API keys. Implement automated revocation triggers for suspicious patterns and maintain runbooks for emergency key revocation. Test your revocation process regularly to ensure it works when needed.
