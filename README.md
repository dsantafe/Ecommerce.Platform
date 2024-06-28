# Despliegue de servicios (frontend y backends) en Docker del E-commerce Platform
Este repositorio contiene los archivos y comandos necesarios para desplegar los servicios (frontend y backends) en Docker del E-commerce Platform

1. Clona este repositorio:
```shell
git clone https://github.com/dsantafe/Ecommerce.Platform
cd Ecommerce.Platform
```

2. Construir la imagen:
```shell
$ docker build -t ms-frontend:1.0 frontend/.
$ docker build -t ms-products:1.0 products/.
$ docker images
```

3. Ejecutar los contenedores a partir de las imagenes:
```shell
$ docker run --name ms-frontend -d -p 3000:3000 \
-e PRODUCTS_SERVICE=host.docker.internal \
-e SHOPPING_CART_SERVICE=host.docker.internal \
-e MERCHANDISE_SERVICE=host.docker.internal \
ms-frontend:1.0

$ docker run --name ms-products -d -p 3001:3001 ms-products:1.0
$ docker run --name ms-shopping-cart -d -p 3002:3002 ms-shopping-cart:1.0
$ docker run --name ms-merchandise -d -p 3003:3003 ms-merchandise:1.0
```

Nota: También puedes ejecutar los contenedores a partir de un Docker Compose:
```shell
$ docker-compose -p ecommerce-platform-fullstack --env-file .env.dev up -d --build
$ docker ps
```