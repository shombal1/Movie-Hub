db.createCollection("Users");
db.Users.createIndex({_id: 1});
sh.shardCollection("MovieHub.Users", {_id: "hashed"});

db.createCollection("Media");
db.Media.createIndex({_id: 1});
db.Media.createIndex({releasedYearAt: 1});
db.Media.createIndex({genres: 1}); 
db.Media.createIndex({countries: 1}); 
sh.shardCollection("MovieHub.Media", {_id: "hashed"});

db.createCollection("MediaBasket");
db.MediaBasket.createIndex({UserId: 1});
db.MediaBasket.createIndex({UserId: 1, MovieId: 1},{ unique: true } );
sh.shardCollection("MovieHub.MediaBasket", {UserId: "hashed"});