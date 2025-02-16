param functionAppName string = 'cloudcommerce-func-app'
param resourceGroupName string = 'CloudCommerceRG'
param location string = 'eastus'
param storageAccountName string = 'cloudcommercefuncstorage'
param appServicePlanName string = 'cloudcommerce-plan'
param identityName string = 'cloudcommerce-identity'

// Azure AD Authentication Parameters
param aadTenantId string = '<your-tenant-id>'  // Replace with your Azure AD Tenant ID
param aadClientId string = '<your-aad-client-id>'  // Replace with your Azure AD App Client ID

// Create Resource Group (if not exists)
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: location
}

// Create Storage Account for Function App
resource storage 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  resourceGroup: resourceGroupName
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

// Create App Service Plan (Consumption Plan)
resource appPlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: appServicePlanName
  location: location
  resourceGroup: resourceGroupName
  kind: 'functionapp'
  sku: {
    tier: 'Dynamic'
    name: 'Y1' // Consumption Plan
  }
}

// Create Managed Identity
resource identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-01' = {
  name: identityName
  location: location
  resourceGroup: resourceGroupName
}

// Create Function App with Azure AD Authentication
resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: functionAppName
  location: location
  resourceGroup: resourceGroupName
  kind: 'functionapp'
  properties: {
    serverFarmId: appPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storage.properties.primaryEndpoints.blob
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'MicrosoftWebAppAuthentication__Enabled'
          value: 'true'
        }
        {
          name: 'MicrosoftWebAppAuthentication__ClientId'
          value: aadClientId
        }
        {
          name: 'MicrosoftWebAppAuthentication__Issuer'
          value: 'https://login.microsoftonline.com/${aadTenantId}/'
        }
        {
          name: 'MicrosoftWebAppAuthentication__AllowedAudiences'
          value: 'api://${aadClientId}'
        }
      ]
    }
    identity: {
      type: 'UserAssigned'
      userAssignedIdentities: {
        '${identity.id}': {}
      }
    }
  }
}
