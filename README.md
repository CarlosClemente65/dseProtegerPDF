<h1 align="center"> dseProtegerPDF </h1>
<br>

<h2> Aplicacion para proteger con contraseña un PDF  </h2>
<br>
<h4> @ Carlos Clemente (Diagram Software Europa S.L.) - 02/2024 </h4>

<h3>Descripción</h3>
Aplicacion para proteger con una contraseña uno o varios ficheros PDF <br><br>

### Control versiones

* v1.0.0 Primera versión 
<br><br>

### Uso:
```
* dseprotegepdf fichero.pdf -c password [fichero_protegido.pdf]
* dseprotegepdf ficheros.txt -cl

Parametros:
    -h                    : Esta ayuda
    fichero.pdf           : nombre del fichero PDF a procesar (unico fichero)
    -c                    : parametro que indica que se proteja con contraseña el fichero.pdf
    password              : contraseña para proteger el PDF
    fichero_protegido.pdf : nombre del fichero protegido; si no se pasa se utiliza el nombre del PDF modificado
    -cl                   : parametro para protejer con contraseña la lista de ficheros que hay en ficheros.txt
    ficheros.txt          : fichero que contiene el nombre de cada PDF a proteger junto a su contraseña

```
<br>

### Notas:
* El fichero.txt tendra en cada linea el nombre del fichero.pdf y la contraseña, separados con el simbolo '#'.
	* Ejemplo: fichero.pdf#contraseña
* Cada fichero protegido tiene una contraseña maestra (propietario) que permite acceso total al fichero.

