Momentuz honela funtzionatzen du:
Proiektuaren egitura ordenatu behar da eta oreaindik ez dago bukatuta.
Datubasearekin lan egiteko badago direktorio bat database izenekoa.
Bertan egitura hau dauka:
erronkaTPVsistema/
├── database/
│   ├── postgresql/  # Datu-basearen datuak (bolumena) gordetzeko direktorioa
│   └── compose.yml  # Docker Compose konfigurazioa
└── ... (WPF aplikazioaren kodea)

Docker compose:

'''bash
cd database
docker compose up
'''
    Hemen altxatuko ditugu bi instantzia.
        - postgresql 5432 portuan:
            Erabiltzailea "admin"
            Pasahitza "admin"
            Datu basearen izena "jatetxea"
        - adminer 8080 portuan
            Erabiliko dugu datu baseko datuak sortu/editatu/ezabatu eta kontsultatzeko
            interfaze grafiko baten bidez.
''' vim
services:

  db:
    image: postgres
    container_name: postgresql_jatetxea
    restart: always
    shm_size: 128mb
    ports:
      - 5432:5432
    
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
      - 8080:8080
'''
