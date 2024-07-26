CREATE TABLE "Items" (
    "Id" SERIAL NOT NULL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL
);

INSERT INTO "Items" ("Id", "Name") VALUES (1, 'Something');
