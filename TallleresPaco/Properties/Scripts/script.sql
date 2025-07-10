CREATE TABLE vehiculos(
    id INT IDENTITY(1,1) PRIMARY KEY,
    matricula VARCHAR(7),
    modelo VARCHAR(50),
    marca VARCHAR(50),
    color VARCHAR(50),
    aniofab DATE,
    tipo VARCHAR(50),
    precio DECIMAL,
    categoria VARCHAR(50),
    estado VARCHAR(50)
);
CREATE TABLE usuarios(
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    fecnac DATE,
    dni VARCHAR(9),
    email VARCHAR(100),
    estado VARCHAR(50)
);
CREATE TABLE alquileres(
    id INT IDENTITY(1,1) PRIMARY KEY,
    idusu INT,
    idveh INT,
    fecini DATE,
    fecfin DATE,
    precio DECIMAL,
    prefin DECIMAL,
    estado VARCHAR(50),
    CONSTRAINT alquileres_fk1 FOREIGN KEY (idusu) REFERENCES usuarios(id),
    CONSTRAINT alquileres_fk2 FOREIGN KEY (idveh) REFERENCES vehiculos(id)
);