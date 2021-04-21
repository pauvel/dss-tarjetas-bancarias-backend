---------------------- CATALOGOS --------------------------

CREATE TABLE Estados_civiles( -- CAN BE : SOLTERO, CASADO
    Id int primary key identity(1,1),
    tipo varchar(25) not null
)
------------------------------------------------------------

-------------------------- LOGISTICA ------------------------

CREATE TABLE Tarjetas_credito(
    Id int primary key identity(1,1),
    Descripcion TEXT not null,
    Limite_credito numeric(18,2),
    Tasa_interes_anual numeric(18,2),
    Min_ingreso_acumulable numeric(10,2)
)

CREATE TABLE Clientes(
    Id int primary key identity(1,1),
    Nombre_completo varchar(max) not null,
    Fecha_nacimiento Date not null,
    Domicilio varchar(max) not null,
    Curp varchar(18) not null,
    Ingresos_mensuales numeric(10,2),
    Url_imagen varchar(max),
    Edad tinyint not null,
    Hijos tinyint not null default 0
)
-------------------------------------------------------------------------------


------------------------------- LOGICA DE SOLICITUD ----------------------------

CREATE TABLE Solicitudes(
    Id int primary key identity(1,1),
    id_cliente int not null,
    Fecha DateTime DEFAULT CURRENT_TIMESTAMP, 
    Aprobado bit not null,
    id_tarjeta_credito int null,
    FOREIGN KEY (id_cliente) REFERENCES Clientes(Id),
    FOREIGN KEY (id_tarjeta_credito) REFERENCES Tarjetas_credito(Id)
)


----------------------- FIXES ---------------------------------
ALTER TABLE Clientes ADD id_estado_civil int
ALTER TABLE Clientes ADD FOREIGN KEY (id_estado_civil) REFERENCES Estados_civiles(Id);