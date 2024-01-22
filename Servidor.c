#include <string.h>
#include <mysql.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <pthread.h>

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

typedef struct {
	char nombre[20];
	int socket;
} Conectado;

typedef struct {
	Conectado conectados [100];
	int num;
} lConectados;
typedef struct {
	char jugador[4][20];
	int socket[4];
	int aceptado[4];
	int ocupado;
} Partida;

typedef struct {
	Partida partida [100];
	int num;
} lPartidas;

lConectados lista;

lPartidas listaPartidas;

int sockets[100];
int i = 0;
char anfitrion[80];
char invitado1[80];



void dameConectados(lConectados *lista, char conectados [300]);
int dameSocket (lConectados * lista, char nombre[20]);
int damePos (lConectados * lista, char nombre[20]);
int desconectar (lConectados *lista, char nombre[20]);
int conectar (lConectados *lista, char nombre[20], int socket);
int contarConectados(int conectados[], int tamano);
int tPartida (char invitados[60], int numJ, lPartidas *listaPartidas, lConectados *lista);
void acceso(char nombre[25], char contrasena[25],  char respuesta[512]);
void registrar(char nombre[25], char contrasena[25],  char respuesta[512]);
void *atenderCliente(void *socket);
void eliminarCliente (char nombre[20], char respuesta[512]);
//------------------------------------------------------------------------------------------------

int main(int argc, char *argv[]) 
{
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;
	pthread_t thread;
	lista.num = 0;
	int conexion = 0;
	int puerto = 9058;
	inicializarPartidas(&listaPartidas);
	
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error al crear socket\n");
	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY); 
	serv_adr.sin_port = htons(puerto);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	int rc;
	while(conexion == 0)
	{
		printf("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf("Conexion recibida\n");
		
		sockets[i] = sock_conn;
		
		rc = pthread_create (&thread, NULL, atenderCliente , &sockets[i]);
		printf("Code %d = %s\n", rc, strerror(rc));
		
		i++;
	}
	return 0;
}
//----------------------------------------------------------------------------------------
void dameConectados(lConectados *lista, char conectados [300])
{//da los conectados que hay en la lista de conectados
	int i;
	sprintf (conectados, "%d", lista->num);
	//printf("Conectados:%s\n", conectados);
	for (i = 0; i < lista->num; i++)
	{
		sprintf(conectados, "%s/%s", conectados, lista->conectados[i].nombre);

	}
	printf("Conectados: %s\n",conectados);
}
//-------------------------------------------------------
int dameSocket (lConectados * lista, char nombre[20])
{//busca el socket de la persona que recibe por parametro
	int n = 0;
	int encontrado = 0;
	printf("Cual es el socket de : %s\n",nombre);
	printf("%d\n", lista->num);
	while ((n<lista->num) && !encontrado)
	{
		printf("%s\n", lista->conectados[n].nombre);
		printf("%s\n", nombre);
		if (strcmp(lista->conectados[n].nombre, nombre) == 0)
		{
			encontrado = 1;
			printf("Si encontrado\n");
		}
		if (!encontrado)
		{
			printf("No encontrado\n");
			n++;
		}
		
	}
	if (encontrado)
	{
		printf("Encontrado\n");
		printf("El socket de %s es: %d\n", nombre, lista->conectados[n].socket);
		return lista->conectados[n].socket;
	}
	else
		return -1;
}
//--------------------------------------------------
int damePos (lConectados * lista, char nombre[20])
{//da la posicion en la lista de coenctados de la persona que recibe por parametro
	int i = 0;
	int encontrado = 0;
	printf("Lista nombres : %s\n", lista->conectados[i].nombre);
	while ((i<lista->num) && (encontrado!= 0))
	{
		if (strcmp(lista->conectados[i].nombre, nombre) == 0)
			encontrado = 1;
		if (!encontrado)
			i++;
		printf("Estamos aquiiiii\n");
	}
	if (encontrado)
		return i;
	else 
		return -1;
}
//-----------------------------------------------
int desconectar (lConectados *lista, char nombre[20])
{//desconecta (a la persona que recibe por parametro) de la lista de conectados
	int pos = damePos(lista, nombre);
	if (pos == -1)
		return -1;
	else
	{
		int i;
		for (i = pos; i < lista->num-1; i++)
		{
			lista->conectados[i] = lista->conectados[i+1];
		}
		lista->num--;
		return 0;
	}
}
//----------------------------------------
int conectar (lConectados *lista, char nombre[20], int socket)
{//Conecta a la persona que recibe por parametros a la lista de conectados
	if (lista->num == 100)
		return -1;
	else
	{
		strcpy(lista->conectados[lista->num].nombre, nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num++;
		printf("Numero en lista : %d\n", lista->num);
		printf("socket: %d\n", socket);
		return 0;
	}
}
//-----------------------------------------
int contarConectados(int conectados[], int tamano)
{
	int contador = 0;
	
	for(int i = 0; i < tamano; i++){
		if(conectados[i] == 1){
			contador++;
		}
	}
	
	return contador;
}
//-------------------------------------------
void inicializarPartidas (lPartidas *listaPartidas)
{//inicializa las partidas
	int n;
	for (n = 0; n < 99; n++)
	{
		listaPartidas->partida[n].ocupado = 0;
	}
}
//-------------------------------------------
int tPartida (char invitados[60], int numJ, lPartidas *listaPartidas, lConectados *lista)
{//Crea la partida y registra a los jugadores
	int n = 0;
	int j = 0;
	int num;
	int encontrado = 0;
	char *p;
	printf("%s\n",invitados);
	p = strtok(invitados, "/");
	
	while(n < 99 && encontrado == 0)
	{
		if (listaPartidas->partida[n].ocupado == 0)
		{
			encontrado = 1;
		}
		else{
			n++;	
		}
	}
	if (encontrado = 1){
		while(j < numJ + 1)
		{
			strcpy(listaPartidas->partida[n].jugador[j], p);
			int z = damePos(lista, p);
			printf("Socket para jugar:%d\n", lista->conectados[z].socket);
			listaPartidas->partida[n].socket[j] = lista->conectados[z].socket;
			j++;
		}
		return n;
	}
	else{
		return -1;
	}
	
}
//--------------------------------------------
void *atenderCliente (void *socket)
{
	int sock_conn, ret;
	int *s;
	s = (int *) socket;
	sock_conn = *s;         	//Estas cuatro lineas son para poner el socket connection con el cliente
	
	char peticion[512];
	char respuesta[512];
	char contestacion[512];
	char contrasena[20];
	char nombre[25];
	char nombre2[70];
	char jugadores[100];
	char fecha[11];
	char conectados[300];
	char respuestaC[300];
	int conexion = 0;
	int r;
	int z;
	int n=0;
	int numJ;
	int partida;
	int res;
	
	while(conexion == 0)
	{
		ret=read(sock_conn,peticion, sizeof(peticion));
		printf ("Recibido\n");
		peticion[ret]='\0';
		int error = 1;
		int codigo = 9999;
		char *p;
		if(strlen(peticion) < 2){
			error = 0;
		} 
		   if (strcmp(peticion, "") != 0){
			printf ("Peticion: %s\n",peticion);
			p = strtok(peticion, "/");
			codigo = atoi(p);
		}		
		if (error == 0){
			codigo = 9999;
		}
		if(codigo == 0)
		{//Da acceso a un usuario
			pthread_mutex_lock(&mutex);
			p = strtok(NULL, "/");
			strcpy(nombre, p);
			printf("Codigo de conexion: %d\n", r);
			p = strtok(NULL, "/");
			strcpy(contrasena, p);
			printf("Codigo: %d, Nombre: %s y Contraseￃﾱa: %s\n", codigo, nombre, contrasena);
			acceso(nombre, contrasena, contestacion);
			if(strcmp (contestacion, "Error") != 0)
			{
			r = conectar(&lista, nombre, sock_conn);
			dameConectados(&lista, conectados);
			sprintf(respuesta, "%s\n", contestacion);
			write (sock_conn,respuesta,strlen(respuesta));
			res =0 ;
			pthread_mutex_unlock(&mutex);
			}
			else
			{
				strcpy(respuesta, "0/Error");
				write (sock_conn,respuesta,strlen(respuesta));
			}
			pthread_mutex_unlock(&mutex);
		}	
		else if(codigo == 4)
		{//registra a un usuario nuevo
			pthread_mutex_lock(&mutex);
			p = strtok(NULL, "/");
			strcpy(nombre, p);
			p = strtok(NULL, "/");
			strcpy(contrasena, p);
			printf("Codigo: %d, contrasena: %s y Nombre: %s\n", codigo, contrasena, nombre);
			registrarse(nombre, contrasena, contestacion);
			if(strcmp (contestacion, "Error") != 0)
			r = conectar(&lista, nombre, socket);
			dameConectados(&lista, conectados);
			sprintf(respuesta, "%s/", contestacion);
			write (sock_conn,respuesta,strlen(respuesta));
			pthread_mutex_unlock(&mutex);
		}
		else if(codigo == 5)
		{//Desconecta al usuario de la base de datos
			pthread_mutex_lock(&mutex);
			p = strtok(NULL, "/");
			strcpy(nombre, p);
			conexion = 1;
			printf("Desconectando a %s\n", nombre);
			r = desconectar(&lista, nombre);
			printf("Codigo de desconexion: %d\n", r);
			dameConectados(&lista, conectados);
			strcpy(respuesta,"5/Desconectado");
			write(sock_conn,respuesta, strlen(respuesta));
			close(sock_conn);
			conexion = 1;
			pthread_mutex_unlock(&mutex);
		}
		else if(codigo == 7)       
		{//Recibe la peticion, envia la invitacion a los jugadores invitados y crea un socket para la partida
			pthread_mutex_lock(&mutex);
			n = 0;
			char invitados[60];
			p = strtok(NULL, "/");
			strcpy(anfitrion, p);
			printf("%s\n", anfitrion);
			p = strtok(NULL, "/");
			numJ = atoi(p);
			printf("Num invitados: %d\n", numJ);
			while(n < numJ)
			{
				p = strtok(NULL, "/");
				strcpy(invitado1, p);
				printf("%s\n",nombre2);
				sprintf(invitados,"%s/%s",invitados, p);
				n++;
			}
			printf("%s\n",invitados);
			z = tPartida(invitados, numJ, &listaPartidas, &lista);
			n = 0;
			while(n < numJ)
			{
				p = strtok(invitados, "/");
				strcpy(nombre2, p);
				printf("%s\n",nombre2);
				int x = dameSocket(&lista, nombre2);
				if(x!=0)
				{
					sprintf(respuesta, "7/OK/%s",anfitrion); 
					printf("%s",respuesta);
				}
				else
				{
					strcpy(respuesta, "7/Error");
					write(x, respuesta, strlen(respuesta));
				}
				printf("Socket: %d\n", listaPartidas.partida[z].socket[n]);
				int sockp = listaPartidas.partida[z].socket[n];
				sprintf(respuesta,"%s/%d",respuesta, sockp);
				write(x, respuesta, strlen(respuesta));
				n++;
			}
			pthread_mutex_unlock(&mutex);
			printf("estamos aquii\n");
		}		
		else if (codigo == 9)
		{//notifica el inicio de partida a los jugadores
			char respuesta1[80];
			char respuesta2[80];
			int x;
			int y;
			char jugador[80];
			pthread_mutex_lock(&mutex);		
			n = 0;
			sprintf(respuesta1, "9/Empieza la partida/0");
			sprintf(respuesta2, "9/Empieza la partida/1");
			p = strtok(NULL, "/");
			partida=atoi(p);
			printf("%s\n", invitado1);
			printf("%s\n", anfitrion);
			x = dameSocket(&lista, invitado1);
			write(x, respuesta1, strlen(respuesta1));
			y = dameSocket(&lista, anfitrion);
			write(y, respuesta2, strlen(respuesta2));
			
			pthread_mutex_unlock(&mutex);
		}
		
		else if (codigo == 8)
		{ //envia la respuesta de los usuarios al host de si se unen a la partida o no
			char apoyo[100];
			int j = 0;
			pthread_mutex_lock(&mutex);
			p = strtok(NULL,"/");
			partida = atoi(p);
			p = strtok(NULL,"/");
			n = 0;
			strcpy(apoyo, p);
			if (strcmp(apoyo, "Yes") == 0)
			{
				p = strtok(NULL,"/");
				strcpy(nombre, p);
				printf("%s\n", nombre);
				printf("Entra al if\n");
				p = strtok(NULL,"/");
				strcpy(anfitrion, p);
				printf("%s\n", anfitrion);
				listaPartidas.partida[partida].aceptado[n] = 1;
				sprintf(jugadores,"%s",anfitrion);
				sprintf(jugadores, "%s", listaPartidas.partida[partida].jugador[n]);
				printf("invitado:%s\n",jugadores);
				strcpy(respuesta, "10/El invitado ha aceptado");
				sprintf(respuesta, "%s/%s", respuesta, invitado1);
				printf("%s\n", respuesta);
				printf("Anfitrion:%s\n", anfitrion);
				z = dameSocket(&lista, anfitrion);
				write(z,respuesta, strlen(respuesta)); 
				pthread_mutex_unlock(&mutex);
			}
		}
		else if (codigo == 13)
		{//recibe el movimiento del jugador y envia al jugador contrario el movimiento de la ficha y si ha comprado esa propiedad
			char apoyo[100];
			char jrecibido[80];
			char propiedadC[100];
			int movimiento;
			int j;
			int x;
			pthread_mutex_lock(&mutex);
			p = strtok(NULL,"/");
			strcpy(jrecibido, p);
			printf("%s\n", jrecibido);
			p = strtok(NULL,"/");
			movimiento = atoi(p);
			p = strtok(NULL,"/");
			strcpy(apoyo, p);
			printf("%s\n", apoyo);
			if(strcmp(apoyo, "Yes") == 0)
			{
				p = strtok(NULL, "/");
				strcpy(propiedadC, p);
				printf("propiedad:%s\n", propiedadC);
				if(strcmp(jrecibido, anfitrion)==0)
				{
				j = dameSocket(&lista, invitado1);
				printf("Estamos en el 1\n");
				strcpy(respuesta,"12");
			    sprintf(respuesta, "%s/%s",respuesta, jrecibido);
			    sprintf(respuesta,"%s/%d", respuesta, movimiento);
			    sprintf(respuesta,"%s/Yes", respuesta);
			    sprintf(respuesta,"%s/%s", respuesta, propiedadC);
			    write(j, respuesta, strlen(respuesta));
				printf("%d\n",j);
				printf("La respuesta es:%s\n", respuesta);
				}
				if(strcmp(jrecibido, anfitrion)!=0)
				{
				j = dameSocket(&lista, anfitrion);
				printf("Estamos en el 2\n");
				strcpy(respuesta,"12");
			    sprintf(respuesta, "%s/%s",respuesta, jrecibido);
				sprintf(respuesta,"%s/%d", respuesta, movimiento);
				sprintf(respuesta,"%s/Yes", respuesta);
				sprintf(respuesta,"%s/%s", respuesta, propiedadC);
				printf("Socket:%d\n",j);
				write(j, respuesta, strlen(respuesta));
				printf("La respuesta es:%s\n", respuesta);		 
				}
			}
			else
			{
			 if(strcmp(jrecibido, anfitrion)==0)
			{
			j = dameSocket(&lista, invitado1);
			strcpy(respuesta,"12");
			sprintf(respuesta, "%s/%s",respuesta, jrecibido);
			sprintf(respuesta,"%s/%d", respuesta, movimiento);
			sprintf(respuesta,"%s/No", respuesta);
			write(j, respuesta, strlen(respuesta));
			printf("Socket:%d\n",j);
			printf("La respuesta es:%s\n", respuesta);
			}
			 if(strcmp(jrecibido, anfitrion)!=0)
			 {
				 j = dameSocket(&lista, anfitrion);
				 strcpy(respuesta,"12");
				 sprintf(respuesta, "%s/%s",respuesta, jrecibido);
				 sprintf(respuesta,"%s/%d", respuesta, movimiento);
				 sprintf(respuesta,"%s/No", respuesta);
				 write(j, respuesta, strlen(respuesta));
				 printf("Socket:%d\n",j);
				 printf("La respuesta es:%s\n", respuesta);
			 }
			}
			pthread_mutex_unlock(&mutex);
		}
		else if(codigo == 14)
		{//avisa al jugador de que el contrario ha caido en bancarrota
		int z;	
		char apoyo[100];	
		p = strtok(NULL,"/");
		strcpy(apoyo, p);
		if(strcmp(apoyo, anfitrion)==0)
		{
			z = dameSocket(&lista, invitado1);
			strcpy(respuesta, "14/El rival ha caido en bancarrota, enhorabuena");
			write(z, respuesta, strlen(respuesta));
		}
		else
		   {
			z = dameSocket(&lista, anfitrion);
			strcpy(respuesta, "14/El rival ha caido en bancarrota, enhorabuena");
			write(z, respuesta, strlen(respuesta));
		   }
		}
		else if(codigo == 15)
		{//avisa al jugador de que el rival ha abandonado la partida
			int z;
			char apoyo[100];
			p = strtok(NULL,"/");
			strcpy(apoyo, p);
			if(strcmp(apoyo, anfitrion)==0)
			{
				
				z = dameSocket(&lista, invitado1);
				strcpy(respuesta, "15/El rival ha abandonado la partida");
				write(z, respuesta, strlen(respuesta));
				
			}
			else
			{
				z = dameSocket(&lista, anfitrion);
				strcpy(respuesta, "15/El rival ha abandonado la partida");
				write(z, respuesta, strlen(respuesta));
			}
		}
				
		
		else if(codigo == 11)
		{//elimina un cliente de la base de datos
			pthread_mutex_lock(&mutex);		
			p = strtok(NULL,"/");
			char nombre[20];
			strcpy(nombre, p);
			eliminarCliente (nombre, contestacion);
			write(sock_conn, contestacion, strlen(contestacion));
			pthread_mutex_unlock(&mutex);
		}
		
		
		
		if ((codigo == 0 && res == 0) || codigo == 5 || codigo == 4)
		{//actualiza la lista de conectados
			pthread_mutex_lock(&mutex);
			int j;
			dameConectados(&lista, conectados);
			sprintf(respuesta, "6/%s", conectados);
			printf("%s\n", respuesta);
			printf("Para conectados\n");
			pthread_mutex_unlock(&mutex);
			printf("%d\n",i);
			for (j = 0; j < i; j++)
			{
				write(sockets[j],respuesta,strlen(respuesta));
				printf("Socket : %d\n", sockets[j]);

			}
			printf("Nombre:%s\n", nombre);

		}
		else{
			printf("Peticion fantasma\n");
		}
	}
	close(sock_conn);
}

void acceso(char nombre[25], char contrasena[25], char respuesta[512])
{//Busca el usuario y la contrase￱a y permite su acceso
	
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	int acceso;
	char consulta[500];
	
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use MONOPOLY;");
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	strcpy (consulta,"select DNI from JUGADOR where nombre = '");
	strcat (consulta, nombre);
	strcat (consulta,"' and Password = '");
	strcat (consulta, contrasena);
	strcat (consulta,"';");
	err=mysql_query (conn, consulta); 
	if (err!=0) 
	{
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("Nombre o contraseￃﾱa incorrectos\n");
		acceso = -1;
		sprintf(respuesta, "Error");
	}	
	else
	{
		printf ("Acceso garantizado al usuario con id: %s\n", row[0]);
		acceso = 0;
		sprintf(respuesta, "0/Conectado", acceso);
	}
}
void registrarse(char nombre[25], char contrasena[25],char respuesta[512])
{ 
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[500];
	int registro;
	int numjug;
	
	conn = mysql_init(NULL);
	if (conn==NULL) 
	{
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use MONOPOLY;");
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query (conn, "select MAX(DNI) from JUGADOR;"); 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	numjug = atoi(row[0]);
	numjug ++;
	printf("este es el numero: %d\n", numjug);
	int numero = numjug;
	sprintf(consulta, "insert into JUGADOR values (%d,'%s','%s', 18);", numjug, nombre, contrasena);
	err=mysql_query (conn, consulta); 
	if (err!=0) 
	{
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
		sprintf(respuesta, "4/Error");
	}
	else
	{
		sprintf(respuesta, "4/Todo ha salido bien");
	}
	
}
//-------------------------------
void eliminarCliente (char nombre[20], char respuesta[512])
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[500];
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use MONOPOLY;");
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	strcpy (consulta,"delete from JUGADOR where nombre = '");		//elimina un cliente de la base de datos
	strcat (consulta, nombre);
	strcat (consulta,"'");
	err=mysql_query (conn, consulta); 
	if (err!=0) 
	{
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	else
		strcpy(respuesta,"11/El usuario se ha eliminado correctamente\n");
		printf("El usuario se ha eliminado correctamente\n");
		
		
	
}
//--------------------------------
void acceso(char nombre[25], char contrasena[25],  char respuesta[512]);
void registrar(char nombre[25], char contrasena[25],  char respuesta[512]);
void *atenderCliente(void *socket);
