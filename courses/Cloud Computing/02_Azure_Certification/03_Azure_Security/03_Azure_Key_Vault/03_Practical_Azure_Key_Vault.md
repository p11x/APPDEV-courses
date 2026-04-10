---
Category: Azure Certification
Subcategory: Azure Security
Concept: Azure Key Vault
Purpose: Practical Azure Key Vault for managing secrets
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Key_Vault.md, 02_Advanced_Azure_Key_Vault.md
RelatedFiles: 01_Basic_Azure_Key_Vault.md, 02_Advanced_Azure_Key_Vault.md
UseCase: Managing secrets
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

# Azure Key Vault - Practical

## Creating a Key Vault

### Prerequisites

Before creating a Key Vault, ensure you have the necessary Azure subscription access and a resource group to contain the vault. Verify your Azure CLI authentication and confirm the subscription containing your target resource group.

```bash
az login
az account show
az group show --name "myResourceGroup"
```

If no resource group exists, create one in your target region:

```bash
az group create --name "myResourceGroup" --location "eastus"
```

The location determines where vault data is stored geographically. Select regions minimizing latency for your applications and satisfying data residency requirements. Key Vault availability zones in selected regions provide additional redundancy.

### Creating the Vault

Creating a Key Vault requires specifying unique names, resource group, location, and pricing tier:

```bash
az keyvault create \
  --name "myProductionVault" \
  --resource-group "myResourceGroup" \
  --location "eastus" \
  --sku "standard" \
  --enabled-for-deployment false \
  --enabled-for-disk-encryption false \
  --enabled-for-template-deployment false \
  --enable-soft-delete true \
  --soft-delete-retention-days 90
```

The vault name must be globally unique (3-24 characters, alphanumeric and hyphens). DNS entries are created in vault.azure.net, so names must be unique across all Azure subscriptions globally. The enabled flags control Azure service integration options.

Soft delete should absolutely be enabled for any production vault. The retention period of 90 days provides adequate recovery window if secrets are accidentally deleted. While newer vaults default to soft delete enabled, always verify explicitly in your configurations.

### Verification

Confirm vault creation and review initial configuration:

```bash
az keyvault show \
  --name "myProductionVault" \
  --resource-group "myResourceGroup"
```

The output confirms vault properties including SKU, location, soft delete status, and creation timestamp. Verify soft delete and purge protection are enabled. The vault URI (output.vaultUri) is required for application configuration—the endpoint always uses HTTPS.

## Storing Secrets

### Creating Secrets

Store your first secret—typically a database connection string or API key—in the vault:

```bash
az keyvault secret set \
  --vault-name "myProductionVault" \
  --name "DBConnectionString" \
  --value "Server=my.database.windows.net;Database=myapp;User=admin;Password=SecurePassword123"
```

Secret values accept any UTF-8 string. For database connection strings, generate secure passwords rather than using memorable phrases. Azure Key Vault accepts secrets up to 25KB—more than sufficient for standard connection strings and API keys.

The secret immediately becomes retrievable by authorized principals. The create operation succeeds even if another version of the same name exists—Azure Key Vault maintains version history automatically. Each secret creation produces a new version GUID.

### Retrieving Secrets

Retrieve secret values using the show command:

```bash
az keyvault secret show \
  --vault-name "myProductionVault" \
  --name "DBConnectionString"
```

The output includes the secret value (value), version (id), creation date, and attributes. For application integration, the value field contains the secret plaintext. Applications typically parse JSON output to extract the value for use.

To retrieve a specific secret version, include the version identifier:

```bash
az keyvault secret show \
  --vault-name "myProductionVault" \
  --name "DBConnectionString" \
  --version "version-guid-here"
```

Listing all secret versions helps identify when secrets were rotated:

```bash
az keyvault secret list-versions \
  --vault-name "myProductionVault" \
  --name "DBConnectionString"
```

### Secret Attributes

Secret attributes enable time-based access control and automatic expiration:

```bash
az keyvault secret set \
  --vault-name "myProductionVault" \
  --name "TempApiKey" \
  --value "temporary-api-key-value" \
  --expires "2025-12-31T23:59:59Z" \
  --not-before "2025-01-01T00:00:00Z"
```

The expires attribute marks secret expiration after the specified datetime. Applications should check expiration before use (the Key Vault API does enforce expiration). The not-before attribute activates the secret only after the specified time, enabling scheduled secret availability.

Content type attributes help applications parse secrets correctly:

```bash
az keyvault secret set \
  --vault-name "myProductionVault" \
  --name "CertificatePassword" \
  --value "cert-password" \
  --content-type "password"
```

Setting content type enables applications to deserialize secret values appropriately. JSON content type indicates the secret is a JSON-formatted string requiring parsing.

## Managing Keys

### Creating Keys

Generate cryptographic keys within the vault:

```bash
az keyvault key create \
  --vault-name "myProductionVault" \
  --name "EncryptionKey" \
  --kty "RSA" \
  --size 2048 \
  --ops "encrypt" "decrypt" \
  --expires "2026-12-31T00:00:00Z"
```

RSA key types provide asymmetric encryption suitable for key encryption scenarios. Elliptic Curve keys provide shorter key lengths for equivalent security. The operations flag restricts cryptographic use—applications can only perform permitted operations even if they possess key access.

Key tags enable organizational metadata:

```bash
az keyvault key create \
  --vault-name "myProductionVault" \
  --name "SigningKey" \
  --kty "EC" \
  --curve "P-256" \
  --tags "Environment=Production" "Department=Finance"
```

Organization-specific tags help identify key ownership, environment, and purpose. Inventory scripts query tags for governance and reporting.

### Importing Keys

Import keys generated externally:

```bash
az keyvault key import \
  --vault-name "myProductionVault" \
  --name "ImportedKey" \
  --pem-file "./private-key.pem" \
  --password "import-password"
```

External key generation enables organizations to generate keys in secure environments separate from Azure. Some compliance requirements mandate that keys never exist outside hardware security modules—the import process supports this requirement.

PFX (PKCS#12) files contain both the private key and certificate for TLS scenarios:

```bash
az keyvault key import \
  --vault-name "myProductionVault" \
  --name "TLSCertKey" \
  --pfx-file "./certificate.pfx" \
  --password "pfx-password"
```

Imported keys become subject to all Key Vault access policies and audit logging. The vault cannot export imported keys in plaintext—key material remains protected within the vault.

### Key Operations

Applications use keys through SDKs. CLI commands help validate configurations.

```bash
az keyvault key show --vault-name "myProductionVault" --name "EncryptionKey"
```

## Access Policies

Configure access policies for applications:

```bash
az keyvault set-policy \
  --name "myProductionVault" \
  --object-id "azure-ad-object-id" \
  --secret-permissions get list \
  --key-permissions get list
```

Grant minimum permissions required—read-only for production applications. Additional permissions for automation accounts requiring secret creation include "set" and "delete".

az keyvault set-policy \
  --name "myProductionVault" \
  --object-id "app2-object-id" \
  --secret-permissions get list set \
  --key-permissions get list
```

Each principal receives policy reflecting its required access. Review policies periodically to remove unnecessary permissions. Automated applications should have separate identities from interactive applications.

## Integrating with App Service

### Enable Managed Identity

Enable system-assigned managed identity for your App Service:

```bash
az webapp identity assign \
  --name "myAppService" \
  --resource-group "myResourceGroup"
```

The output includes the object ID needed for Key Vault access policy. System-assigned identities automatically rotate credentials and persist for the application lifetime. User-assigned identities support scenarios where identities must survive application recreation.

The managed identity appears in Azure AD with the same object ID returned above—grant access to it like any other principal.

### Grant Key Vault Access

Grant the application access to retrieve secrets:

```bash
az keyvault set-policy \
  --name "myProductionVault" \
  --object-id "app-service-identity-object-id" \
  --secret-permissions get \
  --key-permissions get
```

The object ID from the identity assignment step enables App Service access to retrieve secrets. Verify the identity is the correct principal type—the system-assigned identity appears under "Enterprise Applications" in Azure AD with "System Assigned" type.

### Application Configuration

Configure the application to retrieve secrets from Key Vault:

```bash
az webapp config appsettings set \
  --name "myAppService" \
  --resource-group "myResourceGroup" \
  --settings "DBConnection=@Microsoft.KeyVault(SecretUri=https://myProductionVault.vault.azure.net/secrets/DBConnectionString/)"
```

The Key Vault reference syntax retrieves the secret at application startup. The application references configuration settings normally—the Key Vault integration happens transparently. No code changes required.

The SecretUri must include the vault name and secret name exactly. Version specification is optional—omitting the version retrieves the latest. This approach enables secret rotation without application changes.

Test the integration by checking application settings in Azure portal—they should display "KeyVault reference <key-vault-name>" in the value field until application startup retrieves the actual value.

## Using Key Vault References

Configure application settings with Key Vault references:

```bash
az webapp config appsettings set \
  --name "myAppService" \
  --resource-group "myResourceGroup" \
  --settings "ApiKey=@Microsoft.KeyVault(SecretUri=https://myProductionVault.vault.azure.net/secrets/ApiKey/)"
```

Connection strings use the double-underscore notation (ConnectionStrings__) for dotnet-style hierarchical keys. Azure Functions supports the same reference syntax through application settings.

### Validation Testing

Verify Key Vault references resolve correctly before deployment:

```bash
az webapp config appsettings show \
  --name "myAppService" \
  --resource-group "myResourceGroup"
```

The output shows current settings and their sources. Key Vault references without errors indicate successful resolution. Errors indicate authentication failures, missing secrets, or network connectivity issues.

Test integration in development before production deployment. Local debugging does not automatically resolve Key Vault references—set local placeholder values for development.

## Secret Rotation

### Manual Rotation

Rotate secrets by creating new versions. Use the same secret name with a new value:

```bash
az keyvault secret set --vault-name "myProductionVault" --name "DBPassword" --value "new-password"
```

Applications referencing "latest" automatically retrieve the new value. Cached values require application restart.

### Automated Rotation

Timer-triggered Azure Functions can automate rotation. The function retrieves current secrets, generates new values, writes to Key Vault, and updates dependent systems.

## Disaster Recovery

### Backup

Export vault contents to Azure Storage for disaster recovery. Backups include all secrets, keys, and certificates but not access policies.

```bash
az keyvault backup start --vault-name "myProductionVault" --blob-container-url "https://storage.blob.core.windows.net/backup"
```

### Recovery

Recover deleted vaults within the retention period:

```bash
az keyvault recover --name "myDeletedVault" --resource-group "myResourceGroup"
```

Restoration returns all vault objects. Access policies require separate recreation.

## Summary

Creating vaults and storing secrets provides the foundation for Azure Key Vault secret management. Access policies control authorization. App Service integration enables applications to retrieve secrets without storing credentials in configuration.

Key rotation maintains secret security over time. Disaster recovery planning ensures business continuity. The practical patterns covered here address the most common enterprise secret management scenarios.

Azure Key Vault integrates with the Azure platform to provide comprehensive, secure secret management across your application portfolio.