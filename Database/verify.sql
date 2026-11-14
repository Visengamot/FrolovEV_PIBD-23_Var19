-- PostgreSQL: 127.0.0.1:55432, shoestore_lab2, shoestore_app.
-- Local password is in ignored appsettings.Local.json.
SELECT current_database(), current_user, inet_server_addr(), inet_server_port();
SELECT * FROM "__EFMigrationsHistory" ORDER BY "MigrationId";
SELECT * FROM "Manufacturers" ORDER BY "Name";
SELECT * FROM "Shoes" ORDER BY "Article";
SELECT * FROM "ShoeSizes" ORDER BY "Value";
SELECT * FROM "StockMovements" ORDER BY "Date";
SELECT * FROM "MovementItems" ORDER BY "MovementId";

-- Наличие по товару и размеру, продажи и выручка за текущий месяц.
SELECT s."Article", s."Name", z."Value" AS "Размер",
       SUM(CASE WHEN m."Type" = 0 THEN i."Quantity" ELSE -i."Quantity" END) AS "Остаток",
       COALESCE(SUM(i."Quantity") FILTER (WHERE m."Type" = 1 AND m."Date" >= date_trunc('month', CURRENT_DATE)), 0) AS "Продано за месяц",
       COALESCE(SUM(i."Quantity" * i."UnitPrice") FILTER (WHERE m."Type" = 1 AND m."Date" >= date_trunc('month', CURRENT_DATE)), 0) AS "Выручка"
FROM "MovementItems" i
JOIN "StockMovements" m ON m."Id" = i."MovementId"
JOIN "Shoes" s ON s."Id" = i."ShoeId"
JOIN "ShoeSizes" z ON z."Id" = i."ShoeSizeId"
WHERE m."Date" <= CURRENT_DATE
GROUP BY s."Article", s."Name", z."Value"
ORDER BY s."Article", z."Value";
