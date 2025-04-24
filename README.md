# EntityFramework-Core-Additional-Attributes (EN)
This repository implements additional attributes that are missing from the Entity Framework Core.

This solution consists of 3 projects:
- App - for demonstration purposes
- App.Domain - attributes and entities are located here (in this project, entities are presented only for examples, you have your own)
- App.Infrastructure - here are the Helpers implementations of additional attributes, ExtendedDbContext and its auxiliary implementations of additional attributes.

## Examples
I tried to describe some examples of using attributes in the App.Domain project:

### MainEntity
- This entity configure shows the using of attributes such as WithComment and WithDefaultValue.

- For properties marked with the WithComment attribute, they take a comment from the Summary XML documentation.
At the same time, it is necessary to understand that an XML documentation file for these Summaries should be created (see PropertyGroup in App.Domain.csproj), and it is also better to add the nuget package SauceControl.InheritDoc, since it more fully builds XML documentation for entities.

- Properties marked with the WithDefaultValue attribute will have the default value specified in this class for that property (otherwise, if no value is specified for it but marked with an attribute, the default value for the property type will be taken, for example, for System.Int32 the value is set to 0).

### TestEntity1
- This entity configure shows the using of attribute such as ForeignKeyFor for one-to-many relation.

### TestEntity2
- This entity configure shows the using of attribute such as ForeignKeyFor for one-to-one relation.

### TestEntity3
- This entity configure shows the using of attribute such as ForeignKeyFor for one-to-many relation with composite key.

## How to use this (EN):
- Copy the files from App.Domain and App.Infrastructure to your project (or similar projects, i.e. App.Domain to &lt;smth&gt;.Domain and App.Infrastructure to &lt;smth&gt;.Infrastructure, if a clean architecture is used).
- Inherit the ExtendedDbContext for your DbContext, because it is this DbContext that configures the database model with the all additions to EntityFrameworkCore.


# EntityFramework-Core-Additional-Attributes (RU)
Этот репозитории реализует дополнительные атрибуты, отсутствующие в EntityFrameworkCore. 

Это рещение состоит из 3х проектов:
- App - для целей демонстрации
- App.Domain - здесь находятся атрибуты и сущности (в данном проекте сущности представлены лишь для примеров, у вас они свои)
- App.Infrastructure - здесь находятся хелперы реализации дополнительных атрибутов, ExtendedDbContext и его вспомогательная реализация дополнительных атрибутов.

## Примеры
Я попытался описать несколько примеров применения атрибутов в проекте App.Domain:

### MainEntity
- Эта настройка сущности показывает применение таких атрибутов как WithComment и WithDefaultValue.

- Для свойств, помеченные атрибутом WithComment, комментарий берется из Summary XML документации.
При этом необходимо понимать, что для этих Summary комментариев должен быть создан XML-файл документации (смотрите PropertyGroup в App.Domain.csproj), а также лучше добавить пакет nuget SauceControl.InheritDoc, поскольку он более полно создает XML-документацию для сущностей.

- Свойства, помеченные атрибутом WithDefaultValue, будут иметь значение по умолчанию, указанное в этом классе для этого свойства (в противном случае, если для него не указано значение, но помечено атрибутом, будет взято значение по умолчанию для типа свойства, например, для System.Int32 значение будет взято равным 0).

### TestEntity1
- Эта настройка сущности показывает применение атрибута, такого как ForeignKeyFor для отношения "Один-ко-многим".

### TestEntity2
- Эта настройка сущности показывает применение атрибута, такого как ForeignKeyFor для отношения "Один-к-одному".

### TestEntity3
- Эта настройка сущности показывает применение атрибута, такого как ForeignKeyFor для отношения "Один-ко-многим" с составным ключом.

## Как этим пользоваться (RU):
- Скопировать файлы из App.Domain и App.Infrastructure в свой проект (или схожие проекты, т.е. App.Domain в &lt;что-то там&gt;.Domain and App.Infrastructure to &lt;что-то там&gt;.Infrastructure, если применяется чистая архитектура).
- Унаследуйте ExtendedDbContext для своего DbContext, потому что именно этот DbContext настраивает модель БД со всеми дополнениями для EntityFrameworkCore.