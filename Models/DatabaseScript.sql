CREATE DATABASE WinForms;
GO

USE WinForms;
GO

CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL
);

INSERT INTO Users (Username, Password)
VALUES 
('admin','123'),
('user','123'),
('TienKH','0025768');

CREATE TABLE Lop
(
    MaLop INT IDENTITY(1,1) PRIMARY KEY,
    TenLop NVARCHAR(200) NOT NULL
);

INSERT INTO Lop (TenLop)
VALUES
('CNTT1'),
('CNTT2'),
('CNTT3');

CREATE TABLE SinhVien
(
    MSSV INT IDENTITY(1,1) PRIMARY KEY,
    TenSV NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    MaLop INT,
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);

INSERT INTO SinhVien (TenSV, NgaySinh, GioiTinh, MaLop)
VALUES
('Nguyen Van A','2003-01-10','Male',1),
('Tran Thi B','2003-05-22','Female',2),
('Le Van C','2002-11-15','Male',1),
('Pham Thi D','2003-03-30','Female',3);