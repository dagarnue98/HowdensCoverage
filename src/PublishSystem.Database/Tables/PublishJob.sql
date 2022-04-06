CREATE TABLE [dbo].[PublishJob](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [Uniqueidentifier] NULL UNIQUE,
	[VersionId] [int] NULL,
	[State] [nvarchar](50) NULL,
	[DepotNo] [nvarchar](50) NULL,
	[BuilderId] [nvarchar](50) NULL,
	[QuoteNMBR] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[HdVersion] [nvarchar](1) NULL,
	[PlanType] [int] NULL,
	[SenderEmail] [nvarchar](50) NULL,
	[ReceipientEmail1] [nvarchar](50) NULL,
	[ReceipientEmail2] [nvarchar](50) NULL,
	[BatchJobId] [nvarchar](50) NULL,
	[Comments] [nvarchar](max) NULL,
	PRIMARY KEY ([Id])
) 