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

ALTER TABLE Clientes
DROP COLUMN Fecha_nacimiento;


------------- DATA ---------------
INSERT INTO Estados_civiles(tipo)VALUES('Casado')
INSERT INTO Estados_civiles(tipo)VALUES('Soltero')

INSERT INTO Clientes(Nombre_completo, Domicilio, Curp, Ingresos_mensuales, Url_imagen, Edad, Hijos, id_estado_civil)
VALUES('MARTINA ALTAMIRANO CALDERON', 'SINALOA', 'AACM651123MTSLLR06', 8500, NULL, 28, 2, 1)

INSERT INTO Clientes(Nombre_completo, Domicilio, Curp, Ingresos_mensuales, Url_imagen, Edad, Hijos, id_estado_civil)
VALUES('ZULEIMA GABRIELA ORDOÑES ALANIS', 'SINALOA', 'OOAZ900824MTSRLL08', 13400, NULL, 22, 0, 2)

INSERT INTO Clientes(Nombre_completo, Domicilio, Curp, Ingresos_mensuales, Url_imagen, Edad, Hijos, id_estado_civil)
VALUES('MARISOL SANCHEZ PEREZ', 'SINALOA', 'SAPM880429MTSNRR00', 10200, NULL, 32, 0, 1)

INSERT INTO Clientes(Nombre_completo, Domicilio, Curp, Ingresos_mensuales, Url_imagen, Edad, Hijos, id_estado_civil)
VALUES('MAGDALENA GONZALEZ RODRIGUEZ', 'SINALOA', 'GORM680121MTSNDG06', 8500, NULL, 29, 3, 1)

INSERT INTO Clientes(Nombre_completo, Domicilio, Curp, Ingresos_mensuales, Url_imagen, Edad, Hijos, id_estado_civil)
VALUES('GUADALUPE RODRIGUEZ BARTOLO', 'SINALOA', 'ROBG900321MTSDRD01', 8500, NULL, 35, 1, 1)

INSERT INTO Clientes(Nombre_completo, Domicilio, Curp, Ingresos_mensuales, Url_imagen, Edad, Hijos, id_estado_civil)
VALUES('YULETH ARELY ORTIZ RODRIGUEZ', 'SINALOA', 'OIRY910429MTSRDL02', 8500, NULL, 45, 0, 2)

INSERT INTO Clientes(Nombre_completo, Domicilio, Curp, Ingresos_mensuales, Url_imagen, Edad, Hijos, id_estado_civil)
VALUES('JUAN ESPINOZA RODRIGUEZ', 'SINALOA', 'EIRJ720502HTSSDN06', 8500, NULL, 23, 0, 2)

INSERT INTO Tarjetas_credito (Descripcion, Limite_credito, Tasa_interes_anual, Min_ingreso_acumulable) 
VALUES('BASICO', 5000, 65, 4000)

INSERT INTO Tarjetas_credito (Descripcion, Limite_credito, Tasa_interes_anual, Min_ingreso_acumulable) 
VALUES('ORO', 25000, 55, 4001)

INSERT INTO Tarjetas_credito (Descripcion, Limite_credito, Tasa_interes_anual, Min_ingreso_acumulable) 
VALUES('PLATINUM', 100000, 45, 15001)