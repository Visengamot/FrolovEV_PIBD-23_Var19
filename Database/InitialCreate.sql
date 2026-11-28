CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE EXTENSION IF NOT EXISTS citext;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE TABLE "Manufacturers" (
        "Id" uuid NOT NULL,
        "Name" citext NOT NULL,
        "Country" character varying(100) NOT NULL,
        CONSTRAINT "PK_Manufacturers" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_Manufacturers_Text" CHECK (length(btrim("Name")) > 0 AND length(btrim("Country")) > 0)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE TABLE "ShoeSizes" (
        "Id" uuid NOT NULL,
        "Value" numeric(4,1) NOT NULL,
        CONSTRAINT "PK_ShoeSizes" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_ShoeSizes_Value" CHECK ("Value" BETWEEN 10 AND 60 AND mod("Value" * 2, 1) = 0)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE TABLE "StockMovements" (
        "Id" uuid NOT NULL,
        "Date" date NOT NULL,
        "Type" integer NOT NULL,
        CONSTRAINT "PK_StockMovements" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_StockMovements_Type" CHECK ("Type" IN (0, 1))
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE TABLE "Shoes" (
        "Id" uuid NOT NULL,
        "Article" citext NOT NULL,
        "Name" character varying(200) NOT NULL,
        "ManufacturerId" uuid NOT NULL,
        "Price" numeric(18,2) NOT NULL,
        "Category" integer NOT NULL,
        "Seasons" integer NOT NULL,
        CONSTRAINT "PK_Shoes" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_Shoes_Enums" CHECK ("Category" BETWEEN 0 AND 4 AND "Seasons" BETWEEN 1 AND 15),
        CONSTRAINT "CK_Shoes_Price" CHECK ("Price" > 0),
        CONSTRAINT "CK_Shoes_Text" CHECK (length(btrim("Article")) > 0 AND length(btrim("Name")) > 0),
        CONSTRAINT "FK_Shoes_Manufacturers_ManufacturerId" FOREIGN KEY ("ManufacturerId") REFERENCES "Manufacturers" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE TABLE "MovementItems" (
        "Id" uuid NOT NULL,
        "MovementId" uuid NOT NULL,
        "ShoeId" uuid NOT NULL,
        "ShoeSizeId" uuid NOT NULL,
        "Quantity" integer NOT NULL,
        "UnitPrice" numeric(18,2) NOT NULL,
        CONSTRAINT "PK_MovementItems" PRIMARY KEY ("Id"),
        CONSTRAINT "CK_MovementItems_Positive" CHECK ("Quantity" > 0 AND "UnitPrice" > 0),
        CONSTRAINT "FK_MovementItems_ShoeSizes_ShoeSizeId" FOREIGN KEY ("ShoeSizeId") REFERENCES "ShoeSizes" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_MovementItems_Shoes_ShoeId" FOREIGN KEY ("ShoeId") REFERENCES "Shoes" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_MovementItems_StockMovements_MovementId" FOREIGN KEY ("MovementId") REFERENCES "StockMovements" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Manufacturers_Name" ON "Manufacturers" ("Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_MovementItems_MovementId_ShoeId_ShoeSizeId" ON "MovementItems" ("MovementId", "ShoeId", "ShoeSizeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE INDEX "IX_MovementItems_ShoeId" ON "MovementItems" ("ShoeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE INDEX "IX_MovementItems_ShoeSizeId" ON "MovementItems" ("ShoeSizeId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Shoes_Article" ON "Shoes" ("Article");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE INDEX "IX_Shoes_ManufacturerId" ON "Shoes" ("ManufacturerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_ShoeSizes_Value" ON "ShoeSizes" ("Value");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20261113150650_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20261113150650_InitialCreate', '8.0.11');
    END IF;
END $EF$;
COMMIT;
