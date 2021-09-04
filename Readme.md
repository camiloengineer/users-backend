# Administrador de Usuarios

_Este es un proyecto NetCore v3.1_

## Comenzando 🚀

_En caso que requieras obtener una copia del proyecto en tu máquina local para propósitos de desarrollo y pruebas, puedes seguir estas instrucciones ._

### Pre-requisitos 📋

_Es necesario que tengas instalado [Git](https://git-scm.com/) y el SDK [.NET 5.0](https://dotnet.microsoft.com/download) que soporta  NetCore 3.1._


### Instalación 🔧

_el primer paso es clonar el proyecto en tu local_

```
    git clone https://github.com/cgonzalezm1234/users-backend.git
```

_luego tienes que instalar las dependencias de aws  para NetCore_

```
    dotnet tool install -g Amazon.Lambda.Tools
```

_Si las tienes instaladas puedes verificar si hay una nueva versión disponible._
```
    dotnet tool update -g Amazon.Lambda.Tools
```

### Analizar las pruebas de unit testing 🔩
```
    cd "User.Backend.Api/test/User.Backend.Api.Tests"
    dotnet test
```

## Despliegue 📦

_Para realizar el deploy hay que ejecutar este comando_

```
    dotnet lambda deploy-serverless
```

## Construido con 🛠️

_Estas herramamientas se utilizaron para crear el proyecto_

* [.NET](https://dotnet.microsoft.com/) - Es un framework informático administrado, gratuito y de código abierto para los sistemas operativos Windows, Linux y macOS. 

## Autor ✒️

* **Camilo González** - [Linkedin](https://www.linkedin.com/in/camilo-gonzalez-munoz/)

## Expresiones de Gratitud 🎁

* Comenta a otros sobre este proyecto 📢
* Conversemos sobre nuevos proyectos ☕. 

---

