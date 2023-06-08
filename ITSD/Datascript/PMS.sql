USE [master]
GO
/****** Object:  Database [dbPMS]    Script Date: 08/09/2022 05:24:06 PM ******/
CREATE DATABASE [dbPMS]
GO
USE [dbPMS]
GO
/****** Object:  UserDefinedTableType [dbo].[tp_systemlvl]    Script Date: 08/09/2022 05:24:06 PM ******/
CREATE TYPE [dbo].[tp_systemlvl] AS TABLE(
	[ID] [int] NULL,
	[SysID] [int] NULL,
	[Level] [int] NULL,
	[Description] [varchar](max) NULL,
	[Url] [varchar](max) NULL,
	[isDefault] [bit] NULL,
	[encDate] [datetime] NULL DEFAULT (getdate())
)
GO
/****** Object:  StoredProcedure [dbo].[Login]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Login]
@User VARCHAR(MAX),
@Pass VARCHAR(MAX)
AS
BEGIN
	DECLARE @ID INT

	SET @ID = 
	(
		SELECT ID
		FROM tbl_user
		WHERE [User] = @User AND Pass = @Pass AND isActive = 1
	)

	SELECT @ID
END

GO
/****** Object:  StoredProcedure [dbo].[tbl_system_Create]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_system_Create]
@Description varchar(max) = null,
@DateStarted datetime = null,
@DateExpected datetime = null,
@DateCompleted datetime = null,
@DevelopBy int = null,
@Levels tp_systemlvl READONLY
AS
BEGIN
INSERT INTO [tbl_system]
([Description],[DateStarted],[DateExpected],[DateCompleted],[DevelopBy])
VALUES
(@Description,@DateStarted,@DateExpected,@DateCompleted,@DevelopBy)

INSERT INTO tbl_systemlvl ([SysID]
      ,[Level]
      ,[Description]
      ,[Url]
      ,[isDefault])
SELECT IDENT_CURRENT('tbl_system')
      ,[Level]
      ,[Description]
      ,[Url]
      ,ISNULL(isDefault, 0)
FROM @Levels
END



GO
/****** Object:  StoredProcedure [dbo].[tbl_system_Find]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_system_Find]
@ID int = null
AS
BEGIN
SELECT * FROM [tbl_system] WHERE  ID = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[tbl_system_List]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_system_List]
@Search VARCHAR(MAX) = ''
AS
BEGIN
SELECT * FROM [tbl_system] WHERE CONCAT([ID],FORMAT(encDate, 'yy-'), FORMAT(ID, '00000'),[Description],[DateStarted],[DateExpected],[DateCompleted],[DevelopBy],[encDate]) LIKE @Search 
END


GO
/****** Object:  StoredProcedure [dbo].[tbl_system_Update]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[tbl_system_Update]
@ID int = null,
@Description varchar(max) = null,
@DateStarted datetime = null,
@DateExpected datetime = null,
@DateCompleted datetime = null,
@Levels tp_systemlvl READONLY
AS
BEGIN
UPDATE [tbl_system] SET [Description] = @Description
,[DateStarted] = @DateStarted
,[DateExpected] = @DateExpected
,[DateCompleted] = @DateCompleted WHERE [ID] = @ID

DELETE FROM tbl_systemlvl WHERE SysID = @ID
SET IDENTITY_INSERT tbl_systemlvl ON

	INSERT INTO tbl_systemlvl ([ID]
      ,[SysID]
      ,[Level]
      ,[Description]
      ,[Url]
      ,[isDefault])
	SELECT CASE WHEN ID = 0 THEN IDENT_CURRENT('tbl_systemlvl') + 1 ELSE [ID] END
      ,@ID
      ,[Level]
      ,[Description]
      ,[Url]
      ,[isDefault] FROM @Levels

SET IDENTITY_INSERT tbl_systemlvl OFF

END



GO
/****** Object:  StoredProcedure [dbo].[tbl_user_Create]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[tbl_user_Create]
@User varchar(max) = null,
@Pass varchar(max) = null,
@Phone varchar(max) = null,
@Email varchar(max) = null,
@isActive bit = null,
@isAdmin bit = null,
@IDNo varchar(50) = null,
@fname varchar(max) = null,
@mn varchar(max) = null,
@lname varchar(max) = null,
@gender varchar(max) = null,
@bday datetime = null,
@employmentstatus varchar(50) = null,
@company VARCHAR(512) = null,
@department VARCHAR(512) = null,
@position VARCHAR(512) = null
AS
BEGIN
INSERT INTO [tbl_user]
([User],[Pass],[Phone],[Email],[isActive],[isAdmin])
VALUES
(@User,@Pass,@Phone,@Email,@isActive,@isAdmin)

--info
INSERT INTO [tbl_userInfo]
([IDNo],[fname],[mn],[lname],[gender],[bday],[employmentstatus],[company],[department],[position])
VALUES
(@IDNo,@fname,@mn,@lname,@gender,@bday,@employmentstatus,@company,@department,@position)
END


GO
/****** Object:  StoredProcedure [dbo].[tbl_user_Find]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_user_Find]
@ID int = null
AS
BEGIN
SELECT * FROM [tbl_user] WHERE  ID = @ID
END


GO
/****** Object:  StoredProcedure [dbo].[tbl_user_List]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_user_List]
@Search VARCHAR(MAX) = ''
AS
BEGIN
SELECT * FROM [tbl_user] WHERE CONCAT([ID],[User],[Pass],[Phone],[Email],[isActive],[isAdmin],[encDate]) LIKE @Search 
END


GO
/****** Object:  StoredProcedure [dbo].[tbl_user_Update]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[tbl_user_Update]
@ID int = null,
@User varchar(max) = null,
@Pass varchar(max) = null,
@Phone varchar(max) = null,
@Email varchar(max) = null,
@isActive bit = null,
@isAdmin bit = null,
@IDNo varchar(50) = null,
@fname varchar(max) = null,
@mn varchar(max) = null,
@lname varchar(max) = null,
@gender varchar(max) = null,
@bday datetime = null,
@employmentstatus varchar(50) = null,
@company VARCHAR(512) = null,
@department VARCHAR(512) = null,
@position VARCHAR(512) = null
AS
BEGIN
IF @isActive IS NOT NULL OR @isAdmin IS NOT NULL 
BEGIN
	UPDATE [tbl_user] SET [User] = @User
	,[Pass] = @Pass
	,[Phone] = @Phone
	,[Email] = @Email
	,[isActive] = @isActive
	,[isAdmin] = @isAdmin WHERE [ID] = @ID
END
ELSE
BEGIN
	UPDATE [tbl_user] SET [User] = @User
	,[Pass] = @Pass
	,[Phone] = @Phone
	,[Email] = @Email WHERE [ID] = @ID
END


--info
UPDATE [tbl_userInfo] SET [IDNo] = @IDNo
,[fname] = @fname
,[mn] = @mn
,[lname] = @lname
,[gender] = @gender
,[bday] = @bday
,[employmentstatus] = @employmentstatus
,[company] = @company
,[department] = @department
,[position] = @position WHERE [userID] = @ID
END


GO
/****** Object:  Table [dbo].[tbl_system]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_system](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](max) NULL,
	[DateStarted] [datetime] NULL,
	[DateExpected] [datetime] NULL,
	[DateCompleted] [datetime] NULL,
	[DevelopBy] [int] NULL,
	[encDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_systemlvl]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_systemlvl](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SysID] [int] NULL,
	[Level] [int] NULL,
	[Description] [varchar](max) NULL,
	[Url] [varchar](max) NULL,
	[isDefault] [bit] NULL,
	[encDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_user]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_user](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[User] [varchar](max) NULL,
	[Pass] [varchar](max) NULL,
	[Phone] [varchar](max) NULL,
	[Email] [varchar](max) NULL,
	[isActive] [bit] NULL DEFAULT ((1)),
	[isAdmin] [bit] NULL DEFAULT ((0)),
	[encDate] [datetime] NULL DEFAULT (getdate())
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_userInfo]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbl_userInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL CONSTRAINT [DF_tbl_userInfo_userID]  DEFAULT (ident_current('tbl_user')),
	[IDNo] [varchar](50) NULL,
	[fname] [varchar](max) NULL,
	[mn] [varchar](max) NULL,
	[lname] [varchar](max) NULL,
	[gender] [varchar](max) NULL,
	[bday] [datetime] NULL,
	[employmentstatus] [varchar](50) NULL DEFAULT ('N/A'),
	[company] [varchar](512) NULL,
	[department] [varchar](512) NULL,
	[position] [varchar](512) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbl_userSystem]    Script Date: 08/09/2022 05:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_userSystem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[SysID] [int] NULL,
	[syslvl] [int] NULL,
	[encBy] [int] NULL,
	[encDate] [datetime] NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[tbl_system] ADD  DEFAULT (getdate()) FOR [encDate]
GO
ALTER TABLE [dbo].[tbl_systemlvl] ADD  DEFAULT (getdate()) FOR [encDate]
GO
ALTER TABLE [dbo].[tbl_userSystem] ADD  DEFAULT (getdate()) FOR [encDate]
GO
USE [master]
GO
ALTER DATABASE [dbPMS] SET  READ_WRITE 
GO
