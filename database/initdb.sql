CREATE DATABASE "jatetxea";

DROP TYPE IF EXISTS "erabiltzaile_mota";;
CREATE TYPE "erabiltzaile_mota" AS ENUM ('admin', 'user');

DROP TYPE IF EXISTS "mahaiegoera";;
CREATE TYPE "mahaiegoera" AS ENUM ('libre', 'erreserbatuta');

DROP TYPE IF EXISTS "erreserbanoiz";;
CREATE TYPE "erreserbanoiz" AS ENUM ('janaria', 'afaria');

CREATE TABLE "public"."erabiltzaileak" (
    "izena" character(20) NOT NULL,
    "pasahitza" character(20) NOT NULL,
    "mota" erabiltzaile_mota NOT NULL,
    CONSTRAINT "erabiltzaileak_izena" PRIMARY KEY ("izena")
)
WITH (oids = false);

INSERT INTO "erabiltzaileak" ("izena", "pasahitza", "mota") VALUES
('admin               ',	'admin               ',	'admin'),
('proba               ',	'proba               ',	'user'),
('pepe                ',	'pepe                ',	'user'),
('1                   ',	'1                   ',	'user');

CREATE SEQUENCE erreserbak_id_seq INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 START 11 CACHE 1;

CREATE TABLE "public"."erreserbak" (
    "id" integer DEFAULT nextval('erreserbak_id_seq') NOT NULL,
    "mahaizenbakia" integer NOT NULL,
    "data" date NOT NULL,
    "noiz" erreserbanoiz NOT NULL,
    "erabiltzailea" character(20) NOT NULL,
    CONSTRAINT "erreserbak_pkey" PRIMARY KEY ("id")
)
WITH (oids = false);

INSERT INTO "erreserbak" ("id", "mahaizenbakia", "data", "noiz", "erabiltzailea") VALUES
(1,	1,	'2025-11-12',	'janaria',	'pepe                '),
(2,	2,	'2025-11-12',	'afaria',	'pepe                '),
(3,	2,	'2025-11-12',	'janaria',	'pepe                '),
(4,	3,	'2025-11-12',	'janaria',	'pepe                '),
(5,	1,	'2025-11-12',	'afaria',	'pepe                '),
(8,	5,	'2025-11-21',	'janaria',	'pepe                '),
(9,	2,	'2025-11-17',	'janaria',	'pepe                '),
(10,	1,	'2025-11-17',	'janaria',	'pepe                ');

CREATE TABLE "public"."kategoriak" (
    "izena" character(20) NOT NULL
)
WITH (oids = false);

INSERT INTO "kategoriak" ("izena") VALUES
('kafeak              '),
('errefreskoak        '),
('alkoholak           '),
('olioa               '),
('esnea               '),
('besteak             ');

CREATE TABLE "public"."mahaiakgauean" (
    "izena" integer NOT NULL
)
WITH (oids = false);

INSERT INTO "mahaiakgauean" ("izena") VALUES
(1),
(2),
(3),
(4),
(5);

CREATE TABLE "public"."mahaiakgoizean" (
    "zenbakia" integer NOT NULL
)
WITH (oids = false);

INSERT INTO "mahaiakgoizean" ("zenbakia") VALUES
(1),
(2),
(3),
(4),
(5);

CREATE TABLE "public"."produktuak" (
    "izena" character(20) NOT NULL,
    "prezioa" money NOT NULL,
    "stock" integer NOT NULL,
    "kategoria" character(20) NOT NULL
)
WITH (oids = false);

INSERT INTO "produktuak" ("izena", "prezioa", "stock", "kategoria") VALUES
('Kafe beltza         ',	'$1.20',	50,	'kafeak              '),
('Fanta Laranja       ',	'$1.70',	80,	'errefreskoak        '),
('Sprite              ',	'$1.70',	90,	'errefreskoak        '),
('Garagardoa          ',	'$2.00',	70,	'alkoholak           '),
('Ron kola            ',	'$5.00',	40,	'alkoholak           '),
('Oliba olioa         ',	'$4.50',	25,	'olioa               '),
('Girasol olioa       ',	'$3.20',	30,	'olioa               '),
('Ekologikoa olioa    ',	'$6.00',	15,	'olioa               '),
('Esne osoa           ',	'$1.10',	60,	'esnea               '),
('Esne gaingabetua    ',	'$1.05',	70,	'esnea               '),
('Esne begetala       ',	'$1.80',	40,	'esnea               '),
('Azukrea             ',	'$1.00',	50,	'besteak             '),
('Galletak            ',	'$2.50',	45,	'besteak             '),
('Espresso            ',	'$1.50',	45,	'kafeak              '),
('Kafea esnearekin    ',	'$1.30',	58,	'kafeak              '),
('Coca-Cola           ',	'$1.80',	120,	'errefreskoak        '),
('Ardo beltza         ',	'$8.50',	40,	'alkoholak           ');

ALTER TABLE ONLY "public"."erreserbak" ADD CONSTRAINT "erreserbak_erabiltzailea_fkey" FOREIGN KEY (erabiltzailea) REFERENCES erabiltzaileak(izena) ON UPDATE CASCADE ON DELETE CASCADE NOT DEFERRABLE;