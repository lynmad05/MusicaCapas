DROP TABLE IF EXISTS Usuarios;

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Correo NVARCHAR(100) NOT NULL,
    ClaveHash NVARCHAR(100) NOT NULL,
    Rol NVARCHAR(30) NOT NULL,
    CONSTRAINT UQ_Usuarios_Correo UNIQUE (Correo),
    CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN ('Oyente', 'Curador', 'GerenteCatalogo'))
);

