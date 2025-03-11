# PHIRedactApplication

This solution includes:

1. **PHIRedactWebApp (Web API)** – A .NET 8 Web API for handling redacted lab orders.
2. **PHIRedactApp (Azure Function)** – An Azure Function for processing lab order redaction requests.
3. **PHIRedactAppUnitTests (Unit Tests)** – xUnit tests associated with testing the azure function in this solution.
## 📌 Prerequisites

Ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Azure Functions Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools)
- [Azure CLI (Optional)](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/) (Optional, for testing APIs)
- [Docker](https://www.docker.com/) (Optional, if running containers)

---

## 🚀 **Running the Web API (PHIRedactApp)**

### 1️⃣ **Clone the Repository**

**git clone https://github.com/longmatthew/PHIRedactApplication.git**

## **Set Up Configuration**

Before running the application, configure your environment variables in the local.settings.json file in the directory PHIRedactApp.

Steps to run azure function locally:
1. **Azure Function Storage Emulator**  
   If running the function locally, ensure that the Azure Storage Emulator is running or use an actual Azure Storage account.
   
2. Change the directory:
   cd PHIRedactApplication
   cd PHIRedactApp

3. Run dotnet build
   
4. Then run the function: func start

If successful, you should see output similar to:
```
[2025-03-10T12:00:00Z] Host started (Node: 16, .NET: 8.0)
[2025-03-10T12:00:05Z] Functions:
[2025-03-10T12:00:05Z]   PHIRedactApp: [GET, POST] http://localhost:7215/api/PHIRedactApp
```
