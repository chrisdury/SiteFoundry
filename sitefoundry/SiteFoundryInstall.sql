if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_NodeNames_Nodes]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[NodeNames] DROP CONSTRAINT FK_NodeNames_Nodes
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[publishArticles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[publishArticles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[publishLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[publishLinks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[publishNodes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[publishNodes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[publishRawHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[publishRawHtml]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[showPublishArticles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[showPublishArticles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[showPublishLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[showPublishLinks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[showPublishRawHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[showPublishRawHtml]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NodeRolesView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[NodeRolesView]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ArticleBodies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ArticleBodies]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ArticleContainers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ArticleContainers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ArticleStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ArticleStatus]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Links]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NodeNames]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[NodeNames]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Nodes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Nodes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SecurityNodes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SecurityNodes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SecurityRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SecurityRoles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SecurityUserRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SecurityUserRoles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SecurityUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SecurityUsers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SimpleArticles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SimpleArticles]
GO

CREATE TABLE [dbo].[ArticleBodies] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[articleID] [int] NOT NULL ,
	[templateID] [int] NULL ,
	[lang] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
	[pageNumber] [int] NOT NULL ,
	[title] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
	[summary] [text] COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
	[keywords] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
	[body] [text] COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
	[createdBy] [int] NOT NULL ,
	[editedBy] [int] NOT NULL ,
	[statusID] [int] NOT NULL ,
	[publish] [bit] NULL ,
	[dateCreated] [datetime] NULL ,
	[dateModified] [datetime] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[ArticleContainers] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[nodeID] [int] NOT NULL ,
	[typeID] [int] NOT NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
	[publish] [bit] NULL ,
	[dateCreated] [datetime] NULL ,
	[dateModified] [datetime] NULL ,
	[dateExpires] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ArticleStatus] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Links] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[nodeID] [int] NOT NULL ,
	[linkURL] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
	[publish] [bit] NOT NULL ,
	[dateCreated] [datetime] NOT NULL ,
	[dateModified] [datetime] NOT NULL ,
	[createdBy] [int] NOT NULL ,
	[editedBy] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[NodeNames] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[nodeID] [int] NOT NULL ,
	[lang] [char] (5) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
	[name] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Nodes] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[parentID] [int] NOT NULL ,
	[filename] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
	[nodeTypeID] [int] NULL ,
	[publish] [bit] NULL ,
	[visible] [bit] NULL ,
	[rank] [int] NULL ,
	[DateCreated] [datetime] NULL ,
	[DateModified] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SecurityNodes] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[nodeID] [int] NOT NULL ,
	[roleID] [int] NOT NULL ,
	[permissionLevel] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SecurityRoles] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SecurityUserRoles] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[userID] [int] NOT NULL ,
	[roleID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SecurityUsers] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[Username] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
	[Password] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
	[FullName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
	[Email] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
	[LastLogin] [datetime] NULL ,
	[DateCreated] [datetime] NULL ,
	[DateModified] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SimpleArticles] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[nodeID] [int] NOT NULL ,
	[showTemplate] [bit] NULL ,
	[title] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
	[body] [text] COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
	[publish] [bit] NULL ,
	[dateCreated] [datetime] NULL ,
	[dateModified] [datetime] NULL ,
	[editedBy] [int] NULL ,
	[createdBy] [int] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ArticleBodies] WITH NOCHECK ADD 
	CONSTRAINT [DF_ArticleBodies_pageNumber] DEFAULT (0) FOR [pageNumber],
	CONSTRAINT [PK_ArticleBodies] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ArticleContainers] WITH NOCHECK ADD 
	CONSTRAINT [PK_ArticleContainers] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ArticleStatus] WITH NOCHECK ADD 
	CONSTRAINT [PK_ArticleStatus] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Links] WITH NOCHECK ADD 
	CONSTRAINT [PK_Links] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[NodeNames] WITH NOCHECK ADD 
	CONSTRAINT [PK_NodeNames] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Nodes] WITH NOCHECK ADD 
	CONSTRAINT [PK_Nodes] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SecurityNodes] WITH NOCHECK ADD 
	CONSTRAINT [PK_SecurityNodes] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SecurityRoles] WITH NOCHECK ADD 
	CONSTRAINT [PK_SecurityGroups] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SecurityUserRoles] WITH NOCHECK ADD 
	CONSTRAINT [PK_SecurityUserGroups] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SecurityUsers] WITH NOCHECK ADD 
	CONSTRAINT [PK_SecurityUsers] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SimpleArticles] WITH NOCHECK ADD 
	CONSTRAINT [PK_SimpleArticles] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[NodeNames] ADD 
	CONSTRAINT [FK_NodeNames_Nodes] FOREIGN KEY 
	(
		[nodeID]
	) REFERENCES [dbo].[Nodes] (
		[id]
	)
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE VIEW dbo.NodeRolesView
AS
SELECT     n.id, r.name, n.permissionLevel, n.nodeID
FROM         dbo.SecurityNodes n INNER JOIN
                      dbo.SecurityRoles r ON n.roleID = r.id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE publishArticles AS


--first publish the containers
if exists (select * from dbo.sysobjects where id = object_id(N'ArticleContainers_public') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		drop table [dbo].[ArticleContainers_public]
	END
SELECT * INTO [dbo].[ArticleContainers_public] FROM ArticleContainers


--now publish the articles themselves.

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ArticleBodies_public]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		--drop table [dbo].[ArticleBodies_public]
		CREATE TABLE [dbo].[ArticleBodies_public] (
			[id] [int] IDENTITY (1, 1) NOT NULL ,
			[articleID] [int] NOT NULL ,
			[templateID] [int] NULL ,
			[lang] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
			[pageNumber] [int] NOT NULL ,
			[title] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
			[summary] [text] COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
			[keywords] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
			[body] [text] COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
			[createdBy] [int] NOT NULL ,
			[editedBy] [int] NOT NULL ,
			[statusID] [int] NOT NULL ,
			[publish] [bit] NULL ,
			[dateCreated] [datetime] NULL ,
			[dateModified] [datetime] NULL 
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END


SET IDENTITY_INSERT [dbo].[ArticleBodies_public] ON


--insert new rows
INSERT INTO [dbo].[ArticleBodies_public] 
(id,articleID,templateID,lang,pageNumber,title,summary,keywords,body,createdBy,editedBy,statusID,publish,dateCreated,dateModified)
SELECT * FROM ArticleBodies
WHERE ArticleBodies.publish = 1 
AND ArticleBodies.id NOT IN (SELECT id FROM [dbo].[ArticleBodies_public])



--update rows
UPDATE [dbo].[ArticleBodies_public] SET
	articleID = ab.articleID,
	templateID = ab.templateID,
	lang = ab.lang,
	pageNumber = ab.pageNumber,
	title = ab.title,
	summary = ab.summary,
	keywords = ab.keywords,
	body = ab.body,
	createdBy = ab.createdBy,
	editedBy = ab.editedBy,
	statusID = ab.statusID,
	publish = ab.publish,	dateCreated = ab.dateCreated,
	dateModified = ab.dateModified
FROM ArticleBodies ab
WHERE [dbo].[ArticleBodies_public].[id] = ab.id
AND ab.publish = 1



-- delete orphan rows
DELETE FROM [dbo].[ArticleBodies_public] WHERE id NOT IN
(SELECT id FROM ArticleBodies)


-- turn off publish bit
UPDATE ArticleBodies SET publish = 0 WHERE publish = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE publishLinks AS

if not exists (select * from dbo.sysobjects where id = object_id(N'Links_public') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	SELECT * INTO [dbo].[Links_public] FROM Links WHERE Links.publish = 1
else 
	INSERT INTO [dbo].[Links_public] (nodeID, linkURL, publish, dateCreated, dateModified, createdBy, editedBy)
	SELECT nodeID, linkURL, publish, dateCreated, dateModified, createdBy, editedBy FROM Links WHERE publish = 1
--drop table [dbo].[Links_public]
--SELECT * INTO [dbo].[Links_public] FROM Links WHERE Links.publish = 1

UPDATE Links SET publish = 0


GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE  PROCEDURE publishNodes AS

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Nodes_public]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		CREATE TABLE [dbo].[Nodes_public] (
			[id] [int] IDENTITY (1, 1) NOT NULL ,
			[parentID] [int] NOT NULL ,
			[filename] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AI NOT NULL ,
			[nodeTypeID] [int] NULL ,
			[publish] [bit] NULL ,
			[visible] [bit] NULL,
			[DateCreated] [datetime] NULL ,
			[DateModified] [datetime] NULL 
		) ON [PRIMARY]
	END

SET IDENTITY_INSERT [dbo].[Nodes_public] ON


INSERT INTO [dbo].[Nodes_public] 
(id,parentID,filename,nodeTypeID,publish,visible,DateCreated,DateModified)
SELECT * FROM Nodes
WHERE Nodes.publish = 1 
AND Nodes.id NOT IN (SELECT id FROM [dbo].[Nodes_public])


UPDATE [dbo].[Nodes_public] SET
	parentID = n.parentID,
	filename = n.filename,
	nodeTypeID = n.nodeTypeID,
	visible = n.visible,
	DateCreated = n.DateCreated,
	DateModified = n.DateModified
FROM Nodes n
WHERE [dbo].[Nodes_public].[id] = n.id
AND n.publish = 1

DELETE FROM [dbo].[Nodes_public] WHERE id NOT IN
(SELECT id FROM Nodes)


-- turn off publish bit
UPDATE Nodes SET publish = 0 WHERE publish = 1


-- update nodenames
if  exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NodeNames_public]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		DROP TABLE [dbo].[NodeNames_public] 
	END

SELECT * INTO [dbo].[NodeNames_public] FROM NodeNames
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE publishRawHtml AS

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SimpleArticles_public]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN

		CREATE TABLE [dbo].[SimpleArticles_public] (
			[id] [int] IDENTITY (1, 1) NOT NULL ,
			[nodeID] [int] NOT NULL ,
			[showTemplate] [bit] NULL ,
			[title] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
			[body] [text] COLLATE SQL_Latin1_General_CP1_CI_AI NULL ,
			[publish] [bit] NULL ,
			[dateCreated] [datetime] NULL ,
			[dateModified] [datetime] NULL ,
			[editedBy] [int] NULL ,
			[createdBy] [int] NULL 
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END


SET IDENTITY_INSERT [dbo].[SimpleArticles_public] ON


INSERT INTO [dbo].[SimpleArticles_public] 
(id,nodeID,showTemplate,title,body,publish,dateCreated,dateModified,editedBy,createdBy)
SELECT * FROM SimpleArticles
WHERE SimpleArticles.publish = 1 
AND SimpleArticles.id NOT IN (SELECT id FROM [dbo].[SimpleArticles_public])



UPDATE [dbo].[SimpleArticles_public] SET
	nodeID = p.nodeID,
	showTemplate = p.showTemplate,
	title = p.title,
	body = p.body,
	publish = p.publish,
	dateCreated = p.dateCreated,
	dateModified = p.dateModified,
	editedBy = p.editedBy,
	createdBy = p.createdBy
FROM SimpleArticles p
WHERE [dbo].[SimpleArticles_public].[id] = p.id
AND p.publish = 1


DELETE FROM [dbo].[SimpleArticles_public] WHERE id NOT IN
(SELECT id FROM SimpleArticles)


UPDATE SimpleArticles SET publish = 0 WHERE publish = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE showPublishArticles AS

SELECT     ac.nodeID, n.filename, ab.title, ab.dateModified, u.Username
FROM         ArticleBodies ab INNER JOIN
                      ArticleContainers ac ON ab.articleID = ac.id INNER JOIN
                      Nodes n ON ac.nodeID = n.id INNER JOIN
                      SecurityUsers u ON ab.editedBy = u.id
WHERE     (ab.publish = 1)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE showPublishLinks AS
SELECT     l.nodeID, n.filename, l.linkURL AS title, l.dateModified, u.Username
FROM         Links l INNER JOIN
                      Nodes n ON l.nodeID = n.id INNER JOIN
                      SecurityUsers u ON l.editedBy = u.id
WHERE     (l.publish = 1)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE showPublishRawHtml AS
SELECT     sa.nodeID, n.filename, sa.title, sa.dateModified, u.Username
FROM         SimpleArticles sa INNER JOIN
                      Nodes n ON sa.nodeID = n.id INNER JOIN
                      SecurityUsers u ON sa.editedBy = u.id
WHERE     (sa.publish = 1)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

