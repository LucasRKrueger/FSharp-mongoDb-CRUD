module repository

open System
open MongoDB.Bson
open MongoDB.Driver
open MongoDB.Driver.Builders
open MongoDB.FSharp

[<Literal>]
let ConnectionString = "Server=localhost;Port=XXX;User Id=postgres;Password=XXX;databaseName=XXX"

[<Literal>]
let DbName = "DbName"

[<Literal>]
let CollectionName = "Person"

type Person = {Id : BsonObjectId; Name : string; Age : int32}

let client = MongoClient(ConnectionString)
let db = client.GetDatabase(DbName)
let testCollection = db.GetCollection<Person>(CollectionName)

let create (person : Person) = 
    testCollection.InsertOne(person)

let createMany (people : Person list) = 
    testCollection.InsertMany(people)

let getById(id : BsonObjectId ) = 
    testCollection.Find(fun x -> x.Id = id).ToEnumerable()

let getAll = 
    testCollection.Find(Builders.Filter.Empty).ToEnumerable()

let updateOnName (oldName : string)(newName : string) =
    
    let filter =
        Builders<Person>.Filter.Eq((fun x -> x.Name), oldName)

    let update =
        Builders<Person>.Update.Set((fun x -> x.Name), newName)

    testCollection.UpdateMany(filter, update)

let deleteById(id : BsonObjectId) = 
    testCollection.DeleteOne(fun x -> x.Id = id)