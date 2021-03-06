USE [Urbe]
GO
/****** Object:  User [urbe]    Script Date: 17/1/2021 21:14:39 ******/
CREATE USER [urbe] FOR LOGIN [urbe] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [urbe]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idRoom] [int] NOT NULL,
	[attendant] [int] NOT NULL,
	[useProjector] [bit] NOT NULL,
	[useBlackboard] [bit] NOT NULL,
	[useWifi] [bit] NOT NULL,
	[fromDate] [datetime] NOT NULL,
	[toDate] [datetime] NOT NULL,
	[state] [int] NOT NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nchar](50) NOT NULL,
	[capacity] [int] NOT NULL,
	[projector] [bit] NOT NULL,
	[blackboard] [bit] NOT NULL,
	[wifi] [bit] NOT NULL,
 CONSTRAINT [PK_room] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[uspDeleteRoom]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspDeleteRoom]
    @Id int
AS
BEGIN

	IF NOT EXISTS(SELECT NULL FROM Book WHERE idRoom=@Id)
		BEGIN
			DELETE FROM Room WHERE id = @Id;

					SELECT  @Id as id,
							1 AS infoCode,
					       'Sala eliminada' AS infoMessage
		END
	ELSE
		BEGIN
					SELECT  @Id as id,
							0 AS infoCode,
					       'No se pudo eliminar la sala porque tiene una reserva asignada' AS infoMessage
		END
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetRoomById]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspGetRoomById]
    @Id int
AS
BEGIN

	 SELECT id 
		   ,name 
		   ,capacity
		   ,projector  
		   ,blackboard 
		   ,wifi
	 FROM room
	 WHERE id = @Id

END
GO
/****** Object:  StoredProcedure [dbo].[uspGetRooms]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspGetRooms]
    @capacity int,
    @projector bit NULL,   
    @blackboard bit NULL,
	@wifi bit NULL
AS
BEGIN

	 SELECT id 
		   ,TRIM(name) as name
		   ,capacity
		   ,projector  
		   ,blackboard 
		   ,wifi
	 FROM room
	 WHERE capacity >=  @capacity
	 AND (projector = @projector OR  @projector IS NULL)
	 AND (blackboard = @blackboard OR  @blackboard IS NULL)
	 AND (wifi = @wifi OR  @wifi IS NULL)
	 ORDER BY capacity DESC, projector DESC, blackboard DESC, wifi DESC

END
GO
/****** Object:  StoredProcedure [dbo].[uspReportBook]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspReportBook]
    @fromDate datetime,
    @toDate datetime   
AS
BEGIN

	SELECT r.name as nombre,
		   r.capacity as capacidad,
		   r.capacity - b.attendant as disponibilidad,
		   b.attendant ocupacion,
		   r.projector as tieneProyector,
		   b.useProjector as utilizaProyector,
		   r.blackboard as tienePizarra,
		   b.useBlackboard as utilizaPizarra,
		   r.wifi as tieneInternet,
		   b.useWifi as utilizaInternet,
		   b.fromDate as fechaDesde,
		   b.toDate as fechaHasta
	FROM Room r INNER JOIN Book b ON r.id=b.idRoom 
	where  @toDate >= b.fromDate AND
		   @fromDate <= b.toDate AND
		   b.state = 1 -- reserva confirmada
			
END
GO
/****** Object:  StoredProcedure [dbo].[uspSaveBook]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspSaveBook]
    @IdRoom nvarchar(50),   
    @attendant int,
    @useProjector bit,   
    @useBlackboard bit,
	@useWifi bit,
	@fromDate datetime,
	@toDate datetime,
	@state int
AS
BEGIN
	DECLARE @ocupacion AS int
	DECLARE @capacidad AS int
	DECLARE @projector AS bit
	DECLARE @blackboard AS bit
	DECLARE @wifi AS bit
	
	SELECT @capacidad=capacity,
		   @projector=projector,
		   @blackboard=blackboard,
		   @wifi=wifi
	FROM Room 
	WHERE id=@IdRoom 
	
	IF EXISTS (SELECT NULL FROM Room r INNER JOIN Book b ON r.id=b.idRoom AND r.id=@IdRoom
				WHERE @toDate >= b.fromDate AND
				      @fromDate <= b.toDate AND
					  b.state = 1 -- reserva confirmada
			  ) 
		BEGIN

			SELECT @ocupacion=SUM(attendant) 
			FROM Book b 
			WHERE b.idRoom=@IdRoom AND 
				  @toDate >= b.fromDate AND
				  @fromDate <= b.toDate AND
			      b.state = 1 -- reserva confirmada
			
			IF ((@capacidad - @ocupacion) >= @attendant AND
				( @projector = @useProjector OR 0 = @useProjector) AND
				( @blackboard = @useBlackboard OR 0 = @useBlackboard) AND
				( @wifi = @useWifi OR 0 = @useWifi)			
				)
				BEGIN
					INSERT INTO book 
					VALUES (@IdRoom, 
							@attendant, 
							@useProjector, 
							@useBlackboard, 
							@useWifi, 
							@fromDate,
							@toDate,
							@state);

					SELECT  @@IDENTITY as id,
							1 AS infoCode,
					       'Reserva confirmada.' AS infoMessage
				END
			ELSE
				BEGIN
					SELECT  0 as id,
							0 AS infoCode,
					       'No se pudo realizar la reserva. Asistentes > Capacidad o recursos no disponibles' AS infoMessage
				END
		END
	ELSE
		BEGIN

			IF ( @capacidad >= @attendant AND
			    ( @projector = @useProjector OR 0 = @useProjector) AND
				( @blackboard = @useBlackboard OR 0 = @useBlackboard) AND
				( @wifi = @useWifi OR 0 = @useWifi) 
				)
				BEGIN
					INSERT INTO book 
					VALUES (@IdRoom, 
							@attendant, 
							@useProjector, 
							@useBlackboard, 
							@useWifi, 
							@fromDate,
							@toDate,
							@state);

					SELECT  @@IDENTITY as id,
							1 AS infoCode,
					       'Reserva confirmada.' AS infoMessage
				END
			ELSE
				BEGIN
					SELECT  0 as id,
							0 AS infoCode,
					       'No se pudo realizar la reserva. Asistentes > Capacidad o recursos no disponibles' AS infoMessage
				END

		END

END
GO
/****** Object:  StoredProcedure [dbo].[uspSaveRoom]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspSaveRoom]
    @Name nvarchar(50),   
    @Capacity int,
    @Projector bit,   
    @Blackboard bit,
	@Wifi bit
AS
BEGIN

	INSERT INTO room VALUES (@Name, @Capacity, @Projector, @Blackboard, @Wifi);

	SELECT @@IDENTITY;

END
GO
/****** Object:  StoredProcedure [dbo].[uspUpdateRoom]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspUpdateRoom]
    @Id int,
	@Name nvarchar(50),   
    @Capacity int,
    @Projector bit,   
    @Blackboard bit,
	@Wifi bit
AS
BEGIN

	UPDATE room SET  
	 name = @Name, 
	 capacity = @Capacity, 
	 projector = @Projector, 
	 blackboard = @Blackboard, 
	 wifi = @Wifi
	WHERE id = @Id


END
GO
/****** Object:  StoredProcedure [dbo].[uspUpdateStateBook]    Script Date: 17/1/2021 21:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspUpdateStateBook]
    @Id int,
	@State int
AS
BEGIN

	UPDATE book SET  
	 state = @State 
	WHERE id = @Id

END
GO
