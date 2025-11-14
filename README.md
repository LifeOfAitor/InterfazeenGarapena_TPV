Momentuz honela funtzionatzen du:
Proiektuaren egitura oraindik ordenatu behar da eta ez dago bukatuta.

Datubasearekin lan egiteko "database" izeneko direktorio bat dago.
Hau da proiektuaren egitura:

erronkaTPVsistema/
├── database/
│   ├── postgresql/   # Datu-basearen datuak (bolumena) gordetzeko direktorioa
│   └── compose.yml   # Docker Compose konfigurazioa
└── ... (WPF aplikazioaren kodea)

---------------------------------------
Docker Compose
---------------------------------------

Datu-basea eta Adminer altxatzeko:

    cd database
    docker compose up

Honek bi instantzia altxatuko ditu:

1) PostgreSQL — 5432 portuan
   - Erabiltzailea: admin
   - Pasahitza: admin
   - Datu-basearen izena: jatetxea

2) Adminer — 8080 portuan
   Datu-baseko datuak sortu, editatu, ezabatu eta kontsultatzeko interfaze grafiko sinple bat.

---------------------------------------
compose.yml edukia
---------------------------------------

services:

  db:
    image: postgres
    container_name: postgresql_jatetxea
    restart: always
    shm_size: 128mb
    ports:
      - "5432:5432"
    volumes:
      - ./postgresql:/var/lib/postgresql
    environment:
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: admin
      POSTGRES_DB: jatetxea

  adminer:
    image: adminer
    restart: always
    ports:
      - "8080:8080"