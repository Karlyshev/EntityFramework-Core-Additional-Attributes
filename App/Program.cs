using App.Infrastructure;
using App.Infrastructure.Extensions;

var db = new DatabaseContext("Host=localhost;Port=5432;Username=<your db username>;Password=<your db password>;Database=AppTestAttr");
MetadataExtensions.GenerateMetadata(db);