# Publish System

# Introduction 
Design View Rendering and Encoding system. 

## Overview

![Overview Diagram](./docs/Publish-System-Architecture.png =800x)

## Flow

1. Users use the existing publish dialog in DesignView.
2. DesignView enqueues a publish request on the Publish API.
3. The Publish API process queued request and schedules an Azure Batch job.
4. The Publish API subscribes to the Azure Batch job queue to follow-up the status of.
the rendering. Azure Batch process the scheduled job and allocates it to one node.
5. The Azure Batch worker node downloads the plan information from DesignView.
6. The Azure Batrch worker node uses the command line to invoke Fusion 2020,
rendering the required media in the desired format/output.
7. Once the rendering is done, the output is uploaded to the publish storage in raw (no
streaming optimised) format.
8. The Publish API is notified that the rendering is done and it requests to Azure Media
Services the encoding of the raw video to a stream optimised video(s) format.
9. The Publish API loop-requests the status of the encoding to Azure Media Service
until the media is ready.
10. Azure Media Service allocates the encoding job to a node and the node access the
raw video format and starts the encoding from the blob storage.
11. Once the encoding is done, it is uploaded to the publish storage (Rackspace and
Azure Blob Storage).
12. The Publish API notifies the encoding is finalised. The status of the job is emailed.
(multilanguage support). The legacy Publish System database is updated for the
existing MyKitchen website to consume the information.
13. The video is ready to be visualised from MyKitchen and DesignView.


## Publish Request State flow

![Overview Diagram](./docs/PublishStateFlow.png =800x)


# Components

## Orchestrator (Azure Functions)

![Orchestrator Diagram](./docs/CodingComponents.drawio.png =800x)

| Component                           | Type                         | Description                                                  |
| ----------------------------------- | ---------------------------- | ------------------------------------------------------------ |
| Event bus                           | Azure Service Bus            | Message broker with message queues and publish-subscribe topics. |
| Event bus triggers                  | Azure Functions              | Cause an action to run when an event bus message is consumed. |
| Application layer                   | .NET class library           | Coordinating the actions to be performed.                    |
| Domain layer                        | .NET class library           | Persistence of the business state into the relational database. |
| Scripts of the relational database. | SQL Server database project. | Contains the database scripts of the relational database.    |
| Relational database.                | Azure SQL Database.          | Data storage layer based on the SQL Server database engine.  |


## DesignView Integration (DV API)

## Rendering (Azure Batch)

## Encoding (Azure Media Services)

## Publication (Azure Media Services + React + Storage)



# Getting Started


1.	Software installation
2.	Local database configuration
3.	Running the application
4.	Running the tests



## Software installation

| Software                                                     | Description                                            | Link                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------ | ------------------------------------------------------------ |
| Visual Studio 2022 Professional                              | IDE for *.NET*  to develop the orchestrator component. | https://visualstudio.microsoft.com/en/downloads/             |
| SQL Server Management Studio (or Azure Data Studio)          | Tool to manage the relational database.                | https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15 |
| SQL Server LocalDB                                           | Tool to create a DB instance in your local machine.    | https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15#install-localdb |
| Git GUI Client (*SourceTree* or any other you feel comfortable with) | Tool to manage your Git repository branches.           | https://git-scm.com/downloads/guis                           |
| Azure Storage Explorer                                       | Monitoring the *Azure Storage* resources.              | https://azure.microsoft.com/en-us/features/storage-explorer/ |
| Servicer Bus Explorer                                        | Monitoring the *Azure Servicer Bus* resources.         | https://github.com/paolosalvatori/ServiceBusExplorer/releases |
| Batch Explorer                                               | Monitoring the *Azure Batch* resources.                | https://azure.github.io/BatchExplorer/                       |



## Local database configuration

### SQL database access

- Open *Microsoft SQL Server Management Studio* and connect to server:
  - Server type: *Database Engine*
  - Server name: *(LocalDb)\MSSQLLocalDB*
  - Authentication: *Windows Authentication*
- Select *"Connect"*

### SQL database publication

- Open the Visual Studio solution:  *<local_directory_path>/src/PublishSystem.sln*

- Right click *"PublishSystem.Database"* project and select *"Publish"* and a new window *"Publish Database"* will be opened.

- Regarding Target database connection, select *"Edit"*

- At the top of the window, select *"Browse"* and select *"Local/MSSQLLocalDB"*

- Select *"OK"*

- Write *"PublishSystem"* on the *"Database name"*.

- Select *"Publish"*.

  

## Running the application

- Open the Visual Studio solution:  *<local_directory_path>/src/PublishSystem.sln*
- From the *"Solution Explorer"*, right click on *"PublishSystem.Functions.Orchestrator"* project and select *"Set as Startup Project"*
- Select *"Run"*



## Running the tests
### Unit Tests
- Open the Visual Studio solution:  *<local_directory_path>/src/PublishSystem.sln*
- From the *"Test"* tab, select *"Run All Tests".