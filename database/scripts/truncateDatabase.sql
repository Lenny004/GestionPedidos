-- Trigger para INSERT en Customers
CREATE TRIGGER trg_Customers_Insert
ON Customers
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Customers',
        'INSERT',
        i.idCustomer,
        'Nuevo cliente registrado: ' + i.firstName + ' ' + i.lastName,
        ISNULL(i.userCreation, 1) -- Si no hay usuario, usar ID 1 (admin)
    FROM inserted i;
END;
GO

-- Trigger para UPDATE en Customers
CREATE TRIGGER trg_Customers_Update
ON Customers
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Customers',
        'UPDATE',
        i.idCustomer,
        'Cliente modificado: ' + i.firstName + ' ' + i.lastName + 
        CASE 
            WHEN d.isActive = 1 AND i.isActive = 0 THEN ' (Desactivado)'
            WHEN d.isActive = 0 AND i.isActive = 1 THEN ' (Activado)'
            ELSE ''
        END,
        ISNULL(i.userModification, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.idCustomer = d.idCustomer;
END;
GO

-- Trigger para DELETE en Customers (Soft Delete)
CREATE TRIGGER trg_Customers_Delete
ON Customers
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Solo registra si se marcó como eliminado
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Customers',
        'DELETE',
        i.idCustomer,
        'Cliente eliminado: ' + i.firstName + ' ' + i.lastName,
        ISNULL(i.userModification, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.idCustomer = d.idCustomer
    WHERE i.deletedAt IS NOT NULL AND d.deletedAt IS NULL;
END;
GO

-- TRIGGERS PARA TABLA PRODUCTS

CREATE TRIGGER trg_Products_Insert
ON Products
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Products',
        'INSERT',
        i.idProduct,
        'Nuevo producto creado: ' + i.productName + ' (Stock: ' + CAST(i.stockQuantity AS VARCHAR) + ')',
        ISNULL(i.userCreation, 1)
    FROM inserted i;
END;
GO

CREATE TRIGGER trg_Products_Update
ON Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Products',
        'UPDATE',
        i.idProduct,
        'Producto modificado: ' + i.productName + 
        CASE 
            WHEN d.stockQuantity <> i.stockQuantity 
            THEN ' (Stock: ' + CAST(d.stockQuantity AS VARCHAR) + ' → ' + CAST(i.stockQuantity AS VARCHAR) + ')'
            ELSE ''
        END +
        CASE 
            WHEN d.salePrice <> i.salePrice 
            THEN ' (Precio: $' + CAST(d.salePrice AS VARCHAR) + ' → $' + CAST(i.salePrice AS VARCHAR) + ')'
            ELSE ''
        END,
        ISNULL(i.userModification, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.idProduct = d.idProduct;
END;
GO

-- TRIGGERS PARA TABLA ORDERS

CREATE TRIGGER trg_Orders_Insert
ON Orders
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Orders',
        'INSERT',
        i.idOrder,
        'Nueva orden creada #' + CAST(i.idOrder AS VARCHAR) + ' para cliente ID: ' + CAST(i.idCustomer AS VARCHAR) + 
        ' (Total: $' + CAST(i.total AS VARCHAR) + ')',
        ISNULL(i.userCreation, 1)
    FROM inserted i;
END;
GO

CREATE TRIGGER trg_Orders_Update
ON Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Orders',
        'UPDATE',
        i.idOrder,
        'Orden #' + CAST(i.idOrder AS VARCHAR) + 
        CASE 
            WHEN d.orderStatus <> i.orderStatus 
            THEN ' - Estado cambió de ' + d.orderStatus + ' a ' + i.orderStatus
            ELSE ' modificada'
        END,
        ISNULL(i.userCreation, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.idOrder = d.idOrder;
END;
GO

-- TRIGGERS PARA TABLA ORDERDETAILS

CREATE TRIGGER trg_OrderDetails_Insert
ON OrderDetails
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @idUser INT;
    SELECT @idUser = userCreation FROM Orders WHERE idOrder = (SELECT TOP 1 idOrder FROM inserted);
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'OrderDetails',
        'INSERT',
        i.idDetail,
        'Detalle agregado a orden #' + CAST(i.idOrder AS VARCHAR) + 
        ' (Producto ID: ' + CAST(i.idProduct AS VARCHAR) + 
        ', Cantidad: ' + CAST(i.quantity AS VARCHAR) + ')',
        ISNULL(@idUser, 1)
    FROM inserted i;
END;
GO

-- TRIGGERS PARA TABLA USERS

CREATE TRIGGER trg_Users_Insert
ON Users
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Users',
        'INSERT',
        i.idUser,
        'Nuevo usuario creado: ' + i.userName + ' (' + i.fullName + ')',
        i.idUser -- El mismo usuario se registra
    FROM inserted i;
END;
GO

CREATE TRIGGER trg_Users_Update
ON Users
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO History (affectedTable, actionType, recordId, description, idUser)
    SELECT 
        'Users',
        'UPDATE',
        i.idUser,
        'Usuario modificado: ' + i.userName + 
        CASE 
            WHEN d.isActive = 1 AND i.isActive = 0 THEN ' (Desactivado)'
            WHEN d.isActive = 0 AND i.isActive = 1 THEN ' (Activado)'
            WHEN d.passwordHash <> i.passwordHash THEN ' (Contraseña cambiada)'
            ELSE ''
        END,
        i.idUser
    FROM inserted i
    INNER JOIN deleted d ON i.idUser = d.idUser;
END;
GO