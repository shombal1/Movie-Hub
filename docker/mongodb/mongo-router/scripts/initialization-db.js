db.createCollection("Users");
db.Users.createIndex({_id: 1});
sh.shardCollection("MovieHub.Users", {_id: "hashed"});

db.createCollection("Medias");
db.Medias.createIndex({_id: 1});
sh.shardCollection("MovieHub.Medias", {_id: "hashed"});

db.createCollection("MovieBaskets");
db.MovieBaskets.createIndex({UserId: 1});
db.MovieBaskets.createIndex({UserId: 1, MovieId: 1},{ unique: true } );
sh.shardCollection("MovieHub.MovieBaskets", {UserId: "hashed"});