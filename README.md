# Aplikazioaren funtzionamendua

## 1. Docker altxatu

``` bash
cd database
docker-compose up
```

-   Dockerrak **Adminer** altxatuko du **8080** portuan datuak ikusi eta
    aldatzeko
    -   **User:** admin\
    -   **Pass:** admin\
    -   **Datu-basearen izena:** jatetxea

------------------------------------------------------------------------

## 2. Datu basera konektatzeko behar dena

Aplikazioak PostgreSQL-era konektatzeko **Npgsql** instalatu behar da.

-   **NuGet Package Manager** bidez instala daiteke.
    -   Komandoa:

            NuGet\Install-Package Npgsql -Version 9.0.4

    -   Web orria: https://www.nuget.org/packages/Npgsql/

------------------------------------------------------------------------

## 3. Aplikazioa exekutatu

### 3.1 Login lehioa

-   Bi aukera daude probatzeko: **admin** edo **pepe**
-   Erabiltzaile motaren arabera, akzio ezberdinak kudeatuko dira.

### 3.2 Admin lehioa

-   Erabiltzaileak kudeatu (gehitu, editatu, ezabatu)
-   Biltegia kudeatu (produktuak ezabatu, stocka aldatu)

### 3.3 User lehioa

-   Erreserbak kudeatu (sortu)
-   Tiketak egin eta ikusi

------------------------------------------------------------------------

# Proiektuaren egitura

Datubasearekin lan egiteko **database** izeneko direktorio bat dago.

    erronkaTPVsistema/
    ├── database/
    │   ├── postgresql/        # Datu-basearen datuak (bolumena) gordetzeko direktorioa
    │   ├── compose.yml    
    │   └──initdb.sql          # Datu-basea aplikazioa lehenengo aldiz irekitzean datuak sartzeko skripta
    └── ... (WPF aplikazioaren kodea)

------------------------------------------------------------------------

# Docker Compose

## 1) PostgreSQL --- 5432 portuan

-   **Erabiltzailea:** admin\
-   **Pasahitza:** admin\
-   **Datu-basearen izena:** jatetxea\
-   Altxatzean **initdb.sql** scriptak datu-basea populatzen du.

## 2) Adminer --- 8080 portuan

Datu-baseko datuak sortu, editatu, ezabatu eta kontsultatzeko interfaze
grafiko sinple bat.

------------------------------------------------------------------------

# compose.yml edukia

``` yaml
services:

  db:
    image: postgres
    container_name: postgresql_jatetxea
    restart: always
    shm_size: 128mb
    ports:
      - 5432:5432
    
    volumes:
    - ./initdb.sql:/docker-entrypoint-initdb.d/initdb.sql #datubasea inizializatzeko eta datuak sartzeko
    - ./postgresql:/var/lib/postgresql

    environment:
        POSTGRES_USER: admin #datubasearen erabiltzailea
        POSTGRES_PASSWORD: admin #datubasearen pasahitza
        POSTGRES_DB: jatetxea #datubasearen izena

  adminer:
    image: adminer
    restart: always
    ports:
      - 8080:8080
```
