CREATE TABLE JUGADOR(
DNI INTEGER,
Nombre VARCHAR(60),
Password VARCHAR(20),
Edad INTEGER,
PRIMARY KEY(DNI)
);Engine=InnoDB
CREATE TABLE PARTIDA( 
id int, 
Jugadores int, 
PRIMARY KEY(id)
);
CREATE TABLE RESULTADOS(
idp INTEGER,
DNI INTEGER,
GANADOR VARCHAR(20),
Duracion INTEGER,
FECHA VARCHAR(60)
);


INSERT INTO JUGADOR VALUES(123,"Marcos","Password",23)
INSERT INTO JUGADOR VALUES(122,"Rohail","Password",28)
INSERT INTO JUGADOR VALUES(121,"David","Password",25)
INSERT INTO JUGADOR VALUES(124,"Juan","Password",23)
 
insert into PARTIDA values (1,2);
insert into PARTIDA values (2,2);
insert into PARTIDA values (3,3);
insert into PARTIDA values (4,4);
insert into PARTIDA values (5,2);
insert into PARTIDA values (6,4);

insert into RESULTADO values (1,123,'no',5,'07/10/2022');
insert into RESULTADOS values (1,122,'si',5,'07/10/2022');
insert into RESULTADOS values (2,123,'no',5,'09/10/2022');
insert into RESULTADOS values (2,124,'si',5,'09/10/2022');
insert into RESULTADOS values (3,123,'no',5,'09/10/2022');
insert into RESULTADOS values (3,122,'si',5,'09/10/2022');
insert into RESULTADOS values (3,124,'no',5,'09/10/2022');





