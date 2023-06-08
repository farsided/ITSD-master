USE [master]
GO
/****** Object:  Database [dbITSD]    Script Date: 08/09/2022 05:23:14 PM ******/
CREATE DATABASE [dbITSD]
GO
USE [dbITSD]
GO
/****** Object:  StoredProcedure [dbo].[tbl_JobOrder_Create]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[tbl_JobOrder_Create]
@Title varchar(max) = null,
@JobDescription varchar(max) = null,
@JDate datetime = null,
@RequestedBy varchar(512) = null,
@encBy int = null
AS
BEGIN
INSERT INTO [tbl_JobOrder]
([Title],[JobDescription],[JDate],[RequestedBy],[Status],[encBy])
VALUES
(@Title,@JobDescription,@JDate,@RequestedBy,'Pending',@encBy)
END

GO
/****** Object:  StoredProcedure [dbo].[tbl_JobOrder_Find]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_JobOrder_Find]
@ID int = null
AS
BEGIN
SELECT * FROM [tbl_JobOrder] WHERE  ID = @ID
END

GO
/****** Object:  StoredProcedure [dbo].[tbl_JobOrder_List]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_JobOrder_List]
@Search VARCHAR(MAX) = '',
@encBy INT = 0
AS
BEGIN
	IF @encBy != 0
		BEGIN
			SELECT * FROM [tbl_JobOrder]
			WHERE CONCAT([ID],[JobDescription],[JDate],[RequestedBy],[Category],[SubCategory],[Priority],[Status],[ATID],[Cancel],[CancelBy],[encBy],[encDate]) LIKE @Search AND encBy = @encBy
		END
	ELSE
		BEGIN
			SELECT * FROM [tbl_JobOrder]
			WHERE CONCAT([ID],[JobDescription],[JDate],[RequestedBy],[Category],[SubCategory],[Priority],[Status],[ATID],[Cancel],[CancelBy],[encBy],[encDate]) LIKE @Search AND [Status] IN ('Pending', 'In progress')
			ORDER BY [Status],ID ASC
		END
END

GO
/****** Object:  StoredProcedure [dbo].[tbl_JobOrder_Update]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_JobOrder_Update]
@ID int = null,
@Title varchar(max) = null,
@JobDescription varchar(max) = null,
@JDate datetime = null,
@RequestedBy varchar(512) = null,
@encBy int = null
AS
BEGIN
UPDATE [tbl_JobOrder] SET [Title] = @Title
,[JobDescription] = @JobDescription
,[JDate] = @JDate
,[RequestedBy] = @RequestedBy
,[encBy] = @encBy WHERE [ID] = @ID
END

GO
/****** Object:  Table [dbo].[tbl_Category]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](512) NULL,
	[encBy] [int] NULL,
	[encDate] [datetime] NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_JobActionTaken]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_JobActionTaken](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[JOID] [int] NULL,
	[ADate] [datetime] NULL,
	[ActionTaken] [varchar](max) NULL,
	[encBy] [int] NULL,
	[encDate] [datetime] NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_JobOrder]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_JobOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](max) NULL,
	[JobDescription] [varchar](max) NULL,
	[JDate] [datetime] NULL,
	[RequestedBy] [varchar](512) NULL,
	[Category] [varchar](255) NULL,
	[SubCategory] [varchar](255) NULL,
	[Priority] [int] NULL,
	[Status] [varchar](15) NULL,
	[ATID] [int] NULL,
	[ATDate] [datetime] NULL,
	[Cancel] [bit] NULL,
	[CancelBy] [int] NULL,
	[CancelDate] [datetime] NULL,
	[encBy] [int] NULL,
	[encDate] [datetime] NULL CONSTRAINT [DF__tbl_JobOr__encDa__108B795B]  DEFAULT (getdate()),
 CONSTRAINT [PK__tbl_JobO__3214EC27F2133432] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_SubCategory]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_SubCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](512) NULL,
	[encBy] [int] NULL,
	[encDate] [datetime] NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[GeneralEIS]    Script Date: 08/09/2022 05:23:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GeneralEIS]
(	

)
RETURNS TABLE 
AS
RETURN 
(
	SELECT 'MHCI' Company,* FROM [dbMHCI_EIS].[dbo].[tbl_EmployeeInformation]
	UNION ALL
	SELECT 'ISO' Company,* FROM [dbISO_EIS].[dbo].[tbl_EmployeeInformation]
	UNION ALL 
	SELECT 'SET' Company,* FROM [dbSET_EIS].[dbo].[tbl_EmployeeInformation] 
	UNION ALL
	SELECT 'MYFC' Company,* FROM [dbMYFC_EIS].[dbo].[tbl_EmployeeInformation]
)

GO
USE [master]
GO
ALTER DATABASE [dbITSD] SET  READ_WRITE 
GO
