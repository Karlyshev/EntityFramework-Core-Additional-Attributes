# EntityFramework-Core-Additional-Attributes
This repository is done additional attributes which missing in the Entity Framework Core.

## Examples
I tried to describe some examples of using attributes in the App.Entities project:

### MainEntity
This entity configure shows the using attributes such as SummaryAsComment and WithDefaultValue.

Those attributes that are marked as SummaryAsComment will have comment from summary.

The attribute that is marked as WithDefaultValue will have the default value as set for the class property.

### TestEntity1
This entity configure shows the using of attribute such as ForeignKeyFor for one-to-many relation.

### TestEntity2
This entity configure shows the using of attribute such as ForeignKeyFor for one-to-one relation.

### TestEntity3
This entity configure shows the using of attribute such as ForeignKeyFor for one-to-many relation with composite key.