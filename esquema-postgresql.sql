-- Esquema PostgreSQL para Pastelería Canelas

-- Tabla Categorias
CREATE TABLE "Categorias" (
    "CategoriaID" SERIAL PRIMARY KEY,
    "Nombre" VARCHAR(100) NOT NULL,
    "Activo" BOOLEAN NOT NULL DEFAULT TRUE,
    "Descripcion" VARCHAR(500) NULL,
    "ImagenUrl" VARCHAR(500) NULL,
    "Icono" VARCHAR(100) NULL,
    "Slug" VARCHAR(150) NOT NULL
);

-- Tabla Productos
CREATE TABLE "Productos" (
    "ProductoID" SERIAL PRIMARY KEY,
    "CategoriaID" INTEGER NOT NULL,
    "Nombre" VARCHAR(150) NULL,
    "Descripcion" TEXT NULL,
    "ImagenUrl" VARCHAR(500) NULL,
    "Activo" BOOLEAN NOT NULL DEFAULT TRUE,
    "Slug" VARCHAR(250) NOT NULL DEFAULT '',
    "EsDeTemporada" BOOLEAN NOT NULL DEFAULT FALSE,
    CONSTRAINT "FK__Productos__Categ__398D8EEE" FOREIGN KEY ("CategoriaID") REFERENCES "Categorias" ("CategoriaID")
);

-- Tabla ProductoPrecios
CREATE TABLE "ProductoPrecios" (
    "ProductoPrecioID" SERIAL PRIMARY KEY,
    "ProductoID" INTEGER NOT NULL,
    "DescripcionPrecio" VARCHAR(100) NOT NULL,
    "Precio" DECIMAL(10,2) NOT NULL,
    CONSTRAINT "FK__ProductoP__Produ__3D5E1FD2" FOREIGN KEY ("ProductoID") REFERENCES "Productos" ("ProductoID")
);

-- Índices
CREATE INDEX "IX_ProductoPrecios_ProductoID" ON "ProductoPrecios" ("ProductoID");
CREATE INDEX "IX_Productos_CategoriaID" ON "Productos" ("CategoriaID");
