# Bookshelf

* Created by Henrique on 07/2017.

This is an ASP.NET MVC 5 Application, that use a single page to execute CRUD operation.
The project uses Model View ViewModel Pattern (MVVM), so you can reuse all data access and validation in a mobile application for example.
To save the records, it is used a XML file (the application automatically create the folder Bookshelf and the XML file on drive C:\).

The Solution have 3 projects:
  - Bookshelf MVC Project (is the main project, where there is the Controller, Views and JavaScript files);
  - Bookshelf Data Project (where there is the objects, the view model, and the manager object);
  - Bookshelf Common Project (where there is the CRUD operations to use in each page that you create);
  
 
------------------------------------------------------------------------------------------------

Esta é uma aplicação ASP.NET MVC 5, que utiliza uma única página para executar as operações de CRUD.
Este projeto utiliza o padrão Model View View Model (MVVM), desta forma você pode reutilizar os data access e as validações em uma aplicação mobile por exemplo.
Para salvar os registros, é utilizado um arquivo XML (a aplicação cria automaticamente a pasta Bookshelf e o arquivo XML no disco C:\).

A Solution possui 3 projetos:
  - Bookshelf MVC Project (é o projeto principal, onde há o Controller, Views e os arquivos Javascript);
  - Bookshelf Data Project (onte há os objetos, a view model e o objeto de gerenciamento);
  - Bookshelf Common Project (onde há a operação de CRUD, para utilizar em cada página que você criar);
