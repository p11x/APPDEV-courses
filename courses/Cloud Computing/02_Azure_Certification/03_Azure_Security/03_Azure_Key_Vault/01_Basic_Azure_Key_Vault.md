---
Category: Azure Certification
Subcategory: Azure Security
Concept: Azure Key Vault
Purpose: Understanding Azure Key Vault for secret management
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Key_Vault.md, 03_Practical_Azure_Key_Vault.md
UseCase: Secret management
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

# Azure Key Vault - Fundamentals

## Introduction to Azure Key Vault

Azure Key Vault is a cloud-based secret management service that provides a secure store for secrets, keys, and certificates. It serves as a centralized repository for sensitive information in Azure deployments, eliminating the need to embed credentials directly in application code. Key Vault enables organizations to protect cryptographic keys and other secrets while maintaining easy access for authorized applications and services.

The service addresses a fundamental challenge in cloud computing: securely storing and managing sensitive configuration data. Application developers traditionally face the temptation to store passwords, connection strings, and API keys in configuration files or code repositories. This practice creates significant security vulnerabilities, as these files may be accidentally committed to version control or accessed by unauthorized personnel. Azure Key Vault provides a centralized solution that separates sensitive data from application code while maintaining seamless integration with Azure services.

Azure Key Vault operates as a hardware security module (HSM) backed service in Microsoft's Azure cloud infrastructure. The service implements industry-standard encryption and access controls to protect stored secrets. Microsoft manages the underlying infrastructure, handling key rotation, backup, and disaster recovery, allowing organizations to focus on using the service rather than maintaining the security infrastructure.

## Key Vault Concepts

### Keys

Cryptographic keys in Azure Key Vault support multiple key types and sizes for various encryption scenarios. The service supports RSA keys with 2048, 3072, and 4096-bit sizes, along with EC (Elliptic Curve) keys including P-256, P-384, and P-521 curves. Each key can be generated within the vault or imported from external HSM devices or software cryptographic solutions.

Keys stored in Key Vault can encrypt data at rest using Azure storage encryption. The vault manages key lifecycle operations including key rotation, version control, and scheduled expiration. Applications retrieve keys programmatically using the Key Vault API, eliminating the need to store private keys in application configuration or code.

The service maintains complete audit logs of all key operations, including creation, usage, and deletion. Organizations can integrate these logs with Azure Monitor and security information and event management (SIEM) tools for compliance and security monitoring. Key versions allow applications to reference specific key versions while enabling automatic key rotation without code changes.

### Secrets

Secrets in Key Vault include any sensitive string values that applications require, such as database connection strings, API keys, passwords, and storage account keys. The vault stores secrets as versioned objects, enabling applications to reference specific versions while new versions can be created without affecting existing integrations.

Secret attributes include enabled flags, expiration dates, and not-before dates for time-based access control. These attributes enable scenarios such as scheduled secret rotation and time-limited access for maintenance windows. The vault supports secret tags for categorization and metadata, facilitating organization and discovery in large secret deployments.

Applications authenticate to Key Vault using Azure Active Directory identities, eliminating the need to store credentials in application code. The vault returns secrets over HTTPS, ensuring transport encryption for all secret retrieval operations. This architecture prevents secrets from appearing in logs, traces, or error messages where they might be exposed to unauthorized parties.

### Certificates

Azure Key Vault integrates certificate management with public key infrastructure (PKI) operations. The service can generate certificate signing requests (CSRs) for external certificate authorities (CAs), or create self-signed certificates for development and testing purposes. Certificate lifecycle management includes automated renewal based on configurable expiration periods.

The vault supports multiple certificate formats including PFX and PEM for import operations. Stored certificates include the full certificate chain, private keys (if exported), and associated metadata. Certificate properties include subject name, issuer, validity period, and certificate policy definitions.

Key Vault certificates integrate with Azure services for TLS/SSL termination and mutual authentication scenarios. Services such as Azure App Service, API Management, and Azure Application Gateway can retrieve certificates directly from Key Vault, eliminating manual certificate deployment and enabling automated certificate renewal.

## Vault Types

### Standard Tier

The Standard tier provides software-protected keys using Azure-managed HSM infrastructure. Keys in the Standard tier are encrypted using Azure Storage encryption with customer-managed keys optional for additional protection. This tier supports all Key Vault operations including key generation, secret storage, and certificate management.

Standard tier vaults include soft delete functionality with 90-day retention for recovered secrets. The tier supports Azure AD authentication and role-based access control (RBAC) for vault access management. Network firewall rules provide basic network isolation, restricting access to specific IP ranges and virtual networks.

The Standard tier suits most production workloads that do not require dedicated HSM protection. Microsoft manages the cryptographic operations using FIPS 140-2 validated HSMs in Azure infrastructure. This tier provides substantial security guarantees while minimizing operational complexity and cost.

### Premium Tier

The Premium tier adds HSM-protected keys using dedicated Hardware Security Module (HSM) devices. Keys in this tier never leave the HSM boundaries in plaintext, providing the highest level of cryptographic protection. Each vault in the Premium tier includes dedicated HSM partitions for exclusive customer key protection.

Premium tier vaults support key sizes up to 4096-bit RSA and support additional Elliptic Curve types. The tier includes enhanced audit logging with customer-controlled log retention and optional log export to customer storage accounts. Purge protection prevents permanent deletion of deleted vaults and secrets.

The Premium tier addresses regulatory compliance requirements that mandate dedicated cryptographic modules. Industries such as financial services, healthcare, and government often require proof of key isolation from shared infrastructure. The Premium tier provides this isolation along with compliance documentation for various regulatory frameworks.

## Azure CLI Commands

### Creating a Key Vault

The Azure CLI provides commands for creating and managing Key Vault resources. Creating a new vault requires specifying the vault name, resource group, location, and tier. The vault name must be globally unique across all Azure subscriptions since it creates a public DNS endpoint.

```bash
az keyvault create \
  --resource-group "myResourceGroup" \
  --name "myKeyVault" \
  --location "eastus" \
  --sku "standard"
```

The resource group must exist before creating the vault. The location parameter determines the Azure region where the vault and its data are stored. The sku parameter specifies either "standard" or "premium" tier. After creation, the vault initializes within seconds and becomes ready to receive secrets and keys.

### Managing Secrets

Secret operations use the az keyvault secret commands. The following command creates a new secret in the vault:

```bash
az keyvault secret set \
  --vault-name "myKeyVault" \
  --name "dbConnectionString" \
  --value "Server=myserver.database.windows.net;Database=mydb;User=admin;Password=SecretPassword123"
```

Retrieving secrets uses the secret show command, which returns the secret value in the output:

```bash
az keyvault secret show \
  --vault-name "myKeyVault" \
  --name "dbConnectionString"
```

Applications typically retrieve secrets using the Key Vault SDK rather than CLI commands. The CLI serves administrative operations while applications use programmatic APIs for runtime secret access.

### Managing Keys

Key management uses the az keyvault key commands. Creating an RSA key demonstrates key generation:

```bash
az keyvault key create \
  --vault-name "myKeyVault" \
  --name "myEncryptionKey" \
  --kty "RSA" \
  --size 2048
```

Key retrieval for cryptographic operations requires different commands that handle the key material appropriately. Applications typically use the Azure Identity SDK combined with the Key Vault client library for production key operations.

## Access Control

### Azure Active Directory Authentication

Key Vault authentication integrates with Azure Active Directory (Azure AD) for identity management. Applications authenticating to Key Vault must present an Azure AD token, which the vault validates before granting access. This integration eliminates the need for explicit vault credentials in application configuration.

Three authentication methods exist for Azure AD identities: managed identities for Azure resources, service principals with client secrets, and user identities for development scenarios. Managed identities provide the strongest security posture since Azure automatically manages the credential lifecycle and no credentials are stored in application configuration.

The Key Vault firewall provides network-level access control. When enabled, the vault only accepts connections from permitted IP addresses, virtual network subnets, or private endpoints. This defense-in-depth approach restricts access even if Azure AD authentication is somehow compromised.

### Access Policies

Vault-level access policies define granular permissions for principals. Each policy specifies permissions for keys, secrets, and certificates independently. Permissions include get, list, set, delete, backup, and restore operations for each type.

```bash
az keyvault policy set \
  --name "myAppPolicy" \
  --vault-name "myKeyVault" \
  --object-id "principal-object-id" \
  --secret-permissions get list \
  --key-permissions encrypt decrypt
```

The access policy model is distinct from Azure RBAC, though RBAC provides alternative authorization for vault management operations. Modern deployments typically prefer RBAC for its unified permission model across Azure resources, while access policies support legacy scenarios and specific vault-level permissions.

## Integration with Azure Services

### Azure App Service

Azure App Service applications retrieve secrets from Key Vault using managed identities. The application enables a system-assigned or user-assigned managed identity, then grants that identity appropriate access to the vault. The application uses the Azure Identity library to authenticate and retrieve secrets.

This integration eliminates connection strings and API keys from application settings. Secrets exist only in Key Vault and rotate independently of application deployment. The application code remains unchanged during secret rotation, simplifying operational management.

The Azure portal provides Key Vault reference syntax for App Service application settings. Settings configured as Key Vault references automatically retrieve secret values at application startup. This approach maintains secret isolation while providing seamless application configuration.

### Azure Functions

Azure Functions support Key Vault integration through the same mechanisms as App Service. Functions using the v2+ programming model can inject secrets as environment variables resolved from Key Vault references. Functions using the v3 model can directly reference Key Vault using the Azure Identity SDK.

Timer-triggered functions commonly perform secret rotation operations. The function authenticates to Key Vault using managed identity, retrieves current secrets, updates external resources, and writes new secret versions to the vault. This automation ensures secrets rotate according to organizational policies without manual intervention.

## Pricing Model

Azure Key Vault pricing depends on operations and storage consumption. Operations include creates, reads, lists, and deletes for keys, secrets, and certificates. Storage pricing applies to the total size of stored secrets, keys, and certificates. The Premium tier includes additional HSM operations that incur higher costs than Standard tier operations.

Transaction-based pricing suits most workloads with variable usage patterns. Organizations can estimate costs based on expected secret retrieval frequency. High-volume applications may benefit from caching strategies to reduce transaction counts. Azure Cost Management reports help track Key Vault spending against budgets.

## Security Best Practices

Organizations should implement comprehensive security practices for Key Vault deployments. Enable soft delete on all vaults regardless of tier. Configure network firewalls to restrict access to trusted networks. Use managed identities instead of storing credentials. Enable Azure Monitor logging for audit trails.

Password and secret rotation reduces exposure from credential compromise. Establish rotation schedules matching compliance requirements. Implement automated rotation for operational efficiency. Document rotation procedures and maintain runbooks.

Access review should occur regularly to remove unnecessary permissions. Quarterly access reviews identify departed employee access. Justify access based on current job responsibilities. Remove permissions no longer required.

## Azure Key Vault Use Cases

Key Vault solves several common operational challenges. Applications requiring database connections store credentials in Key Vault rather than configuration files. TLS certificates for web applications renew automatically through Key Vault integration. Encryption keys for data protection use Key Vault-generated keys without on-premises key management.

Development teams use Key Vault for local development by referencing secrets through the Azure Identity libraries. Production deployments reference the same secrets with different vault URLs per environment. This pattern enables consistent code across environments without credential changes.

## Summary

Azure Key Vault provides essential secret management capabilities for Azure deployments. The service securely stores keys, secrets, and certificates while integrating with Azure AD for authentication. Organizations choose between Standard and Premium tiers based on their encryption requirements and compliance obligations.

Applications retrieve secrets programmatically using the Key Vault API or SDK, eliminating credentials from configuration files. The service maintains audit logs and supports network isolation for security monitoring and defense. Key Vault integrates natively with Azure services including App Service, Functions, and virtual machines, providing comprehensive secret management across Azure workloads.